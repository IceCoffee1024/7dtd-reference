using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199B RID: 6555
	[Preserve]
	public class ActionBaseTargetAction : BaseAction
	{
		// Token: 0x0600C982 RID: 51586 RVA: 0x0049B340 File Offset: 0x00499540
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.targetGroup != "")
			{
				if (this.targetList == null)
				{
					this.targetList = new List<Entity>();
					List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
					if (entityGroup != null)
					{
						this.targetList.AddRange(entityGroup);
						this.index = 0;
						this.StartTargetAction();
					}
				}
				else
				{
					if (this.targetList.Count <= this.index)
					{
						return BaseAction.ActionCompleteStates.Complete;
					}
					Entity entity = this.targetList[this.index];
					if ((entity is EntityAlive && entity.IsDead()) || entity.IsDespawned)
					{
						this.index++;
						if (this.index >= this.targetList.Count)
						{
							this.EndTargetAction();
							return BaseAction.ActionCompleteStates.Complete;
						}
					}
					else
					{
						BaseAction.ActionCompleteStates actionCompleteStates = this.PerformTargetAction(entity);
						if (actionCompleteStates == BaseAction.ActionCompleteStates.Complete)
						{
							this.index++;
						}
						else if (actionCompleteStates == BaseAction.ActionCompleteStates.InCompleteRefund)
						{
							return BaseAction.ActionCompleteStates.InCompleteRefund;
						}
						if (this.index >= this.targetList.Count)
						{
							this.EndTargetAction();
							return BaseAction.ActionCompleteStates.Complete;
						}
					}
				}
				return BaseAction.ActionCompleteStates.InComplete;
			}
			this.StartTargetAction();
			BaseAction.ActionCompleteStates actionCompleteStates2 = this.PerformTargetAction(base.Owner.Target);
			if (actionCompleteStates2 == BaseAction.ActionCompleteStates.Complete)
			{
				this.EndTargetAction();
				return BaseAction.ActionCompleteStates.Complete;
			}
			if (actionCompleteStates2 == BaseAction.ActionCompleteStates.InCompleteRefund)
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C983 RID: 51587 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartTargetAction()
		{
		}

		// Token: 0x0600C984 RID: 51588 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void EndTargetAction()
		{
		}

		// Token: 0x0600C985 RID: 51589 RVA: 0x0049B47D File Offset: 0x0049967D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.targetList = null;
			this.index = 0;
		}

		// Token: 0x0600C986 RID: 51590 RVA: 0x000470B6 File Offset: 0x000452B6
		public virtual BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C987 RID: 51591 RVA: 0x0049B493 File Offset: 0x00499693
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTargetAction.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C988 RID: 51592 RVA: 0x0049B4AD File Offset: 0x004996AD
		public override BaseAction Clone()
		{
			ActionBaseTargetAction actionBaseTargetAction = (ActionBaseTargetAction)base.Clone();
			actionBaseTargetAction.targetGroup = this.targetGroup;
			return actionBaseTargetAction;
		}

		// Token: 0x0600C989 RID: 51593 RVA: 0x0049B4C6 File Offset: 0x004996C6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBaseTargetAction
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040097E1 RID: 38881
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040097E2 RID: 38882
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040097E3 RID: 38883
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> targetList;

		// Token: 0x040097E4 RID: 38884
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
