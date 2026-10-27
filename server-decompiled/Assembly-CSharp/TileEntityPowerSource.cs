using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C93 RID: 3219
public class TileEntityPowerSource : TileEntityPowered
{
	// Token: 0x060061DA RID: 25050 RVA: 0x0026D51F File Offset: 0x0026B71F
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061DB RID: 25051 RVA: 0x0026D532 File Offset: 0x0026B732
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061DC RID: 25052 RVA: 0x0026D53A File Offset: 0x0026B73A
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x060061DD RID: 25053 RVA: 0x0026D54C File Offset: 0x0026B74C
	// (set) Token: 0x060061DE RID: 25054 RVA: 0x0026D596 File Offset: 0x0026B796
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

	// Token: 0x060061DF RID: 25055 RVA: 0x0026D59F File Offset: 0x0026B79F
	public TileEntityPowerSource(Chunk _chunk)
		: base(_chunk)
	{
		this.ownerID = null;
	}

	// Token: 0x060061E0 RID: 25056 RVA: 0x0026D5C1 File Offset: 0x0026B7C1
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x060061E1 RID: 25057 RVA: 0x0026D5CF File Offset: 0x0026B7CF
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityPowerSource(TileEntityPowerSource _other)
		: base(null)
	{
		this.ownerID = _other.ownerID;
		this.PowerItem = _other.PowerItem;
	}

	// Token: 0x060061E2 RID: 25058 RVA: 0x0026D602 File Offset: 0x0026B802
	public override TileEntity Clone()
	{
		return new TileEntityPowerSource(this);
	}

	// Token: 0x060061E3 RID: 25059 RVA: 0x0026D60C File Offset: 0x0026B80C
	public override bool Activate(bool activated)
	{
		World world = GameManager.Instance.World;
		BlockValue block = this.chunk.GetBlock(base.localChunkPos);
		return block.Block.ActivateBlock(world, base.ToWorldPos(), block, activated, activated);
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x0026D64C File Offset: 0x0026B84C
	public override bool CanHaveParent(IPowered powered)
	{
		return this.PowerItemType == PowerItem.PowerItemTypes.BatteryBank;
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x0026D658 File Offset: 0x0026B858
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

	// Token: 0x060061E6 RID: 25062 RVA: 0x0026D6C0 File Offset: 0x0026B8C0
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

	// Token: 0x060061E7 RID: 25063 RVA: 0x0026D898 File Offset: 0x0026BA98
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

	// Token: 0x060061E8 RID: 25064 RVA: 0x0026D9BC File Offset: 0x0026BBBC
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

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x060061E9 RID: 25065 RVA: 0x0026D9EC File Offset: 0x0026BBEC
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

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x060061EA RID: 25066 RVA: 0x0026DA28 File Offset: 0x0026BC28
	// (set) Token: 0x060061EB RID: 25067 RVA: 0x0026DA52 File Offset: 0x0026BC52
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

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x060061EC RID: 25068 RVA: 0x0026DA84 File Offset: 0x0026BC84
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

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x060061ED RID: 25069 RVA: 0x0026DAAE File Offset: 0x0026BCAE
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

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x060061EE RID: 25070 RVA: 0x0026DAD8 File Offset: 0x0026BCD8
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

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x060061EF RID: 25071 RVA: 0x0026DB02 File Offset: 0x0026BD02
	// (set) Token: 0x060061F0 RID: 25072 RVA: 0x0026DB2C File Offset: 0x0026BD2C
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

	// Token: 0x060061F1 RID: 25073 RVA: 0x0026DB60 File Offset: 0x0026BD60
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

	// Token: 0x060061F2 RID: 25074 RVA: 0x0026DBE4 File Offset: 0x0026BDE4
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

	// Token: 0x060061F3 RID: 25075 RVA: 0x0026DC20 File Offset: 0x0026BE20
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

	// Token: 0x060061F4 RID: 25076 RVA: 0x0026DCF0 File Offset: 0x0026BEF0
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerSource;
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x0026DCF4 File Offset: 0x0026BEF4
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

	// Token: 0x04004CA4 RID: 19620
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004CA5 RID: 19621
	public bool syncNeeded = true;

	// Token: 0x04004CA6 RID: 19622
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004CA7 RID: 19623
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass slotItem;

	// Token: 0x04004CA8 RID: 19624
	public TileEntityPowerSource.ClientPowerData ClientData = new TileEntityPowerSource.ClientPowerData();

	// Token: 0x02000C94 RID: 3220
	public class ClientPowerData
	{
		// Token: 0x060061F6 RID: 25078 RVA: 0x0026DD6C File Offset: 0x0026BF6C
		public ClientPowerData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004CA9 RID: 19625
		public bool IsOn;

		// Token: 0x04004CAA RID: 19626
		public ushort MaxFuel;

		// Token: 0x04004CAB RID: 19627
		public ushort CurrentFuel;

		// Token: 0x04004CAC RID: 19628
		public ushort SolarInput;

		// Token: 0x04004CAD RID: 19629
		public ushort MaxOutput;

		// Token: 0x04004CAE RID: 19630
		public ushort LastOutput;

		// Token: 0x04004CAF RID: 19631
		public ushort AddedFuel;

		// Token: 0x04004CB0 RID: 19632
		public bool SendSlots;

		// Token: 0x04004CB1 RID: 19633
		public ItemStack[] ItemSlots = new ItemStack[6];
	}
}
