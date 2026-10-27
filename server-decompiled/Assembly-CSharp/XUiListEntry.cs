using System;

// Token: 0x02000EA6 RID: 3750
public abstract class XUiListEntry<T> : IComparable<T> where T : XUiListEntry<T>
{
	// Token: 0x060073B3 RID: 29619
	public abstract int CompareTo(T _otherEntry);

	// Token: 0x060073B4 RID: 29620 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool MatchesSearch(string _searchString)
	{
		return true;
	}

	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x060073B5 RID: 29621 RVA: 0x002C2331 File Offset: 0x002C0531
	// (set) Token: 0x060073B6 RID: 29622 RVA: 0x002C2339 File Offset: 0x002C0539
	public virtual bool UiDirty { get; set; }

	// Token: 0x060073B7 RID: 29623 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiListEntry()
	{
	}
}
