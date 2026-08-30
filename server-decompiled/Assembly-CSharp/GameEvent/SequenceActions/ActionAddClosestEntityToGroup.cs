using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001959 RID: 6489
	[Preserve]
	public class ActionAddClosestEntityToGroup : BaseAction
	{
		// Token: 0x0600C7C8 RID: 51144 RVA: 0x00494080 File Offset: 0x00492280
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.Parse(this.tag);
			List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(this.excludeTarget ? base.Owner.Target : null, new Bounds(base.Owner.Target.position, Vector3.one * 2f * this.maxDistance));
			List<Entity> list = new List<Entity>();
			Entity entity = null;
			float num = float.MaxValue;
			if (this.targetIsOwner)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(base.Owner.Target.entityId);
				for (int i = 0; i < entitiesInBounds.Count; i++)
				{
					if (entitiesInBounds[i].HasAnyTags(fastTags))
					{
						EntityVehicle entityVehicle = entitiesInBounds[i] as EntityVehicle;
						if (entityVehicle != null)
						{
							if (this.targetIsOwner && !entityVehicle.IsOwner(playerDataFromEntityID.PrimaryId))
							{
								goto IL_015D;
							}
						}
						else
						{
							EntityTurret entityTurret = entitiesInBounds[i] as EntityTurret;
							if (entityTurret == null || entityTurret.OwnerID == null || (this.targetIsOwner && entityTurret.OwnerID != null && !entityTurret.OwnerID.Equals(playerDataFromEntityID.PrimaryId)))
							{
								goto IL_015D;
							}
						}
						float num2 = Vector3.Distance(base.Owner.Target.position, entitiesInBounds[i].position);
						if (num2 < num)
						{
							num = num2;
							entity = entitiesInBounds[i];
						}
					}
					IL_015D:;
				}
			}
			else
			{
				for (int j = 0; j < entitiesInBounds.Count; j++)
				{
					if (entitiesInBounds[j].HasAnyTags(fastTags))
					{
						float num3 = Vector3.Distance(base.Owner.Target.position, entitiesInBounds[j].position);
						if (num3 < num)
						{
							num = num3;
							entity = entitiesInBounds[j];
						}
					}
				}
			}
			if (entity == null)
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			list.Add(entity);
			base.Owner.AddEntitiesToGroup(this.groupName, list, this.twitchNegative);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7C9 RID: 51145 RVA: 0x00494288 File Offset: 0x00492488
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddClosestEntityToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddClosestEntityToGroup.PropTag, ref this.tag);
			properties.ParseFloat(ActionAddClosestEntityToGroup.PropMaxDistance, ref this.maxDistance);
			properties.ParseBool(ActionAddClosestEntityToGroup.PropTwitchNegative, ref this.twitchNegative);
			properties.ParseBool(ActionAddClosestEntityToGroup.PropTargetIsOwner, ref this.targetIsOwner);
			properties.ParseBool(ActionAddClosestEntityToGroup.PropExcludeTarget, ref this.excludeTarget);
		}

		// Token: 0x0600C7CA RID: 51146 RVA: 0x00494304 File Offset: 0x00492504
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddClosestEntityToGroup
			{
				maxDistance = this.maxDistance,
				tag = this.tag,
				groupName = this.groupName,
				twitchNegative = this.twitchNegative,
				targetIsOwner = this.targetIsOwner,
				excludeTarget = this.excludeTarget
			};
		}

		// Token: 0x04009678 RID: 38520
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009679 RID: 38521
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x0400967A RID: 38522
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x0400967B RID: 38523
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400967C RID: 38524
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x0400967D RID: 38525
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget = true;

		// Token: 0x0400967E RID: 38526
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400967F RID: 38527
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x04009680 RID: 38528
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009681 RID: 38529
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009682 RID: 38530
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x04009683 RID: 38531
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";
	}
}
