using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C4 RID: 6596
	[Preserve]
	public class ActionPullEntities : BaseAction
	{
		// Token: 0x0600CA2A RID: 51754 RVA: 0x004A0235 File Offset: 0x0049E435
		public override bool CanPerform(Entity player)
		{
			return GameManager.Instance.World.CanPlaceBlockAt(new Vector3i(player.position), null, false);
		}

		// Token: 0x0600CA2B RID: 51755 RVA: 0x004A0258 File Offset: 0x0049E458
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

		// Token: 0x0600CA2C RID: 51756 RVA: 0x004A03B4 File Offset: 0x0049E5B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionPullEntities.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(ActionPullEntities.PropPullSound, ref this.pullSound);
			properties.ParseFloat(ActionPullEntities.PropMinDistance, ref this.minDistance);
			properties.ParseFloat(ActionPullEntities.PropMaxDistance, ref this.maxDistance);
		}

		// Token: 0x0600CA2D RID: 51757 RVA: 0x004A040C File Offset: 0x0049E60C
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

		// Token: 0x040098E9 RID: 39145
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040098EA RID: 39146
		[PublicizedFrom(EAccessModifier.Protected)]
		public float minDistance = 7f;

		// Token: 0x040098EB RID: 39147
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 9f;

		// Token: 0x040098EC RID: 39148
		[PublicizedFrom(EAccessModifier.Protected)]
		public string pullSound = "";

		// Token: 0x040098ED RID: 39149
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040098EE RID: 39150
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinDistance = "min_distance";

		// Token: 0x040098EF RID: 39151
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040098F0 RID: 39152
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPullSound = "pull_sound";

		// Token: 0x040098F1 RID: 39153
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Entity> entityPullList;

		// Token: 0x040098F2 RID: 39154
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;
	}
}
