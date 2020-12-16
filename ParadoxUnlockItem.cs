using System;
using System.Collections;
using System.Collections.Generic;
using GungeonAPI;
using ItemAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000019 RID: 25
	public class ParadoxUnlockItem : PlayerItem
	{

		// Token: 0x060000A4 RID: 164 RVA: 0x00007860 File Offset: 0x00005A60
		public static void Init()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[8];
			var random = new System.Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);
			string name = "#ITEM_STRING_NOT_FOUND";
			string resourcePath = "FrostAndGunfireItems/Resources/undefined";
			GameObject gameObject = new GameObject();
			ParadoxUnlockItem item = gameObject.AddComponent<ParadoxUnlockItem>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = finalString;
			string longDesc = finalString;
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "kp");
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.PerRoom, 3f);
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.LICH_KILLED_AND_PARADOX, true);
			item.consumable = false;
			item.quality = PickupObject.ItemQuality.C;

		}


		protected override void DoEffect(PlayerController user)
		{
			int item = myList[UnityEngine.Random.Range(0, myList.Count + 1)];
			PickupObject byId = PickupObjectDatabase.GetById(item);
			PlayerItem playerItem = byId as PlayerItem;
			playerItem.Use(user, out float ImTooTiredToNameVariables);
			base.StartCoroutine(this.HandleDuration(user));
		}

		private IEnumerator HandleDuration(PlayerController user)
		{
			Material material = user.sprite.renderer.material;
			material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
			material.SetFloat("_GlitchInterval", 0.05f);
			material.SetFloat("_DispProbability", 0.4f);
			material.SetFloat("_DispIntensity", 0.04f);
			material.SetFloat("_ColorProbability", 0.4f);
			material.SetFloat("_ColorIntensity", 0.04f);
			yield return new WaitForSeconds(10f);
			user.ClearOverrideShader();
			yield break;
		}

		List<int> myList = new List<int>
		{
			291, //meatbun
			462, //smoke bomb
			108, //bomb
			308, //cluster mine
			366, //molotov
			155, //singularity
			432, //jar of bees
			201, //portable turret
			448, //bommerang
			644, //portable table device
			203, //cigarettes
			306, //escape rope
			411, //coolant
			499, //elder blank
			449, //teleporter
			66, //mine
			71, //decoy
			438, //explosive decoy
			820, //shadow clone
			168, //double vision
			205, //posion vial
			276, //SPICE
			320, //ticket



		};

	}
}
