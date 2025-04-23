using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GungeonAPI.Tools;

namespace FrostAndGunfireItems
{
    class SynergyForms
    {
        public static void AddSynergies()
        {
            //Glooper and Tentacle
            AdvancedTransformGunSynergyProcessor SynergyForme = (PickupObjectDatabase.GetByEncounterName("Glooper") as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
            SynergyForme.NonSynergyGunId = PickupObjectDatabase.GetByEncounterName("Glooper").PickupObjectId;
            SynergyForme.SynergyGunId = Gooper2.GooperSynergyID;
            SynergyForme.SynergyToCheck = "Kaliber's Spawn";
            //
        }
    }
}
