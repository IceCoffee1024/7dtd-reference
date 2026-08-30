using System;
using UnityEngine;

// Token: 0x020015C9 RID: 5577
[Serializable]
public class vp_UnitBankInstance : vp_ItemInstance
{
	// Token: 0x17001425 RID: 5157
	// (get) Token: 0x0600AE5A RID: 44634 RVA: 0x0040F2FE File Offset: 0x0040D4FE
	// (set) Token: 0x0600AE5B RID: 44635 RVA: 0x0040F32A File Offset: 0x0040D52A
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

	// Token: 0x0600AE5C RID: 44636 RVA: 0x0040F339 File Offset: 0x0040D539
	public vp_UnitBankInstance(vp_UnitBankType unitBankType, int id, vp_Inventory inventory)
		: base(unitBankType, id)
	{
		this.UnitType = unitBankType.Unit;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AE5D RID: 44637 RVA: 0x0040F35D File Offset: 0x0040D55D
	public vp_UnitBankInstance(vp_UnitType unitType, vp_Inventory inventory)
		: base(null, 0)
	{
		this.UnitType = unitType;
		this.m_Inventory = inventory;
	}

	// Token: 0x0600AE5E RID: 44638 RVA: 0x0040F37C File Offset: 0x0040D57C
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

	// Token: 0x0600AE5F RID: 44639 RVA: 0x0040F3AC File Offset: 0x0040D5AC
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

	// Token: 0x17001426 RID: 5158
	// (get) Token: 0x0600AE60 RID: 44640 RVA: 0x0040F438 File Offset: 0x0040D638
	public virtual bool IsInternal
	{
		get
		{
			return this.Type == null;
		}
	}

	// Token: 0x0600AE61 RID: 44641 RVA: 0x0040F448 File Offset: 0x0040D648
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

	// Token: 0x0600AE62 RID: 44642 RVA: 0x0040F4D8 File Offset: 0x0040D6D8
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

	// Token: 0x0600AE63 RID: 44643 RVA: 0x0040F574 File Offset: 0x0040D774
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

	// Token: 0x040081CD RID: 33229
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int UNLIMITED = -1;

	// Token: 0x040081CE RID: 33230
	[SerializeField]
	public vp_UnitType UnitType;

	// Token: 0x040081CF RID: 33231
	[SerializeField]
	public int Count;

	// Token: 0x040081D0 RID: 33232
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public int m_Capacity = -1;

	// Token: 0x040081D1 RID: 33233
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Inventory m_Inventory;

	// Token: 0x040081D2 RID: 33234
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Result;

	// Token: 0x040081D3 RID: 33235
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PrevCount;
}
