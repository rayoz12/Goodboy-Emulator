using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator.IO {
    // Split into a different file so that both the external joypad processor and the internal one can depend on it.

    [Flags]
    public enum DirectionalButtons {
        Right = 1 << 0,
        Left = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3
    }

    [Flags]
    public enum ActionButtons {
        A = 1 << 0,
        B = 1 << 1,
        Select = 1 << 2,
        Start = 1 << 3
    }
}
