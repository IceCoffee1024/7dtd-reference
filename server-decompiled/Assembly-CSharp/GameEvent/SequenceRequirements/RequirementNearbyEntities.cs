using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200193E RID: 6462
	[Preserve]
	public class RequirementNearbyEntities : BaseRequirement
	{
		// Token: 0x0600C73C RID: 51004 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C73D RID: 51005 RVA: 0x0049184C File Offset: 0x0048FA4C
		public override bool CanPerform(Entity target)
		{
			FastTags<TagGroup.Global> fastTags = ((this.tag == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.tag));
			List<Entity> entitiesInBounds = GameManager.Instance.World.GetEntitiesInBounds(target, new Bounds(target.position, Vector3.one * 2f * this.maxDistance), this.currentState == RequirementNearbyEntities.EntityStates.Live);
			if (this.targetIsOwner)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.GetPersistentPlayerList().GetPlayerDataFromEntityID(this.Owner.Target.entityId);
				for (int i = 0; i < entitiesInBounds.Count; i++)
				{
					if (entitiesInBounds[i].HasAnyTags(fastTags))
					{
						EntityVehicle entityVehicle = entitiesInBounds[i] as EntityVehicle;
						if (entityVehicle != null)
						{
							if (this.targetIsOwner && !entityVehicle.IsOwner(playerDataFromEntityID.PrimaryId))
							{
								goto IL_0106;
							}
						}
						else
						{
							EntityTurret entityTurret = entitiesInBounds[i] as EntityTurret;
							if (entityTurret == null || (this.targetIsOwner && entityTurret.OwnerID != null && !entityTurret.OwnerID.Equals(playerDataFromEntityID.PrimaryId)))
							{
								goto IL_0106;
							}
						}
						return true;
					}
					IL_0106:;
				}
			}
			else
			{
				for (int j = 0; j < entitiesInBounds.Count; j++)
				{
					Entity entity = entitiesInBounds[j];
					if (fastTags.IsEmpty)
					{
						if (entity is EntityAnimal || entity is EntityEnemy)
						{
							return true;
						}
					}
					else if (entity.HasAnyTags(fastTags))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C73E RID: 51006 RVA: 0x004919B7 File Offset: 0x0048FBB7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementNearbyEntities.PropTag, ref this.tag);
			properties.ParseFloat(RequirementNearbyEntities.PropMaxDistance, ref this.maxDistance);
			properties.ParseEnum<RequirementNearbyEntities.EntityStates>(RequirementNearbyEntities.PropEntityState, ref this.currentState);
		}

		// Token: 0x0600C73F RID: 51007 RVA: 0x004919F3 File Offset: 0x0048FBF3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementNearbyEntities
			{
				maxDistance = this.maxDistance,
				tag = this.tag,
				currentState = this.currentState
			};
		}

		// Token: 0x040095F9 RID: 38393
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x040095FA RID: 38394
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x040095FB RID: 38395
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x040095FC RID: 38396
		[PublicizedFrom(EAccessModifier.Protected)]
		public RequirementNearbyEntities.EntityStates currentState;

		// Token: 0x040095FD RID: 38397
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040095FE RID: 38398
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityState = "entity_state";

		// Token: 0x040095FF RID: 38399
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009600 RID: 38400
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x0200193F RID: 6463
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum EntityStates
		{
			// Token: 0x04009602 RID: 38402
			Live,
			// Token: 0x04009603 RID: 38403
			Dead
		}
	}
}
