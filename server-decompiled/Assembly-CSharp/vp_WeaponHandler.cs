using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015F8 RID: 5624
public class vp_WeaponHandler : MonoBehaviour
{
	// Token: 0x1700148C RID: 5260
	// (get) Token: 0x0600B095 RID: 45205 RVA: 0x0041F231 File Offset: 0x0041D431
	// (set) Token: 0x0600B096 RID: 45206 RVA: 0x0041F247 File Offset: 0x0041D447
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

	// Token: 0x1700148D RID: 5261
	// (get) Token: 0x0600B097 RID: 45207 RVA: 0x0041F250 File Offset: 0x0041D450
	public vp_Weapon CurrentWeapon
	{
		get
		{
			return this.m_CurrentWeapon;
		}
	}

	// Token: 0x1700148E RID: 5262
	// (get) Token: 0x0600B098 RID: 45208 RVA: 0x0041F258 File Offset: 0x0041D458
	[Obsolete("Please use the 'CurrentWeaponIndex' parameter instead.")]
	public int CurrentWeaponID
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x1700148F RID: 5263
	// (get) Token: 0x0600B099 RID: 45209 RVA: 0x0041F258 File Offset: 0x0041D458
	public int CurrentWeaponIndex
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x0600B09A RID: 45210 RVA: 0x0041F260 File Offset: 0x0041D460
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
		if (this.Weapons != null)
		{
			this.StartWeapon = Mathf.Clamp(this.StartWeapon, 0, this.Weapons.Count);
		}
	}

	// Token: 0x0600B09B RID: 45211 RVA: 0x0041F2B8 File Offset: 0x0041D4B8
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

	// Token: 0x0600B09C RID: 45212 RVA: 0x0041F44C File Offset: 0x0041D64C
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

	// Token: 0x0600B09D RID: 45213 RVA: 0x0041F498 File Offset: 0x0041D698
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

	// Token: 0x0600B09E RID: 45214 RVA: 0x0041F549 File Offset: 0x0041D749
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B09F RID: 45215 RVA: 0x0041F565 File Offset: 0x0041D765
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B0A0 RID: 45216 RVA: 0x0041F581 File Offset: 0x0041D781
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.InitWeapon();
		this.UpdateFiring();
	}

	// Token: 0x0600B0A1 RID: 45217 RVA: 0x0041F590 File Offset: 0x0041D790
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

	// Token: 0x0600B0A2 RID: 45218 RVA: 0x0041F618 File Offset: 0x0041D818
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

	// Token: 0x0600B0A3 RID: 45219 RVA: 0x0041F6C4 File Offset: 0x0041D8C4
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

	// Token: 0x0600B0A4 RID: 45220 RVA: 0x0041F73C File Offset: 0x0041D93C
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

	// Token: 0x0600B0A5 RID: 45221 RVA: 0x0041F793 File Offset: 0x0041D993
	public virtual void CancelTimers()
	{
		vp_Timer.CancelAll("EjectShell");
		this.m_DisableAttackStateTimer.Cancel();
		this.m_SetWeaponTimer.Cancel();
		this.m_SetWeaponRefreshTimer.Cancel();
	}

	// Token: 0x0600B0A6 RID: 45222 RVA: 0x0041F7C0 File Offset: 0x0041D9C0
	public virtual void SetWeaponLayer(int layer)
	{
		if (this.m_CurrentWeaponIndex < 1 || this.m_CurrentWeaponIndex > this.Weapons.Count)
		{
			return;
		}
		vp_Layer.Set(this.Weapons[this.m_CurrentWeaponIndex - 1].gameObject, layer, true);
	}

	// Token: 0x0600B0A7 RID: 45223 RVA: 0x0041F7FE File Offset: 0x0041D9FE
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

	// Token: 0x0600B0A8 RID: 45224 RVA: 0x0041F830 File Offset: 0x0041DA30
	public void RefreshAllWeapons()
	{
		foreach (vp_Weapon vp_Weapon in this.Weapons)
		{
			vp_Weapon.Refresh();
			vp_Weapon.RefreshWeaponModel();
		}
	}

	// Token: 0x0600B0A9 RID: 45225 RVA: 0x0041F888 File Offset: 0x0041DA88
	public int GetWeaponIndex(vp_Weapon weapon)
	{
		return this.Weapons.IndexOf(weapon) + 1;
	}

	// Token: 0x0600B0AA RID: 45226 RVA: 0x0041F898 File Offset: 0x0041DA98
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.m_Player.Attack.Stop(this.m_Player.CurrentWeaponReloadDuration.Get() + this.ReloadAttackSleepDuration);
	}

	// Token: 0x0600B0AB RID: 45227 RVA: 0x0041F8C8 File Offset: 0x0041DAC8
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

	// Token: 0x0600B0AC RID: 45228 RVA: 0x0041F964 File Offset: 0x0041DB64
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

	// Token: 0x0600B0AD RID: 45229 RVA: 0x0041F9DC File Offset: 0x0041DBDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num != this.m_CurrentWeaponIndex && num >= 0 && num <= this.Weapons.Count && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B0AE RID: 45230 RVA: 0x0041FA34 File Offset: 0x0041DC34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Attack()
	{
		return !(this.m_CurrentWeapon == null) && !this.m_Player.Attack.Active && !this.m_Player.SetWeapon.Active && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B0AF RID: 45231 RVA: 0x0041FA8E File Offset: 0x0041DC8E
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

	// Token: 0x0600B0B0 RID: 45232 RVA: 0x0041FAB0 File Offset: 0x0041DCB0
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

	// Token: 0x0600B0B1 RID: 45233 RVA: 0x0041FB18 File Offset: 0x0041DD18
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

	// Token: 0x0600B0B2 RID: 45234 RVA: 0x0041FB70 File Offset: 0x0041DD70
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

	// Token: 0x17001490 RID: 5264
	// (get) Token: 0x0600B0B3 RID: 45235 RVA: 0x0041FBC1 File Offset: 0x0041DDC1
	public virtual bool OnValue_CurrentWeaponWielded
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !(this.m_CurrentWeapon == null) && this.m_CurrentWeapon.Wielded;
		}
	}

	// Token: 0x17001491 RID: 5265
	// (get) Token: 0x0600B0B4 RID: 45236 RVA: 0x0041FBDE File Offset: 0x0041DDDE
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

	// Token: 0x17001492 RID: 5266
	// (get) Token: 0x0600B0B5 RID: 45237 RVA: 0x0041F258 File Offset: 0x0041D458
	public virtual int OnValue_CurrentWeaponID
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x17001493 RID: 5267
	// (get) Token: 0x0600B0B6 RID: 45238 RVA: 0x0041F258 File Offset: 0x0041D458
	public virtual int OnValue_CurrentWeaponIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x17001494 RID: 5268
	// (get) Token: 0x0600B0B7 RID: 45239 RVA: 0x0041FC07 File Offset: 0x0041DE07
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

	// Token: 0x17001495 RID: 5269
	// (get) Token: 0x0600B0B8 RID: 45240 RVA: 0x0041FC24 File Offset: 0x0041DE24
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

	// Token: 0x0400849F RID: 33951
	public int StartWeapon;

	// Token: 0x040084A0 RID: 33952
	public float AttackStateDisableDelay = 0.5f;

	// Token: 0x040084A1 RID: 33953
	public float SetWeaponRefreshStatesDelay = 0.5f;

	// Token: 0x040084A2 RID: 33954
	public float SetWeaponDuration = 0.1f;

	// Token: 0x040084A3 RID: 33955
	public float SetWeaponReloadSleepDuration = 0.3f;

	// Token: 0x040084A4 RID: 33956
	public float SetWeaponZoomSleepDuration = 0.3f;

	// Token: 0x040084A5 RID: 33957
	public float SetWeaponAttackSleepDuration = 0.3f;

	// Token: 0x040084A6 RID: 33958
	public float ReloadAttackSleepDuration = 0.3f;

	// Token: 0x040084A7 RID: 33959
	public bool ReloadAutomatically = true;

	// Token: 0x040084A8 RID: 33960
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040084A9 RID: 33961
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Weapon> m_Weapons;

	// Token: 0x040084AA RID: 33962
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<List<vp_Weapon>> m_WeaponLists = new List<List<vp_Weapon>>();

	// Token: 0x040084AB RID: 33963
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWeaponIndex = -1;

	// Token: 0x040084AC RID: 33964
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_CurrentWeapon;

	// Token: 0x040084AD RID: 33965
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponTimer = new vp_Timer.Handle();

	// Token: 0x040084AE RID: 33966
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponRefreshTimer = new vp_Timer.Handle();

	// Token: 0x040084AF RID: 33967
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableAttackStateTimer = new vp_Timer.Handle();

	// Token: 0x040084B0 RID: 33968
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableReloadStateTimer = new vp_Timer.Handle();

	// Token: 0x020015F9 RID: 5625
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WeaponComparer : IComparer
	{
		// Token: 0x0600B0BD RID: 45245 RVA: 0x0041FE10 File Offset: 0x0041E010
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_Weapon)x).gameObject.name, ((vp_Weapon)y).gameObject.name);
		}
	}
}
