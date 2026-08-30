using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001918 RID: 6424
	[Preserve]
	public class RequirementNearbyEntities : BaseRequirement
	{
		// Token: 0x0600C5B5 RID: 50613 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5B6 RID: 50614 RVA: 0x0048B500 File Offset: 0x00489700
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

		// Token: 0x0600C5B7 RID: 50615 RVA: 0x0048B66B File Offset: 0x0048986B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementNearbyEntities.PropTag, ref this.tag);
			properties.ParseFloat(RequirementNearbyEntities.PropMaxDistance, ref this.maxDistance);
			properties.ParseEnum<RequirementNearbyEntities.EntityStates>(RequirementNearbyEntities.PropEntityState, ref this.currentState);
		}

		// Token: 0x0600C5B8 RID: 50616 RVA: 0x0048B6A7 File Offset: 0x004898A7
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

		// Token: 0x040094D0 RID: 38096
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x040094D1 RID: 38097
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x040094D2 RID: 38098
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x040094D3 RID: 38099
		[PublicizedFrom(EAccessModifier.Protected)]
		public RequirementNearbyEntities.EntityStates currentState;

		// Token: 0x040094D4 RID: 38100
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040094D5 RID: 38101
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityState = "entity_state";

		// Token: 0x040094D6 RID: 38102
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040094D7 RID: 38103
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x02001919 RID: 6425
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum EntityStates
		{
			// Token: 0x040094D9 RID: 38105
			Live,
			// Token: 0x040094DA RID: 38106
			Dead
		}
	}
}
