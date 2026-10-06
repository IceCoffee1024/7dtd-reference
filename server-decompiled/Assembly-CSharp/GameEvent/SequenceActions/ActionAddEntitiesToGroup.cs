using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001986 RID: 6534
	[Preserve]
	public class ActionAddEntitiesToGroup : BaseAction
	{
		// Token: 0x0600C925 RID: 51493 RVA: 0x004988B4 File Offset: 0x00496AB4
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			FastTags<TagGroup.Global> fastTags = ((this.tag == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.tag));
			World world = GameManager.Instance.World;
			Vector3 vector = ((this.yHeight == -1f) ? (Vector3.one * 2f * this.maxDistance) : new Vector3(2f * this.maxDistance, this.yHeight, 2f * this.maxDistance));
			Vector3 vector2 = ((base.Owner.Target != null) ? base.Owner.Target.position : base.Owner.TargetPosition);
			if (this.yHeight != -1f)
			{
				vector2 += Vector3.one * (this.yHeight * 0.5f);
			}
			List<Entity> entitiesInBounds = world.GetEntitiesInBounds(this.excludeTarget ? base.Owner.Target : null, new Bounds(vector2, vector), this.currentState == ActionAddEntitiesToGroup.EntityStates.Live);
			List<Entity> list = new List<Entity>();
			if (this.targetIsOwner && base.Owner.Target != null)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(base.Owner.Target.entityId);
				for (int i = 0; i < entitiesInBounds.Count; i++)
				{
					if (entitiesInBounds[i].HasAnyTags(fastTags))
					{
						EntityVehicle entityVehicle = entitiesInBounds[i] as EntityVehicle;
						if (entityVehicle != null)
						{
							if (entityVehicle.GetOwner() == null)
							{
								goto IL_01E2;
							}
							if (this.targetIsOwner && !entityVehicle.IsOwner(playerDataFromEntityID.PrimaryId))
							{
								goto IL_01E2;
							}
						}
						else
						{
							EntityTurret entityTurret = entitiesInBounds[i] as EntityTurret;
							if (entityTurret == null || entityTurret.OwnerID == null || (this.targetIsOwner && !entityTurret.OwnerID.Equals(playerDataFromEntityID.PrimaryId)))
							{
								goto IL_01E2;
							}
						}
						list.Add(entitiesInBounds[i]);
					}
					IL_01E2:;
				}
			}
			else
			{
				for (int j = 0; j < entitiesInBounds.Count; j++)
				{
					Entity entity = entitiesInBounds[j];
					if (fastTags.IsEmpty)
					{
						if (entity is EntityEnemyAnimal || entity is EntityEnemy || (this.allowPlayers && entity is EntityPlayer))
						{
							list.Add(entity);
						}
					}
					else if (entity.HasAnyTags(fastTags))
					{
						list.Add(entity);
					}
				}
			}
			base.Owner.AddEntitiesToGroup(this.groupName, list, this.twitchNegative);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C926 RID: 51494 RVA: 0x00498B3C File Offset: 0x00496D3C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddEntitiesToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddEntitiesToGroup.PropTag, ref this.tag);
			properties.ParseFloat(ActionAddEntitiesToGroup.PropMaxDistance, ref this.maxDistance);
			properties.ParseBool(ActionAddEntitiesToGroup.PropTwitchNegative, ref this.twitchNegative);
			properties.ParseBool(ActionAddEntitiesToGroup.PropTargetIsOwner, ref this.targetIsOwner);
			properties.ParseBool(ActionAddEntitiesToGroup.PropExcludeTarget, ref this.excludeTarget);
			properties.ParseBool(ActionAddEntitiesToGroup.PropAllowPlayers, ref this.allowPlayers);
			properties.ParseFloat(ActionAddEntitiesToGroup.PropYHeight, ref this.yHeight);
			properties.ParseEnum<ActionAddEntitiesToGroup.EntityStates>(ActionAddEntitiesToGroup.PropEntityState, ref this.currentState);
		}

		// Token: 0x0600C927 RID: 51495 RVA: 0x00498BEC File Offset: 0x00496DEC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddEntitiesToGroup
			{
				maxDistance = this.maxDistance,
				tag = this.tag,
				groupName = this.groupName,
				twitchNegative = this.twitchNegative,
				targetIsOwner = this.targetIsOwner,
				yHeight = this.yHeight,
				currentState = this.currentState,
				excludeTarget = this.excludeTarget,
				allowPlayers = this.allowPlayers
			};
		}

		// Token: 0x0400974A RID: 38730
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400974B RID: 38731
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x0400974C RID: 38732
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x0400974D RID: 38733
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400974E RID: 38734
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x0400974F RID: 38735
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yHeight = -1f;

		// Token: 0x04009750 RID: 38736
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget = true;

		// Token: 0x04009751 RID: 38737
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool allowPlayers;

		// Token: 0x04009752 RID: 38738
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionAddEntitiesToGroup.EntityStates currentState;

		// Token: 0x04009753 RID: 38739
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009754 RID: 38740
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x04009755 RID: 38741
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityState = "entity_state";

		// Token: 0x04009756 RID: 38742
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009757 RID: 38743
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009758 RID: 38744
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x04009759 RID: 38745
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYHeight = "y_height";

		// Token: 0x0400975A RID: 38746
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";

		// Token: 0x0400975B RID: 38747
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAllowPlayers = "allow_players";

		// Token: 0x02001987 RID: 6535
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum EntityStates
		{
			// Token: 0x0400975D RID: 38749
			Live,
			// Token: 0x0400975E RID: 38750
			Dead
		}
	}
}
