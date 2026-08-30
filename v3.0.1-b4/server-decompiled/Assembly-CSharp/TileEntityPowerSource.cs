using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C67 RID: 3175
public class TileEntityPowerSource : TileEntityPowered
{
	// Token: 0x06006130 RID: 24880 RVA: 0x00265C67 File Offset: 0x00263E67
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x06006131 RID: 24881 RVA: 0x00265C7A File Offset: 0x00263E7A
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x06006132 RID: 24882 RVA: 0x00265C82 File Offset: 0x00263E82
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x06006133 RID: 24883 RVA: 0x00265C94 File Offset: 0x00263E94
	// (set) Token: 0x06006134 RID: 24884 RVA: 0x00265CDE File Offset: 0x00263EDE
	public ItemClass SlotItem
	{
		get
		{
			if (this.slotItem == null)
			{
				this.slotItem = ItemClass.GetItemClass((this.chunk.GetBlock(base.localChunkPos).Block as BlockPowerSource).SlotItemName, false);
			}
			return this.slotItem;
		}
		set
		{
			this.slotItem = value;
		}
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x00265CE7 File Offset: 0x00263EE7
	public TileEntityPowerSource(Chunk _chunk)
		: base(_chunk)
	{
		this.ownerID = null;
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x00265D09 File Offset: 0x00263F09
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x00265D17 File Offset: 0x00263F17
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource(TileEntityPowerSource _other)
		: base(null)
	{
		this.ownerID = _other.ownerID;
		this.PowerItem = _other.PowerItem;
	}

	// Token: 0x06006138 RID: 24888 RVA: 0x00265D4A File Offset: 0x00263F4A
	public override TileEntity Clone()
	{
		return new TileEntityPowerSource(this);
	}

	// Token: 0x06006139 RID: 24889 RVA: 0x00265D54 File Offset: 0x00263F54
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x0600613A RID: 24890 RVA: 0x00265D94 File Offset: 0x00263F94
	public override bool CanHaveParent(IPowered powered)
	{
		return this.PowerItemType == PowerItem.PowerItemTypes.BatteryBank;
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x00265DA0 File Offset: 0x00263FA0
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.IsOn && base.IsByWater(world, base.ToWorldPos()))
		{
			(this.PowerItem as PowerSource).IsOn = false;
			base.SetModified();
		}
		if (this.bUserAccessing && this.IsOn)
		{
			base.SetModified();
		}
	}

	// Token: 0x0600613C RID: 24892 RVA: 0x00265E08 File Offset: 0x00264008
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
		{
			if (base.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.GetLegacyForkVersion();
			}
		}
		if (this.ClientData == null)
		{
			this.ClientData = new TileEntityPowerSource.ClientPowerData();
		}
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeRead.FromClient)
			{
				this.bUserAccessing = _br.ReadBoolean();
				if (this.PowerItem == null)
				{
					this.PowerItem = base.CreatePowerItemForTileEntity((ushort)this.chunk.GetBlock(base.localChunkPos).type);
				}
				this.ClientData.AddedFuel = _br.ReadUInt16();
				if (this.ClientData.AddedFuel > 0)
				{
					ushort num = this.CurrentFuel + this.ClientData.AddedFuel;
					if (num > this.MaxFuel)
					{
						num = this.MaxFuel;
					}
					(this.PowerItem as PowerGenerator).CurrentFuel = num;
					this.ClientData.AddedFuel = 0;
					base.SetModified();
				}
				if (_br.ReadBoolean())
				{
					this.ClientData.ItemSlots = GameUtils.ReadItemStack(_br);
					(this.PowerItem as PowerSource).SetSlots(this.ClientData.ItemSlots);
					return;
				}
			}
			else if (_br.ReadBoolean())
			{
				this.ClientData.IsOn = _br.ReadBoolean();
				if (this.PowerItemType == PowerItem.PowerItemTypes.Generator)
				{
					this.ClientData.MaxFuel = _br.ReadUInt16();
					this.ClientData.CurrentFuel = _br.ReadUInt16();
				}
				else if (this.PowerItemType == PowerItem.PowerItemTypes.SolarPanel)
				{
					this.ClientData.SolarInput = _br.ReadUInt16();
				}
				ItemStack[] array = GameUtils.ReadItemStack(_br);
				if (!this.bUserAccessing || (this.bUserAccessing && this.IsOn))
				{
					this.ClientData.ItemSlots = array;
				}
				this.ClientData.MaxOutput = _br.ReadUInt16();
				this.ClientData.LastOutput = _br.ReadUInt16();
			}
		}
	}

	// Token: 0x0600613D RID: 24893 RVA: 0x00265FE0 File Offset: 0x002641E0
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (_eStreamMode != TileEntity.StreamModeWrite.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeWrite.ToServer)
			{
				_bw.Write(this.bUserAccessing);
				_bw.Write(this.ClientData.AddedFuel);
				this.ClientData.AddedFuel = 0;
				_bw.Write(this.ClientData.SendSlots);
				if (this.ClientData.SendSlots)
				{
					GameUtils.WriteItemStack(_bw, this.ClientData.ItemSlots);
					this.ClientData.SendSlots = false;
					return;
				}
			}
			else
			{
				PowerSource powerSource = this.PowerItem as PowerSource;
				_bw.Write(powerSource != null);
				if (powerSource != null)
				{
					_bw.Write(powerSource.IsOn);
					if (this.PowerItemType == PowerItem.PowerItemTypes.Generator)
					{
						_bw.Write((powerSource as PowerGenerator).MaxFuel);
						_bw.Write((powerSource as PowerGenerator).CurrentFuel);
					}
					else if (this.PowerItemType == PowerItem.PowerItemTypes.SolarPanel)
					{
						_bw.Write((powerSource as PowerSolarPanel).InputFromSun);
					}
					GameUtils.WriteItemStack(_bw, powerSource.Stacks);
					_bw.Write(powerSource.MaxOutput);
					_bw.Write(powerSource.LastPowerUsed);
				}
			}
		}
	}

	// Token: 0x0600613E RID: 24894 RVA: 0x00266104 File Offset: 0x00264304
	public bool HasSlottedItems()
	{
		ItemStack[] itemSlots = this.ItemSlots;
		for (int i = 0; i < itemSlots.Length; i++)
		{
			if (!itemSlots[i].IsEmpty())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x0600613F RID: 24895 RVA: 0x00266134 File Offset: 0x00264334
	public bool IsOn
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				PowerSource powerSource = this.PowerItem as PowerSource;
				return powerSource != null && powerSource.IsOn;
			}
			return this.ClientData.IsOn;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06006140 RID: 24896 RVA: 0x00266170 File Offset: 0x00264370
	// (set) Token: 0x06006141 RID: 24897 RVA: 0x0026619A File Offset: 0x0026439A
	public ushort CurrentFuel
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerGenerator).CurrentFuel;
			}
			return this.ClientData.CurrentFuel;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerGenerator).CurrentFuel = value;
				return;
			}
			this.ClientData.CurrentFuel = value;
			base.SetModified();
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06006142 RID: 24898 RVA: 0x002661CC File Offset: 0x002643CC
	public ushort MaxFuel
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerGenerator).MaxFuel;
			}
			return this.ClientData.MaxFuel;
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06006143 RID: 24899 RVA: 0x002661F6 File Offset: 0x002643F6
	public ushort MaxOutput
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerSource).MaxOutput;
			}
			return this.ClientData.MaxOutput;
		}
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06006144 RID: 24900 RVA: 0x00266220 File Offset: 0x00264420
	public ushort LastOutput
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerSource).LastPowerUsed;
			}
			return this.ClientData.LastOutput;
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06006145 RID: 24901 RVA: 0x0026624A File Offset: 0x0026444A
	// (set) Token: 0x06006146 RID: 24902 RVA: 0x00266274 File Offset: 0x00264474
	public ItemStack[] ItemSlots
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerSource).Stacks;
			}
			return this.ClientData.ItemSlots;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerSource).SetSlots(value);
				return;
			}
			this.ClientData.ItemSlots = value;
			base.SetModified();
		}
	}

	// Token: 0x06006147 RID: 24903 RVA: 0x002662A8 File Offset: 0x002644A8
	public bool TryAddItemToSlot(ItemClass itemClass, ItemStack itemStack)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return (this.PowerItem as PowerSource).TryAddItemToSlot(itemClass, itemStack);
		}
		if (!this.IsOn)
		{
			for (int i = 0; i < this.ClientData.ItemSlots.Length; i++)
			{
				if (this.ClientData.ItemSlots[i].IsEmpty())
				{
					this.ClientData.ItemSlots[i] = itemStack;
					this.ClientData.SendSlots = true;
					base.SetModified();
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006148 RID: 24904 RVA: 0x0026632C File Offset: 0x0026452C
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			PowerSource powerSource = this.PowerItem as PowerSource;
			powerSource.HandleDisconnect();
			PowerManager.Instance.RemovePowerNode(powerSource);
		}
	}

	// Token: 0x06006149 RID: 24905 RVA: 0x00266368 File Offset: 0x00264568
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		TileEntityPowerSource tileEntityPowerSource;
		if (_teNew.TryGetSelfOrFeature<TileEntityPowerSource>(out tileEntityPowerSource))
		{
			return;
		}
		List<ItemStack> list = new List<ItemStack>();
		list.AddRange(this.ItemSlots);
		if (this.PowerItemType == PowerItem.PowerItemTypes.Generator && this.CurrentFuel > 0)
		{
			ItemValue itemValue = new ItemValue(ItemClass.GetItemWithTag(XUiC_PowerSourceStats.tag).Id, false);
			int value = itemValue.ItemClass.Stacknumber.Value;
			int num;
			for (int i = (int)this.CurrentFuel; i > 0; i -= num)
			{
				num = Mathf.Min(i, value);
				list.Add(new ItemStack(itemValue, num));
			}
		}
		Vector3 vector = base.ToWorldCenterPos();
		vector.y += 0.9f;
		GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
	}

	// Token: 0x0600614A RID: 24906 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerSource;
	}

	// Token: 0x0600614B RID: 24907 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x0600614C RID: 24908 RVA: 0x00266440 File Offset: 0x00264640
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		((XUiC_PowerSourceWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("powersource")).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open("powersource", true);
	}

	// Token: 0x04004BAB RID: 19371
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004BAC RID: 19372
	public bool syncNeeded = true;

	// Token: 0x04004BAD RID: 19373
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004BAE RID: 19374
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass slotItem;

	// Token: 0x04004BAF RID: 19375
	public TileEntityPowerSource.ClientPowerData ClientData = new TileEntityPowerSource.ClientPowerData();

	// Token: 0x02000C68 RID: 3176
	public class ClientPowerData
	{
		// Token: 0x0600614D RID: 24909 RVA: 0x002664B8 File Offset: 0x002646B8
		public ClientPowerData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004BB0 RID: 19376
		public bool IsOn;

		// Token: 0x04004BB1 RID: 19377
		public ushort MaxFuel;

		// Token: 0x04004BB2 RID: 19378
		public ushort CurrentFuel;

		// Token: 0x04004BB3 RID: 19379
		public ushort SolarInput;

		// Token: 0x04004BB4 RID: 19380
		public ushort MaxOutput;

		// Token: 0x04004BB5 RID: 19381
		public ushort LastOutput;

		// Token: 0x04004BB6 RID: 19382
		public ushort AddedFuel;

		// Token: 0x04004BB7 RID: 19383
		public bool SendSlots;

		// Token: 0x04004BB8 RID: 19384
		public ItemStack[] ItemSlots = new ItemStack[6];
	}
}
