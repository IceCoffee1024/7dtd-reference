using System;
using System.Collections;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C0 RID: 6592
	[Preserve]
	public class ActionPlaySound : ActionBaseTargetAction
	{
		// Token: 0x0600CA11 RID: 51729 RVA: 0x0049FB88 File Offset: 0x0049DD88
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

		// Token: 0x0600CA12 RID: 51730 RVA: 0x0049FE54 File Offset: 0x0049E054
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

		// Token: 0x0600CA13 RID: 51731 RVA: 0x0049FED3 File Offset: 0x0049E0D3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator StopSound(Vector3 position, string soundName)
		{
			yield return new WaitForSeconds(this.duration + 0.001f);
			Manager.BroadcastStop(position, soundName);
			yield break;
		}

		// Token: 0x0600CA14 RID: 51732 RVA: 0x0049FEF0 File Offset: 0x0049E0F0
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator StopInHeadSound(string soundName)
		{
			yield return new WaitForSeconds(this.duration + 0.001f);
			Manager.StopLoopInsidePlayerHead(soundName, -1, false);
			yield break;
		}

		// Token: 0x0600CA15 RID: 51733 RVA: 0x0049FF08 File Offset: 0x0049E108
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

		// Token: 0x0600CA16 RID: 51734 RVA: 0x004A0004 File Offset: 0x0049E204
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

		// Token: 0x040098D2 RID: 39122
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool insideHead;

		// Token: 0x040098D3 RID: 39123
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool behindPlayer;

		// Token: 0x040098D4 RID: 39124
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duration;

		// Token: 0x040098D5 RID: 39125
		[PublicizedFrom(EAccessModifier.Protected)]
		public float delay;

		// Token: 0x040098D6 RID: 39126
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool canDisable = true;

		// Token: 0x040098D7 RID: 39127
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] soundNames;

		// Token: 0x040098D8 RID: 39128
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSound = "sound";

		// Token: 0x040098D9 RID: 39129
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropInsideHead = "inside_head";

		// Token: 0x040098DA RID: 39130
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBehindPlayer = "behind_player";

		// Token: 0x040098DB RID: 39131
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLoopDuration = "loop_duration";

		// Token: 0x040098DC RID: 39132
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDelay = "delay";

		// Token: 0x040098DD RID: 39133
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCanDisable = "can_disable";
	}
}
