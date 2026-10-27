using System;
using GameEvent.SequenceActions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceLoops
{
	// Token: 0x020019A8 RID: 6568
	[Preserve]
	public class LoopFor : BaseLoop
	{
		// Token: 0x0600C9F5 RID: 51701 RVA: 0x0049A43C File Offset: 0x0049863C
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

		// Token: 0x0600C9F6 RID: 51702 RVA: 0x0049A4D1 File Offset: 0x004986D1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.loopCount = -1;
			this.currentLoop = 0;
		}

		// Token: 0x0600C9F7 RID: 51703 RVA: 0x0049A4E7 File Offset: 0x004986E7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(LoopFor.PropLoopCount, ref this.loopCountText);
		}

		// Token: 0x0600C9F8 RID: 51704 RVA: 0x0049A501 File Offset: 0x00498701
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new LoopFor
			{
				loopCountText = this.loopCountText
			};
		}

		// Token: 0x040097DC RID: 38876
		[PublicizedFrom(EAccessModifier.Private)]
		public int loopCount = -1;

		// Token: 0x040097DD RID: 38877
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentLoop;

		// Token: 0x040097DE RID: 38878
		public string loopCountText;

		// Token: 0x040097DF RID: 38879
		public static string PropLoopCount = "loop_count";
	}
}
