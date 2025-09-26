namespace SimpleInventoryTooltip {
    // For future extension if you want static, predefined items.
    public class Item {
        public string Name { get; set; }
        public string Description { get; set; }

        public Item(string name, string description) {
            Name = name;
            Description = description;
        }
    }
}
