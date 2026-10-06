using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x02001977 RID: 6519
	[Preserve]
	public class LoopFor : BaseLoop
	{
		// Token: 0x0600C8E1 RID: 51425 RVA: 0x00496A48 File Offset: 0x00494C48
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

		// Token: 0x0600C8E2 RID: 51426 RVA: 0x00496ADD File Offset: 0x00494CDD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.loopCount = -1;
			this.currentLoop = 0;
		}

		// Token: 0x0600C8E3 RID: 51427 RVA: 0x00496AF3 File Offset: 0x00494CF3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(LoopFor.PropLoopCount, ref this.loopCountText);
		}

		// Token: 0x0600C8E4 RID: 51428 RVA: 0x00496B0D File Offset: 0x00494D0D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopFor
			{
				loopCountText = this.loopCountText
			};
		}

		// Token: 0x040096F3 RID: 38643
		[PublicizedFrom(EAccessModifier.Private)]
		public int loopCount = -1;

		// Token: 0x040096F4 RID: 38644
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentLoop;

		// Token: 0x040096F5 RID: 38645
		public string loopCountText;

		// Token: 0x040096F6 RID: 38646
		public static string PropLoopCount = "loop_count";
	}
}
