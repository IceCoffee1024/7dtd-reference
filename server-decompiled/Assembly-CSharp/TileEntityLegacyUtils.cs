using System;
using System.Collections.Generic;

// Token: 0x02000C6D RID: 3181
public static class TileEntityLegacyUtils
{
	// Token: 0x0600611D RID: 24861 RVA: 0x00265958 File Offset: 0x00263B58
	public static bool TryReadLegacyType(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode, TileEntityType _type, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock, out TileEntity _tileEntity)
	{
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			_tileEntity = null;
			return false;
		}
		switch (_type)
		{
		case TileEntityType.LandClaim:
			_tileEntity = TileEntityLegacyUtils.ReadLegacyLandClaimIntoComposite(_br, _chunk, _getBlock);
			return true;
		case TileEntityType.Loot:
		{
			BlockValue blockValue;
			_tileEntity = TileEntityLegacyUtils.ReadLegacyLootIntoComposite(_br, _chunk, _getBlock, out blockValue);
			return true;
		}
		case TileEntityType.Trader:
			_tileEntity = null;
			return true;
		case TileEntityType.VendingMachine:
		case TileEntityType.Forge:
		case TileEntityType.Campfire:
		case TileEntityType.Workstation:
			break;
		case TileEntityType.SecureLoot:
			_tileEntity = TileEntityLegacyUtils.ReadLegacySecureLootIntoComposite(_br, _chunk, _getBlock);
			return true;
		case TileEntityType.SecureDoor:
			_tileEntity = TileEntityLegacyUtils.ReadLegacySecureDoorIntoComposite(_br, _chunk, _getBlock);
			return true;
		case TileEntityType.Sign:
			_tileEntity = TileEntityLegacyUtils.ReadLegacySignIntoComposite(_br, _chunk, _getBlock);
			return true;
		case TileEntityType.GoreBlock:
			TileEntityLegacyUtils.ReadLegacyGoreAndDiscard(_br);
			_tileEntity = null;
			return true;
		default:
			if (_type == TileEntityType.SecureLootSigned)
			{
				_tileEntity = TileEntityLegacyUtils.ReadLegacySecureLootSignedIntoComposite(_br, _chunk, _getBlock);
				return true;
			}
			break;
		}
		_tileEntity = null;
		return false;
	}

	// Token: 0x0600611E RID: 24862 RVA: 0x00265A18 File Offset: 0x00263C18
	public static void MigrateLegacyMetadata(TileEntityComposite _teComposite, BlockValue _blockValue)
	{
		byte meta = _blockValue.meta;
		_blockValue.meta = 0;
		TEFeatureDoor tefeatureDoor;
		if (_teComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor))
		{
			bool flag = (meta & 1) > 0;
			tefeatureDoor.SetOpen(flag, false);
			TEFeatureLockable tefeatureLockable;
			if (_teComposite.TryGetSelfOrFeature<TEFeatureLockable>(out tefeatureLockable))
			{
				bool flag2 = (meta & 4) > 0;
				tefeatureLockable.SetLocked(flag2);
			}
		}
	}

	// Token: 0x0600611F RID: 24863 RVA: 0x00265A68 File Offset: 0x00263C68
	public static Bag ReadLegacyLootIntoBag(PooledBinaryReader _br)
	{
		TileEntityLegacyUtils.LegacyLootContainerData legacyLootContainerData = TileEntityLegacyUtils.ReadLegacyLootContainerData(_br);
		Bag bag = new Bag(legacyLootContainerData.items.Length);
		bag.SetSlots(legacyLootContainerData.items);
		bag.LockedSlots = legacyLootContainerData.slotLocks;
		if (bag.LockedSlots != null)
		{
			bag.LockedSlots.Length = bag.SlotCount;
		}
		bag.Touched = legacyLootContainerData.bTouched;
		bag.preferences = (legacyLootContainerData.hasPreferences ? legacyLootContainerData.preferences : null);
		return bag;
	}

	// Token: 0x06006120 RID: 24864 RVA: 0x00265AE0 File Offset: 0x00263CE0
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityLegacyUtils.LegacyLootContainerData ReadLegacyLootContainerData(PooledBinaryReader _br)
	{
		TileEntityLegacyUtils.LegacyLootContainerData legacyLootContainerData = default(TileEntityLegacyUtils.LegacyLootContainerData);
		int num = (int)_br.ReadUInt16();
		Vector3i vector3i = StreamUtils.ReadVector3i(_br);
		int num2 = _br.ReadInt32();
		ulong num3 = 0UL;
		ulong num4 = 0UL;
		if (num > 1)
		{
			num3 = _br.ReadUInt64();
			num4 = num3 - AIDirector.GetActivityWorldTimeDelay();
		}
		if (num >= 18)
		{
			num = (int)_br.ReadUInt16();
		}
		if (num > 8)
		{
			if (_br.ReadBoolean())
			{
				legacyLootContainerData.lootListName = _br.ReadString();
			}
			legacyLootContainerData.containerSize = new Vector2i
			{
				x = (int)_br.ReadUInt16(),
				y = (int)_br.ReadUInt16()
			};
			legacyLootContainerData.bTouched = _br.ReadBoolean();
			legacyLootContainerData.worldTimeTouched = (ulong)_br.ReadUInt32();
			legacyLootContainerData.bPlayerBackpack = _br.ReadBoolean();
			int num5 = Math.Min((int)_br.ReadInt16(), legacyLootContainerData.containerSize.x * legacyLootContainerData.containerSize.y);
			legacyLootContainerData.items = ItemStack.CreateArray(legacyLootContainerData.containerSize.x * legacyLootContainerData.containerSize.y);
			if (num < 3)
			{
				for (int i = 0; i < num5; i++)
				{
					legacyLootContainerData.items[i].Clear();
					legacyLootContainerData.items[i].ReadOld(_br);
				}
			}
			else
			{
				for (int j = 0; j < num5; j++)
				{
					legacyLootContainerData.items[j].Clear();
					legacyLootContainerData.items[j].Read(_br);
				}
			}
			legacyLootContainerData.bPlayerStorage = _br.ReadBoolean();
			if (num > 9)
			{
				legacyLootContainerData.hasPreferences = _br.ReadBoolean();
				if (legacyLootContainerData.hasPreferences)
				{
					legacyLootContainerData.preferences = new PreferenceTracker(-1);
					legacyLootContainerData.preferences.Read(_br);
				}
			}
			if (num >= 12)
			{
				legacyLootContainerData.slotLocks = new PackedBoolArray(0);
				legacyLootContainerData.slotLocks.Read(_br);
			}
			else
			{
				legacyLootContainerData.slotLocks = new PackedBoolArray(num5);
			}
			legacyLootContainerData.state = new TileEntityLegacyUtils.LegacyState(vector3i, num2, num3, num4);
			return legacyLootContainerData;
		}
		throw new Exception("Outdated loot data");
	}

	// Token: 0x06006121 RID: 24865 RVA: 0x00265CDC File Offset: 0x00263EDC
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityComposite ReadLegacyLootIntoComposite(PooledBinaryReader _br, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock, out BlockValue _blockValue)
	{
		TileEntityLegacyUtils.LegacyLootContainerData legacyLootContainerData = TileEntityLegacyUtils.ReadLegacyLootContainerData(_br);
		_blockValue = _getBlock(legacyLootContainerData.state.chunkPos.x, legacyLootContainerData.state.chunkPos.y, legacyLootContainerData.state.chunkPos.z);
		BlockCompositeTileEntity blockCompositeTileEntity = _blockValue.Block as BlockCompositeTileEntity;
		if (blockCompositeTileEntity == null)
		{
			Log.Error("TileEntityComposite.ReadLegacyLootIntoComposite: failed to convert legacy TE data into a composite TE.");
			return null;
		}
		TileEntityComposite tileEntityComposite = new TileEntityComposite(_chunk, blockCompositeTileEntity, legacyLootContainerData.state);
		TEFeatureStorage feature = tileEntityComposite.GetFeature<TEFeatureStorage>();
		if (feature != null)
		{
			feature.lootListName = legacyLootContainerData.lootListName;
			feature.SetContainerSize(legacyLootContainerData.containerSize, true);
			feature.bTouched = legacyLootContainerData.bTouched;
			feature.worldTimeTouched = legacyLootContainerData.worldTimeTouched;
			feature.bPlayerStorage = legacyLootContainerData.bPlayerStorage;
			feature.items = legacyLootContainerData.items;
			if (legacyLootContainerData.hasPreferences && legacyLootContainerData.preferences != null)
			{
				feature.preferences = legacyLootContainerData.preferences;
			}
			feature.SlotLocks = legacyLootContainerData.slotLocks;
		}
		return tileEntityComposite;
	}

	// Token: 0x06006122 RID: 24866 RVA: 0x00265DD0 File Offset: 0x00263FD0
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityComposite ReadLegacySecureLootIntoComposite(PooledBinaryReader _br, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock)
	{
		BlockValue blockValue;
		TileEntityComposite tileEntityComposite = TileEntityLegacyUtils.ReadLegacyLootIntoComposite(_br, _chunk, _getBlock, out blockValue);
		_br.ReadInt32();
		_br.ReadBoolean();
		bool flag = _br.ReadBoolean();
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		string text = _br.ReadString();
		int num = _br.ReadInt32();
		List<PlatformUserIdentifierAbs> list = null;
		for (int i = 0; i < num; i++)
		{
			if (list == null)
			{
				list = new List<PlatformUserIdentifierAbs>();
			}
			list.Add(PlatformUserIdentifierAbs.FromStream(_br, false, false));
		}
		if (tileEntityComposite == null)
		{
			return null;
		}
		tileEntityComposite.SetOwner(platformUserIdentifierAbs);
		TEFeatureLockable feature = tileEntityComposite.GetFeature<TEFeatureLockable>();
		if (feature != null)
		{
			feature.SetLocked(flag);
			feature.SetPasswordHash(text, platformUserIdentifierAbs);
			if (list != null)
			{
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs2 in list)
				{
					feature.CheckPasswordHash(text, platformUserIdentifierAbs2);
				}
			}
		}
		return tileEntityComposite;
	}

	// Token: 0x06006123 RID: 24867 RVA: 0x00265EBC File Offset: 0x002640BC
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityComposite ReadLegacySecureLootSignedIntoComposite(PooledBinaryReader _br, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock)
	{
		TileEntityComposite tileEntityComposite = TileEntityLegacyUtils.ReadLegacySecureLootIntoComposite(_br, _chunk, _getBlock);
		int num = _br.ReadInt32();
		_br.ReadBoolean();
		_br.ReadBoolean();
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		_br.ReadString();
		int num2 = _br.ReadInt32();
		for (int i = 0; i < num2; i++)
		{
			PlatformUserIdentifierAbs.FromStream(_br, false, false);
		}
		AuthoredText authoredText;
		if (num > 1)
		{
			authoredText = AuthoredText.FromStream(_br);
		}
		else
		{
			authoredText = new AuthoredText(_br.ReadString(), ((tileEntityComposite != null) ? tileEntityComposite.Owner : null) ?? platformUserIdentifierAbs);
		}
		if (tileEntityComposite == null)
		{
			return null;
		}
		TEFeatureSignable feature = tileEntityComposite.GetFeature<TEFeatureSignable>();
		if (feature != null)
		{
			feature.SetText(authoredText, false);
		}
		return tileEntityComposite;
	}

	// Token: 0x06006124 RID: 24868 RVA: 0x00265F64 File Offset: 0x00264164
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityComposite ReadLegacyLandClaimIntoComposite(PooledBinaryReader _br, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock)
	{
		int num = (int)_br.ReadUInt16();
		Vector3i vector3i = StreamUtils.ReadVector3i(_br);
		int num2 = _br.ReadInt32();
		ulong num3 = 0UL;
		ulong num4 = 0UL;
		if (num > 1)
		{
			num3 = _br.ReadUInt64();
			num4 = num3 - AIDirector.GetActivityWorldTimeDelay();
		}
		_br.ReadInt32();
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		bool flag = _br.ReadBoolean();
		BlockCompositeTileEntity blockCompositeTileEntity = ((_getBlock != null) ? _getBlock(vector3i.x, vector3i.y, vector3i.z).Block : null) as BlockCompositeTileEntity;
		if (blockCompositeTileEntity == null)
		{
			Log.Error("TileEntityComposite.ReadLegacyLandClaimIntoComposite: failed to convert legacy TE data into a composite TE.");
			return null;
		}
		TileEntityLegacyUtils.LegacyState legacyState = new TileEntityLegacyUtils.LegacyState(vector3i, num2, num3, num4);
		TileEntityComposite tileEntityComposite = new TileEntityComposite(_chunk, blockCompositeTileEntity, legacyState);
		if (tileEntityComposite.Owner == null)
		{
			tileEntityComposite.SetOwner(platformUserIdentifierAbs);
		}
		TEFeatureLandClaim feature = tileEntityComposite.GetFeature<TEFeatureLandClaim>();
		if (feature != null)
		{
			feature.ShowBounds = flag;
		}
		return tileEntityComposite;
	}

	// Token: 0x06006125 RID: 24869 RVA: 0x00266038 File Offset: 0x00264238
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityComposite ReadLegacySignIntoComposite(PooledBinaryReader _br, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock)
	{
		int num = (int)_br.ReadUInt16();
		Vector3i vector3i = StreamUtils.ReadVector3i(_br);
		int num2 = _br.ReadInt32();
		ulong num3 = 0UL;
		ulong num4 = 0UL;
		if (num > 1)
		{
			num3 = _br.ReadUInt64();
			num4 = num3 - AIDirector.GetActivityWorldTimeDelay();
		}
		int num5 = _br.ReadInt32();
		bool flag = _br.ReadBoolean();
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		AuthoredText authoredText = null;
		if (num5 > 1)
		{
			authoredText = AuthoredText.FromStream(_br);
		}
		string text = _br.ReadString();
		int num6 = _br.ReadInt32();
		List<PlatformUserIdentifierAbs> list = null;
		for (int i = 0; i < num6; i++)
		{
			if (list == null)
			{
				list = new List<PlatformUserIdentifierAbs>();
			}
			list.Add(PlatformUserIdentifierAbs.FromStream(_br, false, false));
		}
		if (num5 <= 1)
		{
			authoredText = new AuthoredText(_br.ReadString(), platformUserIdentifierAbs);
		}
		BlockCompositeTileEntity blockCompositeTileEntity = ((_getBlock != null) ? _getBlock(vector3i.x, vector3i.y, vector3i.z).Block : null) as BlockCompositeTileEntity;
		if (blockCompositeTileEntity == null)
		{
			Log.Error("TileEntityComposite.ReadLegacySignIntoComposite: failed to convert legacy TE data into a composite TE.");
			return null;
		}
		TileEntityLegacyUtils.LegacyState legacyState = new TileEntityLegacyUtils.LegacyState(vector3i, num2, num3, num4);
		TileEntityComposite tileEntityComposite = new TileEntityComposite(_chunk, blockCompositeTileEntity, legacyState);
		if (tileEntityComposite.Owner == null)
		{
			tileEntityComposite.SetOwner(platformUserIdentifierAbs);
		}
		TEFeatureSignable feature = tileEntityComposite.GetFeature<TEFeatureSignable>();
		if (feature != null)
		{
			feature.SetText(authoredText, true);
		}
		TEFeatureLockable feature2 = tileEntityComposite.GetFeature<TEFeatureLockable>();
		if (feature2 != null)
		{
			feature2.SetLocked(flag);
			feature2.SetPasswordHash(text, platformUserIdentifierAbs);
			if (list != null)
			{
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs2 in list)
				{
					feature2.CheckPasswordHash(text, platformUserIdentifierAbs2);
				}
			}
		}
		return tileEntityComposite;
	}

	// Token: 0x06006126 RID: 24870 RVA: 0x002661DC File Offset: 0x002643DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static TileEntityComposite ReadLegacySecureDoorIntoComposite(PooledBinaryReader _br, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock)
	{
		BlockValue blockValue;
		TileEntityComposite tileEntityComposite = TileEntityLegacyUtils.ReadLegacyLootIntoComposite(_br, _chunk, _getBlock, out blockValue);
		int num = _br.ReadInt32();
		bool flag = false;
		PlatformUserIdentifierAbs platformUserIdentifierAbs = null;
		List<PlatformUserIdentifierAbs> list = null;
		string text = string.Empty;
		if (num > 0)
		{
			_br.ReadBoolean();
			flag = _br.ReadBoolean();
			platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			int num2 = _br.ReadInt32();
			list = new List<PlatformUserIdentifierAbs>();
			for (int i = 0; i < num2; i++)
			{
				list.Add(PlatformUserIdentifierAbs.FromStream(_br, false, false));
			}
			text = _br.ReadString();
		}
		if (tileEntityComposite == null)
		{
			return null;
		}
		if (platformUserIdentifierAbs != null)
		{
			tileEntityComposite.SetOwner(platformUserIdentifierAbs);
		}
		TEFeatureLockable feature = tileEntityComposite.GetFeature<TEFeatureLockable>();
		if (feature != null)
		{
			feature.SetLocked(flag);
			if (!string.IsNullOrEmpty(text))
			{
				feature.SetPasswordHash(text, platformUserIdentifierAbs);
			}
			if (list != null)
			{
				foreach (PlatformUserIdentifierAbs platformUserIdentifierAbs2 in list)
				{
					feature.CheckPasswordHash(text, platformUserIdentifierAbs2);
				}
			}
		}
		return tileEntityComposite;
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x002662E0 File Offset: 0x002644E0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReadLegacyGoreAndDiscard(PooledBinaryReader _br)
	{
		uint num = (uint)_br.ReadUInt16();
		StreamUtils.ReadVector3i(_br);
		_br.ReadInt32();
		if (num > 1U)
		{
			_br.ReadUInt64();
		}
		if (num > 8U)
		{
			if (_br.ReadBoolean())
			{
				_br.ReadString();
			}
			Vector2i vector2i = default(Vector2i);
			vector2i.x = (int)_br.ReadUInt16();
			vector2i.y = (int)_br.ReadUInt16();
			_br.ReadBoolean();
			_br.ReadUInt32();
			_br.ReadBoolean();
			int num2 = Math.Min((int)_br.ReadInt16(), vector2i.x * vector2i.y);
			ItemStack itemStack = new ItemStack();
			if (num < 3U)
			{
				for (int i = 0; i < num2; i++)
				{
					itemStack.ReadOld(_br);
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					itemStack.Read(_br);
				}
			}
			_br.ReadBoolean();
			if (num > 9U && _br.ReadBoolean())
			{
				new PreferenceTracker(-1).Read(_br);
			}
			if (num >= 12U)
			{
				new PackedBoolArray(0).Read(_br);
			}
			_br.ReadUInt64();
			return;
		}
		throw new Exception("Outdated loot data");
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x002663F1 File Offset: 0x002645F1
	public static TraderData ReadLegacyTileEntityTraderData(PooledBinaryReader _br)
	{
		byte b = (byte)_br.ReadInt32();
		int num = (int)_br.ReadUInt16();
		StreamUtils.ReadVector3i(_br);
		_br.ReadInt32();
		if (num > 1)
		{
			_br.ReadUInt64();
		}
		TraderData traderData = new TraderData();
		_br.ReadInt32();
		traderData.Read(_br);
		return traderData;
	}

	// Token: 0x02000C6E RID: 3182
	public struct LegacyState
	{
		// Token: 0x06006129 RID: 24873 RVA: 0x0026642C File Offset: 0x0026462C
		public LegacyState(Vector3i _chunkPos, int _entityId, ulong _heapMapUpdateTime, ulong _heapMapLastTime)
		{
			this.chunkPos = _chunkPos;
			this.entityId = _entityId;
			this.heapMapUpdateTime = _heapMapUpdateTime;
			this.heapMapLastTime = _heapMapLastTime;
		}

		// Token: 0x04004C03 RID: 19459
		public Vector3i chunkPos;

		// Token: 0x04004C04 RID: 19460
		public int entityId;

		// Token: 0x04004C05 RID: 19461
		public ulong heapMapUpdateTime;

		// Token: 0x04004C06 RID: 19462
		public ulong heapMapLastTime;
	}

	// Token: 0x02000C6F RID: 3183
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LegacyLootContainerData
	{
		// Token: 0x04004C07 RID: 19463
		public TileEntityLegacyUtils.LegacyState state;

		// Token: 0x04004C08 RID: 19464
		public string lootListName;

		// Token: 0x04004C09 RID: 19465
		public Vector2i containerSize;

		// Token: 0x04004C0A RID: 19466
		public bool bTouched;

		// Token: 0x04004C0B RID: 19467
		public ulong worldTimeTouched;

		// Token: 0x04004C0C RID: 19468
		public bool bPlayerBackpack;

		// Token: 0x04004C0D RID: 19469
		public ItemStack[] items;

		// Token: 0x04004C0E RID: 19470
		public bool bPlayerStorage;

		// Token: 0x04004C0F RID: 19471
		public bool hasPreferences;

		// Token: 0x04004C10 RID: 19472
		public PreferenceTracker preferences;

		// Token: 0x04004C11 RID: 19473
		public PackedBoolArray slotLocks;
	}
}
