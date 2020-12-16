﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Dungeonator;
namespace GungeonAPI
{
    public static class StaticReferences
    {
        public static Dictionary<string, AssetBundle> AssetBundles;
        public static Dictionary<string, GenericRoomTable> RoomTables;
        public static SharedInjectionData subShopTable;
        public static Dictionary<string, string> roomTableMap = new Dictionary<string, string>()
        {
            { "castle", "Castle_RoomTable" },
            { "gungeon", "Gungeon_RoomTable" },
            { "mines", "Mines_RoomTable" },
            { "catacombs", "Catacomb_RoomTable" },
            { "forge", "Forge_RoomTable" },
            { "sewer", "Sewer_RoomTable" },
            { "cathedral", "Cathedral_RoomTable" },
            { "bullethell", "BulletHell_RoomTable" }, 
        };

        public static Dictionary<string, string> specialRoomTableMap = new Dictionary<string, string>()
        {
            { "special", "basic special rooms (shrines, etc)" },
            { "shop", "Shop Room Table" },
            { "secret", "secret_room_table_01" },
        };

        public static string[] assetBundleNames = new string[]
        {
            "shared_auto_001",
            "shared_auto_002",
            "brave_resources_001"
        };

        public static string[] dungeonPrefabNames = new string[]
        {
            "base_gungeon",
            "base_castle",
            "base_mines",
            "base_catacombs",
            "base_forge",
            "base_sewer",
            "base_cathedral",
            "base_bullethell",
        };

        public static void Init()
        {
            AssetBundles = new Dictionary<string, AssetBundle>();
            foreach (var name in assetBundleNames)
            {
                try
                {
                    var bundle = ResourceManager.LoadAssetBundle(name);
                    if(bundle == null)
                    {
                        Tools.PrintError($"Failed to load asset bundle: {name}");
                        continue;
                    }
                    AssetBundles.Add(name, ResourceManager.LoadAssetBundle(name));
                }
                catch (Exception e)
                {
                    Tools.PrintError($"Failed to load asset bundle: {name}");
                    Tools.PrintException(e);
                }
            }

            RoomTables = new Dictionary<string, GenericRoomTable>();
            foreach (var entry in roomTableMap)
            {
                try
                {
                    var table = DungeonDatabase.GetOrLoadByName($"base_{entry.Key}").PatternSettings.flows[0].fallbackRoomTable;
                    RoomTables.Add(entry.Key, table);
                }
                catch (Exception e)
                {
                    Tools.PrintError($"Failed to load room table: {entry.Key}:{entry.Value}");
                    Tools.PrintException(e);
                }
            }

            foreach (var entry in specialRoomTableMap)
            {
                try
                {
                    var table = GetAsset<GenericRoomTable>(entry.Value);
                    RoomTables.Add(entry.Key, table);
                }
                catch (Exception e)
                {
                    Tools.PrintError($"Failed to load special room table: {entry.Key}:{entry.Value}");
                    Tools.PrintException(e);
                }
            }

            subShopTable = AssetBundles["shared_auto_001"].LoadAsset<SharedInjectionData>("_global injected subshop table");
            //foreach(var data in subShopTable.InjectionData)
            //{
            //    Tools.LogPropertiesAndFields(data, data.annotation);
            //}

            Tools.Print("Static references initialized.");
        }

        public static GenericRoomTable GetRoomTable(GlobalDungeonData.ValidTilesets tileset)
        {
            switch (tileset)
            {
                case GlobalDungeonData.ValidTilesets.CASTLEGEON:
                    return RoomTables["castle"];
                case GlobalDungeonData.ValidTilesets.GUNGEON:
                    return RoomTables["gungeon"];
                case GlobalDungeonData.ValidTilesets.MINEGEON:
                    return RoomTables["mines"];
                case GlobalDungeonData.ValidTilesets.CATACOMBGEON:
                    return RoomTables["catacombs"];
                case GlobalDungeonData.ValidTilesets.FORGEGEON:
                    return RoomTables["forge"];
                case GlobalDungeonData.ValidTilesets.SEWERGEON:
                    return RoomTables["sewer"];
                case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
                    return RoomTables["cathedral"];
                case GlobalDungeonData.ValidTilesets.HELLGEON:
                    return RoomTables["bullethell"];
                default:
                    return RoomTables["gungeon"];
            }
        }

        public static T GetAsset<T>(string assetName) where T : UnityEngine.Object
        {
            T item = null;
            foreach (var bundle in AssetBundles.Values)
            {
                item = bundle.LoadAsset<T>(assetName);
                if (item != null)
                    break;
            }
            return item;
        }

    }
}
