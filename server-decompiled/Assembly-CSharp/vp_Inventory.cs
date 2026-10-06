using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015DD RID: 5597
[Serializable]
public class vp_Inventory : MonoBehaviour
{
	// Token: 0x17001461 RID: 5217
	// (get) Token: 0x0600AEB2 RID: 44722 RVA: 0x00412081 File Offset: 0x00410281
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

	// Token: 0x17001462 RID: 5218
	// (get) Token: 0x0600AEB3 RID: 44723 RVA: 0x004120A3 File Offset: 0x004102A3
	public List<vp_UnitBankInstance> UnitBankInstances
	{
		get
		{
			return this.m_UnitBankInstances;
		}
	}

	// Token: 0x17001463 RID: 5219
	// (get) Token: 0x0600AEB4 RID: 44724 RVA: 0x004120AB File Offset: 0x004102AB
	public List<vp_UnitBankInstance> InternalUnitBanks
	{
		get
		{
			return this.m_InternalUnitBanks;
		}
	}

	// Token: 0x17001464 RID: 5220
	// (get) Token: 0x0600AEB5 RID: 44725 RVA: 0x004120B3 File Offset: 0x004102B3
	// (set) Token: 0x0600AEB6 RID: 44726 RVA: 0x004120C5 File Offset: 0x004102C5
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

	// Token: 0x17001465 RID: 5221
	// (get) Token: 0x0600AEB8 RID: 44728 RVA: 0x004120E1 File Offset: 0x004102E1
	// (set) Token: 0x0600AEB7 RID: 44727 RVA: 0x004120CE File Offset: 0x004102CE
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

	// Token: 0x17001466 RID: 5222
	// (get) Token: 0x0600AEB9 RID: 44729 RVA: 0x004120F3 File Offset: 0x004102F3
	[HideInInspector]
	public float RemainingSpace
	{
		get
		{
			return Mathf.Max(0f, this.TotalSpace - this.UsedSpace);
		}
	}

	// Token: 0x0600AEBA RID: 44730 RVA: 0x0041210C File Offset: 0x0041030C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.SaveInitialState();
	}

	// Token: 0x0600AEBB RID: 44731 RVA: 0x00412114 File Offset: 0x00410314
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.Refresh();
	}

	// Token: 0x0600AEBC RID: 44732 RVA: 0x0041211C File Offset: 0x0041031C
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

	// Token: 0x0600AEBD RID: 44733 RVA: 0x004121F4 File Offset: 0x004103F4
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

	// Token: 0x0600AEBE RID: 44734 RVA: 0x0012AC85 File Offset: 0x00128E85
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual vp_Inventory GetInventory()
	{
		return this;
	}

	// Token: 0x0600AEBF RID: 44735 RVA: 0x004122CC File Offset: 0x004104CC
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

	// Token: 0x0600AEC0 RID: 44736 RVA: 0x004122F4 File Offset: 0x004104F4
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

	// Token: 0x0600AEC1 RID: 44737 RVA: 0x004123A8 File Offset: 0x004105A8
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

	// Token: 0x0600AEC2 RID: 44738 RVA: 0x004123F8 File Offset: 0x004105F8
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

	// Token: 0x0600AEC3 RID: 44739 RVA: 0x00412468 File Offset: 0x00410668
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

	// Token: 0x0600AEC4 RID: 44740 RVA: 0x00412510 File Offset: 0x00410710
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

	// Token: 0x0600AEC5 RID: 44741 RVA: 0x004125B1 File Offset: 0x004107B1
	public virtual bool DoAddUnits(vp_UnitBankInstance bank, int amount)
	{
		return bank.DoAddUnits(amount);
	}

	// Token: 0x0600AEC6 RID: 44742 RVA: 0x004125BA File Offset: 0x004107BA
	public virtual bool DoRemoveUnits(vp_UnitBankInstance bank, int amount)
	{
		return bank.DoRemoveUnits(amount);
	}

	// Token: 0x0600AEC7 RID: 44743 RVA: 0x004125C3 File Offset: 0x004107C3
	public virtual bool TryGiveUnits(vp_UnitType unitType, int amount)
	{
		return this.GetItemCap(unitType) != 0 && this.TryGiveUnits(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AEC8 RID: 44744 RVA: 0x004125E0 File Offset: 0x004107E0
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

	// Token: 0x0600AEC9 RID: 44745 RVA: 0x00412650 File Offset: 0x00410850
	public virtual bool TryRemoveUnits(vp_UnitType unitType, int amount)
	{
		vp_UnitBankInstance internalUnitBank = this.GetInternalUnitBank(unitType);
		return internalUnitBank != null && this.DoRemoveUnits(internalUnitBank, amount);
	}

	// Token: 0x0600AECA RID: 44746 RVA: 0x00412674 File Offset: 0x00410874
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

	// Token: 0x0600AECB RID: 44747 RVA: 0x00412770 File Offset: 0x00410970
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

	// Token: 0x0600AECC RID: 44748 RVA: 0x00412797 File Offset: 0x00410997
	public virtual bool TryRemoveItem(vp_ItemType type, int id)
	{
		return this.TryRemoveItem(this.GetItem(type, id));
	}

	// Token: 0x0600AECD RID: 44749 RVA: 0x004127A7 File Offset: 0x004109A7
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

	// Token: 0x0600AECE RID: 44750 RVA: 0x004127BC File Offset: 0x004109BC
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

	// Token: 0x0600AECF RID: 44751 RVA: 0x004127E3 File Offset: 0x004109E3
	public virtual bool TryRemoveUnitBank(vp_UnitBankType type, int id)
	{
		return this.TryRemoveUnitBank(this.GetItem(type, id) as vp_UnitBankInstance);
	}

	// Token: 0x0600AED0 RID: 44752 RVA: 0x004127F8 File Offset: 0x004109F8
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

	// Token: 0x0600AED1 RID: 44753 RVA: 0x0041280D File Offset: 0x00410A0D
	public virtual bool TryReload(vp_ItemType itemType, int unitBankId)
	{
		return this.TryReload(this.GetItem(itemType, unitBankId) as vp_UnitBankInstance, -1);
	}

	// Token: 0x0600AED2 RID: 44754 RVA: 0x00412823 File Offset: 0x00410A23
	public virtual bool TryReload(vp_ItemType itemType, int unitBankId, int amount)
	{
		return this.TryReload(this.GetItem(itemType, unitBankId) as vp_UnitBankInstance, amount);
	}

	// Token: 0x0600AED3 RID: 44755 RVA: 0x00412839 File Offset: 0x00410A39
	public virtual bool TryReload(vp_UnitBankInstance bank)
	{
		return this.TryReload(bank, -1);
	}

	// Token: 0x0600AED4 RID: 44756 RVA: 0x00412844 File Offset: 0x00410A44
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

	// Token: 0x0600AED5 RID: 44757 RVA: 0x00412910 File Offset: 0x00410B10
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

	// Token: 0x0600AED6 RID: 44758 RVA: 0x00412974 File Offset: 0x00410B74
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

	// Token: 0x0600AED7 RID: 44759 RVA: 0x00412AA4 File Offset: 0x00410CA4
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

	// Token: 0x0600AED8 RID: 44760 RVA: 0x00412B90 File Offset: 0x00410D90
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

	// Token: 0x0600AED9 RID: 44761 RVA: 0x00412C64 File Offset: 0x00410E64
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

	// Token: 0x0600AEDA RID: 44762 RVA: 0x00412D1A File Offset: 0x00410F1A
	public virtual bool HaveItem(vp_ItemType itemType, int id = -1)
	{
		return !(itemType == null) && this.GetItem(itemType, id) != null;
	}

	// Token: 0x0600AEDB RID: 44763 RVA: 0x00412D34 File Offset: 0x00410F34
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

	// Token: 0x0600AEDC RID: 44764 RVA: 0x00412DD8 File Offset: 0x00410FD8
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

	// Token: 0x0600AEDD RID: 44765 RVA: 0x00412E54 File Offset: 0x00411054
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

	// Token: 0x0600AEDE RID: 44766 RVA: 0x00412FDC File Offset: 0x004111DC
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

	// Token: 0x0600AEDF RID: 44767 RVA: 0x0041306C File Offset: 0x0041126C
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

	// Token: 0x0600AEE0 RID: 44768 RVA: 0x004130E0 File Offset: 0x004112E0
	public virtual void SetUnitCount(vp_UnitType unitType, int amount)
	{
		this.TrySetUnitCount(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AEE1 RID: 44769 RVA: 0x004130F4 File Offset: 0x004112F4
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

	// Token: 0x0600AEE2 RID: 44770 RVA: 0x0041314A File Offset: 0x0041134A
	public virtual bool TrySetUnitCount(vp_UnitType unitType, int amount)
	{
		return this.TrySetUnitCount(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AEE3 RID: 44771 RVA: 0x0041315C File Offset: 0x0041135C
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

	// Token: 0x0600AEE4 RID: 44772 RVA: 0x004131F8 File Offset: 0x004113F8
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

	// Token: 0x0600AEE5 RID: 44773 RVA: 0x0041325C File Offset: 0x0041145C
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

	// Token: 0x0600AEE6 RID: 44774 RVA: 0x0041335C File Offset: 0x0041155C
	public virtual int GetUnitCount(vp_UnitType unitType)
	{
		vp_UnitBankInstance internalUnitBank = this.GetInternalUnitBank(unitType);
		if (internalUnitBank == null)
		{
			return 0;
		}
		return internalUnitBank.Count;
	}

	// Token: 0x0600AEE7 RID: 44775 RVA: 0x0041337C File Offset: 0x0041157C
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

	// Token: 0x0600AEE8 RID: 44776 RVA: 0x00413474 File Offset: 0x00411674
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

	// Token: 0x0600AEE9 RID: 44777 RVA: 0x004136E0 File Offset: 0x004118E0
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

	// Token: 0x0600AEEA RID: 44778 RVA: 0x0041376B File Offset: 0x0041196B
	public virtual void SetTotalSpace(float spaceLimitation)
	{
		this.SetDirty();
		this.TotalSpace = Mathf.Max(0f, spaceLimitation);
	}

	// Token: 0x0600AEEB RID: 44779 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetDirty()
	{
	}

	// Token: 0x0600AEEC RID: 44780 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClearItemDictionaries()
	{
	}

	// Token: 0x04008211 RID: 33297
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.ItemRecordsSection m_ItemRecords;

	// Token: 0x04008212 RID: 33298
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.ItemCapsSection m_ItemCaps;

	// Token: 0x04008213 RID: 33299
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.SpaceLimitSection m_SpaceLimit;

	// Token: 0x04008214 RID: 33300
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008215 RID: 33301
	[SerializeField]
	[HideInInspector]
	public List<vp_ItemInstance> ItemInstances = new List<vp_ItemInstance>();

	// Token: 0x04008216 RID: 33302
	[SerializeField]
	[HideInInspector]
	public List<vp_Inventory.ItemCap> m_ItemCapInstances = new List<vp_Inventory.ItemCap>();

	// Token: 0x04008217 RID: 33303
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_UnitBankInstance> m_UnitBankInstances = new List<vp_UnitBankInstance>();

	// Token: 0x04008218 RID: 33304
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_UnitBankInstance> m_InternalUnitBanks = new List<vp_UnitBankInstance>();

	// Token: 0x04008219 RID: 33305
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x0400821A RID: 33306
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int UNIDENTIFIED = -1;

	// Token: 0x0400821B RID: 33307
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int MAXCAPACITY = -1;

	// Token: 0x0400821C RID: 33308
	[SerializeField]
	[HideInInspector]
	public bool CapsEnabled;

	// Token: 0x0400821D RID: 33309
	[SerializeField]
	[HideInInspector]
	public bool SpaceEnabled;

	// Token: 0x0400821E RID: 33310
	[SerializeField]
	[HideInInspector]
	public vp_Inventory.Mode SpaceMode;

	// Token: 0x0400821F RID: 33311
	[SerializeField]
	[HideInInspector]
	public bool AllowOnlyListed;

	// Token: 0x04008220 RID: 33312
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_TotalSpace = 100f;

	// Token: 0x04008221 RID: 33313
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_UsedSpace;

	// Token: 0x04008222 RID: 33314
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x04008223 RID: 33315
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Inventory.StartItemRecord> m_StartItems = new List<vp_Inventory.StartItemRecord>();

	// Token: 0x04008224 RID: 33316
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_FirstItemsDirty = true;

	// Token: 0x04008225 RID: 33317
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, vp_ItemInstance> m_FirstItemsOfType = new Dictionary<vp_ItemType, vp_ItemInstance>(100);

	// Token: 0x04008226 RID: 33318
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_GetFirstItemInstanceResult;

	// Token: 0x04008227 RID: 33319
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ItemDictionaryDirty = true;

	// Token: 0x04008228 RID: 33320
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<int, vp_ItemInstance> m_ItemDictionary = new Dictionary<int, vp_ItemInstance>();

	// Token: 0x04008229 RID: 33321
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_GetItemResult;

	// Token: 0x020015DE RID: 5598
	[Serializable]
	public class ItemRecordsSection
	{
	}

	// Token: 0x020015DF RID: 5599
	[Serializable]
	public class ItemCapsSection
	{
	}

	// Token: 0x020015E0 RID: 5600
	[Serializable]
	public class SpaceLimitSection
	{
	}

	// Token: 0x020015E1 RID: 5601
	[Serializable]
	public class ItemCap
	{
		// Token: 0x0600AEF1 RID: 44785 RVA: 0x004137FF File Offset: 0x004119FF
		public ItemCap(vp_ItemType type, int cap)
		{
			this.Type = type;
			this.Cap = cap;
		}

		// Token: 0x0400822A RID: 33322
		[SerializeField]
		public vp_ItemType Type;

		// Token: 0x0400822B RID: 33323
		[SerializeField]
		public int Cap;
	}

	// Token: 0x020015E2 RID: 5602
	public enum Mode
	{
		// Token: 0x0400822D RID: 33325
		Weight,
		// Token: 0x0400822E RID: 33326
		Volume
	}

	// Token: 0x020015E3 RID: 5603
	[PublicizedFrom(EAccessModifier.Protected)]
	public struct StartItemRecord
	{
		// Token: 0x0600AEF2 RID: 44786 RVA: 0x00413815 File Offset: 0x00411A15
		public StartItemRecord(vp_ItemType type, int id, int amount)
		{
			this.Type = type;
			this.ID = id;
			this.Amount = amount;
		}

		// Token: 0x0400822F RID: 33327
		public vp_ItemType Type;

		// Token: 0x04008230 RID: 33328
		public int ID;

		// Token: 0x04008231 RID: 33329
		public int Amount;
	}
}
