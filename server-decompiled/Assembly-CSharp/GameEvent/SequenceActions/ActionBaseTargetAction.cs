using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CC RID: 6604
	[Preserve]
	public class ActionBaseTargetAction : BaseAction
	{
		// Token: 0x0600CA96 RID: 51862 RVA: 0x0049EE40 File Offset: 0x0049D040
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

		// Token: 0x0600CA97 RID: 51863 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartTargetAction()
		{
		}

		// Token: 0x0600CA98 RID: 51864 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void EndTargetAction()
		{
		}

		// Token: 0x0600CA99 RID: 51865 RVA: 0x0049EF7D File Offset: 0x0049D17D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.targetList = null;
			this.index = 0;
		}

		// Token: 0x0600CA9A RID: 51866 RVA: 0x00047076 File Offset: 0x00045276
		public virtual BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA9B RID: 51867 RVA: 0x0049EF93 File Offset: 0x0049D193
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTargetAction.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600CA9C RID: 51868 RVA: 0x0049EFAD File Offset: 0x0049D1AD
		public override BaseAction Clone()
		{
			ActionBaseTargetAction actionBaseTargetAction = (ActionBaseTargetAction)base.Clone();
			actionBaseTargetAction.targetGroup = this.targetGroup;
			return actionBaseTargetAction;
		}

		// Token: 0x0600CA9D RID: 51869 RVA: 0x0049EFC6 File Offset: 0x0049D1C6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBaseTargetAction
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040098CA RID: 39114
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040098CB RID: 39115
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040098CC RID: 39116
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> targetList;

		// Token: 0x040098CD RID: 39117
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
