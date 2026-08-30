using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B0 RID: 6576
	[Preserve]
	public class ActionWaitForDead : BaseAction
	{
		// Token: 0x0600C868 RID: 51304 RVA: 0x0049AD80 File Offset: 0x00498F80
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.entityList == null)
			{
				this.entityList = new List<EntityAlive>();
				List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
				if (entityGroup == null)
				{
					Debug.LogWarning("ActionWaitForDead: Target Group " + this.targetGroup + " Does not exist!");
					return BaseAction.ActionCompleteStates.InCompleteRefund;
				}
				for (int i = 0; i < entityGroup.Count; i++)
				{
					EntityAlive entityAlive = entityGroup[i] as EntityAlive;
					if (entityAlive != null)
					{
						this.entityList.Add(entityAlive);
					}
				}
			}
			else
			{
				this.checkTime -= Time.deltaTime;
				if (this.checkTime <= 0f)
				{
					if (base.Owner.HasDespawn)
					{
						this.PhaseOnComplete = this.phaseOnDespawn;
						return BaseAction.ActionCompleteStates.Complete;
					}
					bool flag = false;
					for (int j = this.entityList.Count - 1; j >= 0; j--)
					{
						EntityAlive entityAlive2 = this.entityList[j];
						if (entityAlive2 != null)
						{
							if (entityAlive2.IsAlive())
							{
								flag = true;
							}
							else
							{
								this.entityList.RemoveAt(j);
							}
						}
					}
					if (!flag)
					{
						return BaseAction.ActionCompleteStates.Complete;
					}
					this.checkTime = 1f;
					return BaseAction.ActionCompleteStates.InComplete;
				}
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C869 RID: 51305 RVA: 0x0049AEA7 File Offset: 0x004990A7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600C86A RID: 51306 RVA: 0x0049AEB0 File Offset: 0x004990B0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionWaitForDead.PropTargetGroup))
			{
				this.targetGroup = properties.Values[ActionWaitForDead.PropTargetGroup];
			}
			properties.ParseInt(ActionWaitForDead.PropPhaseOnDespawn, ref this.phaseOnDespawn);
		}

		// Token: 0x0600C86B RID: 51307 RVA: 0x0049AEFD File Offset: 0x004990FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionWaitForDead
			{
				targetGroup = this.targetGroup,
				phaseOnDespawn = this.phaseOnDespawn
			};
		}

		// Token: 0x04009813 RID: 38931
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009814 RID: 38932
		[PublicizedFrom(EAccessModifier.Protected)]
		public int phaseOnDespawn = -1;

		// Token: 0x04009815 RID: 38933
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009816 RID: 38934
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDespawn = "phase_on_despawn";

		// Token: 0x04009817 RID: 38935
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x04009818 RID: 38936
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
