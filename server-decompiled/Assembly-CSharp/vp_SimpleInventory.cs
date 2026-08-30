using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015D2 RID: 5586
public class vp_SimpleInventory : MonoBehaviour
{
	// Token: 0x170013F9 RID: 5113
	// (get) Token: 0x0600AE1C RID: 44572 RVA: 0x0040F27E File Offset: 0x0040D47E
	// (set) Token: 0x0600AE1D RID: 44573 RVA: 0x0040F286 File Offset: 0x0040D486
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

	// Token: 0x170013FA RID: 5114
	// (get) Token: 0x0600AE1E RID: 44574 RVA: 0x0040F290 File Offset: 0x0040D490
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

	// Token: 0x170013FB RID: 5115
	// (get) Token: 0x0600AE1F RID: 44575 RVA: 0x0040F2EC File Offset: 0x0040D4EC
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

	// Token: 0x0600AE20 RID: 44576 RVA: 0x0040F36C File Offset: 0x0040D56C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AE21 RID: 44577 RVA: 0x0040F388 File Offset: 0x0040D588
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AE22 RID: 44578 RVA: 0x0040F3A4 File Offset: 0x0040D5A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		IComparer comparer = new vp_SimpleInventory.InventoryWeaponStatusComparer();
		this.m_WeaponTypes.Sort(new Comparison<vp_SimpleInventory.InventoryWeaponStatus>(comparer.Compare));
	}

	// Token: 0x170013FC RID: 5116
	// (get) Token: 0x0600AE23 RID: 44579 RVA: 0x0040F3F4 File Offset: 0x0040D5F4
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

	// Token: 0x0600AE24 RID: 44580 RVA: 0x0040F510 File Offset: 0x0040D710
	public bool HaveItem(object name)
	{
		vp_SimpleInventory.InventoryItemStatus inventoryItemStatus;
		return this.ItemStatusDictionary.TryGetValue((string)name, out inventoryItemStatus) && inventoryItemStatus.Have >= 1;
	}

	// Token: 0x0600AE25 RID: 44581 RVA: 0x0040F540 File Offset: 0x0040D740
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

	// Token: 0x0600AE26 RID: 44582 RVA: 0x0040F59C File Offset: 0x0040D79C
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

	// Token: 0x0600AE27 RID: 44583 RVA: 0x0040F658 File Offset: 0x0040D858
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

	// Token: 0x170013FD RID: 5117
	// (get) Token: 0x0600AE28 RID: 44584 RVA: 0x0040F6DF File Offset: 0x0040D8DF
	// (set) Token: 0x0600AE29 RID: 44585 RVA: 0x0040F6F6 File Offset: 0x0040D8F6
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

	// Token: 0x170013FE RID: 5118
	// (get) Token: 0x0600AE2A RID: 44586 RVA: 0x0040F710 File Offset: 0x0040D910
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

	// Token: 0x170013FF RID: 5119
	// (get) Token: 0x0600AE2B RID: 44587 RVA: 0x0040F749 File Offset: 0x0040D949
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

	// Token: 0x0600AE2C RID: 44588 RVA: 0x0040F764 File Offset: 0x0040D964
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

	// Token: 0x0600AE2D RID: 44589 RVA: 0x0040F789 File Offset: 0x0040D989
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

	// Token: 0x0600AE2E RID: 44590 RVA: 0x0040F7C8 File Offset: 0x0040D9C8
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

	// Token: 0x0600AE2F RID: 44591 RVA: 0x0040F830 File Offset: 0x0040DA30
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

	// Token: 0x0600AE30 RID: 44592 RVA: 0x0040F8A8 File Offset: 0x0040DAA8
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

	// Token: 0x0600AE31 RID: 44593 RVA: 0x0040F904 File Offset: 0x0040DB04
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnAttempt_RemoveClip()
	{
		return this.m_CurrentWeaponStatus != null && this.GetItemStatus(this.m_CurrentWeaponStatus.ClipType) != null && this.m_CurrentWeaponStatus.LoadedAmmo < this.m_CurrentWeaponStatus.MaxAmmo && this.m_Player.RemoveItem.Try(new object[] { this.m_CurrentWeaponStatus.ClipType });
	}

	// Token: 0x0600AE32 RID: 44594 RVA: 0x0040F978 File Offset: 0x0040DB78
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return num == 0 || (num >= 0 && num <= this.m_WeaponTypes.Count && this.HaveItem(this.m_WeaponTypes[num - 1].Name));
	}

	// Token: 0x0600AE33 RID: 44595 RVA: 0x0040F9CD File Offset: 0x0040DBCD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.RefreshWeaponStatus();
	}

	// Token: 0x0600AE34 RID: 44596 RVA: 0x0040F9D8 File Offset: 0x0040DBD8
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

	// Token: 0x04008207 RID: 33287
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x04008208 RID: 33288
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryItemStatus> m_ItemTypes;

	// Token: 0x04008209 RID: 33289
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_SimpleInventory.InventoryWeaponStatus> m_WeaponTypes;

	// Token: 0x0400820A RID: 33290
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, vp_SimpleInventory.InventoryItemStatus> m_ItemStatusDictionary;

	// Token: 0x0400820B RID: 33291
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_SimpleInventory.InventoryWeaponStatus m_CurrentWeaponStatus;

	// Token: 0x0400820C RID: 33292
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_RefreshWeaponStatusIterations;

	// Token: 0x020015D3 RID: 5587
	[PublicizedFrom(EAccessModifier.Protected)]
	public class InventoryWeaponStatusComparer : IComparer
	{
		// Token: 0x0600AE36 RID: 44598 RVA: 0x0040FA64 File Offset: 0x0040DC64
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((vp_SimpleInventory.InventoryWeaponStatus)x).Name, ((vp_SimpleInventory.InventoryWeaponStatus)y).Name);
		}
	}

	// Token: 0x020015D4 RID: 5588
	[Serializable]
	public class InventoryItemStatus
	{
		// Token: 0x0400820D RID: 33293
		public string Name = "Unnamed";

		// Token: 0x0400820E RID: 33294
		public int Have;

		// Token: 0x0400820F RID: 33295
		public int CanHave = 1;

		// Token: 0x04008210 RID: 33296
		public bool ClearOnDeath = true;
	}

	// Token: 0x020015D5 RID: 5589
	[Serializable]
	public class InventoryWeaponStatus : vp_SimpleInventory.InventoryItemStatus
	{
		// Token: 0x04008211 RID: 33297
		public string ClipType = "";

		// Token: 0x04008212 RID: 33298
		public int LoadedAmmo;

		// Token: 0x04008213 RID: 33299
		public int MaxAmmo = 10;
	}
}
