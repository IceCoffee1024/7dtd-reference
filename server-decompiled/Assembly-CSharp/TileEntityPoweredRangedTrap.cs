using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x02000C8F RID: 3215
public class TileEntityPoweredRangedTrap : TileEntityPoweredBlock
{
	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x060061A1 RID: 24993 RVA: 0x0026C3C4 File Offset: 0x0026A5C4
	public ItemClass[] AmmoItems
	{
		get
		{
			if (this.ammoItems == null)
			{
				Block block = this.chunk.GetBlock(base.localChunkPos).Block;
				List<ItemClass> list = new List<ItemClass>();
				BlockLauncher blockLauncher = block as BlockLauncher;
				if (blockLauncher != null)
				{
					XUiC_RequiredItemStack.ParseItemClassesFromString(list, blockLauncher.AmmoItemName);
				}
				else
				{
					BlockRanged blockRanged = block as BlockRanged;
					if (blockRanged != null)
					{
						XUiC_RequiredItemStack.ParseItemClassesFromString(list, blockRanged.AmmoItemName);
					}
				}
				this.ammoItems = list.ToArray();
			}
			return this.ammoItems;
		}
	}

	// Token: 0x060061A2 RID: 24994 RVA: 0x0026C43C File Offset: 0x0026A63C
	public TileEntityPoweredRangedTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x060061A3 RID: 24995 RVA: 0x0026C45E File Offset: 0x0026A65E
	// (set) Token: 0x060061A4 RID: 24996 RVA: 0x0026C475 File Offset: 0x0026A675
	public int OwnerEntityID
	{
		get
		{
			if (this.ownerEntityID == -1)
			{
				this.SetOwnerEntityID();
			}
			return this.ownerEntityID;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.ownerEntityID = value;
		}
	}

	// Token: 0x060061A5 RID: 24997 RVA: 0x0026C47E File Offset: 0x0026A67E
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061A6 RID: 24998 RVA: 0x0026C491 File Offset: 0x0026A691
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061A7 RID: 24999 RVA: 0x0026C499 File Offset: 0x0026A699
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x060061A8 RID: 25000 RVA: 0x0026C4AE File Offset: 0x0026A6AE
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x060061A9 RID: 25001 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x060061AA RID: 25002 RVA: 0x0026C4BC File Offset: 0x0026A6BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetOwnerEntityID()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		PersistentPlayerList persistentPlayerList = GameManager.Instance.GetPersistentPlayerList();
		PersistentPlayerData persistentPlayerData = ((persistentPlayerList != null) ? persistentPlayerList.GetPlayerData(this.ownerID) : null);
		if (persistentPlayerData != null)
		{
			this.ownerEntityID = persistentPlayerData.EntityId;
			return;
		}
		this.ownerEntityID = -1;
	}

	// Token: 0x060061AB RID: 25003 RVA: 0x0026C50C File Offset: 0x0026A70C
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
		this.ownerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.SetOwnerEntityID();
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			if (_eStreamMode == StreamModeRead.FromClient)
			{
				this.bUserAccessing = _br.ReadBoolean();
				if (this.PowerItem == null)
				{
					this.PowerItem = base.CreatePowerItemForTileEntity((ushort)this.chunk.GetBlock(base.localChunkPos).type);
				}
				(this.PowerItem as PowerRangedTrap).IsLocked = _br.ReadBoolean();
				if (_br.ReadBoolean())
				{
					this.ClientData.ItemSlots = GameUtils.ReadItemStack(_br);
					(this.PowerItem as PowerRangedTrap).SetSlots(this.ClientData.ItemSlots);
				}
				this.TargetType = _br.ReadInt32();
				return;
			}
			bool flag = _br.ReadBoolean();
			bool flag2 = !this.bUserAccessing || (this.bUserAccessing && this.ClientData.IsLocked);
			if (flag)
			{
				this.ClientData.IsLocked = _br.ReadBoolean();
				ItemStack[] array = GameUtils.ReadItemStack(_br);
				if (flag2)
				{
					this.ClientData.ItemSlots = array;
				}
			}
			int num = _br.ReadInt32();
			if (!this.bUserAccessing)
			{
				this.TargetType = num;
			}
		}
	}

	// Token: 0x060061AC RID: 25004 RVA: 0x0026C658 File Offset: 0x0026A858
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		this.ownerID.ToStream(_bw, false);
		if (_eStreamMode != StreamModeWrite.Persistency)
		{
			if (_eStreamMode == StreamModeWrite.ToServer)
			{
				_bw.Write(this.bUserAccessing);
				_bw.Write(this.IsLocked);
				_bw.Write(this.ClientData.SendSlots);
				if (this.ClientData.SendSlots)
				{
					GameUtils.WriteItemStack(_bw, this.ClientData.ItemSlots);
					this.ClientData.SendSlots = false;
				}
				_bw.Write(this.TargetType);
				return;
			}
			PowerRangedTrap powerRangedTrap = this.PowerItem as PowerRangedTrap;
			_bw.Write(powerRangedTrap != null);
			if (powerRangedTrap != null)
			{
				_bw.Write(powerRangedTrap.IsLocked);
				GameUtils.WriteItemStack(_bw, powerRangedTrap.Stacks);
			}
			_bw.Write(this.TargetType);
		}
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x060061AD RID: 25005 RVA: 0x0026C72C File Offset: 0x0026A92C
	// (set) Token: 0x060061AE RID: 25006 RVA: 0x0026C756 File Offset: 0x0026A956
	public bool IsLocked
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerRangedTrap).IsLocked;
			}
			return this.ClientData.IsLocked;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerRangedTrap).IsLocked = value;
				return;
			}
			this.ClientData.IsLocked = value;
			base.SetModified();
		}
	}

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x060061AF RID: 25007 RVA: 0x0026C788 File Offset: 0x0026A988
	// (set) Token: 0x060061B0 RID: 25008 RVA: 0x0026C7B2 File Offset: 0x0026A9B2
	public ItemStack[] ItemSlots
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (this.PowerItem as PowerRangedTrap).Stacks;
			}
			return this.ClientData.ItemSlots;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerRangedTrap).SetSlots(value);
				return;
			}
			this.ClientData.ItemSlots = value;
			base.SetModified();
		}
	}

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x060061B1 RID: 25009 RVA: 0x0026C7E4 File Offset: 0x0026A9E4
	// (set) Token: 0x060061B2 RID: 25010 RVA: 0x0026C80E File Offset: 0x0026AA0E
	public int TargetType
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return (int)(this.PowerItem as PowerRangedTrap).TargetType;
			}
			return this.ClientData.TargetType;
		}
		set
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				(this.PowerItem as PowerRangedTrap).TargetType = (PowerRangedTrap.TargetTypes)value;
				return;
			}
			this.ClientData.TargetType = value;
		}
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x060061B3 RID: 25011 RVA: 0x0026C83A File Offset: 0x0026AA3A
	public bool TargetSelf
	{
		get
		{
			return (this.TargetType & 1) == 1;
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x060061B4 RID: 25012 RVA: 0x0026C847 File Offset: 0x0026AA47
	public bool TargetAllies
	{
		get
		{
			return (this.TargetType & 2) == 2;
		}
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x060061B5 RID: 25013 RVA: 0x0026C854 File Offset: 0x0026AA54
	public bool TargetStrangers
	{
		get
		{
			return (this.TargetType & 4) == 4;
		}
	}

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x060061B6 RID: 25014 RVA: 0x0026C861 File Offset: 0x0026AA61
	public bool TargetZombies
	{
		get
		{
			return (this.TargetType & 8) == 8;
		}
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x0026C870 File Offset: 0x0026AA70
	public bool TryStackItem(ItemStack itemStack)
	{
		if (this.IsLocked)
		{
			return false;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return (this.PowerItem as PowerRangedTrap).TryStackItem(itemStack);
		}
		for (int i = 0; i < this.ClientData.ItemSlots.Length; i++)
		{
			int count = itemStack.count;
			if (this.ClientData.ItemSlots[i].IsEmpty())
			{
				this.ClientData.ItemSlots[i] = itemStack.Clone();
				this.ClientData.SendSlots = true;
				base.SetModified();
				itemStack.count = 0;
				return true;
			}
			if (this.ClientData.ItemSlots[i].itemValue.type == itemStack.itemValue.type && this.ClientData.ItemSlots[i].CanStackPartly(ref count))
			{
				this.ClientData.ItemSlots[i].count += count;
				itemStack.count -= count;
				this.ClientData.SendSlots = true;
				base.SetModified();
				if (itemStack.count == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060061B8 RID: 25016 RVA: 0x0026C98C File Offset: 0x0026AB8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetValuesFromBlock(ushort blockID)
	{
		base.SetValuesFromBlock(blockID);
		if (Block.list[(int)blockID].Properties.Values.ContainsKey("BurstFireRate"))
		{
			this.DelayTime = StringParsers.ParseFloat(Block.list[(int)blockID].Properties.Values["BurstFireRate"], 0, -1, NumberStyles.Any);
		}
		else
		{
			this.DelayTime = 0.5f;
		}
		if (Block.list[(int)blockID].Properties.Values.ContainsKey("ShowTargeting"))
		{
			this.ShowTargeting = StringParsers.ParseBool(Block.list[(int)blockID].Properties.Values["ShowTargeting"], 0, -1, true);
			return;
		}
		this.ShowTargeting = true;
	}

	// Token: 0x060061B9 RID: 25017 RVA: 0x0026CA45 File Offset: 0x0026AC45
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerRangeTrap;
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x0026CA4C File Offset: 0x0026AC4C
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool DecrementAmmo(out ItemClass _ammoItem)
	{
		for (int i = 0; i < this.ItemSlots.Length; i++)
		{
			if (this.ItemSlots[i].count > 0)
			{
				_ammoItem = this.ItemSlots[i].itemValue.ItemClass;
				ItemStack itemStack = this.ItemSlots[i];
				int count = itemStack.count;
				itemStack.count = count - 1;
				if (this.ItemSlots[i].count == 0)
				{
					this.ItemSlots[i] = ItemStack.Empty;
				}
				base.SetModified();
				return true;
			}
		}
		_ammoItem = null;
		return false;
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x0026CAD0 File Offset: 0x0026ACD0
	public ItemClass CurrentAmmoItem()
	{
		foreach (ItemStack itemStack in this.ItemSlots)
		{
			if (itemStack.count > 0)
			{
				return itemStack.itemValue.ItemClass;
			}
		}
		return null;
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x0026CB0C File Offset: 0x0026AD0C
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation, -1))
		{
			return false;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return (this.PowerItem as PowerRangedTrap).AddItem(_itemStack);
		}
		for (int i = 0; i < this.ItemSlots.Length; i++)
		{
			if (this.ItemSlots[i].IsEmpty())
			{
				this.ItemSlots[i] = _itemStack;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060061BD RID: 25021 RVA: 0x0026CB74 File Offset: 0x0026AD74
	public override void ClientUpdate()
	{
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + this.DelayTime;
			World world = GameManager.Instance.World;
			BlockValue block = this.chunk.GetBlock(base.localChunkPos);
			Block block2 = block.Block;
			BlockLauncher blockLauncher = block2 as BlockLauncher;
			if (blockLauncher != null)
			{
				blockLauncher.InstantiateProjectile(world, base.ToWorldPos());
				return;
			}
			if (block2 is BlockRanged)
			{
				block2.ActivateBlock(world, base.ToWorldPos(), block, base.IsPowered, base.IsPowered);
			}
		}
	}

	// Token: 0x060061BE RID: 25022 RVA: 0x0026CC04 File Offset: 0x0026AE04
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		TileEntityPoweredRangedTrap tileEntityPoweredRangedTrap;
		if (_teNew.TryGetSelfOrFeature<TileEntityPoweredRangedTrap>(out tileEntityPoweredRangedTrap))
		{
			return;
		}
		List<ItemStack> list = new List<ItemStack>();
		list.AddRange(this.ItemSlots);
		Vector3 vector = base.ToWorldCenterPos();
		vector.y += 0.9f;
		GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
	}

	// Token: 0x060061BF RID: 25023 RVA: 0x0026CC70 File Offset: 0x0026AE70
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		((XUiC_PowerRangedTrapWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("powerrangedtrap")).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open("powerrangedtrap", true);
		return true;
	}

	// Token: 0x04004C90 RID: 19600
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C91 RID: 19601
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] ammoItems;

	// Token: 0x04004C92 RID: 19602
	public readonly TileEntityPoweredRangedTrap.ClientAmmoData ClientData = new TileEntityPoweredRangedTrap.ClientAmmoData();

	// Token: 0x04004C93 RID: 19603
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C94 RID: 19604
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;

	// Token: 0x04004C95 RID: 19605
	public bool ShowTargeting = true;

	// Token: 0x02000C90 RID: 3216
	public class ClientAmmoData
	{
		// Token: 0x060061C0 RID: 25024 RVA: 0x0026CCE8 File Offset: 0x0026AEE8
		public ClientAmmoData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004C96 RID: 19606
		public bool IsLocked;

		// Token: 0x04004C97 RID: 19607
		public bool SendSlots;

		// Token: 0x04004C98 RID: 19608
		public ItemStack[] ItemSlots = new ItemStack[3];

		// Token: 0x04004C99 RID: 19609
		public int TargetType = 12;
	}
}
