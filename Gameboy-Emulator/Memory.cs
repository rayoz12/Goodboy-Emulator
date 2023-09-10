using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator
{

    /// <summary>
    /// To be used for Memory Bank Controllers
    /// </summary>
    public interface IMemoryProxy {
        /// <summary>
        /// Which addresses we intend to proxy
        /// </summary>
        int[] addresses { get; }

    }

    public class Memory {
        private byte[] _memoryMap = new byte[0x10000];

        public byte this[int i] {
            get { return _memoryMap[i]; }
            set { _memoryMap[i] = value; }
        }
        public byte this[Index i] {
            get { return _memoryMap[i]; }
            set { _memoryMap[i] = value; }
        }
        public byte[] this[Range i] {
            get { return _memoryMap[i]; }
            //set { _memoryMap[i] = value; }
        }

        public static readonly Range ROMBank00 = 0x0000..0x3F00;
        public static readonly Range ROMBankN  = 0x4000..0x7FFF;
        public static readonly Range VRAM = 0x8000..0x9FFF;
        public static readonly Range WRAM = 0xC000..0xCFFF;
        public static readonly Range WRAM2 = 0xD000..0xDFFF;
        public static readonly Range ECHORAM = 0xE000..0xFDFF;
        public static readonly Range OAMRAM = 0xFE00..0xFE8F;
        public static readonly Range IO = 0xFF00..0xFF7F;
        public static readonly Range HRAM = 0xFF80..0xFFFE;

        public static readonly Index IME = 0xFFFF;

        /// <summary>
        /// Setup the memory Subsystem with the loaded rom
        /// </summary>
        /// <param name="rom"></param>
        public void Init(ROM rom) {
            // HRAM
            _memoryMap[0xFF05] = 0x00;
            _memoryMap[0xFF06] = 0x00;
            _memoryMap[0xFF07] = 0x00;
            _memoryMap[0xFF10] = 0x80;
            _memoryMap[0xFF11] = 0xFB;
            _memoryMap[0xFF12] = 0xF3;
            _memoryMap[0xFF14] = 0xBF;
            _memoryMap[0xFF16] = 0x3F;
            _memoryMap[0xFF17] = 0x00;
            _memoryMap[0xFF19] = 0xBF;
            _memoryMap[0xFF1A] = 0x7F;
            _memoryMap[0xFF1B] = 0xFF;
            _memoryMap[0xFF1C] = 0x9F;
            _memoryMap[0xFF1E] = 0xBF;
            _memoryMap[0xFF20] = 0xFF;
            _memoryMap[0xFF21] = 0x00;
            _memoryMap[0xFF22] = 0x00;
            _memoryMap[0xFF23] = 0xBF;
            _memoryMap[0xFF24] = 0x77;
            _memoryMap[0xFF25] = 0xF3;
            _memoryMap[0xFF26] = 0xF1;
            _memoryMap[0xFF40] = 0x91;
            _memoryMap[0xFF42] = 0x00;
            _memoryMap[0xFF43] = 0x00;
            _memoryMap[0xFF45] = 0x00;
            _memoryMap[0xFF47] = 0xFC;
            _memoryMap[0xFF48] = 0xFF;
            _memoryMap[0xFF49] = 0xFF;
            _memoryMap[0xFF4A] = 0x00;
            _memoryMap[0xFF4B] = 0x00;
            _memoryMap[0xFFFF] = 0x00;

            // Initialise Game cart
            var bank0 = rom[ROMBank00];
            _memoryMap.Copy(ROMBank00, bank0, ROMBank00);
        }

        public byte GetIOOffset(int offset) {
            return _memoryMap[IO.Start.Value + offset];
        }

        public void setIME(bool enabled) {
            _memoryMap[IME] = (byte)(enabled ? 1 : 0);
        }
    }
}
