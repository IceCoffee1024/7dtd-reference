using System;
using UnityEngine;

// Token: 0x020015AE RID: 5550
[Serializable]
public class vp_UnitBankInstance : vp_ItemInstance
{
	// Token: 0x170013EC RID: 5100
	// (get) Token: 0x0600AD5D RID: 44381 RVA: 0x00409FBE File Offset: 0x004081BE
	// (set) Token: 0x0600AD5E RID: 44382 RVA: 0x00409FEA File Offset: 0x004081EA
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

	// Token: 0x0600AD5F RID: 44383 RVA: 0x00409FF9 File Offset: 0x004081F9
	public vp_UnitBankInstance(vp_UnitBankType unitBankType, int id, vp_Inventory inventory)
		: base(unitBankType, id)
	{
		this.UnitType = unitBankType.Unit;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AD60 RID: 44384 RVA: 0x0040A01D File Offset: 0x0040821D
	public vp_UnitBankInstance(vp_UnitType unitType, vp_Inventory inventory)
		: base(null, 0)
	{
		this.UnitType = unitType;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AD61 RID: 44385 RVA: 0x0040A03C File Offset: 0x0040823C
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

	// Token: 0x0600AD62 RID: 44386 RVA: 0x0040A06C File Offset: 0x0040826C
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

	// Token: 0x170013ED RID: 5101
	// (get) Token: 0x0600AD63 RID: 44387 RVA: 0x0040A0F8 File Offset: 0x004082F8
	public virtual bool IsInternal
	{
		get
		{
			return this.Type == null;
		}
	}

	// Token: 0x0600AD64 RID: 44388 RVA: 0x0040A108 File Offset: 0x00408308
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

	// Token: 0x0600AD65 RID: 44389 RVA: 0x0040A198 File Offset: 0x00408398
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

	// Token: 0x0600AD66 RID: 44390 RVA: 0x0040A234 File Offset: 0x00408434
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

	// Token: 0x040080F4 RID: 33012
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x040080F5 RID: 33013
	[SerializeField]
	public vp_UnitType UnitType;

	// Token: 0x040080F6 RID: 33014
	[SerializeField]
	public int Count;

	// Token: 0x040080F7 RID: 33015
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_Capacity = -1;

	// Token: 0x040080F8 RID: 33016
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory m_Inventory;

	// Token: 0x040080F9 RID: 33017
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x040080FA RID: 33018
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PrevCount;
}
