using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace Gameboy_Emulator.IO {


    /// <summary>
    /// (Low (0) = On, High (1) = Off)
    /// </summary>
    [Flags]
    public enum JOYPFlags {
        /// <summary>
        /// Right or A
        /// </summary>
        Button0 = 1 << 0,
        /// <summary>
        /// Left or B
        /// </summary>
        Button1 = 1 << 1,
        /// <summary>
        /// Up or Select
        /// </summary>
        Button2 = 1 << 2,
        /// <summary>
        /// Down or Start
        /// </summary>
        Button3 = 1 << 3,
        DirectionSelect = 1 << 4,
        ActionSelect = 1 << 5
        //ActionSelect = 1 << 6,
        //ActionSelect = 1 << 7,
    }

    /// <summary>
    /// This class reads input and writes it to memory
    /// </summary>
    public class Joypad {
        public Memory memory;
        public IUserJoypadInput input;

        Index JOYP = Memory.IO.Start;

        public Joypad(Memory memory, IUserJoypadInput input) {
            this.memory = memory;
            this.input = input;
        }

        public void Init() {
            memory[JOYP] = 0b11111111;
        }

        public void Tick() {
            // Read the joypad selected and update the bits
            JOYPFlags register = (JOYPFlags) memory[JOYP];

            bool isNoneSelected = false;
            bool isDirectionSelected = false;

            if (register.HasFlag(JOYPFlags.DirectionSelect) && register.HasFlag(JOYPFlags.ActionSelect)) {
                // Both are set to 1 in this state
                // 11
                isNoneSelected = true;
                return;
            }
            else if (!register.HasFlag(JOYPFlags.DirectionSelect)) {
                // Direction select bit is set to 0
                // 01
                isDirectionSelected = true;
            }
            else if (!register.HasFlag(JOYPFlags.ActionSelect)) {
                // Action select bit is set to 0
                // 10
                isDirectionSelected = false;
            }
            else {
                // Both are selected (invalid state...), default to direction (needs to be investigated what the specified behvaiour )
                // 00
                throw new InvalidOperationException("Both inputs are selected!");
            }
            byte state = memory[JOYP];
            byte buttons = isDirectionSelected ? (byte) input.getDirectionalButtonState() : (byte) input.getActionButtonState();
            
            // Invert to GB
            buttons = (byte)~buttons;
            // Convert the current state to a bit array so we can modify the bits that matters
            BitArray stateBits = new BitArray(state);
            BitArray newButtons = new BitArray(buttons);
            // Assign the first 4 bits
            for (var i = 0; i <= 3; i++) {
                stateBits[i] = newButtons[i];
            }

            byte[] bytes = new byte[1];
            stateBits.CopyTo(bytes, 0);
            memory[JOYP] = bytes[0];
        }
    }
}
