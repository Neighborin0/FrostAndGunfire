using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace FrostAndGunfireItems
{
    class AdvancedGunBehaviour : MonoBehaviour, IGunInheritable
    {
        protected virtual void Update()
        {
            if (this.Player != null)
            {
                if (!this.everPickedUpByPlayer)
                {
                    this.everPickedUpByPlayer = true;
                }
            }
            if (this.Owner != null)
            {
                if (!this.everPickedUp)
                {
                    this.everPickedUp = true;
                }
            }
            if (this.lastOwner != this.Owner)
            {
                this.lastOwner = this.Owner;
            }
            if (this.Owner != null && !this.pickedUpLast)
            {
                this.OnPickup(this.Owner);
                this.pickedUpLast = true;
            }
            if (this.Owner == null && this.pickedUpLast)
            {
                if (this.lastOwner != null)
                {
                    this.OnPostDrop(this.lastOwner);
                    this.lastOwner = null;
                }
                this.pickedUpLast = false;
            }
            if (this.gun != null && !this.gun.IsReloading && !this.hasReloaded)
            {
                this.hasReloaded = true;
            }
            this.gun.PreventNormalFireAudio = this.preventNormalFireAudio;
            this.gun.OverrideNormalFireAudioEvent = this.overrideNormalFireAudio;
        }

        public virtual void InheritData(Gun source)
        {
            AdvancedGunBehaviour component = source.GetComponent<AdvancedGunBehaviour>();
            if (component != null)
            {
                this.preventNormalFireAudio = component.preventNormalFireAudio;
                this.preventNormalReloadAudio = component.preventNormalReloadAudio;
                this.overrideNormalReloadAudio = component.overrideNormalReloadAudio;
                this.overrideNormalFireAudio = component.overrideNormalFireAudio;
                this.everPickedUpByPlayer = component.everPickedUpByPlayer;
                this.everPickedUp = component.everPickedUp;
            }
        }

        public virtual void MidGameSerialize(List<object> data, int dataIndex)
        {
            data.Add(this.preventNormalFireAudio);
            data.Add(this.preventNormalReloadAudio);
            data.Add(this.overrideNormalReloadAudio);
            data.Add(this.overrideNormalFireAudio);
            data.Add(this.everPickedUpByPlayer);
            data.Add(this.everPickedUp);
        }

        public virtual void MidGameDeserialize(List<object> data, ref int dataIndex)
        {
            this.preventNormalFireAudio = (bool)data[dataIndex];
            this.preventNormalReloadAudio = (bool)data[dataIndex + 1];
            this.overrideNormalReloadAudio = (string)data[dataIndex + 2];
            this.overrideNormalFireAudio = (string)data[dataIndex + 3];
            this.everPickedUpByPlayer = (bool)data[dataIndex + 4];
            this.everPickedUp = (bool)data[dataIndex + 5];
            dataIndex += 6;
        }

        public virtual void Start()
        {
            this.gun = base.GetComponent<Gun>();
            this.gun.OnInitializedWithOwner += this.OnInitializedWithOwner;
            if (this.gun.CurrentOwner != null)
            {
                this.OnInitializedWithOwner(this.gun.CurrentOwner);
            }
            this.gun.PostProcessProjectile += this.PostProcessProjectile;
            this.gun.PostProcessVolley += this.PostProcessVolley;
            this.gun.OnDropped += this.OnDropped;
            this.gun.OnAutoReload += this.OnAutoReload;
            this.gun.OnReloadPressed += this.OnReloadPressed;
            this.gun.OnFinishAttack += this.OnFinishAttack;
            this.gun.OnPostFired += this.OnPostFired;
            this.gun.OnAmmoChanged += this.OnAmmoChanged;
            this.gun.OnBurstContinued += this.OnBurstContinued;
            this.gun.OnPreFireProjectileModifier += this.OnPreFireProjectileModifier;
        }

        public virtual void OnInitializedWithOwner(GameActor actor)
        {
        }

        public virtual void PostProcessProjectile(Projectile projectile)
        {
        }

        public virtual void PostProcessVolley(ProjectileVolleyData volley)
        {
        }

        public virtual void OnDropped()
        {
        }

        public virtual void OnAutoReload(PlayerController player, Gun gun)
        {
            if (player != null)
            {
                this.OnAutoReloadSafe(player, gun);
            }
        }

        public virtual void OnAutoReloadSafe(PlayerController player, Gun gun)
        {
        }

        public virtual void OnReloadPressed(PlayerController player, Gun gun, bool manualReload)
        {
            if (this.hasReloaded && gun.IsReloading)
            {
                this.OnReload(player, gun);
                this.hasReloaded = false;
            }
            if (player != null)
            {
                this.OnReloadPressedSafe(player, gun, manualReload);
            }
        }

        public virtual void OnReloadPressedSafe(PlayerController player, Gun gun, bool manualReload)
        {
            if (this.hasReloaded && gun.IsReloading)
            {
                this.OnReloadSafe(player, gun);
                this.hasReloaded = false;
            }
        }

        public virtual void OnReload(PlayerController player, Gun gun)
        {
            if (this.preventNormalReloadAudio)
            {
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                if (!string.IsNullOrEmpty(this.overrideNormalReloadAudio))
                {
                    AkSoundEngine.PostEvent(this.overrideNormalReloadAudio, base.gameObject);
                }
            }
        }

        public virtual void OnReloadSafe(PlayerController player, Gun gun)
        {
            if (this.preventNormalReloadAudio)
            {
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                if (!string.IsNullOrEmpty(this.overrideNormalReloadAudio))
                {
                    AkSoundEngine.PostEvent(this.overrideNormalReloadAudio, base.gameObject);
                }
            }
        }

        public virtual void OnFinishAttack(PlayerController player, Gun gun)
        {
        }

        public virtual void OnPostFired(PlayerController player, Gun gun)
        {
            if (gun.IsHeroSword)
            {
                if ((float)heroSwordCooldown.GetValue(gun) == 0.5f)
                {
                    this.OnHeroSwordCooldownStarted(player, gun);
                }
            }
        }

        public virtual void OnHeroSwordCooldownStarted(PlayerController player, Gun gun)
        {
        }

        public virtual void OnAmmoChanged(PlayerController player, Gun gun)
        {
            if (player != null)
            {
                this.OnAmmoChangedSafe(player, gun);
            }
        }

        public virtual void OnAmmoChangedSafe(PlayerController player, Gun gun)
        {
        }

        public virtual void OnBurstContinued(PlayerController player, Gun gun)
        {
            if (player != null)
            {
                this.OnBurstContinuedSafe(player, gun);
            }
        }

        public virtual void OnBurstContinuedSafe(PlayerController player, Gun gun)
        {
        }

        public virtual Projectile OnPreFireProjectileModifier(Gun gun, Projectile projectile, ProjectileModule mod)
        {
            return projectile;
        }

        public AdvancedGunBehaviour()
        {
        }

        protected virtual void OnPickup(GameActor owner)
        {
            if (owner is PlayerController)
            {
                this.OnPickedUpByPlayer(owner as PlayerController);
            }
            if (owner is AIActor)
            {
                this.OnPickedUpByEnemy(owner as AIActor);
            }
        }

        protected virtual void OnPostDrop(GameActor owner)
        {
            if (owner is PlayerController)
            {
                this.OnPostDroppedByPlayer(owner as PlayerController);
            }
            if (owner is AIActor)
            {
                this.OnPostDroppedByEnemy(owner as AIActor);
            }
        }

        protected virtual void OnPickedUpByPlayer(PlayerController player)
        {
        }

        protected virtual void OnPostDroppedByPlayer(PlayerController player)
        {
        }

        protected virtual void OnPickedUpByEnemy(AIActor enemy)
        {
        }

        protected virtual void OnPostDroppedByEnemy(AIActor enemy)
        {
        }

        public bool PickedUp
        {
            get
            {
                return this.gun.CurrentOwner != null;
            }
        }

        public PlayerController Player
        {
            get
            {
                if (this.gun.CurrentOwner is PlayerController)
                {
                    return this.gun.CurrentOwner as PlayerController;
                }
                return null;
            }
        }

        public float HeroSwordCooldown
        {
            get
            {
                if (this.gun != null)
                {
                    return (float)heroSwordCooldown.GetValue(this.gun);
                }
                return -1f;
            }
            set
            {
                if (this.gun != null)
                {
                    heroSwordCooldown.SetValue(this.gun, value);
                }
            }
        }

        public GameActor Owner
        {
            get
            {
                return this.gun.CurrentOwner;
            }
        }

        public bool PickedUpByPlayer
        {
            get
            {
                return this.Player != null;
            }
        }

        private bool pickedUpLast = false;
        private GameActor lastOwner = null;
        public bool everPickedUpByPlayer = false;
        public bool everPickedUp = false;
        protected Gun gun;
        private bool hasReloaded = true;
        public bool preventNormalFireAudio;
        public bool preventNormalReloadAudio;
        public string overrideNormalFireAudio;
        public string overrideNormalReloadAudio;
        private static FieldInfo heroSwordCooldown = typeof(Gun).GetField("HeroSwordCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}