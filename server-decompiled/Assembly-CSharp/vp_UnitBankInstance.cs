using System;
using UnityEngine;

// Token: 0x020015FD RID: 5629
[Serializable]
public class vp_UnitBankInstance : vp_ItemInstance
{
	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x0600AF65 RID: 44901 RVA: 0x00416556 File Offset: 0x00414756
	// (set) Token: 0x0600AF66 RID: 44902 RVA: 0x00416582 File Offset: 0x00414782
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

	// Token: 0x0600AF67 RID: 44903 RVA: 0x00416591 File Offset: 0x00414791
	public vp_UnitBankInstance(vp_UnitBankType unitBankType, int id, vp_Inventory inventory)
		: base(unitBankType, id)
	{
		this.UnitType = unitBankType.Unit;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AF68 RID: 44904 RVA: 0x004165B5 File Offset: 0x004147B5
	public vp_UnitBankInstance(vp_UnitType unitType, vp_Inventory inventory)
		: base(null, 0)
	{
		this.UnitType = unitType;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AF69 RID: 44905 RVA: 0x004165D4 File Offset: 0x004147D4
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

	// Token: 0x0600AF6A RID: 44906 RVA: 0x00416604 File Offset: 0x00414804
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

	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x0600AF6B RID: 44907 RVA: 0x00416690 File Offset: 0x00414890
	public virtual bool IsInternal
	{
		get
		{
			return this.Type == null;
		}
	}

	// Token: 0x0600AF6C RID: 44908 RVA: 0x004166A0 File Offset: 0x004148A0
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

	// Token: 0x0600AF6D RID: 44909 RVA: 0x00416730 File Offset: 0x00414930
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

	// Token: 0x0600AF6E RID: 44910 RVA: 0x004167CC File Offset: 0x004149CC
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

	// Token: 0x04008291 RID: 33425
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x04008292 RID: 33426
	[SerializeField]
	public vp_UnitType UnitType;

	// Token: 0x04008293 RID: 33427
	[SerializeField]
	public int Count;

	// Token: 0x04008294 RID: 33428
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_Capacity = -1;

	// Token: 0x04008295 RID: 33429
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory m_Inventory;

	// Token: 0x04008296 RID: 33430
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x04008297 RID: 33431
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PrevCount;
}
