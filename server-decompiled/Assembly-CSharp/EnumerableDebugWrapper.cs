using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02001407 RID: 5127
public class EnumerableDebugWrapper<T> : EnumerableDebugWrapper, IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A089 RID: 41097 RVA: 0x003C83C0 File Offset: 0x003C65C0
	public EnumerableDebugWrapper(IEnumerable<T> enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x0600A08A RID: 41098 RVA: 0x003C83CA File Offset: 0x003C65CA
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable<T> enumerable)
		: base(parent, enumerable)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x0600A08B RID: 41099 RVA: 0x003C83DB File Offset: 0x003C65DB
	public new IEnumerator<T> GetEnumerator()
	{
		return base.DebugEnumerator<T>(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x04007992 RID: 31122
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly IEnumerable<T> m_enumerable;
}
