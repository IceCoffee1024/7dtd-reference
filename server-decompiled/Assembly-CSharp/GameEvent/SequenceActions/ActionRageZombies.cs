using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F9 RID: 6649
	[Preserve]
	public class ActionRageZombies : ActionBaseTargetAction
	{
		// Token: 0x0600CB4F RID: 52047 RVA: 0x004A4310 File Offset: 0x004A2510
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				if (entityAlive is EntityPlayer || entityAlive is EntityNPC)
				{
					return BaseAction.ActionCompleteStates.Complete;
				}
				EntityHuman entityHuman = entityAlive as EntityHuman;
				if (entityHuman != null)
				{
					entityHuman.ConditionalTriggerSleeperWakeUp();
					entityHuman.StartRage(GameEventManager.GetFloatValue(entityAlive, this.speedPercentText, 2f), GameEventManager.GetFloatValue(entityAlive, this.rageTimeText, 5f) + 1f);
				}
				EntityAlive entityAlive2 = base.Owner.Target as EntityAlive;
				if (entityAlive2 != null)
				{
					entityAlive.SetAttackTarget(entityAlive2, 12000);
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB50 RID: 52048 RVA: 0x004A439F File Offset: 0x004A259F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRageZombies.PropTime, ref this.rageTimeText);
			properties.ParseString(ActionRageZombies.PropSpeedPercent, ref this.speedPercentText);
		}

		// Token: 0x0600CB51 RID: 52049 RVA: 0x004A43CA File Offset: 0x004A25CA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRageZombies
			{
				rageTimeText = this.rageTimeText,
				speedPercentText = this.speedPercentText,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040099E8 RID: 39400
		[PublicizedFrom(EAccessModifier.Protected)]
		public string rageTimeText;

		// Token: 0x040099E9 RID: 39401
		[PublicizedFrom(EAccessModifier.Protected)]
		public string speedPercentText;

		// Token: 0x040099EA RID: 39402
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x040099EB RID: 39403
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpeedPercent = "speed_percent";
	}
}
