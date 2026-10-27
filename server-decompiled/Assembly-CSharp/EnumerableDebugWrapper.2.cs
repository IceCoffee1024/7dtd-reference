using System;
using System.Collections;

// Token: 0x02001408 RID: 5128
public class EnumerableDebugWrapper : DebugWrapper, IEnumerable
{
	// Token: 0x0600A08C RID: 41100 RVA: 0x003C83EE File Offset: 0x003C65EE
	public EnumerableDebugWrapper(IEnumerable enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x0600A08D RID: 41101 RVA: 0x003C83F8 File Offset: 0x003C65F8
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable enumerable)
		: base(parent)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x0600A08E RID: 41102 RVA: 0x003C8408 File Offset: 0x003C6608
	public IEnumerator GetEnumerator()
	{
		return base.DebugEnumerator(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x04007993 RID: 31123
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IEnumerable m_enumerable;
}
