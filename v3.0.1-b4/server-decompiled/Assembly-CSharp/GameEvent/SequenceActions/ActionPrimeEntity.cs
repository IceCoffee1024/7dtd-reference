using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001971 RID: 6513
	[Preserve]
	public class ActionPrimeEntity : ActionBaseTargetAction
	{
		// Token: 0x0600C746 RID: 51014 RVA: 0x004959A4 File Offset: 0x00493BA4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityZombieCop entityZombieCop = target as EntityZombieCop;
			if (entityZombieCop != null)
			{
				entityZombieCop.HandlePrimingDetonator(GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.overrideTimeText, -1f));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C747 RID: 51015 RVA: 0x004959E8 File Offset: 0x00493BE8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPrimeEntity.PropOverrideTime, ref this.overrideTimeText);
		}

		// Token: 0x0600C748 RID: 51016 RVA: 0x00495A02 File Offset: 0x00493C02
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPrimeEntity
			{
				targetGroup = this.targetGroup,
				overrideTimeText = this.overrideTimeText
			};
		}

		// Token: 0x040096F8 RID: 38648
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTimeText;

		// Token: 0x040096F9 RID: 38649
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideTime = "override_time";
	}
}
