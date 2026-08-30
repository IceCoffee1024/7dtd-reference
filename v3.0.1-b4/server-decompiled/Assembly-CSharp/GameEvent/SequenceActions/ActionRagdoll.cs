using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001975 RID: 6517
	[Preserve]
	public class ActionRagdoll : ActionBaseTargetAction
	{
		// Token: 0x0600C757 RID: 51031 RVA: 0x00495F50 File Offset: 0x00494150
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

		// Token: 0x0600C758 RID: 51032 RVA: 0x00495FC1 File Offset: 0x004941C1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRagdoll.PropStunDuration, ref this.stunDurationText);
		}

		// Token: 0x0600C759 RID: 51033 RVA: 0x00495FDB File Offset: 0x004941DB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRagdoll
			{
				targetGroup = this.targetGroup,
				stunDurationText = this.stunDurationText
			};
		}

		// Token: 0x0400970E RID: 38670
		[PublicizedFrom(EAccessModifier.Protected)]
		public string stunDurationText;

		// Token: 0x0400970F RID: 38671
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStunDuration = "stun_duration";
	}
}
