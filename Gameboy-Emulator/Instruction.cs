using Gameboy_Emulator.CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator
{

    public interface IInstruction {
        /// <summary>
        /// Runs the instruction and returns the clock cycles it takes
        /// </summary>
        /// <returns>Clock cycles</returns>
        public abstract int Execute(CPU.CPU cpu);
    }

    public struct InstructionReference {
        public string Disassembly;
        public string Mnemonic;
        public byte OperandLength;
        //IInstruction Instruction;
        public InstructionReference(string disassembly, string mnemonic, byte operandLength) {  
            Disassembly = disassembly;
            Mnemonic = mnemonic;
            OperandLength = operandLength;
        }

        public override string ToString() => string.Format("{0}: {1} {2}", Mnemonic, Disassembly, OperandLength);
    }

    abstract public class Instruction: IInstruction {

        public byte opcode;
        public InstructionReference reference;
        public byte[] operands;

        public Instruction(byte opcode, InstructionReference reference, byte[] operands) {
            this.opcode = opcode;
            this.reference = reference;
            this.operands = operands;
        }

        public virtual int Execute(CPU.CPU cpu) {
            return 0;
        }

        public override string ToString() {
            string[] hex = operands.Select(x => x.ToString("X")).ToArray();
            return string.Format("{0}: {1} {2}", reference.Disassembly, reference.Mnemonic, string.Join(",", hex));
        }
    }

    public class LoadInstruction: Instruction, IInstruction {

        public LoadInstruction(byte opcode, InstructionReference reference, byte[] operands) :
            base(opcode, reference, operands) {

        }

        public override int Execute(CPU.CPU cpu) {
            int address;
            switch (opcode) {
                case 0x02:
                    address = Memory.IO.Start.Value + cpu.registers.BC;
                    cpu.memory[address] = cpu.registers.A;
                    return 8;
                case 0x12:
                    address = Memory.IO.Start.Value + cpu.registers.DE;
                    cpu.memory[address] = cpu.registers.A;
                    return 8;
                case 0x22:
                    address = cpu.registers.HL;
                    cpu.memory[address] = cpu.registers.A;
                    cpu.registers.HL++;
                    return 8;
                case 0x32:
                    address = cpu.registers.HL;
                    cpu.memory[address] = cpu.registers.A;
                    cpu.registers.HL--;
                    return 8;
                case 0x06:
                    cpu.registers.B = operands[0];
                    return 8;
                case 0x16:
                    cpu.registers.D = operands[0];
                    return 8;
                case 0x26:
                    cpu.registers.H = operands[0];
                    return 8;
                case 0x36:
                    address = Memory.IO.Start.Value + cpu.registers.HL;
                    cpu.memory[address] = operands[0];
                    return 12;
                case 0x0A:
                    address = Memory.IO.Start.Value + cpu.registers.BC;
                    cpu.registers.A = cpu.memory[address];
                    return 8;
                case 0x1A:
                    address = Memory.IO.Start.Value + cpu.registers.DE;
                    cpu.registers.A = cpu.memory[address];
                    return 8;
                case 0x2A:
                    address = Memory.IO.Start.Value + cpu.registers.HL;
                    cpu.registers.A = cpu.memory[address];
                    cpu.registers.HL++;
                    return 8;
                case 0x3A:
                    address = Memory.IO.Start.Value + cpu.registers.HL;
                    cpu.registers.A = cpu.memory[address];
                    cpu.registers.HL--;
                    return 8;
                case 0x0E:
                    cpu.registers.C = operands[0];
                    return 8;
                case 0x1E:
                    cpu.registers.E = operands[0];
                    return 8;
                case 0x2E:
                    cpu.registers.L = operands[0];
                    return 8;
                case 0x3E:
                    cpu.registers.A = operands[0];
                    return 8;

                case 0x40:
                    // cpu.registers.B = cpu.registers.B; // Excellent, just implementing to the spec
                    return 4;
                case 0x50:
                    cpu.registers.D = cpu.registers.B;
                    return 4;
                case 0x60:
                    cpu.registers.H = cpu.registers.B;
                    return 4;
                case 0x70:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.B;
                    return 8;
                case 0x41:
                     cpu.registers.B = cpu.registers.C;
                    return 4;
                case 0x51:
                    cpu.registers.D = cpu.registers.C;
                    return 4;
                case 0x61:
                    cpu.registers.H = cpu.registers.C;
                    return 4;
                case 0x71:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.C;
                    return 8;
                case 0x42:
                    cpu.registers.B = cpu.registers.D;
                    return 4;
                case 0x52:
                    //cpu.registers.D = cpu.registers.D;
                    return 4;
                case 0x62:
                    cpu.registers.H = cpu.registers.D;
                    return 4;
                case 0x72:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.D;
                    return 8;
                case 0x43:
                    cpu.registers.B = cpu.registers.E;
                    return 4;
                case 0x53:
                    cpu.registers.D = cpu.registers.E;
                    return 4;
                case 0x63:
                    cpu.registers.H = cpu.registers.E;
                    return 4;
                case 0x73:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.E;
                    return 8;
                case 0x44:
                    cpu.registers.B = cpu.registers.H;
                    return 4;
                case 0x54:
                    cpu.registers.D = cpu.registers.H;
                    return 4;
                case 0x64:
                    //cpu.registers.H = cpu.registers.H;
                    return 4;
                case 0x74:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.H;
                    return 8;
                case 0x45:
                    cpu.registers.B = cpu.registers.L;
                    return 4;
                case 0x55:
                    cpu.registers.D = cpu.registers.L;
                    return 4;
                case 0x65:
                    cpu.registers.H = cpu.registers.L;
                    return 4;
                case 0x75:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.L;
                    return 8;
                
                case 0x46:
                    cpu.registers.B = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;
                case 0x56:
                    cpu.registers.D = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;
                case 0x66:
                    cpu.registers.H = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;

                case 0x47:
                    cpu.registers.B = cpu.registers.A;
                    return 4;
                case 0x57:
                    cpu.registers.D = cpu.registers.A;
                    return 4;
                case 0x67:
                    cpu.registers.H = cpu.registers.A;
                    return 4;
                case 0x77:
                    cpu.memory[Memory.IO.Start.Value + cpu.registers.HL] = cpu.registers.A;
                    return 8;
                
                case 0x48:
                    cpu.registers.C = cpu.registers.B;
                    return 4;
                case 0x58:
                    cpu.registers.E = cpu.registers.B;
                    return 4;
                case 0x68:
                    cpu.registers.L = cpu.registers.B;
                    return 4;
                case 0x78:
                    cpu.registers.A = cpu.registers.B;
                    return 4;
                case 0x49:
                    //cpu.registers.C = cpu.registers.C;
                    return 4;
                case 0x59:
                    cpu.registers.E = cpu.registers.C;
                    return 4;
                case 0x69:
                    cpu.registers.L = cpu.registers.C;
                    return 4;
                case 0x79:
                    cpu.registers.A = cpu.registers.C;
                    return 4;
                case 0x4A:
                    cpu.registers.C = cpu.registers.D;
                    return 4;
                case 0x5A:
                    cpu.registers.E = cpu.registers.D;
                    return 4;
                case 0x6A:
                    cpu.registers.L = cpu.registers.D;
                    return 4;
                case 0x7A:
                    cpu.registers.A = cpu.registers.D;
                    return 4;
                case 0x4B:
                    cpu.registers.C = cpu.registers.E;
                    return 4;
                case 0x5B:
                    //cpu.registers.E = cpu.registers.E;
                    return 4;
                case 0x6B:
                    cpu.registers.L = cpu.registers.E;
                    return 4;
                case 0x7B:
                    cpu.registers.A = cpu.registers.E;
                    return 4;
                case 0x4C:
                    cpu.registers.C = cpu.registers.H;
                    return 4;
                case 0x5C:
                    cpu.registers.E = cpu.registers.H;
                    return 4;
                case 0x6C:
                    cpu.registers.L = cpu.registers.H;
                    return 4;
                case 0x7C:
                    cpu.registers.A = cpu.registers.H;
                    return 4;
                case 0x4D:
                    cpu.registers.C = cpu.registers.L;
                    return 4;
                case 0x5D:
                    cpu.registers.E = cpu.registers.L;
                    return 4;
                case 0x6D:
                    //cpu.registers.L = cpu.registers.L;
                    return 4;
                case 0x7D:
                    cpu.registers.A = cpu.registers.L;
                    return 4;
                case 0x4E:
                    cpu.registers.C = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;
                case 0x5E:
                    cpu.registers.E = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;
                case 0x6E:
                    cpu.registers.L = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;
                case 0x7E:
                    cpu.registers.A = cpu.memory[Memory.IO.Start.Value + cpu.registers.HL];
                    return 4;
                case 0x4F:
                    cpu.registers.C = cpu.registers.A;
                    return 4;
                case 0x5F:
                    cpu.registers.E = cpu.registers.A;
                    return 4;
                case 0x6F:
                    cpu.registers.L = cpu.registers.A;
                    return 4;
                case 0x7F:
                    //cpu.registers.A = cpu.registers.A;
                    return 4;


                case 0x01:
                    cpu.registers.BC = Registers.Combine(operands[1], operands[0]);
                    return 12;
                case 0x11:
                    cpu.registers.DE = Registers.Combine(operands[1], operands[0]);
                    return 12;
                case 0x21:
                    cpu.registers.HL = Registers.Combine(operands[1], operands[0]);
                    return 12;
                case 0x31:
                    cpu.registers.SP = Registers.Combine(operands[1], operands[0]);
                    return 12;


                case 0xE0:
                    address = Memory.IO.Start.Value + operands[0];
                    cpu.memory[address] = cpu.registers.A;
                    return 12;
                case 0xF0:
                    address = Memory.IO.Start.Value + operands[0];
                    cpu.registers.A = cpu.memory[address];
                    return 12;
                case 0xE2:
                    address = Memory.IO.Start.Value + cpu.registers.C;
                    cpu.memory[address] = cpu.registers.A;
                    return 8;
                case 0xF2:
                    address = Memory.IO.Start.Value + cpu.registers.C;
                    cpu.registers.A = cpu.memory[address];
                    return 8;
                case 0xEA:
                    address = Memory.IO.Start.Value + Registers.Combine(operands[1], operands[0]);
                    cpu.memory[address] = cpu.registers.A;
                    return 16;
                case 0xFA:
                    address = Memory.IO.Start.Value + Registers.Combine(operands[1], operands[0]);
                    cpu.registers.A = cpu.memory[address];
                    return 16;

                case 0x08:
                    address = Memory.IO.Start.Value + Registers.Combine(operands[1], operands[0]);
                    cpu.memory[address] = (byte) cpu.registers.SP;
                    cpu.memory[address+1] = (byte) (cpu.registers.SP >> 8);
                    return 20;
                case 0xF8:
                    cpu.registers.HL = (ushort)(cpu.registers.SP + ((ushort) operands[0]));
                    cpu.registers.F = 0;
                    if ((((cpu.registers.SP & 0xFFF) + (cpu.registers.HL & 0xFFF)) & 0x1000) == 0x1000) {
                        cpu.registers.F |= Registers.CPUFlags.H;
                    }
                    if (((int)cpu.registers.SP) + ((int)operands[0]) > 0xFFFF) {
                        cpu.registers.F |= Registers.CPUFlags.C;
                    }
                    return 12;

                default:
                    throw new InstructionNotImplementedException(opcode, reference);
            }
        }
    }

    public class JumpInstruction: Instruction, IInstruction {

        public JumpInstruction(byte opcode, InstructionReference reference, byte[] operands):
            base(opcode, reference, operands) {
            
        }
        public override int Execute(CPU.CPU cpu) {
            switch (opcode) {
                case 0xC3:
                    cpu.registers.PC = Registers.Combine(operands[1], operands[0]);
                    return 16;
                case 0x20:
                    if ((cpu.registers.F & Registers.CPUFlags.Z) == 0) {
                        sbyte s = unchecked((sbyte)operands[0]);
                        cpu.registers.PC = (ushort)(cpu.registers.PC + s);
                    }
                    return 16;
                //break;
                default:
                    throw new InstructionNotImplementedException(opcode, reference);
            }
        }
    }
    public class NOPInstruction : Instruction, IInstruction {

        public NOPInstruction(byte opcode, InstructionReference reference, byte[] operands):
            base(opcode, reference, operands) { }
        public override int Execute(CPU.CPU cpu) {
            return 4;
        }
    }

    public class INCInstruction : Instruction, IInstruction {

        //ushort[] incInstructions = new ushort[] { 0x03, 0x04, 0x0C, 0x13, 0x14, 0x1C, 0x23, 0x24, 0x2C, 0x33, 0x3C };
        //ushort[] decInstructions = new ushort[] { }
        bool isIncrementing;

        public INCInstruction(byte opcode, InstructionReference reference, byte[] operands) :
            base(opcode, reference, operands) {
            isIncrementing = reference.Mnemonic == "INC";

        }
        public override int Execute(CPU.CPU cpu) {
            int clock = 4;
            ushort oldValue;
            ushort newValue;
            if (opcode == 0x34 || opcode == 0x35) {
                // Special memory inc/dec
                oldValue = cpu.memory[operands[0]];
                newValue = opcode == 0x34 ? ++cpu.memory[operands[0]] : --cpu.memory[operands[0]];
                clock = 12;
            }
            else {
                Register reg;
                switch (opcode) {
                    case 0x03: // INC
                    case 0x0B: // DEC
                        reg = Register.BC;
                        break;
                    case 0x04: // INC
                    case 0x05: // DEC
                        reg = Register.B;
                        break;
                    case 0x0C: // INC
                    case 0x0D: // DEC
                        reg = Register.C;
                        break;
                    case 0x13: // INC
                    case 0x1B: // DEC
                        reg = Register.DE;
                        break;
                    case 0x14: // INC
                    case 0x15: // DEC
                        reg = Register.D;
                        break;
                    case 0x1C: // INC
                    case 0x1D: // DEC
                        reg = Register.E;
                        break;
                    case 0x23: // INC
                    case 0x2B: // DEC
                        reg = Register.HL;
                        break;
                    case 0x24: // INC
                    case 0x25: // DEC
                        reg = Register.H;
                        break;
                    case 0x2C: // INC
                    case 0x2D: // DEC
                        reg = Register.L;
                        break;
                    case 0x33: // INC
                    case 0x3B: // DEC
                        reg = Register.SP;
                        break;
                    case 0x3C: // INC
                    case 0x3D: // DEC
                        reg = Register.A;
                        break;
                    default:
                        throw new InstructionNotImplementedException(opcode, reference);
                }



                if (Enum.IsDefined<SixteenBitRegister>((SixteenBitRegister)reg)) {
                    SixteenBitRegister sixteen = (SixteenBitRegister)reg;
                    ushort value = cpu.registers.GetRegisterValue(sixteen);
                    oldValue = value;
                    value = isIncrementing ? ++value : --value;
                    cpu.registers.SetRegisterValue(sixteen, value);
                    newValue = (ushort)value;
                }
                else if (Enum.IsDefined<EightBitRegister>((EightBitRegister)reg)) {
                    EightBitRegister eight = (EightBitRegister)reg;
                    byte value = cpu.registers.GetRegisterValue(eight);
                    oldValue = value;
                    value = isIncrementing ? ++value : --value;
                    cpu.registers.SetRegisterValue(eight, value);
                    newValue = (ushort)value;
                }
                else {
                    throw new Exception("Unkown Register!");
                }
            }

            if (newValue == 0) {
                cpu.registers.F |= Registers.CPUFlags.Z;
            }

            if (isIncrementing) {
                cpu.registers.F &= ~Registers.CPUFlags.N;
            }
            else {
                cpu.registers.F |= Registers.CPUFlags.N;
            }

            bool HC;
            if (opcode == 0x34) {
                HC = (((oldValue & 0xFFF) + (newValue & 0xFFF)) & 0x1000) == 0x1000;
            }
            else {
                HC = (((oldValue & 0xF) + (newValue & 0xF)) & 0x10) == 0x10;
            }

            if (HC) {
                cpu.registers.F |= Registers.CPUFlags.H;
            }
            else {
                cpu.registers.F &= ~Registers.CPUFlags.H;
            }

            if (newValue == 0) {
                cpu.registers.F |= Registers.CPUFlags.Z;
            }
            else {
                cpu.registers.F &= ~Registers.CPUFlags.Z;
            }

            return clock;
        }
    }

    public class CompareInstruction : Instruction, IInstruction {

        public CompareInstruction(byte opcode, InstructionReference reference, byte[] operands) :
            base(opcode, reference, operands) { }
        public override int Execute(CPU.CPU cpu) {
            int clock;
            int value;

            if (opcode == 0xBE) {
                // Value from pointer in HL
                clock = 8;
                value = cpu.memory[cpu.registers.HL];
            }
            else if (opcode == 0xFE) {
                // Value from literal
                clock = 8;
                value = operands[0];
            }
            else {
                // Value from register
                clock = 4;
                Register reg;
                switch (opcode) {
                    case 0xB8:
                        reg = Register.B;
                        break;
                    case 0xB9:
                        reg = Register.C;
                        break;
                    case 0xBA:
                        reg = Register.D;
                        break;
                    case 0xBB:
                        reg = Register.E;
                        break;
                    case 0xBC:
                        reg = Register.H;
                        break;
                    case 0xBD:
                        reg = Register.L;
                        break;
                    case 0xBF:
                        reg = Register.A;
                        break;
                    default:
                        throw new InstructionNotImplementedException(opcode, reference);
                }
                value = cpu.registers.GetRegisterValue((EightBitRegister)reg);
            }

            byte A = cpu.registers.A;
            int result = A - value;
            cpu.registers.F = Registers.CPUFlags.N;
            
            if (result == 0) {
                cpu.registers.F |= Registers.CPUFlags.Z;
            }
            
            if ((((A & 0xF) - (value & 0xF)) & 0x10) == 0x10) {
                cpu.registers.F |= Registers.CPUFlags.H;
            }

            if (A < value) {
                cpu.registers.F |= Registers.CPUFlags.C;
            }

            return clock;
        }
    }

    public class XORInstruction : Instruction, IInstruction {

        public XORInstruction(byte opcode, InstructionReference reference, byte[] operands) :
            base(opcode, reference, operands) { }
        public override int Execute(CPU.CPU cpu) {
            int clock;
            switch (opcode) {
                case 0xA8:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.B);
                    clock = 4;
                    break;
                case 0xA9:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.C);
                    clock = 4;
                    break;
                case 0xAA:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.D);
                    clock = 4;
                    break;
                case 0xAB:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.E);
                    clock = 4;
                    break;
                case 0xAC:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.H);
                    clock = 4;
                    break;
                case 0xAD:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.L);
                    clock = 4;
                    break;
                case 0xAE:
                    byte value = cpu.memory.GetIOOffset(cpu.registers.HL);
                    cpu.registers.A = (byte) (cpu.registers.A ^ value);
                    clock = 8;
                    break;
                case 0xAF:
                    cpu.registers.A = (byte) (cpu.registers.A ^ cpu.registers.A);
                    clock = 4;
                    break;
                case 0xEE:
                    cpu.registers.A = (byte) (cpu.registers.A ^ operands[0]);
                    clock = 4;
                    break;
                default:
                    throw new InstructionNotImplementedException(opcode, reference);
            }

            cpu.registers.F = cpu.registers.A == 0 ? Registers.CPUFlags.Z : 0;
            return clock;
        }
    }

    public class CPUControlInstruction : Instruction, IInstruction {

        public CPUControlInstruction(byte opcode, InstructionReference reference, byte[] operands) :
            base(opcode, reference, operands) { }
        public override int Execute(CPU.CPU cpu) {
            switch (opcode) {
                case 0xF3:
                    cpu.memory.setIME(false);
                    break;
            }

            return 4;
        }
    }
}
