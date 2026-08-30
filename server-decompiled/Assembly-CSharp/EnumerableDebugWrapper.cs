using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020013BA RID: 5050
public class EnumerableDebugWrapper<T> : EnumerableDebugWrapper, IEnumerable<T>, IEnumerable
{
	// Token: 0x06009E89 RID: 40585 RVA: 0x003BC0DC File Offset: 0x003BA2DC
	public EnumerableDebugWrapper(IEnumerable<T> enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x06009E8A RID: 40586 RVA: 0x003BC0E6 File Offset: 0x003BA2E6
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable<T> enumerable)
		: base(parent, enumerable)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x06009E8B RID: 40587 RVA: 0x003BC0F7 File Offset: 0x003BA2F7
	public new IEnumerator<T> GetEnumerator()
	{
		return base.DebugEnumerator<T>(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x040077FD RID: 30717
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly IEnumerable<T> m_enumerable;
}
