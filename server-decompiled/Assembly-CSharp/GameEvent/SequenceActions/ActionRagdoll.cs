using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199B RID: 6555
	[Preserve]
	public class ActionRagdoll : ActionBaseTargetAction
	{
		// Token: 0x0600C8DE RID: 51422 RVA: 0x0049C294 File Offset: 0x0049A494
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

		// Token: 0x0600C8DF RID: 51423 RVA: 0x0049C305 File Offset: 0x0049A505
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRagdoll.PropStunDuration, ref this.stunDurationText);
		}

		// Token: 0x0600C8E0 RID: 51424 RVA: 0x0049C31F File Offset: 0x0049A51F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRagdoll
			{
				targetGroup = this.targetGroup,
				stunDurationText = this.stunDurationText
			};
		}

		// Token: 0x04009837 RID: 38967
		[PublicizedFrom(EAccessModifier.Protected)]
		public string stunDurationText;

		// Token: 0x04009838 RID: 38968
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStunDuration = "stun_duration";
	}
}
