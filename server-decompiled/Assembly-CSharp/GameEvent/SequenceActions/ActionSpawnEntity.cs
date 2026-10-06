using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F3 RID: 6643
	[Preserve]
	public class ActionSpawnEntity : ActionBaseSpawn
	{
		// Token: 0x0600CB0B RID: 51979 RVA: 0x004A4478 File Offset: 0x004A2678
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

		// Token: 0x0600CB0C RID: 51980 RVA: 0x004A44D0 File Offset: 0x004A26D0
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

		// Token: 0x0600CB0D RID: 51981 RVA: 0x004A4534 File Offset: 0x004A2734
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

		// Token: 0x0600CB0E RID: 51982 RVA: 0x004A4588 File Offset: 0x004A2788
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

		// Token: 0x040099BF RID: 39359
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] AddBuffs;

		// Token: 0x040099C0 RID: 39360
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool onlyTargetPlayers = true;

		// Token: 0x040099C1 RID: 39361
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAddBuffs = "add_buffs";

		// Token: 0x040099C2 RID: 39362
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOnlyTargetPlayers = "only_target_players";
	}
}
