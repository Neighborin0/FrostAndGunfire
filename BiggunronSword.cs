using System;
using System.Collections;
using Dungeonator;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{

	public class BiggunronSword : GunBehaviour
	{
		private bool HasReloaded;
		//Fix gun stat ups spreading to other guns
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Biggunron Sword", "big_sword");
			Game.Items.Rename("outdated_gun_mods:biggunron_sword", "kp:biggunron_sword");
			gun.gameObject.AddComponent<BiggunronSword>();
			gun.SetShortDescription("A Big Point");
			gun.SetLongDescription("A huge sword was crafted by the ancient Gunron race, the Gunrons forged this weapon to take down the evil tyrant, Cannon. Unfortunately, this weapon was unable to harm the grand king so they just tossed it into a chest.");
			gun.SetupSprite(null, "big_sword_idle_001", 3);
			gun.SetAnimationFPS(gun.shootAnimation, 10);
			gun.SetAnimationFPS(gun.reloadAnimation, 6);
			gun.AddProjectileModuleFrom("blasphemy", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = 0.4f;
			gun.DefaultModule.numberOfShotsInClip = gun.GetBaseMaxAmmo();
			gun.quality = PickupObject.ItemQuality.A;
			gun.gunHandedness = GunHandedness.OneHanded;
			gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(574) as Gun).muzzleFlashEffects;
			gun.SetBaseMaxAmmo(250);
			Guid.NewGuid().ToString();
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage *= 3f;
			projectile.AppliesKnockbackToPlayer = true;
			projectile.PlayerKnockbackForce = -25f;
			projectile.baseData.speed *= 1f;
			projectile.AdditionalScaleMultiplier *= 10f;
			projectile.baseData.range = 1.5f;
			gun.barrelOffset.transform.localPosition = new Vector3(projectile.baseData.range, 0.3f, 0f);
			projectile.sprite.renderer.enabled = false;
			projectile.specRigidbody.CollideWithTileMap = false;
			projectile.hitEffects.suppressMidairDeathVfx = true;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			ItemBuilder.AddPassiveStatModifier(gun, PlayerStats.StatType.Curse, 2.5f, StatModifier.ModifyMethod.ADDITIVE);
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_BULLET, true);
			gun.AddToSubShop(ItemBuilder.ShopType.Cursula);
			gun.AddToSubShop(ItemBuilder.ShopType.OldRed);
		}



		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
		}


	}
}
