using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C88 RID: 3208
public class TileEntityPowerSource : TileEntityPowered
{
	// Token: 0x060061B5 RID: 25013 RVA: 0x0026BFF7 File Offset: 0x0026A1F7
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061B6 RID: 25014 RVA: 0x0026C00A File Offset: 0x0026A20A
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x0026C012 File Offset: 0x0026A212
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060061B8 RID: 25016 RVA: 0x0026C024 File Offset: 0x0026A224
	// (set) Token: 0x060061B9 RID: 25017 RVA: 0x0026C06E File Offset: 0x0026A26E
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

	// Token: 0x060061BA RID: 25018 RVA: 0x0026C077 File Offset: 0x0026A277
	public TileEntityPowerSource(Chunk _chunk)
		: base(_chunk)
	{
		this.ownerID = null;
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x0026C099 File Offset: 0x0026A299
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x0026C0A7 File Offset: 0x0026A2A7
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource(TileEntityPowerSource _other)
		: base(null)
	{
		this.ownerID = _other.ownerID;
		this.PowerItem = _other.PowerItem;
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x0026C0DA File Offset: 0x0026A2DA
	public override TileEntity Clone()
	{
		return new TileEntityPowerSource(this);
	}

	// Token: 0x060061BE RID: 25022 RVA: 0x0026C0E4 File Offset: 0x0026A2E4
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x060061BF RID: 25023 RVA: 0x0026C124 File Offset: 0x0026A324
	public override bool CanHaveParent(IPowered powered)
	{
		return this.PowerItemType == PowerItem.PowerItemTypes.BatteryBank;
	}

	// Token: 0x060061C0 RID: 25024 RVA: 0x0026C130 File Offset: 0x0026A330
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

	// Token: 0x060061C1 RID: 25025 RVA: 0x0026C198 File Offset: 0x0026A398
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
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
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			if (_eStreamMode == StreamModeRead.FromClient)
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

	// Token: 0x060061C2 RID: 25026 RVA: 0x0026C370 File Offset: 0x0026A570
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (_eStreamMode != StreamModeWrite.Persistency)
		{
			if (_eStreamMode == StreamModeWrite.ToServer)
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

	// Token: 0x060061C3 RID: 25027 RVA: 0x0026C494 File Offset: 0x0026A694
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

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060061C4 RID: 25028 RVA: 0x0026C4C4 File Offset: 0x0026A6C4
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

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x060061C5 RID: 25029 RVA: 0x0026C500 File Offset: 0x0026A700
	// (set) Token: 0x060061C6 RID: 25030 RVA: 0x0026C52A File Offset: 0x0026A72A
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

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x060061C7 RID: 25031 RVA: 0x0026C55C File Offset: 0x0026A75C
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

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x060061C8 RID: 25032 RVA: 0x0026C586 File Offset: 0x0026A786
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

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x060061C9 RID: 25033 RVA: 0x0026C5B0 File Offset: 0x0026A7B0
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

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060061CA RID: 25034 RVA: 0x0026C5DA File Offset: 0x0026A7DA
	// (set) Token: 0x060061CB RID: 25035 RVA: 0x0026C604 File Offset: 0x0026A804
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

	// Token: 0x060061CC RID: 25036 RVA: 0x0026C638 File Offset: 0x0026A838
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

	// Token: 0x060061CD RID: 25037 RVA: 0x0026C6BC File Offset: 0x0026A8BC
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

	// Token: 0x060061CE RID: 25038 RVA: 0x0026C6F8 File Offset: 0x0026A8F8
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
			int maxCount = itemValue.ItemClass.MaxCount;
			int num;
			for (int i = (int)this.CurrentFuel; i > 0; i -= num)
			{
				num = Mathf.Min(i, maxCount);
				list.Add(new ItemStack(itemValue, num));
			}
		}
		Vector3 vector = base.ToWorldCenterPos();
		vector.y += 0.9f;
		GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
	}

	// Token: 0x060061CF RID: 25039 RVA: 0x0026C7C8 File Offset: 0x0026A9C8
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerSource;
	}

	// Token: 0x060061D0 RID: 25040 RVA: 0x0026C7CC File Offset: 0x0026A9CC
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		((XUiC_PowerSourceWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("powersource")).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open("powersource", true);
		return true;
	}

	// Token: 0x04004C7F RID: 19583
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C80 RID: 19584
	public bool syncNeeded = true;

	// Token: 0x04004C81 RID: 19585
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C82 RID: 19586
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass slotItem;

	// Token: 0x04004C83 RID: 19587
	public TileEntityPowerSource.ClientPowerData ClientData = new TileEntityPowerSource.ClientPowerData();

	// Token: 0x02000C89 RID: 3209
	public class ClientPowerData
	{
		// Token: 0x060061D1 RID: 25041 RVA: 0x0026C844 File Offset: 0x0026AA44
		public ClientPowerData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004C84 RID: 19588
		public bool IsOn;

		// Token: 0x04004C85 RID: 19589
		public ushort MaxFuel;

		// Token: 0x04004C86 RID: 19590
		public ushort CurrentFuel;

		// Token: 0x04004C87 RID: 19591
		public ushort SolarInput;

		// Token: 0x04004C88 RID: 19592
		public ushort MaxOutput;

		// Token: 0x04004C89 RID: 19593
		public ushort LastOutput;

		// Token: 0x04004C8A RID: 19594
		public ushort AddedFuel;

		// Token: 0x04004C8B RID: 19595
		public bool SendSlots;

		// Token: 0x04004C8C RID: 19596
		public ItemStack[] ItemSlots = new ItemStack[6];
	}
}
