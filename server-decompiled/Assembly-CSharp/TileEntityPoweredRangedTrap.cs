using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x02000C63 RID: 3171
public class TileEntityPoweredRangedTrap : TileEntityPoweredBlock
{
	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x060060F5 RID: 24821 RVA: 0x00264B10 File Offset: 0x00262D10
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

	// Token: 0x060060F6 RID: 24822 RVA: 0x00264B88 File Offset: 0x00262D88
	public TileEntityPoweredRangedTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x060060F7 RID: 24823 RVA: 0x00264BAA File Offset: 0x00262DAA
	// (set) Token: 0x060060F8 RID: 24824 RVA: 0x00264BC1 File Offset: 0x00262DC1
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

	// Token: 0x060060F9 RID: 24825 RVA: 0x00264BCA File Offset: 0x00262DCA
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060060FA RID: 24826 RVA: 0x00264BDD File Offset: 0x00262DDD
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060060FB RID: 24827 RVA: 0x00264BE5 File Offset: 0x00262DE5
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x060060FC RID: 24828 RVA: 0x00264BFA File Offset: 0x00262DFA
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x060060FD RID: 24829 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x060060FE RID: 24830 RVA: 0x00264C08 File Offset: 0x00262E08
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

	// Token: 0x060060FF RID: 24831 RVA: 0x00264C58 File Offset: 0x00262E58
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
		this.ownerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.SetOwnerEntityID();
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeRead.FromClient)
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

	// Token: 0x06006100 RID: 24832 RVA: 0x00264DA4 File Offset: 0x00262FA4
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		this.ownerID.ToStream(_bw, false);
		if (_eStreamMode != TileEntity.StreamModeWrite.Persistency)
		{
			if (_eStreamMode == TileEntity.StreamModeWrite.ToServer)
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

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x06006101 RID: 24833 RVA: 0x00264E78 File Offset: 0x00263078
	// (set) Token: 0x06006102 RID: 24834 RVA: 0x00264EA2 File Offset: 0x002630A2
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

	// Token: 0x170009E0 RID: 2528
	// (get) Token: 0x06006103 RID: 24835 RVA: 0x00264ED4 File Offset: 0x002630D4
	// (set) Token: 0x06006104 RID: 24836 RVA: 0x00264EFE File Offset: 0x002630FE
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

	// Token: 0x170009E1 RID: 2529
	// (get) Token: 0x06006105 RID: 24837 RVA: 0x00264F30 File Offset: 0x00263130
	// (set) Token: 0x06006106 RID: 24838 RVA: 0x00264F5A File Offset: 0x0026315A
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

	// Token: 0x170009E2 RID: 2530
	// (get) Token: 0x06006107 RID: 24839 RVA: 0x00264F86 File Offset: 0x00263186
	public bool TargetSelf
	{
		get
		{
			return (this.TargetType & 1) == 1;
		}
	}

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x06006108 RID: 24840 RVA: 0x00264F93 File Offset: 0x00263193
	public bool TargetAllies
	{
		get
		{
			return (this.TargetType & 2) == 2;
		}
	}

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x06006109 RID: 24841 RVA: 0x00264FA0 File Offset: 0x002631A0
	public bool TargetStrangers
	{
		get
		{
			return (this.TargetType & 4) == 4;
		}
	}

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x0600610A RID: 24842 RVA: 0x00264FAD File Offset: 0x002631AD
	public bool TargetZombies
	{
		get
		{
			return (this.TargetType & 8) == 8;
		}
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x00264FBC File Offset: 0x002631BC
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

	// Token: 0x0600610C RID: 24844 RVA: 0x002650D8 File Offset: 0x002632D8
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

	// Token: 0x0600610D RID: 24845 RVA: 0x00265191 File Offset: 0x00263391
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerRangeTrap;
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x00265198 File Offset: 0x00263398
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool DecrementAmmo(out ItemClass _ammoItem)
	{
		for (int i = 0; i < this.ItemSlots.Length; i++)
		{
			if (this.ItemSlots[i].count > 0)
			{
				_ammoItem = this.ItemSlots[i].itemValue.ItemClass;
				this.ItemSlots[i].count--;
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

	// Token: 0x0600610F RID: 24847 RVA: 0x0026521C File Offset: 0x0026341C
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

	// Token: 0x06006110 RID: 24848 RVA: 0x00265258 File Offset: 0x00263458
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Workstation))
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

	// Token: 0x06006111 RID: 24849 RVA: 0x002652BC File Offset: 0x002634BC
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

	// Token: 0x06006112 RID: 24850 RVA: 0x0026534C File Offset: 0x0026354C
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

	// Token: 0x06006113 RID: 24851 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06006114 RID: 24852 RVA: 0x002653B8 File Offset: 0x002635B8
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		((XUiC_PowerRangedTrapWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("powerrangedtrap")).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open("powerrangedtrap", true);
	}

	// Token: 0x04004B97 RID: 19351
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004B98 RID: 19352
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] ammoItems;

	// Token: 0x04004B99 RID: 19353
	public readonly TileEntityPoweredRangedTrap.ClientAmmoData ClientData = new TileEntityPoweredRangedTrap.ClientAmmoData();

	// Token: 0x04004B9A RID: 19354
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004B9B RID: 19355
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;

	// Token: 0x04004B9C RID: 19356
	public bool ShowTargeting = true;

	// Token: 0x02000C64 RID: 3172
	public class ClientAmmoData
	{
		// Token: 0x06006115 RID: 24853 RVA: 0x00265430 File Offset: 0x00263630
		public ClientAmmoData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004B9D RID: 19357
		public bool IsLocked;

		// Token: 0x04004B9E RID: 19358
		public bool SendSlots;

		// Token: 0x04004B9F RID: 19359
		public ItemStack[] ItemSlots = new ItemStack[3];

		// Token: 0x04004BA0 RID: 19360
		public int TargetType = 12;
	}
}
