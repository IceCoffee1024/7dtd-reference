using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000183 RID: 387
[Preserve]
public class TEFeatureStorage : TEFeatureAbs, ITileEntityLootable, ITileEntity, ILockTarget, IInventory
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00049715 File Offset: 0x00047915
	// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0004971D File Offset: 0x0004791D
	public float LootStageMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00049726 File Offset: 0x00047926
	// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0004972E File Offset: 0x0004792E
	public float LootStageBonus
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00049738 File Offset: 0x00047938
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

	// Token: 0x06000B99 RID: 2969 RVA: 0x000498EC File Offset: 0x00047AEC
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

	// Token: 0x06000B9A RID: 2970 RVA: 0x00049944 File Offset: 0x00047B44
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (!GameManager.IsDedicatedServer)
		{
			XUiC_LootWindowGroup.CloseIfOpenAtPos(base.ToWorldPos(), null);
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0004995F File Offset: 0x00047B5F
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && base.Parent.Owner != null)
		{
			this.worldTimeTouched = GameManager.Instance.World.GetWorldTime();
			this.SetEmpty();
		}
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x000499A0 File Offset: 0x00047BA0
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

	// Token: 0x06000B9D RID: 2973 RVA: 0x000499E0 File Offset: 0x00047BE0
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		ITileEntityLootable tileEntityLootable;
		if (!_teNew.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
		{
			GameManager.Instance.DropContentOfLootContainerServer(_bvOld, base.ToWorldPos(), this);
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00049A14 File Offset: 0x00047C14
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

	// Token: 0x06000B9F RID: 2975 RVA: 0x00049B0C File Offset: 0x00047D0C
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

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00049BB0 File Offset: 0x00047DB0
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

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00049CAC File Offset: 0x00047EAC
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

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00049DBE File Offset: 0x00047FBE
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Search", "search", true, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00049DE8 File Offset: 0x00047FE8
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
	// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x00049E8B File Offset: 0x0004808B
	// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x00049E93 File Offset: 0x00048093
	public string lootListName { get; set; }

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00049E9C File Offset: 0x0004809C
	// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00049EB3 File Offset: 0x000480B3
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
	// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00049EBC File Offset: 0x000480BC
	// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00049EC4 File Offset: 0x000480C4
	public PreferenceTracker preferences { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00049ECD File Offset: 0x000480CD
	// (set) Token: 0x06000BAB RID: 2987 RVA: 0x00049ED5 File Offset: 0x000480D5
	public ulong worldTimeTouched { get; set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00049EDE File Offset: 0x000480DE
	// (set) Token: 0x06000BAD RID: 2989 RVA: 0x00049EF0 File Offset: 0x000480F0
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
	// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00049EF9 File Offset: 0x000480F9
	// (set) Token: 0x06000BAF RID: 2991 RVA: 0x00049F01 File Offset: 0x00048101
	public bool bWasTouched { get; set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00049F0C File Offset: 0x0004810C
	// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x00049F48 File Offset: 0x00048148
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

	// Token: 0x06000BB2 RID: 2994 RVA: 0x00049F51 File Offset: 0x00048151
	public virtual Vector2i GetContainerSize()
	{
		return this.containerSize;
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x00049F5C File Offset: 0x0004815C
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

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00049FD8 File Offset: 0x000481D8
	public void UpdateSlot(int _idx, ItemStack _item)
	{
		this.items[_idx] = _item.Clone();
		base.Parent.NotifyListeners();
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00049FF4 File Offset: 0x000481F4
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

	// Token: 0x06000BB6 RID: 2998 RVA: 0x0004A03C File Offset: 0x0004823C
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

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0004A104 File Offset: 0x00048304
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

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0004A138 File Offset: 0x00048338
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

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0004A190 File Offset: 0x00048390
	public bool ShouldDestroyOnClose()
	{
		LootContainer.DestroyOnClose destroyOnClose = LootContainer.GetLootContainer(this.lootListName, true).destroyOnClose;
		return destroyOnClose == LootContainer.DestroyOnClose.True || (destroyOnClose == LootContainer.DestroyOnClose.Empty && this.IsEmpty());
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool HasSlotLocksSupport
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0004A1C1 File Offset: 0x000483C1
	// (set) Token: 0x06000BBC RID: 3004 RVA: 0x0004A1C9 File Offset: 0x000483C9
	public PackedBoolArray SlotLocks { get; set; }

	// Token: 0x06000BBD RID: 3005 RVA: 0x0004A1D4 File Offset: 0x000483D4
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

	// Token: 0x06000BBE RID: 3006 RVA: 0x0004A214 File Offset: 0x00048414
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

	// Token: 0x06000BBF RID: 3007 RVA: 0x0004A2CC File Offset: 0x000484CC
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

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0004A30C File Offset: 0x0004850C
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

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0004A354 File Offset: 0x00048554
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

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0004A508 File Offset: 0x00048708
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

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x0004A612 File Offset: 0x00048812
	public override void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		if (_success)
		{
			this.PopulateTE(_lockingPlayerID);
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x0004A61E File Offset: 0x0004881E
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		this.ShowUI(_success);
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x0004A627 File Offset: 0x00048827
	public override void OnUnlockedServer(int _unlockingPlayerId, ushort _channel)
	{
		GameManager.Instance.CheckDestroyTileEntity(this, base.ToWorldPos());
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0004A63C File Offset: 0x0004883C
	[PublicizedFrom(EAccessModifier.Private)]
	public void PopulateTE(int _playerThatOpened)
	{
		BlockValue block = GameManager.Instance.World.GetBlock(base.ToWorldPos());
		GameManager.Instance.lootManager.LootContainerOpened(this, _playerThatOpened, block.Block.Tags);
		this.bTouched = true;
		base.SetModified();
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0004A68C File Offset: 0x0004888C
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

	// Token: 0x04000A9E RID: 2718
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A9F RID: 2719
	public static string PropLootList = "LootList";

	// Token: 0x04000AA0 RID: 2720
	public static string PropAlternateLootList = "AlternateLootList";

	// Token: 0x04000AA1 RID: 2721
	public static string PropLootStageMod = "LootStageMod";

	// Token: 0x04000AA2 RID: 2722
	public static string PropLootStageBonus = "LootStageBonus";

	// Token: 0x04000AA3 RID: 2723
	public static string PropIsJammed = "IsJammed";

	// Token: 0x04000AA4 RID: 2724
	public static string PropIsQuestLoot = "IsQuestLoot";

	// Token: 0x04000AA5 RID: 2725
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000AA6 RID: 2726
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000AA9 RID: 2729
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isJammed;

	// Token: 0x04000AAA RID: 2730
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isQuestLoot;

	// Token: 0x04000AAB RID: 2731
	public List<TEFeatureStorage.AlternateLootEntry> AlternateLootList;

	// Token: 0x04000AAC RID: 2732
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> entityTempList;

	// Token: 0x04000AAD RID: 2733
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSize = Vector2i.one;

	// Token: 0x04000AAE RID: 2734
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] itemsArr;

	// Token: 0x04000AB0 RID: 2736
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playerStorage;

	// Token: 0x04000AB3 RID: 2739
	[PublicizedFrom(EAccessModifier.Private)]
	public bool internalTouched;

	// Token: 0x02000184 RID: 388
	public struct AlternateLootEntry
	{
		// Token: 0x04000AB6 RID: 2742
		public FastTags<TagGroup.Global> tag;

		// Token: 0x04000AB7 RID: 2743
		public string lootEntry;
	}
}
