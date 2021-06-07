using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscCommands
{
    internal static class Utils
    {
        private static Game.EntityFramework.CraftableObject[] _AllItems = null;

        private static String[] _MiscItems = new string[] {
            "Components/ResearchCrystal/ResearchCrystal",
            "Components/Stick/Stick",
            "Components/Sand/Sand",
            "Objects/Functional/Computer/Computer",
            "Components/Wood/Wood"
        };

        internal static System.Collections.IEnumerator LoadAllItems()
        {
            while (!Game.Crafting.CraftingMenu.InstanceExists)
                yield return new UnityEngine.WaitForSeconds(1f);

            var allItems = UnityEngine.Resources.FindObjectsOfTypeAll<Game.EntityFramework.CraftableObject>();
            List<Game.EntityFramework.CraftableObject> items = new List<Game.EntityFramework.CraftableObject>();

            foreach (var item in allItems)
            {
                if (!items.Any(i => i.pPrefabAssetPath == item.pPrefabAssetPath && i.pEntityName == item.pEntityName))
                    items.Add(item);
            }

            yield return new UnityEngine.WaitForEndOfFrame();

            if (Game.Crafting.CraftingMenu.InstanceExists)
            {
                System.Reflection.FieldInfo _CraftingMenuBoxRectsField = typeof(Game.Crafting.CraftingMenu).GetField("boxRects", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                System.Reflection.FieldInfo _CraftingMenuBoxRectToAssetPathField = typeof(Game.Crafting.CraftingMenu).GetField("boxRectToAssetPath", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                List<UnityEngine.RectTransform> boxRects = (List<UnityEngine.RectTransform>)_CraftingMenuBoxRectsField.GetValue(Game.Crafting.CraftingMenu.Instance);
                Dictionary<UnityEngine.RectTransform, String> boxRectToAssetPath = (Dictionary<UnityEngine.RectTransform, String>)_CraftingMenuBoxRectToAssetPathField.GetValue(Game.Crafting.CraftingMenu.Instance);

                List<String> visitedAssetPaths = new List<String>();

                if (boxRectToAssetPath != null)
                {
                    int pathsRan = 0;
                    foreach (var pair in boxRectToAssetPath)
                    {
                        pathsRan++;
                        if (!visitedAssetPaths.Contains(pair.Value))
                        {
                            visitedAssetPaths.Add(pair.Value);
                            if (!Game.InventoryFramework.Inventory.Instance.IsAssetPathValid(pair.Value))
                                continue;

                            Game.EntityFramework.CraftableObject component = UnityEngine.Resources.Load<UnityEngine.GameObject>(pair.Value).GetComponent<Game.EntityFramework.CraftableObject>();
                            if (component == null)
                                MiscCommandsMod.ModLogger.LogWarning($"Failed to load object '{pair.Value}'");
                            else
                            {
                                if (!items.Any(i => i.pPrefabAssetPath == component.pPrefabAssetPath && i.pEntityName == component.pEntityName))
                                    items.Add(component);
                            }
                        }

                        if (pathsRan % 5 == 0)
                            yield return new UnityEngine.WaitForEndOfFrame();
                    }
                }
            }

            foreach(String miscItem in _MiscItems)
            {
                if(!items.Any(i => i.pPrefabAssetPath == miscItem))
                {
                    Game.EntityFramework.CraftableObject component = UnityEngine.Resources.Load<UnityEngine.GameObject>(miscItem).GetComponent<Game.EntityFramework.CraftableObject>();
                    if (component == null)
                        MiscCommandsMod.ModLogger.LogWarning($"Failed to load object '{miscItem}'");
                    else
                        items.Add(component);
                }
            }

            _AllItems = items.ToArray();
            MiscCommandsMod.ModLogger.LogInfo("All items loaded");
        }

        internal static Game.EntityFramework.CraftableObject[] GetAllItems()
        {
            if(_AllItems == null)
            {
                var allItems = UnityEngine.Resources.FindObjectsOfTypeAll<Game.EntityFramework.CraftableObject>();
                List<Game.EntityFramework.CraftableObject> items = new List<Game.EntityFramework.CraftableObject>();

                foreach (var item in allItems)
                {
                    if (!items.Any(i => i.pPrefabAssetPath == item.pPrefabAssetPath && i.pEntityName == item.pEntityName))
                        items.Add(item);
                }

                if (Game.Crafting.CraftingMenu.InstanceExists)
                {
                    System.Reflection.FieldInfo _CraftingMenuBoxRectsField = typeof(Game.Crafting.CraftingMenu).GetField("boxRects", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    System.Reflection.FieldInfo _CraftingMenuBoxRectToAssetPathField = typeof(Game.Crafting.CraftingMenu).GetField("boxRectToAssetPath", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                    List<UnityEngine.RectTransform> boxRects = (List<UnityEngine.RectTransform>)_CraftingMenuBoxRectsField.GetValue(Game.Crafting.CraftingMenu.Instance);
                    Dictionary<UnityEngine.RectTransform, String> boxRectToAssetPath = (Dictionary<UnityEngine.RectTransform, String>)_CraftingMenuBoxRectToAssetPathField.GetValue(Game.Crafting.CraftingMenu.Instance);

                    List<String> visitedAssetPaths = new List<String>();

                    if (boxRectToAssetPath != null)
                    {
                        foreach (var pair in boxRectToAssetPath)
                        {
                            if (!visitedAssetPaths.Contains(pair.Value))
                            {
                                visitedAssetPaths.Add(pair.Value);
                                if (!Game.InventoryFramework.Inventory.Instance.IsAssetPathValid(pair.Value))
                                    continue;

                                Game.EntityFramework.CraftableObject component = UnityEngine.Resources.Load<UnityEngine.GameObject>(pair.Value).GetComponent<Game.EntityFramework.CraftableObject>();
                                if (component == null)
                                    MiscCommandsMod.ModLogger.LogWarning($"Failed to load object '{pair.Value}'");
                                else
                                {
                                    if (!items.Any(i => i.pPrefabAssetPath == component.pPrefabAssetPath && i.pEntityName == component.pEntityName))
                                        items.Add(component);
                                }
                            }
                        }
                    }
                }

                if(!items.Any(i => i.pPrefabAssetPath == "Components/ResearchCrystal/ResearchCrystal"))
                {
                    
                }

                _AllItems = items.ToArray();
            }

            return _AllItems;
        }
    }
}
