using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001934 RID: 6452
	[Preserve]
	public class ActionAddEntitiesToGroup : BaseAction
	{
		// Token: 0x0600C646 RID: 50758 RVA: 0x0048E07C File Offset: 0x0048C27C
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

		// Token: 0x0600C647 RID: 50759 RVA: 0x0048E304 File Offset: 0x0048C504
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

		// Token: 0x0600C648 RID: 50760 RVA: 0x0048E3B4 File Offset: 0x0048C5B4
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

		// Token: 0x0400955B RID: 38235
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400955C RID: 38236
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x0400955D RID: 38237
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x0400955E RID: 38238
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400955F RID: 38239
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x04009560 RID: 38240
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yHeight = -1f;

		// Token: 0x04009561 RID: 38241
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget = true;

		// Token: 0x04009562 RID: 38242
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool allowPlayers;

		// Token: 0x04009563 RID: 38243
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionAddEntitiesToGroup.EntityStates currentState;

		// Token: 0x04009564 RID: 38244
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009565 RID: 38245
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x04009566 RID: 38246
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityState = "entity_state";

		// Token: 0x04009567 RID: 38247
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009568 RID: 38248
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009569 RID: 38249
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x0400956A RID: 38250
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYHeight = "y_height";

		// Token: 0x0400956B RID: 38251
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";

		// Token: 0x0400956C RID: 38252
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAllowPlayers = "allow_players";

		// Token: 0x02001935 RID: 6453
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum EntityStates
		{
			// Token: 0x0400956E RID: 38254
			Live,
			// Token: 0x0400956F RID: 38255
			Dead
		}
	}
}
