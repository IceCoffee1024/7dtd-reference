using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A24 RID: 6692
	[Preserve]
	public class ActionSpawnEntity : ActionBaseSpawn
	{
		// Token: 0x0600CC1F RID: 52255 RVA: 0x004A7F74 File Offset: 0x004A6174
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

		// Token: 0x0600CC20 RID: 52256 RVA: 0x004A7FCC File Offset: 0x004A61CC
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

		// Token: 0x0600CC21 RID: 52257 RVA: 0x004A8030 File Offset: 0x004A6230
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

		// Token: 0x0600CC22 RID: 52258 RVA: 0x004A8084 File Offset: 0x004A6284
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

		// Token: 0x04009AA8 RID: 39592
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddBuffs;

		// Token: 0x04009AA9 RID: 39593
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool onlyTargetPlayers = true;

		// Token: 0x04009AAA RID: 39594
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddBuffs = "add_buffs";

		// Token: 0x04009AAB RID: 39595
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOnlyTargetPlayers = "only_target_players";
	}
}
