using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GungeonAPI;

namespace FrostAndGunfireItems
{
    class AdvancedGameStats
    {
        public AdvancedGameStats()
        {
          
            this.stats = new Dictionary<CustomTrackedStats, float>(new CustomTrackedStatsComparer());
            this.maxima = new Dictionary<CustomTrackedMaximums, float>(new CustomTrackedMaximumsComparer());
        }

        public float GetStatValue(CustomTrackedStats statToCheck)
        {
            if (!this.stats.ContainsKey(statToCheck))
            {
                return 0f;
            }
            return this.stats[statToCheck];
        }

        public float GetMaximumValue(CustomTrackedMaximums maxToCheck)
        {
            if (!this.maxima.ContainsKey(maxToCheck))
            {
                return 0f;
            }
            return this.maxima[maxToCheck];
        }

    
        public void SetStat(CustomTrackedStats stat, float val)
        {
            if (this.stats.ContainsKey(stat))
            {
                this.stats[stat] = val;
            }
            else
            {
                this.stats.Add(stat, val);
            }
        }

        public void SetMax(CustomTrackedMaximums max, float val)
        {
            if (this.maxima.ContainsKey(max))
            {
                this.maxima[max] = Mathf.Max(this.maxima[max], val);
            }
            else
            {
                this.maxima.Add(max, val);
            }
        }

      
        public void IncrementStat(CustomTrackedStats stat, float val)
        {
            if (this.stats.ContainsKey(stat))
            {
                this.stats[stat] = this.stats[stat] + val;
            }
            else
            {
                this.stats.Add(stat, val);
            }
        }

        public void AddStats(AdvancedGameStats otherStats)
        {
            foreach (KeyValuePair<CustomTrackedStats, float> keyValuePair in otherStats.stats)
            {
                this.IncrementStat(keyValuePair.Key, keyValuePair.Value);
            }
            foreach (KeyValuePair<CustomTrackedMaximums, float> keyValuePair2 in otherStats.maxima)
            {
                this.SetMax(keyValuePair2.Key, keyValuePair2.Value);
            }
       
        }

        public void ClearAllState()
        {
            List<CustomTrackedStats> list = new List<CustomTrackedStats>();
            foreach (KeyValuePair<CustomTrackedStats, float> keyValuePair in this.stats)
            {
                list.Add(keyValuePair.Key);
            }
            foreach (CustomTrackedStats key in list)
            {
                this.stats[key] = 0f;
            }
            List<CustomTrackedMaximums> list2 = new List<CustomTrackedMaximums>();
            foreach (KeyValuePair<CustomTrackedMaximums, float> keyValuePair2 in this.maxima)
            {
                list2.Add(keyValuePair2.Key);
            }
            foreach (CustomTrackedMaximums key2 in list2)
            {
                this.maxima[key2] = 0f;
            }
        }

        private Dictionary<CustomTrackedStats, float> stats;
        private Dictionary<CustomTrackedMaximums, float> maxima;
    
    }
}