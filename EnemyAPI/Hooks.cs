using FrostAndGunfireItems;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ItemAPI
{
    public static class Hooks
    {
        

        public static void Init()
        {
            Hook customEnemyChangesHook = new Hook(
                typeof(AIActor).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public),
                typeof(Hooks).GetMethod("HandleCustomEnemyChanges")
            );
        }

        public static void HandleCustomEnemyChanges(Action<AIActor> orig, AIActor self)
        {
            orig(self);
            try
            {
                if (!Champions.AllChampions)
                {
                    float value = UnityEngine.Random.value;
                    if ((double)value < 0.05)
                    {
                        var obehaviors = EnemyTools.overrideBehaviors.Where(ob => ob.OverrideAIActorGUID == self.EnemyGuid);
                        foreach (var obehavior in obehaviors)
                        {
                            obehavior.SetupOB(self);
                            if (obehavior.ShouldOverride())
                            {
                                obehavior.DoOverride();
                            }
                        }
                    }
                }
                else
                {
                    var obehaviors = EnemyTools.overrideBehaviors.Where(ob => ob.OverrideAIActorGUID == self.EnemyGuid);
                    foreach (var obehavior in obehaviors)
                    {
                        obehavior.SetupOB(self);
                        if (obehavior.ShouldOverride())
                        {
                            obehavior.DoOverride();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ETGModConsole.Log(e.ToString());
            }
        }
    }
}
