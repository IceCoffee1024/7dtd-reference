using System;
using System.Collections.Generic;

// Token: 0x0200089A RID: 2202
public class CompanionGroup
{
	// Token: 0x170006A3 RID: 1699
	public EntityAlive this[int index]
	{
		get
		{
			return this.MemberList[index];
		}
	}

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06003FEA RID: 16362 RVA: 0x0018FEBF File Offset: 0x0018E0BF
	public int Count
	{
		get
		{
			return this.MemberList.Count;
		}
	}

	// Token: 0x06003FEB RID: 16363 RVA: 0x0018FECC File Offset: 0x0018E0CC
	public void Add(EntityAlive entity)
	{
		this.MemberList.Add(entity);
		OnCompanionGroupChanged onGroupChanged = this.OnGroupChanged;
		if (onGroupChanged == null)
		{
			return;
		}
		onGroupChanged();
	}

	// Token: 0x06003FEC RID: 16364 RVA: 0x0018FEEA File Offset: 0x0018E0EA
	public void Remove(EntityAlive entity)
	{
		this.MemberList.Remove(entity);
		OnCompanionGroupChanged onGroupChanged = this.OnGroupChanged;
		if (onGroupChanged == null)
		{
			return;
		}
		onGroupChanged();
	}

	// Token: 0x06003FED RID: 16365 RVA: 0x0018FF09 File Offset: 0x0018E109
	public int IndexOf(EntityAlive entity)
	{
		return this.MemberList.IndexOf(entity);
	}

	// Token: 0x0400335D RID: 13149
	public OnCompanionGroupChanged OnGroupChanged;

	// Token: 0x0400335E RID: 13150
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> MemberList = new List<EntityAlive>();
}
