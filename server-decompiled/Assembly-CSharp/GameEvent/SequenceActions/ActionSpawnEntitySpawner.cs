using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C8 RID: 6600
	[Preserve]
	public class ActionSpawnEntitySpawner : ActionSpawnEntity
	{
		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x0600C9B9 RID: 51641 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool UseRepeating
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600C9BA RID: 51642 RVA: 0x004A022F File Offset: 0x0049E42F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.AddToGroup = this.AddToGroup + "," + this.internalGroupName;
			base.OnInit();
		}

		// Token: 0x0600C9BB RID: 51643 RVA: 0x004A0254 File Offset: 0x0049E454
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleExtraAction()
		{
			if (this.spawnOnHit && base.Owner.EventVariables.EventVariables.ContainsKey("Damaged"))
			{
				this.newZombieNeeded += (int)base.Owner.EventVariables.EventVariables["Damaged"];
				base.Owner.EventVariables.EventVariables.Remove("Damaged");
			}
		}

		// Token: 0x0600C9BC RID: 51644 RVA: 0x004A02CC File Offset: 0x0049E4CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleRepeat()
		{
			EntityPlayer entityPlayer = base.Owner.Target as EntityPlayer;
			if (entityPlayer == null)
			{
				return false;
			}
			if (base.Owner.GetEntityGroupLiveCount(this.internalGroupName) < this.spawnerMin + base.GetPartyAdditionCount(entityPlayer))
			{
				return true;
			}
			if (this.newZombieNeeded > 0)
			{
				this.newZombieNeeded--;
				return true;
			}
			return false;
		}

		// Token: 0x0600C9BD RID: 51645 RVA: 0x004A032C File Offset: 0x0049E52C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSpawnEntitySpawner.PropSpawnerMin, ref this.spawnerMin);
			properties.ParseBool(ActionSpawnEntitySpawner.PropSpawnOnHit, ref this.spawnOnHit);
		}

		// Token: 0x0600C9BE RID: 51646 RVA: 0x004A0358 File Offset: 0x0049E558
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSpawnEntitySpawner
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
				spawnerMin = this.spawnerMin,
				spawnOnHit = this.spawnOnHit,
				spawnSound = this.spawnSound
			};
		}

		// Token: 0x040098FD RID: 39165
		public int spawnerMin = 5;

		// Token: 0x040098FE RID: 39166
		public bool spawnOnHit = true;

		// Token: 0x040098FF RID: 39167
		[PublicizedFrom(EAccessModifier.Private)]
		public string internalGroupName = "_spawner";

		// Token: 0x04009900 RID: 39168
		[PublicizedFrom(EAccessModifier.Private)]
		public int newZombieNeeded;

		// Token: 0x04009901 RID: 39169
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnerMin = "spawner_min";

		// Token: 0x04009902 RID: 39170
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnOnHit = "spawn_on_hit";
	}
}
