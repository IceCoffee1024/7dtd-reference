using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001949 RID: 6473
	[Preserve]
	public class ActionBaseTargetAction : BaseAction
	{
		// Token: 0x0600C6A3 RID: 50851 RVA: 0x00490B18 File Offset: 0x0048ED18
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

		// Token: 0x0600C6A4 RID: 50852 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void StartTargetAction()
		{
		}

		// Token: 0x0600C6A5 RID: 50853 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void EndTargetAction()
		{
		}

		// Token: 0x0600C6A6 RID: 50854 RVA: 0x00490C55 File Offset: 0x0048EE55
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			base.OnReset();
			this.targetList = null;
			this.index = 0;
		}

		// Token: 0x0600C6A7 RID: 50855 RVA: 0x00046C7A File Offset: 0x00044E7A
		public virtual BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C6A8 RID: 50856 RVA: 0x00490C6B File Offset: 0x0048EE6B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionBaseTargetAction.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C6A9 RID: 50857 RVA: 0x00490C85 File Offset: 0x0048EE85
		public override BaseAction Clone()
		{
			ActionBaseTargetAction actionBaseTargetAction = (ActionBaseTargetAction)base.Clone();
			actionBaseTargetAction.targetGroup = this.targetGroup;
			return actionBaseTargetAction;
		}

		// Token: 0x0600C6AA RID: 50858 RVA: 0x00490C9E File Offset: 0x0048EE9E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBaseTargetAction
			{
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040095F2 RID: 38386
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040095F3 RID: 38387
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040095F4 RID: 38388
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> targetList;

		// Token: 0x040095F5 RID: 38389
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
