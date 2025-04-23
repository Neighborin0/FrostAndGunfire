using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrostAndGunfireItems
{
    class DualWields
    {
        public static void AddDualWields()
        {
            //Glooper and Tentacle
            AdvancedDualWieldSynergyProcessor synergyProcessor = (PickupObjectDatabase.GetByEncounterName("Glooper") as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            synergyProcessor.PartnerGunID = 474;
            synergyProcessor.SynergyNameToCheck = "Abyssal Depths";
            //
            AdvancedDualWieldSynergyProcessor synergyProcessor2 = (PickupObjectDatabase.GetById(474) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
            synergyProcessor2.PartnerGunID = PickupObjectDatabase.GetByEncounterName("Glooper").PickupObjectId;
            synergyProcessor2.SynergyNameToCheck = "Abyssal Depths";
            //
        }
    }
}
