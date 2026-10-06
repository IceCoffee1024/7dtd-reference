using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200196A RID: 6506
	[Preserve]
	public class RequirementNearbyEntities : BaseRequirement
	{
		// Token: 0x0600C894 RID: 51348 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C895 RID: 51349 RVA: 0x00495D38 File Offset: 0x00493F38
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

		// Token: 0x0600C896 RID: 51350 RVA: 0x00495EA3 File Offset: 0x004940A3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementNearbyEntities.PropTag, ref this.tag);
			properties.ParseFloat(RequirementNearbyEntities.PropMaxDistance, ref this.maxDistance);
			properties.ParseEnum<RequirementNearbyEntities.EntityStates>(RequirementNearbyEntities.PropEntityState, ref this.currentState);
		}

		// Token: 0x0600C897 RID: 51351 RVA: 0x00495EDF File Offset: 0x004940DF
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

		// Token: 0x040096BF RID: 38591
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x040096C0 RID: 38592
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 10f;

		// Token: 0x040096C1 RID: 38593
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool targetIsOwner;

		// Token: 0x040096C2 RID: 38594
		[PublicizedFrom(EAccessModifier.Protected)]
		public RequirementNearbyEntities.EntityStates currentState;

		// Token: 0x040096C3 RID: 38595
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";

		// Token: 0x040096C4 RID: 38596
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEntityState = "entity_state";

		// Token: 0x040096C5 RID: 38597
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040096C6 RID: 38598
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetIsOwner = "target_is_owner";

		// Token: 0x0200196B RID: 6507
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum EntityStates
		{
			// Token: 0x040096C8 RID: 38600
			Live,
			// Token: 0x040096C9 RID: 38601
			Dead
		}
	}
}
