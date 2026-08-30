using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200160A RID: 5642
public class vp_PlayerInventory : vp_Inventory
{
	// Token: 0x170014AD RID: 5293
	// (get) Token: 0x0600B128 RID: 45352 RVA: 0x004227D4 File Offset: 0x004209D4
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

	// Token: 0x170014AE RID: 5294
	// (get) Token: 0x0600B129 RID: 45353 RVA: 0x0042285C File Offset: 0x00420A5C
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

	// Token: 0x170014AF RID: 5295
	// (get) Token: 0x0600B12A RID: 45354 RVA: 0x00422964 File Offset: 0x00420B64
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

	// Token: 0x170014B0 RID: 5296
	// (get) Token: 0x0600B12B RID: 45355 RVA: 0x004229DD File Offset: 0x00420BDD
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

	// Token: 0x170014B1 RID: 5297
	// (get) Token: 0x0600B12C RID: 45356 RVA: 0x00422A04 File Offset: 0x00420C04
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

	// Token: 0x170014B2 RID: 5298
	// (get) Token: 0x0600B12D RID: 45357 RVA: 0x00422A2B File Offset: 0x00420C2B
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

	// Token: 0x0600B12E RID: 45358 RVA: 0x00422A48 File Offset: 0x00420C48
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

	// Token: 0x0600B12F RID: 45359 RVA: 0x00422AD1 File Offset: 0x00420CD1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.Player == null || this.WeaponHandler == null)
		{
			Debug.LogError(this.m_MissingHandlerError);
		}
	}

	// Token: 0x0600B130 RID: 45360 RVA: 0x00422B00 File Offset: 0x00420D00
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

	// Token: 0x0600B131 RID: 45361 RVA: 0x00422B28 File Offset: 0x00420D28
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B132 RID: 45362 RVA: 0x00422B4C File Offset: 0x00420D4C
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

	// Token: 0x0600B133 RID: 45363 RVA: 0x00422BC8 File Offset: 0x00420DC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddItem(vp_ItemType type, int id)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(type, -1) : this.HaveItem(type, id));
		base.DoAddItem(type, id);
		this.TryWieldNewItem(type, flag);
	}

	// Token: 0x0600B134 RID: 45364 RVA: 0x00422BFF File Offset: 0x00420DFF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveItem(vp_ItemInstance item)
	{
		this.Unwield(item);
		base.DoRemoveItem(item);
	}

	// Token: 0x0600B135 RID: 45365 RVA: 0x00422C10 File Offset: 0x00420E10
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddUnitBank(vp_UnitBankType unitBankType, int id, int unitsLoaded)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(unitBankType, -1) : this.HaveItem(unitBankType, id));
		base.DoAddUnitBank(unitBankType, id, unitsLoaded);
		this.TryWieldNewItem(unitBankType, flag);
	}

	// Token: 0x0600B136 RID: 45366 RVA: 0x00422C48 File Offset: 0x00420E48
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

	// Token: 0x0600B137 RID: 45367 RVA: 0x00422DC5 File Offset: 0x00420FC5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveUnitBank(vp_UnitBankInstance bank)
	{
		this.Unwield(bank);
		base.DoRemoveUnitBank(bank);
	}

	// Token: 0x0600B138 RID: 45368 RVA: 0x00422DD8 File Offset: 0x00420FD8
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

	// Token: 0x0600B139 RID: 45369 RVA: 0x00422EF0 File Offset: 0x004210F0
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

	// Token: 0x0600B13A RID: 45370 RVA: 0x00422FA0 File Offset: 0x004211A0
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

	// Token: 0x0600B13B RID: 45371 RVA: 0x00422FC9 File Offset: 0x004211C9
	public vp_UnitBankInstance GetUnitBankInstanceOfWeapon(vp_Weapon weapon)
	{
		return this.GetItemInstanceOfWeapon(weapon) as vp_UnitBankInstance;
	}

	// Token: 0x0600B13C RID: 45372 RVA: 0x00422FD8 File Offset: 0x004211D8
	public vp_ItemInstance GetItemInstanceOfWeapon(vp_Weapon weapon)
	{
		vp_ItemIdentifier weaponIdentifier = this.GetWeaponIdentifier(weapon);
		if (weaponIdentifier == null)
		{
			return null;
		}
		return this.GetItem(weaponIdentifier.Type);
	}

	// Token: 0x0600B13D RID: 45373 RVA: 0x00423004 File Offset: 0x00421204
	public int GetAmmoInWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return unitBankInstanceOfWeapon.Count;
	}

	// Token: 0x0600B13E RID: 45374 RVA: 0x00423024 File Offset: 0x00421224
	public int GetExtraAmmoForWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return this.GetUnitCount(unitBankInstanceOfWeapon.UnitType);
	}

	// Token: 0x0600B13F RID: 45375 RVA: 0x0042304A File Offset: 0x0042124A
	public int GetAmmoInCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponAmmoCount;
	}

	// Token: 0x0600B140 RID: 45376 RVA: 0x00423052 File Offset: 0x00421252
	public int GetExtraAmmoForCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponClipCount;
	}

	// Token: 0x0600B141 RID: 45377 RVA: 0x0042305C File Offset: 0x0042125C
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

	// Token: 0x0600B142 RID: 45378 RVA: 0x004230E0 File Offset: 0x004212E0
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

	// Token: 0x0600B143 RID: 45379 RVA: 0x00423170 File Offset: 0x00421370
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

	// Token: 0x0600B144 RID: 45380 RVA: 0x00423224 File Offset: 0x00421424
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

	// Token: 0x0600B145 RID: 45381 RVA: 0x004232EE File Offset: 0x004214EE
	public override void Refresh()
	{
		base.Refresh();
		this.UnwieldMissingWeapon();
	}

	// Token: 0x0600B146 RID: 45382 RVA: 0x004232FC File Offset: 0x004214FC
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

	// Token: 0x0600B147 RID: 45383 RVA: 0x00423444 File Offset: 0x00421644
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

	// Token: 0x0600B148 RID: 45384 RVA: 0x004234AE File Offset: 0x004216AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RefillCurrentWeapon()
	{
		if (this.CurrentWeaponIdentifier == null)
		{
			return this.MissingIdentifierError(0);
		}
		return this.TryReload(this.CurrentWeaponIdentifier.Type as vp_UnitBankType, this.CurrentWeaponIdentifier.ID);
	}

	// Token: 0x0600B149 RID: 45385 RVA: 0x004234E7 File Offset: 0x004216E7
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

	// Token: 0x170014B3 RID: 5299
	// (get) Token: 0x0600B14A RID: 45386 RVA: 0x00423510 File Offset: 0x00421710
	// (set) Token: 0x0600B14B RID: 45387 RVA: 0x00423534 File Offset: 0x00421734
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

	// Token: 0x170014B4 RID: 5300
	// (get) Token: 0x0600B14C RID: 45388 RVA: 0x0042355C File Offset: 0x0042175C
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

	// Token: 0x170014B5 RID: 5301
	// (get) Token: 0x0600B14D RID: 45389 RVA: 0x00423580 File Offset: 0x00421780
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

	// Token: 0x0600B14E RID: 45390 RVA: 0x004235AC File Offset: 0x004217AC
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

	// Token: 0x0600B14F RID: 45391 RVA: 0x004235F4 File Offset: 0x004217F4
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

	// Token: 0x0600B150 RID: 45392 RVA: 0x0042364C File Offset: 0x0042184C
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

	// Token: 0x170014B6 RID: 5302
	// (get) Token: 0x0600B151 RID: 45393 RVA: 0x004236A4 File Offset: 0x004218A4
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

	// Token: 0x0600B152 RID: 45394 RVA: 0x00423707 File Offset: 0x00421907
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_CurrentWeaponInstance = null;
	}

	// Token: 0x04008537 RID: 34103
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, object> m_PreviouslyOwnedItems = new Dictionary<vp_ItemType, object>();

	// Token: 0x04008538 RID: 34104
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_WeaponIdentifierResult;

	// Token: 0x04008539 RID: 34105
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_MissingHandlerError = "Error (vp_PlayerInventory) this component must be on the same transform as a vp_PlayerEventHandler + vp_WeaponHandler.";

	// Token: 0x0400853A RID: 34106
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitBankInstance, vp_Weapon> m_UnitBankWeapons;

	// Token: 0x0400853B RID: 34107
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemInstance, vp_Weapon> m_ItemWeapons;

	// Token: 0x0400853C RID: 34108
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_Weapon, vp_ItemIdentifier> m_WeaponIdentifiers;

	// Token: 0x0400853D RID: 34109
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitType, List<vp_Weapon>> m_WeaponsByUnit;

	// Token: 0x0400853E RID: 34110
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_CurrentWeaponInstance;

	// Token: 0x0400853F RID: 34111
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008540 RID: 34112
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008541 RID: 34113
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_PlayerInventory.AutoWieldSection m_AutoWield;

	// Token: 0x04008542 RID: 34114
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_PlayerInventory.MiscSection m_Misc;

	// Token: 0x0200160B RID: 5643
	[Serializable]
	public class AutoWieldSection
	{
		// Token: 0x04008543 RID: 34115
		public bool Always;

		// Token: 0x04008544 RID: 34116
		public bool IfUnarmed = true;

		// Token: 0x04008545 RID: 34117
		public bool IfOutOfAmmo = true;

		// Token: 0x04008546 RID: 34118
		public bool IfNotPresent = true;

		// Token: 0x04008547 RID: 34119
		public bool FirstTimeOnly = true;
	}

	// Token: 0x0200160C RID: 5644
	[Serializable]
	public class MiscSection
	{
		// Token: 0x04008548 RID: 34120
		public bool ResetOnRespawn = true;
	}
}
