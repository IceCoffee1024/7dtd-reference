using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A02 RID: 6658
	[Preserve]
	public class ActionWaitForDead : BaseAction
	{
		// Token: 0x0600CB47 RID: 52039 RVA: 0x004A557C File Offset: 0x004A377C
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

		// Token: 0x0600CB48 RID: 52040 RVA: 0x004A56A3 File Offset: 0x004A38A3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.entityList = null;
		}

		// Token: 0x0600CB49 RID: 52041 RVA: 0x004A56AC File Offset: 0x004A38AC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionWaitForDead.PropTargetGroup))
			{
				this.targetGroup = properties.Values[ActionWaitForDead.PropTargetGroup];
			}
			properties.ParseInt(ActionWaitForDead.PropPhaseOnDespawn, ref this.phaseOnDespawn);
		}

		// Token: 0x0600CB4A RID: 52042 RVA: 0x004A56F9 File Offset: 0x004A38F9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionWaitForDead
			{
				targetGroup = this.targetGroup,
				phaseOnDespawn = this.phaseOnDespawn
			};
		}

		// Token: 0x04009A02 RID: 39426
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009A03 RID: 39427
		[PublicizedFrom(EAccessModifier.Protected)]
		public int phaseOnDespawn = -1;

		// Token: 0x04009A04 RID: 39428
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009A05 RID: 39429
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPhaseOnDespawn = "phase_on_despawn";

		// Token: 0x04009A06 RID: 39430
		[PublicizedFrom(EAccessModifier.Private)]
		public List<EntityAlive> entityList;

		// Token: 0x04009A07 RID: 39431
		[PublicizedFrom(EAccessModifier.Private)]
		public float checkTime = 1f;
	}
}
