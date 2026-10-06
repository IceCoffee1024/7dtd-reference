using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001619 RID: 5657
public class vp_FPInventory : vp_Inventory
{
	// Token: 0x170014BD RID: 5309
	// (get) Token: 0x0600B0C7 RID: 45255 RVA: 0x0041F220 File Offset: 0x0041D420
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

	// Token: 0x170014BE RID: 5310
	// (get) Token: 0x0600B0C8 RID: 45256 RVA: 0x0041F2A8 File Offset: 0x0041D4A8
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

	// Token: 0x170014BF RID: 5311
	// (get) Token: 0x0600B0C9 RID: 45257 RVA: 0x0041F3B0 File Offset: 0x0041D5B0
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

	// Token: 0x170014C0 RID: 5312
	// (get) Token: 0x0600B0CA RID: 45258 RVA: 0x0041F429 File Offset: 0x0041D629
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

	// Token: 0x170014C1 RID: 5313
	// (get) Token: 0x0600B0CB RID: 45259 RVA: 0x0041F450 File Offset: 0x0041D650
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

	// Token: 0x170014C2 RID: 5314
	// (get) Token: 0x0600B0CC RID: 45260 RVA: 0x0041F477 File Offset: 0x0041D677
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

	// Token: 0x0600B0CD RID: 45261 RVA: 0x0041F494 File Offset: 0x0041D694
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

	// Token: 0x0600B0CE RID: 45262 RVA: 0x0041F520 File Offset: 0x0041D720
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		Debug.LogWarning("Warning (" + ((this != null) ? this.ToString() : null) + ") The 'vp_FPInventory' class is obsolete. Please replace this component with a 'vp_PlayerInventory' component.");
		base.Awake();
		if (this.Player == null || this.WeaponHandler == null)
		{
			Debug.LogError(this.m_MissingHandlerError);
		}
	}

	// Token: 0x0600B0CF RID: 45263 RVA: 0x0041F57B File Offset: 0x0041D77B
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

	// Token: 0x0600B0D0 RID: 45264 RVA: 0x0041F5A3 File Offset: 0x0041D7A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B0D1 RID: 45265 RVA: 0x0041F5C8 File Offset: 0x0041D7C8
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

	// Token: 0x0600B0D2 RID: 45266 RVA: 0x0041F644 File Offset: 0x0041D844
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddItem(vp_ItemType type, int id)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(type, -1) : this.HaveItem(type, id));
		base.DoAddItem(type, id);
		this.TryWieldNewItem(type, flag);
	}

	// Token: 0x0600B0D3 RID: 45267 RVA: 0x0041F67B File Offset: 0x0041D87B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveItem(vp_ItemInstance item)
	{
		this.Unwield(item);
		base.DoRemoveItem(item);
	}

	// Token: 0x0600B0D4 RID: 45268 RVA: 0x0041F68C File Offset: 0x0041D88C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddUnitBank(vp_UnitBankType unitBankType, int id, int unitsLoaded)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(unitBankType, -1) : this.HaveItem(unitBankType, id));
		base.DoAddUnitBank(unitBankType, id, unitsLoaded);
		this.TryWieldNewItem(unitBankType, flag);
	}

	// Token: 0x0600B0D5 RID: 45269 RVA: 0x0041F6C4 File Offset: 0x0041D8C4
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

	// Token: 0x0600B0D6 RID: 45270 RVA: 0x0041F841 File Offset: 0x0041DA41
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveUnitBank(vp_UnitBankInstance bank)
	{
		this.Unwield(bank);
		base.DoRemoveUnitBank(bank);
	}

	// Token: 0x0600B0D7 RID: 45271 RVA: 0x0041F854 File Offset: 0x0041DA54
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

	// Token: 0x0600B0D8 RID: 45272 RVA: 0x0041F96C File Offset: 0x0041DB6C
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

	// Token: 0x0600B0D9 RID: 45273 RVA: 0x0041FA1C File Offset: 0x0041DC1C
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

	// Token: 0x0600B0DA RID: 45274 RVA: 0x0041FA45 File Offset: 0x0041DC45
	public vp_UnitBankInstance GetUnitBankInstanceOfWeapon(vp_Weapon weapon)
	{
		return this.GetItemInstanceOfWeapon(weapon) as vp_UnitBankInstance;
	}

	// Token: 0x0600B0DB RID: 45275 RVA: 0x0041FA54 File Offset: 0x0041DC54
	public vp_ItemInstance GetItemInstanceOfWeapon(vp_Weapon weapon)
	{
		vp_ItemIdentifier weaponIdentifier = this.GetWeaponIdentifier(weapon);
		if (weaponIdentifier == null)
		{
			return null;
		}
		return this.GetItem(weaponIdentifier.Type);
	}

	// Token: 0x0600B0DC RID: 45276 RVA: 0x0041FA80 File Offset: 0x0041DC80
	public int GetAmmoInWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return unitBankInstanceOfWeapon.Count;
	}

	// Token: 0x0600B0DD RID: 45277 RVA: 0x0041FAA0 File Offset: 0x0041DCA0
	public int GetExtraAmmoForWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return this.GetUnitCount(unitBankInstanceOfWeapon.UnitType);
	}

	// Token: 0x0600B0DE RID: 45278 RVA: 0x0041FAC6 File Offset: 0x0041DCC6
	public int GetAmmoInCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponAmmoCount;
	}

	// Token: 0x0600B0DF RID: 45279 RVA: 0x0041FACE File Offset: 0x0041DCCE
	public int GetExtraAmmoForCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponClipCount;
	}

	// Token: 0x0600B0E0 RID: 45280 RVA: 0x0041FAD8 File Offset: 0x0041DCD8
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

	// Token: 0x0600B0E1 RID: 45281 RVA: 0x0041FB5C File Offset: 0x0041DD5C
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

	// Token: 0x0600B0E2 RID: 45282 RVA: 0x0041FBEC File Offset: 0x0041DDEC
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

	// Token: 0x0600B0E3 RID: 45283 RVA: 0x0041FCA0 File Offset: 0x0041DEA0
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

	// Token: 0x0600B0E4 RID: 45284 RVA: 0x0041FD6A File Offset: 0x0041DF6A
	public override void Refresh()
	{
		base.Refresh();
		this.UnwieldMissingWeapon();
	}

	// Token: 0x0600B0E5 RID: 45285 RVA: 0x0041FD78 File Offset: 0x0041DF78
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

	// Token: 0x0600B0E6 RID: 45286 RVA: 0x0041FEC0 File Offset: 0x0041E0C0
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

	// Token: 0x0600B0E7 RID: 45287 RVA: 0x0041FF2A File Offset: 0x0041E12A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RefillCurrentWeapon()
	{
		if (this.CurrentWeaponIdentifier == null)
		{
			return this.MissingIdentifierError(0);
		}
		return this.TryReload(this.CurrentWeaponIdentifier.Type as vp_UnitBankType, this.CurrentWeaponIdentifier.ID);
	}

	// Token: 0x0600B0E8 RID: 45288 RVA: 0x0041FF63 File Offset: 0x0041E163
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

	// Token: 0x170014C3 RID: 5315
	// (get) Token: 0x0600B0E9 RID: 45289 RVA: 0x0041FF8C File Offset: 0x0041E18C
	// (set) Token: 0x0600B0EA RID: 45290 RVA: 0x0041FFB0 File Offset: 0x0041E1B0
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

	// Token: 0x170014C4 RID: 5316
	// (get) Token: 0x0600B0EB RID: 45291 RVA: 0x0041FFD8 File Offset: 0x0041E1D8
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

	// Token: 0x170014C5 RID: 5317
	// (get) Token: 0x0600B0EC RID: 45292 RVA: 0x0041FFFC File Offset: 0x0041E1FC
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

	// Token: 0x0600B0ED RID: 45293 RVA: 0x00420028 File Offset: 0x0041E228
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

	// Token: 0x0600B0EE RID: 45294 RVA: 0x00420070 File Offset: 0x0041E270
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

	// Token: 0x0600B0EF RID: 45295 RVA: 0x004200C8 File Offset: 0x0041E2C8
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

	// Token: 0x170014C6 RID: 5318
	// (get) Token: 0x0600B0F0 RID: 45296 RVA: 0x00420120 File Offset: 0x0041E320
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

	// Token: 0x0600B0F1 RID: 45297 RVA: 0x00420183 File Offset: 0x0041E383
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_CurrentWeaponInstance = null;
	}

	// Token: 0x04008415 RID: 33813
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, object> m_PreviouslyOwnedItems = new Dictionary<vp_ItemType, object>();

	// Token: 0x04008416 RID: 33814
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_WeaponIdentifierResult;

	// Token: 0x04008417 RID: 33815
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_MissingHandlerError = "Error (vp_FPInventory) this component must be on the same transform as a vp_PlayerEventHandler + vp_WeaponHandler.";

	// Token: 0x04008418 RID: 33816
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitBankInstance, vp_Weapon> m_UnitBankWeapons;

	// Token: 0x04008419 RID: 33817
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemInstance, vp_Weapon> m_ItemWeapons;

	// Token: 0x0400841A RID: 33818
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_Weapon, vp_ItemIdentifier> m_WeaponIdentifiers;

	// Token: 0x0400841B RID: 33819
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitType, List<vp_Weapon>> m_WeaponsByUnit;

	// Token: 0x0400841C RID: 33820
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_CurrentWeaponInstance;

	// Token: 0x0400841D RID: 33821
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400841E RID: 33822
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x0400841F RID: 33823
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_FPInventory.AutoWieldSection m_AutoWield;

	// Token: 0x04008420 RID: 33824
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_FPInventory.MiscSection m_Misc;

	// Token: 0x0200161A RID: 5658
	[Serializable]
	public class AutoWieldSection
	{
		// Token: 0x04008421 RID: 33825
		public bool Always;

		// Token: 0x04008422 RID: 33826
		public bool IfUnarmed = true;

		// Token: 0x04008423 RID: 33827
		public bool IfOutOfAmmo = true;

		// Token: 0x04008424 RID: 33828
		public bool IfNotPresent = true;

		// Token: 0x04008425 RID: 33829
		public bool FirstTimeOnly = true;
	}

	// Token: 0x0200161B RID: 5659
	[Serializable]
	public class MiscSection
	{
		// Token: 0x04008426 RID: 33830
		public bool ResetOnRespawn = true;
	}
}
