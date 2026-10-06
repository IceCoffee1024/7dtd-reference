using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x02000C84 RID: 3204
public class TileEntityPoweredRangedTrap : TileEntityPoweredBlock
{
	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x0600617C RID: 24956 RVA: 0x0026AE9C File Offset: 0x0026909C
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

	// Token: 0x0600617D RID: 24957 RVA: 0x0026AF14 File Offset: 0x00269114
	public TileEntityPoweredRangedTrap(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x0600617E RID: 24958 RVA: 0x0026AF36 File Offset: 0x00269136
	// (set) Token: 0x0600617F RID: 24959 RVA: 0x0026AF4D File Offset: 0x0026914D
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

	// Token: 0x06006180 RID: 24960 RVA: 0x0026AF56 File Offset: 0x00269156
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x06006181 RID: 24961 RVA: 0x0026AF69 File Offset: 0x00269169
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x06006182 RID: 24962 RVA: 0x0026AF71 File Offset: 0x00269171
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.SetOwnerEntityID();
		this.setModified();
	}

	// Token: 0x06006183 RID: 24963 RVA: 0x0026AF86 File Offset: 0x00269186
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSendSlots()
	{
		this.ClientData.SendSlots = true;
	}

	// Token: 0x06006184 RID: 24964 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OnSetLocalChunkPosition()
	{
	}

	// Token: 0x06006185 RID: 24965 RVA: 0x0026AF94 File Offset: 0x00269194
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

	// Token: 0x06006186 RID: 24966 RVA: 0x0026AFE4 File Offset: 0x002691E4
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

	// Token: 0x06006187 RID: 24967 RVA: 0x0026B130 File Offset: 0x00269330
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

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06006188 RID: 24968 RVA: 0x0026B204 File Offset: 0x00269404
	// (set) Token: 0x06006189 RID: 24969 RVA: 0x0026B22E File Offset: 0x0026942E
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

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x0600618A RID: 24970 RVA: 0x0026B260 File Offset: 0x00269460
	// (set) Token: 0x0600618B RID: 24971 RVA: 0x0026B28A File Offset: 0x0026948A
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

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x0600618C RID: 24972 RVA: 0x0026B2BC File Offset: 0x002694BC
	// (set) Token: 0x0600618D RID: 24973 RVA: 0x0026B2E6 File Offset: 0x002694E6
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

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x0600618E RID: 24974 RVA: 0x0026B312 File Offset: 0x00269512
	public bool TargetSelf
	{
		get
		{
			return (this.TargetType & 1) == 1;
		}
	}

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x0600618F RID: 24975 RVA: 0x0026B31F File Offset: 0x0026951F
	public bool TargetAllies
	{
		get
		{
			return (this.TargetType & 2) == 2;
		}
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x06006190 RID: 24976 RVA: 0x0026B32C File Offset: 0x0026952C
	public bool TargetStrangers
	{
		get
		{
			return (this.TargetType & 4) == 4;
		}
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x06006191 RID: 24977 RVA: 0x0026B339 File Offset: 0x00269539
	public bool TargetZombies
	{
		get
		{
			return (this.TargetType & 8) == 8;
		}
	}

	// Token: 0x06006192 RID: 24978 RVA: 0x0026B348 File Offset: 0x00269548
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

	// Token: 0x06006193 RID: 24979 RVA: 0x0026B464 File Offset: 0x00269664
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

	// Token: 0x06006194 RID: 24980 RVA: 0x0026B51D File Offset: 0x0026971D
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.PowerRangeTrap;
	}

	// Token: 0x06006195 RID: 24981 RVA: 0x0026B524 File Offset: 0x00269724
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

	// Token: 0x06006196 RID: 24982 RVA: 0x0026B5A8 File Offset: 0x002697A8
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

	// Token: 0x06006197 RID: 24983 RVA: 0x0026B5E4 File Offset: 0x002697E4
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

	// Token: 0x06006198 RID: 24984 RVA: 0x0026B64C File Offset: 0x0026984C
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

	// Token: 0x06006199 RID: 24985 RVA: 0x0026B6DC File Offset: 0x002698DC
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

	// Token: 0x0600619A RID: 24986 RVA: 0x0026B748 File Offset: 0x00269948
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

	// Token: 0x04004C6B RID: 19563
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 18;

	// Token: 0x04004C6C RID: 19564
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass[] ammoItems;

	// Token: 0x04004C6D RID: 19565
	public readonly TileEntityPoweredRangedTrap.ClientAmmoData ClientData = new TileEntityPoweredRangedTrap.ClientAmmoData();

	// Token: 0x04004C6E RID: 19566
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C6F RID: 19567
	[PublicizedFrom(EAccessModifier.Private)]
	public int ownerEntityID = -1;

	// Token: 0x04004C70 RID: 19568
	public bool ShowTargeting = true;

	// Token: 0x02000C85 RID: 3205
	public class ClientAmmoData
	{
		// Token: 0x0600619B RID: 24987 RVA: 0x0026B7C0 File Offset: 0x002699C0
		public ClientAmmoData()
		{
			for (int i = 0; i < this.ItemSlots.Length; i++)
			{
				this.ItemSlots[i] = ItemStack.Empty;
			}
		}

		// Token: 0x04004C71 RID: 19569
		public bool IsLocked;

		// Token: 0x04004C72 RID: 19570
		public bool SendSlots;

		// Token: 0x04004C73 RID: 19571
		public ItemStack[] ItemSlots = new ItemStack[3];

		// Token: 0x04004C74 RID: 19572
		public int TargetType = 12;
	}
}
