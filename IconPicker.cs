using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace SimpleInventoryTooltip {
    public class IconPicker {
        private List<string> iconPaths = new();
        private Dictionary<string, IDalamudTextureWrap> icons = new();
        private Vector2 iconSize = new(32, 32);
        private int iconsPerRow = 8;
        private string selectedIcon = "";
        private readonly string iconFolder;

        public IconPicker(string folder) {
            iconFolder = folder;
            ReloadIcons();
        }

        public void ReloadIcons() {
            iconPaths.Clear();
            icons.Clear();

            if (!Directory.Exists(iconFolder))
                Directory.CreateDirectory(iconFolder);

            foreach (var file in Directory.GetFiles(iconFolder, "*.png")) {
                iconPaths.Add(file);
            }
        }

        public string Draw(IDalamudTextureWrapFactory texFactory) {
            int count = 0;
            ImGui.BeginChild("IconPickerScroll", new Vector2(0, 200), true);

            foreach (var path in iconPaths) {
                if (!icons.ContainsKey(path)) {
                    try {
                        icons[path] = texFactory.CreateFromImagePath(path);
                    }
                    catch {
                        continue;
                    }
                }

                var icon = icons[path];
                if (ImGui.ImageButton(icon.ImGuiHandle, iconSize)) {
                    selectedIcon = path;
                }

                count++;
                if (count % iconsPerRow != 0)
                    ImGui.SameLine();
            }

            ImGui.EndChild();

            if (!string.IsNullOrEmpty(selectedIcon)) {
                ImGui.Text("Selected:");
                if (icons.TryGetValue(selectedIcon, out var preview)) {
                    ImGui.Image(preview.ImGuiHandle, new Vector2(32, 32));
                    ImGui.SameLine();
                    ImGui.TextUnformatted(Path.GetFileName(selectedIcon));
                }
            }

            return selectedIcon;
        }
    }
}
