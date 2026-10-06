using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013F5 RID: 5109
public class EnumerableDebugWrapper<T> : EnumerableDebugWrapper, IEnumerable<T>, IEnumerable
{
	// Token: 0x0600A031 RID: 41009 RVA: 0x003C5FBC File Offset: 0x003C41BC
	public EnumerableDebugWrapper(IEnumerable<T> enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x0600A032 RID: 41010 RVA: 0x003C5FC6 File Offset: 0x003C41C6
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable<T> enumerable)
		: base(parent, enumerable)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x0600A033 RID: 41011 RVA: 0x003C5FD7 File Offset: 0x003C41D7
	public new IEnumerator<T> GetEnumerator()
	{
		return base.DebugEnumerator<T>(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x04007951 RID: 31057
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly IEnumerable<T> m_enumerable;
}
