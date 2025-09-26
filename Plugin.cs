using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;

namespace SimpleInventoryTooltip {
    public sealed class Plugin : IDalamudPlugin {
        public string Name => "SimpleInventoryTooltip";

        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] internal static IDalamudTextureWrapFactory TexFactory { get; private set; } = null!;

        private readonly ItemManager itemManager;
        private readonly ItemUI itemUI;
        private readonly WindowSystem windowSystem = new("SimpleInventoryTooltip");

        public static string PluginDir => PluginInterface.AssemblyLocation.Directory?.FullName ?? ".";

        public Plugin() {
            var iconDir = Path.Combine(PluginDir, "Icons");
            itemManager = new ItemManager(PluginDir);
            itemUI = new ItemUI(iconDir);

            itemManager.Load(TexFactory);

            windowSystem.AddWindow(new ItemWindow(itemManager, itemUI, TexFactory));

            CommandManager.AddHandler("/tooltipinv", new CommandInfo((_, _) => {
                windowSystem.GetWindow("Item Window")!.IsOpen ^= true;
            }));
        }

        public void Dispose() {
            windowSystem.RemoveAllWindows();
            CommandManager.RemoveHandler("/tooltipinv");
        }

        public void DrawUI() {
            windowSystem.Draw();
        }

        private class ItemWindow : Window {
            private readonly ItemManager manager;
            private readonly ItemUI ui;
            private readonly IDalamudTextureWrapFactory texFactory;

            public ItemWindow(ItemManager manager, ItemUI ui, IDalamudTextureWrapFactory texFactory)
                : base("Item Window", ImGuiNET.ImGuiWindowFlags.NoCollapse) {
                this.manager = manager;
                this.ui = ui;
                this.texFactory = texFactory;
            }

            public override void Draw() {
                ui.Draw(manager, texFactory);
            }
        }
    }
}
