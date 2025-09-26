using Dalamud.Interface.Textures.TextureWraps;

namespace SimpleInventoryTooltip {
    public class PlayerItem {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string IconPath { get; set; } = "";  // Path to the icon file
        public IDalamudTextureWrap? Icon { get; set; }
    }
}
