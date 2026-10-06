using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F4 RID: 6644
	[Preserve]
	public class ActionSpawnEntitySpawner : ActionSpawnEntity
	{
		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600CB11 RID: 51985 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool UseRepeating
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CB12 RID: 51986 RVA: 0x004A46D3 File Offset: 0x004A28D3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.AddToGroup = this.AddToGroup + "," + this.internalGroupName;
			base.OnInit();
		}

		// Token: 0x0600CB13 RID: 51987 RVA: 0x004A46F8 File Offset: 0x004A28F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleExtraAction()
		{
			if (this.spawnOnHit && base.Owner.EventVariables.EventVariables.ContainsKey("Damaged"))
			{
				this.newZombieNeeded += (int)base.Owner.EventVariables.EventVariables["Damaged"];
				base.Owner.EventVariables.EventVariables.Remove("Damaged");
			}
		}

		// Token: 0x0600CB14 RID: 51988 RVA: 0x004A4770 File Offset: 0x004A2970
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

		// Token: 0x0600CB15 RID: 51989 RVA: 0x004A47D0 File Offset: 0x004A29D0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSpawnEntitySpawner.PropSpawnerMin, ref this.spawnerMin);
			properties.ParseBool(ActionSpawnEntitySpawner.PropSpawnOnHit, ref this.spawnOnHit);
		}

		// Token: 0x0600CB16 RID: 51990 RVA: 0x004A47FC File Offset: 0x004A29FC
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

		// Token: 0x040099C3 RID: 39363
		public int spawnerMin = 5;

		// Token: 0x040099C4 RID: 39364
		public bool spawnOnHit = true;

		// Token: 0x040099C5 RID: 39365
		[PublicizedFrom(EAccessModifier.Private)]
		public string internalGroupName = "_spawner";

		// Token: 0x040099C6 RID: 39366
		[PublicizedFrom(EAccessModifier.Private)]
		public int newZombieNeeded;

		// Token: 0x040099C7 RID: 39367
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnerMin = "spawner_min";

		// Token: 0x040099C8 RID: 39368
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnOnHit = "spawn_on_hit";
	}
}
