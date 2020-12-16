using System;
using UnityEngine;

namespace FrostAndGunfireItems
{
    public class GameActorPetrifyEffect : GameActorEffect
    {
        private bool prev = true;
        public PlayerController Owner;

        public GameActorPetrifyEffect(PlayerController owner)
        {
           
            AffectsPlayers = false;           
            duration = 3.8f;
            AppliesTint = true;
            TintColor = new Color(0.2f, 0.2f, 0.2f, Mathf.Clamp01(duration));
            Owner = owner;
            
        }

        public bool ShouldVanishOnDeath(GameActor actor)
        {
            return (!actor.healthHaver || !actor.healthHaver.IsBoss) && (!(actor is AIActor) || !(actor as AIActor).IsSignatureEnemy);
        }
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
           
            if (!actor.healthHaver.IsBoss && !actor.healthHaver.IsDead)
            {
               
                AIActor aiactor = actor.aiActor;
                prev = aiactor.CanTargetPlayers;
                aiactor.CanTargetPlayers = false;
                aiactor.MovementSpeed = 0;
                base.OnEffectApplied(actor, effectData, partialAmount);
              
               
            }
        }

     
        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            AIActor aiactor = actor.aiActor;
            aiactor.MovementSpeed = aiactor.BaseMovementSpeed;
            aiactor.CanTargetPlayers = prev;
            base.OnEffectRemoved(actor, effectData);
        }
    }
}