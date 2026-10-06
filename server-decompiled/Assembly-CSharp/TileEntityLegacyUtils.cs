using System;
using System.Collections.Generic;

// Token: 0x02000C7C RID: 3196
public static class TileEntityLegacyUtils
{
	// Token: 0x0600611A RID: 24858 RVA: 0x00268DD8 File Offset: 0x00266FD8
	public static bool TryReadLegacyType(PooledBinaryReader _br, StreamModeRead _eStreamMode, TileEntityType _type, Chunk _chunk, Func<int, int, int, BlockValue> _getBlock, out TileEntity _tileEntity)
	{
		if (_eStreamMode != StreamModeRead.Persistency)
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

	// Token: 0x0600611B RID: 24859 RVA: 0x00268E98 File Offset: 0x00267098
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

	// Token: 0x0600611C RID: 24860 RVA: 0x00268EE8 File Offset: 0x002670E8
	public static Bag ReadLegacyLootIntoBag(PooledBinaryReader _br)
	{
		TileEntityLegacyUtils.LegacyLootContainerData legacyLootContainerData = TileEntityLegacyUtils.ReadLegacyLootContainerData(_br);
		PackedBoolArray packedBoolArray = legacyLootContainerData.slotLocks ?? new PackedBoolArray(legacyLootContainerData.items.Length);
		packedBoolArray.Length = legacyLootContainerData.items.Length;
		Bag bag = new Bag(new Vector2i(packedBoolArray.Length, 1), XUiC_ItemStack.StackLocationTypes.LootContainer, null);
		bag.ItemGrid.SetItems(legacyLootContainerData.items, false);
		bag.ItemGrid.SetSlotLocks(packedBoolArray);
		bag.ItemGrid.SetPreferences(legacyLootContainerData.preferences);
		if (legacyLootContainerData.bTouched)
		{
			bag.ItemGrid.Touch();
		}
		return bag;
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x00268F7C File Offset: 0x0026717C
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

	// Token: 0x0600611E RID: 24862 RVA: 0x00269178 File Offset: 0x00267378
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
			feature.ItemGrid.Resize(legacyLootContainerData.containerSize);
			feature.ItemGrid.SetItems(legacyLootContainerData.items, false);
			feature.ItemGrid.SetSlotLocks(legacyLootContainerData.slotLocks);
			feature.ItemGrid.SetPreferences(legacyLootContainerData.preferences);
			feature.ItemGrid.WorldTimeTouched = legacyLootContainerData.worldTimeTouched;
			feature.ItemGrid.PlayerOwned = legacyLootContainerData.bPlayerStorage;
		}
		return tileEntityComposite;
	}

	// Token: 0x0600611F RID: 24863 RVA: 0x00269270 File Offset: 0x00267470
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

	// Token: 0x06006120 RID: 24864 RVA: 0x0026935C File Offset: 0x0026755C
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

	// Token: 0x06006121 RID: 24865 RVA: 0x00269404 File Offset: 0x00267604
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

	// Token: 0x06006122 RID: 24866 RVA: 0x002694D8 File Offset: 0x002676D8
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

	// Token: 0x06006123 RID: 24867 RVA: 0x0026967C File Offset: 0x0026787C
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

	// Token: 0x06006124 RID: 24868 RVA: 0x00269780 File Offset: 0x00267980
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

	// Token: 0x06006125 RID: 24869 RVA: 0x00269891 File Offset: 0x00267A91
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

	// Token: 0x02000C7D RID: 3197
	public struct LegacyState
	{
		// Token: 0x06006126 RID: 24870 RVA: 0x002698CC File Offset: 0x00267ACC
		public LegacyState(Vector3i _chunkPos, int _entityId, ulong _heapMapUpdateTime, ulong _heapMapLastTime)
		{
			this.chunkPos = _chunkPos;
			this.entityId = _entityId;
			this.heapMapUpdateTime = _heapMapUpdateTime;
			this.heapMapLastTime = _heapMapLastTime;
		}

		// Token: 0x04004C3A RID: 19514
		public Vector3i chunkPos;

		// Token: 0x04004C3B RID: 19515
		public int entityId;

		// Token: 0x04004C3C RID: 19516
		public ulong heapMapUpdateTime;

		// Token: 0x04004C3D RID: 19517
		public ulong heapMapLastTime;
	}

	// Token: 0x02000C7E RID: 3198
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LegacyLootContainerData
	{
		// Token: 0x04004C3E RID: 19518
		public TileEntityLegacyUtils.LegacyState state;

		// Token: 0x04004C3F RID: 19519
		public string lootListName;

		// Token: 0x04004C40 RID: 19520
		public Vector2i containerSize;

		// Token: 0x04004C41 RID: 19521
		public bool bTouched;

		// Token: 0x04004C42 RID: 19522
		public ulong worldTimeTouched;

		// Token: 0x04004C43 RID: 19523
		public bool bPlayerBackpack;

		// Token: 0x04004C44 RID: 19524
		public ItemStack[] items;

		// Token: 0x04004C45 RID: 19525
		public bool bPlayerStorage;

		// Token: 0x04004C46 RID: 19526
		public bool hasPreferences;

		// Token: 0x04004C47 RID: 19527
		public PreferenceTracker preferences;

		// Token: 0x04004C48 RID: 19528
		public PackedBoolArray slotLocks;
	}
}
