using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A25 RID: 6693
	[Preserve]
	public class ActionSpawnEntitySpawner : ActionSpawnEntity
	{
		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x0600CC25 RID: 52261 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool UseRepeating
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CC26 RID: 52262 RVA: 0x004A81CF File Offset: 0x004A63CF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.AddToGroup = this.AddToGroup + "," + this.internalGroupName;
			base.OnInit();
		}

		// Token: 0x0600CC27 RID: 52263 RVA: 0x004A81F4 File Offset: 0x004A63F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleExtraAction()
		{
			if (this.spawnOnHit && base.Owner.EventVariables.EventVariables.ContainsKey("Damaged"))
			{
				this.newZombieNeeded += (int)base.Owner.EventVariables.EventVariables["Damaged"];
				base.Owner.EventVariables.EventVariables.Remove("Damaged");
			}
		}

		// Token: 0x0600CC28 RID: 52264 RVA: 0x004A826C File Offset: 0x004A646C
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

		// Token: 0x0600CC29 RID: 52265 RVA: 0x004A82CC File Offset: 0x004A64CC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSpawnEntitySpawner.PropSpawnerMin, ref this.spawnerMin);
			properties.ParseBool(ActionSpawnEntitySpawner.PropSpawnOnHit, ref this.spawnOnHit);
		}

		// Token: 0x0600CC2A RID: 52266 RVA: 0x004A82F8 File Offset: 0x004A64F8
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

		// Token: 0x04009AAC RID: 39596
		public int spawnerMin = 5;

		// Token: 0x04009AAD RID: 39597
		public bool spawnOnHit = true;

		// Token: 0x04009AAE RID: 39598
		[PublicizedFrom(EAccessModifier.Private)]
		public string internalGroupName = "_spawner";

		// Token: 0x04009AAF RID: 39599
		[PublicizedFrom(EAccessModifier.Private)]
		public int newZombieNeeded;

		// Token: 0x04009AB0 RID: 39600
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnerMin = "spawner_min";

		// Token: 0x04009AB1 RID: 39601
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSpawnOnHit = "spawn_on_hit";
	}
}
