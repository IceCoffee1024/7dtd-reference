using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199C RID: 6556
	[Preserve]
	public class ActionRageZombies : ActionBaseTargetAction
	{
		// Token: 0x0600C8E3 RID: 51427 RVA: 0x0049C34C File Offset: 0x0049A54C
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

		// Token: 0x0600C8E4 RID: 51428 RVA: 0x0049C3DB File Offset: 0x0049A5DB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionRageZombies.PropTime, ref this.rageTimeText);
			properties.ParseString(ActionRageZombies.PropSpeedPercent, ref this.speedPercentText);
		}

		// Token: 0x0600C8E5 RID: 51429 RVA: 0x0049C406 File Offset: 0x0049A606
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

		// Token: 0x04009839 RID: 38969
		[PublicizedFrom(EAccessModifier.Protected)]
		public string rageTimeText;

		// Token: 0x0400983A RID: 38970
		[PublicizedFrom(EAccessModifier.Protected)]
		public string speedPercentText;

		// Token: 0x0400983B RID: 38971
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "time";

		// Token: 0x0400983C RID: 38972
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpeedPercent = "speed_percent";
	}
}
