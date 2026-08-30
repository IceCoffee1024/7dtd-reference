using System;
using System.Collections;

// Token: 0x020013BB RID: 5051
public class EnumerableDebugWrapper : DebugWrapper, IEnumerable
{
	// Token: 0x06009E8C RID: 40588 RVA: 0x003BC10A File Offset: 0x003BA30A
	public EnumerableDebugWrapper(IEnumerable enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x06009E8D RID: 40589 RVA: 0x003BC114 File Offset: 0x003BA314
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable enumerable)
		: base(parent)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x06009E8E RID: 40590 RVA: 0x003BC124 File Offset: 0x003BA324
	public IEnumerator GetEnumerator()
	{
		return base.DebugEnumerator(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x040077FE RID: 30718
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IEnumerable m_enumerable;
}
