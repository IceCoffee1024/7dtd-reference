using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015BB RID: 5563
[Serializable]
public class vp_Inventory : MonoBehaviour
{
	// Token: 0x1700141C RID: 5148
	// (get) Token: 0x0600ADFF RID: 44543 RVA: 0x0040D22D File Offset: 0x0040B42D
	public Transform Transform
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x1700141D RID: 5149
	// (get) Token: 0x0600AE00 RID: 44544 RVA: 0x0040D24F File Offset: 0x0040B44F
	public List<vp_UnitBankInstance> UnitBankInstances
	{
		get
		{
			return this.m_UnitBankInstances;
		}
	}

	// Token: 0x1700141E RID: 5150
	// (get) Token: 0x0600AE01 RID: 44545 RVA: 0x0040D257 File Offset: 0x0040B457
	public List<vp_UnitBankInstance> InternalUnitBanks
	{
		get
		{
			return this.m_InternalUnitBanks;
		}
	}

	// Token: 0x1700141F RID: 5151
	// (get) Token: 0x0600AE02 RID: 44546 RVA: 0x0040D25F File Offset: 0x0040B45F
	// (set) Token: 0x0600AE03 RID: 44547 RVA: 0x0040D271 File Offset: 0x0040B471
	public float TotalSpace
	{
		get
		{
			return Mathf.Max(-1f, this.m_TotalSpace);
		}
		set
		{
			this.m_TotalSpace = value;
		}
	}

	// Token: 0x17001420 RID: 5152
	// (get) Token: 0x0600AE05 RID: 44549 RVA: 0x0040D28D File Offset: 0x0040B48D
	// (set) Token: 0x0600AE04 RID: 44548 RVA: 0x0040D27A File Offset: 0x0040B47A
	public float UsedSpace
	{
		get
		{
			return Mathf.Max(0f, this.m_UsedSpace);
		}
		set
		{
			this.m_UsedSpace = Mathf.Max(0f, value);
		}
	}

	// Token: 0x17001421 RID: 5153
	// (get) Token: 0x0600AE06 RID: 44550 RVA: 0x0040D29F File Offset: 0x0040B49F
	[HideInInspector]
	public float RemainingSpace
	{
		get
		{
			return Mathf.Max(0f, this.TotalSpace - this.UsedSpace);
		}
	}

	// Token: 0x0600AE07 RID: 44551 RVA: 0x0040D2B8 File Offset: 0x0040B4B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.SaveInitialState();
	}

	// Token: 0x0600AE08 RID: 44552 RVA: 0x0040D2C0 File Offset: 0x0040B4C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.Refresh();
	}

	// Token: 0x0600AE09 RID: 44553 RVA: 0x0040D2C8 File Offset: 0x0040B4C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		vp_TargetEventReturn<vp_Inventory>.Register(this.Transform, "GetInventory", new Func<vp_Inventory>(this.GetInventory));
		vp_TargetEventReturn<vp_ItemType, int, bool>.Register(this.Transform, "TryGiveItem", new Func<vp_ItemType, int, bool>(this.TryGiveItem));
		vp_TargetEventReturn<vp_ItemType, int, bool>.Register(this.Transform, "TryGiveItems", new Func<vp_ItemType, int, bool>(this.TryGiveItems));
		vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.Register(this.Transform, "TryGiveUnitBank", new Func<vp_UnitBankType, int, int, bool>(this.TryGiveUnitBank));
		vp_TargetEventReturn<vp_UnitType, int, bool>.Register(this.Transform, "TryGiveUnits", new Func<vp_UnitType, int, bool>(this.TryGiveUnits));
		vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.Register(this.Transform, "TryDeduct", new Func<vp_UnitBankType, int, int, bool>(this.TryDeduct));
		vp_TargetEventReturn<vp_ItemType, int>.Register(this.Transform, "GetItemCount", new Func<vp_ItemType, int>(this.GetItemCount));
	}

	// Token: 0x0600AE0A RID: 44554 RVA: 0x0040D3A0 File Offset: 0x0040B5A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		vp_TargetEventReturn<vp_ItemType, int, bool>.Unregister(this.Transform, "TryGiveItem", new Func<vp_ItemType, int, bool>(this.TryGiveItem));
		vp_TargetEventReturn<vp_ItemType, int, bool>.Unregister(this.Transform, "TryGiveItems", new Func<vp_ItemType, int, bool>(this.TryGiveItems));
		vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.Unregister(this.Transform, "TryGiveUnitBank", new Func<vp_UnitBankType, int, int, bool>(this.TryGiveUnitBank));
		vp_TargetEventReturn<vp_UnitType, int, bool>.Unregister(this.Transform, "TryGiveUnits", new Func<vp_UnitType, int, bool>(this.TryGiveUnits));
		vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.Unregister(this.Transform, "TryDeduct", new Func<vp_UnitBankType, int, int, bool>(this.TryDeduct));
		vp_TargetEventReturn<vp_ItemType, int>.Unregister(this.Transform, "GetItemCount", new Func<vp_ItemType, int>(this.GetItemCount));
		vp_TargetEventReturn<vp_Inventory>.Unregister(this.Transform, "HasInventory", new Func<vp_Inventory>(this.GetInventory));
	}

	// Token: 0x0600AE0B RID: 44555 RVA: 0x0012A165 File Offset: 0x00128365
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual vp_Inventory GetInventory()
	{
		return this;
	}

	// Token: 0x0600AE0C RID: 44556 RVA: 0x0040D478 File Offset: 0x0040B678
	public virtual bool TryGiveItems(vp_ItemType type, int amount)
	{
		bool flag = false;
		while (amount > 0)
		{
			if (this.TryGiveItem(type, 0))
			{
				flag = true;
			}
			amount--;
		}
		return flag;
	}

	// Token: 0x0600AE0D RID: 44557 RVA: 0x0040D4A0 File Offset: 0x0040B6A0
	public virtual bool TryGiveItem(vp_ItemType itemType, int id)
	{
		if (itemType == null)
		{
			Debug.LogError("Error (" + vp_Utility.GetErrorLocation(2, false) + ") Item type was null.");
			return false;
		}
		vp_UnitType vp_UnitType = itemType as vp_UnitType;
		if (vp_UnitType != null)
		{
			return this.TryGiveUnits(vp_UnitType, id);
		}
		vp_UnitBankType vp_UnitBankType = itemType as vp_UnitBankType;
		if (vp_UnitBankType != null)
		{
			return this.TryGiveUnitBank(vp_UnitBankType, vp_UnitBankType.Capacity, id);
		}
		if (this.CapsEnabled)
		{
			int itemCap = this.GetItemCap(itemType);
			if (itemCap != -1 && this.GetItemCount(itemType) >= itemCap)
			{
				return false;
			}
		}
		if (this.SpaceEnabled && this.UsedSpace + itemType.Space > this.TotalSpace)
		{
			return false;
		}
		this.DoAddItem(itemType, id);
		return true;
	}

	// Token: 0x0600AE0E RID: 44558 RVA: 0x0040D554 File Offset: 0x0040B754
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DoAddItem(vp_ItemType type, int id)
	{
		this.ItemInstances.Add(new vp_ItemInstance(type, id));
		if (this.SpaceEnabled)
		{
			this.m_UsedSpace += type.Space;
		}
		this.m_FirstItemsDirty = true;
		this.m_ItemDictionaryDirty = true;
		this.SetDirty();
	}

	// Token: 0x0600AE0F RID: 44559 RVA: 0x0040D5A4 File Offset: 0x0040B7A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DoRemoveItem(vp_ItemInstance item)
	{
		if (item is vp_UnitBankInstance)
		{
			this.DoRemoveUnitBank(item as vp_UnitBankInstance);
			return;
		}
		this.ItemInstances.Remove(item);
		this.m_FirstItemsDirty = true;
		this.m_ItemDictionaryDirty = true;
		if (this.SpaceEnabled)
		{
			this.m_UsedSpace = Mathf.Max(0f, this.m_UsedSpace - item.Type.Space);
		}
		this.SetDirty();
	}

	// Token: 0x0600AE10 RID: 44560 RVA: 0x0040D614 File Offset: 0x0040B814
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DoAddUnitBank(vp_UnitBankType unitBankType, int id, int unitsLoaded)
	{
		vp_UnitBankInstance vp_UnitBankInstance = new vp_UnitBankInstance(unitBankType, id, this);
		this.m_UnitBankInstances.Add(vp_UnitBankInstance);
		this.m_FirstItemsDirty = true;
		this.m_ItemDictionaryDirty = true;
		if (this.SpaceEnabled && !vp_UnitBankInstance.IsInternal)
		{
			this.m_UsedSpace += unitBankType.Space;
		}
		vp_UnitBankInstance.TryGiveUnits(unitsLoaded);
		if (this.SpaceEnabled && !vp_UnitBankInstance.IsInternal && this.SpaceMode == vp_Inventory.Mode.Weight && unitBankType.Unit != null)
		{
			this.m_UsedSpace += unitBankType.Unit.Space * (float)vp_UnitBankInstance.Count;
		}
		this.SetDirty();
	}

	// Token: 0x0600AE11 RID: 44561 RVA: 0x0040D6BC File Offset: 0x0040B8BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DoRemoveUnitBank(vp_UnitBankInstance bank)
	{
		if (!bank.IsInternal)
		{
			this.m_UnitBankInstances.RemoveAt(this.m_UnitBankInstances.IndexOf(bank));
			this.m_FirstItemsDirty = true;
			this.m_ItemDictionaryDirty = true;
			if (this.SpaceEnabled)
			{
				this.m_UsedSpace -= bank.Type.Space;
				if (this.SpaceMode == vp_Inventory.Mode.Weight)
				{
					this.m_UsedSpace -= bank.UnitType.Space * (float)bank.Count;
				}
			}
		}
		else
		{
			this.m_InternalUnitBanks.RemoveAt(this.m_InternalUnitBanks.IndexOf(bank));
		}
		this.SetDirty();
	}

	// Token: 0x0600AE12 RID: 44562 RVA: 0x0040D75D File Offset: 0x0040B95D
	public virtual bool DoAddUnits(vp_UnitBankInstance bank, int amount)
	{
		return bank.DoAddUnits(amount);
	}

	// Token: 0x0600AE13 RID: 44563 RVA: 0x0040D766 File Offset: 0x0040B966
	public virtual bool DoRemoveUnits(vp_UnitBankInstance bank, int amount)
	{
		return bank.DoRemoveUnits(amount);
	}

	// Token: 0x0600AE14 RID: 44564 RVA: 0x0040D76F File Offset: 0x0040B96F
	public virtual bool TryGiveUnits(vp_UnitType unitType, int amount)
	{
		return this.GetItemCap(unitType) != 0 && this.TryGiveUnits(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AE15 RID: 44565 RVA: 0x0040D78C File Offset: 0x0040B98C
	public virtual bool TryGiveUnits(vp_UnitBankInstance bank, int amount)
	{
		if (bank == null)
		{
			return false;
		}
		amount = Mathf.Max(0, amount);
		if (this.SpaceEnabled && (bank.IsInternal || this.SpaceMode == vp_Inventory.Mode.Weight) && this.RemainingSpace < (float)amount * bank.UnitType.Space)
		{
			amount = (int)(this.RemainingSpace / bank.UnitType.Space);
			return this.DoAddUnits(bank, amount);
		}
		return this.DoAddUnits(bank, amount);
	}

	// Token: 0x0600AE16 RID: 44566 RVA: 0x0040D7FC File Offset: 0x0040B9FC
	public virtual bool TryRemoveUnits(vp_UnitType unitType, int amount)
	{
		vp_UnitBankInstance internalUnitBank = this.GetInternalUnitBank(unitType);
		return internalUnitBank != null && this.DoRemoveUnits(internalUnitBank, amount);
	}

	// Token: 0x0600AE17 RID: 44567 RVA: 0x0040D820 File Offset: 0x0040BA20
	public virtual bool TryGiveUnitBank(vp_UnitBankType unitBankType, int unitsLoaded, int id)
	{
		if (unitBankType == null)
		{
			Debug.LogError("Error (" + vp_Utility.GetErrorLocation(1, false) + ") 'unitBankType' was null.");
			return false;
		}
		if (this.CapsEnabled)
		{
			int itemCap = this.GetItemCap(unitBankType);
			if (itemCap != -1 && this.GetItemCount(unitBankType) >= itemCap)
			{
				return false;
			}
			if (unitBankType.Capacity != -1)
			{
				unitsLoaded = Mathf.Min(unitsLoaded, unitBankType.Capacity);
			}
		}
		if (this.SpaceEnabled)
		{
			vp_Inventory.Mode spaceMode = this.SpaceMode;
			if (spaceMode != vp_Inventory.Mode.Weight)
			{
				if (spaceMode == vp_Inventory.Mode.Volume)
				{
					if (this.UsedSpace + unitBankType.Space > this.TotalSpace)
					{
						return false;
					}
				}
			}
			else
			{
				if (unitBankType.Unit == null)
				{
					Debug.LogError("Error (vp_Inventory) UnitBank item type " + ((unitBankType != null) ? unitBankType.ToString() : null) + " can't be added because its unit type has not been set.");
					return false;
				}
				if (this.UsedSpace + unitBankType.Space + unitBankType.Unit.Space * (float)unitsLoaded > this.TotalSpace)
				{
					return false;
				}
			}
		}
		this.DoAddUnitBank(unitBankType, id, unitsLoaded);
		return true;
	}

	// Token: 0x0600AE18 RID: 44568 RVA: 0x0040D91C File Offset: 0x0040BB1C
	public virtual bool TryRemoveItems(vp_ItemType type, int amount)
	{
		bool flag = false;
		while (amount > 0)
		{
			if (this.TryRemoveItem(type, -1))
			{
				flag = true;
			}
			amount--;
		}
		return flag;
	}

	// Token: 0x0600AE19 RID: 44569 RVA: 0x0040D943 File Offset: 0x0040BB43
	public virtual bool TryRemoveItem(vp_ItemType type, int id)
	{
		return this.TryRemoveItem(this.GetItem(type, id));
	}

	// Token: 0x0600AE1A RID: 44570 RVA: 0x0040D953 File Offset: 0x0040BB53
	public virtual bool TryRemoveItem(vp_ItemInstance item)
	{
		if (item == null)
		{
			return false;
		}
		this.DoRemoveItem(item);
		this.SetDirty();
		return true;
	}

	// Token: 0x0600AE1B RID: 44571 RVA: 0x0040D968 File Offset: 0x0040BB68
	public virtual bool TryRemoveUnitBanks(vp_UnitBankType type, int amount)
	{
		bool flag = false;
		while (amount > 0)
		{
			if (this.TryRemoveUnitBank(type, -1))
			{
				flag = true;
			}
			amount--;
		}
		return flag;
	}

	// Token: 0x0600AE1C RID: 44572 RVA: 0x0040D98F File Offset: 0x0040BB8F
	public virtual bool TryRemoveUnitBank(vp_UnitBankType type, int id)
	{
		return this.TryRemoveUnitBank(this.GetItem(type, id) as vp_UnitBankInstance);
	}

	// Token: 0x0600AE1D RID: 44573 RVA: 0x0040D9A4 File Offset: 0x0040BBA4
	public virtual bool TryRemoveUnitBank(vp_UnitBankInstance unitBank)
	{
		if (unitBank == null)
		{
			return false;
		}
		this.DoRemoveUnitBank(unitBank);
		this.SetDirty();
		return true;
	}

	// Token: 0x0600AE1E RID: 44574 RVA: 0x0040D9B9 File Offset: 0x0040BBB9
	public virtual bool TryReload(vp_ItemType itemType, int unitBankId)
	{
		return this.TryReload(this.GetItem(itemType, unitBankId) as vp_UnitBankInstance, -1);
	}

	// Token: 0x0600AE1F RID: 44575 RVA: 0x0040D9CF File Offset: 0x0040BBCF
	public virtual bool TryReload(vp_ItemType itemType, int unitBankId, int amount)
	{
		return this.TryReload(this.GetItem(itemType, unitBankId) as vp_UnitBankInstance, amount);
	}

	// Token: 0x0600AE20 RID: 44576 RVA: 0x0040D9E5 File Offset: 0x0040BBE5
	public virtual bool TryReload(vp_UnitBankInstance bank)
	{
		return this.TryReload(bank, -1);
	}

	// Token: 0x0600AE21 RID: 44577 RVA: 0x0040D9F0 File Offset: 0x0040BBF0
	public virtual bool TryReload(vp_UnitBankInstance bank, int amount)
	{
		if (bank == null || bank.IsInternal || bank.ID == -1)
		{
			Debug.LogWarning("Warning (" + vp_Utility.GetErrorLocation(1, false) + ") 'TryReloadUnitBank' could not identify a target item. If you are trying to add units to the main inventory please instead use 'TryGiveUnits'.");
			return false;
		}
		int count = bank.Count;
		if (count >= bank.Capacity)
		{
			return false;
		}
		int unitCount = this.GetUnitCount(bank.UnitType);
		if (unitCount < 1)
		{
			return false;
		}
		if (amount == -1)
		{
			amount = bank.Capacity;
		}
		this.TryRemoveUnits(bank.UnitType, amount);
		int num = Mathf.Max(0, unitCount - this.GetUnitCount(bank.UnitType));
		if (!this.DoAddUnits(bank, num))
		{
			return false;
		}
		int num2 = Mathf.Max(0, bank.Count - count);
		if (num2 < 1)
		{
			return false;
		}
		if (num2 > 0 && num2 < num)
		{
			this.TryGiveUnits(bank.UnitType, num - num2);
		}
		return true;
	}

	// Token: 0x0600AE22 RID: 44578 RVA: 0x0040DABC File Offset: 0x0040BCBC
	public virtual bool TryDeduct(vp_UnitBankType unitBankType, int unitBankId, int amount)
	{
		vp_UnitBankInstance vp_UnitBankInstance = ((unitBankId < 1) ? (this.GetItem(unitBankType) as vp_UnitBankInstance) : (this.GetItem(unitBankType, unitBankId) as vp_UnitBankInstance));
		if (vp_UnitBankInstance == null)
		{
			return false;
		}
		if (!this.DoRemoveUnits(vp_UnitBankInstance, amount))
		{
			return false;
		}
		if (vp_UnitBankInstance.Count <= 0 && (vp_UnitBankInstance.Type as vp_UnitBankType).RemoveWhenDepleted)
		{
			this.DoRemoveUnitBank(vp_UnitBankInstance);
		}
		return true;
	}

	// Token: 0x0600AE23 RID: 44579 RVA: 0x0040DB20 File Offset: 0x0040BD20
	public virtual vp_ItemInstance GetItem(vp_ItemType itemType)
	{
		if (this.m_FirstItemsDirty)
		{
			this.m_FirstItemsOfType.Clear();
			foreach (vp_ItemInstance vp_ItemInstance in this.ItemInstances)
			{
				if (vp_ItemInstance != null && !this.m_FirstItemsOfType.ContainsKey(vp_ItemInstance.Type))
				{
					this.m_FirstItemsOfType.Add(vp_ItemInstance.Type, vp_ItemInstance);
				}
			}
			foreach (vp_UnitBankInstance vp_UnitBankInstance in this.UnitBankInstances)
			{
				if (vp_UnitBankInstance != null && !this.m_FirstItemsOfType.ContainsKey(vp_UnitBankInstance.Type))
				{
					this.m_FirstItemsOfType.Add(vp_UnitBankInstance.Type, vp_UnitBankInstance);
				}
			}
			this.m_FirstItemsDirty = false;
		}
		if (itemType == null || !this.m_FirstItemsOfType.TryGetValue(itemType, out this.m_GetFirstItemInstanceResult))
		{
			return null;
		}
		if (this.m_GetFirstItemInstanceResult == null)
		{
			this.m_FirstItemsDirty = true;
			return this.GetItem(itemType);
		}
		return this.m_GetFirstItemInstanceResult;
	}

	// Token: 0x0600AE24 RID: 44580 RVA: 0x0040DC50 File Offset: 0x0040BE50
	public vp_ItemInstance GetItem(vp_ItemType itemType, int id)
	{
		if (itemType == null)
		{
			Debug.LogError("Error (" + vp_Utility.GetErrorLocation(1, true) + ") Sent a null itemType to 'GetItem'.");
			return null;
		}
		if (id < 1)
		{
			return this.GetItem(itemType);
		}
		if (this.m_ItemDictionaryDirty)
		{
			this.m_ItemDictionary.Clear();
			this.m_ItemDictionaryDirty = false;
		}
		if (!this.m_ItemDictionary.TryGetValue(id, out this.m_GetItemResult))
		{
			this.m_GetItemResult = this.GetItemFromList(itemType, id);
			if (this.m_GetItemResult != null && id > 0)
			{
				this.m_ItemDictionary.Add(id, this.m_GetItemResult);
			}
		}
		else if (this.m_GetItemResult != null)
		{
			if (this.m_GetItemResult.Type != itemType)
			{
				Debug.LogWarning("Warning: (vp_Inventory) Player has vp_FPWeapons with identical, non-zero vp_ItemIdentifier IDs! This is much slower than using zero or differing IDs.");
				this.m_GetItemResult = this.GetItemFromList(itemType, id);
			}
		}
		else
		{
			this.m_ItemDictionary.Remove(id);
			this.GetItem(itemType, id);
		}
		return this.m_GetItemResult;
	}

	// Token: 0x0600AE25 RID: 44581 RVA: 0x0040DD3C File Offset: 0x0040BF3C
	public virtual vp_ItemInstance GetItem(string itemTypeName)
	{
		for (int i = 0; i < this.InternalUnitBanks.Count; i++)
		{
			if (this.InternalUnitBanks[i].UnitType.name == itemTypeName)
			{
				return this.InternalUnitBanks[i];
			}
		}
		for (int j = 0; j < this.m_UnitBankInstances.Count; j++)
		{
			if (this.m_UnitBankInstances[j].Type.name == itemTypeName)
			{
				return this.m_UnitBankInstances[j];
			}
		}
		for (int k = 0; k < this.ItemInstances.Count; k++)
		{
			if (this.ItemInstances[k].Type.name == itemTypeName)
			{
				return this.ItemInstances[k];
			}
		}
		return null;
	}

	// Token: 0x0600AE26 RID: 44582 RVA: 0x0040DE10 File Offset: 0x0040C010
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual vp_ItemInstance GetItemFromList(vp_ItemType itemType, int id = -1)
	{
		for (int i = 0; i < this.m_UnitBankInstances.Count; i++)
		{
			if (!(this.m_UnitBankInstances[i].Type != itemType) && (id == -1 || this.m_UnitBankInstances[i].ID == id))
			{
				return this.m_UnitBankInstances[i];
			}
		}
		for (int j = 0; j < this.ItemInstances.Count; j++)
		{
			if (!(this.ItemInstances[j].Type != itemType) && (id == -1 || this.ItemInstances[j].ID == id))
			{
				return this.ItemInstances[j];
			}
		}
		return null;
	}

	// Token: 0x0600AE27 RID: 44583 RVA: 0x0040DEC6 File Offset: 0x0040C0C6
	public virtual bool HaveItem(vp_ItemType itemType, int id = -1)
	{
		return !(itemType == null) && this.GetItem(itemType, id) != null;
	}

	// Token: 0x0600AE28 RID: 44584 RVA: 0x0040DEE0 File Offset: 0x0040C0E0
	public virtual vp_UnitBankInstance GetInternalUnitBank(vp_UnitType unitType)
	{
		for (int i = 0; i < this.m_InternalUnitBanks.Count; i++)
		{
			if (!(this.m_InternalUnitBanks[i].GetType() != typeof(vp_UnitBankInstance)) && !(this.m_InternalUnitBanks[i].Type != null))
			{
				vp_UnitBankInstance vp_UnitBankInstance = this.m_InternalUnitBanks[i];
				if (!(vp_UnitBankInstance.UnitType != unitType))
				{
					return vp_UnitBankInstance;
				}
			}
		}
		this.SetDirty();
		vp_UnitBankInstance vp_UnitBankInstance2 = new vp_UnitBankInstance(unitType, this);
		vp_UnitBankInstance2.Capacity = this.GetItemCap(unitType);
		this.m_InternalUnitBanks.Add(vp_UnitBankInstance2);
		return vp_UnitBankInstance2;
	}

	// Token: 0x0600AE29 RID: 44585 RVA: 0x0040DF84 File Offset: 0x0040C184
	public virtual bool HaveInternalUnitBank(vp_UnitType unitType)
	{
		for (int i = 0; i < this.m_InternalUnitBanks.Count; i++)
		{
			if (!(this.m_InternalUnitBanks[i].GetType() != typeof(vp_UnitBankInstance)) && !(this.m_InternalUnitBanks[i].Type != null) && !(this.m_InternalUnitBanks[i].UnitType != unitType))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600AE2A RID: 44586 RVA: 0x0040E000 File Offset: 0x0040C200
	public virtual void Refresh()
	{
		for (int i = 0; i < this.m_InternalUnitBanks.Count; i++)
		{
			this.m_InternalUnitBanks[i].Capacity = this.GetItemCap(this.m_InternalUnitBanks[i].UnitType);
		}
		if (!this.SpaceEnabled)
		{
			return;
		}
		this.m_UsedSpace = 0f;
		for (int j = 0; j < this.ItemInstances.Count; j++)
		{
			this.m_UsedSpace += this.ItemInstances[j].Type.Space;
		}
		for (int k = 0; k < this.m_UnitBankInstances.Count; k++)
		{
			vp_Inventory.Mode spaceMode = this.SpaceMode;
			if (spaceMode != vp_Inventory.Mode.Weight)
			{
				if (spaceMode == vp_Inventory.Mode.Volume)
				{
					this.m_UsedSpace += this.m_UnitBankInstances[k].Type.Space;
				}
			}
			else
			{
				this.m_UsedSpace += this.m_UnitBankInstances[k].Type.Space + this.m_UnitBankInstances[k].UnitType.Space * (float)this.m_UnitBankInstances[k].Count;
			}
		}
		for (int l = 0; l < this.m_InternalUnitBanks.Count; l++)
		{
			this.m_UsedSpace += this.m_InternalUnitBanks[l].UnitType.Space * (float)this.m_InternalUnitBanks[l].Count;
		}
	}

	// Token: 0x0600AE2B RID: 44587 RVA: 0x0040E188 File Offset: 0x0040C388
	public virtual int GetItemCount(vp_ItemType type)
	{
		vp_UnitType vp_UnitType = type as vp_UnitType;
		if (vp_UnitType != null)
		{
			return this.GetUnitCount(vp_UnitType);
		}
		int num = 0;
		for (int i = 0; i < this.ItemInstances.Count; i++)
		{
			if (this.ItemInstances[i].Type == type)
			{
				num++;
			}
		}
		for (int j = 0; j < this.m_UnitBankInstances.Count; j++)
		{
			if (this.m_UnitBankInstances[j].Type == type)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600AE2C RID: 44588 RVA: 0x0040E218 File Offset: 0x0040C418
	public virtual void SetItemCount(vp_ItemType type, int amount)
	{
		if (type is vp_UnitType)
		{
			this.SetUnitCount((vp_UnitType)type, amount);
			return;
		}
		bool capsEnabled = this.CapsEnabled;
		bool spaceEnabled = this.SpaceEnabled;
		this.CapsEnabled = false;
		this.SpaceEnabled = false;
		int num = amount - this.GetItemCount(type);
		if (num > 0)
		{
			this.TryGiveItems(type, amount);
		}
		else if (num < 0)
		{
			this.TryRemoveItems(type, -amount);
		}
		this.CapsEnabled = capsEnabled;
		this.SpaceEnabled = spaceEnabled;
	}

	// Token: 0x0600AE2D RID: 44589 RVA: 0x0040E28C File Offset: 0x0040C48C
	public virtual void SetUnitCount(vp_UnitType unitType, int amount)
	{
		this.TrySetUnitCount(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AE2E RID: 44590 RVA: 0x0040E2A0 File Offset: 0x0040C4A0
	public virtual void SetUnitCount(vp_UnitBankInstance bank, int amount)
	{
		if (bank == null)
		{
			return;
		}
		amount = Mathf.Max(0, amount);
		if (amount == bank.Count)
		{
			return;
		}
		int count = bank.Count;
		if (!this.DoRemoveUnits(bank, bank.Count))
		{
			bank.Count = count;
		}
		if (amount == 0)
		{
			return;
		}
		if (!this.DoAddUnits(bank, amount))
		{
			bank.Count = count;
		}
	}

	// Token: 0x0600AE2F RID: 44591 RVA: 0x0040E2F6 File Offset: 0x0040C4F6
	public virtual bool TrySetUnitCount(vp_UnitType unitType, int amount)
	{
		return this.TrySetUnitCount(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AE30 RID: 44592 RVA: 0x0040E308 File Offset: 0x0040C508
	public virtual bool TrySetUnitCount(vp_UnitBankInstance bank, int amount)
	{
		if (bank == null)
		{
			return false;
		}
		amount = Mathf.Max(0, amount);
		if (amount == bank.Count)
		{
			return true;
		}
		int count = bank.Count;
		if (!this.DoRemoveUnits(bank, bank.Count))
		{
			bank.Count = count;
		}
		if (amount == 0)
		{
			return true;
		}
		if (bank.IsInternal)
		{
			this.m_Result = this.TryGiveUnits(bank.UnitType, amount);
			if (!this.m_Result)
			{
				bank.Count = count;
			}
			return this.m_Result;
		}
		this.m_Result = this.TryGiveUnits(bank, amount);
		if (!this.m_Result)
		{
			bank.Count = count;
		}
		return this.m_Result;
	}

	// Token: 0x0600AE31 RID: 44593 RVA: 0x0040E3A4 File Offset: 0x0040C5A4
	public virtual int GetItemCap(vp_ItemType type)
	{
		if (!this.CapsEnabled)
		{
			return -1;
		}
		for (int i = 0; i < this.m_ItemCapInstances.Count; i++)
		{
			if (this.m_ItemCapInstances[i].Type == type)
			{
				return this.m_ItemCapInstances[i].Cap;
			}
		}
		if (this.AllowOnlyListed)
		{
			return 0;
		}
		return -1;
	}

	// Token: 0x0600AE32 RID: 44594 RVA: 0x0040E408 File Offset: 0x0040C608
	public virtual void SetItemCap(vp_ItemType type, int cap, bool clamp = false)
	{
		this.SetDirty();
		int i = 0;
		while (i < this.m_ItemCapInstances.Count)
		{
			if (this.m_ItemCapInstances[i].Type == type)
			{
				this.m_ItemCapInstances[i].Cap = cap;
				IL_005B:
				if (type is vp_UnitType)
				{
					for (int j = 0; j < this.m_InternalUnitBanks.Count; j++)
					{
						if (this.m_InternalUnitBanks[j].UnitType != null && this.m_InternalUnitBanks[j].UnitType == type)
						{
							this.m_InternalUnitBanks[j].Capacity = cap;
							if (clamp)
							{
								this.m_InternalUnitBanks[j].ClampToCapacity();
							}
						}
					}
					return;
				}
				if (clamp && this.GetItemCount(type) > cap)
				{
					this.TryRemoveItems(type, this.GetItemCount(type) - cap);
				}
				return;
			}
			else
			{
				i++;
			}
		}
		this.m_ItemCapInstances.Add(new vp_Inventory.ItemCap(type, cap));
		goto IL_005B;
	}

	// Token: 0x0600AE33 RID: 44595 RVA: 0x0040E508 File Offset: 0x0040C708
	public virtual int GetUnitCount(vp_UnitType unitType)
	{
		vp_UnitBankInstance internalUnitBank = this.GetInternalUnitBank(unitType);
		if (internalUnitBank == null)
		{
			return 0;
		}
		return internalUnitBank.Count;
	}

	// Token: 0x0600AE34 RID: 44596 RVA: 0x0040E528 File Offset: 0x0040C728
	public virtual void SaveInitialState()
	{
		for (int i = 0; i < this.InternalUnitBanks.Count; i++)
		{
			this.m_StartItems.Add(new vp_Inventory.StartItemRecord(this.InternalUnitBanks[i].UnitType, 0, this.InternalUnitBanks[i].Count));
		}
		for (int j = 0; j < this.m_UnitBankInstances.Count; j++)
		{
			this.m_StartItems.Add(new vp_Inventory.StartItemRecord(this.m_UnitBankInstances[j].Type, this.m_UnitBankInstances[j].ID, this.m_UnitBankInstances[j].Count));
		}
		for (int k = 0; k < this.ItemInstances.Count; k++)
		{
			this.m_StartItems.Add(new vp_Inventory.StartItemRecord(this.ItemInstances[k].Type, this.ItemInstances[k].ID, 1));
		}
	}

	// Token: 0x0600AE35 RID: 44597 RVA: 0x0040E620 File Offset: 0x0040C820
	public virtual void Reset()
	{
		this.Clear();
		for (int i = 0; i < this.m_StartItems.Count; i++)
		{
			if (this.m_StartItems[i].Type.GetType() == typeof(vp_ItemType))
			{
				this.TryGiveItem(this.m_StartItems[i].Type, this.m_StartItems[i].ID);
			}
			else if (this.m_StartItems[i].Type.GetType() == typeof(vp_UnitBankType))
			{
				this.TryGiveUnitBank(this.m_StartItems[i].Type as vp_UnitBankType, this.m_StartItems[i].Amount, this.m_StartItems[i].ID);
			}
			else if (this.m_StartItems[i].Type.GetType() == typeof(vp_UnitType))
			{
				this.TryGiveUnits(this.m_StartItems[i].Type as vp_UnitType, this.m_StartItems[i].Amount);
			}
			else if (this.m_StartItems[i].Type.GetType().BaseType == typeof(vp_ItemType))
			{
				this.TryGiveItem(this.m_StartItems[i].Type, this.m_StartItems[i].ID);
			}
			else if (this.m_StartItems[i].Type.GetType().BaseType == typeof(vp_UnitBankType))
			{
				this.TryGiveUnitBank(this.m_StartItems[i].Type as vp_UnitBankType, this.m_StartItems[i].Amount, this.m_StartItems[i].ID);
			}
			else if (this.m_StartItems[i].Type.GetType().BaseType == typeof(vp_UnitType))
			{
				this.TryGiveUnits(this.m_StartItems[i].Type as vp_UnitType, this.m_StartItems[i].Amount);
			}
		}
	}

	// Token: 0x0600AE36 RID: 44598 RVA: 0x0040E88C File Offset: 0x0040CA8C
	public virtual void Clear()
	{
		for (int i = this.InternalUnitBanks.Count - 1; i > -1; i--)
		{
			this.DoRemoveUnitBank(this.InternalUnitBanks[i]);
		}
		for (int j = this.m_UnitBankInstances.Count - 1; j > -1; j--)
		{
			this.DoRemoveUnitBank(this.m_UnitBankInstances[j]);
		}
		for (int k = this.ItemInstances.Count - 1; k > -1; k--)
		{
			this.DoRemoveItem(this.ItemInstances[k]);
		}
	}

	// Token: 0x0600AE37 RID: 44599 RVA: 0x0040E917 File Offset: 0x0040CB17
	public virtual void SetTotalSpace(float spaceLimitation)
	{
		this.SetDirty();
		this.TotalSpace = Mathf.Max(0f, spaceLimitation);
	}

	// Token: 0x0600AE38 RID: 44600 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetDirty()
	{
	}

	// Token: 0x0600AE39 RID: 44601 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClearItemDictionaries()
	{
	}

	// Token: 0x0400818E RID: 33166
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.ItemRecordsSection m_ItemRecords;

	// Token: 0x0400818F RID: 33167
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.ItemCapsSection m_ItemCaps;

	// Token: 0x04008190 RID: 33168
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.SpaceLimitSection m_SpaceLimit;

	// Token: 0x04008191 RID: 33169
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008192 RID: 33170
	[SerializeField]
	[HideInInspector]
	public List<vp_ItemInstance> ItemInstances = new List<vp_ItemInstance>();

	// Token: 0x04008193 RID: 33171
	[SerializeField]
	[HideInInspector]
	public List<vp_Inventory.ItemCap> m_ItemCapInstances = new List<vp_Inventory.ItemCap>();

	// Token: 0x04008194 RID: 33172
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_UnitBankInstance> m_UnitBankInstances = new List<vp_UnitBankInstance>();

	// Token: 0x04008195 RID: 33173
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_UnitBankInstance> m_InternalUnitBanks = new List<vp_UnitBankInstance>();

	// Token: 0x04008196 RID: 33174
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x04008197 RID: 33175
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int UNIDENTIFIED = -1;

	// Token: 0x04008198 RID: 33176
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int MAXCAPACITY = -1;

	// Token: 0x04008199 RID: 33177
	[SerializeField]
	[HideInInspector]
	public bool CapsEnabled;

	// Token: 0x0400819A RID: 33178
	[SerializeField]
	[HideInInspector]
	public bool SpaceEnabled;

	// Token: 0x0400819B RID: 33179
	[SerializeField]
	[HideInInspector]
	public vp_Inventory.Mode SpaceMode;

	// Token: 0x0400819C RID: 33180
	[SerializeField]
	[HideInInspector]
	public bool AllowOnlyListed;

	// Token: 0x0400819D RID: 33181
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_TotalSpace = 100f;

	// Token: 0x0400819E RID: 33182
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_UsedSpace;

	// Token: 0x0400819F RID: 33183
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x040081A0 RID: 33184
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Inventory.StartItemRecord> m_StartItems = new List<vp_Inventory.StartItemRecord>();

	// Token: 0x040081A1 RID: 33185
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_FirstItemsDirty = true;

	// Token: 0x040081A2 RID: 33186
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, vp_ItemInstance> m_FirstItemsOfType = new Dictionary<vp_ItemType, vp_ItemInstance>(100);

	// Token: 0x040081A3 RID: 33187
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_GetFirstItemInstanceResult;

	// Token: 0x040081A4 RID: 33188
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ItemDictionaryDirty = true;

	// Token: 0x040081A5 RID: 33189
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<int, vp_ItemInstance> m_ItemDictionary = new Dictionary<int, vp_ItemInstance>();

	// Token: 0x040081A6 RID: 33190
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_GetItemResult;

	// Token: 0x020015BC RID: 5564
	[Serializable]
	public class ItemRecordsSection
	{
	}

	// Token: 0x020015BD RID: 5565
	[Serializable]
	public class ItemCapsSection
	{
	}

	// Token: 0x020015BE RID: 5566
	[Serializable]
	public class SpaceLimitSection
	{
	}

	// Token: 0x020015BF RID: 5567
	[Serializable]
	public class ItemCap
	{
		// Token: 0x0600AE3E RID: 44606 RVA: 0x0040E9AB File Offset: 0x0040CBAB
		public ItemCap(vp_ItemType type, int cap)
		{
			this.Type = type;
			this.Cap = cap;
		}

		// Token: 0x040081A7 RID: 33191
		[SerializeField]
		public vp_ItemType Type;

		// Token: 0x040081A8 RID: 33192
		[SerializeField]
		public int Cap;
	}

	// Token: 0x020015C0 RID: 5568
	public enum Mode
	{
		// Token: 0x040081AA RID: 33194
		Weight,
		// Token: 0x040081AB RID: 33195
		Volume
	}

	// Token: 0x020015C1 RID: 5569
	[PublicizedFrom(EAccessModifier.Protected)]
	public struct StartItemRecord
	{
		// Token: 0x0600AE3F RID: 44607 RVA: 0x0040E9C1 File Offset: 0x0040CBC1
		public StartItemRecord(vp_ItemType type, int id, int amount)
		{
			this.Type = type;
			this.ID = id;
			this.Amount = amount;
		}

		// Token: 0x040081AC RID: 33196
		public vp_ItemType Type;

		// Token: 0x040081AD RID: 33197
		public int ID;

		// Token: 0x040081AE RID: 33198
		public int Amount;
	}
}
