using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F5 RID: 6645
	[Preserve]
	public class ActionPullEntities : BaseAction
	{
		// Token: 0x0600CB3E RID: 52030 RVA: 0x004A3D35 File Offset: 0x004A1F35
		public override bool CanPerform(Entity player)
		{
			return GameManager.Instance.World.CanPlaceBlockAt(new Vector3i(player.position), null, false);
		}

		// Token: 0x0600CB3F RID: 52031 RVA: 0x004A3D58 File Offset: 0x004A1F58
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.targetGroup != "")
			{
				if (this.entityPullList == null)
				{
					this.entityPullList = new List<Entity>();
					List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
					if (entityGroup != null)
					{
						this.entityPullList.AddRange(entityGroup);
						this.index = 0;
					}
					if (this.entityPullList.Count == 0)
					{
						return BaseAction.ActionCompleteStates.InCompleteRefund;
					}
				}
				else
				{
					Entity entity = this.entityPullList[this.index];
					if (entity.IsDead() || entity.IsDespawned)
					{
						this.index++;
						if (this.index >= this.entityPullList.Count)
						{
							return BaseAction.ActionCompleteStates.Complete;
						}
					}
					Vector3 zero = Vector3.zero;
					if (ActionBaseSpawn.FindValidPosition(out zero, base.Owner.Target, this.minDistance, this.maxDistance, false, 0f, false))
					{
						entity.SetPosition(zero, true);
						EntityAlive entityAlive = entity as EntityAlive;
						if (entityAlive != null)
						{
							EntityAlive entityAlive2 = base.Owner.Target as EntityAlive;
							if (entityAlive2 != null)
							{
								entityAlive.SetAttackTarget(entityAlive2, 12000);
							}
						}
						if (this.pullSound != "")
						{
							Manager.BroadcastPlayByLocalPlayer(zero, this.pullSound);
						}
						this.index++;
					}
					if (this.index >= this.entityPullList.Count)
					{
						return BaseAction.ActionCompleteStates.Complete;
					}
				}
				return BaseAction.ActionCompleteStates.InComplete;
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB40 RID: 52032 RVA: 0x004A3EB4 File Offset: 0x004A20B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPullEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionPullEntities.PropPullSound, ref this.pullSound);
			properties.ParseFloat(ActionPullEntities.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionPullEntities.PropMaxDistance, ref this.maxDistance);
		}

		// Token: 0x0600CB41 RID: 52033 RVA: 0x004A3F0C File Offset: 0x004A210C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionPullEntities
			{
				targetGroup = this.targetGroup,
				pullSound = this.pullSound,
				minDistance = this.minDistance,
				maxDistance = this.maxDistance
			};
		}

		// Token: 0x040099D2 RID: 39378
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040099D3 RID: 39379
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 7f;

		// Token: 0x040099D4 RID: 39380
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 9f;

		// Token: 0x040099D5 RID: 39381
		[PublicizedFrom(EAccessModifier.Protected)]
		public string pullSound = "";

		// Token: 0x040099D6 RID: 39382
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040099D7 RID: 39383
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040099D8 RID: 39384
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040099D9 RID: 39385
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPullSound = "pull_sound";

		// Token: 0x040099DA RID: 39386
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> entityPullList;

		// Token: 0x040099DB RID: 39387
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
