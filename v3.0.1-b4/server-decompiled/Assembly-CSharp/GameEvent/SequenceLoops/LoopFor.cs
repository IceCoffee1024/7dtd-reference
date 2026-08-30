using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x02001925 RID: 6437
	[Preserve]
	public class LoopFor : BaseLoop
	{
		// Token: 0x0600C602 RID: 50690 RVA: 0x0048C210 File Offset: 0x0048A410
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

		// Token: 0x0600C603 RID: 50691 RVA: 0x0048C2A5 File Offset: 0x0048A4A5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.loopCount = -1;
			this.currentLoop = 0;
		}

		// Token: 0x0600C604 RID: 50692 RVA: 0x0048C2BB File Offset: 0x0048A4BB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(LoopFor.PropLoopCount, ref this.loopCountText);
		}

		// Token: 0x0600C605 RID: 50693 RVA: 0x0048C2D5 File Offset: 0x0048A4D5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopFor
			{
				loopCountText = this.loopCountText
			};
		}

		// Token: 0x04009504 RID: 38148
		[PublicizedFrom(EAccessModifier.Private)]
		public int loopCount = -1;

		// Token: 0x04009505 RID: 38149
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentLoop;

		// Token: 0x04009506 RID: 38150
		public string loopCountText;

		// Token: 0x04009507 RID: 38151
		public static string PropLoopCount = "loop_count";
	}
}
