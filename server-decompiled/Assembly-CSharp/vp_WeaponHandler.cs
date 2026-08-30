using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001613 RID: 5651
public class vp_WeaponHandler : MonoBehaviour
{
	// Token: 0x170014C5 RID: 5317
	// (get) Token: 0x0600B192 RID: 45458 RVA: 0x00424571 File Offset: 0x00422771
	// (set) Token: 0x0600B193 RID: 45459 RVA: 0x00424587 File Offset: 0x00422787
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

	// Token: 0x170014C6 RID: 5318
	// (get) Token: 0x0600B194 RID: 45460 RVA: 0x00424590 File Offset: 0x00422790
	public vp_Weapon CurrentWeapon
	{
		get
		{
			return this.m_CurrentWeapon;
		}
	}

	// Token: 0x170014C7 RID: 5319
	// (get) Token: 0x0600B195 RID: 45461 RVA: 0x00424598 File Offset: 0x00422798
	[Obsolete("Please use the 'CurrentWeaponIndex' parameter instead.")]
	public int CurrentWeaponID
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x170014C8 RID: 5320
	// (get) Token: 0x0600B196 RID: 45462 RVA: 0x00424598 File Offset: 0x00422798
	public int CurrentWeaponIndex
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x0600B197 RID: 45463 RVA: 0x004245A0 File Offset: 0x004227A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
		if (this.Weapons != null)
		{
			this.StartWeapon = Mathf.Clamp(this.StartWeapon, 0, this.Weapons.Count);
		}
	}

	// Token: 0x0600B198 RID: 45464 RVA: 0x004245F8 File Offset: 0x004227F8
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

	// Token: 0x0600B199 RID: 45465 RVA: 0x0042478C File Offset: 0x0042298C
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

	// Token: 0x0600B19A RID: 45466 RVA: 0x004247D8 File Offset: 0x004229D8
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

	// Token: 0x0600B19B RID: 45467 RVA: 0x00424889 File Offset: 0x00422A89
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B19C RID: 45468 RVA: 0x004248A5 File Offset: 0x00422AA5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B19D RID: 45469 RVA: 0x004248C1 File Offset: 0x00422AC1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.InitWeapon();
		this.UpdateFiring();
	}

	// Token: 0x0600B19E RID: 45470 RVA: 0x004248D0 File Offset: 0x00422AD0
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

	// Token: 0x0600B19F RID: 45471 RVA: 0x00424958 File Offset: 0x00422B58
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

	// Token: 0x0600B1A0 RID: 45472 RVA: 0x00424A04 File Offset: 0x00422C04
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

	// Token: 0x0600B1A1 RID: 45473 RVA: 0x00424A7C File Offset: 0x00422C7C
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

	// Token: 0x0600B1A2 RID: 45474 RVA: 0x00424AD3 File Offset: 0x00422CD3
	public virtual void CancelTimers()
	{
		vp_Timer.CancelAll("EjectShell");
		this.m_DisableAttackStateTimer.Cancel();
		this.m_SetWeaponTimer.Cancel();
		this.m_SetWeaponRefreshTimer.Cancel();
	}

	// Token: 0x0600B1A3 RID: 45475 RVA: 0x00424B00 File Offset: 0x00422D00
	public virtual void SetWeaponLayer(int layer)
	{
		if (this.m_CurrentWeaponIndex < 1 || this.m_CurrentWeaponIndex > this.Weapons.Count)
		{
			return;
		}
		vp_Layer.Set(this.Weapons[this.m_CurrentWeaponIndex - 1].gameObject, layer, true);
	}

	// Token: 0x0600B1A4 RID: 45476 RVA: 0x00424B3E File Offset: 0x00422D3E
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

	// Token: 0x0600B1A5 RID: 45477 RVA: 0x00424B70 File Offset: 0x00422D70
	public void RefreshAllWeapons()
	{
		foreach (vp_Weapon vp_Weapon in this.Weapons)
		{
			vp_Weapon.Refresh();
			vp_Weapon.RefreshWeaponModel();
		}
	}

	// Token: 0x0600B1A6 RID: 45478 RVA: 0x00424BC8 File Offset: 0x00422DC8
	public int GetWeaponIndex(vp_Weapon weapon)
	{
		return this.Weapons.IndexOf(weapon) + 1;
	}

	// Token: 0x0600B1A7 RID: 45479 RVA: 0x00424BD8 File Offset: 0x00422DD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.m_Player.Attack.Stop(this.m_Player.CurrentWeaponReloadDuration.Get() + this.ReloadAttackSleepDuration);
	}

	// Token: 0x0600B1A8 RID: 45480 RVA: 0x00424C08 File Offset: 0x00422E08
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

	// Token: 0x0600B1A9 RID: 45481 RVA: 0x00424CA4 File Offset: 0x00422EA4
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

	// Token: 0x0600B1AA RID: 45482 RVA: 0x00424D1C File Offset: 0x00422F1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num != this.m_CurrentWeaponIndex && num >= 0 && num <= this.Weapons.Count && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B1AB RID: 45483 RVA: 0x00424D74 File Offset: 0x00422F74
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Attack()
	{
		return !(this.m_CurrentWeapon == null) && !this.m_Player.Attack.Active && !this.m_Player.SetWeapon.Active && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B1AC RID: 45484 RVA: 0x00424DCE File Offset: 0x00422FCE
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

	// Token: 0x0600B1AD RID: 45485 RVA: 0x00424DF0 File Offset: 0x00422FF0
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

	// Token: 0x0600B1AE RID: 45486 RVA: 0x00424E58 File Offset: 0x00423058
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

	// Token: 0x0600B1AF RID: 45487 RVA: 0x00424EB0 File Offset: 0x004230B0
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

	// Token: 0x170014C9 RID: 5321
	// (get) Token: 0x0600B1B0 RID: 45488 RVA: 0x00424F01 File Offset: 0x00423101
	public virtual bool OnValue_CurrentWeaponWielded
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !(this.m_CurrentWeapon == null) && this.m_CurrentWeapon.Wielded;
		}
	}

	// Token: 0x170014CA RID: 5322
	// (get) Token: 0x0600B1B1 RID: 45489 RVA: 0x00424F1E File Offset: 0x0042311E
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

	// Token: 0x170014CB RID: 5323
	// (get) Token: 0x0600B1B2 RID: 45490 RVA: 0x00424598 File Offset: 0x00422798
	public virtual int OnValue_CurrentWeaponID
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x170014CC RID: 5324
	// (get) Token: 0x0600B1B3 RID: 45491 RVA: 0x00424598 File Offset: 0x00422798
	public virtual int OnValue_CurrentWeaponIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x170014CD RID: 5325
	// (get) Token: 0x0600B1B4 RID: 45492 RVA: 0x00424F47 File Offset: 0x00423147
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

	// Token: 0x170014CE RID: 5326
	// (get) Token: 0x0600B1B5 RID: 45493 RVA: 0x00424F64 File Offset: 0x00423164
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

	// Token: 0x04008578 RID: 34168
	public int StartWeapon;

	// Token: 0x04008579 RID: 34169
	public float AttackStateDisableDelay = 0.5f;

	// Token: 0x0400857A RID: 34170
	public float SetWeaponRefreshStatesDelay = 0.5f;

	// Token: 0x0400857B RID: 34171
	public float SetWeaponDuration = 0.1f;

	// Token: 0x0400857C RID: 34172
	public float SetWeaponReloadSleepDuration = 0.3f;

	// Token: 0x0400857D RID: 34173
	public float SetWeaponZoomSleepDuration = 0.3f;

	// Token: 0x0400857E RID: 34174
	public float SetWeaponAttackSleepDuration = 0.3f;

	// Token: 0x0400857F RID: 34175
	public float ReloadAttackSleepDuration = 0.3f;

	// Token: 0x04008580 RID: 34176
	public bool ReloadAutomatically = true;

	// Token: 0x04008581 RID: 34177
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008582 RID: 34178
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Weapon> m_Weapons;

	// Token: 0x04008583 RID: 34179
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<List<vp_Weapon>> m_WeaponLists = new List<List<vp_Weapon>>();

	// Token: 0x04008584 RID: 34180
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWeaponIndex = -1;

	// Token: 0x04008585 RID: 34181
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_CurrentWeapon;

	// Token: 0x04008586 RID: 34182
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponTimer = new vp_Timer.Handle();

	// Token: 0x04008587 RID: 34183
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponRefreshTimer = new vp_Timer.Handle();

	// Token: 0x04008588 RID: 34184
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableAttackStateTimer = new vp_Timer.Handle();

	// Token: 0x04008589 RID: 34185
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableReloadStateTimer = new vp_Timer.Handle();

	// Token: 0x02001614 RID: 5652
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WeaponComparer : IComparer
	{
		// Token: 0x0600B1BA RID: 45498 RVA: 0x00425150 File Offset: 0x00423350
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_Weapon)x).gameObject.name, ((vp_Weapon)y).gameObject.name);
		}
	}
}
