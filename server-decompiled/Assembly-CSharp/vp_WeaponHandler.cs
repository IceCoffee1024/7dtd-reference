using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001644 RID: 5700
public class vp_WeaponHandler : MonoBehaviour
{
	// Token: 0x17001514 RID: 5396
	// (get) Token: 0x0600B27C RID: 45692 RVA: 0x0042A651 File Offset: 0x00428851
	// (set) Token: 0x0600B27D RID: 45693 RVA: 0x0042A667 File Offset: 0x00428867
	public List<vp_Weapon> Weapons
	{
		get
		{
			if (this.m_Weapons == null)
			{
				this.InitWeaponLists();
			}
			return this.m_Weapons;
		}
		set
		{
			this.m_Weapons = value;
		}
	}

	// Token: 0x17001515 RID: 5397
	// (get) Token: 0x0600B27E RID: 45694 RVA: 0x0042A670 File Offset: 0x00428870
	public vp_Weapon CurrentWeapon
	{
		get
		{
			return this.m_CurrentWeapon;
		}
	}

	// Token: 0x17001516 RID: 5398
	// (get) Token: 0x0600B27F RID: 45695 RVA: 0x0042A678 File Offset: 0x00428878
	[Obsolete("Please use the 'CurrentWeaponIndex' parameter instead.")]
	public int CurrentWeaponID
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x17001517 RID: 5399
	// (get) Token: 0x0600B280 RID: 45696 RVA: 0x0042A678 File Offset: 0x00428878
	public int CurrentWeaponIndex
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x0600B281 RID: 45697 RVA: 0x0042A680 File Offset: 0x00428880
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
		if (this.Weapons != null)
		{
			this.StartWeapon = Mathf.Clamp(this.StartWeapon, 0, this.Weapons.Count);
		}
	}

	// Token: 0x0600B282 RID: 45698 RVA: 0x0042A6D8 File Offset: 0x004288D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void InitWeaponLists()
	{
		List<vp_Weapon> list = null;
		vp_FPCamera componentInChildren = base.transform.GetComponentInChildren<vp_FPCamera>();
		if (componentInChildren != null)
		{
			list = this.GetWeaponList(Camera.main.transform);
			if (list != null && list.Count > 0)
			{
				this.m_WeaponLists.Add(list);
			}
		}
		List<vp_Weapon> list2 = new List<vp_Weapon>(base.transform.GetComponentsInChildren<vp_Weapon>());
		if (list != null && list.Count == list2.Count)
		{
			this.Weapons = this.m_WeaponLists[0];
			return;
		}
		List<Transform> list3 = new List<Transform>();
		foreach (vp_Weapon vp_Weapon in list2)
		{
			if ((!(componentInChildren != null) || !list.Contains(vp_Weapon)) && !list3.Contains(vp_Weapon.Parent))
			{
				list3.Add(vp_Weapon.Parent);
			}
		}
		foreach (Transform transform in list3)
		{
			List<vp_Weapon> weaponList = this.GetWeaponList(transform);
			this.DeactivateAll(weaponList);
			this.m_WeaponLists.Add(weaponList);
		}
		if (this.m_WeaponLists.Count < 1)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") WeaponHandler found no weapons in its hierarchy. Disabling self.");
			base.enabled = false;
			return;
		}
		this.Weapons = this.m_WeaponLists[0];
	}

	// Token: 0x0600B283 RID: 45699 RVA: 0x0042A86C File Offset: 0x00428A6C
	public void EnableWeaponList(int index)
	{
		if (this.m_WeaponLists == null)
		{
			return;
		}
		if (this.m_WeaponLists.Count < 1)
		{
			return;
		}
		if (index < 0 || index > this.m_WeaponLists.Count - 1)
		{
			return;
		}
		this.Weapons = this.m_WeaponLists[index];
	}

	// Token: 0x0600B284 RID: 45700 RVA: 0x0042A8B8 File Offset: 0x00428AB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_Weapon> GetWeaponList(Transform target)
	{
		List<vp_Weapon> list = new List<vp_Weapon>();
		if (target.GetComponent<vp_Weapon>())
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") Hierarchy error. This component should sit above any vp_Weapons in the gameobject hierarchy.");
			return list;
		}
		foreach (vp_Weapon vp_Weapon in target.GetComponentsInChildren<vp_Weapon>(true))
		{
			list.Insert(list.Count, vp_Weapon);
		}
		if (list.Count == 0)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") Hierarchy error. This component must be added to a gameobject with vp_Weapon components in child gameobjects.");
			return list;
		}
		IComparer comparer = new vp_WeaponHandler.WeaponComparer();
		list.Sort(new Comparison<vp_Weapon>(comparer.Compare));
		return list;
	}

	// Token: 0x0600B285 RID: 45701 RVA: 0x0042A969 File Offset: 0x00428B69
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B286 RID: 45702 RVA: 0x0042A985 File Offset: 0x00428B85
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B287 RID: 45703 RVA: 0x0042A9A1 File Offset: 0x00428BA1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.InitWeapon();
		this.UpdateFiring();
	}

	// Token: 0x0600B288 RID: 45704 RVA: 0x0042A9B0 File Offset: 0x00428BB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateFiring()
	{
		if (!this.m_Player.IsLocal.Get() && !this.m_Player.IsAI.Get())
		{
			return;
		}
		if (!this.m_Player.Attack.Active)
		{
			return;
		}
		if (this.m_Player.SetWeapon.Active || !this.m_CurrentWeapon.Wielded)
		{
			return;
		}
		this.m_Player.Fire.Try();
	}

	// Token: 0x0600B289 RID: 45705 RVA: 0x0042AA38 File Offset: 0x00428C38
	public virtual void SetWeapon(int weaponIndex)
	{
		if (this.Weapons == null || this.Weapons.Count < 1)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") Tried to set weapon with an empty weapon list.");
			return;
		}
		if (weaponIndex < 0 || weaponIndex > this.Weapons.Count)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") Weapon list does not have a weapon with index: " + weaponIndex.ToString());
			return;
		}
		if (this.m_CurrentWeapon != null)
		{
			this.m_CurrentWeapon.ResetState();
		}
		this.DeactivateAll(this.Weapons);
		this.ActivateWeapon(weaponIndex);
	}

	// Token: 0x0600B28A RID: 45706 RVA: 0x0042AAE4 File Offset: 0x00428CE4
	public void DeactivateAll(List<vp_Weapon> weaponList)
	{
		foreach (vp_Weapon vp_Weapon in weaponList)
		{
			vp_Weapon.ActivateGameObject(false);
			vp_FPWeapon vp_FPWeapon = vp_Weapon as vp_FPWeapon;
			if (vp_FPWeapon != null && vp_FPWeapon.Weapon3rdPersonModel != null)
			{
				vp_Utility.Activate(vp_FPWeapon.Weapon3rdPersonModel, false);
			}
		}
	}

	// Token: 0x0600B28B RID: 45707 RVA: 0x0042AB5C File Offset: 0x00428D5C
	public void ActivateWeapon(int index)
	{
		this.m_CurrentWeaponIndex = index;
		this.m_CurrentWeapon = null;
		if (this.m_CurrentWeaponIndex > 0)
		{
			this.m_CurrentWeapon = this.Weapons[this.m_CurrentWeaponIndex - 1];
			if (this.m_CurrentWeapon != null)
			{
				this.m_CurrentWeapon.ActivateGameObject(true);
			}
		}
	}

	// Token: 0x0600B28C RID: 45708 RVA: 0x0042ABB3 File Offset: 0x00428DB3
	public virtual void CancelTimers()
	{
		vp_Timer.CancelAll("EjectShell");
		this.m_DisableAttackStateTimer.Cancel();
		this.m_SetWeaponTimer.Cancel();
		this.m_SetWeaponRefreshTimer.Cancel();
	}

	// Token: 0x0600B28D RID: 45709 RVA: 0x0042ABE0 File Offset: 0x00428DE0
	public virtual void SetWeaponLayer(int layer)
	{
		if (this.m_CurrentWeaponIndex < 1 || this.m_CurrentWeaponIndex > this.Weapons.Count)
		{
			return;
		}
		vp_Layer.Set(this.Weapons[this.m_CurrentWeaponIndex - 1].gameObject, layer, true);
	}

	// Token: 0x0600B28E RID: 45710 RVA: 0x0042AC1E File Offset: 0x00428E1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitWeapon()
	{
		if (this.m_CurrentWeaponIndex == -1)
		{
			this.SetWeapon(0);
			vp_Timer.In(this.SetWeaponDuration + 0.1f, delegate
			{
				if (this.StartWeapon > 0 && this.StartWeapon < this.Weapons.Count + 1 && !this.m_Player.SetWeapon.TryStart<int>(this.StartWeapon))
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"Warning (",
						(this != null) ? this.ToString() : null,
						") Requested 'StartWeapon' (",
						this.Weapons[this.StartWeapon - 1].name,
						") was denied, likely by the inventory. Make sure it's present in the inventory from the beginning."
					}));
				}
			}, null);
		}
	}

	// Token: 0x0600B28F RID: 45711 RVA: 0x0042AC50 File Offset: 0x00428E50
	public void RefreshAllWeapons()
	{
		foreach (vp_Weapon vp_Weapon in this.Weapons)
		{
			vp_Weapon.Refresh();
			vp_Weapon.RefreshWeaponModel();
		}
	}

	// Token: 0x0600B290 RID: 45712 RVA: 0x0042ACA8 File Offset: 0x00428EA8
	public int GetWeaponIndex(vp_Weapon weapon)
	{
		return this.Weapons.IndexOf(weapon) + 1;
	}

	// Token: 0x0600B291 RID: 45713 RVA: 0x0042ACB8 File Offset: 0x00428EB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.m_Player.Attack.Stop(this.m_Player.CurrentWeaponReloadDuration.Get() + this.ReloadAttackSleepDuration);
	}

	// Token: 0x0600B292 RID: 45714 RVA: 0x0042ACE8 File Offset: 0x00428EE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_SetWeapon()
	{
		this.CancelTimers();
		this.m_Player.Reload.Stop(this.SetWeaponDuration + this.SetWeaponReloadSleepDuration);
		this.m_Player.Zoom.Stop(this.SetWeaponDuration + this.SetWeaponZoomSleepDuration);
		this.m_Player.Attack.Stop(this.SetWeaponDuration + this.SetWeaponAttackSleepDuration);
		if (this.m_CurrentWeapon != null)
		{
			this.m_CurrentWeapon.Wield(false);
		}
		this.m_Player.SetWeapon.AutoDuration = this.SetWeaponDuration;
	}

	// Token: 0x0600B293 RID: 45715 RVA: 0x0042AD84 File Offset: 0x00428F84
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		int num = 0;
		if (this.m_Player.SetWeapon.Argument != null)
		{
			num = (int)this.m_Player.SetWeapon.Argument;
		}
		this.SetWeapon(num);
		if (this.m_CurrentWeapon != null)
		{
			this.m_CurrentWeapon.Wield(true);
		}
		vp_Timer.In(this.SetWeaponRefreshStatesDelay, delegate
		{
			this.m_Player.RefreshActivityStates();
			if (this.m_CurrentWeapon != null && this.m_Player.CurrentWeaponAmmoCount.Get() == 0)
			{
				this.m_Player.AutoReload.Try();
			}
		}, this.m_SetWeaponRefreshTimer);
	}

	// Token: 0x0600B294 RID: 45716 RVA: 0x0042ADFC File Offset: 0x00428FFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num != this.m_CurrentWeaponIndex && num >= 0 && num <= this.Weapons.Count && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B295 RID: 45717 RVA: 0x0042AE54 File Offset: 0x00429054
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Attack()
	{
		return !(this.m_CurrentWeapon == null) && !this.m_Player.Attack.Active && !this.m_Player.SetWeapon.Active && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B296 RID: 45718 RVA: 0x0042AEAE File Offset: 0x004290AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		vp_Timer.In(this.AttackStateDisableDelay, delegate
		{
			if (!this.m_Player.Attack.Active && this.m_CurrentWeapon != null)
			{
				this.m_CurrentWeapon.SetState("Attack", false, false, false);
			}
		}, this.m_DisableAttackStateTimer);
	}

	// Token: 0x0600B297 RID: 45719 RVA: 0x0042AED0 File Offset: 0x004290D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_SetPrevWeapon()
	{
		int num = this.m_CurrentWeaponIndex - 1;
		if (num < 1)
		{
			num = this.Weapons.Count;
		}
		int num2 = 0;
		while (!this.m_Player.SetWeapon.TryStart<int>(num))
		{
			num--;
			if (num < 1)
			{
				num = this.Weapons.Count;
			}
			num2++;
			if (num2 > this.Weapons.Count)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600B298 RID: 45720 RVA: 0x0042AF38 File Offset: 0x00429138
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_SetNextWeapon()
	{
		int num = this.m_CurrentWeaponIndex + 1;
		int num2 = 0;
		while (!this.m_Player.SetWeapon.TryStart<int>(num))
		{
			if (num > this.Weapons.Count + 1)
			{
				num = 0;
			}
			num++;
			num2++;
			if (num2 > this.Weapons.Count)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600B299 RID: 45721 RVA: 0x0042AF90 File Offset: 0x00429190
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_SetWeaponByName(string name)
	{
		for (int i = 0; i < this.Weapons.Count; i++)
		{
			if (this.Weapons[i].name == name)
			{
				return this.m_Player.SetWeapon.TryStart<int>(i + 1);
			}
		}
		return false;
	}

	// Token: 0x17001518 RID: 5400
	// (get) Token: 0x0600B29A RID: 45722 RVA: 0x0042AFE1 File Offset: 0x004291E1
	public virtual bool OnValue_CurrentWeaponWielded
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !(this.m_CurrentWeapon == null) && this.m_CurrentWeapon.Wielded;
		}
	}

	// Token: 0x17001519 RID: 5401
	// (get) Token: 0x0600B29B RID: 45723 RVA: 0x0042AFFE File Offset: 0x004291FE
	public virtual string OnValue_CurrentWeaponName
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CurrentWeapon == null || this.Weapons == null)
			{
				return "";
			}
			return this.m_CurrentWeapon.name;
		}
	}

	// Token: 0x1700151A RID: 5402
	// (get) Token: 0x0600B29C RID: 45724 RVA: 0x0042A678 File Offset: 0x00428878
	public virtual int OnValue_CurrentWeaponID
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x1700151B RID: 5403
	// (get) Token: 0x0600B29D RID: 45725 RVA: 0x0042A678 File Offset: 0x00428878
	public virtual int OnValue_CurrentWeaponIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x1700151C RID: 5404
	// (get) Token: 0x0600B29E RID: 45726 RVA: 0x0042B027 File Offset: 0x00429227
	public virtual int OnValue_CurrentWeaponType
	{
		get
		{
			if (!(this.CurrentWeapon == null))
			{
				return this.CurrentWeapon.AnimationType;
			}
			return 0;
		}
	}

	// Token: 0x1700151D RID: 5405
	// (get) Token: 0x0600B29F RID: 45727 RVA: 0x0042B044 File Offset: 0x00429244
	public virtual int OnValue_CurrentWeaponGrip
	{
		get
		{
			if (!(this.CurrentWeapon == null))
			{
				return this.CurrentWeapon.AnimationGrip;
			}
			return 0;
		}
	}

	// Token: 0x0400860F RID: 34319
	public int StartWeapon;

	// Token: 0x04008610 RID: 34320
	public float AttackStateDisableDelay = 0.5f;

	// Token: 0x04008611 RID: 34321
	public float SetWeaponRefreshStatesDelay = 0.5f;

	// Token: 0x04008612 RID: 34322
	public float SetWeaponDuration = 0.1f;

	// Token: 0x04008613 RID: 34323
	public float SetWeaponReloadSleepDuration = 0.3f;

	// Token: 0x04008614 RID: 34324
	public float SetWeaponZoomSleepDuration = 0.3f;

	// Token: 0x04008615 RID: 34325
	public float SetWeaponAttackSleepDuration = 0.3f;

	// Token: 0x04008616 RID: 34326
	public float ReloadAttackSleepDuration = 0.3f;

	// Token: 0x04008617 RID: 34327
	public bool ReloadAutomatically = true;

	// Token: 0x04008618 RID: 34328
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008619 RID: 34329
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Weapon> m_Weapons;

	// Token: 0x0400861A RID: 34330
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<List<vp_Weapon>> m_WeaponLists = new List<List<vp_Weapon>>();

	// Token: 0x0400861B RID: 34331
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWeaponIndex = -1;

	// Token: 0x0400861C RID: 34332
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_CurrentWeapon;

	// Token: 0x0400861D RID: 34333
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponTimer = new vp_Timer.Handle();

	// Token: 0x0400861E RID: 34334
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponRefreshTimer = new vp_Timer.Handle();

	// Token: 0x0400861F RID: 34335
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableAttackStateTimer = new vp_Timer.Handle();

	// Token: 0x04008620 RID: 34336
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableReloadStateTimer = new vp_Timer.Handle();

	// Token: 0x02001645 RID: 5701
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WeaponComparer : IComparer
	{
		// Token: 0x0600B2A4 RID: 45732 RVA: 0x0042B230 File Offset: 0x00429430
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_Weapon)x).gameObject.name, ((vp_Weapon)y).gameObject.name);
		}
	}
}
