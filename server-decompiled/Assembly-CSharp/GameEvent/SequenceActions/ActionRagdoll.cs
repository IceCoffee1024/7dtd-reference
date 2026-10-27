using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F8 RID: 6648
	[Preserve]
	public class ActionRagdoll : ActionBaseTargetAction
	{
		// Token: 0x0600CB4A RID: 52042 RVA: 0x004A4258 File Offset: 0x004A2458
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

		// Token: 0x0600CB4B RID: 52043 RVA: 0x004A42C9 File Offset: 0x004A24C9
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRagdoll.PropStunDuration, ref this.stunDurationText);
		}

		// Token: 0x0600CB4C RID: 52044 RVA: 0x004A42E3 File Offset: 0x004A24E3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRagdoll
			{
				targetGroup = this.targetGroup,
				stunDurationText = this.stunDurationText
			};
		}

		// Token: 0x040099E6 RID: 39398
		[PublicizedFrom(EAccessModifier.Protected)]
		public string stunDurationText;

		// Token: 0x040099E7 RID: 39399
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStunDuration = "stun_duration";
	}
}
