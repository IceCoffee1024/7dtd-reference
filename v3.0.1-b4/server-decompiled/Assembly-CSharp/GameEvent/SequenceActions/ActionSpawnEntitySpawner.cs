using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A2 RID: 6562
	[Preserve]
	public class ActionSpawnEntitySpawner : ActionSpawnEntity
	{
		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x0600C832 RID: 51250 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool UseRepeating
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600C833 RID: 51251 RVA: 0x00499ED7 File Offset: 0x004980D7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.AddToGroup = this.AddToGroup + "," + this.internalGroupName;
			base.OnInit();
		}

		// Token: 0x0600C834 RID: 51252 RVA: 0x00499EFC File Offset: 0x004980FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleExtraAction()
		{
			if (this.spawnOnHit && base.Owner.EventVariables.EventVariables.ContainsKey("Damaged"))
			{
				this.newZombieNeeded += (int)base.Owner.EventVariables.EventVariables["Damaged"];
				base.Owner.EventVariables.EventVariables.Remove("Damaged");
			}
		}

		// Token: 0x0600C835 RID: 51253 RVA: 0x00499F74 File Offset: 0x00498174
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

		// Token: 0x0600C836 RID: 51254 RVA: 0x00499FD4 File Offset: 0x004981D4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSpawnEntitySpawner.PropSpawnerMin, ref this.spawnerMin);
			properties.ParseBool(ActionSpawnEntitySpawner.PropSpawnOnHit, ref this.spawnOnHit);
		}

		// Token: 0x0600C837 RID: 51255 RVA: 0x0049A000 File Offset: 0x00498200
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

		// Token: 0x040097D4 RID: 38868
		public int spawnerMin = 5;

		// Token: 0x040097D5 RID: 38869
		public bool spawnOnHit = true;

		// Token: 0x040097D6 RID: 38870
		[PublicizedFrom(EAccessModifier.Private)]
		public string internalGroupName = "_spawner";

		// Token: 0x040097D7 RID: 38871
		[PublicizedFrom(EAccessModifier.Private)]
		public int newZombieNeeded;

		// Token: 0x040097D8 RID: 38872
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnerMin = "spawner_min";

		// Token: 0x040097D9 RID: 38873
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnOnHit = "spawn_on_hit";
	}
}
