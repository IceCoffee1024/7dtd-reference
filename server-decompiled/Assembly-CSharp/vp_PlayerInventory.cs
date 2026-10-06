using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001629 RID: 5673
public class vp_PlayerInventory : vp_Inventory
{
	// Token: 0x170014F0 RID: 5360
	// (get) Token: 0x0600B1BA RID: 45498 RVA: 0x004264B0 File Offset: 0x004246B0
	public Dictionary<vp_Weapon, vp_ItemIdentifier> WeaponIdentifiers
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_WeaponIdentifiers == null)
			{
				this.m_WeaponIdentifiers = new Dictionary<vp_Weapon, vp_ItemIdentifier>();
				foreach (vp_Weapon vp_Weapon in this.WeaponHandler.Weapons)
				{
					vp_ItemIdentifier component = vp_Weapon.GetComponent<vp_ItemIdentifier>();
					if (component != null)
					{
						this.m_WeaponIdentifiers.Add(vp_Weapon, component);
					}
				}
			}
			return this.m_WeaponIdentifiers;
		}
	}

	// Token: 0x170014F1 RID: 5361
	// (get) Token: 0x0600B1BB RID: 45499 RVA: 0x00426538 File Offset: 0x00424738
	public Dictionary<vp_UnitType, List<vp_Weapon>> WeaponsByUnit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_WeaponsByUnit == null)
			{
				this.m_WeaponsByUnit = new Dictionary<vp_UnitType, List<vp_Weapon>>();
				foreach (vp_Weapon vp_Weapon in this.WeaponHandler.Weapons)
				{
					vp_ItemIdentifier vp_ItemIdentifier;
					if (this.WeaponIdentifiers.TryGetValue(vp_Weapon, out vp_ItemIdentifier) && vp_ItemIdentifier != null)
					{
						vp_UnitBankType vp_UnitBankType = vp_ItemIdentifier.Type as vp_UnitBankType;
						if (vp_UnitBankType != null && vp_UnitBankType.Unit != null)
						{
							List<vp_Weapon> list;
							if (this.m_WeaponsByUnit.TryGetValue(vp_UnitBankType.Unit, out list))
							{
								if (list == null)
								{
									list = new List<vp_Weapon>();
								}
								this.m_WeaponsByUnit.Remove(vp_UnitBankType.Unit);
							}
							else
							{
								list = new List<vp_Weapon>();
							}
							list.Add(vp_Weapon);
							this.m_WeaponsByUnit.Add(vp_UnitBankType.Unit, list);
						}
					}
				}
			}
			return this.m_WeaponsByUnit;
		}
	}

	// Token: 0x170014F2 RID: 5362
	// (get) Token: 0x0600B1BC RID: 45500 RVA: 0x00426640 File Offset: 0x00424840
	public virtual vp_ItemInstance CurrentWeaponInstance
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (Application.isPlaying && this.WeaponHandler.CurrentWeaponIndex == 0)
			{
				this.m_CurrentWeaponInstance = null;
				return null;
			}
			if (this.m_CurrentWeaponInstance == null)
			{
				if (this.CurrentWeaponIdentifier == null)
				{
					this.MissingIdentifierError(0);
					this.m_CurrentWeaponInstance = null;
					return null;
				}
				this.m_CurrentWeaponInstance = base.GetItem(this.CurrentWeaponIdentifier.Type, this.CurrentWeaponIdentifier.ID);
			}
			return this.m_CurrentWeaponInstance;
		}
	}

	// Token: 0x170014F3 RID: 5363
	// (get) Token: 0x0600B1BD RID: 45501 RVA: 0x004266B9 File Offset: 0x004248B9
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null)
			{
				this.m_Player = base.transform.GetComponent<vp_PlayerEventHandler>();
			}
			return this.m_Player;
		}
	}

	// Token: 0x170014F4 RID: 5364
	// (get) Token: 0x0600B1BE RID: 45502 RVA: 0x004266E0 File Offset: 0x004248E0
	public vp_WeaponHandler WeaponHandler
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_WeaponHandler == null)
			{
				this.m_WeaponHandler = base.transform.GetComponent<vp_WeaponHandler>();
			}
			return this.m_WeaponHandler;
		}
	}

	// Token: 0x170014F5 RID: 5365
	// (get) Token: 0x0600B1BF RID: 45503 RVA: 0x00426707 File Offset: 0x00424907
	public vp_ItemIdentifier CurrentWeaponIdentifier
	{
		get
		{
			if (!Application.isPlaying)
			{
				return null;
			}
			return this.GetWeaponIdentifier(this.WeaponHandler.CurrentWeapon);
		}
	}

	// Token: 0x0600B1C0 RID: 45504 RVA: 0x00426724 File Offset: 0x00424924
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual vp_ItemIdentifier GetWeaponIdentifier(vp_Weapon weapon)
	{
		if (!Application.isPlaying)
		{
			return null;
		}
		if (weapon == null)
		{
			return null;
		}
		if (!this.WeaponIdentifiers.TryGetValue(weapon, out this.m_WeaponIdentifierResult))
		{
			if (weapon == null)
			{
				return null;
			}
			this.m_WeaponIdentifierResult = weapon.GetComponent<vp_ItemIdentifier>();
			if (this.m_WeaponIdentifierResult == null)
			{
				return null;
			}
			if (this.m_WeaponIdentifierResult.Type == null)
			{
				return null;
			}
			this.WeaponIdentifiers.Add(weapon, this.m_WeaponIdentifierResult);
		}
		return this.m_WeaponIdentifierResult;
	}

	// Token: 0x0600B1C1 RID: 45505 RVA: 0x004267AD File Offset: 0x004249AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.Player == null || this.WeaponHandler == null)
		{
			Debug.LogError(this.m_MissingHandlerError);
		}
	}

	// Token: 0x0600B1C2 RID: 45506 RVA: 0x004267DC File Offset: 0x004249DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
		this.UnwieldMissingWeapon();
	}

	// Token: 0x0600B1C3 RID: 45507 RVA: 0x00426804 File Offset: 0x00424A04
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1C4 RID: 45508 RVA: 0x00426828 File Offset: 0x00424A28
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool MissingIdentifierError(int weaponIndex = 0)
	{
		if (!Application.isPlaying)
		{
			return false;
		}
		if (weaponIndex < 1)
		{
			return false;
		}
		if (this.WeaponHandler == null)
		{
			return false;
		}
		if (this.WeaponHandler.Weapons.Count <= weaponIndex - 1)
		{
			return false;
		}
		Debug.LogWarning(string.Format("Warning: Weapon gameobject '" + this.WeaponHandler.Weapons[weaponIndex - 1].name + "' lacks a properly set up vp_ItemIdentifier component!", Array.Empty<object>()));
		return false;
	}

	// Token: 0x0600B1C5 RID: 45509 RVA: 0x004268A4 File Offset: 0x00424AA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddItem(vp_ItemType type, int id)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(type, -1) : this.HaveItem(type, id));
		base.DoAddItem(type, id);
		this.TryWieldNewItem(type, flag);
	}

	// Token: 0x0600B1C6 RID: 45510 RVA: 0x004268DB File Offset: 0x00424ADB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveItem(vp_ItemInstance item)
	{
		this.Unwield(item);
		base.DoRemoveItem(item);
	}

	// Token: 0x0600B1C7 RID: 45511 RVA: 0x004268EC File Offset: 0x00424AEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddUnitBank(vp_UnitBankType unitBankType, int id, int unitsLoaded)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(unitBankType, -1) : this.HaveItem(unitBankType, id));
		base.DoAddUnitBank(unitBankType, id, unitsLoaded);
		this.TryWieldNewItem(unitBankType, flag);
	}

	// Token: 0x0600B1C8 RID: 45512 RVA: 0x00426924 File Offset: 0x00424B24
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TryWieldNewItem(vp_ItemType type, bool alreadyHaveIt)
	{
		bool flag = this.m_PreviouslyOwnedItems.ContainsKey(type);
		if (!flag)
		{
			this.m_PreviouslyOwnedItems.Add(type, null);
		}
		if (!this.m_AutoWield.Always && (!this.m_AutoWield.IfUnarmed || this.WeaponHandler.CurrentWeaponIndex >= 1) && (!this.m_AutoWield.IfOutOfAmmo || this.WeaponHandler.CurrentWeaponIndex <= 0 || this.WeaponHandler.CurrentWeapon.AnimationType == 2 || this.m_Player.CurrentWeaponAmmoCount.Get() >= 1) && (!this.m_AutoWield.IfNotPresent || this.m_AutoWield.FirstTimeOnly || alreadyHaveIt) && (!this.m_AutoWield.FirstTimeOnly || flag))
		{
			return;
		}
		if (type is vp_UnitBankType)
		{
			this.TryWield(this.GetItem(type));
			return;
		}
		if (type is vp_UnitType)
		{
			this.TryWieldByUnit(type as vp_UnitType);
			return;
		}
		if (type != null)
		{
			this.TryWield(this.GetItem(type));
			return;
		}
		Type type2 = type.GetType();
		if (type2 == null)
		{
			return;
		}
		type2 = type2.BaseType;
		if (type2 == typeof(vp_UnitBankType))
		{
			this.TryWield(this.GetItem(type));
			return;
		}
		if (type2 == typeof(vp_UnitType))
		{
			this.TryWieldByUnit(type as vp_UnitType);
			return;
		}
		if (type2 == typeof(vp_ItemType))
		{
			this.TryWield(this.GetItem(type));
		}
	}

	// Token: 0x0600B1C9 RID: 45513 RVA: 0x00426AA1 File Offset: 0x00424CA1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveUnitBank(vp_UnitBankInstance bank)
	{
		this.Unwield(bank);
		base.DoRemoveUnitBank(bank);
	}

	// Token: 0x0600B1CA RID: 45514 RVA: 0x00426AB4 File Offset: 0x00424CB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual vp_Weapon GetWeaponOfItemInstance(vp_ItemInstance itemInstance)
	{
		if (this.m_ItemWeapons == null)
		{
			this.m_ItemWeapons = new Dictionary<vp_ItemInstance, vp_Weapon>();
		}
		vp_Weapon vp_Weapon;
		this.m_ItemWeapons.TryGetValue(itemInstance, out vp_Weapon);
		if (vp_Weapon != null)
		{
			return vp_Weapon;
		}
		try
		{
			for (int i = 0; i < this.WeaponHandler.Weapons.Count; i++)
			{
				vp_ItemInstance itemInstanceOfWeapon = this.GetItemInstanceOfWeapon(this.WeaponHandler.Weapons[i]);
				Debug.Log("weapon with index: " + i.ToString() + ", item instance: " + ((itemInstanceOfWeapon == null) ? "(have none)" : itemInstanceOfWeapon.Type.ToString()));
				if (itemInstanceOfWeapon != null && itemInstanceOfWeapon.Type == itemInstance.Type)
				{
					vp_Weapon = this.WeaponHandler.Weapons[i];
					this.m_ItemWeapons.Add(itemInstanceOfWeapon, vp_Weapon);
					return vp_Weapon;
				}
			}
		}
		catch
		{
			Debug.LogError("Exception " + ((this != null) ? this.ToString() : null) + " Crashed while trying to get item instance for a weapon. Likely a nullreference.");
		}
		return null;
	}

	// Token: 0x0600B1CB RID: 45515 RVA: 0x00426BCC File Offset: 0x00424DCC
	public override bool DoAddUnits(vp_UnitBankInstance bank, int amount)
	{
		if (bank == null)
		{
			return false;
		}
		int unitCount = this.GetUnitCount(bank.UnitType);
		bool flag = base.DoAddUnits(bank, amount);
		if (flag && bank.IsInternal)
		{
			try
			{
				this.TryWieldNewItem(bank.UnitType, unitCount != 0);
			}
			catch
			{
			}
			if (!Application.isPlaying || this.WeaponHandler.CurrentWeaponIndex != 0)
			{
				vp_UnitBankInstance vp_UnitBankInstance = this.CurrentWeaponInstance as vp_UnitBankInstance;
				if (vp_UnitBankInstance != null && bank.UnitType == vp_UnitBankInstance.UnitType && vp_UnitBankInstance.Count == 0)
				{
					this.Player.AutoReload.Try();
				}
			}
		}
		return flag;
	}

	// Token: 0x0600B1CC RID: 45516 RVA: 0x00426C7C File Offset: 0x00424E7C
	public override bool DoRemoveUnits(vp_UnitBankInstance bank, int amount)
	{
		bool flag = base.DoRemoveUnits(bank, amount);
		if (bank.Count == 0)
		{
			vp_Timer.In(0.3f, delegate
			{
				this.Player.AutoReload.Try();
			}, null);
		}
		return flag;
	}

	// Token: 0x0600B1CD RID: 45517 RVA: 0x00426CA5 File Offset: 0x00424EA5
	public vp_UnitBankInstance GetUnitBankInstanceOfWeapon(vp_Weapon weapon)
	{
		return this.GetItemInstanceOfWeapon(weapon) as vp_UnitBankInstance;
	}

	// Token: 0x0600B1CE RID: 45518 RVA: 0x00426CB4 File Offset: 0x00424EB4
	public vp_ItemInstance GetItemInstanceOfWeapon(vp_Weapon weapon)
	{
		vp_ItemIdentifier weaponIdentifier = this.GetWeaponIdentifier(weapon);
		if (weaponIdentifier == null)
		{
			return null;
		}
		return this.GetItem(weaponIdentifier.Type);
	}

	// Token: 0x0600B1CF RID: 45519 RVA: 0x00426CE0 File Offset: 0x00424EE0
	public int GetAmmoInWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return unitBankInstanceOfWeapon.Count;
	}

	// Token: 0x0600B1D0 RID: 45520 RVA: 0x00426D00 File Offset: 0x00424F00
	public int GetExtraAmmoForWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return this.GetUnitCount(unitBankInstanceOfWeapon.UnitType);
	}

	// Token: 0x0600B1D1 RID: 45521 RVA: 0x00426D26 File Offset: 0x00424F26
	public int GetAmmoInCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponAmmoCount;
	}

	// Token: 0x0600B1D2 RID: 45522 RVA: 0x00426D2E File Offset: 0x00424F2E
	public int GetExtraAmmoForCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponClipCount;
	}

	// Token: 0x0600B1D3 RID: 45523 RVA: 0x00426D38 File Offset: 0x00424F38
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UnwieldMissingWeapon()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.WeaponHandler.CurrentWeaponIndex < 1)
		{
			return;
		}
		if (this.CurrentWeaponIdentifier != null && this.HaveItem(this.CurrentWeaponIdentifier.Type, this.CurrentWeaponIdentifier.ID))
		{
			return;
		}
		if (this.CurrentWeaponIdentifier == null)
		{
			this.MissingIdentifierError(this.WeaponHandler.CurrentWeaponIndex);
		}
		this.Player.SetWeapon.TryStart<int>(0);
	}

	// Token: 0x0600B1D4 RID: 45524 RVA: 0x00426DBC File Offset: 0x00424FBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool TryWieldByUnit(vp_UnitType unitType)
	{
		List<vp_Weapon> list;
		if (this.WeaponsByUnit.TryGetValue(unitType, out list) && list != null && list.Count > 0)
		{
			foreach (vp_Weapon vp_Weapon in list)
			{
				if (this.m_Player.SetWeapon.TryStart<int>(this.WeaponHandler.Weapons.IndexOf(vp_Weapon) + 1))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600B1D5 RID: 45525 RVA: 0x00426E4C File Offset: 0x0042504C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TryWield(vp_ItemInstance item)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.Player.Dead.Active)
		{
			return;
		}
		if (!this.WeaponHandler.enabled)
		{
			return;
		}
		for (int i = 1; i < this.WeaponHandler.Weapons.Count + 1; i++)
		{
			vp_ItemIdentifier weaponIdentifier = this.GetWeaponIdentifier(this.WeaponHandler.Weapons[i - 1]);
			if (!(weaponIdentifier == null) && !(item.Type != weaponIdentifier.Type) && (weaponIdentifier.ID == 0 || item.ID == weaponIdentifier.ID))
			{
				this.Player.SetWeapon.TryStart<int>(i);
				return;
			}
		}
	}

	// Token: 0x0600B1D6 RID: 45526 RVA: 0x00426F00 File Offset: 0x00425100
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Unwield(vp_ItemInstance item)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.WeaponHandler.CurrentWeaponIndex == 0)
		{
			return;
		}
		if (this.CurrentWeaponIdentifier == null)
		{
			this.MissingIdentifierError(0);
			return;
		}
		if (item.Type != this.CurrentWeaponIdentifier.Type)
		{
			return;
		}
		if (this.CurrentWeaponIdentifier.ID != 0 && item.ID != this.CurrentWeaponIdentifier.ID)
		{
			return;
		}
		this.Player.SetWeapon.Start(0f);
		if (!this.Player.Dead.Active)
		{
			vp_Timer.In(0.35f, delegate
			{
				this.Player.SetNextWeapon.Try();
			}, null);
		}
		vp_Timer.In(1f, new vp_Timer.Callback(this.UnwieldMissingWeapon), null);
	}

	// Token: 0x0600B1D7 RID: 45527 RVA: 0x00426FCA File Offset: 0x004251CA
	public override void Refresh()
	{
		base.Refresh();
		this.UnwieldMissingWeapon();
	}

	// Token: 0x0600B1D8 RID: 45528 RVA: 0x00426FD8 File Offset: 0x004251D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.Player.SetWeapon.Argument;
		if (num == 0)
		{
			return true;
		}
		if (num < 1 || num > this.WeaponHandler.Weapons.Count)
		{
			return false;
		}
		vp_ItemIdentifier weaponIdentifier = this.GetWeaponIdentifier(this.WeaponHandler.Weapons[num - 1]);
		if (weaponIdentifier == null)
		{
			return this.MissingIdentifierError(num);
		}
		bool flag = this.HaveItem(weaponIdentifier.Type, weaponIdentifier.ID);
		if (flag && this.WeaponHandler.Weapons[num - 1].AnimationType == 3 && this.GetAmmoInWeapon(this.WeaponHandler.Weapons[num - 1]) < 1)
		{
			vp_UnitBankType vp_UnitBankType = weaponIdentifier.Type as vp_UnitBankType;
			if (vp_UnitBankType == null)
			{
				string[] array = new string[5];
				array[0] = "Error (";
				array[1] = ((this != null) ? this.ToString() : null);
				array[2] = ") Tried to wield thrown weapon ";
				int num2 = 3;
				vp_Weapon vp_Weapon = this.WeaponHandler.Weapons[num - 1];
				array[num2] = ((vp_Weapon != null) ? vp_Weapon.ToString() : null);
				array[4] = " but its item identifier does not point to a UnitBank.";
				Debug.LogError(string.Concat(array));
				return false;
			}
			if (!this.TryReload(vp_UnitBankType, weaponIdentifier.ID))
			{
				return false;
			}
		}
		return flag;
	}

	// Token: 0x0600B1D9 RID: 45529 RVA: 0x00427120 File Offset: 0x00425320
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_DepleteAmmo()
	{
		if (this.CurrentWeaponIdentifier == null)
		{
			return this.MissingIdentifierError(0);
		}
		if (this.WeaponHandler.CurrentWeapon.AnimationType == 3)
		{
			this.TryReload(this.CurrentWeaponInstance as vp_UnitBankInstance);
		}
		return this.TryDeduct(this.CurrentWeaponIdentifier.Type as vp_UnitBankType, this.CurrentWeaponIdentifier.ID, 1);
	}

	// Token: 0x0600B1DA RID: 45530 RVA: 0x0042718A File Offset: 0x0042538A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RefillCurrentWeapon()
	{
		if (this.CurrentWeaponIdentifier == null)
		{
			return this.MissingIdentifierError(0);
		}
		return this.TryReload(this.CurrentWeaponIdentifier.Type as vp_UnitBankType, this.CurrentWeaponIdentifier.ID);
	}

	// Token: 0x0600B1DB RID: 45531 RVA: 0x004271C3 File Offset: 0x004253C3
	public override void Reset()
	{
		this.m_PreviouslyOwnedItems.Clear();
		this.m_CurrentWeaponInstance = null;
		if (!this.m_Misc.ResetOnRespawn)
		{
			return;
		}
		base.Reset();
	}

	// Token: 0x170014F6 RID: 5366
	// (get) Token: 0x0600B1DC RID: 45532 RVA: 0x004271EC File Offset: 0x004253EC
	// (set) Token: 0x0600B1DD RID: 45533 RVA: 0x00427210 File Offset: 0x00425410
	public virtual int OnValue_CurrentWeaponAmmoCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			vp_UnitBankInstance vp_UnitBankInstance = this.CurrentWeaponInstance as vp_UnitBankInstance;
			if (vp_UnitBankInstance == null)
			{
				return 0;
			}
			return vp_UnitBankInstance.Count;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			vp_UnitBankInstance vp_UnitBankInstance = this.CurrentWeaponInstance as vp_UnitBankInstance;
			if (vp_UnitBankInstance == null)
			{
				return;
			}
			vp_UnitBankInstance.TryGiveUnits(value);
		}
	}

	// Token: 0x170014F7 RID: 5367
	// (get) Token: 0x0600B1DE RID: 45534 RVA: 0x00427238 File Offset: 0x00425438
	public virtual int OnValue_CurrentWeaponMaxAmmoCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			vp_UnitBankInstance vp_UnitBankInstance = this.CurrentWeaponInstance as vp_UnitBankInstance;
			if (vp_UnitBankInstance == null)
			{
				return 0;
			}
			return vp_UnitBankInstance.Capacity;
		}
	}

	// Token: 0x170014F8 RID: 5368
	// (get) Token: 0x0600B1DF RID: 45535 RVA: 0x0042725C File Offset: 0x0042545C
	public virtual int OnValue_CurrentWeaponClipCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			vp_UnitBankInstance vp_UnitBankInstance = this.CurrentWeaponInstance as vp_UnitBankInstance;
			if (vp_UnitBankInstance == null)
			{
				return 0;
			}
			return this.GetUnitCount(vp_UnitBankInstance.UnitType);
		}
	}

	// Token: 0x0600B1E0 RID: 45536 RVA: 0x00427288 File Offset: 0x00425488
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual int OnMessage_GetItemCount(string itemTypeObjectName)
	{
		vp_ItemInstance item = this.GetItem(itemTypeObjectName);
		if (item == null)
		{
			return 0;
		}
		vp_UnitBankInstance vp_UnitBankInstance = item as vp_UnitBankInstance;
		if (vp_UnitBankInstance != null && vp_UnitBankInstance.IsInternal)
		{
			return this.GetItemCount(vp_UnitBankInstance.UnitType);
		}
		return this.GetItemCount(item.Type);
	}

	// Token: 0x0600B1E1 RID: 45537 RVA: 0x004272D0 File Offset: 0x004254D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AddItem(object args)
	{
		object[] array = (object[])args;
		vp_ItemType vp_ItemType = array[0] as vp_ItemType;
		if (vp_ItemType == null)
		{
			return false;
		}
		int num = ((array.Length == 2) ? ((int)array[1]) : 1);
		if (vp_ItemType is vp_UnitType)
		{
			return this.TryGiveUnits(vp_ItemType as vp_UnitType, num);
		}
		return this.TryGiveItems(vp_ItemType, num);
	}

	// Token: 0x0600B1E2 RID: 45538 RVA: 0x00427328 File Offset: 0x00425528
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RemoveItem(object args)
	{
		object[] array = (object[])args;
		vp_ItemType vp_ItemType = array[0] as vp_ItemType;
		if (vp_ItemType == null)
		{
			return false;
		}
		int num = ((array.Length == 2) ? ((int)array[1]) : 1);
		if (vp_ItemType is vp_UnitType)
		{
			return this.TryRemoveUnits(vp_ItemType as vp_UnitType, num);
		}
		return this.TryRemoveItems(vp_ItemType, num);
	}

	// Token: 0x170014F9 RID: 5369
	// (get) Token: 0x0600B1E3 RID: 45539 RVA: 0x00427380 File Offset: 0x00425580
	public virtual Texture2D OnValue_CurrentAmmoIcon
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.CurrentWeaponInstance == null)
			{
				return null;
			}
			if (this.CurrentWeaponInstance.Type == null)
			{
				return null;
			}
			vp_UnitBankType vp_UnitBankType = this.CurrentWeaponInstance.Type as vp_UnitBankType;
			if (vp_UnitBankType == null)
			{
				return null;
			}
			if (vp_UnitBankType.Unit == null)
			{
				return null;
			}
			return vp_UnitBankType.Unit.Icon;
		}
	}

	// Token: 0x0600B1E4 RID: 45540 RVA: 0x004273E3 File Offset: 0x004255E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_CurrentWeaponInstance = null;
	}

	// Token: 0x0400858D RID: 34189
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, object> m_PreviouslyOwnedItems = new Dictionary<vp_ItemType, object>();

	// Token: 0x0400858E RID: 34190
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_WeaponIdentifierResult;

	// Token: 0x0400858F RID: 34191
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_MissingHandlerError = "Error (vp_PlayerInventory) this component must be on the same transform as a vp_PlayerEventHandler + vp_WeaponHandler.";

	// Token: 0x04008590 RID: 34192
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitBankInstance, vp_Weapon> m_UnitBankWeapons;

	// Token: 0x04008591 RID: 34193
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemInstance, vp_Weapon> m_ItemWeapons;

	// Token: 0x04008592 RID: 34194
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_Weapon, vp_ItemIdentifier> m_WeaponIdentifiers;

	// Token: 0x04008593 RID: 34195
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitType, List<vp_Weapon>> m_WeaponsByUnit;

	// Token: 0x04008594 RID: 34196
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_CurrentWeaponInstance;

	// Token: 0x04008595 RID: 34197
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008596 RID: 34198
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008597 RID: 34199
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_PlayerInventory.AutoWieldSection m_AutoWield;

	// Token: 0x04008598 RID: 34200
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_PlayerInventory.MiscSection m_Misc;

	// Token: 0x0200162A RID: 5674
	[Serializable]
	public class AutoWieldSection
	{
		// Token: 0x04008599 RID: 34201
		public bool Always;

		// Token: 0x0400859A RID: 34202
		public bool IfUnarmed = true;

		// Token: 0x0400859B RID: 34203
		public bool IfOutOfAmmo = true;

		// Token: 0x0400859C RID: 34204
		public bool IfNotPresent = true;

		// Token: 0x0400859D RID: 34205
		public bool FirstTimeOnly = true;
	}

	// Token: 0x0200162B RID: 5675
	[Serializable]
	public class MiscSection
	{
		// Token: 0x0400859E RID: 34206
		public bool ResetOnRespawn = true;
	}
}
