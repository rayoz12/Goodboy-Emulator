using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator.CPU {

    public enum Register {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        H = 6,
        L = 7,
        AF = 8,
        BC = 9,
        DE = 10,
        HL = 11,
        SP = 12,
        PC = 13
    }

    public enum EightBitRegister {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        H = 6,
        L = 7
    }

    public enum SixteenBitRegister {
        AF = 8,
        BC = 9,
        DE = 10,
        HL = 11,
        SP = 12,
        PC = 13
    }

    internal class Registers {
        /// <summary>
        /// Accumulator
        /// </summary>
        public byte A = 0;

        public byte B = 0;
        public byte C = 0;
        public byte D = 0;
        public byte E = 0;
        public byte H = 0;
        public byte L = 0;
        /// <summary>
        /// Flags
        /// </summary>
        public CPUFlags F = 0;

        /// <summary>
        /// Stack Pointer
        /// </summary>
        public ushort SP = 0;
        /// <summary>
        /// Program Counter
        /// </summary>
        public ushort PC = 0;

        /// <summary>
        /// Combine two 8 bit params into a 16 bit param, taking r1 as the high bit and r2 as the low bits
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static ushort Combine(byte r1, byte r2) {
            ushort value = r1;
            value = (ushort) (value << 8);
            value |= r2;
            return value;
        }

        public ushort AF {
            get { return Combine(A, (byte)F); }
            //set { A = (byte)value; F = (CPUFlags)(value >> 8); }
        }

        public ushort BC {
            get { return Combine(B, C); }
            set { B = (byte)value; C = (byte)(value >> 8); }
        }

        public ushort DE {
            get { return Combine(D, E); }
            set { D = (byte)value; E = (byte)(value >> 8); }
        }

        public ushort HL {
            get { return Combine(H, L); }
            set { L = (byte)value; H = (byte)(value >> 8); }
        }

        [Flags]
        public enum CPUFlags
        {
            //_0 = 1 << 0,
            //_1 = 1 << 1,
            //_2 = 1 << 2,
            //_3 = 1 << 3,
            /// <summary>
            /// Carry Flag
            /// </summary>
            C  = 1 << 4,
            /// <summary>
            /// Half Carry Flag
            /// </summary>
            H  = 1 << 5,
            /// <summary>
            /// Subtract Flag
            /// </summary>
            N  = 1 << 6,
            /// <summary>
            /// Zero Flag
            /// </summary>
            Z  = 1 << 7
        }

        /// <summary>
        /// Set registers up ROM execution point: https://gbdev.io/pandocs/Power_Up_Sequence.html#cpu-registers
        /// </summary>
        public void Init() {
            A = 0x1;
            F = CPUFlags.Z;
            B = 0x0;
            C = 0x13;
            D = 0x0;
            E = 0xD8;
            H = 0x01;
            L = 0x4D;
            PC = 0x100;
            SP = 0xFFEE;

        }

        public byte GetRegisterValue(EightBitRegister reg) {
            switch (reg) {
                case EightBitRegister.A:
                    return A;
                case EightBitRegister.B:
                    return B;
                case EightBitRegister.C:
                    return C;
                case EightBitRegister.D:
                    return D;
                case EightBitRegister.E:
                    return E;
                case EightBitRegister.F:
                    return (byte)F;
                case EightBitRegister.H:
                    return H;
                case EightBitRegister.L:
                    return L;
                default:
                    throw new ArgumentException("Unknown Register!");
            }
        }

        public ushort GetRegisterValue(SixteenBitRegister reg) {
            switch (reg) {
                case SixteenBitRegister.AF:
                    return AF;
                case SixteenBitRegister.BC:
                    return BC;
                case SixteenBitRegister.DE:
                    return DE;
                case SixteenBitRegister.HL:
                    return HL;
                default:
                    throw new ArgumentException("Unknown Register!");
            }
        }

        public void SetRegisterValue(EightBitRegister reg, byte value) {
            switch (reg) {
                case EightBitRegister.A:
                    A = value;
                    break;
                case EightBitRegister.B:
                    B = value;
                    break;
                case EightBitRegister.C:
                    C = value;
                    break;
                case EightBitRegister.D:
                    D = value;
                    break;
                case EightBitRegister.E:
                    E = value;
                    break;
                case EightBitRegister.F:
                    F = (CPUFlags) value;
                    break;
                case EightBitRegister.H:
                    H = value;
                    break;
                case EightBitRegister.L:
                    L = value;
                    break;
                default:
                    throw new ArgumentException("Unknown Register!");
            }
        }

        public void SetRegisterValue(SixteenBitRegister reg, ushort value) {
            switch (reg) {
                case SixteenBitRegister.AF:
                    throw new ArgumentException("Can't set AF!");
                case SixteenBitRegister.BC:
                    BC = value;
                    break;
                case SixteenBitRegister.DE:
                    DE = value;
                    break;
                case SixteenBitRegister.HL:
                    HL = value;
                    break;
                default:
                    throw new ArgumentException("Unknown Register!");
            }
        }
    }
}
