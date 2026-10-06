using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200160C RID: 5644
public class vp_SimpleInventory : MonoBehaviour
{
	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x0600AFAB RID: 44971 RVA: 0x0041829A File Offset: 0x0041649A
	// (set) Token: 0x0600AFAC RID: 44972 RVA: 0x004182A2 File Offset: 0x004164A2
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

	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x0600AFAD RID: 44973 RVA: 0x004182AC File Offset: 0x004164AC
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

	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x0600AFAE RID: 44974 RVA: 0x00418308 File Offset: 0x00416508
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

	// Token: 0x0600AFAF RID: 44975 RVA: 0x00418388 File Offset: 0x00416588
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AFB0 RID: 44976 RVA: 0x004183A4 File Offset: 0x004165A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AFB1 RID: 44977 RVA: 0x004183C0 File Offset: 0x004165C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		IComparer comparer = new vp_SimpleInventory.InventoryWeaponStatusComparer();
		this.m_WeaponTypes.Sort(new Comparison<vp_SimpleInventory.InventoryWeaponStatus>(comparer.Compare));
	}

	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x0600AFB2 RID: 44978 RVA: 0x00418410 File Offset: 0x00416610
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

	// Token: 0x0600AFB3 RID: 44979 RVA: 0x0041852C File Offset: 0x0041672C
	public bool HaveItem(object name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		return this.ItemStatusDictionary.TryGetValue((string)name, out inventoryItemStatus) && inventoryItemStatus.Have >= 1;
	}

	// Token: 0x0600AFB4 RID: 44980 RVA: 0x0041855C File Offset: 0x0041675C
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

	// Token: 0x0600AFB5 RID: 44981 RVA: 0x004185B8 File Offset: 0x004167B8
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

	// Token: 0x0600AFB6 RID: 44982 RVA: 0x00418674 File Offset: 0x00416874
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

	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x0600AFB7 RID: 44983 RVA: 0x004186FB File Offset: 0x004168FB
	// (set) Token: 0x0600AFB8 RID: 44984 RVA: 0x00418712 File Offset: 0x00416912
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

	// Token: 0x1700147A RID: 5242
	// (get) Token: 0x0600AFB9 RID: 44985 RVA: 0x0041872C File Offset: 0x0041692C
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

	// Token: 0x1700147B RID: 5243
	// (get) Token: 0x0600AFBA RID: 44986 RVA: 0x00418765 File Offset: 0x00416965
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

	// Token: 0x0600AFBB RID: 44987 RVA: 0x00418780 File Offset: 0x00416980
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

	// Token: 0x0600AFBC RID: 44988 RVA: 0x004187A5 File Offset: 0x004169A5
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

	// Token: 0x0600AFBD RID: 44989 RVA: 0x004187E4 File Offset: 0x004169E4
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

	// Token: 0x0600AFBE RID: 44990 RVA: 0x0041884C File Offset: 0x00416A4C
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

	// Token: 0x0600AFBF RID: 44991 RVA: 0x004188C4 File Offset: 0x00416AC4
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

	// Token: 0x0600AFC0 RID: 44992 RVA: 0x00418920 File Offset: 0x00416B20
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RemoveClip()
	{
		return this.m_CurrentWeaponStatus != null && this.GetItemStatus(this.m_CurrentWeaponStatus.ClipType) != null && this.m_CurrentWeaponStatus.LoadedAmmo < this.m_CurrentWeaponStatus.MaxAmmo && this.m_Player.RemoveItem.Try(new object[] { this.m_CurrentWeaponStatus.ClipType });
	}

	// Token: 0x0600AFC1 RID: 44993 RVA: 0x00418994 File Offset: 0x00416B94
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num == 0 || (num >= 0 && num <= this.m_WeaponTypes.Count && this.HaveItem(this.m_WeaponTypes[num - 1].Name));
	}

	// Token: 0x0600AFC2 RID: 44994 RVA: 0x004189E9 File Offset: 0x00416BE9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshWeaponStatus();
	}

	// Token: 0x0600AFC3 RID: 44995 RVA: 0x004189F4 File Offset: 0x00416BF4
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

	// Token: 0x04008336 RID: 33590
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008337 RID: 33591
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryItemStatus> m_ItemTypes;

	// Token: 0x04008338 RID: 33592
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryWeaponStatus> m_WeaponTypes;

	// Token: 0x04008339 RID: 33593
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, vp_SimpleInventory.InventoryItemStatus> m_ItemStatusDictionary;

	// Token: 0x0400833A RID: 33594
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SimpleInventory.InventoryWeaponStatus m_CurrentWeaponStatus;

	// Token: 0x0400833B RID: 33595
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_RefreshWeaponStatusIterations;

	// Token: 0x0200160D RID: 5645
	[PublicizedFrom(EAccessModifier.Protected)]
	public class InventoryWeaponStatusComparer : IComparer
	{
		// Token: 0x0600AFC5 RID: 44997 RVA: 0x00418A80 File Offset: 0x00416C80
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_SimpleInventory.InventoryWeaponStatus)x).Name, ((vp_SimpleInventory.InventoryWeaponStatus)y).Name);
		}
	}

	// Token: 0x0200160E RID: 5646
	[Serializable]
	public class InventoryItemStatus
	{
		// Token: 0x0400833C RID: 33596
		public string Name = "Unnamed";

		// Token: 0x0400833D RID: 33597
		public int Have;

		// Token: 0x0400833E RID: 33598
		public int CanHave = 1;

		// Token: 0x0400833F RID: 33599
		public bool ClearOnDeath = true;
	}

	// Token: 0x0200160F RID: 5647
	[Serializable]
	public class InventoryWeaponStatus : vp_SimpleInventory.InventoryItemStatus
	{
		// Token: 0x04008340 RID: 33600
		public string ClipType = "";

		// Token: 0x04008341 RID: 33601
		public int LoadedAmmo;

		// Token: 0x04008342 RID: 33602
		public int MaxAmmo = 10;
	}
}
