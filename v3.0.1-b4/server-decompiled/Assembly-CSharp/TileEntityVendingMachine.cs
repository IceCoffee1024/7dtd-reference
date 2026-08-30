using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C6A RID: 3178
public class TileEntityVendingMachine : TileEntity, ILockable, ITrader
{
	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x0600615C RID: 24924 RVA: 0x002666CC File Offset: 0x002648CC
	// (set) Token: 0x0600615D RID: 24925 RVA: 0x002666D4 File Offset: 0x002648D4
	public TraderData TraderData { get; set; }

	// Token: 0x0600615E RID: 24926 RVA: 0x002666E0 File Offset: 0x002648E0
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

	// Token: 0x0600615F RID: 24927 RVA: 0x00266758 File Offset: 0x00264958
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

	// Token: 0x06006160 RID: 24928 RVA: 0x002667E4 File Offset: 0x002649E4
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

	// Token: 0x06006161 RID: 24929 RVA: 0x00266850 File Offset: 0x00264A50
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkAlreadyRentingVM()
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		Vector3i rentedVMPosition = primaryPlayer.RentedVMPosition;
		return !(rentedVMPosition == base.ToWorldPos()) && !(rentedVMPosition == Vector3i.zero) && primaryPlayer.RentalEndDay > GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
	}

	// Token: 0x06006162 RID: 24930 RVA: 0x002668AD File Offset: 0x00264AAD
	public bool IsLocked()
	{
		return this.isLocked;
	}

	// Token: 0x06006163 RID: 24931 RVA: 0x002668B5 File Offset: 0x00264AB5
	public void SetLocked(bool _isLocked)
	{
		this.isLocked = _isLocked;
		this.setModified();
	}

	// Token: 0x06006164 RID: 24932 RVA: 0x002668C4 File Offset: 0x00264AC4
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x06006165 RID: 24933 RVA: 0x002668D3 File Offset: 0x00264AD3
	public bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier)
	{
		return (_userIdentifier != null && _userIdentifier.Equals(this.ownerID)) || this.allowedUserIds.Contains(_userIdentifier);
	}

	// Token: 0x06006166 RID: 24934 RVA: 0x002668F7 File Offset: 0x00264AF7
	public bool LocalPlayerIsOwner()
	{
		return this.IsOwner(PlatformManager.InternalLocalUserIdentifier);
	}

	// Token: 0x06006167 RID: 24935 RVA: 0x00266904 File Offset: 0x00264B04
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x06006168 RID: 24936 RVA: 0x00266917 File Offset: 0x00264B17
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x06006169 RID: 24937 RVA: 0x0026691F File Offset: 0x00264B1F
	public bool HasPassword()
	{
		return !string.IsNullOrEmpty(this.passwordHash);
	}

	// Token: 0x0600616A RID: 24938 RVA: 0x0026692F File Offset: 0x00264B2F
	public string GetPasswordHash()
	{
		return this.passwordHash;
	}

	// Token: 0x0600616B RID: 24939 RVA: 0x00266937 File Offset: 0x00264B37
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

	// Token: 0x0600616C RID: 24940 RVA: 0x0026696B File Offset: 0x00264B6B
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

	// Token: 0x0600616D RID: 24941 RVA: 0x002669A4 File Offset: 0x00264BA4
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
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

	// Token: 0x0600616E RID: 24942 RVA: 0x00266B54 File Offset: 0x00264D54
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
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

	// Token: 0x0600616F RID: 24943 RVA: 0x00266C08 File Offset: 0x00264E08
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

	// Token: 0x06006170 RID: 24944 RVA: 0x00266C66 File Offset: 0x00264E66
	public List<PlatformUserIdentifierAbs> GetUsers()
	{
		return this.allowedUserIds;
	}

	// Token: 0x06006171 RID: 24945 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.VendingMachine;
	}

	// Token: 0x06006172 RID: 24946 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06006173 RID: 24947 RVA: 0x00266C70 File Offset: 0x00264E70
	public override void OnLockedServer(bool _success, int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		if (!_success)
		{
			return;
		}
		TileEntityVendingMachine.VendingMachineLockContext vendingMachineLockContext = _context as TileEntityVendingMachine.VendingMachineLockContext;
		if (vendingMachineLockContext != null)
		{
			GameManager.Instance.traderManager.TraderInventoryRequested(this.TraderData, _lockingPlayerID);
			vendingMachineLockContext.TraderData = this.TraderData.Clone();
		}
	}

	// Token: 0x06006174 RID: 24948 RVA: 0x00266CB4 File Offset: 0x00264EB4
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractPerson", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			uiforPrimaryPlayer.entityPlayer.OverrideFOV = -1f;
			uiforPrimaryPlayer.xui.Dialog.KeepZoomOnClose = false;
			return;
		}
		TileEntityVendingMachine.VendingMachineLockContext vendingMachineLockContext = _context as TileEntityVendingMachine.VendingMachineLockContext;
		if (vendingMachineLockContext != null && vendingMachineLockContext.TraderData != null)
		{
			if (this.TraderData == null)
			{
				this.TraderData = new TraderData();
			}
			this.TraderData.CopyFrom(vendingMachineLockContext.TraderData);
		}
		uiforPrimaryPlayer.xui.Trader.Trader = this;
		uiforPrimaryPlayer.windowManager.CloseAllOpenModalWindows(null, false);
		uiforPrimaryPlayer.windowManager.Open("trader", true);
	}

	// Token: 0x06006175 RID: 24949 RVA: 0x00266D7D File Offset: 0x00264F7D
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		this.TraderData.SetModified(this);
	}

	// Token: 0x06006176 RID: 24950 RVA: 0x00266D8B File Offset: 0x00264F8B
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearOwner()
	{
		this.ownerID = null;
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06006177 RID: 24951 RVA: 0x00266D94 File Offset: 0x00264F94
	public bool IsRentable
	{
		get
		{
			return this.TraderData.TraderInfo.Rentable;
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06006178 RID: 24952 RVA: 0x00266DA6 File Offset: 0x00264FA6
	public float RentTimeRemaining
	{
		get
		{
			return (float)(this.rentalEndDay - GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime));
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x06006179 RID: 24953 RVA: 0x00266DC4 File Offset: 0x00264FC4
	public int RentalEndDay
	{
		get
		{
			return this.rentalEndDay;
		}
	}

	// Token: 0x0600617A RID: 24954 RVA: 0x00266DCC File Offset: 0x00264FCC
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

	// Token: 0x0600617B RID: 24955 RVA: 0x00266EE8 File Offset: 0x002650E8
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

	// Token: 0x0600617C RID: 24956 RVA: 0x00266F3C File Offset: 0x0026513C
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

	// Token: 0x0600617D RID: 24957 RVA: 0x002671FC File Offset: 0x002653FC
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

	// Token: 0x0600617E RID: 24958 RVA: 0x0026723C File Offset: 0x0026543C
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (!this.TraderData.TraderInfo.PlayerOwned && this.TraderData.TraderInfo.Rentable && this.ownerID != null && this.rentalEndDay <= GameUtils.WorldTimeToDays(world.worldTime))
		{
			this.ClearVendingMachine();
		}
	}

	// Token: 0x04004BBE RID: 19390
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ver = 3;

	// Token: 0x04004BC0 RID: 19392
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04004BC1 RID: 19393
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004BC2 RID: 19394
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlatformUserIdentifierAbs> allowedUserIds;

	// Token: 0x04004BC3 RID: 19395
	[PublicizedFrom(EAccessModifier.Private)]
	public string passwordHash;

	// Token: 0x04004BC4 RID: 19396
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong rentalEndTime;

	// Token: 0x04004BC5 RID: 19397
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAutoBuy;

	// Token: 0x04004BC6 RID: 19398
	[PublicizedFrom(EAccessModifier.Private)]
	public int rentalEndDay;

	// Token: 0x04004BC7 RID: 19399
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThresholdStep = 0.33333334f;

	// Token: 0x04004BC8 RID: 19400
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThreshold = 0.33333334f;

	// Token: 0x04004BC9 RID: 19401
	[PublicizedFrom(EAccessModifier.Private)]
	public int minimumAutoBuyCount = 5;

	// Token: 0x02000C6B RID: 3179
	public enum RentResult
	{
		// Token: 0x04004BCB RID: 19403
		Allowed,
		// Token: 0x04004BCC RID: 19404
		AlreadyRented,
		// Token: 0x04004BCD RID: 19405
		AlreadyRentingVM,
		// Token: 0x04004BCE RID: 19406
		NotEnoughMoney
	}

	// Token: 0x02000C6C RID: 3180
	[Preserve]
	public class VendingMachineLockContext : ILockContext
	{
		// Token: 0x0600617F RID: 24959 RVA: 0x0000640C File Offset: 0x0000460C
		public VendingMachineLockContext()
		{
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00267295 File Offset: 0x00265495
		public VendingMachineLockContext(TraderData _traderData)
		{
			this.TraderData = _traderData.Clone();
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x002672A9 File Offset: 0x002654A9
		public void Read(PooledBinaryReader _br)
		{
			if (this.TraderData == null)
			{
				this.TraderData = new TraderData();
			}
			this.TraderData.Read(_br);
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x002672CA File Offset: 0x002654CA
		public void Write(PooledBinaryWriter _bw)
		{
			this.TraderData.Write(_bw);
		}

		// Token: 0x04004BCF RID: 19407
		public TraderData TraderData;
	}
}
