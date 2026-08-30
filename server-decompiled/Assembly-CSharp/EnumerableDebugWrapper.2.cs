using System;
using System.Collections;

// Token: 0x020013D6 RID: 5078
public class EnumerableDebugWrapper : DebugWrapper, IEnumerable
{
	// Token: 0x06009F89 RID: 40841 RVA: 0x003C144E File Offset: 0x003BF64E
	public EnumerableDebugWrapper(IEnumerable enumerable)
		: this(null, enumerable)
	{
	}

	// Token: 0x06009F8A RID: 40842 RVA: 0x003C1458 File Offset: 0x003BF658
	public EnumerableDebugWrapper(DebugWrapper parent, IEnumerable enumerable)
		: base(parent)
	{
		this.m_enumerable = enumerable;
	}

	// Token: 0x06009F8B RID: 40843 RVA: 0x003C1468 File Offset: 0x003BF668
	public IEnumerator GetEnumerator()
	{
		return base.DebugEnumerator(this.m_enumerable.GetEnumerator());
	}

	// Token: 0x040078D7 RID: 30935
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly IEnumerable m_enumerable;
}
