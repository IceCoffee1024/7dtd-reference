using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000185 RID: 389
[Preserve]
public class TEFeatureStorage : TEFeatureAbs, ITileEntityLootable, ITileEntity, ILockTarget, IInventory
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00049B2D File Offset: 0x00047D2D
	// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x00049B35 File Offset: 0x00047D35
	public float LootStageMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00049B3E File Offset: 0x00047D3E
	// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x00049B46 File Offset: 0x00047D46
	public float LootStageBonus
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00049B50 File Offset: 0x00047D50
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
		this.SetContainerSize(LootContainer.GetLootContainer(this.lootListName, true).size, true);
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00049D04 File Offset: 0x00047F04
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		ITileEntityLootable tileEntityLootable;
		if (_other.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
		{
			this.lootListName = tileEntityLootable.lootListName;
			this.containerSize = tileEntityLootable.GetContainerSize();
			this.items = ItemStack.Clone(tileEntityLootable.items);
			this.worldTimeTouched = tileEntityLootable.worldTimeTouched;
			this.bTouched = tileEntityLootable.bTouched;
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00049D5C File Offset: 0x00047F5C
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (!GameManager.IsDedicatedServer)
		{
			XUiC_LootWindowGroup.CloseIfOpenAtPos(base.ToWorldPos(), null);
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00049D77 File Offset: 0x00047F77
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && base.Parent.Owner != null)
		{
			this.worldTimeTouched = GameManager.Instance.World.GetWorldTime();
			this.SetEmpty();
		}
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x00049DB8 File Offset: 0x00047FB8
	public override void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		base.UpgradeDowngradeFrom(_other);
		ITileEntityLootable feature = _other.GetFeature<ITileEntityLootable>();
		if (feature != null)
		{
			this.bTouched = feature.bTouched;
			this.worldTimeTouched = feature.worldTimeTouched;
			this.migrateItemsFromOtherContainer(feature);
		}
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00049DF8 File Offset: 0x00047FF8
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		ITileEntityLootable tileEntityLootable;
		if (!_teNew.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
		{
			GameManager.Instance.DropContentOfLootContainerServer(_bvOld, base.ToWorldPos(), this);
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00049E2C File Offset: 0x0004802C
	[PublicizedFrom(EAccessModifier.Private)]
	public void migrateItemsFromOtherContainer(ITileEntityLootable _other)
	{
		this.items = ItemStack.Clone(_other.items, 0, this.containerSize.x * this.containerSize.y);
		if (this.items.Length < _other.items.Length)
		{
			List<ItemStack> list = new List<ItemStack>();
			for (int i = this.items.Length; i < _other.items.Length; i++)
			{
				list.Add(_other.items[i]);
			}
			Vector3 vector = base.ToWorldCenterPos();
			vector.y += 0.9f;
			GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
		}
		if (_other.HasSlotLocksSupport && _other.SlotLocks != null)
		{
			this.SlotLocks = _other.SlotLocks.Clone();
			this.SlotLocks.Length = this.items.Length;
			return;
		}
		this.SlotLocks = new PackedBoolArray(this.items.Length);
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00049F24 File Offset: 0x00048124
	public override void Reset(FastTags<TagGroup.Global> _questTags)
	{
		base.Reset(_questTags);
		if (this.bPlayerStorage)
		{
			return;
		}
		this.bTouched = false;
		this.bWasTouched = false;
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].Clear();
		}
		if (this.AlternateLootList != null)
		{
			for (int j = 0; j < this.AlternateLootList.Count; j++)
			{
				if (_questTags.Test_AnySet(this.AlternateLootList[j].tag))
				{
					this.lootListName = this.AlternateLootList[j].lootEntry;
					break;
				}
			}
		}
		base.SetModified();
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00049FC8 File Offset: 0x000481C8
	public override void UpdateTick(World _world)
	{
		base.UpdateTick(_world);
		if (base.Parent.PlayerPlaced)
		{
			return;
		}
		if (!this.bTouched || this.bPlayerStorage || !this.IsEmpty() || GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays) <= 0)
		{
			return;
		}
		int num = GameUtils.WorldTimeToTotalHours(this.worldTimeTouched);
		if ((GameUtils.WorldTimeToTotalHours(_world.worldTime) - num) / 24 >= GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays))
		{
			this.bWasTouched = false;
			this.bTouched = false;
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
			this.worldTimeTouched = _world.worldTime;
			base.SetModified();
			return;
		}
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x0004A0C4 File Offset: 0x000482C4
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
		if (!this.bTouched)
		{
			return string.Format(Localization.Get("lootTooltipNew", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.IsEmpty())
		{
			return string.Format(Localization.Get("lootTooltipEmpty", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		return string.Format(Localization.Get("lootTooltipTouched", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0004A1D6 File Offset: 0x000483D6
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Search", "search", true, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0004A200 File Offset: 0x00048400
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
		this.bWasTouched = this.bTouched;
		LockManager.Instance.LockRequestLocal(this, null, 0);
		return true;
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0004A2A3 File Offset: 0x000484A3
	// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x0004A2AB File Offset: 0x000484AB
	public string lootListName { get; set; }

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0004A2B4 File Offset: 0x000484B4
	// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0004A2CB File Offset: 0x000484CB
	public bool bPlayerStorage
	{
		get
		{
			return base.Parent.Owner != null || this.playerStorage;
		}
		set
		{
			this.playerStorage = value;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0004A2D4 File Offset: 0x000484D4
	// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x0004A2DC File Offset: 0x000484DC
	public PreferenceTracker preferences { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0004A2E5 File Offset: 0x000484E5
	// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x0004A2ED File Offset: 0x000484ED
	public ulong worldTimeTouched { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0004A2F6 File Offset: 0x000484F6
	// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x0004A308 File Offset: 0x00048508
	public bool bTouched
	{
		get
		{
			return this.bPlayerStorage || this.internalTouched;
		}
		set
		{
			this.internalTouched = value;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0004A311 File Offset: 0x00048511
	// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0004A319 File Offset: 0x00048519
	public bool bWasTouched { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0004A324 File Offset: 0x00048524
	// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0004A360 File Offset: 0x00048560
	public ItemStack[] items
	{
		get
		{
			ItemStack[] array;
			if ((array = this.itemsArr) == null)
			{
				array = (this.itemsArr = ItemStack.CreateArray(this.containerSize.x * this.containerSize.y));
			}
			return array;
		}
		set
		{
			this.itemsArr = value;
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0004A369 File Offset: 0x00048569
	public virtual Vector2i GetContainerSize()
	{
		return this.containerSize;
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0004A374 File Offset: 0x00048574
	public virtual void SetContainerSize(Vector2i _containerSize, bool _clearItems = true)
	{
		this.containerSize = _containerSize;
		if (!_clearItems)
		{
			return;
		}
		if (this.containerSize.x * this.containerSize.y != this.items.Length)
		{
			this.items = ItemStack.CreateArray(this.containerSize.x * this.containerSize.y);
			return;
		}
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i] = ItemStack.Empty;
		}
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0004A3F0 File Offset: 0x000485F0
	public void UpdateSlot(int _idx, ItemStack _item)
	{
		this.items[_idx] = _item.Clone();
		base.Parent.NotifyListeners();
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0004A40C File Offset: 0x0004860C
	public void RemoveItem(ItemValue _itemValue)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].itemValue.ItemClass == _itemValue.ItemClass)
			{
				this.UpdateSlot(i, ItemStack.Empty);
			}
		}
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0004A454 File Offset: 0x00048654
	public int RemoveItems(ItemValue _itemValue, int _count)
	{
		int num = _count;
		int num2 = 0;
		while (_count > 0 && num2 < this.items.Length - 1)
		{
			if (this.items[num2].itemValue.ItemClass == _itemValue.ItemClass)
			{
				if (_itemValue.ItemClass.CanStack())
				{
					int count = this.items[num2].count;
					int num3 = ((count >= _count) ? _count : count);
					this.items[num2].count -= num3;
					_count -= num3;
					if (this.items[num2].count <= 0)
					{
						this.items[num2].Clear();
					}
				}
				else
				{
					this.items[num2].Clear();
					_count--;
				}
			}
			num2++;
		}
		base.Parent.NotifyListeners();
		return num - _count;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0004A51C File Offset: 0x0004871C
	public bool IsEmpty()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (!this.items[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x0004A550 File Offset: 0x00048750
	public void SetEmpty()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].Clear();
		}
		PackedBoolArray slotLocks = this.SlotLocks;
		if (slotLocks != null)
		{
			slotLocks.Clear();
		}
		base.Parent.NotifyListeners();
		this.bTouched = true;
		base.SetModified();
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x0004A5A8 File Offset: 0x000487A8
	public bool ShouldDestroyOnClose()
	{
		LootContainer.DestroyOnClose destroyOnClose = LootContainer.GetLootContainer(this.lootListName, true).destroyOnClose;
		return destroyOnClose == LootContainer.DestroyOnClose.True || (destroyOnClose == LootContainer.DestroyOnClose.Empty && this.IsEmpty());
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00020021 File Offset: 0x0001E221
	public bool HasSlotLocksSupport
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0004A5D9 File Offset: 0x000487D9
	// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0004A5E1 File Offset: 0x000487E1
	public PackedBoolArray SlotLocks { get; set; }

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0004A5EC File Offset: 0x000487EC
	public bool AddItem(ItemStack _itemStack)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].IsEmpty())
			{
				this.UpdateSlot(i, _itemStack);
				base.SetModified();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x0004A62C File Offset: 0x0004882C
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public ValueTuple<bool, bool> TryStackItem(int _startIndex, ItemStack _itemStack)
	{
		bool flag = false;
		int count = _itemStack.count;
		for (int i = _startIndex; i < this.items.Length; i++)
		{
			int count2 = _itemStack.count;
			if (_itemStack.itemValue.type == this.items[i].itemValue.type && this.items[i].CanStackPartly(ref count2))
			{
				this.items[i].count += count2;
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

	// Token: 0x06000BCA RID: 3018 RVA: 0x0004A6E4 File Offset: 0x000488E4
	public bool HasItem(ItemValue _item)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].itemValue.type == _item.type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x0004A724 File Offset: 0x00048924
	public int CountItem(ItemClass _itemClass)
	{
		int num = 0;
		for (int i = 0; i < this.items.Length; i++)
		{
			if (this.items[i].itemValue.ItemClass == _itemClass)
			{
				num += this.items[i].count;
			}
		}
		return num;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0004A76C File Offset: 0x0004896C
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			num = 18;
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
		this.containerSize = new Vector2i
		{
			x = (int)_br.ReadUInt16(),
			y = (int)_br.ReadUInt16()
		};
		this.bTouched = _br.ReadBoolean();
		this.worldTimeTouched = (ulong)_br.ReadUInt32();
		this.bPlayerStorage = _br.ReadBoolean();
		int num2 = Math.Min((int)_br.ReadInt16(), this.containerSize.x * this.containerSize.y);
		if (base.Parent.IsUserAccessing())
		{
			ItemStack empty = ItemStack.Empty;
			for (int i = 0; i < num2; i++)
			{
				empty.Read(_br);
			}
		}
		else
		{
			if (this.containerSize.x * this.containerSize.y != this.items.Length)
			{
				this.items = ItemStack.CreateArray(this.containerSize.x * this.containerSize.y);
			}
			for (int j = 0; j < num2; j++)
			{
				this.items[j].Clear();
				this.items[j].Read(_br);
			}
		}
		if (_br.ReadBoolean())
		{
			this.preferences = new PreferenceTracker(-1);
			this.preferences.Read(_br);
		}
		if (num >= 12)
		{
			this.SlotLocks = new PackedBoolArray(0);
			this.SlotLocks.Read(_br);
			return;
		}
		this.SlotLocks = new PackedBoolArray(this.items.Length);
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x0004A920 File Offset: 0x00048B20
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		bool flag = !string.IsNullOrEmpty(this.lootListName);
		_bw.Write(flag);
		if (flag)
		{
			_bw.Write(this.lootListName);
		}
		_bw.Write((ushort)this.containerSize.x);
		_bw.Write((ushort)this.containerSize.y);
		_bw.Write(this.bTouched);
		_bw.Write((uint)this.worldTimeTouched);
		_bw.Write(this.bPlayerStorage);
		_bw.Write((short)this.items.Length);
		ItemStack[] items = this.items;
		for (int i = 0; i < items.Length; i++)
		{
			items[i].Clone().Write(_bw);
		}
		bool flag2 = this.preferences != null;
		_bw.Write(flag2);
		if (flag2)
		{
			this.preferences.Write(_bw);
		}
		if (this.SlotLocks == null)
		{
			this.SlotLocks = new PackedBoolArray(this.items.Length);
		}
		this.SlotLocks.Write(_bw);
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0004AA2A File Offset: 0x00048C2A
	public override void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		if (_success)
		{
			this.PopulateTE(_lockingPlayerID);
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x0004AA36 File Offset: 0x00048C36
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		this.ShowUI(_success);
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0004AA3F File Offset: 0x00048C3F
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		GameManager.Instance.CheckDestroyTileEntity(this, base.ToWorldPos());
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0004AA54 File Offset: 0x00048C54
	[PublicizedFrom(EAccessModifier.Private)]
	public void PopulateTE(int _playerThatOpened)
	{
		BlockValue block = GameManager.Instance.World.GetBlock(base.ToWorldPos());
		GameManager.Instance.lootManager.LootContainerOpened(this, _playerThatOpened, block.Block.Tags);
		this.bTouched = true;
		base.SetModified();
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0004AAA4 File Offset: 0x00048CA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowUI(bool _lockGranted)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_lockGranted)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		BlockValue block = GameManager.Instance.World.GetBlock(base.ToWorldPos());
		string localizedBlockName = block.Block.GetLocalizedBlockName();
		((XUiC_LootWindowGroup)uiforPrimaryPlayer.xui.FindWindowGroupByName(XUiC_LootWindowGroup.ID)).OpenLooting(localizedBlockName, this);
		LootContainer lootContainer = LootContainer.GetLootContainer(this.lootListName, true);
		if (lootContainer != null && uiforPrimaryPlayer.entityPlayer != null)
		{
			lootContainer.ExecuteBuffActions(uiforPrimaryPlayer.entityPlayer.entityId, uiforPrimaryPlayer.entityPlayer);
		}
		if (!GameManager.Instance.World.IsEditor() && !this.bTouched)
		{
			EntityPlayerLocal entityPlayer = uiforPrimaryPlayer.entityPlayer;
			entityPlayer.MinEventContext.TileEntity = this;
			entityPlayer.MinEventContext.BlockValue = block;
			entityPlayer.FireEvent(MinEventTypes.onSelfOpenLootContainer, true);
		}
	}

	// Token: 0x04000AAB RID: 2731
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000AAC RID: 2732
	public static string PropLootList = "LootList";

	// Token: 0x04000AAD RID: 2733
	public static string PropAlternateLootList = "AlternateLootList";

	// Token: 0x04000AAE RID: 2734
	public static string PropLootStageMod = "LootStageMod";

	// Token: 0x04000AAF RID: 2735
	public static string PropLootStageBonus = "LootStageBonus";

	// Token: 0x04000AB0 RID: 2736
	public static string PropIsJammed = "IsJammed";

	// Token: 0x04000AB1 RID: 2737
	public static string PropIsQuestLoot = "IsQuestLoot";

	// Token: 0x04000AB2 RID: 2738
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000AB3 RID: 2739
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000AB6 RID: 2742
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isJammed;

	// Token: 0x04000AB7 RID: 2743
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isQuestLoot;

	// Token: 0x04000AB8 RID: 2744
	public List<TEFeatureStorage.AlternateLootEntry> AlternateLootList;

	// Token: 0x04000AB9 RID: 2745
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> entityTempList;

	// Token: 0x04000ABA RID: 2746
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSize = Vector2i.one;

	// Token: 0x04000ABB RID: 2747
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] itemsArr;

	// Token: 0x04000ABD RID: 2749
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerStorage;

	// Token: 0x04000AC0 RID: 2752
	[PublicizedFrom(EAccessModifier.Private)]
	public bool internalTouched;

	// Token: 0x02000186 RID: 390
	public struct AlternateLootEntry
	{
		// Token: 0x04000AC3 RID: 2755
		public FastTags<TagGroup.Global> tag;

		// Token: 0x04000AC4 RID: 2756
		public string lootEntry;
	}
}
