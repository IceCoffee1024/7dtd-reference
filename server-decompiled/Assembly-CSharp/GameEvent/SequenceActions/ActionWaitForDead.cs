using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A33 RID: 6707
	[Preserve]
	public class ActionWaitForDead : BaseAction
	{
		// Token: 0x0600CC5B RID: 52315 RVA: 0x004A9078 File Offset: 0x004A7278
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

		// Token: 0x0600CC5C RID: 52316 RVA: 0x004A919F File Offset: 0x004A739F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600CC5D RID: 52317 RVA: 0x004A91A8 File Offset: 0x004A73A8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionWaitForDead.PropTargetGroup))
			{
				this.targetGroup = properties.Values[ActionWaitForDead.PropTargetGroup];
			}
			properties.ParseInt(ActionWaitForDead.PropPhaseOnDespawn, ref this.phaseOnDespawn);
		}

		// Token: 0x0600CC5E RID: 52318 RVA: 0x004A91F5 File Offset: 0x004A73F5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionWaitForDead
			{
				targetGroup = this.targetGroup,
				phaseOnDespawn = this.phaseOnDespawn
			};
		}

		// Token: 0x04009AEB RID: 39659
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009AEC RID: 39660
		[PublicizedFrom(EAccessModifier.Protected)]
		public int phaseOnDespawn = -1;

		// Token: 0x04009AED RID: 39661
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009AEE RID: 39662
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDespawn = "phase_on_despawn";

		// Token: 0x04009AEF RID: 39663
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x04009AF0 RID: 39664
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
