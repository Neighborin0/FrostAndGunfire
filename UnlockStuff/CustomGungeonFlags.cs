using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrostAndGunfireItems
{
    public enum CustomDungeonFlags
    {
        NONE,
        //Wanderer Unlocks
        DRAGUN_KILLED_AND_WANDERER,//done
        LICH_KILLED_AND_WANDERER,//done
        RAT_KILLED_AND_WANDERER,//done
        CHALLENGE_MODE_AND_WANDERER,//done
        BOSS_RUSH_AND_WANDERER,//done
        BLESSED_AND_WANDERER,//done
        ADVANCED_AND_WANDERER, //needs item

        //
        //Rest of the cast
        LICH_KILLED_AND_PILOT, //done
        LICH_KILLED_AND_MARINE, //done
        LICH_KILLED_AND_HUNTER, // done
        LICH_KILLED_AND_ROBOT, //done
        LICH_KILLED_AND_PARADOX, //done
        LICH_KILLED_AND_BULLET, //need item
        LICH_KILLED_AND_CONVICT, //need item
        //
        //RANDOM STUFF
        ROOMIMIC_KILLED
        //BULLETKIN_KILLED_AND_WANDERER



    }
}
