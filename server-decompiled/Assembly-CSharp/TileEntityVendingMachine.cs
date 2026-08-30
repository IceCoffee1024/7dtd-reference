using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C7C RID: 3196
public class TileEntityVendingMachine : TileEntity, ILockable, ITrader
{
	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x060061E7 RID: 25063 RVA: 0x002695B0 File Offset: 0x002677B0
	// (set) Token: 0x060061E8 RID: 25064 RVA: 0x002695B8 File Offset: 0x002677B8
	public TraderData TraderData { get; set; }

	// Token: 0x060061E9 RID: 25065 RVA: 0x002695C4 File Offset: 0x002677C4
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

	// Token: 0x060061EA RID: 25066 RVA: 0x0026963C File Offset: 0x0026783C
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

	// Token: 0x060061EB RID: 25067 RVA: 0x002696C8 File Offset: 0x002678C8
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

	// Token: 0x060061EC RID: 25068 RVA: 0x00269734 File Offset: 0x00267934
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkAlreadyRentingVM()
	{
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		Vector3i rentedVMPosition = primaryPlayer.RentedVMPosition;
		return !(rentedVMPosition == base.ToWorldPos()) && !(rentedVMPosition == Vector3i.zero) && primaryPlayer.RentalEndDay > GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime);
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x00269791 File Offset: 0x00267991
	public bool IsLocked()
	{
		return this.isLocked;
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x00269799 File Offset: 0x00267999
	public void SetLocked(bool _isLocked)
	{
		this.isLocked = _isLocked;
		this.setModified();
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x002697A8 File Offset: 0x002679A8
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		this.ownerID = _userIdentifier;
		this.setModified();
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x002697B7 File Offset: 0x002679B7
	public bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier)
	{
		return (_userIdentifier != null && _userIdentifier.Equals(this.ownerID)) || this.allowedUserIds.Contains(_userIdentifier);
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x002697DB File Offset: 0x002679DB
	public bool LocalPlayerIsOwner()
	{
		return this.IsOwner(PlatformManager.InternalLocalUserIdentifier);
	}

	// Token: 0x060061F2 RID: 25074 RVA: 0x002697E8 File Offset: 0x002679E8
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(this.ownerID);
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x002697FB File Offset: 0x002679FB
	public PlatformUserIdentifierAbs GetOwner()
	{
		return this.ownerID;
	}

	// Token: 0x060061F4 RID: 25076 RVA: 0x00269803 File Offset: 0x00267A03
	public bool HasPassword()
	{
		return !string.IsNullOrEmpty(this.passwordHash);
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x00269813 File Offset: 0x00267A13
	public string GetPasswordHash()
	{
		return this.passwordHash;
	}

	// Token: 0x060061F6 RID: 25078 RVA: 0x0026981B File Offset: 0x00267A1B
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

	// Token: 0x060061F7 RID: 25079 RVA: 0x0026984F File Offset: 0x00267A4F
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

	// Token: 0x060061F8 RID: 25080 RVA: 0x00269888 File Offset: 0x00267A88
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

	// Token: 0x060061F9 RID: 25081 RVA: 0x00269A38 File Offset: 0x00267C38
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

	// Token: 0x060061FA RID: 25082 RVA: 0x00269AEC File Offset: 0x00267CEC
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

	// Token: 0x060061FB RID: 25083 RVA: 0x00269B4A File Offset: 0x00267D4A
	public List<PlatformUserIdentifierAbs> GetUsers()
	{
		return this.allowedUserIds;
	}

	// Token: 0x060061FC RID: 25084 RVA: 0x000601DD File Offset: 0x0005E3DD
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.VendingMachine;
	}

	// Token: 0x060061FD RID: 25085 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060061FE RID: 25086 RVA: 0x00269B54 File Offset: 0x00267D54
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

	// Token: 0x060061FF RID: 25087 RVA: 0x00269B98 File Offset: 0x00267D98
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

	// Token: 0x06006200 RID: 25088 RVA: 0x00269C61 File Offset: 0x00267E61
	public override void OnUnlockedServer(int _unlockingPlayerID, ushort _channel)
	{
		this.TraderData.SetModified(this);
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x00269C6F File Offset: 0x00267E6F
	[PublicizedFrom(EAccessModifier.Internal)]
	public void ClearOwner()
	{
		this.ownerID = null;
	}

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x06006202 RID: 25090 RVA: 0x00269C78 File Offset: 0x00267E78
	public bool IsRentable
	{
		get
		{
			return this.TraderData.TraderInfo.Rentable;
		}
	}

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06006203 RID: 25091 RVA: 0x00269C8A File Offset: 0x00267E8A
	public float RentTimeRemaining
	{
		get
		{
			return (float)(this.rentalEndDay - GameUtils.WorldTimeToDays(GameManager.Instance.World.worldTime));
		}
	}

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x06006204 RID: 25092 RVA: 0x00269CA8 File Offset: 0x00267EA8
	public int RentalEndDay
	{
		get
		{
			return this.rentalEndDay;
		}
	}

	// Token: 0x06006205 RID: 25093 RVA: 0x00269CB0 File Offset: 0x00267EB0
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

	// Token: 0x06006206 RID: 25094 RVA: 0x00269DCC File Offset: 0x00267FCC
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

	// Token: 0x06006207 RID: 25095 RVA: 0x00269E20 File Offset: 0x00268020
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

	// Token: 0x06006208 RID: 25096 RVA: 0x0026A0E0 File Offset: 0x002682E0
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

	// Token: 0x06006209 RID: 25097 RVA: 0x0026A120 File Offset: 0x00268320
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (!this.TraderData.TraderInfo.PlayerOwned && this.TraderData.TraderInfo.Rentable && this.ownerID != null && this.rentalEndDay <= GameUtils.WorldTimeToDays(world.worldTime))
		{
			this.ClearVendingMachine();
		}
	}

	// Token: 0x04004C5B RID: 19547
	[PublicizedFrom(EAccessModifier.Private)]
	public const int ver = 3;

	// Token: 0x04004C5D RID: 19549
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isLocked;

	// Token: 0x04004C5E RID: 19550
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs ownerID;

	// Token: 0x04004C5F RID: 19551
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlatformUserIdentifierAbs> allowedUserIds;

	// Token: 0x04004C60 RID: 19552
	[PublicizedFrom(EAccessModifier.Private)]
	public string passwordHash;

	// Token: 0x04004C61 RID: 19553
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong rentalEndTime;

	// Token: 0x04004C62 RID: 19554
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextAutoBuy;

	// Token: 0x04004C63 RID: 19555
	[PublicizedFrom(EAccessModifier.Private)]
	public int rentalEndDay;

	// Token: 0x04004C64 RID: 19556
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThresholdStep = 0.33333334f;

	// Token: 0x04004C65 RID: 19557
	[PublicizedFrom(EAccessModifier.Private)]
	public float autoBuyThreshold = 0.33333334f;

	// Token: 0x04004C66 RID: 19558
	[PublicizedFrom(EAccessModifier.Private)]
	public int minimumAutoBuyCount = 5;

	// Token: 0x02000C7D RID: 3197
	public enum RentResult
	{
		// Token: 0x04004C68 RID: 19560
		Allowed,
		// Token: 0x04004C69 RID: 19561
		AlreadyRented,
		// Token: 0x04004C6A RID: 19562
		AlreadyRentingVM,
		// Token: 0x04004C6B RID: 19563
		NotEnoughMoney
	}

	// Token: 0x02000C7E RID: 3198
	[Preserve]
	public class VendingMachineLockContext : ILockContext
	{
		// Token: 0x0600620A RID: 25098 RVA: 0x0000640C File Offset: 0x0000460C
		public VendingMachineLockContext()
		{
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x0026A179 File Offset: 0x00268379
		public VendingMachineLockContext(TraderData _traderData)
		{
			this.TraderData = _traderData.Clone();
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x0026A18D File Offset: 0x0026838D
		public void Read(PooledBinaryReader _br)
		{
			if (this.TraderData == null)
			{
				this.TraderData = new TraderData();
			}
			this.TraderData.Read(_br);
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x0026A1AE File Offset: 0x002683AE
		public void Write(PooledBinaryWriter _bw)
		{
			this.TraderData.Write(_bw);
		}

		// Token: 0x04004C6C RID: 19564
		public TraderData TraderData;
	}
}
