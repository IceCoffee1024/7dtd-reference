using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A1 RID: 6561
	[Preserve]
	public class ActionSpawnEntity : ActionBaseSpawn
	{
		// Token: 0x0600C82C RID: 51244 RVA: 0x00499C7C File Offset: 0x00497E7C
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

		// Token: 0x0600C82D RID: 51245 RVA: 0x00499CD4 File Offset: 0x00497ED4
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

		// Token: 0x0600C82E RID: 51246 RVA: 0x00499D38 File Offset: 0x00497F38
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

		// Token: 0x0600C82F RID: 51247 RVA: 0x00499D8C File Offset: 0x00497F8C
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

		// Token: 0x040097D0 RID: 38864
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddBuffs;

		// Token: 0x040097D1 RID: 38865
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool onlyTargetPlayers = true;

		// Token: 0x040097D2 RID: 38866
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddBuffs = "add_buffs";

		// Token: 0x040097D3 RID: 38867
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOnlyTargetPlayers = "only_target_players";
	}
}
