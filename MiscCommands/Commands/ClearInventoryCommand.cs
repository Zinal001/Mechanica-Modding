using ChatCommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiscCommands.Commands
{
    public class ClearInventoryCommand : ChatCommand
    {
        public override string Name => "Clear Inventory";
        public override string Command => "clearinv";
        public override string Description => "Removes all items from your inventory and hotbar.";
        public override string Syntax => "/clearinv";

        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                System.Reflection.FieldInfo _InventoryItemsField = typeof(Game.InventoryFramework.Inventory).GetField("items", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                System.Reflection.FieldInfo _InventoryHotbarItemsField = typeof(Game.InventoryFramework.Inventory).GetField("hotBarItems", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                if (Game.InventoryFramework.Inventory.InstanceExists)
                {
                    Game.InventoryFramework.InventoryItem[] items = (Game.InventoryFramework.InventoryItem[])_InventoryItemsField.GetValue(Game.InventoryFramework.Inventory.Instance);

                    if (items != null)
                    {
                        foreach (Game.InventoryFramework.InventoryItem item in items)
                            Game.InventoryFramework.Inventory.Instance.RemoveItemFromInventory(item);
                    }

                    Game.InventoryFramework.HotbarItem[] hotbarItems = (Game.InventoryFramework.HotbarItem[])_InventoryHotbarItemsField.GetValue(Game.InventoryFramework.Inventory.Instance);

                    if (hotbarItems != null)
                    {
                        foreach (Game.InventoryFramework.HotbarItem item in hotbarItems)
                            Game.InventoryFramework.Inventory.Instance.RemoveItemFromHotbar(item);
                    }

                    e.AddResponse("Inventory cleared");
                }
                else
                    e.AddResponse("Failed to clear inventory: No inventory instance found.");

                e.Handled = true;
            }
        }
    }
}
