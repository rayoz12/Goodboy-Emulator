using Gameboy_Emulator.CPU;
using Gameboy_Emulator.UI;

namespace Gameboy_Emulator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GoodBoy");


            ROM rom = new ROM("C:\\Users\\Ryan\\Desktop\\emulators\\developed\\Gameboy-Emulator\\Tetris.gb");
            CPU.CPU cpu = new CPU.CPU(rom);
            cpu.Init();

            UI.UI ui = new UI.UI(cpu);
            ui.Init();
            while (true) {
                cpu.Tick();
                if (!ui.Tick()) {
                    break;
                }
            }
        }
    }
}