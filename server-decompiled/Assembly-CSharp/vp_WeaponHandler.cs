using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001632 RID: 5682
public class vp_WeaponHandler : MonoBehaviour
{
	// Token: 0x17001508 RID: 5384
	// (get) Token: 0x0600B224 RID: 45604 RVA: 0x0042824D File Offset: 0x0042644D
	// (set) Token: 0x0600B225 RID: 45605 RVA: 0x00428263 File Offset: 0x00426463
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

	// Token: 0x17001509 RID: 5385
	// (get) Token: 0x0600B226 RID: 45606 RVA: 0x0042826C File Offset: 0x0042646C
	public vp_Weapon CurrentWeapon
	{
		get
		{
			return this.m_CurrentWeapon;
		}
	}

	// Token: 0x1700150A RID: 5386
	// (get) Token: 0x0600B227 RID: 45607 RVA: 0x00428274 File Offset: 0x00426474
	[Obsolete("Please use the 'CurrentWeaponIndex' parameter instead.")]
	public int CurrentWeaponID
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x1700150B RID: 5387
	// (get) Token: 0x0600B228 RID: 45608 RVA: 0x00428274 File Offset: 0x00426474
	public int CurrentWeaponIndex
	{
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x0600B229 RID: 45609 RVA: 0x0042827C File Offset: 0x0042647C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = (vp_PlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
		if (this.Weapons != null)
		{
			this.StartWeapon = Mathf.Clamp(this.StartWeapon, 0, this.Weapons.Count);
		}
	}

	// Token: 0x0600B22A RID: 45610 RVA: 0x004282D4 File Offset: 0x004264D4
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

	// Token: 0x0600B22B RID: 45611 RVA: 0x00428468 File Offset: 0x00426668
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

	// Token: 0x0600B22C RID: 45612 RVA: 0x004284B4 File Offset: 0x004266B4
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

	// Token: 0x0600B22D RID: 45613 RVA: 0x00428565 File Offset: 0x00426765
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B22E RID: 45614 RVA: 0x00428581 File Offset: 0x00426781
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B22F RID: 45615 RVA: 0x0042859D File Offset: 0x0042679D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.InitWeapon();
		this.UpdateFiring();
	}

	// Token: 0x0600B230 RID: 45616 RVA: 0x004285AC File Offset: 0x004267AC
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

	// Token: 0x0600B231 RID: 45617 RVA: 0x00428634 File Offset: 0x00426834
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

	// Token: 0x0600B232 RID: 45618 RVA: 0x004286E0 File Offset: 0x004268E0
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

	// Token: 0x0600B233 RID: 45619 RVA: 0x00428758 File Offset: 0x00426958
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

	// Token: 0x0600B234 RID: 45620 RVA: 0x004287AF File Offset: 0x004269AF
	public virtual void CancelTimers()
	{
		vp_Timer.CancelAll("EjectShell");
		this.m_DisableAttackStateTimer.Cancel();
		this.m_SetWeaponTimer.Cancel();
		this.m_SetWeaponRefreshTimer.Cancel();
	}

	// Token: 0x0600B235 RID: 45621 RVA: 0x004287DC File Offset: 0x004269DC
	public virtual void SetWeaponLayer(int layer)
	{
		if (this.m_CurrentWeaponIndex < 1 || this.m_CurrentWeaponIndex > this.Weapons.Count)
		{
			return;
		}
		vp_Layer.Set(this.Weapons[this.m_CurrentWeaponIndex - 1].gameObject, layer, true);
	}

	// Token: 0x0600B236 RID: 45622 RVA: 0x0042881A File Offset: 0x00426A1A
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

	// Token: 0x0600B237 RID: 45623 RVA: 0x0042884C File Offset: 0x00426A4C
	public void RefreshAllWeapons()
	{
		foreach (vp_Weapon vp_Weapon in this.Weapons)
		{
			vp_Weapon.Refresh();
			vp_Weapon.RefreshWeaponModel();
		}
	}

	// Token: 0x0600B238 RID: 45624 RVA: 0x004288A4 File Offset: 0x00426AA4
	public int GetWeaponIndex(vp_Weapon weapon)
	{
		return this.Weapons.IndexOf(weapon) + 1;
	}

	// Token: 0x0600B239 RID: 45625 RVA: 0x004288B4 File Offset: 0x00426AB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Reload()
	{
		this.m_Player.Attack.Stop(this.m_Player.CurrentWeaponReloadDuration.Get() + this.ReloadAttackSleepDuration);
	}

	// Token: 0x0600B23A RID: 45626 RVA: 0x004288E4 File Offset: 0x00426AE4
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

	// Token: 0x0600B23B RID: 45627 RVA: 0x00428980 File Offset: 0x00426B80
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

	// Token: 0x0600B23C RID: 45628 RVA: 0x004289F8 File Offset: 0x00426BF8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num != this.m_CurrentWeaponIndex && num >= 0 && num <= this.Weapons.Count && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B23D RID: 45629 RVA: 0x00428A50 File Offset: 0x00426C50
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Attack()
	{
		return !(this.m_CurrentWeapon == null) && !this.m_Player.Attack.Active && !this.m_Player.SetWeapon.Active && !this.m_Player.Reload.Active;
	}

	// Token: 0x0600B23E RID: 45630 RVA: 0x00428AAA File Offset: 0x00426CAA
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

	// Token: 0x0600B23F RID: 45631 RVA: 0x00428ACC File Offset: 0x00426CCC
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

	// Token: 0x0600B240 RID: 45632 RVA: 0x00428B34 File Offset: 0x00426D34
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

	// Token: 0x0600B241 RID: 45633 RVA: 0x00428B8C File Offset: 0x00426D8C
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

	// Token: 0x1700150C RID: 5388
	// (get) Token: 0x0600B242 RID: 45634 RVA: 0x00428BDD File Offset: 0x00426DDD
	public virtual bool OnValue_CurrentWeaponWielded
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !(this.m_CurrentWeapon == null) && this.m_CurrentWeapon.Wielded;
		}
	}

	// Token: 0x1700150D RID: 5389
	// (get) Token: 0x0600B243 RID: 45635 RVA: 0x00428BFA File Offset: 0x00426DFA
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

	// Token: 0x1700150E RID: 5390
	// (get) Token: 0x0600B244 RID: 45636 RVA: 0x00428274 File Offset: 0x00426474
	public virtual int OnValue_CurrentWeaponID
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x1700150F RID: 5391
	// (get) Token: 0x0600B245 RID: 45637 RVA: 0x00428274 File Offset: 0x00426474
	public virtual int OnValue_CurrentWeaponIndex
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentWeaponIndex;
		}
	}

	// Token: 0x17001510 RID: 5392
	// (get) Token: 0x0600B246 RID: 45638 RVA: 0x00428C23 File Offset: 0x00426E23
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

	// Token: 0x17001511 RID: 5393
	// (get) Token: 0x0600B247 RID: 45639 RVA: 0x00428C40 File Offset: 0x00426E40
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

	// Token: 0x040085CE RID: 34254
	public int StartWeapon;

	// Token: 0x040085CF RID: 34255
	public float AttackStateDisableDelay = 0.5f;

	// Token: 0x040085D0 RID: 34256
	public float SetWeaponRefreshStatesDelay = 0.5f;

	// Token: 0x040085D1 RID: 34257
	public float SetWeaponDuration = 0.1f;

	// Token: 0x040085D2 RID: 34258
	public float SetWeaponReloadSleepDuration = 0.3f;

	// Token: 0x040085D3 RID: 34259
	public float SetWeaponZoomSleepDuration = 0.3f;

	// Token: 0x040085D4 RID: 34260
	public float SetWeaponAttackSleepDuration = 0.3f;

	// Token: 0x040085D5 RID: 34261
	public float ReloadAttackSleepDuration = 0.3f;

	// Token: 0x040085D6 RID: 34262
	public bool ReloadAutomatically = true;

	// Token: 0x040085D7 RID: 34263
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040085D8 RID: 34264
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Weapon> m_Weapons;

	// Token: 0x040085D9 RID: 34265
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<List<vp_Weapon>> m_WeaponLists = new List<List<vp_Weapon>>();

	// Token: 0x040085DA RID: 34266
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWeaponIndex = -1;

	// Token: 0x040085DB RID: 34267
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_CurrentWeapon;

	// Token: 0x040085DC RID: 34268
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponTimer = new vp_Timer.Handle();

	// Token: 0x040085DD RID: 34269
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_SetWeaponRefreshTimer = new vp_Timer.Handle();

	// Token: 0x040085DE RID: 34270
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableAttackStateTimer = new vp_Timer.Handle();

	// Token: 0x040085DF RID: 34271
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableReloadStateTimer = new vp_Timer.Handle();

	// Token: 0x02001633 RID: 5683
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WeaponComparer : IComparer
	{
		// Token: 0x0600B24C RID: 45644 RVA: 0x00428E2C File Offset: 0x0042702C
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_Weapon)x).gameObject.name, ((vp_Weapon)y).gameObject.name);
		}
	}
}
