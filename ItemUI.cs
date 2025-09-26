using Dalamud.Interface.Textures;
using ImGuiNET;
using System.IO;
using System.Numerics;

namespace SimpleInventoryTooltip {
    public class ItemUI {
        private string newName = "";
        private string newDesc = "";
        private string selectedIconPath = "";
        private readonly IconPicker iconPicker;

        public ItemUI(string iconFolder) {
            iconPicker = new IconPicker(iconFolder);
        }

        public void Draw(ItemManager manager, IDalamudTextureWrapFactory texFactory) {
            ImGui.InputText("Item Name", ref newName, 100);
            ImGui.InputTextMultiline("Description", ref newDesc, 500);

            ImGui.Text("Pick an Icon:");
            selectedIconPath = iconPicker.Draw(texFactory);

            if (ImGui.Button("Add Item")) {
                var item = new PlayerItem {
                    Name = newName,
                    Description = newDesc,
                    IconPath = selectedIconPath
                };

                if (!string.IsNullOrEmpty(selectedIconPath) && File.Exists(selectedIconPath))
                    item.Icon = texFactory.CreateFromImagePath(selectedIconPath);

                manager.AddItem(item);
                newName = "";
                newDesc = "";
                selectedIconPath = "";

                manager.Save();
            }

            ImGui.Separator();
            foreach (var item in manager.GetItems()) {
                if (item.Icon != null) {
                    ImGui.Image(item.Icon.ImGuiHandle, new Vector2(32, 32));
                    ImGui.SameLine();
                }
                ImGui.Text(item.Name);

                if (ImGui.IsItemHovered()) {
                    ImGui.BeginTooltip();
                    if (item.Icon != null) {
                        ImGui.Image(item.Icon.ImGuiHandle, new Vector2(32, 32));
                        ImGui.SameLine();
                    }
                    ImGui.TextUnformatted(item.Name);
                    ImGui.Separator();
                    ImGui.TextWrapped(item.Description);
                    ImGui.EndTooltip();
                }
            }
        }
    }
}
