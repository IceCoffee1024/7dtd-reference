using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015ED RID: 5613
public class vp_SimpleInventory : MonoBehaviour
{
	// Token: 0x17001432 RID: 5170
	// (get) Token: 0x0600AF19 RID: 44825 RVA: 0x004145BE File Offset: 0x004127BE
	// (set) Token: 0x0600AF1A RID: 44826 RVA: 0x004145C6 File Offset: 0x004127C6
	public vp_SimpleInventory.InventoryWeaponStatus CurrentWeaponStatus
	{
		get
		{
			return this.m_CurrentWeaponStatus;
		}
		set
		{
			this.m_CurrentWeaponStatus = value;
		}
	}

	// Token: 0x17001433 RID: 5171
	// (get) Token: 0x0600AF1B RID: 44827 RVA: 0x004145D0 File Offset: 0x004127D0
	public List<vp_SimpleInventory.InventoryItemStatus> Weapons
	{
		get
		{
			List<vp_SimpleInventory.InventoryItemStatus> list = new List<vp_SimpleInventory.InventoryItemStatus>();
			foreach (vp_SimpleInventory.InventoryItemStatus inventoryItemStatus in this.m_WeaponTypes)
			{
				list.Add(inventoryItemStatus);
			}
			return list;
		}
	}

	// Token: 0x17001434 RID: 5172
	// (get) Token: 0x0600AF1C RID: 44828 RVA: 0x0041462C File Offset: 0x0041282C
	public List<vp_SimpleInventory.InventoryItemStatus> EquippedWeapons
	{
		get
		{
			List<vp_SimpleInventory.InventoryItemStatus> list = new List<vp_SimpleInventory.InventoryItemStatus>();
			foreach (vp_SimpleInventory.InventoryItemStatus inventoryItemStatus in this.m_ItemStatusDictionary.Values)
			{
				if (inventoryItemStatus.GetType() == typeof(vp_SimpleInventory.InventoryWeaponStatus) && inventoryItemStatus.Have == 1)
				{
					list.Add(inventoryItemStatus);
				}
			}
			return list;
		}
	}

	// Token: 0x0600AF1D RID: 44829 RVA: 0x004146AC File Offset: 0x004128AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AF1E RID: 44830 RVA: 0x004146C8 File Offset: 0x004128C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AF1F RID: 44831 RVA: 0x004146E4 File Offset: 0x004128E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		IComparer comparer = new vp_SimpleInventory.InventoryWeaponStatusComparer();
		this.m_WeaponTypes.Sort(new Comparison<vp_SimpleInventory.InventoryWeaponStatus>(comparer.Compare));
	}

	// Token: 0x17001435 RID: 5173
	// (get) Token: 0x0600AF20 RID: 44832 RVA: 0x00414734 File Offset: 0x00412934
	public Dictionary<string, vp_SimpleInventory.InventoryItemStatus> ItemStatusDictionary
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_ItemStatusDictionary == null)
			{
				this.m_ItemStatusDictionary = new Dictionary<string, vp_SimpleInventory.InventoryItemStatus>();
				for (int i = this.m_ItemTypes.Count - 1; i > -1; i--)
				{
					if (!this.m_ItemStatusDictionary.ContainsKey(this.m_ItemTypes[i].Name))
					{
						this.m_ItemStatusDictionary.Add(this.m_ItemTypes[i].Name, this.m_ItemTypes[i]);
					}
					else
					{
						this.m_ItemTypes.Remove(this.m_ItemTypes[i]);
					}
				}
				for (int j = this.m_WeaponTypes.Count - 1; j > -1; j--)
				{
					if (!this.m_ItemStatusDictionary.ContainsKey(this.m_WeaponTypes[j].Name))
					{
						this.m_ItemStatusDictionary.Add(this.m_WeaponTypes[j].Name, this.m_WeaponTypes[j]);
					}
					else
					{
						this.m_WeaponTypes.Remove(this.m_WeaponTypes[j]);
					}
				}
			}
			return this.m_ItemStatusDictionary;
		}
	}

	// Token: 0x0600AF21 RID: 44833 RVA: 0x00414850 File Offset: 0x00412A50
	public bool HaveItem(object name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		return this.ItemStatusDictionary.TryGetValue((string)name, out inventoryItemStatus) && inventoryItemStatus.Have >= 1;
	}

	// Token: 0x0600AF22 RID: 44834 RVA: 0x00414880 File Offset: 0x00412A80
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_SimpleInventory.InventoryItemStatus GetItemStatus(string name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		if (!this.ItemStatusDictionary.TryGetValue(name, out inventoryItemStatus))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error: (",
				(this != null) ? this.ToString() : null,
				"). Unknown item type: '",
				name,
				"'."
			}));
		}
		return inventoryItemStatus;
	}

	// Token: 0x0600AF23 RID: 44835 RVA: 0x004148DC File Offset: 0x00412ADC
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_SimpleInventory.InventoryWeaponStatus GetWeaponStatus(string name)
	{
		if (name == null)
		{
			return null;
		}
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		if (!this.ItemStatusDictionary.TryGetValue(name, out inventoryItemStatus))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error: (",
				(this != null) ? this.ToString() : null,
				"). Unknown item type: '",
				name,
				"'."
			}));
			return null;
		}
		if (inventoryItemStatus.GetType() != typeof(vp_SimpleInventory.InventoryWeaponStatus))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Error: (",
				(this != null) ? this.ToString() : null,
				"). Item is not a weapon: '",
				name,
				"'."
			}));
			return null;
		}
		return (vp_SimpleInventory.InventoryWeaponStatus)inventoryItemStatus;
	}

	// Token: 0x0600AF24 RID: 44836 RVA: 0x00414998 File Offset: 0x00412B98
	[PublicizedFrom(EAccessModifier.Protected)]
	public void RefreshWeaponStatus()
	{
		if (!this.m_Player.CurrentWeaponWielded.Get() && this.m_RefreshWeaponStatusIterations < 50)
		{
			this.m_RefreshWeaponStatusIterations++;
			vp_Timer.In(0.1f, new vp_Timer.Callback(this.RefreshWeaponStatus), null);
			return;
		}
		this.m_RefreshWeaponStatusIterations = 0;
		string text = this.m_Player.CurrentWeaponName.Get();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		this.m_CurrentWeaponStatus = this.GetWeaponStatus(text);
	}

	// Token: 0x17001436 RID: 5174
	// (get) Token: 0x0600AF25 RID: 44837 RVA: 0x00414A1F File Offset: 0x00412C1F
	// (set) Token: 0x0600AF26 RID: 44838 RVA: 0x00414A36 File Offset: 0x00412C36
	public virtual int OnValue_CurrentWeaponAmmoCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return 0;
			}
			return this.m_CurrentWeaponStatus.LoadedAmmo;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return;
			}
			this.m_CurrentWeaponStatus.LoadedAmmo = value;
		}
	}

	// Token: 0x17001437 RID: 5175
	// (get) Token: 0x0600AF27 RID: 44839 RVA: 0x00414A50 File Offset: 0x00412C50
	public virtual int OnValue_CurrentWeaponClipCount
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return 0;
			}
			vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
			if (!this.ItemStatusDictionary.TryGetValue(this.m_CurrentWeaponStatus.ClipType, out inventoryItemStatus))
			{
				return 0;
			}
			return inventoryItemStatus.Have;
		}
	}

	// Token: 0x17001438 RID: 5176
	// (get) Token: 0x0600AF28 RID: 44840 RVA: 0x00414A89 File Offset: 0x00412C89
	public virtual string OnValue_CurrentWeaponClipType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CurrentWeaponStatus == null)
			{
				return "";
			}
			return this.m_CurrentWeaponStatus.ClipType;
		}
	}

	// Token: 0x0600AF29 RID: 44841 RVA: 0x00414AA4 File Offset: 0x00412CA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual int OnMessage_GetItemCount(string name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		if (!this.ItemStatusDictionary.TryGetValue(name, out inventoryItemStatus))
		{
			return 0;
		}
		return inventoryItemStatus.Have;
	}

	// Token: 0x0600AF2A RID: 44842 RVA: 0x00414AC9 File Offset: 0x00412CC9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_DepleteAmmo()
	{
		if (this.m_CurrentWeaponStatus == null)
		{
			return false;
		}
		if (this.m_CurrentWeaponStatus.LoadedAmmo < 1)
		{
			return this.m_CurrentWeaponStatus.MaxAmmo == 0;
		}
		this.m_CurrentWeaponStatus.LoadedAmmo--;
		return true;
	}

	// Token: 0x0600AF2B RID: 44843 RVA: 0x00414B08 File Offset: 0x00412D08
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AddAmmo(object arg)
	{
		object[] array = (object[])arg;
		string text = (string)array[0];
		int num = ((array.Length == 2) ? ((int)array[1]) : (-1));
		vp_SimpleInventory.InventoryWeaponStatus weaponStatus = this.GetWeaponStatus(text);
		if (weaponStatus == null)
		{
			return false;
		}
		if (num == -1)
		{
			weaponStatus.LoadedAmmo = weaponStatus.MaxAmmo;
		}
		else
		{
			weaponStatus.LoadedAmmo = Mathf.Min(weaponStatus.LoadedAmmo + num, weaponStatus.MaxAmmo);
		}
		return true;
	}

	// Token: 0x0600AF2C RID: 44844 RVA: 0x00414B70 File Offset: 0x00412D70
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_AddItem(object args)
	{
		object[] array = (object[])args;
		string text = (string)array[0];
		int num = ((array.Length == 2) ? ((int)array[1]) : 1);
		vp_SimpleInventory.InventoryItemStatus itemStatus = this.GetItemStatus(text);
		if (itemStatus == null)
		{
			return false;
		}
		itemStatus.CanHave = Mathf.Max(1, itemStatus.CanHave);
		if (itemStatus.Have >= itemStatus.CanHave)
		{
			return false;
		}
		itemStatus.Have = Mathf.Min(itemStatus.Have + num, itemStatus.CanHave);
		return true;
	}

	// Token: 0x0600AF2D RID: 44845 RVA: 0x00414BE8 File Offset: 0x00412DE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RemoveItem(object args)
	{
		object[] array = (object[])args;
		string text = (string)array[0];
		int num = ((array.Length == 2) ? ((int)array[1]) : 1);
		vp_SimpleInventory.InventoryItemStatus itemStatus = this.GetItemStatus(text);
		if (itemStatus == null)
		{
			return false;
		}
		if (itemStatus.Have <= 0)
		{
			return false;
		}
		itemStatus.Have = Mathf.Max(itemStatus.Have - num, 0);
		return true;
	}

	// Token: 0x0600AF2E RID: 44846 RVA: 0x00414C44 File Offset: 0x00412E44
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RemoveClip()
	{
		return this.m_CurrentWeaponStatus != null && this.GetItemStatus(this.m_CurrentWeaponStatus.ClipType) != null && this.m_CurrentWeaponStatus.LoadedAmmo < this.m_CurrentWeaponStatus.MaxAmmo && this.m_Player.RemoveItem.Try(new object[] { this.m_CurrentWeaponStatus.ClipType });
	}

	// Token: 0x0600AF2F RID: 44847 RVA: 0x00414CB8 File Offset: 0x00412EB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num == 0 || (num >= 0 && num <= this.m_WeaponTypes.Count && this.HaveItem(this.m_WeaponTypes[num - 1].Name));
	}

	// Token: 0x0600AF30 RID: 44848 RVA: 0x00414D0D File Offset: 0x00412F0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshWeaponStatus();
	}

	// Token: 0x0600AF31 RID: 44849 RVA: 0x00414D18 File Offset: 0x00412F18
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.m_ItemStatusDictionary == null)
		{
			return;
		}
		foreach (vp_SimpleInventory.InventoryItemStatus inventoryItemStatus in this.m_ItemStatusDictionary.Values)
		{
			if (inventoryItemStatus.ClearOnDeath)
			{
				inventoryItemStatus.Have = 0;
				if (inventoryItemStatus.GetType() == typeof(vp_SimpleInventory.InventoryWeaponStatus))
				{
					((vp_SimpleInventory.InventoryWeaponStatus)inventoryItemStatus).LoadedAmmo = 0;
				}
			}
		}
	}

	// Token: 0x040082E0 RID: 33504
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040082E1 RID: 33505
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryItemStatus> m_ItemTypes;

	// Token: 0x040082E2 RID: 33506
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryWeaponStatus> m_WeaponTypes;

	// Token: 0x040082E3 RID: 33507
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, vp_SimpleInventory.InventoryItemStatus> m_ItemStatusDictionary;

	// Token: 0x040082E4 RID: 33508
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SimpleInventory.InventoryWeaponStatus m_CurrentWeaponStatus;

	// Token: 0x040082E5 RID: 33509
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_RefreshWeaponStatusIterations;

	// Token: 0x020015EE RID: 5614
	[PublicizedFrom(EAccessModifier.Protected)]
	public class InventoryWeaponStatusComparer : IComparer
	{
		// Token: 0x0600AF33 RID: 44851 RVA: 0x00414DA4 File Offset: 0x00412FA4
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_SimpleInventory.InventoryWeaponStatus)x).Name, ((vp_SimpleInventory.InventoryWeaponStatus)y).Name);
		}
	}

	// Token: 0x020015EF RID: 5615
	[Serializable]
	public class InventoryItemStatus
	{
		// Token: 0x040082E6 RID: 33510
		public string Name = "Unnamed";

		// Token: 0x040082E7 RID: 33511
		public int Have;

		// Token: 0x040082E8 RID: 33512
		public int CanHave = 1;

		// Token: 0x040082E9 RID: 33513
		public bool ClearOnDeath = true;
	}

	// Token: 0x020015F0 RID: 5616
	[Serializable]
	public class InventoryWeaponStatus : vp_SimpleInventory.InventoryItemStatus
	{
		// Token: 0x040082EA RID: 33514
		public string ClipType = "";

		// Token: 0x040082EB RID: 33515
		public int LoadedAmmo;

		// Token: 0x040082EC RID: 33516
		public int MaxAmmo = 10;
	}
}
