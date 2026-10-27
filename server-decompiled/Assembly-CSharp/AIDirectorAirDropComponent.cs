using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000408 RID: 1032
[Preserve]
public class AIDirectorAirDropComponent : AIDirectorComponent
{
	// Token: 0x06001FC6 RID: 8134 RVA: 0x000C0AD5 File Offset: 0x000BECD5
	public override void Connect()
	{
		base.Connect();
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000C0AE0 File Offset: 0x000BECE0
	public override void InitNewGame()
	{
		base.InitNewGame();
		this.activeAirDrop = null;
		this.lastAirdropCheckTime = this.Director.World.worldTime;
		this.nextAirDropTime = this.calcNextAirdrop(this.lastAirdropCheckTime);
		this.supplyCrates.Clear();
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000C0B30 File Offset: 0x000BED30
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

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000C0B80 File Offset: 0x000BED80
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

	// Token: 0x06001FCA RID: 8138 RVA: 0x000C0C50 File Offset: 0x000BEE50
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

	// Token: 0x06001FCB RID: 8139 RVA: 0x000C0D20 File Offset: 0x000BEF20
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

	// Token: 0x06001FCC RID: 8140 RVA: 0x000C0E08 File Offset: 0x000BF008
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

	// Token: 0x06001FCD RID: 8141 RVA: 0x000C0EAC File Offset: 0x000BF0AC
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

	// Token: 0x06001FCE RID: 8142 RVA: 0x000C0F44 File Offset: 0x000BF144
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

	// Token: 0x06001FCF RID: 8143 RVA: 0x000C0FB8 File Offset: 0x000BF1B8
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

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000C10AC File Offset: 0x000BF2AC
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

	// Token: 0x06001FD1 RID: 8145 RVA: 0x000C1290 File Offset: 0x000BF490
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

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000C12FC File Offset: 0x000BF4FC
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

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000C1364 File Offset: 0x000BF564
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong calcNextAirdrop(ulong currentTime)
	{
		int num = GameUtils.WorldTimeToDays(currentTime) + base.Random.RandomRange(AIDirectorAirDropComponent.MinDayCount, AIDirectorAirDropComponent.MaxDayCount + 1) - 1;
		ulong num2 = (ulong)base.Random.RandomRange(AIDirectorAirDropComponent.MinTimeOfDay, AIDirectorAirDropComponent.MaxTimeOfDay + 1UL);
		ulong num3 = (ulong)((long)(num * 24000) + (long)num2);
		Log.Warning("Next Airdrop: " + GameUtils.WorldTimeToString(num3));
		return num3;
	}

	// Token: 0x0400158B RID: 5515
	public List<AIDirectorAirDropComponent.SupplyCrateCache> supplyCrates = new List<AIDirectorAirDropComponent.SupplyCrateCache>();

	// Token: 0x0400158C RID: 5516
	[PublicizedFrom(EAccessModifier.Private)]
	public AIAirDrop activeAirDrop;

	// Token: 0x0400158D RID: 5517
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAirDropTime;

	// Token: 0x0400158E RID: 5518
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastAirdropCheckTime;

	// Token: 0x0400158F RID: 5519
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastFrequency;

	// Token: 0x04001590 RID: 5520
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastID = -1;

	// Token: 0x04001591 RID: 5521
	public static ulong MinTimeOfDay = 12000UL;

	// Token: 0x04001592 RID: 5522
	public static ulong MaxTimeOfDay = 12000UL;

	// Token: 0x04001593 RID: 5523
	public static int MinDayCount = 3;

	// Token: 0x04001594 RID: 5524
	public static int MaxDayCount = 3;

	// Token: 0x04001595 RID: 5525
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NavObjectClass = "supply_drop";

	// Token: 0x04001596 RID: 5526
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] crateTypes = new string[] { "sc_General" };

	// Token: 0x02000409 RID: 1033
	[Preserve]
	public class SupplyCrateCache
	{
		// Token: 0x06001FD6 RID: 8150 RVA: 0x000C1420 File Offset: 0x000BF620
		public SupplyCrateCache(int id, Vector3i blockPos, bool requiresObserver)
		{
			this.entityId = id;
			this.blockPos = blockPos;
			this.requiresObserver = requiresObserver;
		}

		// Token: 0x04001597 RID: 5527
		public ChunkManager.ChunkObserver ChunkObserver;

		// Token: 0x04001598 RID: 5528
		public int entityId;

		// Token: 0x04001599 RID: 5529
		public Vector3i blockPos;

		// Token: 0x0400159A RID: 5530
		public bool requiresObserver;
	}

	// Token: 0x0200040A RID: 1034
	[StructLayout(LayoutKind.Explicit)]
	public struct UlongAsUshorts
	{
		// Token: 0x0400159B RID: 5531
		[FieldOffset(0)]
		public ulong Value;

		// Token: 0x0400159C RID: 5532
		[FieldOffset(0)]
		public ushort MinDays;

		// Token: 0x0400159D RID: 5533
		[FieldOffset(2)]
		public ushort MaxDays;

		// Token: 0x0400159E RID: 5534
		[FieldOffset(4)]
		public ushort MinTime;

		// Token: 0x0400159F RID: 5535
		[FieldOffset(6)]
		public ushort MaxTime;
	}
}
