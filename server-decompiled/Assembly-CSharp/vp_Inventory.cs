using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015EF RID: 5615
[Serializable]
public class vp_Inventory : MonoBehaviour
{
	// Token: 0x1700146D RID: 5229
	// (get) Token: 0x0600AF0A RID: 44810 RVA: 0x00414485 File Offset: 0x00412685
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

	// Token: 0x1700146E RID: 5230
	// (get) Token: 0x0600AF0B RID: 44811 RVA: 0x004144A7 File Offset: 0x004126A7
	public List<vp_UnitBankInstance> UnitBankInstances
	{
		get
		{
			return this.m_UnitBankInstances;
		}
	}

	// Token: 0x1700146F RID: 5231
	// (get) Token: 0x0600AF0C RID: 44812 RVA: 0x004144AF File Offset: 0x004126AF
	public List<vp_UnitBankInstance> InternalUnitBanks
	{
		get
		{
			return this.m_InternalUnitBanks;
		}
	}

	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x0600AF0D RID: 44813 RVA: 0x004144B7 File Offset: 0x004126B7
	// (set) Token: 0x0600AF0E RID: 44814 RVA: 0x004144C9 File Offset: 0x004126C9
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

	// Token: 0x17001471 RID: 5233
	// (get) Token: 0x0600AF10 RID: 44816 RVA: 0x004144E5 File Offset: 0x004126E5
	// (set) Token: 0x0600AF0F RID: 44815 RVA: 0x004144D2 File Offset: 0x004126D2
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

	// Token: 0x17001472 RID: 5234
	// (get) Token: 0x0600AF11 RID: 44817 RVA: 0x004144F7 File Offset: 0x004126F7
	[HideInInspector]
	public float RemainingSpace
	{
		get
		{
			return Mathf.Max(0f, this.TotalSpace - this.UsedSpace);
		}
	}

	// Token: 0x0600AF12 RID: 44818 RVA: 0x00414510 File Offset: 0x00412710
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.SaveInitialState();
	}

	// Token: 0x0600AF13 RID: 44819 RVA: 0x00414518 File Offset: 0x00412718
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.Refresh();
	}

	// Token: 0x0600AF14 RID: 44820 RVA: 0x00414520 File Offset: 0x00412720
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

	// Token: 0x0600AF15 RID: 44821 RVA: 0x004145F8 File Offset: 0x004127F8
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

	// Token: 0x0600AF16 RID: 44822 RVA: 0x0012B879 File Offset: 0x00129A79
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual vp_Inventory GetInventory()
	{
		return this;
	}

	// Token: 0x0600AF17 RID: 44823 RVA: 0x004146D0 File Offset: 0x004128D0
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

	// Token: 0x0600AF18 RID: 44824 RVA: 0x004146F8 File Offset: 0x004128F8
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

	// Token: 0x0600AF19 RID: 44825 RVA: 0x004147AC File Offset: 0x004129AC
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

	// Token: 0x0600AF1A RID: 44826 RVA: 0x004147FC File Offset: 0x004129FC
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

	// Token: 0x0600AF1B RID: 44827 RVA: 0x0041486C File Offset: 0x00412A6C
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

	// Token: 0x0600AF1C RID: 44828 RVA: 0x00414914 File Offset: 0x00412B14
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

	// Token: 0x0600AF1D RID: 44829 RVA: 0x004149B5 File Offset: 0x00412BB5
	public virtual bool DoAddUnits(vp_UnitBankInstance bank, int amount)
	{
		return bank.DoAddUnits(amount);
	}

	// Token: 0x0600AF1E RID: 44830 RVA: 0x004149BE File Offset: 0x00412BBE
	public virtual bool DoRemoveUnits(vp_UnitBankInstance bank, int amount)
	{
		return bank.DoRemoveUnits(amount);
	}

	// Token: 0x0600AF1F RID: 44831 RVA: 0x004149C7 File Offset: 0x00412BC7
	public virtual bool TryGiveUnits(vp_UnitType unitType, int amount)
	{
		return this.GetItemCap(unitType) != 0 && this.TryGiveUnits(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AF20 RID: 44832 RVA: 0x004149E4 File Offset: 0x00412BE4
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

	// Token: 0x0600AF21 RID: 44833 RVA: 0x00414A54 File Offset: 0x00412C54
	public virtual bool TryRemoveUnits(vp_UnitType unitType, int amount)
	{
		vp_UnitBankInstance internalUnitBank = this.GetInternalUnitBank(unitType);
		return internalUnitBank != null && this.DoRemoveUnits(internalUnitBank, amount);
	}

	// Token: 0x0600AF22 RID: 44834 RVA: 0x00414A78 File Offset: 0x00412C78
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

	// Token: 0x0600AF23 RID: 44835 RVA: 0x00414B74 File Offset: 0x00412D74
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

	// Token: 0x0600AF24 RID: 44836 RVA: 0x00414B9B File Offset: 0x00412D9B
	public virtual bool TryRemoveItem(vp_ItemType type, int id)
	{
		return this.TryRemoveItem(this.GetItem(type, id));
	}

	// Token: 0x0600AF25 RID: 44837 RVA: 0x00414BAB File Offset: 0x00412DAB
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

	// Token: 0x0600AF26 RID: 44838 RVA: 0x00414BC0 File Offset: 0x00412DC0
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

	// Token: 0x0600AF27 RID: 44839 RVA: 0x00414BE7 File Offset: 0x00412DE7
	public virtual bool TryRemoveUnitBank(vp_UnitBankType type, int id)
	{
		return this.TryRemoveUnitBank(this.GetItem(type, id) as vp_UnitBankInstance);
	}

	// Token: 0x0600AF28 RID: 44840 RVA: 0x00414BFC File Offset: 0x00412DFC
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

	// Token: 0x0600AF29 RID: 44841 RVA: 0x00414C11 File Offset: 0x00412E11
	public virtual bool TryReload(vp_ItemType itemType, int unitBankId)
	{
		return this.TryReload(this.GetItem(itemType, unitBankId) as vp_UnitBankInstance, -1);
	}

	// Token: 0x0600AF2A RID: 44842 RVA: 0x00414C27 File Offset: 0x00412E27
	public virtual bool TryReload(vp_ItemType itemType, int unitBankId, int amount)
	{
		return this.TryReload(this.GetItem(itemType, unitBankId) as vp_UnitBankInstance, amount);
	}

	// Token: 0x0600AF2B RID: 44843 RVA: 0x00414C3D File Offset: 0x00412E3D
	public virtual bool TryReload(vp_UnitBankInstance bank)
	{
		return this.TryReload(bank, -1);
	}

	// Token: 0x0600AF2C RID: 44844 RVA: 0x00414C48 File Offset: 0x00412E48
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

	// Token: 0x0600AF2D RID: 44845 RVA: 0x00414D14 File Offset: 0x00412F14
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

	// Token: 0x0600AF2E RID: 44846 RVA: 0x00414D78 File Offset: 0x00412F78
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

	// Token: 0x0600AF2F RID: 44847 RVA: 0x00414EA8 File Offset: 0x004130A8
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

	// Token: 0x0600AF30 RID: 44848 RVA: 0x00414F94 File Offset: 0x00413194
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

	// Token: 0x0600AF31 RID: 44849 RVA: 0x00415068 File Offset: 0x00413268
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

	// Token: 0x0600AF32 RID: 44850 RVA: 0x0041511E File Offset: 0x0041331E
	public virtual bool HaveItem(vp_ItemType itemType, int id = -1)
	{
		return !(itemType == null) && this.GetItem(itemType, id) != null;
	}

	// Token: 0x0600AF33 RID: 44851 RVA: 0x00415138 File Offset: 0x00413338
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

	// Token: 0x0600AF34 RID: 44852 RVA: 0x004151DC File Offset: 0x004133DC
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

	// Token: 0x0600AF35 RID: 44853 RVA: 0x00415258 File Offset: 0x00413458
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

	// Token: 0x0600AF36 RID: 44854 RVA: 0x004153E0 File Offset: 0x004135E0
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

	// Token: 0x0600AF37 RID: 44855 RVA: 0x00415470 File Offset: 0x00413670
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

	// Token: 0x0600AF38 RID: 44856 RVA: 0x004154E4 File Offset: 0x004136E4
	public virtual void SetUnitCount(vp_UnitType unitType, int amount)
	{
		this.TrySetUnitCount(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AF39 RID: 44857 RVA: 0x004154F8 File Offset: 0x004136F8
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

	// Token: 0x0600AF3A RID: 44858 RVA: 0x0041554E File Offset: 0x0041374E
	public virtual bool TrySetUnitCount(vp_UnitType unitType, int amount)
	{
		return this.TrySetUnitCount(this.GetInternalUnitBank(unitType), amount);
	}

	// Token: 0x0600AF3B RID: 44859 RVA: 0x00415560 File Offset: 0x00413760
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

	// Token: 0x0600AF3C RID: 44860 RVA: 0x004155FC File Offset: 0x004137FC
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

	// Token: 0x0600AF3D RID: 44861 RVA: 0x00415660 File Offset: 0x00413860
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

	// Token: 0x0600AF3E RID: 44862 RVA: 0x00415760 File Offset: 0x00413960
	public virtual int GetUnitCount(vp_UnitType unitType)
	{
		vp_UnitBankInstance internalUnitBank = this.GetInternalUnitBank(unitType);
		if (internalUnitBank == null)
		{
			return 0;
		}
		return internalUnitBank.Count;
	}

	// Token: 0x0600AF3F RID: 44863 RVA: 0x00415780 File Offset: 0x00413980
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

	// Token: 0x0600AF40 RID: 44864 RVA: 0x00415878 File Offset: 0x00413A78
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

	// Token: 0x0600AF41 RID: 44865 RVA: 0x00415AE4 File Offset: 0x00413CE4
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

	// Token: 0x0600AF42 RID: 44866 RVA: 0x00415B6F File Offset: 0x00413D6F
	public virtual void SetTotalSpace(float spaceLimitation)
	{
		this.SetDirty();
		this.TotalSpace = Mathf.Max(0f, spaceLimitation);
	}

	// Token: 0x0600AF43 RID: 44867 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetDirty()
	{
	}

	// Token: 0x0600AF44 RID: 44868 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ClearItemDictionaries()
	{
	}

	// Token: 0x04008252 RID: 33362
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.ItemRecordsSection m_ItemRecords;

	// Token: 0x04008253 RID: 33363
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.ItemCapsSection m_ItemCaps;

	// Token: 0x04008254 RID: 33364
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory.SpaceLimitSection m_SpaceLimit;

	// Token: 0x04008255 RID: 33365
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008256 RID: 33366
	[SerializeField]
	[HideInInspector]
	public List<vp_ItemInstance> ItemInstances = new List<vp_ItemInstance>();

	// Token: 0x04008257 RID: 33367
	[SerializeField]
	[HideInInspector]
	public List<vp_Inventory.ItemCap> m_ItemCapInstances = new List<vp_Inventory.ItemCap>();

	// Token: 0x04008258 RID: 33368
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_UnitBankInstance> m_UnitBankInstances = new List<vp_UnitBankInstance>();

	// Token: 0x04008259 RID: 33369
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<vp_UnitBankInstance> m_InternalUnitBanks = new List<vp_UnitBankInstance>();

	// Token: 0x0400825A RID: 33370
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x0400825B RID: 33371
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int UNIDENTIFIED = -1;

	// Token: 0x0400825C RID: 33372
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public const int MAXCAPACITY = -1;

	// Token: 0x0400825D RID: 33373
	[SerializeField]
	[HideInInspector]
	public bool CapsEnabled;

	// Token: 0x0400825E RID: 33374
	[SerializeField]
	[HideInInspector]
	public bool SpaceEnabled;

	// Token: 0x0400825F RID: 33375
	[SerializeField]
	[HideInInspector]
	public vp_Inventory.Mode SpaceMode;

	// Token: 0x04008260 RID: 33376
	[SerializeField]
	[HideInInspector]
	public bool AllowOnlyListed;

	// Token: 0x04008261 RID: 33377
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_TotalSpace = 100f;

	// Token: 0x04008262 RID: 33378
	[SerializeField]
	[HideInInspector]
	[PublicizedFrom(EAccessModifier.Protected)]
	public float m_UsedSpace;

	// Token: 0x04008263 RID: 33379
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x04008264 RID: 33380
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_Inventory.StartItemRecord> m_StartItems = new List<vp_Inventory.StartItemRecord>();

	// Token: 0x04008265 RID: 33381
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_FirstItemsDirty = true;

	// Token: 0x04008266 RID: 33382
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<vp_ItemType, vp_ItemInstance> m_FirstItemsOfType = new Dictionary<vp_ItemType, vp_ItemInstance>(100);

	// Token: 0x04008267 RID: 33383
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_GetFirstItemInstanceResult;

	// Token: 0x04008268 RID: 33384
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_ItemDictionaryDirty = true;

	// Token: 0x04008269 RID: 33385
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<int, vp_ItemInstance> m_ItemDictionary = new Dictionary<int, vp_ItemInstance>();

	// Token: 0x0400826A RID: 33386
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemInstance m_GetItemResult;

	// Token: 0x020015F0 RID: 5616
	[Serializable]
	public class ItemRecordsSection
	{
	}

	// Token: 0x020015F1 RID: 5617
	[Serializable]
	public class ItemCapsSection
	{
	}

	// Token: 0x020015F2 RID: 5618
	[Serializable]
	public class SpaceLimitSection
	{
	}

	// Token: 0x020015F3 RID: 5619
	[Serializable]
	public class ItemCap
	{
		// Token: 0x0600AF49 RID: 44873 RVA: 0x00415C03 File Offset: 0x00413E03
		public ItemCap(vp_ItemType type, int cap)
		{
			this.Type = type;
			this.Cap = cap;
		}

		// Token: 0x0400826B RID: 33387
		[SerializeField]
		public vp_ItemType Type;

		// Token: 0x0400826C RID: 33388
		[SerializeField]
		public int Cap;
	}

	// Token: 0x020015F4 RID: 5620
	public enum Mode
	{
		// Token: 0x0400826E RID: 33390
		Weight,
		// Token: 0x0400826F RID: 33391
		Volume
	}

	// Token: 0x020015F5 RID: 5621
	[PublicizedFrom(EAccessModifier.Protected)]
	public struct StartItemRecord
	{
		// Token: 0x0600AF4A RID: 44874 RVA: 0x00415C19 File Offset: 0x00413E19
		public StartItemRecord(vp_ItemType type, int id, int amount)
		{
			this.Type = type;
			this.ID = id;
			this.Amount = amount;
		}

		// Token: 0x04008270 RID: 33392
		public vp_ItemType Type;

		// Token: 0x04008271 RID: 33393
		public int ID;

		// Token: 0x04008272 RID: 33394
		public int Amount;
	}
}
