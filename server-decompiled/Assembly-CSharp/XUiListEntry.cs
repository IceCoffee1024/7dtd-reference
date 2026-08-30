using System;

// Token: 0x02000E8C RID: 3724
public abstract class XUiListEntry<T> : IComparable<T> where T : XUiListEntry<T>
{
	// Token: 0x06007396 RID: 29590
	public abstract int CompareTo(T _otherEntry);

	// Token: 0x06007397 RID: 29591 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool MatchesSearch(string _searchString)
	{
		return true;
	}

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06007398 RID: 29592 RVA: 0x002BDA15 File Offset: 0x002BBC15
	// (set) Token: 0x06007399 RID: 29593 RVA: 0x002BDA1D File Offset: 0x002BBC1D
	public virtual bool UiDirty { get; set; }

	// Token: 0x0600739A RID: 29594 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiListEntry()
	{
	}
}
