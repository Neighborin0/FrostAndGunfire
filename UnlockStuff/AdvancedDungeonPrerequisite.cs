using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FrostAndGunfireItems
{
    class AdvancedDungeonPrerequisite : DungeonPrerequisite
    {
        public new bool CheckConditionsFulfilled()
        {
            if (this.advancedPrerequisiteType != AdvancedPrerequisiteType.NONE)
            {
                if (this.advancedPrerequisiteType == AdvancedPrerequisiteType.CUSTOM_FLAG)
                {
                    return AdvancedGameStatsManager.Instance.GetFlag(this.customFlagToCheck) == this.requireCustomFlag;
                }
                else if (this.advancedPrerequisiteType == AdvancedPrerequisiteType.CUSTOM_STAT)
                {
                    return false;
                }
            }
            else
            {
                return this.CheckConditionsFulfilledOrig();
            }
            return false;
        }

        public bool CheckConditionsFulfilledOrig()
        {
            EncounterDatabaseEntry encounterDatabaseEntry = null;
            if (!string.IsNullOrEmpty(this.encounteredObjectGuid))
            {
                encounterDatabaseEntry = EncounterDatabase.GetEntry(this.encounteredObjectGuid);
            }
            switch (this.prerequisiteType)
            {
                case DungeonPrerequisite.PrerequisiteType.ENCOUNTER:
                    if (encounterDatabaseEntry == null && this.encounteredRoom == null)
                    {
                        return true;
                    }
                    if (encounterDatabaseEntry != null)
                    {
                        int num = GameStatsManager.Instance.QueryEncounterable(encounterDatabaseEntry);
                        switch (this.prerequisiteOperation)
                        {
                            case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
                                return num < this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
                                return num == this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
                                return num > this.requiredNumberOfEncounters;
                            default:
                                Debug.LogError("Switching on invalid stat comparison operation!");
                                break;
                        }
                    }
                    else if (this.encounteredRoom != null)
                    {
                        int num2 = GameStatsManager.Instance.QueryRoomEncountered(this.encounteredRoom.GUID);
                        switch (this.prerequisiteOperation)
                        {
                            case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
                                return num2 < this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
                                return num2 == this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
                                return num2 > this.requiredNumberOfEncounters;
                            default:
                                Debug.LogError("Switching on invalid stat comparison operation!");
                                break;
                        }
                    }
                    return false;
                case DungeonPrerequisite.PrerequisiteType.COMPARISON:
                    {
                        float playerStatValue = GameStatsManager.Instance.GetPlayerStatValue(this.statToCheck);
                        switch (this.prerequisiteOperation)
                        {
                            case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
                                return playerStatValue < this.comparisonValue;
                            case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
                                return playerStatValue == this.comparisonValue;
                            case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
                                return playerStatValue > this.comparisonValue;
                            default:
                                Debug.LogError("Switching on invalid stat comparison operation!");
                                break;
                        }
                        break;
                    }
                case DungeonPrerequisite.PrerequisiteType.CHARACTER:
                    {
                        PlayableCharacters playableCharacters = (PlayableCharacters)(-1);
                        if (!BraveRandom.IgnoreGenerationDifferentiator)
                        {
                            if (GameManager.Instance.PrimaryPlayer != null)
                            {
                                playableCharacters = GameManager.Instance.PrimaryPlayer.characterIdentity;
                            }
                            else if (GameManager.PlayerPrefabForNewGame != null)
                            {
                                playableCharacters = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>().characterIdentity;
                            }
                            else if (GameManager.Instance.BestGenerationDungeonPrefab != null)
                            {
                                playableCharacters = GameManager.Instance.BestGenerationDungeonPrefab.defaultPlayerPrefab.GetComponent<PlayerController>().characterIdentity;
                            }
                        }
                        return this.requireCharacter == (playableCharacters == this.requiredCharacter);
                    }
                case DungeonPrerequisite.PrerequisiteType.TILESET:
                    if (GameManager.Instance.BestGenerationDungeonPrefab != null)
                    {
                        return this.requireTileset == (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == this.requiredTileset);
                    }
                    return this.requireTileset == (GameManager.Instance.Dungeon.tileIndices.tilesetId == this.requiredTileset);
                case DungeonPrerequisite.PrerequisiteType.FLAG:
                    return GameStatsManager.Instance.GetFlag(this.saveFlagToCheck) == this.requireFlag;
                case DungeonPrerequisite.PrerequisiteType.DEMO_MODE:
                    return !this.requireDemoMode;
                case DungeonPrerequisite.PrerequisiteType.MAXIMUM_COMPARISON:
                    {
                        float playerMaximum = GameStatsManager.Instance.GetPlayerMaximum(this.maxToCheck);
                        switch (this.prerequisiteOperation)
                        {
                            case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
                                return playerMaximum < this.comparisonValue;
                            case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
                                return playerMaximum == this.comparisonValue;
                            case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
                                return playerMaximum > this.comparisonValue;
                            default:
                                Debug.LogError("Switching on invalid stat comparison operation!");
                                break;
                        }
                        break;
                    }
                case DungeonPrerequisite.PrerequisiteType.ENCOUNTER_OR_FLAG:
                    if (GameStatsManager.Instance.GetFlag(this.saveFlagToCheck) == this.requireFlag)
                    {
                        return true;
                    }
                    if (encounterDatabaseEntry != null)
                    {
                        int num3 = GameStatsManager.Instance.QueryEncounterable(encounterDatabaseEntry);
                        switch (this.prerequisiteOperation)
                        {
                            case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
                                return num3 < this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
                                return num3 == this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
                                return num3 > this.requiredNumberOfEncounters;
                            default:
                                Debug.LogError("Switching on invalid stat comparison operation!");
                                break;
                        }
                    }
                    else if (this.encounteredRoom != null)
                    {
                        int num4 = GameStatsManager.Instance.QueryRoomEncountered(this.encounteredRoom.GUID);
                        switch (this.prerequisiteOperation)
                        {
                            case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
                                return num4 < this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
                                return num4 == this.requiredNumberOfEncounters;
                            case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
                                return num4 > this.requiredNumberOfEncounters;
                            default:
                                Debug.LogError("Switching on invalid stat comparison operation!");
                                break;
                        }
                    }
                    return false;
                case DungeonPrerequisite.PrerequisiteType.NUMBER_PASTS_COMPLETED:
                    return (float)GameStatsManager.Instance.GetNumberPastsBeaten() >= this.comparisonValue;
                default:
                    Debug.LogError("Switching on invalid prerequisite type!!!");
                    break;
            }
            return false;
        }

        public AdvancedPrerequisiteType advancedPrerequisiteType;
        public CustomDungeonFlags customFlagToCheck;
        public bool requireCustomFlag;

        public enum AdvancedPrerequisiteType
        {
            NONE,
            CUSTOM_FLAG,
            CUSTOM_STAT
        }
    }
}

