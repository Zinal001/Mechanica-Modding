using ChatCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscCommands.Commands
{
    public class GiveCommand : ChatCommand
    {
        public override string Name => "Give Item";
        public override string Command => "give";
        public override string Description => "Add an item to your inventory.";
        public override string Syntax => "/give [itemName] <amount>";

        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                var allItems = Utils.GetAllItems();

                if (!e.Arguments.Any())
                {
                    e.AddResponse("Syntax error. Expected syntax: /give \"[itemname]\" <amount>. (Amount is optional)");
                    return;
                }

                var foundItems = allItems.Where(i => i.pEntityName.Equals(e.Arguments[0], StringComparison.OrdinalIgnoreCase)).ToArray();
                if (!foundItems.Any())
                {
                    foundItems = allItems.Where(i => i.pEntityName.ToLower().Contains(e.Arguments[0])).ToArray();

                    if (!foundItems.Any())
                    {
                        e.AddResponse($"Unable to find item with name '{e.Arguments[0]}'.");
                        return;
                    }
                }

                if (foundItems.Length > 1)
                {
                    String foundItemNames = String.Join(", ", foundItems.Select(i => i.pEntityName));
                    e.AddResponse($"Found multiple items with name '{e.Arguments[0]}', please be more specific. Items found: {foundItemNames}");
                }
                else if (!Game.InventoryFramework.Inventory.InstanceExists)
                    e.AddResponse("Failed to give item: No inventory instance found.");
                else
                {
                    int amount = 1;
                    if (e.Arguments.Length > 1 && int.TryParse(e.Arguments[1], out int tAmount))
                        amount = tAmount;

                    Game.InventoryFramework.Inventory.Instance.AddItemToInventory(foundItems[0].pPrefabAssetPath, amount, false);
                    e.AddResponse($"{amount} of {foundItems[0].pEntityName} added to inventory.");
                }
            }
        }
    }
}
