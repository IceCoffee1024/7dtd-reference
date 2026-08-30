using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C79 RID: 3193
public class TileEntityPowerSource : TileEntityPowered
{
	// Token: 0x060061BB RID: 25019 RVA: 0x00268B53 File Offset: 0x00266D53
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x00268B66 File Offset: 0x00266D66
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x00268B6E File Offset: 0x00266D6E
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x060061BE RID: 25022 RVA: 0x00268B80 File Offset: 0x00266D80
	// (set) Token: 0x060061BF RID: 25023 RVA: 0x00268BCA File Offset: 0x00266DCA
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

	// Token: 0x060061C0 RID: 25024 RVA: 0x00268BD3 File Offset: 0x00266DD3
	public TileEntityPowerSource(Chunk _chunk)
		: base(_chunk)
	{
		this.ownerID = null;
	}

	// Token: 0x060061C1 RID: 25025 RVA: 0x00268BF5 File Offset: 0x00266DF5
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x060061C2 RID: 25026 RVA: 0x00268C03 File Offset: 0x00266E03
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource(TileEntityPowerSource _other)
		: base(null)
	{
		this.ownerID = _other.ownerID;
		this.PowerItem = _other.PowerItem;
	}

	// Token: 0x060061C3 RID: 25027 RVA: 0x00268C36 File Offset: 0x00266E36
	public override TileEntity Clone()
	{
		return new TileEntityPowerSource(this);
	}

	// Token: 0x060061C4 RID: 25028 RVA: 0x00268C40 File Offset: 0x00266E40
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x060061C5 RID: 25029 RVA: 0x00268C80 File Offset: 0x00266E80
	public override bool CanHaveParent(IPowered powered)
	{
		return this.PowerItemType == PowerItem.PowerItemTypes.BatteryBank;
	}

	// Token: 0x060061C6 RID: 25030 RVA: 0x00268C8C File Offset: 0x00266E8C
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

	// Token: 0x060061C7 RID: 25031 RVA: 0x00268CF4 File Offset: 0x00266EF4
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

	// Token: 0x060061C8 RID: 25032 RVA: 0x00268ECC File Offset: 0x002670CC
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

	// Token: 0x060061C9 RID: 25033 RVA: 0x00268FF0 File Offset: 0x002671F0
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

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x060061CA RID: 25034 RVA: 0x00269020 File Offset: 0x00267220
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

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x060061CB RID: 25035 RVA: 0x0026905C File Offset: 0x0026725C
	// (set) Token: 0x060061CC RID: 25036 RVA: 0x00269086 File Offset: 0x00267286
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

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x060061CD RID: 25037 RVA: 0x002690B8 File Offset: 0x002672B8
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

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x060061CE RID: 25038 RVA: 0x002690E2 File Offset: 0x002672E2
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

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x060061CF RID: 25039 RVA: 0x0026910C File Offset: 0x0026730C
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

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x060061D0 RID: 25040 RVA: 0x00269136 File Offset: 0x00267336
	// (set) Token: 0x060061D1 RID: 25041 RVA: 0x00269160 File Offset: 0x00267360
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

	// Token: 0x060061D2 RID: 25042 RVA: 0x00269194 File Offset: 0x00267394
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

	// Token: 0x060061D3 RID: 25043 RVA: 0x00269218 File Offset: 0x00267418
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

	// Token: 0x060061D4 RID: 25044 RVA: 0x00269254 File Offset: 0x00267454
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

	// Token: 0x060061D5 RID: 25045 RVA: 0x0017E246 File Offset: 0x0017C446
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerSource;
	}

	// Token: 0x060061D6 RID: 25046 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060061D7 RID: 25047 RVA: 0x00269324 File Offset: 0x00267524
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

	// Token: 0x04004C48 RID: 19528
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C49 RID: 19529
	public bool syncNeeded = true;

	// Token: 0x04004C4A RID: 19530
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C4B RID: 19531
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass slotItem;

	// Token: 0x04004C4C RID: 19532
	public TileEntityPowerSource.ClientPowerData ClientData = new TileEntityPowerSource.ClientPowerData();

	// Token: 0x02000C7A RID: 3194
	public class ClientPowerData
	{
		// Token: 0x060061D8 RID: 25048 RVA: 0x0026939C File Offset: 0x0026759C
		public ClientPowerData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004C4D RID: 19533
		public bool IsOn;

		// Token: 0x04004C4E RID: 19534
		public ushort MaxFuel;

		// Token: 0x04004C4F RID: 19535
		public ushort CurrentFuel;

		// Token: 0x04004C50 RID: 19536
		public ushort SolarInput;

		// Token: 0x04004C51 RID: 19537
		public ushort MaxOutput;

		// Token: 0x04004C52 RID: 19538
		public ushort LastOutput;

		// Token: 0x04004C53 RID: 19539
		public ushort AddedFuel;

		// Token: 0x04004C54 RID: 19540
		public bool SendSlots;

		// Token: 0x04004C55 RID: 19541
		public ItemStack[] ItemSlots = new ItemStack[6];
	}
}
