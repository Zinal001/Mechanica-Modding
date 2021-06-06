using ChatCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscCommands.Commands
{
    public class ExportItemsCommand : ChatCommand
    {
        public override string Name => "Export Items";
        public override string Command => "exportitems";
        public override string Description => "Export a list of all items to a file.";
        public override string Syntax => "/exportitems";

        public override void OnHandle(ChatCommandEventArgs e)
        {
            if(Command.Equals(e.Command, StringComparison.OrdinalIgnoreCase))
            {
                var allItems = Utils.GetAllItems();

                String lst = "\"Name\",\"Asset Path\",\"Type\",\"Description\"\n";
                lst += String.Join("\n", allItems.OrderBy(i => i.pObjectType).ThenBy(i => i.pEntityName).Select(i => $"\"{i.pEntityName}\",\"{i.pPrefabAssetPath}\",\"{i.pObjectType}\",\"{i.pEntityDesc.Replace("\"", "\"\"")}\""));

                System.IO.File.WriteAllText("items.csv", lst);

                e.AddResponse("All craftable items written to items.csv");
                e.Handled = true;
            }
        }
    }
}
