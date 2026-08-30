using System;

// Token: 0x02000E77 RID: 3703
public abstract class XUiListEntry<T> : IComparable<T> where T : XUiListEntry<T>
{
	// Token: 0x060072F6 RID: 29430
	public abstract int CompareTo(T _otherEntry);

	// Token: 0x060072F7 RID: 29431 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool MatchesSearch(string _searchString)
	{
		return true;
	}

	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x060072F8 RID: 29432 RVA: 0x002BA4F1 File Offset: 0x002B86F1
	// (set) Token: 0x060072F9 RID: 29433 RVA: 0x002BA4F9 File Offset: 0x002B86F9
	public virtual bool UiDirty { get; set; }

	// Token: 0x060072FA RID: 29434 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiListEntry()
	{
	}
}
