using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015EF RID: 5615
public class vp_PlayerInventory : vp_Inventory
{
	// Token: 0x17001474 RID: 5236
	// (get) Token: 0x0600B02B RID: 45099 RVA: 0x0041D494 File Offset: 0x0041B694
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

	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x0600B02C RID: 45100 RVA: 0x0041D51C File Offset: 0x0041B71C
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

	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x0600B02D RID: 45101 RVA: 0x0041D624 File Offset: 0x0041B824
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

	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x0600B02E RID: 45102 RVA: 0x0041D69D File Offset: 0x0041B89D
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

	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x0600B02F RID: 45103 RVA: 0x0041D6C4 File Offset: 0x0041B8C4
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

	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x0600B030 RID: 45104 RVA: 0x0041D6EB File Offset: 0x0041B8EB
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

	// Token: 0x0600B031 RID: 45105 RVA: 0x0041D708 File Offset: 0x0041B908
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

	// Token: 0x0600B032 RID: 45106 RVA: 0x0041D791 File Offset: 0x0041B991
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.Player == null || this.WeaponHandler == null)
		{
			Debug.LogError(this.m_MissingHandlerError);
		}
	}

	// Token: 0x0600B033 RID: 45107 RVA: 0x0041D7C0 File Offset: 0x0041B9C0
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

	// Token: 0x0600B034 RID: 45108 RVA: 0x0041D7E8 File Offset: 0x0041B9E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		base.OnDisable();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B035 RID: 45109 RVA: 0x0041D80C File Offset: 0x0041BA0C
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

	// Token: 0x0600B036 RID: 45110 RVA: 0x0041D888 File Offset: 0x0041BA88
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddItem(vp_ItemType type, int id)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(type, -1) : this.HaveItem(type, id));
		base.DoAddItem(type, id);
		this.TryWieldNewItem(type, flag);
	}

	// Token: 0x0600B037 RID: 45111 RVA: 0x0041D8BF File Offset: 0x0041BABF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveItem(vp_ItemInstance item)
	{
		this.Unwield(item);
		base.DoRemoveItem(item);
	}

	// Token: 0x0600B038 RID: 45112 RVA: 0x0041D8D0 File Offset: 0x0041BAD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoAddUnitBank(vp_UnitBankType unitBankType, int id, int unitsLoaded)
	{
		bool flag = (vp_Gameplay.isMultiplayer ? this.HaveItem(unitBankType, -1) : this.HaveItem(unitBankType, id));
		base.DoAddUnitBank(unitBankType, id, unitsLoaded);
		this.TryWieldNewItem(unitBankType, flag);
	}

	// Token: 0x0600B039 RID: 45113 RVA: 0x0041D908 File Offset: 0x0041BB08
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

	// Token: 0x0600B03A RID: 45114 RVA: 0x0041DA85 File Offset: 0x0041BC85
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void DoRemoveUnitBank(vp_UnitBankInstance bank)
	{
		this.Unwield(bank);
		base.DoRemoveUnitBank(bank);
	}

	// Token: 0x0600B03B RID: 45115 RVA: 0x0041DA98 File Offset: 0x0041BC98
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

	// Token: 0x0600B03C RID: 45116 RVA: 0x0041DBB0 File Offset: 0x0041BDB0
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

	// Token: 0x0600B03D RID: 45117 RVA: 0x0041DC60 File Offset: 0x0041BE60
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

	// Token: 0x0600B03E RID: 45118 RVA: 0x0041DC89 File Offset: 0x0041BE89
	public vp_UnitBankInstance GetUnitBankInstanceOfWeapon(vp_Weapon weapon)
	{
		return this.GetItemInstanceOfWeapon(weapon) as vp_UnitBankInstance;
	}

	// Token: 0x0600B03F RID: 45119 RVA: 0x0041DC98 File Offset: 0x0041BE98
	public vp_ItemInstance GetItemInstanceOfWeapon(vp_Weapon weapon)
	{
		vp_ItemIdentifier weaponIdentifier = this.GetWeaponIdentifier(weapon);
		if (weaponIdentifier == null)
		{
			return null;
		}
		return this.GetItem(weaponIdentifier.Type);
	}

	// Token: 0x0600B040 RID: 45120 RVA: 0x0041DCC4 File Offset: 0x0041BEC4
	public int GetAmmoInWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return unitBankInstanceOfWeapon.Count;
	}

	// Token: 0x0600B041 RID: 45121 RVA: 0x0041DCE4 File Offset: 0x0041BEE4
	public int GetExtraAmmoForWeapon(vp_Weapon weapon)
	{
		vp_UnitBankInstance unitBankInstanceOfWeapon = this.GetUnitBankInstanceOfWeapon(weapon);
		if (unitBankInstanceOfWeapon == null)
		{
			return 0;
		}
		return this.GetUnitCount(unitBankInstanceOfWeapon.UnitType);
	}

	// Token: 0x0600B042 RID: 45122 RVA: 0x0041DD0A File Offset: 0x0041BF0A
	public int GetAmmoInCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponAmmoCount;
	}

	// Token: 0x0600B043 RID: 45123 RVA: 0x0041DD12 File Offset: 0x0041BF12
	public int GetExtraAmmoForCurrentWeapon()
	{
		return this.OnValue_CurrentWeaponClipCount;
	}

	// Token: 0x0600B044 RID: 45124 RVA: 0x0041DD1C File Offset: 0x0041BF1C
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

	// Token: 0x0600B045 RID: 45125 RVA: 0x0041DDA0 File Offset: 0x0041BFA0
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

	// Token: 0x0600B046 RID: 45126 RVA: 0x0041DE30 File Offset: 0x0041C030
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

	// Token: 0x0600B047 RID: 45127 RVA: 0x0041DEE4 File Offset: 0x0041C0E4
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

	// Token: 0x0600B048 RID: 45128 RVA: 0x0041DFAE File Offset: 0x0041C1AE
	public override void Refresh()
	{
		base.Refresh();
		this.UnwieldMissingWeapon();
	}

	// Token: 0x0600B049 RID: 45129 RVA: 0x0041DFBC File Offset: 0x0041C1BC
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

	// Token: 0x0600B04A RID: 45130 RVA: 0x0041E104 File Offset: 0x0041C304
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

	// Token: 0x0600B04B RID: 45131 RVA: 0x0041E16E File Offset: 0x0041C36E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RefillCurrentWeapon()
	{
		if (this.CurrentWeaponIdentifier == null)
		{
			return this.MissingIdentifierError(0);
		}
		return this.TryReload(this.CurrentWeaponIdentifier.Type as vp_UnitBankType, this.CurrentWeaponIdentifier.ID);
	}

	// Token: 0x0600B04C RID: 45132 RVA: 0x0041E1A7 File Offset: 0x0041C3A7
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

	// Token: 0x1700147A RID: 5242
	// (get) Token: 0x0600B04D RID: 45133 RVA: 0x0041E1D0 File Offset: 0x0041C3D0
	// (set) Token: 0x0600B04E RID: 45134 RVA: 0x0041E1F4 File Offset: 0x0041C3F4
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

	// Token: 0x1700147B RID: 5243
	// (get) Token: 0x0600B04F RID: 45135 RVA: 0x0041E21C File Offset: 0x0041C41C
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

	// Token: 0x1700147C RID: 5244
	// (get) Token: 0x0600B050 RID: 45136 RVA: 0x0041E240 File Offset: 0x0041C440
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

	// Token: 0x0600B051 RID: 45137 RVA: 0x0041E26C File Offset: 0x0041C46C
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

	// Token: 0x0600B052 RID: 45138 RVA: 0x0041E2B4 File Offset: 0x0041C4B4
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

	// Token: 0x0600B053 RID: 45139 RVA: 0x0041E30C File Offset: 0x0041C50C
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

	// Token: 0x1700147D RID: 5245
	// (get) Token: 0x0600B054 RID: 45140 RVA: 0x0041E364 File Offset: 0x0041C564
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

	// Token: 0x0600B055 RID: 45141 RVA: 0x0041E3C7 File Offset: 0x0041C5C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_CurrentWeaponInstance = null;
	}

	// Token: 0x0400845E RID: 33886
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, object> m_PreviouslyOwnedItems = new Dictionary<vp_ItemType, object>();

	// Token: 0x0400845F RID: 33887
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_WeaponIdentifierResult;

	// Token: 0x04008460 RID: 33888
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string m_MissingHandlerError = "Error (vp_PlayerInventory) this component must be on the same transform as a vp_PlayerEventHandler + vp_WeaponHandler.";

	// Token: 0x04008461 RID: 33889
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitBankInstance, vp_Weapon> m_UnitBankWeapons;

	// Token: 0x04008462 RID: 33890
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemInstance, vp_Weapon> m_ItemWeapons;

	// Token: 0x04008463 RID: 33891
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_Weapon, vp_ItemIdentifier> m_WeaponIdentifiers;

	// Token: 0x04008464 RID: 33892
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_UnitType, List<vp_Weapon>> m_WeaponsByUnit;

	// Token: 0x04008465 RID: 33893
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_CurrentWeaponInstance;

	// Token: 0x04008466 RID: 33894
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008467 RID: 33895
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008468 RID: 33896
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_PlayerInventory.AutoWieldSection m_AutoWield;

	// Token: 0x04008469 RID: 33897
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_PlayerInventory.MiscSection m_Misc;

	// Token: 0x020015F0 RID: 5616
	[Serializable]
	public class AutoWieldSection
	{
		// Token: 0x0400846A RID: 33898
		public bool Always;

		// Token: 0x0400846B RID: 33899
		public bool IfUnarmed = true;

		// Token: 0x0400846C RID: 33900
		public bool IfOutOfAmmo = true;

		// Token: 0x0400846D RID: 33901
		public bool IfNotPresent = true;

		// Token: 0x0400846E RID: 33902
		public bool FirstTimeOnly = true;
	}

	// Token: 0x020015F1 RID: 5617
	[Serializable]
	public class MiscSection
	{
		// Token: 0x0400846F RID: 33903
		public bool ResetOnRespawn = true;
	}
}
