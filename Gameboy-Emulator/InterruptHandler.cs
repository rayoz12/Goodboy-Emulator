using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator {

    [Flags]
    public enum InterruptFlags {
        VBlank = 1 << 0,
        LCDStat = 1 << 1,
        Timer = 1 << 2,
        Serial = 1 << 3,
        Joypad = 1 << 4
    }

    internal class InterruptHandler {

        Memory memory;
        public InterruptHandler(Memory memory) { 
            this.memory = memory;
        }

        public void Init() {
            memory[0xFF0F] = 0xE0; // 1110 0000, All interrupts disabled
        }
    }
}
