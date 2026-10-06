using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001985 RID: 6533
	[Preserve]
	public class ActionAddClosestEntityToGroup : BaseAction
	{
		// Token: 0x0600C920 RID: 51488 RVA: 0x0049856C File Offset: 0x0049676C
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

		// Token: 0x0600C921 RID: 51489 RVA: 0x00498774 File Offset: 0x00496974
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

		// Token: 0x0600C922 RID: 51490 RVA: 0x004987F0 File Offset: 0x004969F0
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

		// Token: 0x0400973E RID: 38718
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400973F RID: 38719
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag;

		// Token: 0x04009740 RID: 38720
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x04009741 RID: 38721
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009742 RID: 38722
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x04009743 RID: 38723
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget = true;

		// Token: 0x04009744 RID: 38724
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009745 RID: 38725
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x04009746 RID: 38726
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009747 RID: 38727
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009748 RID: 38728
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x04009749 RID: 38729
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";
	}
}
