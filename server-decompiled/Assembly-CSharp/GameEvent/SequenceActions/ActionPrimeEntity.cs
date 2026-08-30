using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001997 RID: 6551
	[Preserve]
	public class ActionPrimeEntity : ActionBaseTargetAction
	{
		// Token: 0x0600C8CD RID: 51405 RVA: 0x0049BCE8 File Offset: 0x00499EE8
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityZombieCop entityZombieCop = target as EntityZombieCop;
			if (entityZombieCop != null)
			{
				entityZombieCop.HandlePrimingDetonator(GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.overrideTimeText, -1f));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C8CE RID: 51406 RVA: 0x0049BD2C File Offset: 0x00499F2C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPrimeEntity.PropOverrideTime, ref this.overrideTimeText);
		}

		// Token: 0x0600C8CF RID: 51407 RVA: 0x0049BD46 File Offset: 0x00499F46
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPrimeEntity
			{
				targetGroup = this.targetGroup,
				overrideTimeText = this.overrideTimeText
			};
		}

		// Token: 0x04009821 RID: 38945
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTimeText;

		// Token: 0x04009822 RID: 38946
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideTime = "override_time";
	}
}
