using System;
using System.Collections;
using System.Collections.Generic;
using GungeonAPI;
using UnityEngine;

namespace FrostAndGunfireItems
{
	// Token: 0x02000090 RID: 144
	public class TableNPCInteractable : SimpleInteractable, IPlayerInteractable
	{
		public bool m_allowMeToIntroduceMyself = true;
		private void Start()
		{

			this.talkPoint = base.transform.Find("talkpoint");
			this.m_isToggled = false;
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			this.m_canUse = true;
			base.spriteAnimator.Play("idle");
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001CF1C File Offset: 0x0001B11C
		public void Interact(PlayerController interactor)
		{
			bool flag = TextBoxManager.HasTextBox(this.talkPoint);
			bool flag2 = !flag;
			if (flag2)
			{
				this.m_canUse = ((this.CanUse != null) ? this.CanUse(interactor, base.gameObject) : this.m_canUse);
				bool flag3 = !this.m_canUse;
				bool flag4 = flag3;
				if (flag4)
				{
					base.spriteAnimator.PlayForDuration("talk", 2f, "idle", false);
					TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 2f, "No... not this time.", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
				}
				else
				{
					if (!TableNPC.QuestComplete)
					{
						base.StartCoroutine(this.HandleConversation(interactor));
					}
					else
					{
						base.StartCoroutine(this.HandleConversation2(interactor));
					}
				}
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001CFD5 File Offset: 0x0001B1D5
		private IEnumerator HandleConversation(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			base.spriteAnimator.PlayForDuration("talk", 1f, "talk", false);
			interactor.SetInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return null;
			List<string> conversationToUse = TableNPC.YouTalkedToMeAlready ? this.conversation3 : this.conversation;
			int num;
			for (int conversationIndex = 0; conversationIndex < conversationToUse.Count - 1; conversationIndex = num + 1)
			{
				
				TextBoxManager.ClearTextBox(this.talkPoint);
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, conversationToUse[conversationIndex], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
				float timer = 0f;
				while (!BraveInput.GetInstanceForPlayer(interactor.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
				{
					timer += BraveTime.DeltaTime;
					yield return null;
				}
				num = conversationIndex;
			}
			this.m_allowMeToIntroduceMyself = false;
			if (!TableNPC.QuestComplete)
			{ 
			TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, conversationToUse[conversationToUse.Count - 1], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
		    }
			
			string acceptanceTextToUse = this.acceptText;
			string declineTextToUse = this.declineText;
			if (conversationToUse == this.conversation)
			{
				GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, acceptanceTextToUse, declineTextToUse);
			}
			int selectedResponse = -1;
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			bool flag = selectedResponse == 0;
			if (flag)
			{
				TextBoxManager.ClearTextBox(this.talkPoint);
				base.spriteAnimator.PlayForDuration("talk", -1f, "talk", false);
				Action<PlayerController, GameObject> onAccept = this.OnAccept;
				if (onAccept != null)
				{
					if (!TableNPC.YouTalkedToMeAlready)
					{
						TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "Perform this arduous task and I'll consider teaching you...", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
					}
					onAccept(interactor, base.gameObject);
				}
				base.spriteAnimator.Play("talk");
			}
			else
			{
				Action<PlayerController, GameObject> onDecline = this.OnDecline;
				if (onDecline != null)
				{
					onDecline(interactor, base.gameObject);
				}
				TextBoxManager.ClearTextBox(this.talkPoint);
			}
			interactor.ClearInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
			base.spriteAnimator.Play("idle");
			yield break;
		}

		private IEnumerator HandleConversation2(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
			base.spriteAnimator.PlayForDuration("talk", 1f, "talk", false);
			interactor.SetInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
			yield return null;
			List<string> conversationToUse = TableNPC.RewardGotten ? this.conversation4 : this.conversation2;
			int num;
			for (int conversationIndex = 0; conversationIndex < conversationToUse.Count - 1; conversationIndex = num + 1)
			{
				
				TextBoxManager.ClearTextBox(this.talkPoint);
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, conversationToUse[conversationIndex], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
				float timer = 0f;
				while (!BraveInput.GetInstanceForPlayer(interactor.PlayerIDX).ActiveActions.GetActionFromType(GungeonActions.GungeonActionType.Interact).WasPressed || timer < 0.4f)
				{
					timer += BraveTime.DeltaTime;
					yield return null;
				}
				num = conversationIndex;
			}
			this.m_allowMeToIntroduceMyself = false;
			if (!TableNPC.QuestComplete)
			{
				TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, -1f, conversationToUse[conversationToUse.Count - 1], interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, true, false);
			}

			int selectedResponse = -1;
			while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
			{
				yield return null;
			}
			bool flag = selectedResponse == 0;
			if (flag)
			{
				TextBoxManager.ClearTextBox(this.talkPoint);
				base.spriteAnimator.PlayForDuration("talk", -1f, "talk", false);
				Action<PlayerController, GameObject> onAccept = this.OnAccept;
				if (onAccept != null)
				{
					if (!TableNPC.YouTalkedToMeAlready)
					{
						TextBoxManager.ShowTextBox(this.talkPoint.position, this.talkPoint, 1f, "Perform this arduous task and I'll consider teaching you...", interactor.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
					}
					onAccept(interactor, base.gameObject);
				}
				base.spriteAnimator.Play("talk");
			}
			else
			{
				Action<PlayerController, GameObject> onDecline = this.OnDecline;
				if (onDecline != null)
				{
					onDecline(interactor, base.gameObject);
				}
				TextBoxManager.ClearTextBox(this.talkPoint);
			}
			interactor.ClearInputOverride("npcConversation");
			Pixelator.Instance.LerpToLetterbox(1f, 0.25f);
			base.spriteAnimator.Play("idle");
			yield break;
		}
		// Token: 0x06000373 RID: 883 RVA: 0x0001CFEB File Offset: 0x0001B1EB
		public void OnEnteredRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001D016 File Offset: 0x0001B216
		public void OnExitRange(PlayerController interactor)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001D038 File Offset: 0x0001B238
		public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
		{
			shouldBeFlipped = false;
			return string.Empty;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001D054 File Offset: 0x0001B254
		public float GetDistanceToPoint(Vector2 point)
		{
			bool flag = base.sprite == null;
			bool flag2 = flag;
			float result;
			if (flag2)
			{
				result = 100f;
			}
			else
			{
				Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
				result = Vector2.Distance(point, v) / 1.5f;
			}
			return result;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001D0BC File Offset: 0x0001B2BC
		public float GetOverrideMaxDistance()
		{
			return -1f;
		}

		// Token: 0x040000F7 RID: 247
		
	}
}
