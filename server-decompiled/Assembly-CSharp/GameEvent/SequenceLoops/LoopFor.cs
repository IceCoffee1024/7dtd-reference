using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x0200194B RID: 6475
	[Preserve]
	public class LoopFor : BaseLoop
	{
		// Token: 0x0600C789 RID: 51081 RVA: 0x0049255C File Offset: 0x0049075C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.loopCount == -1)
			{
				this.loopCount = GameEventManager.GetIntValue(base.Owner.Target as EntityAlive, this.loopCountText, 1);
			}
			if (base.HandleActions() == BaseAction.ActionCompleteStates.Complete)
			{
				this.currentLoop++;
				this.CurrentPhase = 0;
				for (int i = 0; i < this.Actions.Count; i++)
				{
					this.Actions[i].Reset();
				}
				if (this.currentLoop >= this.loopCount)
				{
					this.IsComplete = true;
					return BaseAction.ActionCompleteStates.Complete;
				}
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C78A RID: 51082 RVA: 0x004925F1 File Offset: 0x004907F1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.loopCount = -1;
			this.currentLoop = 0;
		}

		// Token: 0x0600C78B RID: 51083 RVA: 0x00492607 File Offset: 0x00490807
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(LoopFor.PropLoopCount, ref this.loopCountText);
		}

		// Token: 0x0600C78C RID: 51084 RVA: 0x00492621 File Offset: 0x00490821
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopFor
			{
				loopCountText = this.loopCountText
			};
		}

		// Token: 0x0400962D RID: 38445
		[PublicizedFrom(EAccessModifier.Private)]
		public int loopCount = -1;

		// Token: 0x0400962E RID: 38446
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentLoop;

		// Token: 0x0400962F RID: 38447
		public string loopCountText;

		// Token: 0x04009630 RID: 38448
		public static string PropLoopCount = "loop_count";
	}
}
