using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C3 RID: 6595
	[Preserve]
	public class ActionPrimeEntity : ActionBaseTargetAction
	{
		// Token: 0x0600CA25 RID: 51749 RVA: 0x004A01AC File Offset: 0x0049E3AC
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityZombieCop entityZombieCop = target as EntityZombieCop;
			if (entityZombieCop != null)
			{
				entityZombieCop.HandlePrimingDetonator(GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.overrideTimeText, -1f));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA26 RID: 51750 RVA: 0x004A01F0 File Offset: 0x0049E3F0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPrimeEntity.PropOverrideTime, ref this.overrideTimeText);
		}

		// Token: 0x0600CA27 RID: 51751 RVA: 0x004A020A File Offset: 0x0049E40A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPrimeEntity
			{
				targetGroup = this.targetGroup,
				overrideTimeText = this.overrideTimeText
			};
		}

		// Token: 0x040098E7 RID: 39143
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTimeText;

		// Token: 0x040098E8 RID: 39144
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideTime = "override_time";
	}
}
