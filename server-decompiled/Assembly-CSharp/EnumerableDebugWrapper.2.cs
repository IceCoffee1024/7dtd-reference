using System;
using System.Collections;

// Token: 0x020013F6 RID: 5110
public class EnumerableDebugWrapper : DebugWrapper, IEnumerable
{
	// Token: 0x0600A034 RID: 41012 RVA: 0x003C5FEA File Offset: 0x003C41EA
	public EnumerableDebugWrapper(IEnumerable enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x0600A035 RID: 41013 RVA: 0x003C5FF4 File Offset: 0x003C41F4
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable enumerable)
		: base(parent)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x0600A036 RID: 41014 RVA: 0x003C6004 File Offset: 0x003C4204
	public IEnumerator GetEnumerator()
	{
		return base.DebugEnumerator(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x04007952 RID: 31058
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IEnumerable m_enumerable;
}
