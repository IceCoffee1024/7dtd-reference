using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C7 RID: 6599
	[Preserve]
	public class ActionRagdoll : ActionBaseTargetAction
	{
		// Token: 0x0600CA36 RID: 51766 RVA: 0x004A0758 File Offset: 0x0049E958
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				if (entityAlive.IsInElevator())
				{
					return BaseAction.ActionCompleteStates.InComplete;
				}
				if (entityAlive.AttachedToEntity != null)
				{
					entityAlive.Detach();
				}
				DamageResponse damageResponse = DamageResponse.New(false);
				damageResponse.StunDuration = GameEventManager.GetFloatValue(entityAlive, this.stunDurationText, 1f);
				damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Bashing);
				entityAlive.DoRagdoll(in damageResponse);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA37 RID: 51767 RVA: 0x004A07C9 File Offset: 0x0049E9C9
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRagdoll.PropStunDuration, ref this.stunDurationText);
		}

		// Token: 0x0600CA38 RID: 51768 RVA: 0x004A07E3 File Offset: 0x0049E9E3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRagdoll
			{
				targetGroup = this.targetGroup,
				stunDurationText = this.stunDurationText
			};
		}

		// Token: 0x040098FD RID: 39165
		[PublicizedFrom(EAccessModifier.Protected)]
		public string stunDurationText;

		// Token: 0x040098FE RID: 39166
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStunDuration = "stun_duration";
	}
}
