using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator.IO {
    public interface IJoypadInput {
        DirectionalButtons getDirectionalButtonState();
        ActionButtons getActionButtonState();
    }
}
