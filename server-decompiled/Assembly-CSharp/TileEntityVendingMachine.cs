using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000C8B RID: 3211
public class TileEntityVendingMachine : TileEntity, ILockable, ITrader
{
	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x060061E0 RID: 25056 RVA: 0x0026CA58 File Offset: 0x0026AC58
	// (set) Token: 0x060061E1 RID: 25057 RVA: 0x0026CA60 File Offset: 0x0026AC60
	public TraderData TraderData { get; set; }

	// Token: 0x060061E2 RID: 25058 RVA: 0x0026CA6C File Offset: 0x0026AC6C
	public TileEntityVendingMachine(Chunk _chunk)
		: base(_chunk)
	{
		this.allowedUserIds = new List<PlatformUserIdentifierAbs>();
		this.isLocked = true;
		this.ownerID = null;
		this.passwordHash = "";
		this.rentalEndTime = 0UL;
		this.rentalEndDay = 0;
		this.nextAutoBuy = 0UL;
		this.TraderData = new TraderData();
	}

	// Token: 0x060061E3 RID: 25059 RVA: 0x0026CAE4 File Offset: 0x0026ACE4
	public override TileEntity Clone()
	{
		return new TileEntityVendingMachine(null)
		{
			bUserAccessing = this.bUserAccessing,
			TraderData = this.TraderData.Clone(),
			allowedUserIds = new List<PlatformUserIdentifierAbs>(this.allowedUserIds),
			isLocked = this.isLocked,
			ownerID = this.ownerID,
			passwordHash = this.passwordHash,
			rentalEndTime = this.rentalEndTime,
			rentalEndDay = this.rentalEndDay,
			nextAutoBuy = this.nextAutoBuy
		};
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x0026CB70 File Offset: 0x0026AD70
	public TileEntityVendingMachine.RentResult CanRent()
	{
		if (this.ownerID != null && !this.ownerID.Equals(PlatformManager.InternalLocalUserIdentifier))
		{
			return TileEntityVendingMachine.RentResult.AlreadyRented;
		}
		if (GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui.PlayerInventory.CurrencyAmount < this.TraderData.TraderInfo.RentCost)
		{
			return TileEntityVendingMachine.RentResult.NotEnoughMoney;
		}
		if (this.checkAlreadyRentingVM())
		{
			return TileEntityVendingMachine.RentResult.AlreadyRentingVM;
		}
		return TileEntityVendingMachine.RentResult.Allowed;
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x0026CBDC File Offset: 0x0026ADDC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkAlreadyRentingVM()
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		Vector3i rentedVMPosition = primaryPlayer.RentedVMPosition;
		return !(rentedVMPosition == base.ToWorldPos()) && !(rentedVMPosition == Vector3i.zero) && primaryPlayer.RentalEndDay > GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
	}

	// Token: 0x060061E6 RID: 25062 RVA: 0x0026CC39 File Offset: 0x0026AE39
	public bool IsLocked()
	{
		return this.isLocked;
	}

	// Token: 0x060061E7 RID: 25063 RVA: 0x0026CC41 File Offset: 0x0026AE41
	public void SetLocked(bool _isLocked)
	{
		this.isLocked = _isLocked;
		this.setModified();
	}

	// Token: 0x060061E8 RID: 25064 RVA: 0x0026CC50 File Offset: 0x0026AE50
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x060061E9 RID: 25065 RVA: 0x0026CC5F File Offset: 0x0026AE5F
	public bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier)
	{
		return (_userIdentifier != null && _userIdentifier.Equals(this.ownerID)) || this.allowedUserIds.Contains(_userIdentifier);
	}

	// Token: 0x060061EA RID: 25066 RVA: 0x0026CC83 File Offset: 0x0026AE83
	public bool LocalPlayerIsOwner()
	{
		return this.IsOwner(PlatformManager.InternalLocalUserIdentifier);
	}

	// Token: 0x060061EB RID: 25067 RVA: 0x0026CC90 File Offset: 0x0026AE90
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061EC RID: 25068 RVA: 0x0026CCA3 File Offset: 0x0026AEA3
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x0026CCAB File Offset: 0x0026AEAB
	public bool HasPassword()
	{
		return !string.IsNullOrEmpty(this.passwordHash);
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x0026CCBB File Offset: 0x0026AEBB
	public string GetPasswordHash()
	{
		return this.passwordHash;
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x0026CCC3 File Offset: 0x0026AEC3
	public bool SetPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.IsOwner(_userIdentifier) && _passwordHash != this.passwordHash)
		{
			this.passwordHash = _passwordHash;
			this.allowedUserIds.Clear();
			base.SetModified();
			return true;
		}
		return false;
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x0026CCF7 File Offset: 0x0026AEF7
	public bool CheckPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.IsOwner(_userIdentifier) || !this.HasPassword())
		{
			return true;
		}
		if (_passwordHash == this.passwordHash)
		{
			this.allowedUserIds.Add(_userIdentifier);
			base.SetModified();
			return true;
		}
		return false;
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x0026CD30 File Offset: 0x0026AF30
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num = _br.ReadInt32();
		if (this.TraderData == null)
		{
			this.TraderData = new TraderData();
		}
		if (this.allowedUserIds == null)
		{
			this.allowedUserIds = new List<PlatformUserIdentifierAbs>();
		}
		this.allowedUserIds.Clear();
		if (num < 3)
		{
			this.TraderData.Read(_br);
			int num2 = _br.ReadInt32();
			this.isLocked = _br.ReadBoolean();
			this.ownerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			this.passwordHash = _br.ReadString();
			int num3 = _br.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				this.allowedUserIds.Add(PlatformUserIdentifierAbs.FromStream(_br, false, false));
			}
			if (num2 > 1)
			{
				this.rentalEndDay = _br.ReadInt32();
				this.rentalEndTime = 0UL;
			}
			else
			{
				this.rentalEndTime = _br.ReadUInt64();
				this.rentalEndDay = GameUtils.WorldTimeToDays(this.rentalEndTime);
			}
			this.TraderData.Read(_br);
			this.nextAutoBuy = (this.TraderData.TraderInfo.Rentable ? _br.ReadUInt64() : 0UL);
			return;
		}
		this.isLocked = _br.ReadBoolean();
		this.ownerID = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		this.passwordHash = _br.ReadString();
		int num4 = _br.ReadInt32();
		for (int j = 0; j < num4; j++)
		{
			this.allowedUserIds.Add(PlatformUserIdentifierAbs.FromStream(_br, false, false));
		}
		this.rentalEndDay = _br.ReadInt32();
		this.rentalEndTime = 0UL;
		this.TraderData.Read(_br);
		this.nextAutoBuy = (this.TraderData.TraderInfo.Rentable ? _br.ReadUInt64() : 0UL);
	}

	// Token: 0x060061F2 RID: 25074 RVA: 0x0026CEE0 File Offset: 0x0026B0E0
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		_bw.Write(3);
		_bw.Write(this.isLocked);
		this.ownerID.ToStream(_bw, false);
		_bw.Write(this.passwordHash);
		_bw.Write(this.allowedUserIds.Count);
		for (int i = 0; i < this.allowedUserIds.Count; i++)
		{
			this.allowedUserIds[i].ToStream(_bw, false);
		}
		_bw.Write(this.rentalEndDay);
		this.TraderData.Write(_bw);
		if (this.TraderData.TraderInfo.Rentable)
		{
			_bw.Write(this.nextAutoBuy);
		}
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x0026CF94 File Offset: 0x0026B194
	public override void UpgradeDowngradeFrom(TileEntity _other)
	{
		base.UpgradeDowngradeFrom(_other);
		if (_other is ILockable)
		{
			ILockable lockable = _other as ILockable;
			this.SetLocked(lockable.IsLocked());
			this.SetOwner(lockable.GetOwner());
			this.allowedUserIds = new List<PlatformUserIdentifierAbs>(lockable.GetUsers());
			this.passwordHash = lockable.GetPasswordHash();
			this.setModified();
		}
	}

	// Token: 0x060061F4 RID: 25076 RVA: 0x0026CFF2 File Offset: 0x0026B1F2
	public List<PlatformUserIdentifierAbs> GetUsers()
	{
		return this.allowedUserIds;
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.VendingMachine;
	}

	// Token: 0x060061F6 RID: 25078 RVA: 0x0026CFFA File Offset: 0x0026B1FA
	public override void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
		GameManager.Instance.traderManager.TraderInventoryRequested(this.TraderData, _lockingPlayerID);
		this.TraderData.Write(_bwServerContext);
	}

	// Token: 0x060061F7 RID: 25079 RVA: 0x0026D020 File Offset: 0x0026B220
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractPerson", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			uiforPrimaryPlayer.entityPlayer.OverrideFOV = -1f;
			uiforPrimaryPlayer.xui.Dialog.KeepZoomOnClose = false;
			return false;
		}
		if (this.TraderData == null)
		{
			this.TraderData = new TraderData();
		}
		this.TraderData.Read(_brServerContext);
		uiforPrimaryPlayer.xui.Trader.Trader = this;
		uiforPrimaryPlayer.windowManager.CloseAllOpenModalWindows(null, false);
		uiforPrimaryPlayer.windowManager.Open("trader", true);
		return true;
	}

	// Token: 0x060061F8 RID: 25080 RVA: 0x0026D0D4 File Offset: 0x0026B2D4
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		this.TraderData.SetModified(this);
	}

	// Token: 0x060061F9 RID: 25081 RVA: 0x0026D0E2 File Offset: 0x0026B2E2
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearOwner()
	{
		this.ownerID = null;
	}

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x060061FA RID: 25082 RVA: 0x0026D0EB File Offset: 0x0026B2EB
	public bool IsRentable
	{
		get
		{
			return this.TraderData.TraderInfo.Rentable;
		}
	}

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x060061FB RID: 25083 RVA: 0x0026D0FD File Offset: 0x0026B2FD
	public float RentTimeRemaining
	{
		get
		{
			return (float)(this.rentalEndDay - GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime));
		}
	}

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x060061FC RID: 25084 RVA: 0x0026D11B File Offset: 0x0026B31B
	public int RentalEndDay
	{
		get
		{
			return this.rentalEndDay;
		}
	}

	// Token: 0x060061FD RID: 25085 RVA: 0x0026D124 File Offset: 0x0026B324
	public bool Rent()
	{
		if ((this.ownerID != null && !this.ownerID.Equals(PlatformManager.InternalLocalUserIdentifier)) || !this.TraderData.TraderInfo.Rentable)
		{
			return false;
		}
		XUi xui = GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui;
		if (xui.PlayerInventory.CurrencyAmount >= this.TraderData.TraderInfo.RentCost)
		{
			ItemStack itemStack = new ItemStack(ItemClass.GetItem(TraderInfo.CurrencyItem, false), this.TraderData.TraderInfo.RentCost);
			xui.PlayerInventory.RemoveItem(itemStack);
			if (this.ownerID == null)
			{
				this.ownerID = PlatformManager.InternalLocalUserIdentifier;
				this.rentalEndDay = GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime) + 30;
				this.SetAutoBuyTime(true);
			}
			else
			{
				this.rentalEndDay += 30;
			}
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			primaryPlayer.RentedVMPosition = base.ToWorldPos();
			primaryPlayer.RentalEndDay = this.rentalEndDay;
			this.setModified();
			return true;
		}
		return false;
	}

	// Token: 0x060061FE RID: 25086 RVA: 0x0026D240 File Offset: 0x0026B440
	public void ClearVendingMachine()
	{
		this.TraderData.AvailableMoney = 0;
		this.TraderData.PrimaryInventory.Clear();
		this.ownerID = null;
		this.allowedUserIds.Clear();
		this.rentalEndTime = 0UL;
		this.passwordHash = "";
		this.setModified();
	}

	// Token: 0x060061FF RID: 25087 RVA: 0x0026D294 File Offset: 0x0026B494
	public bool TryAutoBuy(bool isInitial = true)
	{
		if (this.nextAutoBuy == 0UL)
		{
			this.SetAutoBuyTime(true);
		}
		if (GameManager.Instance.World.worldTime > this.nextAutoBuy)
		{
			GameRandom random = GameManager.Instance.lootManager.Random;
			if (random.RandomFloat < this.autoBuyThreshold && this.TraderData.PrimaryInventory.Count > this.minimumAutoBuyCount)
			{
				int num = random.RandomRange(1, Mathf.Max(this.TraderData.PrimaryInventory.Count / 10, 1));
				Log.Warning("Items Purchased: " + num.ToString());
				for (int i = 0; i < num; i++)
				{
					int num2 = 0;
					for (int j = 0; j < this.TraderData.PrimaryInventory.Count; j++)
					{
						if (this.TraderData.PrimaryInventory[j].Markup <= 0)
						{
							ItemStack item = this.TraderData.PrimaryInventory[j].Item;
							if ((item.itemValue.ItemClass.IsBlock() ? Block.list[item.itemValue.type].EconomicValue : item.itemValue.ItemClass.EconomicValue) > 0f && item.itemValue.ItemClass.SellableToTrader)
							{
								num2++;
							}
						}
					}
					if (num2 > 0)
					{
						int num3 = random.RandomRange(num2);
						num2 = 0;
						for (int k = 0; k < this.TraderData.PrimaryInventory.Count; k++)
						{
							if (this.TraderData.PrimaryInventory[k].Markup <= 0)
							{
								ItemStack item2 = this.TraderData.PrimaryInventory[k].Item;
								if ((item2.itemValue.ItemClass.IsBlock() ? Block.list[item2.itemValue.type].EconomicValue : item2.itemValue.ItemClass.EconomicValue) > 0f && item2.itemValue.ItemClass.SellableToTrader)
								{
									if (num2 == num3)
									{
										int count = item2.count;
										int buyPrice = XUiM_Trader.GetBuyPrice(LocalPlayerUI.GetUIForPrimaryPlayer().xui, item2.itemValue, count, null, k);
										this.TraderData.PrimaryInventory.RemoveAt(k);
										this.TraderData.AvailableMoney += buyPrice;
										break;
									}
									num2++;
								}
							}
						}
					}
				}
				this.autoBuyThreshold = this.autoBuyThresholdStep;
			}
			else
			{
				this.autoBuyThreshold += this.autoBuyThresholdStep;
			}
			this.SetAutoBuyTime(false);
			return this.TryAutoBuy(false);
		}
		return !isInitial;
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x0026D554 File Offset: 0x0026B754
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAutoBuyTime(bool isInitial)
	{
		uint num = 24000U;
		if (isInitial)
		{
			this.nextAutoBuy = GameManager.Instance.World.worldTime + (ulong)num;
			return;
		}
		this.nextAutoBuy += (ulong)num;
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x0026D594 File Offset: 0x0026B794
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (!this.TraderData.TraderInfo.PlayerOwned && this.TraderData.TraderInfo.Rentable && this.ownerID != null && this.rentalEndDay <= GameUtils.WorldTimeToDays(world.worldTime))
		{
			this.ClearVendingMachine();
		}
	}

	// Token: 0x04004C92 RID: 19602
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ver = 3;

	// Token: 0x04004C94 RID: 19604
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04004C95 RID: 19605
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C96 RID: 19606
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlatformUserIdentifierAbs> allowedUserIds;

	// Token: 0x04004C97 RID: 19607
	[PublicizedFrom(EAccessModifier.Private)]
	public string passwordHash;

	// Token: 0x04004C98 RID: 19608
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong rentalEndTime;

	// Token: 0x04004C99 RID: 19609
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAutoBuy;

	// Token: 0x04004C9A RID: 19610
	[PublicizedFrom(EAccessModifier.Private)]
	public int rentalEndDay;

	// Token: 0x04004C9B RID: 19611
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThresholdStep = 0.33333334f;

	// Token: 0x04004C9C RID: 19612
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThreshold = 0.33333334f;

	// Token: 0x04004C9D RID: 19613
	[PublicizedFrom(EAccessModifier.Private)]
	public int minimumAutoBuyCount = 5;

	// Token: 0x02000C8C RID: 3212
	public enum RentResult
	{
		// Token: 0x04004C9F RID: 19615
		Allowed,
		// Token: 0x04004CA0 RID: 19616
		AlreadyRented,
		// Token: 0x04004CA1 RID: 19617
		AlreadyRentingVM,
		// Token: 0x04004CA2 RID: 19618
		NotEnoughMoney
	}
}
