using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator
{
    /**
     * Decodes a ROM into it's instructions
     */
    public class ROM
    {

        byte[] _rom;

        public byte this[int i] {
            get { return _rom[i]; }
            //set { _rom[i] = value; }
        }
        public byte this[Index i] {
            get { return _rom[i]; }
            //set { _rom[i] = value; }
        }
        public byte[] this[Range i] {
            get { return _rom[i]; }
            //set { _rom[i] = value; }
        }
        public string RomPath { get; }
        public ROM(string rom)
        {
            RomPath = rom;
        }

        public byte[] loadROM()
        {
            if (RomPath == null) {
                throw new Exception("ROM not selected!");
            }

            _rom = File.ReadAllBytes(RomPath);
            return _rom;
        }        
    }
}
