using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator {
    public class InstructionNotImplementedException: NotImplementedException {
        
        public InstructionReference instruction;
        public InstructionNotImplementedException(byte opcode, InstructionReference reference):
            base("Opcode: 0x" + opcode.ToString("X2") + ". " + reference.ToString() + " not Implemented") { 
            instruction = reference;
        }
    }
}
