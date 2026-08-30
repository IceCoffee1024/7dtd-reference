using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013D5 RID: 5077
public class EnumerableDebugWrapper<T> : EnumerableDebugWrapper, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009F86 RID: 40838 RVA: 0x003C1420 File Offset: 0x003BF620
	public EnumerableDebugWrapper(IEnumerable<T> enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x06009F87 RID: 40839 RVA: 0x003C142A File Offset: 0x003BF62A
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable<T> enumerable)
		: base(parent, enumerable)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x06009F88 RID: 40840 RVA: 0x003C143B File Offset: 0x003BF63B
	public new IEnumerator<T> GetEnumerator()
	{
		return base.DebugEnumerator<T>(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x040078D6 RID: 30934
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly IEnumerable<T> m_enumerable;
}
