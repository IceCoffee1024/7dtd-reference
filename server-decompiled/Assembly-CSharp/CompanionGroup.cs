using System;
using System.Collections.Generic;

// Token: 0x020008B5 RID: 2229
public class CompanionGroup
{
	// Token: 0x170006D4 RID: 1748
	public EntityAlive this[int index]
	{
		get
		{
			return this.MemberList[index];
		}
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06004008 RID: 16392 RVA: 0x0019313B File Offset: 0x0019133B
	public int Count
	{
		get
		{
			return this.MemberList.Count;
		}
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x00193148 File Offset: 0x00191348
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

	// Token: 0x0600400A RID: 16394 RVA: 0x00193166 File Offset: 0x00191366
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

	// Token: 0x0600400B RID: 16395 RVA: 0x00193185 File Offset: 0x00191385
	public int IndexOf(EntityAlive entity)
	{
		return this.MemberList.IndexOf(entity);
	}

	// Token: 0x040033F2 RID: 13298
	public OnCompanionGroupChanged OnGroupChanged;

	// Token: 0x040033F3 RID: 13299
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> MemberList = new List<EntityAlive>();
}
