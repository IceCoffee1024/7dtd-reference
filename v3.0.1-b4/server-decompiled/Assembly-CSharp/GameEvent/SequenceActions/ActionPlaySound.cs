using System;
using System.Collections;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200196E RID: 6510
	[Preserve]
	public class ActionPlaySound : ActionBaseTargetAction
	{
		// Token: 0x0600C732 RID: 50994 RVA: 0x00495380 File Offset: 0x00493580
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			if (target != null)
			{
				if (this.canDisable)
				{
					EntityAlive entityAlive = target as EntityAlive;
					if (entityAlive != null && EffectManager.GetValue(PassiveEffects.DisableGameEventNotify, null, 0f, entityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0f)
					{
						return BaseAction.ActionCompleteStates.Complete;
					}
				}
				if (this.insideHead)
				{
					EntityPlayer entityPlayer = target as EntityPlayer;
					if (entityPlayer != null)
					{
						if (entityPlayer is EntityPlayerLocal)
						{
							this.OnClientPerform(entityPlayer);
						}
						else
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(base.Owner.Name, entityPlayer.entityId, base.Owner.ExtraData, base.Owner.Tag, NetPackageGameEventResponse.ResponseTypes.ClientSequenceAction, -1, -1, false, base.GetActionKey()), false, entityPlayer.entityId, -1, -1, null, 192, false);
						}
					}
				}
				else if (this.soundNames != null)
				{
					if (this.duration == 0f)
					{
						if (this.behindPlayer)
						{
							Manager.BroadcastPlayByLocalPlayer(target.position + (target.transform.forward * -1f + new Vector3(global::UnityEngine.Random.Range(-5f, 5f), 0f, 0f)), this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)]);
						}
						else
						{
							Manager.BroadcastPlayByLocalPlayer(target.position, this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)]);
						}
					}
					else
					{
						Vector3 vector = (this.behindPlayer ? (target.position + (target.transform.forward * -1f + new Vector3(global::UnityEngine.Random.Range(-5f, 5f), 0f, 0f))) : target.position);
						string text = this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)];
						Manager.BroadcastPlayByLocalPlayer(vector, text);
						GameManager.Instance.StartCoroutine(this.StopSound(vector, text));
					}
				}
			}
			else if (base.Owner.TargetPosition != Vector3.zero && this.soundNames != null)
			{
				if (this.duration == 0f)
				{
					Manager.BroadcastPlay(base.Owner.TargetPosition, this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)], 0f);
				}
				else
				{
					Vector3 targetPosition = base.Owner.TargetPosition;
					string text2 = this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)];
					Manager.BroadcastPlay(targetPosition, text2, 0f);
					GameManager.Instance.StartCoroutine(this.StopSound(targetPosition, text2));
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C733 RID: 50995 RVA: 0x0049564C File Offset: 0x0049384C
		public override void OnClientPerform(Entity target)
		{
			if (this.duration == 0f)
			{
				Manager.PlayInsidePlayerHead(this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)], target.entityId, this.delay, false, false);
				return;
			}
			string text = this.soundNames[global::UnityEngine.Random.Range(0, this.soundNames.Length)];
			Manager.PlayInsidePlayerHead(text, target.entityId, this.delay, false, false);
			GameManager.Instance.StartCoroutine(this.StopInHeadSound(text));
		}

		// Token: 0x0600C734 RID: 50996 RVA: 0x004956CB File Offset: 0x004938CB
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator StopSound(Vector3 position, string soundName)
		{
			yield return new WaitForSeconds(this.duration + 0.001f);
			Manager.BroadcastStop(position, soundName);
			yield break;
		}

		// Token: 0x0600C735 RID: 50997 RVA: 0x004956E8 File Offset: 0x004938E8
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator StopInHeadSound(string soundName)
		{
			yield return new WaitForSeconds(this.duration + 0.001f);
			Manager.StopLoopInsidePlayerHead(soundName, -1, false);
			yield break;
		}

		// Token: 0x0600C736 RID: 50998 RVA: 0x00495700 File Offset: 0x00493900
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionPlaySound.PropSound))
			{
				this.soundNames = properties.Values[ActionPlaySound.PropSound].Split(',', StringSplitOptions.None);
			}
			if (properties.Values.ContainsKey(ActionPlaySound.PropInsideHead))
			{
				this.insideHead = StringParsers.ParseBool(properties.Values[ActionPlaySound.PropInsideHead], 0, -1, true);
			}
			if (properties.Values.ContainsKey(ActionPlaySound.PropBehindPlayer))
			{
				this.behindPlayer = StringParsers.ParseBool(properties.Values[ActionPlaySound.PropBehindPlayer], 0, -1, true);
			}
			if (properties.Values.ContainsKey(ActionPlaySound.PropLoopDuration))
			{
				this.duration = StringParsers.ParseFloat(properties.Values[ActionPlaySound.PropLoopDuration], 0, -1, NumberStyles.Any);
			}
			properties.ParseBool(ActionPlaySound.PropCanDisable, ref this.canDisable);
			properties.ParseFloat(ActionPlaySound.PropDelay, ref this.delay);
		}

		// Token: 0x0600C737 RID: 50999 RVA: 0x004957FC File Offset: 0x004939FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPlaySound
			{
				targetGroup = this.targetGroup,
				soundNames = this.soundNames,
				insideHead = this.insideHead,
				behindPlayer = this.behindPlayer,
				duration = this.duration,
				delay = this.delay,
				canDisable = this.canDisable
			};
		}

		// Token: 0x040096E3 RID: 38627
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool insideHead;

		// Token: 0x040096E4 RID: 38628
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool behindPlayer;

		// Token: 0x040096E5 RID: 38629
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duration;

		// Token: 0x040096E6 RID: 38630
		[PublicizedFrom(EAccessModifier.Protected)]
		public float delay;

		// Token: 0x040096E7 RID: 38631
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canDisable = true;

		// Token: 0x040096E8 RID: 38632
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] soundNames;

		// Token: 0x040096E9 RID: 38633
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSound = "sound";

		// Token: 0x040096EA RID: 38634
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropInsideHead = "inside_head";

		// Token: 0x040096EB RID: 38635
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBehindPlayer = "behind_player";

		// Token: 0x040096EC RID: 38636
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLoopDuration = "loop_duration";

		// Token: 0x040096ED RID: 38637
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDelay = "delay";

		// Token: 0x040096EE RID: 38638
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCanDisable = "can_disable";
	}
}
