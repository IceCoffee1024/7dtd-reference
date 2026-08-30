using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200196F RID: 6511
	[Preserve]
	public class ActionBaseTargetAction : BaseAction
	{
		// Token: 0x0600C82A RID: 51242 RVA: 0x00496E64 File Offset: 0x00495064
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

		// Token: 0x0600C82B RID: 51243 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartTargetAction()
		{
		}

		// Token: 0x0600C82C RID: 51244 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void EndTargetAction()
		{
		}

		// Token: 0x0600C82D RID: 51245 RVA: 0x00496FA1 File Offset: 0x004951A1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.targetList = null;
			this.index = 0;
		}

		// Token: 0x0600C82E RID: 51246 RVA: 0x00046EDA File Offset: 0x000450DA
		public virtual BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C82F RID: 51247 RVA: 0x00496FB7 File Offset: 0x004951B7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTargetAction.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C830 RID: 51248 RVA: 0x00496FD1 File Offset: 0x004951D1
		public override BaseAction Clone()
		{
			ActionBaseTargetAction actionBaseTargetAction = (ActionBaseTargetAction)base.Clone();
			actionBaseTargetAction.targetGroup = this.targetGroup;
			return actionBaseTargetAction;
		}

		// Token: 0x0600C831 RID: 51249 RVA: 0x00496FEA File Offset: 0x004951EA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBaseTargetAction
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x0400971B RID: 38683
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400971C RID: 38684
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x0400971D RID: 38685
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> targetList;

		// Token: 0x0400971E RID: 38686
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
