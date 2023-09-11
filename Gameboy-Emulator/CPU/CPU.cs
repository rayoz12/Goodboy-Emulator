using Gameboy_Emulator.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator.CPU
{
    public class CPU
    {
        internal ROM rom;
        internal Registers registers = new Registers();
        internal Memory memory = new Memory();
        public Joypad joypad;
        internal int clock = 0;

        public IInstruction LastIntruction;
        public InstructionReference LastIntructionRef;


        public CPU(ROM rom, IUserJoypadInput input) {
            this.rom = rom;
            joypad = new Joypad(memory, input);
        }

        public void Init() {
            registers.Init();
            rom.loadROM();
            memory.Init(rom);
            joypad.Init();
        }

        /// <summary>
        /// Intented to be called as part of a while (true) loop
        /// </summary>
        public void Tick() {

            // Update IO
            joypad.Tick();

            // Trigger interrupts


            // read from PC
            // Parse Opcode
            // Execute
            // Fetch
            byte opcode = memory[registers.PC];
            InstructionReference instructionRef = Constants.instructions[opcode];
            // Read in advance
            byte[] operands = new byte[instructionRef.OperandLength];
            for (int i = 0; i < instructionRef.OperandLength; i++) {
                operands[i] = memory[registers.PC + (i+1)];
            }
            registers.PC += instructionRef.OperandLength;

            // Decode
            IInstruction instruction = InstructionFactory.FromOpcode(opcode, instructionRef, operands);
                
            ushort oldPC = registers.PC;
            //Console.WriteLine("PC: 0x{0:X}, Instruction: {1}", registers.PC - instructionRef.OperandLength, instruction);
                
            // Execute
            clock += instruction.Execute(this);

            if (oldPC == registers.PC) {
                // We have jumped, we don't increment the PC, otherwise we inc
                registers.PC++;
            }


            // externalities
            LastIntruction = instruction;
            LastIntructionRef = instructionRef;

        }

    }
}
