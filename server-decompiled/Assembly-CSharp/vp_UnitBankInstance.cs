using System;
using UnityEngine;

// Token: 0x020015EB RID: 5611
[Serializable]
public class vp_UnitBankInstance : vp_ItemInstance
{
	// Token: 0x1700146A RID: 5226
	// (get) Token: 0x0600AF0D RID: 44813 RVA: 0x00414152 File Offset: 0x00412352
	// (set) Token: 0x0600AF0E RID: 44814 RVA: 0x0041417E File Offset: 0x0041237E
	public int Capacity
	{
		get
		{
			if (this.Type != null)
			{
				this.m_Capacity = ((vp_UnitBankType)this.Type).Capacity;
			}
			return this.m_Capacity;
		}
		set
		{
			this.m_Capacity = Mathf.Max(-1, value);
		}
	}

	// Token: 0x0600AF0F RID: 44815 RVA: 0x0041418D File Offset: 0x0041238D
	public vp_UnitBankInstance(vp_UnitBankType unitBankType, int id, vp_Inventory inventory)
		: base(unitBankType, id)
	{
		this.UnitType = unitBankType.Unit;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AF10 RID: 44816 RVA: 0x004141B1 File Offset: 0x004123B1
	public vp_UnitBankInstance(vp_UnitType unitType, vp_Inventory inventory)
		: base(null, 0)
	{
		this.UnitType = unitType;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AF11 RID: 44817 RVA: 0x004141D0 File Offset: 0x004123D0
	public virtual bool TryRemoveUnits(int amount)
	{
		if (this.Count <= 0)
		{
			return false;
		}
		amount = Mathf.Max(0, amount);
		if (amount == 0)
		{
			return false;
		}
		this.Count = Mathf.Max(0, this.Count - amount);
		return true;
	}

	// Token: 0x0600AF12 RID: 44818 RVA: 0x00414200 File Offset: 0x00412400
	public virtual bool TryGiveUnits(int amount)
	{
		if (this.Type != null && !((vp_UnitBankType)this.Type).Reloadable)
		{
			return false;
		}
		if (this.Capacity != -1 && this.Count >= this.Capacity)
		{
			return false;
		}
		amount = Mathf.Max(0, amount);
		if (amount == 0)
		{
			return false;
		}
		this.Count += amount;
		if (this.Count <= this.Capacity)
		{
			return true;
		}
		if (this.Capacity == -1)
		{
			return true;
		}
		this.Count = this.Capacity;
		return true;
	}

	// Token: 0x1700146B RID: 5227
	// (get) Token: 0x0600AF13 RID: 44819 RVA: 0x0041428C File Offset: 0x0041248C
	public virtual bool IsInternal
	{
		get
		{
			return this.Type == null;
		}
	}

	// Token: 0x0600AF14 RID: 44820 RVA: 0x0041429C File Offset: 0x0041249C
	public virtual bool DoAddUnits(int amount)
	{
		this.m_PrevCount = this.Count;
		this.m_Result = this.TryGiveUnits(amount);
		if (this.m_Inventory.SpaceEnabled && this.m_Result && !this.IsInternal && this.m_Inventory.SpaceMode == vp_Inventory.Mode.Weight)
		{
			this.m_Inventory.UsedSpace += (float)(this.Count - this.m_PrevCount) * this.UnitType.Space;
		}
		this.m_Inventory.SetDirty();
		return this.m_Result;
	}

	// Token: 0x0600AF15 RID: 44821 RVA: 0x0041432C File Offset: 0x0041252C
	public virtual bool DoRemoveUnits(int amount)
	{
		this.m_PrevCount = this.Count;
		this.m_Result = this.TryRemoveUnits(amount);
		if (this.m_Inventory.SpaceEnabled && this.m_Result && !this.IsInternal && this.m_Inventory.SpaceMode == vp_Inventory.Mode.Weight)
		{
			this.m_Inventory.UsedSpace = Mathf.Max(0f, this.m_Inventory.UsedSpace - (float)(this.m_PrevCount - this.Count) * this.UnitType.Space);
		}
		this.m_Inventory.SetDirty();
		return this.m_Result;
	}

	// Token: 0x0600AF16 RID: 44822 RVA: 0x004143C8 File Offset: 0x004125C8
	public virtual int ClampToCapacity()
	{
		int count = this.Count;
		if (this.Capacity != -1)
		{
			this.Count = Mathf.Clamp(this.Count, 0, this.Capacity);
		}
		this.Count = Mathf.Max(0, this.Count);
		return count - this.Count;
	}

	// Token: 0x04008250 RID: 33360
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x04008251 RID: 33361
	[SerializeField]
	public vp_UnitType UnitType;

	// Token: 0x04008252 RID: 33362
	[SerializeField]
	public int Count;

	// Token: 0x04008253 RID: 33363
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_Capacity = -1;

	// Token: 0x04008254 RID: 33364
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory m_Inventory;

	// Token: 0x04008255 RID: 33365
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x04008256 RID: 33366
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PrevCount;
}
