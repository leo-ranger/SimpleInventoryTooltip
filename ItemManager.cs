using Dalamud.Interface.Textures;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SimpleInventoryTooltip {
    public class ItemManager {
        private List<PlayerItem> items = new();
        private readonly string saveFile;

        public ItemManager(string pluginDir) {
            saveFile = Path.Combine(pluginDir, "items.json");
        }

        public void AddItem(PlayerItem item) => items.Add(item);
        public List<PlayerItem> GetItems() => items;

        public void Save() {
            File.WriteAllText(saveFile, JsonSerializer.Serialize(items));
        }

        public void Load(IDalamudTextureWrapFactory texFactory) {
            if (!File.Exists(saveFile)) return;

            var loaded = JsonSerializer.Deserialize<List<PlayerItem>>(File.ReadAllText(saveFile));
            if (loaded == null) return;

            items = loaded;

            foreach (var item in items) {
                if (File.Exists(item.IconPath)) {
                    item.Icon = texFactory.CreateFromImagePath(item.IconPath);
                }
            }
        }
    }
}
