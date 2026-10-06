using System;
using System.Collections.Generic;

// Token: 0x020008AB RID: 2219
public class CompanionGroup
{
	// Token: 0x170006CE RID: 1742
	public EntityAlive this[int index]
	{
		get
		{
			return this.MemberList[index];
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06003FE5 RID: 16357 RVA: 0x00192093 File Offset: 0x00190293
	public int Count
	{
		get
		{
			return this.MemberList.Count;
		}
	}

	// Token: 0x06003FE6 RID: 16358 RVA: 0x001920A0 File Offset: 0x001902A0
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

	// Token: 0x06003FE7 RID: 16359 RVA: 0x001920BE File Offset: 0x001902BE
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

	// Token: 0x06003FE8 RID: 16360 RVA: 0x001920DD File Offset: 0x001902DD
	public int IndexOf(EntityAlive entity)
	{
		return this.MemberList.IndexOf(entity);
	}

	// Token: 0x040033D2 RID: 13266
	public OnCompanionGroupChanged OnGroupChanged;

	// Token: 0x040033D3 RID: 13267
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> MemberList = new List<EntityAlive>();
}
