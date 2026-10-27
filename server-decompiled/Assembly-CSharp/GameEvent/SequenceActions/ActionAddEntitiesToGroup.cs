using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B7 RID: 6583
	[Preserve]
	public class ActionAddEntitiesToGroup : BaseAction
	{
		// Token: 0x0600CA39 RID: 51769 RVA: 0x0049C2A8 File Offset: 0x0049A4A8
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

		// Token: 0x0600CA3A RID: 51770 RVA: 0x0049C530 File Offset: 0x0049A730
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

		// Token: 0x0600CA3B RID: 51771 RVA: 0x0049C5E0 File Offset: 0x0049A7E0
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

		// Token: 0x04009833 RID: 38963
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009834 RID: 38964
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x04009835 RID: 38965
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x04009836 RID: 38966
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009837 RID: 38967
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x04009838 RID: 38968
		[PublicizedFrom(EAccessModifier.Protected)]
		public float yHeight = -1f;

		// Token: 0x04009839 RID: 38969
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget = true;

		// Token: 0x0400983A RID: 38970
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool allowPlayers;

		// Token: 0x0400983B RID: 38971
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionAddEntitiesToGroup.EntityStates currentState;

		// Token: 0x0400983C RID: 38972
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400983D RID: 38973
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x0400983E RID: 38974
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityState = "entity_state";

		// Token: 0x0400983F RID: 38975
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009840 RID: 38976
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009841 RID: 38977
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x04009842 RID: 38978
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropYHeight = "y_height";

		// Token: 0x04009843 RID: 38979
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";

		// Token: 0x04009844 RID: 38980
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAllowPlayers = "allow_players";

		// Token: 0x020019B8 RID: 6584
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum EntityStates
		{
			// Token: 0x04009846 RID: 38982
			Live,
			// Token: 0x04009847 RID: 38983
			Dead
		}
	}
}
