using Gameboy_Emulator.IO;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameboy_Emulator.UI {
    public class ImGuiInput : IJoypadInput {
        public DirectionalButtons getDirectionalButtonState() {
            DirectionalButtons buttons = 0;
            if (ImGui.IsKeyDown(ImGuiKey.RightArrow)) {
                buttons |= DirectionalButtons.Right;
            }
            if (ImGui.IsKeyDown(ImGuiKey.LeftArrow)) {
                buttons |= DirectionalButtons.Left;
            }
            if (ImGui.IsKeyDown(ImGuiKey.UpArrow)) {
                buttons |= DirectionalButtons.Up;
            }
            if (ImGui.IsKeyDown(ImGuiKey.DownArrow)) {
                buttons |= DirectionalButtons.Down;
            }

            return buttons;
        }
        public ActionButtons getActionButtonState() {
            ActionButtons buttons = 0;
            if (ImGui.IsKeyDown(ImGuiKey.A)) {
                buttons |= ActionButtons.A;
            }
            if (ImGui.IsKeyDown(ImGuiKey.B)) {
                buttons |= ActionButtons.B;
            }
            if (ImGui.IsKeyDown(ImGuiKey.Space)) {
                buttons |= ActionButtons.Select;
            }
            if (ImGui.IsKeyDown(ImGuiKey.Enter)) {
                buttons |= ActionButtons.Start;
            }

            return buttons;
        }
    }
}
