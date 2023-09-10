using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Veldrid.Sdl2;
using Veldrid;
using Veldrid.StartupUtilities;
using System.Diagnostics;
using Gameboy_Emulator.CPU;
using Gameboy_Emulator.IO;

namespace Gameboy_Emulator.UI
{
    class UI : IDisposable {

        private CPU.CPU cpu;

        private Sdl2Window _window;
        private GraphicsDevice _gd;
        private CommandList _cl;
        private ImGuiController _controller;

        float deltaTime = 0f;
        Stopwatch stopwatch = Stopwatch.StartNew();

        // UI state
        private float _f = 0.0f;
        private int _counter = 0;
        private int _dragInt = 0;
        private Vector3 _clearColor = new Vector3(0.45f, 0.55f, 0.6f);

        public UI(CPU.CPU cpu) {
            this.cpu = cpu;
        }

        public void Init() {
            // Create window, GraphicsDevice, and all resources necessary for the demo.
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "Goodboy Emulator"),
                new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
                out _window,
                out _gd);
            _window.Resized += () => {
                _gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
                _controller.WindowResized(_window.Width, _window.Height);
            };
            _cl = _gd.ResourceFactory.CreateCommandList();
            _controller = new ImGuiController(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);
        }

        public bool Tick() {
            deltaTime = stopwatch.ElapsedTicks / (float)Stopwatch.Frequency;
            stopwatch.Restart();
            InputSnapshot snapshot = _window.PumpEvents();
            if (!_window.Exists) { return false; }

            _controller.Update(deltaTime, snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.

            DrawUI();

            _cl.Begin();
            _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
            _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));
            _controller.Render(_gd, _cl);
            _cl.End();
            _gd.SubmitCommands(_cl);
            _gd.SwapBuffers(_gd.MainSwapchain);

            return true;
        }

        private void DrawUI() {

            ImGui.Begin("GoodBoy Debug");
            ImGui.Text(string.Format("PC: 0x{0:X}, Instruction: {1}", cpu.registers.PC - cpu.LastIntructionRef.OperandLength, cpu.LastIntruction));

            if (ImGui.BeginTable("Registers", 2)) {
                ImGui.TableSetupColumn("Register");
                ImGui.TableSetupColumn("Value");
                ImGui.TableHeadersRow();

                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("AF");
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:X}", cpu.registers.AF));

                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("BC");
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:X}", cpu.registers.BC));
                
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("DE");
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:X}", cpu.registers.DE));
                
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("HL");
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:X}", cpu.registers.HL));
                
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("SP");
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:X}", cpu.registers.SP));
                
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text("PC");
                ImGui.TableNextColumn();
                ImGui.Text(string.Format("{0:X}", cpu.registers.PC));

                ImGui.EndTable();
            }

            float framerate = ImGui.GetIO().Framerate;
            ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");
            ImGui.End();
        }

        public void Dispose() {
            // Clean up Veldrid resources
            _gd.WaitForIdle();
            _controller.Dispose();
            _cl.Dispose();
            _gd.Dispose();
        }
    }
}
