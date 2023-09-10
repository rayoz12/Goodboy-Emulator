using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator {
    public class InstructionFactory {

        public static IInstruction FromOpcode(byte opcode, InstructionReference reference, byte[] operands) {
            InstructionReference instruction = Constants.instructions[opcode];
            switch (instruction.Mnemonic) {
                case "JP":
                case "JR":
                    return new JumpInstruction(opcode, reference, operands);
                case "DI":
                    return new CPUControlInstruction(opcode, reference, operands);
                case "NOP":
                    return new NOPInstruction(opcode, reference, operands);
                case "INC":
                case "DEC":
                    return new INCInstruction(opcode, reference, operands);
                case "XOR":
                    return new XORInstruction(opcode, reference, operands);
                case "CP":
                    return new CompareInstruction(opcode, reference, operands);
                case "LD":
                case "LDH":
                    return new LoadInstruction(opcode, reference, operands);
                default:
                    throw new InstructionNotImplementedException(opcode, instruction);
            }
        }
    }
}
