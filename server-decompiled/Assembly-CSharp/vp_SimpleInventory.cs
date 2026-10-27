using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200161E RID: 5662
public class vp_SimpleInventory : MonoBehaviour
{
	// Token: 0x17001481 RID: 5249
	// (get) Token: 0x0600B003 RID: 45059 RVA: 0x0041A69E File Offset: 0x0041889E
	// (set) Token: 0x0600B004 RID: 45060 RVA: 0x0041A6A6 File Offset: 0x004188A6
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

	// Token: 0x17001482 RID: 5250
	// (get) Token: 0x0600B005 RID: 45061 RVA: 0x0041A6B0 File Offset: 0x004188B0
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

	// Token: 0x17001483 RID: 5251
	// (get) Token: 0x0600B006 RID: 45062 RVA: 0x0041A70C File Offset: 0x0041890C
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

	// Token: 0x0600B007 RID: 45063 RVA: 0x0041A78C File Offset: 0x0041898C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B008 RID: 45064 RVA: 0x0041A7A8 File Offset: 0x004189A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B009 RID: 45065 RVA: 0x0041A7C4 File Offset: 0x004189C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		IComparer comparer = new vp_SimpleInventory.InventoryWeaponStatusComparer();
		this.m_WeaponTypes.Sort(new Comparison<vp_SimpleInventory.InventoryWeaponStatus>(comparer.Compare));
	}

	// Token: 0x17001484 RID: 5252
	// (get) Token: 0x0600B00A RID: 45066 RVA: 0x0041A814 File Offset: 0x00418A14
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

	// Token: 0x0600B00B RID: 45067 RVA: 0x0041A930 File Offset: 0x00418B30
	public bool HaveItem(object name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		return this.ItemStatusDictionary.TryGetValue((string)name, out inventoryItemStatus) && inventoryItemStatus.Have >= 1;
	}

	// Token: 0x0600B00C RID: 45068 RVA: 0x0041A960 File Offset: 0x00418B60
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

	// Token: 0x0600B00D RID: 45069 RVA: 0x0041A9BC File Offset: 0x00418BBC
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

	// Token: 0x0600B00E RID: 45070 RVA: 0x0041AA78 File Offset: 0x00418C78
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

	// Token: 0x17001485 RID: 5253
	// (get) Token: 0x0600B00F RID: 45071 RVA: 0x0041AAFF File Offset: 0x00418CFF
	// (set) Token: 0x0600B010 RID: 45072 RVA: 0x0041AB16 File Offset: 0x00418D16
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

	// Token: 0x17001486 RID: 5254
	// (get) Token: 0x0600B011 RID: 45073 RVA: 0x0041AB30 File Offset: 0x00418D30
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

	// Token: 0x17001487 RID: 5255
	// (get) Token: 0x0600B012 RID: 45074 RVA: 0x0041AB69 File Offset: 0x00418D69
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

	// Token: 0x0600B013 RID: 45075 RVA: 0x0041AB84 File Offset: 0x00418D84
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

	// Token: 0x0600B014 RID: 45076 RVA: 0x0041ABA9 File Offset: 0x00418DA9
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

	// Token: 0x0600B015 RID: 45077 RVA: 0x0041ABE8 File Offset: 0x00418DE8
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

	// Token: 0x0600B016 RID: 45078 RVA: 0x0041AC50 File Offset: 0x00418E50
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

	// Token: 0x0600B017 RID: 45079 RVA: 0x0041ACC8 File Offset: 0x00418EC8
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

	// Token: 0x0600B018 RID: 45080 RVA: 0x0041AD24 File Offset: 0x00418F24
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RemoveClip()
	{
		return this.m_CurrentWeaponStatus != null && this.GetItemStatus(this.m_CurrentWeaponStatus.ClipType) != null && this.m_CurrentWeaponStatus.LoadedAmmo < this.m_CurrentWeaponStatus.MaxAmmo && this.m_Player.RemoveItem.Try(new object[] { this.m_CurrentWeaponStatus.ClipType });
	}

	// Token: 0x0600B019 RID: 45081 RVA: 0x0041AD98 File Offset: 0x00418F98
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num == 0 || (num >= 0 && num <= this.m_WeaponTypes.Count && this.HaveItem(this.m_WeaponTypes[num - 1].Name));
	}

	// Token: 0x0600B01A RID: 45082 RVA: 0x0041ADED File Offset: 0x00418FED
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshWeaponStatus();
	}

	// Token: 0x0600B01B RID: 45083 RVA: 0x0041ADF8 File Offset: 0x00418FF8
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

	// Token: 0x04008377 RID: 33655
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008378 RID: 33656
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryItemStatus> m_ItemTypes;

	// Token: 0x04008379 RID: 33657
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryWeaponStatus> m_WeaponTypes;

	// Token: 0x0400837A RID: 33658
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, vp_SimpleInventory.InventoryItemStatus> m_ItemStatusDictionary;

	// Token: 0x0400837B RID: 33659
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SimpleInventory.InventoryWeaponStatus m_CurrentWeaponStatus;

	// Token: 0x0400837C RID: 33660
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_RefreshWeaponStatusIterations;

	// Token: 0x0200161F RID: 5663
	[PublicizedFrom(EAccessModifier.Protected)]
	public class InventoryWeaponStatusComparer : IComparer
	{
		// Token: 0x0600B01D RID: 45085 RVA: 0x0041AE84 File Offset: 0x00419084
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_SimpleInventory.InventoryWeaponStatus)x).Name, ((vp_SimpleInventory.InventoryWeaponStatus)y).Name);
		}
	}

	// Token: 0x02001620 RID: 5664
	[Serializable]
	public class InventoryItemStatus
	{
		// Token: 0x0400837D RID: 33661
		public string Name = "Unnamed";

		// Token: 0x0400837E RID: 33662
		public int Have;

		// Token: 0x0400837F RID: 33663
		public int CanHave = 1;

		// Token: 0x04008380 RID: 33664
		public bool ClearOnDeath = true;
	}

	// Token: 0x02001621 RID: 5665
	[Serializable]
	public class InventoryWeaponStatus : vp_SimpleInventory.InventoryItemStatus
	{
		// Token: 0x04008381 RID: 33665
		public string ClipType = "";

		// Token: 0x04008382 RID: 33666
		public int LoadedAmmo;

		// Token: 0x04008383 RID: 33667
		public int MaxAmmo = 10;
	}
}
