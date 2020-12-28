using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using FrostAndGunfireItems;
using UnityEngine;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Collections;
using System.Collections.ObjectModel;

namespace GungeonAPI
{
    //Utility methods
    public static class Tools
    {
        public static bool verbose = true;
        private static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "FAGF.txt");
        public static string modID = "FAGF";

        private static Dictionary<string, float> timers = new Dictionary<string, float>();

        public static void Init()
        {
            if (File.Exists(defaultLog)) File.Delete(defaultLog);
        }

    
        public static void Print<T>(T obj, string color = "FFFFFF", bool force = false)
        {
            if (verbose || force)
            {
                string[] lines = obj.ToString().Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
            }

            Log(obj.ToString());
        }
        public static void PrintNoID<T>(T obj, string color = "FFFFFF", bool force = false)
        {
            if (verbose || force)
            {
                string[] lines = obj.ToString().Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}> {line}</color>");
            }

            Log(obj.ToString());
        }

        public static void PrintRaw<T>(T obj, bool force = false)
        {
            if (verbose || force)
                LogToConsole(obj.ToString());

            Log(obj.ToString());
        }

        public static void PrintError<T>(T obj, string color = "FF0000")
        {
            string[] lines = obj.ToString().Split('\n');
            foreach (var line in lines)
                LogToConsole($"<color=#{color}>[{modID}] {line}</color>");

            Log(obj.ToString());
        }

        public static void PrintException(Exception e, string color = "FF0000")
        {
            string message = e.Message + "\n" + e.StackTrace;
            {
                string[] lines = message.Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}>[{modID}] {line}</color>");
            }

            Log(e.Message);
            Log("\t" + e.StackTrace);
        }

        public static void Log<T>(T obj)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, defaultLog), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void Log<T>(T obj, string fileName)
        {
            if (!verbose) return;
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, fileName), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void LogToConsole(string message)
        {
            message.Replace("\t", "    ");
            ETGModConsole.Log(message);
        }

        private static void BreakdownComponentsInternal(this GameObject obj, int lvl = 0)
        {
            string space = "";
            for (int i = 0; i < lvl; i++)
            {
                space += "\t";
            }

            Log(space + obj.name + "...");
            foreach (var comp in obj.GetComponents<Component>())
            {
                Log(space + "    -" + comp.GetType());
            }

            foreach (var child in obj.GetComponentsInChildren<Transform>())
            {
                if (child != obj.transform)
                    child.gameObject.BreakdownComponentsInternal(lvl + 1);
            }
        }

        public static void BreakdownComponents(this GameObject obj)
        {
            BreakdownComponentsInternal(obj, 0);
        }

        public static void ExportTexture(Texture texture, string folder = "")
        {
            string path = Path.Combine(ETGMod.ResourcesDirectory, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllBytes(Path.Combine(path, texture.name + ".png"), ((Texture2D)texture).EncodeToPNG());
        }

        public static T GetEnumValue<T>(string val) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), val.ToUpper());
        }


        public static void LogPropertiesAndFields<T>(T obj, string header = "")
        {
            Log(header);
            Log("=======================");
            if (obj == null) { Log("LogPropertiesAndFields: Null object"); return; }
            Type type = obj.GetType();
            Log($"Type: {type}");
            PropertyInfo[] pinfos = type.GetProperties();
            Log($"{typeof(T)} Properties: ");
            foreach (var pinfo in pinfos)
            {
                try
                {
                    var value = pinfo.GetValue(obj, null);
                    string valueString = value.ToString();
                    bool isList = obj?.GetType().GetGenericTypeDefinition() == typeof(List<>);
                    if (isList)
                    {
                        var list = value as List<object>;
                        valueString = $"List[{list.Count}]";
                        foreach (var subval in list)
                        {
                            valueString += "\n\t\t" + subval.ToString();
                        }
                    }
                    Log($"\t{pinfo.Name}: {valueString}");
                }
                catch { }
            }
            Log($"{typeof(T)} Fields: ");
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                Log($"\t{finfo.Name}: {finfo.GetValue(obj)}");
            }
        }


        public static void DebugInformation(BehaviorSpeculator behavior, string path = "")
        {
            List<string> logs = new List<string>();

            logs.Add("Enemy report for enemy '" + behavior.aiActor.ActorName + "' with ID " + behavior.aiActor.EnemyGuid + ":");
            logs.Add("");

            logs.Add("--- Beginning behavior report");
            foreach (var b in behavior.AttackBehaviors)
            {
                if (b is AttackBehaviorGroup)
                {
                    logs.Add("Note: This actor has an AttackBehaviorGroup. The nicknames and probabilities are as follows:");
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(" - " + be.NickName + " | " + be.Probability);
                    }
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(ReturnPropertiesAndFields(be.Behavior, "Logging AttackBehaviorGroup behavior " + be.Behavior.GetType().Name + " with nickname " + be.NickName + " and probability " + be.Probability));
                    }
                }
                else
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging attack behavior " + b.GetType().Name));
                }
            }
            logs.Add("-----");
            foreach (var b in behavior.MovementBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging movement behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OtherBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging other behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OverrideBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging override behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.TargetBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging target behavior " + b.GetType().Name));
            }
            logs.Add("--- End of behavior report");
            logs.Add("");

            logs.Add("Components attached to the actor object are listed below.");
            foreach (var c in behavior.aiActor.gameObject.GetComponents(typeof(object)))
            {
                logs.Add(c.GetType().Name);
            }

            logs.Add("");
            if (behavior.bulletBank)
            {
                logs.Add("--- Beginning bullet bank report");

                foreach (var b in behavior.bulletBank.Bullets)
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging bullet " + b.Name));
                }
                logs.Add("--- End of bullet bank report");
            }
            else
            {
                logs.Add("--- Actor does not have a bullet bank.");
            }

            var retstr = string.Join("\n", logs.ToArray());
            if (string.IsNullOrEmpty(path))
            {
                ETGModConsole.Log(retstr);
            }
            else
            {
                File.WriteAllText(path, retstr);
            }
        }

        public static void DebugInformationNoAIActor(BehaviorSpeculator behavior, string path = "")
        {
            List<string> logs = new List<string>();

            logs.Add("Enemy report");
            logs.Add("");

            logs.Add("--- Beginning behavior report");
            foreach (var b in behavior.AttackBehaviors)
            {
                if (b is AttackBehaviorGroup)
                {
                    logs.Add("Note: This actor has an AttackBehaviorGroup. The nicknames and probabilities are as follows:");
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(" - " + be.NickName + " | " + be.Probability);
                    }
                    foreach (var be in (b as AttackBehaviorGroup).AttackBehaviors)
                    {
                        logs.Add(ReturnPropertiesAndFields(be.Behavior, "Logging AttackBehaviorGroup behavior " + be.Behavior.GetType().Name + " with nickname " + be.NickName + " and probability " + be.Probability));
                    }
                }
                else
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging attack behavior " + b.GetType().Name));
                }
            }
            logs.Add("-----");
            foreach (var b in behavior.MovementBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging movement behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OtherBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging other behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.OverrideBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging override behavior " + b.GetType().Name));
            }
            logs.Add("-----");
            foreach (var b in behavior.TargetBehaviors)
            {
                logs.Add(ReturnPropertiesAndFields(b, "Logging target behavior " + b.GetType().Name));
            }
            logs.Add("--- End of behavior report");
            logs.Add("");

            logs.Add("Components attached to the object are listed below.");
            foreach (var c in behavior.gameObject.GetComponents(typeof(object)))
            {
                logs.Add(c.GetType().Name);
            }

            logs.Add("");
            if (behavior.bulletBank)
            {
                logs.Add("--- Beginning bullet bank report");

                foreach (var b in behavior.bulletBank.Bullets)
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging bullet " + b.Name));
                }
                logs.Add("--- End of bullet bank report");
            }
            else
            {
                logs.Add("--- Actor does not have a bullet bank.");
            }

            var retstr = string.Join("\n", logs.ToArray());
            if (string.IsNullOrEmpty(path))
            {
                ETGModConsole.Log(retstr);
            }
            else
            {
                File.WriteAllText(path, retstr);
            }
        }

        public static void DebugBulletBank(AIBulletBank bank, string path = "")
        {
            List<string> logs = new List<string>();

            logs.Add("bullet bank report");
            logs.Add("");

            logs.Add("");
            if (bank)
            {
                logs.Add("--- Beginning bullet bank report");

                foreach (var b in bank.Bullets)
                {
                    logs.Add(ReturnPropertiesAndFields(b, "Logging bullet " + b.Name));
                }
                logs.Add("--- End of bullet bank report");
            }
            else
            {
                logs.Add("--- Actor does not have a bullet bank.");
            }

            var retstr = string.Join("\n", logs.ToArray());
            if (string.IsNullOrEmpty(path))
            {
                ETGModConsole.Log(retstr);
            }
            else
            {
                File.WriteAllText(path, retstr);
            }
        }

        public static void SetupUnlockOnStat(this PickupObject self, TrackedStats stat, DungeonPrerequisite.PrerequisiteOperation operation, int comparisonValue)
        {
            EncounterTrackable encounterTrackable = self.encounterTrackable;
            if (encounterTrackable.prerequisites == null)
            {
                encounterTrackable.prerequisites = new DungeonPrerequisite[1];
                encounterTrackable.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                    prerequisiteOperation = operation,
                    statToCheck = stat,
                    comparisonValue = comparisonValue
                };
            }
            else
            {
                encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[] { new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                    prerequisiteOperation = operation,
                    statToCheck = stat,
                    comparisonValue = comparisonValue
                }}).ToArray();
            }
            EncounterDatabaseEntry databaseEntry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
            if (!string.IsNullOrEmpty(databaseEntry.ProxyEncounterGuid))
            {
                databaseEntry.ProxyEncounterGuid = "";
            }
            if (databaseEntry.prerequisites == null)
            {
                databaseEntry.prerequisites = new DungeonPrerequisite[1];
                databaseEntry.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                    prerequisiteOperation = operation,
                    statToCheck = stat,
                    comparisonValue = comparisonValue
                };
            }
            else
            {
                databaseEntry.prerequisites = databaseEntry.prerequisites.Concat(new DungeonPrerequisite[] { new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                    prerequisiteOperation = operation,
                    statToCheck = stat,
                    comparisonValue = comparisonValue
                }}).ToArray();
            }
        }
        public static void SetupUnlockOnFlag(this PickupObject self, GungeonFlags flag, bool requiredFlagValue)
        {
            EncounterTrackable encounterTrackable = self.encounterTrackable;
            if (encounterTrackable.prerequisites == null)
            {
                encounterTrackable.prerequisites = new DungeonPrerequisite[1];
                encounterTrackable.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = requiredFlagValue,
                    saveFlagToCheck = flag
                };
            }
            else
            {
                encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[] { new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = requiredFlagValue,
                    saveFlagToCheck = flag
                }}).ToArray();
            }
            EncounterDatabaseEntry databaseEntry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
            if (!string.IsNullOrEmpty(databaseEntry.ProxyEncounterGuid))
            {
                databaseEntry.ProxyEncounterGuid = "";
            }
            if (databaseEntry.prerequisites == null)
            {
                databaseEntry.prerequisites = new DungeonPrerequisite[1];
                databaseEntry.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = requiredFlagValue,
                    saveFlagToCheck = flag
                };
            }
            else
            {
                databaseEntry.prerequisites = databaseEntry.prerequisites.Concat(new DungeonPrerequisite[] { new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = requiredFlagValue,
                    saveFlagToCheck = flag
                }}).ToArray();
            }
        }
        public static void SetupUnlockOnEncounter(this PickupObject self, string guid, DungeonPrerequisite.PrerequisiteOperation operation, int comparisonValue)
        {
            EncounterTrackable encounterTrackable = self.encounterTrackable;
            if (encounterTrackable.prerequisites == null)
            {
                encounterTrackable.prerequisites = new DungeonPrerequisite[1];
                encounterTrackable.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.ENCOUNTER,
                    prerequisiteOperation = operation,
                    encounteredObjectGuid = guid,
                    requiredNumberOfEncounters = comparisonValue
                };
            }
            else
            {
                encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[] { new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.ENCOUNTER,
                    prerequisiteOperation = operation,
                    encounteredObjectGuid = guid,
                    requiredNumberOfEncounters = comparisonValue
                }}).ToArray();
            }
            EncounterDatabaseEntry databaseEntry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
            if (!string.IsNullOrEmpty(databaseEntry.ProxyEncounterGuid))
            {
                databaseEntry.ProxyEncounterGuid = "";
            }
            if (databaseEntry.prerequisites == null)
            {
                databaseEntry.prerequisites = new DungeonPrerequisite[1];
                databaseEntry.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.ENCOUNTER,
                    prerequisiteOperation = operation,
                    encounteredObjectGuid = guid,
                    requiredNumberOfEncounters = comparisonValue
                };
            }
            else
            {
                databaseEntry.prerequisites = databaseEntry.prerequisites.Concat(new DungeonPrerequisite[] { new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.ENCOUNTER,
                    prerequisiteOperation = operation,
                    encounteredObjectGuid = guid,
                    requiredNumberOfEncounters = comparisonValue
                }}).ToArray();
            }
        }

       
        public static void SetupUnlockOnCustomFlag(this PickupObject self, CustomDungeonFlags flag, bool requiredFlagValue)
        {
            EncounterTrackable encounterTrackable = self.encounterTrackable;
            if (encounterTrackable.prerequisites == null)
            {
                encounterTrackable.prerequisites = new DungeonPrerequisite[1];
                encounterTrackable.prerequisites[0] = new AdvancedDungeonPrerequisite
                {
                    advancedPrerequisiteType = AdvancedDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                    requireCustomFlag = requiredFlagValue,
                    customFlagToCheck = flag
                };
            }
            else
            {
                encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[] { new AdvancedDungeonPrerequisite
                {
                    advancedPrerequisiteType = AdvancedDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                    requireCustomFlag = requiredFlagValue,
                    customFlagToCheck = flag
                }}).ToArray();
            }
            EncounterDatabaseEntry databaseEntry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
            if (!string.IsNullOrEmpty(databaseEntry.ProxyEncounterGuid))
            {
                databaseEntry.ProxyEncounterGuid = "";
            }
            if (databaseEntry.prerequisites == null)
            {
                databaseEntry.prerequisites = new DungeonPrerequisite[1];
                databaseEntry.prerequisites[0] = new AdvancedDungeonPrerequisite
                {
                    advancedPrerequisiteType = AdvancedDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                    requireCustomFlag = requiredFlagValue,
                    customFlagToCheck = flag
                };
            }
            else
            {
                databaseEntry.prerequisites = databaseEntry.prerequisites.Concat(new DungeonPrerequisite[] { new AdvancedDungeonPrerequisite
                {
                    advancedPrerequisiteType = AdvancedDungeonPrerequisite.AdvancedPrerequisiteType.CUSTOM_FLAG,
                    requireCustomFlag = requiredFlagValue,
                    customFlagToCheck = flag
                }}).ToArray();
            }
        }

        public static string PathCombine(string a, string b, string c)
        {
            return Path.Combine(Path.Combine(a, b), c);
        }

        public static void SafeMove(string oldPath, string newPath, bool allowOverwritting = false)
        {
            if (File.Exists(oldPath) && (allowOverwritting || !File.Exists(newPath)))
            {
                string contents = SaveManager.ReadAllText(oldPath);
                try
                {
                    SaveManager.WriteAllText(newPath, contents);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("Failed to move {0} to {1}: {2}", new object[]
                    {
                    oldPath,
                    newPath,
                    ex
                    });
                    return;
                }
                try
                {
                    File.Delete(oldPath);
                }
                catch (Exception ex2)
                {
                    Debug.LogErrorFormat("Failed to delete old file {0}: {1}", new object[]
                    {
                    oldPath,
                    newPath,
                    ex2
                    });
                    return;
                }
                if (File.Exists(oldPath + ".bak"))
                {
                    File.Delete(oldPath + ".bak");
                }
            }
        }

        public static string ReturnPropertiesAndFields<T>(T obj, string header = "")
        {
            string ret = "";
            ret += "\r\n" + (header);
            ret += "\r\n" + ("=======================");
            if (obj == null) { ret += "\r\n" + ("LogPropertiesAndFields: Null object"); return ret; }
            Type type = obj.GetType();
            ret += "\r\n" + ($"{typeof(T)} Fields: ");
            FieldInfo[] finfos = type.GetFields();
            foreach (var finfo in finfos)
            {
                try
                {
                    var value = finfo.GetValue(obj);
                    string valueString = value.ToString();
                    bool isArray = value.GetType().IsArray == true;
                    if (isArray)
                    {
                        var ar = (value as IEnumerable);
                        valueString = $"Array[]";
                        foreach (var subval in ar)
                        {
                            valueString += "\r\n\t\t" + subval.ToString();
                        }
                    }
                    else if (value is BulletScriptSelector)
                    {
                        valueString = (value as BulletScriptSelector).scriptTypeName;
                    }
                    ret += "\r\n" + ($"\t{finfo.Name}: {valueString}");
                }
                catch
                {
                    ret += "\r\n" + ($"\t{finfo.Name}: {finfo.GetValue(obj)}");
                }
            }

            return ret;
        }
    }
}

    

