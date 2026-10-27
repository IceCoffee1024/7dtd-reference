using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000186 RID: 390
[Preserve]
public class TEFeatureStorage : TEFeatureAbs, IInventory, IItemStackGridHolder, IHasWorldAddress
{
	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00049CB5 File Offset: 0x00047EB5
	// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x00049CBD File Offset: 0x00047EBD
	public float LootStageMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00049CC6 File Offset: 0x00047EC6
	// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x00049CCE File Offset: 0x00047ECE
	public float LootStageBonus
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00049CD8 File Offset: 0x00047ED8
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		this.lockFeature = base.Parent.GetFeature<TEFeatureLockable>();
		this.lockpickFeature = base.Parent.GetFeature<TEFeatureLockPickable>();
		DynamicProperties props = _featureData.Props;
		if (!props.Values.ContainsKey(TEFeatureStorage.PropLootList))
		{
			Log.Error("Block with name " + base.Parent.TeData.Block.GetBlockName() + " does not have a loot list");
		}
		else
		{
			this.lootListName = props.Values[TEFeatureStorage.PropLootList];
		}
		float num = 0f;
		float num2 = 0f;
		props.ParseFloat(TEFeatureStorage.PropLootStageMod, ref num);
		props.ParseFloat(TEFeatureStorage.PropLootStageBonus, ref num2);
		props.ParseBool(TEFeatureStorage.PropIsJammed, ref this.isJammed);
		props.ParseBool(TEFeatureStorage.PropIsQuestLoot, ref this.isQuestLoot);
		this.LootStageMod = num;
		this.LootStageBonus = num2;
		for (int i = 1; i < 99; i++)
		{
			string text = TEFeatureStorage.PropAlternateLootList + i.ToString();
			if (!props.Values.ContainsKey(text))
			{
				break;
			}
			string text2 = "";
			if (props.Params1.ContainsKey(text))
			{
				text2 = props.Params1[text];
			}
			if (!string.IsNullOrEmpty(text2))
			{
				FastTags<TagGroup.Global> fastTags = FastTags<TagGroup.Global>.Parse(text2);
				if (this.AlternateLootList == null)
				{
					this.AlternateLootList = new List<TEFeatureStorage.AlternateLootEntry>();
				}
				this.AlternateLootList.Add(new TEFeatureStorage.AlternateLootEntry
				{
					tag = fastTags,
					lootEntry = props.Values[text]
				});
			}
		}
		Vector2i size = LootContainer.GetLootContainer(this.lootListName, true).size;
		this.itemGrid = ItemStackGrid.Create(size, XUiC_ItemStack.StackLocationTypes.LootContainer, true, true, this);
		this.itemGrid.SlotChanged += this.OnSlotChanged;
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00049EB0 File Offset: 0x000480B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _index)
	{
		if (this.isReading)
		{
			return;
		}
		base.SetModified();
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00049EC4 File Offset: 0x000480C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureStorage tefeatureStorage;
		if (_other.TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage))
		{
			this.lootListName = tefeatureStorage.lootListName;
			this.ItemGrid.CopyFrom(tefeatureStorage.ItemGrid, true);
		}
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00049EF9 File Offset: 0x000480F9
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (!GameManager.IsDedicatedServer)
		{
			XUiC_LootWindowGroup.CloseIfOpenAtPos(base.ToWorldPos(), null);
		}
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00049F14 File Offset: 0x00048114
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && base.Parent.Owner != null)
		{
			this.itemGrid.Touch();
			this.itemGrid.PlayerOwned = true;
			this.SetEmpty();
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x00049F54 File Offset: 0x00048154
	public override void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		base.UpgradeDowngradeFrom(_other);
		TEFeatureStorage feature = _other.GetFeature<TEFeatureStorage>();
		if (feature != null)
		{
			Vector2i containerSize = this.ItemGrid.ContainerSize;
			this.isReading = true;
			List<ItemStack> list;
			try
			{
				this.ItemGrid.CopyFrom(feature.ItemGrid, true);
				list = this.ItemGrid.Resize(containerSize);
			}
			finally
			{
				this.isReading = false;
			}
			if (list != null)
			{
				Vector3 vector = base.ToWorldCenterPos();
				vector.y += 0.9f;
				GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
			}
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x00049FFC File Offset: 0x000481FC
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		TEFeatureStorage tefeatureStorage;
		if (!_teNew.TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage))
		{
			GameManager.Instance.DropContentOfLootContainerServer(_bvOld, base.ToWorldPos(), this);
		}
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x0004A030 File Offset: 0x00048230
	public override void Reset(FastTags<TagGroup.Global> _questTags)
	{
		base.Reset(_questTags);
		if (this.itemGrid.Reset())
		{
			if (this.AlternateLootList != null)
			{
				for (int i = 0; i < this.AlternateLootList.Count; i++)
				{
					if (_questTags.Test_AnySet(this.AlternateLootList[i].tag))
					{
						this.lootListName = this.AlternateLootList[i].lootEntry;
						break;
					}
				}
			}
			base.SetModified();
		}
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0004A0A8 File Offset: 0x000482A8
	public override void UpdateTick(World _world)
	{
		base.UpdateTick(_world);
		if (base.Parent.PlayerPlaced)
		{
			return;
		}
		if (!this.itemGrid.Touched || this.itemGrid.PlayerOwned || !this.IsEmpty() || GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays) <= 0)
		{
			return;
		}
		int num = GameUtils.WorldTimeToTotalHours(this.itemGrid.WorldTimeTouched);
		if ((GameUtils.WorldTimeToTotalHours(_world.worldTime) - num) / 24 >= GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays))
		{
			this.itemGrid.Reset();
			base.SetModified();
			return;
		}
		if (this.entityTempList == null)
		{
			this.entityTempList = new List<Entity>();
		}
		else
		{
			this.entityTempList.Clear();
		}
		_world.GetEntitiesInBounds(typeof(EntityPlayer), new Bounds(base.ToWorldPos().ToVector3(), Vector3.one * 16f), this.entityTempList);
		if (this.entityTempList.Count > 0)
		{
			this.itemGrid.Touch();
			base.SetModified();
			return;
		}
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0004A1B0 File Offset: 0x000483B0
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		if (this.lockpickFeature != null && this.lockpickFeature.NeedsLockpicking())
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.lockFeature != null && this.lockFeature.IsLocked() && !this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			return string.Format(Localization.Get("tooltipJammed", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.isJammed)
		{
			return string.Format(Localization.Get("tooltipJammed", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.isQuestLoot)
		{
			return string.Format(Localization.Get("lootTooltipTouched", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (!this.itemGrid.Touched)
		{
			return string.Format(Localization.Get("lootTooltipNew", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.IsEmpty())
		{
			return string.Format(Localization.Get("lootTooltipEmpty", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		return string.Format(Localization.Get("lootTooltipTouched", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x0004A2C7 File Offset: 0x000484C7
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Search", "search", true, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x0004A2F0 File Offset: 0x000484F0
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (!base.CommandIs(_commandName, "Search"))
		{
			return false;
		}
		if ((this.lockFeature != null && this.lockFeature.IsLocked() && !this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier)) || this.isJammed)
		{
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, "Misc/locked");
			return false;
		}
		_player.AimingGun = false;
		LockManager.Instance.LockRequestLocal(this, 0);
		return true;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0004A386 File Offset: 0x00048586
	// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x0004A38E File Offset: 0x0004858E
	public string lootListName { get; set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0004A397 File Offset: 0x00048597
	public ItemStackGrid ItemGrid
	{
		get
		{
			return this.itemGrid;
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x00010E62 File Offset: 0x0000F062
	public byte GetItemStackGridKey(ItemStackGrid _grid)
	{
		return 0;
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0004A397 File Offset: 0x00048597
	public ItemStackGrid GetItemStackGrid(byte _key)
	{
		return this.itemGrid;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x0004A39F File Offset: 0x0004859F
	public void UpdateSlot(int _idx, ItemStack _item)
	{
		this.ItemGrid[_idx] = _item;
		base.Parent.NotifyListeners();
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0004A3BC File Offset: 0x000485BC
	public void RemoveItem(ItemValue _itemValue)
	{
		for (int i = 0; i < this.ItemGrid.Length; i++)
		{
			if (this.ItemGrid[i].itemValue.ItemClass == _itemValue.ItemClass)
			{
				this.UpdateSlot(i, ItemStack.Empty);
			}
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x0004A40C File Offset: 0x0004860C
	public int RemoveItems(ItemValue _itemValue, int _count)
	{
		int num = _count;
		int num2 = 0;
		while (_count > 0 && num2 < this.ItemGrid.Length)
		{
			if (this.ItemGrid[num2].itemValue.ItemClass == _itemValue.ItemClass)
			{
				if (_itemValue.ItemClass.CanStack())
				{
					int count = this.ItemGrid[num2].count;
					int num3 = ((count >= _count) ? _count : count);
					this.ItemGrid[num2].count -= num3;
					_count -= num3;
					if (this.ItemGrid[num2].count <= 0)
					{
						this.ItemGrid[num2].Clear();
					}
				}
				else
				{
					this.ItemGrid[num2].Clear();
					_count--;
				}
			}
			num2++;
		}
		base.Parent.NotifyListeners();
		return num - _count;
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0004A4F0 File Offset: 0x000486F0
	public bool IsEmpty()
	{
		for (int i = 0; i < this.ItemGrid.Length; i++)
		{
			if (!this.ItemGrid[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0004A529 File Offset: 0x00048729
	public void SetEmpty()
	{
		this.ItemGrid.Clear();
		this.ItemGrid.Touch();
		base.Parent.NotifyListeners();
		base.SetModified();
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0004A554 File Offset: 0x00048754
	public bool ShouldDestroyOnClose()
	{
		LootContainer.DestroyOnClose destroyOnClose = LootContainer.GetLootContainer(this.lootListName, true).destroyOnClose;
		return destroyOnClose == LootContainer.DestroyOnClose.True || (destroyOnClose == LootContainer.DestroyOnClose.Empty && this.IsEmpty());
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0004A588 File Offset: 0x00048788
	public bool AddItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.ItemGrid.Length; i++)
		{
			if (this.ItemGrid[i].IsEmpty())
			{
				this.UpdateSlot(i, _itemStack);
				base.SetModified();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0004A5D0 File Offset: 0x000487D0
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack)
	{
		bool flag = false;
		int count = _itemStack.count;
		for (int i = _startIndex; i < this.ItemGrid.Length; i++)
		{
			int count2 = _itemStack.count;
			if (_itemStack.itemValue.type == this.ItemGrid[i].itemValue.type && this.ItemGrid[i].CanStackPartly(ref count2))
			{
				this.ItemGrid[i].count += count2;
				_itemStack.count -= count2;
				if (_itemStack.count == 0)
				{
					break;
				}
			}
		}
		if (_itemStack.count != count)
		{
			flag = true;
			base.SetModified();
			base.Parent.NotifyListeners();
		}
		return new ValueTuple<bool, bool>(flag, _itemStack.count == 0);
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0004A698 File Offset: 0x00048898
	public bool HasItem(ItemValue _item)
	{
		for (int i = 0; i < this.ItemGrid.Length; i++)
		{
			if (this.ItemGrid[i].itemValue.type == _item.type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x0004A6DC File Offset: 0x000488DC
	public int CountItem(ItemClass _itemClass)
	{
		int num = 0;
		for (int i = 0; i < this.ItemGrid.Length; i++)
		{
			if (this.ItemGrid[i].itemValue.ItemClass == _itemClass)
			{
				num += this.ItemGrid[i].count;
			}
		}
		return num;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x0004A730 File Offset: 0x00048930
	public override void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		this.isReading = true;
		try
		{
			this.readInternal(_br, _eStreamMode);
		}
		finally
		{
			this.isReading = false;
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x0004A768 File Offset: 0x00048968
	[PublicizedFrom(EAccessModifier.Private)]
	public void readInternal(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			num = 20;
		}
		else if (base.Parent.UseLocalVersioning())
		{
			num = (int)_br.ReadUInt16();
		}
		else
		{
			num = base.Parent.GetLegacyForkVersion();
		}
		if (_br.ReadBoolean())
		{
			this.lootListName = _br.ReadString();
		}
		if (num < 20)
		{
			if (num == 19)
			{
				bool flag = _br.ReadBoolean();
				ulong num2 = (ulong)_br.ReadUInt32();
				bool flag2 = _br.ReadBoolean();
				this.itemGrid.ReadInto(_br, _eStreamMode);
				this.itemGrid.WorldTimeTouched = num2;
				this.itemGrid.PlayerOwned = flag2;
				if (flag && num2 == 0UL)
				{
					this.itemGrid.Touch();
					return;
				}
			}
			else
			{
				Vector2i vector2i = new Vector2i
				{
					x = (int)_br.ReadUInt16(),
					y = (int)_br.ReadUInt16()
				};
				_br.ReadBoolean();
				ulong num3 = (ulong)_br.ReadUInt32();
				bool flag3 = _br.ReadBoolean();
				int num4 = Math.Min((int)_br.ReadInt16(), vector2i.x * vector2i.y);
				ItemStack[] array = ItemStack.CreateArray(num4);
				PreferenceTracker preferenceTracker = null;
				for (int i = 0; i < num4; i++)
				{
					array[i].Read(_br);
				}
				if (_br.ReadBoolean())
				{
					preferenceTracker = new PreferenceTracker(-1);
					preferenceTracker.Read(_br);
				}
				PackedBoolArray packedBoolArray;
				if (num >= 12)
				{
					packedBoolArray = new PackedBoolArray(0);
					packedBoolArray.Read(_br);
				}
				else
				{
					packedBoolArray = new PackedBoolArray(array.Length);
				}
				this.itemGrid.Resize(vector2i);
				this.itemGrid.SetItems(array, false);
				this.itemGrid.SetSlotLocks(packedBoolArray);
				this.itemGrid.SetPreferences(preferenceTracker);
				this.itemGrid.WorldTimeTouched = num3;
				this.itemGrid.PlayerOwned = flag3;
			}
			return;
		}
		if (_eStreamMode == StreamModeRead.FromServer && base.Parent.IsUserAccessing())
		{
			ItemStackGrid.Read(_br, _eStreamMode);
			return;
		}
		this.itemGrid.ReadInto(_br, _eStreamMode);
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x0004A94C File Offset: 0x00048B4C
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(20);
		}
		bool flag = !string.IsNullOrEmpty(this.lootListName);
		_bw.Write(flag);
		if (flag)
		{
			_bw.Write(this.lootListName);
		}
		this.itemGrid.Write(_bw, _eStreamMode);
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0004A9A0 File Offset: 0x00048BA0
	public override bool OnLockRequestServer(int _lockingPlayerID, PooledBinaryReader _brLocalContext, ushort _channel)
	{
		if (!this.itemGrid.PlayerOwned && !this.itemGrid.Touched)
		{
			BlockValue block = GameManager.Instance.World.GetBlock(base.ToWorldPos());
			GameManager.Instance.lootManager.LootContainerOpened(this, _lockingPlayerID, block.Block.Tags);
		}
		return true;
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0004A9FC File Offset: 0x00048BFC
	public override void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
		bool flag = !this.itemGrid.PlayerOwned && !this.itemGrid.Touched;
		this.itemGrid.Touch();
		if (flag)
		{
			base.SetModified();
		}
		_bwServerContext.Write(flag);
		this.itemGrid.Write(_bwServerContext, StreamModeWrite.ToClient);
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x0004AA50 File Offset: 0x00048C50
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		bool flag = _brServerContext.ReadBoolean();
		this.itemGrid.ReadInto(_brServerContext, StreamModeRead.FromServer);
		BlockValue block = GameManager.Instance.World.GetBlock(base.ToWorldPos());
		string localizedBlockName = block.Block.GetLocalizedBlockName();
		((XUiC_LootWindowGroup)uiforPrimaryPlayer.xui.FindWindowGroupByName(XUiC_LootWindowGroup.ID)).OpenLooting(localizedBlockName, this, flag);
		LootContainer lootContainer = LootContainer.GetLootContainer(this.lootListName, true);
		if (lootContainer != null && uiforPrimaryPlayer.entityPlayer != null)
		{
			lootContainer.ExecuteBuffActions(uiforPrimaryPlayer.entityPlayer.entityId, uiforPrimaryPlayer.entityPlayer);
		}
		if (!GameManager.Instance.World.IsEditor() && flag)
		{
			EntityPlayerLocal entityPlayer = uiforPrimaryPlayer.entityPlayer;
			entityPlayer.MinEventContext.TileEntity = this;
			entityPlayer.MinEventContext.BlockValue = block;
			entityPlayer.FireEvent(MinEventTypes.onSelfOpenLootContainer, true);
		}
		return true;
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x0004AB5A File Offset: 0x00048D5A
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		GameManager.Instance.CheckDestroyTileEntity(this, base.ToWorldPos());
	}

	// Token: 0x04000AAA RID: 2730
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 20;

	// Token: 0x04000AAB RID: 2731
	public static string PropLootList = "LootList";

	// Token: 0x04000AAC RID: 2732
	public static string PropAlternateLootList = "AlternateLootList";

	// Token: 0x04000AAD RID: 2733
	public static string PropLootStageMod = "LootStageMod";

	// Token: 0x04000AAE RID: 2734
	public static string PropLootStageBonus = "LootStageBonus";

	// Token: 0x04000AAF RID: 2735
	public static string PropIsJammed = "IsJammed";

	// Token: 0x04000AB0 RID: 2736
	public static string PropIsQuestLoot = "IsQuestLoot";

	// Token: 0x04000AB1 RID: 2737
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000AB2 RID: 2738
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000AB5 RID: 2741
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isJammed;

	// Token: 0x04000AB6 RID: 2742
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isQuestLoot;

	// Token: 0x04000AB7 RID: 2743
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReading;

	// Token: 0x04000AB8 RID: 2744
	public List<TEFeatureStorage.AlternateLootEntry> AlternateLootList;

	// Token: 0x04000AB9 RID: 2745
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> entityTempList;

	// Token: 0x04000ABB RID: 2747
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid itemGrid;

	// Token: 0x02000187 RID: 391
	public struct AlternateLootEntry
	{
		// Token: 0x04000ABC RID: 2748
		public FastTags<TagGroup.Global> tag;

		// Token: 0x04000ABD RID: 2749
		public string lootEntry;
	}
}
