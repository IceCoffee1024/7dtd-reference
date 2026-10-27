using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B6 RID: 6582
	[Preserve]
	public class ActionAddClosestEntityToGroup : BaseAction
	{
		// Token: 0x0600CA34 RID: 51764 RVA: 0x0049BF60 File Offset: 0x0049A160
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

		// Token: 0x0600CA35 RID: 51765 RVA: 0x0049C168 File Offset: 0x0049A368
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

		// Token: 0x0600CA36 RID: 51766 RVA: 0x0049C1E4 File Offset: 0x0049A3E4
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

		// Token: 0x04009827 RID: 38951
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009828 RID: 38952
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x04009829 RID: 38953
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x0400982A RID: 38954
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400982B RID: 38955
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x0400982C RID: 38956
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget = true;

		// Token: 0x0400982D RID: 38957
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400982E RID: 38958
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x0400982F RID: 38959
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009830 RID: 38960
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009831 RID: 38961
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x04009832 RID: 38962
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";
	}
}
