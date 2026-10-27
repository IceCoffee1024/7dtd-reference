using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F4 RID: 6644
	[Preserve]
	public class ActionPrimeEntity : ActionBaseTargetAction
	{
		// Token: 0x0600CB39 RID: 52025 RVA: 0x004A3CAC File Offset: 0x004A1EAC
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityZombieCop entityZombieCop = target as EntityZombieCop;
			if (entityZombieCop != null)
			{
				entityZombieCop.HandlePrimingDetonator(GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.overrideTimeText, -1f));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB3A RID: 52026 RVA: 0x004A3CF0 File Offset: 0x004A1EF0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPrimeEntity.PropOverrideTime, ref this.overrideTimeText);
		}

		// Token: 0x0600CB3B RID: 52027 RVA: 0x004A3D0A File Offset: 0x004A1F0A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPrimeEntity
			{
				targetGroup = this.targetGroup,
				overrideTimeText = this.overrideTimeText
			};
		}

		// Token: 0x040099D0 RID: 39376
		[PublicizedFrom(EAccessModifier.Protected)]
		public string overrideTimeText;

		// Token: 0x040099D1 RID: 39377
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOverrideTime = "override_time";
	}
}
