using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001972 RID: 6514
	[Preserve]
	public class ActionPullEntities : BaseAction
	{
		// Token: 0x0600C74B RID: 51019 RVA: 0x00495A2D File Offset: 0x00493C2D
		public override bool CanPerform(Entity player)
		{
			return GameManager.Instance.World.CanPlaceBlockAt(new Vector3i(player.position), null, false);
		}

		// Token: 0x0600C74C RID: 51020 RVA: 0x00495A50 File Offset: 0x00493C50
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

		// Token: 0x0600C74D RID: 51021 RVA: 0x00495BAC File Offset: 0x00493DAC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPullEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionPullEntities.PropPullSound, ref this.pullSound);
			properties.ParseFloat(ActionPullEntities.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionPullEntities.PropMaxDistance, ref this.maxDistance);
		}

		// Token: 0x0600C74E RID: 51022 RVA: 0x00495C04 File Offset: 0x00493E04
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

		// Token: 0x040096FA RID: 38650
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040096FB RID: 38651
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 7f;

		// Token: 0x040096FC RID: 38652
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 9f;

		// Token: 0x040096FD RID: 38653
		[PublicizedFrom(EAccessModifier.Protected)]
		public string pullSound = "";

		// Token: 0x040096FE RID: 38654
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040096FF RID: 38655
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009700 RID: 38656
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009701 RID: 38657
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPullSound = "pull_sound";

		// Token: 0x04009702 RID: 38658
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> entityPullList;

		// Token: 0x04009703 RID: 38659
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
