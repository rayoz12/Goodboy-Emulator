using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace Gameboy_Emulator.IO {

    public interface IJoypadInput {
        DirectionalButtons getDirectionalButtonState();
        ActionButtons getActionButtonState();
    }

    [Flags]
    public enum JOYPFlags {
        Button0 = 1 << 0,
        Button1 = 1 << 1,
        Button2 = 1 << 2,
        Button3 = 1 << 3,
        DirectionSelect = 1 << 4,
        ActionSelect = 1 << 5
        //ActionSelect = 1 << 6,
        //ActionSelect = 1 << 7,
    }

    /// <summary>
    /// This class reads input and writes it to memory
    /// </summary>
    internal class Joypad {
        Memory memory;
        
        /// <summary>
        /// Low = Off, High = On. Note this is opposite to the gameboy (Low = On, High = Off)
        /// </summary>
        DirectionalButtons directionalIsPressed = 0;
        ActionButtons actionIsPressed = 0;

        Index JOYP = Memory.IO.Start;

        public Joypad(Memory memory, IJoypadInput input) {
            this.memory = memory;
        }

        public void Init() {
            memory[JOYP] = 0b11111111;
        }

        public void Process() {
            // Read the joypad selected and update the bits
            JOYPFlags register = (JOYPFlags) memory[JOYP];

            bool isNoneSelected = false;
            bool isDirectionSelected = false;

            if (!register.HasFlag(JOYPFlags.DirectionSelect) && !register.HasFlag(JOYPFlags.ActionSelect)) {
                // Both are set to 1 in this state
                // 11
                isNoneSelected = true;
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

            }

            if (register.HasFlag(JOYPFlags.DirectionSelect)) {

            }
        }
    }
}
