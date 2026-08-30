using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D6 RID: 6614
	[Preserve]
	public class ActionWaitForDead : BaseAction
	{
		// Token: 0x0600C9EF RID: 51695 RVA: 0x004A10D8 File Offset: 0x0049F2D8
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

		// Token: 0x0600C9F0 RID: 51696 RVA: 0x004A11FF File Offset: 0x0049F3FF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600C9F1 RID: 51697 RVA: 0x004A1208 File Offset: 0x0049F408
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionWaitForDead.PropTargetGroup))
			{
				this.targetGroup = properties.Values[ActionWaitForDead.PropTargetGroup];
			}
			properties.ParseInt(ActionWaitForDead.PropPhaseOnDespawn, ref this.phaseOnDespawn);
		}

		// Token: 0x0600C9F2 RID: 51698 RVA: 0x004A1255 File Offset: 0x0049F455
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionWaitForDead
			{
				targetGroup = this.targetGroup,
				phaseOnDespawn = this.phaseOnDespawn
			};
		}

		// Token: 0x0400993C RID: 39228
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400993D RID: 39229
		[PublicizedFrom(EAccessModifier.Protected)]
		public int phaseOnDespawn = -1;

		// Token: 0x0400993E RID: 39230
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x0400993F RID: 39231
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDespawn = "phase_on_despawn";

		// Token: 0x04009940 RID: 39232
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x04009941 RID: 39233
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
