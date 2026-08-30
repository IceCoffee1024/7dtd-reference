using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001976 RID: 6518
	[Preserve]
	public class ActionRageZombies : ActionBaseTargetAction
	{
		// Token: 0x0600C75C RID: 51036 RVA: 0x00496008 File Offset: 0x00494208
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

		// Token: 0x0600C75D RID: 51037 RVA: 0x00496097 File Offset: 0x00494297
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRageZombies.PropTime, ref this.rageTimeText);
			properties.ParseString(ActionRageZombies.PropSpeedPercent, ref this.speedPercentText);
		}

		// Token: 0x0600C75E RID: 51038 RVA: 0x004960C2 File Offset: 0x004942C2
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

		// Token: 0x04009710 RID: 38672
		[PublicizedFrom(EAccessModifier.Protected)]
		public string rageTimeText;

		// Token: 0x04009711 RID: 38673
		[PublicizedFrom(EAccessModifier.Protected)]
		public string speedPercentText;

		// Token: 0x04009712 RID: 38674
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x04009713 RID: 38675
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpeedPercent = "speed_percent";
	}
}
