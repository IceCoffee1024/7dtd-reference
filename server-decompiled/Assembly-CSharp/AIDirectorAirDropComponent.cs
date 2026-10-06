using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000406 RID: 1030
[Preserve]
public class AIDirectorAirDropComponent : AIDirectorComponent
{
	// Token: 0x06001FC1 RID: 8129 RVA: 0x000C0729 File Offset: 0x000BE929
	public override void Connect()
	{
		base.Connect();
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000C0734 File Offset: 0x000BE934
	public override void InitNewGame()
	{
		base.InitNewGame();
		this.activeAirDrop = null;
		this.lastAirdropCheckTime = this.Director.World.worldTime;
		this.nextAirDropTime = this.calcNextAirdrop(this.lastAirdropCheckTime);
		this.supplyCrates.Clear();
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000C0784 File Offset: 0x000BE984
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong packDropFrequency()
	{
		return new AIDirectorAirDropComponent.UlongAsUshorts
		{
			MinDays = (ushort)AIDirectorAirDropComponent.MinDayCount,
			MaxDays = (ushort)AIDirectorAirDropComponent.MaxDayCount,
			MinTime = (ushort)AIDirectorAirDropComponent.MinTimeOfDay,
			MaxTime = (ushort)AIDirectorAirDropComponent.MaxTimeOfDay
		}.Value;
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000C07D4 File Offset: 0x000BE9D4
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		if (this.activeAirDrop != null)
		{
			if (this.activeAirDrop.Tick((float)_dt))
			{
				this.activeAirDrop = null;
				return;
			}
		}
		else
		{
			ulong num = this.packDropFrequency();
			ulong worldTime = this.Director.World.worldTime;
			if (!GameUtils.IsPlaytesting() && !this.Director.World.IsEditor() && num > 0UL && !LootContainer.NoLoot && AIDirectorAirDropComponent.MaxDayCount != 0)
			{
				if (worldTime >= this.nextAirDropTime)
				{
					if (this.SpawnAirDrop())
					{
						this.lastAirdropCheckTime = worldTime;
						this.nextAirDropTime = this.calcNextAirdrop(worldTime);
						return;
					}
				}
				else if (num != this.lastFrequency || worldTime < this.lastAirdropCheckTime)
				{
					this.nextAirDropTime = this.calcNextAirdrop(worldTime);
					this.lastFrequency = num;
					this.lastAirdropCheckTime = worldTime;
				}
			}
		}
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000C08A4 File Offset: 0x000BEAA4
	public override void Read(PooledBinaryReader _stream, int _version)
	{
		base.Read(_stream, _version);
		this.nextAirDropTime = _stream.ReadUInt64();
		if (_version >= 9)
		{
			this.lastFrequency = _stream.ReadUInt64();
		}
		else
		{
			this.lastFrequency = this.packDropFrequency();
		}
		this.supplyCrates.Clear();
		int num = _stream.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			int num2 = _stream.ReadInt32();
			if (num2 > this.lastID)
			{
				this.lastID = num2;
			}
			Vector3i vector3i = StreamUtils.ReadVector3i(_stream);
			bool flag = false;
			if (_version >= 10)
			{
				flag = _stream.ReadBoolean();
			}
			AIDirectorAirDropComponent.SupplyCrateCache supplyCrateCache = new AIDirectorAirDropComponent.SupplyCrateCache(num2, vector3i, flag);
			if (flag)
			{
				supplyCrateCache.ChunkObserver = this.Director.World.GetGameManager().AddChunkObserver(vector3i, false, 3, -1);
			}
			this.AddSupplyCrate(supplyCrateCache);
		}
		this.RefreshCrates(-1);
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000C0974 File Offset: 0x000BEB74
	public override void Write(PooledBinaryWriter _stream)
	{
		base.Write(_stream);
		_stream.Write(this.nextAirDropTime);
		_stream.Write(this.lastFrequency);
		_stream.Write(this.supplyCrates.Count);
		for (int i = 0; i < this.supplyCrates.Count; i++)
		{
			_stream.Write(this.supplyCrates[i].entityId);
			StreamUtils.Write(_stream, this.supplyCrates[i].blockPos);
			EntitySupplyCrate entitySupplyCrate = this.Director.World.GetEntity(this.supplyCrates[i].entityId) as EntitySupplyCrate;
			if (entitySupplyCrate != null)
			{
				this.supplyCrates[i].requiresObserver = entitySupplyCrate.RequiresChunkObserver();
			}
			_stream.Write(this.supplyCrates[i].requiresObserver);
		}
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000C0A5C File Offset: 0x000BEC5C
	public bool SpawnAirDrop()
	{
		bool flag = false;
		if (LootContainer.NoLoot || AIDirectorAirDropComponent.MaxDayCount == 0)
		{
			return true;
		}
		if (this.activeAirDrop == null)
		{
			List<EntityPlayer> list = new List<EntityPlayer>();
			DictionaryList<int, AIDirectorPlayerState> trackedPlayers = this.Director.GetComponent<AIDirectorPlayerManagementComponent>().trackedPlayers;
			for (int i = 0; i < trackedPlayers.list.Count; i++)
			{
				AIDirectorPlayerState aidirectorPlayerState = trackedPlayers.list[i];
				if (!aidirectorPlayerState.Player.IsDead())
				{
					list.Add(aidirectorPlayerState.Player);
				}
			}
			if (list.Count > 0)
			{
				this.activeAirDrop = new AIAirDrop(this, this.Director.World, list);
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000C0B00 File Offset: 0x000BED00
	public void RemoveSupplyCrate(int entityId)
	{
		int num = -1;
		for (int i = 0; i < this.supplyCrates.Count; i++)
		{
			if (this.supplyCrates[i].entityId == entityId)
			{
				num = i;
				break;
			}
		}
		if (num > -1)
		{
			this.supplyCrates.RemoveAt(num);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup(entityId), false, -1, -1, -1, null, 192, false);
			return;
		}
		Log.Warning(string.Format("{0} AIDirectorAirDropComponent: Attempted to remove supply crate cache with missing entityID {1}", GameManager.frameCount, entityId));
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000C0B98 File Offset: 0x000BED98
	public void SetSupplyCratePosition(int entityId, Vector3i blockPos)
	{
		foreach (AIDirectorAirDropComponent.SupplyCrateCache supplyCrateCache in this.supplyCrates)
		{
			if (supplyCrateCache.entityId == entityId)
			{
				supplyCrateCache.blockPos = blockPos;
				return;
			}
		}
		Log.Warning(string.Format("Supply crate {0} not in the list, can't set position", entityId));
	}

	// Token: 0x06001FCA RID: 8138 RVA: 0x000C0C0C File Offset: 0x000BEE0C
	public EntitySupplyCrate SpawnSupplyCrate(Vector3 spawnPos, ChunkManager.ChunkObserver chunkObserver)
	{
		if (this.Director.World == null)
		{
			return null;
		}
		if (this.supplyCrates.Count >= 12)
		{
			Entity entity = this.Director.World.GetEntity(this.supplyCrates[0].entityId);
			if (entity != null)
			{
				entity.MarkToUnload();
			}
			this.supplyCrates.RemoveAt(0);
		}
		Entity entity2 = EntityFactory.CreateEntity(EntityClass.FromString(AIDirectorAirDropComponent.crateTypes[base.Random.RandomRange(0, AIDirectorAirDropComponent.crateTypes.Length)]), spawnPos, new Vector3(base.Random.RandomFloat * 360f, 0f, 0f));
		this.Director.World.SpawnEntityInWorld(entity2);
		this.AddSupplyCrate(new AIDirectorAirDropComponent.SupplyCrateCache(entity2.entityId, World.worldToBlockPos(entity2.position), true)
		{
			ChunkObserver = chunkObserver
		});
		this.RefreshCrates(-1);
		return entity2 as EntitySupplyCrate;
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x000C0D00 File Offset: 0x000BEF00
	public void RefreshCrates(int _shareWithClient = -1)
	{
		foreach (AIDirectorAirDropComponent.SupplyCrateCache supplyCrateCache in this.supplyCrates)
		{
			if (supplyCrateCache.requiresObserver)
			{
				EntitySupplyCrate entitySupplyCrate = this.Director.World.GetEntity(supplyCrateCache.entityId) as EntitySupplyCrate;
				if (entitySupplyCrate != null)
				{
					supplyCrateCache.requiresObserver = entitySupplyCrate.RequiresChunkObserver();
				}
			}
			if (!supplyCrateCache.requiresObserver && supplyCrateCache.ChunkObserver != null)
			{
				this.Director.World.GetGameManager().RemoveChunkObserver(supplyCrateCache.ChunkObserver);
				supplyCrateCache.ChunkObserver = null;
			}
			if (GameStats.GetBool(EnumGameStats.AirDropMarker))
			{
				NavObject navObject = ((supplyCrateCache.entityId == -1) ? null : NavObjectManager.Instance.GetNavObjectByEntityID(supplyCrateCache.entityId));
				if (navObject != null && navObject.TrackType == NavObject.TrackTypes.Entity)
				{
					if (_shareWithClient != -1)
					{
						Vector3 vector = navObject.GetPosition() + Origin.position;
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup(navObject.NavObjectClass.NavObjectClassName, navObject.DisplayName, vector, true, navObject.usingLocalizationId, supplyCrateCache.entityId), false, -1, -1, -1, null, 192, false);
					}
				}
				else
				{
					navObject = NavObjectManager.Instance.RegisterNavObject("supply_drop", supplyCrateCache.blockPos, "", false, -1, null);
					navObject.EntityID = supplyCrateCache.entityId;
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNavObject>().Setup(navObject.NavObjectClass.NavObjectClassName, navObject.DisplayName, supplyCrateCache.blockPos, true, navObject.usingLocalizationId, supplyCrateCache.entityId), false, -1, -1, -1, null, 192, false);
				}
			}
		}
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x000C0EE4 File Offset: 0x000BF0E4
	public void AddSupplyCrate(int entityId)
	{
		using (List<AIDirectorAirDropComponent.SupplyCrateCache>.Enumerator enumerator = this.supplyCrates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.entityId == entityId)
				{
					return;
				}
			}
		}
		this.supplyCrates.Add(new AIDirectorAirDropComponent.SupplyCrateCache(entityId, Vector3i.zero, false));
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000C0F50 File Offset: 0x000BF150
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddSupplyCrate(AIDirectorAirDropComponent.SupplyCrateCache scc)
	{
		using (List<AIDirectorAirDropComponent.SupplyCrateCache>.Enumerator enumerator = this.supplyCrates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.entityId == scc.entityId)
				{
					return;
				}
			}
		}
		this.supplyCrates.Add(scc);
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x000C0FB8 File Offset: 0x000BF1B8
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong calcNextAirdrop(ulong currentTime)
	{
		int num = GameUtils.WorldTimeToDays(currentTime) + base.Random.RandomRange(AIDirectorAirDropComponent.MinDayCount, AIDirectorAirDropComponent.MaxDayCount + 1) - 1;
		ulong num2 = (ulong)base.Random.RandomRange(AIDirectorAirDropComponent.MinTimeOfDay, AIDirectorAirDropComponent.MaxTimeOfDay + 1UL);
		ulong num3 = (ulong)((long)(num * 24000) + (long)num2);
		Log.Warning("Next Airdrop: " + GameUtils.WorldTimeToString(num3));
		return num3;
	}

	// Token: 0x0400157B RID: 5499
	public List<AIDirectorAirDropComponent.SupplyCrateCache> supplyCrates = new List<AIDirectorAirDropComponent.SupplyCrateCache>();

	// Token: 0x0400157C RID: 5500
	[PublicizedFrom(EAccessModifier.Private)]
	public AIAirDrop activeAirDrop;

	// Token: 0x0400157D RID: 5501
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAirDropTime;

	// Token: 0x0400157E RID: 5502
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastAirdropCheckTime;

	// Token: 0x0400157F RID: 5503
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastFrequency;

	// Token: 0x04001580 RID: 5504
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastID = -1;

	// Token: 0x04001581 RID: 5505
	public static ulong MinTimeOfDay = 12000UL;

	// Token: 0x04001582 RID: 5506
	public static ulong MaxTimeOfDay = 12000UL;

	// Token: 0x04001583 RID: 5507
	public static int MinDayCount = 3;

	// Token: 0x04001584 RID: 5508
	public static int MaxDayCount = 3;

	// Token: 0x04001585 RID: 5509
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NavObjectClass = "supply_drop";

	// Token: 0x04001586 RID: 5510
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] crateTypes = new string[] { "sc_General" };

	// Token: 0x02000407 RID: 1031
	[Preserve]
	public class SupplyCrateCache
	{
		// Token: 0x06001FD1 RID: 8145 RVA: 0x000C1074 File Offset: 0x000BF274
		public SupplyCrateCache(int id, Vector3i blockPos, bool requiresObserver)
		{
			this.entityId = id;
			this.blockPos = blockPos;
			this.requiresObserver = requiresObserver;
		}

		// Token: 0x04001587 RID: 5511
		public ChunkManager.ChunkObserver ChunkObserver;

		// Token: 0x04001588 RID: 5512
		public int entityId;

		// Token: 0x04001589 RID: 5513
		public Vector3i blockPos;

		// Token: 0x0400158A RID: 5514
		public bool requiresObserver;
	}

	// Token: 0x02000408 RID: 1032
	[StructLayout(LayoutKind.Explicit)]
	public struct UlongAsUshorts
	{
		// Token: 0x0400158B RID: 5515
		[FieldOffset(0)]
		public ulong Value;

		// Token: 0x0400158C RID: 5516
		[FieldOffset(0)]
		public ushort MinDays;

		// Token: 0x0400158D RID: 5517
		[FieldOffset(2)]
		public ushort MaxDays;

		// Token: 0x0400158E RID: 5518
		[FieldOffset(4)]
		public ushort MinTime;

		// Token: 0x0400158F RID: 5519
		[FieldOffset(6)]
		public ushort MaxTime;
	}
}
