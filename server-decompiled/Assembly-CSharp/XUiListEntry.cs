using System;

// Token: 0x02000E9B RID: 3739
public abstract class XUiListEntry<T> : IComparable<T> where T : XUiListEntry<T>
{
	// Token: 0x0600738E RID: 29582
	public abstract int CompareTo(T _otherEntry);

	// Token: 0x0600738F RID: 29583 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool MatchesSearch(string _searchString)
	{
		return true;
	}

	// Token: 0x17000D86 RID: 3462
	// (get) Token: 0x06007390 RID: 29584 RVA: 0x002C0DC5 File Offset: 0x002BEFC5
	// (set) Token: 0x06007391 RID: 29585 RVA: 0x002C0DCD File Offset: 0x002BEFCD
	public virtual bool UiDirty { get; set; }

	// Token: 0x06007392 RID: 29586 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiListEntry()
	{
	}
}
