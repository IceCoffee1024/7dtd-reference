using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02000C96 RID: 3222
public class TileEntityVendingMachine : TileEntity, ILockable, ITrader
{
	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x06006205 RID: 25093 RVA: 0x0026DF80 File Offset: 0x0026C180
	// (set) Token: 0x06006206 RID: 25094 RVA: 0x0026DF88 File Offset: 0x0026C188
	public TraderData TraderData { get; set; }

	// Token: 0x06006207 RID: 25095 RVA: 0x0026DF94 File Offset: 0x0026C194
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

	// Token: 0x06006208 RID: 25096 RVA: 0x0026E00C File Offset: 0x0026C20C
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

	// Token: 0x06006209 RID: 25097 RVA: 0x0026E098 File Offset: 0x0026C298
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

	// Token: 0x0600620A RID: 25098 RVA: 0x0026E104 File Offset: 0x0026C304
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkAlreadyRentingVM()
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		Vector3i rentedVMPosition = primaryPlayer.RentedVMPosition;
		return !(rentedVMPosition == base.ToWorldPos()) && !(rentedVMPosition == Vector3i.zero) && primaryPlayer.RentalEndDay > GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
	}

	// Token: 0x0600620B RID: 25099 RVA: 0x0026E161 File Offset: 0x0026C361
	public bool IsLocked()
	{
		return this.isLocked;
	}

	// Token: 0x0600620C RID: 25100 RVA: 0x0026E169 File Offset: 0x0026C369
	public void SetLocked(bool _isLocked)
	{
		this.isLocked = _isLocked;
		this.setModified();
	}

	// Token: 0x0600620D RID: 25101 RVA: 0x0026E178 File Offset: 0x0026C378
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x0600620E RID: 25102 RVA: 0x0026E187 File Offset: 0x0026C387
	public bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier)
	{
		return (_userIdentifier != null && _userIdentifier.Equals(this.ownerID)) || this.allowedUserIds.Contains(_userIdentifier);
	}

	// Token: 0x0600620F RID: 25103 RVA: 0x0026E1AB File Offset: 0x0026C3AB
	public bool LocalPlayerIsOwner()
	{
		return this.IsOwner(PlatformManager.InternalLocalUserIdentifier);
	}

	// Token: 0x06006210 RID: 25104 RVA: 0x0026E1B8 File Offset: 0x0026C3B8
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x06006211 RID: 25105 RVA: 0x0026E1CB File Offset: 0x0026C3CB
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x06006212 RID: 25106 RVA: 0x0026E1D3 File Offset: 0x0026C3D3
	public bool HasPassword()
	{
		return !string.IsNullOrEmpty(this.passwordHash);
	}

	// Token: 0x06006213 RID: 25107 RVA: 0x0026E1E3 File Offset: 0x0026C3E3
	public string GetPasswordHash()
	{
		return this.passwordHash;
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x0026E1EB File Offset: 0x0026C3EB
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

	// Token: 0x06006215 RID: 25109 RVA: 0x0026E21F File Offset: 0x0026C41F
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

	// Token: 0x06006216 RID: 25110 RVA: 0x0026E258 File Offset: 0x0026C458
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

	// Token: 0x06006217 RID: 25111 RVA: 0x0026E408 File Offset: 0x0026C608
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

	// Token: 0x06006218 RID: 25112 RVA: 0x0026E4BC File Offset: 0x0026C6BC
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

	// Token: 0x06006219 RID: 25113 RVA: 0x0026E51A File Offset: 0x0026C71A
	public List<PlatformUserIdentifierAbs> GetUsers()
	{
		return this.allowedUserIds;
	}

	// Token: 0x0600621A RID: 25114 RVA: 0x00060491 File Offset: 0x0005E691
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.VendingMachine;
	}

	// Token: 0x0600621B RID: 25115 RVA: 0x0026E522 File Offset: 0x0026C722
	public override void OnLockResponseServer(int _lockingPlayerID, PooledBinaryWriter _bwServerContext, ushort _channel)
	{
		GameManager.Instance.traderManager.TraderInventoryRequested(this.TraderData, _lockingPlayerID);
		this.TraderData.Write(_bwServerContext);
	}

	// Token: 0x0600621C RID: 25116 RVA: 0x0026E548 File Offset: 0x0026C748
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

	// Token: 0x0600621D RID: 25117 RVA: 0x0026E5FC File Offset: 0x0026C7FC
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		this.TraderData.SetModified(this);
	}

	// Token: 0x0600621E RID: 25118 RVA: 0x0026E60A File Offset: 0x0026C80A
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearOwner()
	{
		this.ownerID = null;
	}

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x0600621F RID: 25119 RVA: 0x0026E613 File Offset: 0x0026C813
	public bool IsRentable
	{
		get
		{
			return this.TraderData.TraderInfo.Rentable;
		}
	}

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x06006220 RID: 25120 RVA: 0x0026E625 File Offset: 0x0026C825
	public float RentTimeRemaining
	{
		get
		{
			return (float)(this.rentalEndDay - GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime));
		}
	}

	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x06006221 RID: 25121 RVA: 0x0026E643 File Offset: 0x0026C843
	public int RentalEndDay
	{
		get
		{
			return this.rentalEndDay;
		}
	}

	// Token: 0x06006222 RID: 25122 RVA: 0x0026E64C File Offset: 0x0026C84C
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

	// Token: 0x06006223 RID: 25123 RVA: 0x0026E768 File Offset: 0x0026C968
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

	// Token: 0x06006224 RID: 25124 RVA: 0x0026E7BC File Offset: 0x0026C9BC
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

	// Token: 0x06006225 RID: 25125 RVA: 0x0026EA7C File Offset: 0x0026CC7C
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

	// Token: 0x06006226 RID: 25126 RVA: 0x0026EABC File Offset: 0x0026CCBC
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (!this.TraderData.TraderInfo.PlayerOwned && this.TraderData.TraderInfo.Rentable && this.ownerID != null && this.rentalEndDay <= GameUtils.WorldTimeToDays(world.worldTime))
		{
			this.ClearVendingMachine();
		}
	}

	// Token: 0x04004CB7 RID: 19639
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ver = 3;

	// Token: 0x04004CB9 RID: 19641
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04004CBA RID: 19642
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004CBB RID: 19643
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlatformUserIdentifierAbs> allowedUserIds;

	// Token: 0x04004CBC RID: 19644
	[PublicizedFrom(EAccessModifier.Private)]
	public string passwordHash;

	// Token: 0x04004CBD RID: 19645
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong rentalEndTime;

	// Token: 0x04004CBE RID: 19646
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAutoBuy;

	// Token: 0x04004CBF RID: 19647
	[PublicizedFrom(EAccessModifier.Private)]
	public int rentalEndDay;

	// Token: 0x04004CC0 RID: 19648
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThresholdStep = 0.33333334f;

	// Token: 0x04004CC1 RID: 19649
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThreshold = 0.33333334f;

	// Token: 0x04004CC2 RID: 19650
	[PublicizedFrom(EAccessModifier.Private)]
	public int minimumAutoBuyCount = 5;

	// Token: 0x02000C97 RID: 3223
	public enum RentResult
	{
		// Token: 0x04004CC4 RID: 19652
		Allowed,
		// Token: 0x04004CC5 RID: 19653
		AlreadyRented,
		// Token: 0x04004CC6 RID: 19654
		AlreadyRentingVM,
		// Token: 0x04004CC7 RID: 19655
		NotEnoughMoney
	}
}
