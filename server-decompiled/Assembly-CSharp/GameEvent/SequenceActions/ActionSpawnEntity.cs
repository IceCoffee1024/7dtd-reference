using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C7 RID: 6599
	[Preserve]
	public class ActionSpawnEntity : ActionBaseSpawn
	{
		// Token: 0x0600C9B3 RID: 51635 RVA: 0x0049FFD4 File Offset: 0x0049E1D4
		public override void AddPropertiesToSpawnedEntity(Entity entity)
		{
			if (this.AddBuffs != null)
			{
				EntityAlive entityAlive = entity as EntityAlive;
				if (entityAlive == null)
				{
					return;
				}
				for (int i = 0; i < this.AddBuffs.Length; i++)
				{
					entityAlive.Buffs.AddBuff(this.AddBuffs[i], -1, true, false, -1f);
				}
			}
		}

		// Token: 0x0600C9B4 RID: 51636 RVA: 0x004A002C File Offset: 0x0049E22C
		public override void HandleTargeting(EntityAlive attacker, EntityAlive targetAlive)
		{
			base.HandleTargeting(attacker, targetAlive);
			attacker.SetMaxViewAngle(360f);
			attacker.sightRangeBase = 100f;
			attacker.SetSightLightThreshold(new Vector2(-2f, -2f));
			attacker.SetAttackTarget(targetAlive, 12000);
			if (this.onlyTargetPlayers)
			{
				attacker.aiManager.SetTargetOnlyPlayers(100f);
			}
		}

		// Token: 0x0600C9B5 RID: 51637 RVA: 0x004A0090 File Offset: 0x0049E290
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(ActionSpawnEntity.PropAddBuffs, ref text);
			if (text != "")
			{
				this.AddBuffs = text.Split(',', StringSplitOptions.None);
			}
			properties.ParseBool(ActionBaseSpawn.PropIsAggressive, ref this.isAggressive);
		}

		// Token: 0x0600C9B6 RID: 51638 RVA: 0x004A00E4 File Offset: 0x0049E2E4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSpawnEntity
			{
				count = this.count,
				currentCount = this.currentCount,
				entityNames = this.entityNames,
				maxDistance = this.maxDistance,
				minDistance = this.minDistance,
				safeSpawn = this.safeSpawn,
				airSpawn = this.airSpawn,
				singleChoice = this.singleChoice,
				targetGroup = this.targetGroup,
				partyAdditionText = this.partyAdditionText,
				AddToGroup = this.AddToGroup,
				AddToGroups = this.AddToGroups,
				AddBuffs = this.AddBuffs,
				spawnType = this.spawnType,
				clearPositionOnComplete = this.clearPositionOnComplete,
				yOffset = this.yOffset,
				attackTarget = this.attackTarget,
				useEntityGroup = this.useEntityGroup,
				ignoreMultiplier = this.ignoreMultiplier,
				onlyTargetPlayers = this.onlyTargetPlayers,
				raycastOffset = this.raycastOffset,
				isAggressive = this.isAggressive,
				spawnSound = this.spawnSound
			};
		}

		// Token: 0x040098F9 RID: 39161
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddBuffs;

		// Token: 0x040098FA RID: 39162
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool onlyTargetPlayers = true;

		// Token: 0x040098FB RID: 39163
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddBuffs = "add_buffs";

		// Token: 0x040098FC RID: 39164
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOnlyTargetPlayers = "only_target_players";
	}
}
