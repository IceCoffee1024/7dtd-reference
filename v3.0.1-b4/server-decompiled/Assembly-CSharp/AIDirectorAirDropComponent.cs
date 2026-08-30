using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000400 RID: 1024
[Preserve]
public class AIDirectorAirDropComponent : AIDirectorComponent
{
	// Token: 0x06001FB5 RID: 8117 RVA: 0x000BFF09 File Offset: 0x000BE109
	public override void Connect()
	{
		base.Connect();
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000BFF14 File Offset: 0x000BE114
	public override void InitNewGame()
	{
		base.InitNewGame();
		this.activeAirDrop = null;
		this.lastAirdropCheckTime = this.Director.World.worldTime;
		this.nextAirDropTime = this.calcNextAirdrop(this.lastAirdropCheckTime);
		this.supplyCrates.Clear();
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000BFF64 File Offset: 0x000BE164
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

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000BFFB4 File Offset: 0x000BE1B4
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

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000C0084 File Offset: 0x000BE284
	public override void Read(BinaryReader _stream, int _version)
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

	// Token: 0x06001FBA RID: 8122 RVA: 0x000C0154 File Offset: 0x000BE354
	public override void Write(BinaryWriter _stream)
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

	// Token: 0x06001FBB RID: 8123 RVA: 0x000C023C File Offset: 0x000BE43C
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

	// Token: 0x06001FBC RID: 8124 RVA: 0x000C02E0 File Offset: 0x000BE4E0
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

	// Token: 0x06001FBD RID: 8125 RVA: 0x000C0378 File Offset: 0x000BE578
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

	// Token: 0x06001FBE RID: 8126 RVA: 0x000C03EC File Offset: 0x000BE5EC
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

	// Token: 0x06001FBF RID: 8127 RVA: 0x000C04E0 File Offset: 0x000BE6E0
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

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000C06C4 File Offset: 0x000BE8C4
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

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000C0730 File Offset: 0x000BE930
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

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000C0798 File Offset: 0x000BE998
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong calcNextAirdrop(ulong currentTime)
	{
		int num = GameUtils.WorldTimeToDays(currentTime) + base.Random.RandomRange(AIDirectorAirDropComponent.MinDayCount, AIDirectorAirDropComponent.MaxDayCount + 1) - 1;
		ulong num2 = (ulong)base.Random.RandomRange(AIDirectorAirDropComponent.MinTimeOfDay, AIDirectorAirDropComponent.MaxTimeOfDay + 1UL);
		ulong num3 = (ulong)((long)(num * 24000) + (long)num2);
		Log.Warning("Next Airdrop: " + GameUtils.WorldTimeToString(num3));
		return num3;
	}

	// Token: 0x0400156F RID: 5487
	public List<AIDirectorAirDropComponent.SupplyCrateCache> supplyCrates = new List<AIDirectorAirDropComponent.SupplyCrateCache>();

	// Token: 0x04001570 RID: 5488
	[PublicizedFrom(EAccessModifier.Private)]
	public AIAirDrop activeAirDrop;

	// Token: 0x04001571 RID: 5489
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAirDropTime;

	// Token: 0x04001572 RID: 5490
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastAirdropCheckTime;

	// Token: 0x04001573 RID: 5491
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastFrequency;

	// Token: 0x04001574 RID: 5492
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastID = -1;

	// Token: 0x04001575 RID: 5493
	public static ulong MinTimeOfDay = 12000UL;

	// Token: 0x04001576 RID: 5494
	public static ulong MaxTimeOfDay = 12000UL;

	// Token: 0x04001577 RID: 5495
	public static int MinDayCount = 3;

	// Token: 0x04001578 RID: 5496
	public static int MaxDayCount = 3;

	// Token: 0x04001579 RID: 5497
	[PublicizedFrom(EAccessModifier.Private)]
	public const string NavObjectClass = "supply_drop";

	// Token: 0x0400157A RID: 5498
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] crateTypes = new string[] { "sc_General" };

	// Token: 0x02000401 RID: 1025
	[Preserve]
	public class SupplyCrateCache
	{
		// Token: 0x06001FC5 RID: 8133 RVA: 0x000C0854 File Offset: 0x000BEA54
		public SupplyCrateCache(int id, Vector3i blockPos, bool requiresObserver)
		{
			this.entityId = id;
			this.blockPos = blockPos;
			this.requiresObserver = requiresObserver;
		}

		// Token: 0x0400157B RID: 5499
		public ChunkManager.ChunkObserver ChunkObserver;

		// Token: 0x0400157C RID: 5500
		public int entityId;

		// Token: 0x0400157D RID: 5501
		public Vector3i blockPos;

		// Token: 0x0400157E RID: 5502
		public bool requiresObserver;
	}

	// Token: 0x02000402 RID: 1026
	[StructLayout(LayoutKind.Explicit)]
	public struct UlongAsUshorts
	{
		// Token: 0x0400157F RID: 5503
		[FieldOffset(0)]
		public ulong Value;

		// Token: 0x04001580 RID: 5504
		[FieldOffset(0)]
		public ushort MinDays;

		// Token: 0x04001581 RID: 5505
		[FieldOffset(2)]
		public ushort MaxDays;

		// Token: 0x04001582 RID: 5506
		[FieldOffset(4)]
		public ushort MinTime;

		// Token: 0x04001583 RID: 5507
		[FieldOffset(6)]
		public ushort MaxTime;
	}
}
