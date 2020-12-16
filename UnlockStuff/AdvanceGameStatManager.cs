using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.RuntimeDetour;
using GungeonAPI;

namespace FrostAndGunfireItems
{

        class AdvancedGameStatsManager
        {
            public AdvancedGameStatsManager()
            {
                this.m_flags = new HashSet<CustomDungeonFlags>(new CustomDungeonFlagsComparer());
            }

            public static void Init()
            {
                Load();
                Hook saveHook = new Hook(
                    typeof(GameStatsManager).GetMethod("Save", BindingFlags.Public | BindingFlags.Static),
                    typeof(AdvancedGameStatsManager).GetMethod("SaveHook")
                );
                Hook loadHook = new Hook(
                    typeof(GameStatsManager).GetMethod("Load", BindingFlags.Public | BindingFlags.Static),
                    typeof(AdvancedGameStatsManager).GetMethod("LoadHook")
                );
                Hook resetHook = new Hook(
                    typeof(GameStatsManager).GetMethod("DANGEROUS_ResetAllStats", BindingFlags.Public | BindingFlags.Static),
                    typeof(AdvancedGameStatsManager).GetMethod("ResetHook")
                );
                Hook prerequisiteHook = new Hook(
                    typeof(DungeonPrerequisite).GetMethod("CheckConditionsFulfilled", BindingFlags.Public | BindingFlags.Instance),
                    typeof(AdvancedGameStatsManager).GetMethod("PrerequisiteHook")
                );
            }

            public static void Load()
            {
                SaveManager.Init();
                if (!SaveManager.Load<AdvancedGameStatsManager>(AdvancedGameSave, out AdvancedGameStatsManager.m_instance, true, 0u, null, null))
                {
                    AdvancedGameStatsManager.m_instance = new AdvancedGameStatsManager();
                }
            }

            public static void ResetHook(Action orig)
            {
                DANGEROUS_ResetAllStats();
                orig();
            }

            public static bool SaveHook(Func<bool> orig)
            {
                bool result = orig();
                Save();
                return result;
            }

            public static void LoadHook(Action orig)
            {
                orig();
                Load();
            }

 
        public static void DANGEROUS_ResetAllStats()
            {
                AdvancedGameStatsManager.m_instance = new AdvancedGameStatsManager();
                SaveManager.DeleteAllBackups(AdvancedGameStatsManager.AdvancedGameSave, null);
            }

            public static bool PrerequisiteHook(Func<DungeonPrerequisite, bool> orig, DungeonPrerequisite self)
            {
                if (self is AdvancedDungeonPrerequisite)
                {
                    return (self as AdvancedDungeonPrerequisite).CheckConditionsFulfilled();
                }
                return orig(self);
            }

            public bool GetFlag(CustomDungeonFlags flag)
            {
                if (flag == CustomDungeonFlags.NONE)
                {
                    Debug.LogError("Something is attempting to get a NONE save flag!");
                    return false;
                }
                return this.m_flags.Contains(flag);
            }

            public void SetFlag(CustomDungeonFlags flag, bool value)
            {
                if (flag == CustomDungeonFlags.NONE)
                {
                    Debug.LogError("Something is attempting to set a NONE save flag!");
                    return;
                }
                if (value)
                {
                    this.m_flags.Add(flag);
                }
                else
                {
                    this.m_flags.Remove(flag);
                }
            }

            public static bool Save()
            {
                bool result = false;
                try
                {
                    result = SaveManager.Save<AdvancedGameStatsManager>(AdvancedGameStatsManager.m_instance, AdvancedGameSave, GameStatsManager.Instance.PlaytimeMin, 0u, null);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("SAVE FAILED: {0}", new object[]
                    {
                ex
                    });
                }
                return result;
            }

            public static AdvancedGameStatsManager Instance
            {
                get
                {
                    return m_instance;
                }
            }

            private static AdvancedGameStatsManager m_instance;
            public HashSet<CustomDungeonFlags> m_flags;
            public Dictionary<PlayableCharacters, AdvancedGameStats> m_characterStats;
            public static SaveManager.SaveType AdvancedGameSave;
            private AdvancedGameStats m_sessionStats;
            private AdvancedGameStats m_savedSessionStats;
        }
    }


