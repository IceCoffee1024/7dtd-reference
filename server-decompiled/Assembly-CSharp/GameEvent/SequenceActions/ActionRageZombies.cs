using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C8 RID: 6600
	[Preserve]
	public class ActionRageZombies : ActionBaseTargetAction
	{
		// Token: 0x0600CA3B RID: 51771 RVA: 0x004A0810 File Offset: 0x0049EA10
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

		// Token: 0x0600CA3C RID: 51772 RVA: 0x004A089F File Offset: 0x0049EA9F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRageZombies.PropTime, ref this.rageTimeText);
			properties.ParseString(ActionRageZombies.PropSpeedPercent, ref this.speedPercentText);
		}

		// Token: 0x0600CA3D RID: 51773 RVA: 0x004A08CA File Offset: 0x0049EACA
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

		// Token: 0x040098FF RID: 39167
		[PublicizedFrom(EAccessModifier.Protected)]
		public string rageTimeText;

		// Token: 0x04009900 RID: 39168
		[PublicizedFrom(EAccessModifier.Protected)]
		public string speedPercentText;

		// Token: 0x04009901 RID: 39169
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x04009902 RID: 39170
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpeedPercent = "speed_percent";
	}
}
