using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001998 RID: 6552
	[Preserve]
	public class ActionPullEntities : BaseAction
	{
		// Token: 0x0600C8D2 RID: 51410 RVA: 0x0049BD71 File Offset: 0x00499F71
		public override bool CanPerform(Entity player)
		{
			return GameManager.Instance.World.CanPlaceBlockAt(new Vector3i(player.position), null, false);
		}

		// Token: 0x0600C8D3 RID: 51411 RVA: 0x0049BD94 File Offset: 0x00499F94
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

		// Token: 0x0600C8D4 RID: 51412 RVA: 0x0049BEF0 File Offset: 0x0049A0F0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPullEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionPullEntities.PropPullSound, ref this.pullSound);
			properties.ParseFloat(ActionPullEntities.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionPullEntities.PropMaxDistance, ref this.maxDistance);
		}

		// Token: 0x0600C8D5 RID: 51413 RVA: 0x0049BF48 File Offset: 0x0049A148
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

		// Token: 0x04009823 RID: 38947
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009824 RID: 38948
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 7f;

		// Token: 0x04009825 RID: 38949
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 9f;

		// Token: 0x04009826 RID: 38950
		[PublicizedFrom(EAccessModifier.Protected)]
		public string pullSound = "";

		// Token: 0x04009827 RID: 38951
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009828 RID: 38952
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x04009829 RID: 38953
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x0400982A RID: 38954
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPullSound = "pull_sound";

		// Token: 0x0400982B RID: 38955
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> entityPullList;

		// Token: 0x0400982C RID: 38956
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
