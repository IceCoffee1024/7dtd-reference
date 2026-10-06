using System;
using System.Collections;
using System.Threading;

// Token: 0x020014AD RID: 5293
public class ReferenceEqualityComparer : IEqualityComparer
{
	// Token: 0x0600A5D2 RID: 42450 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x170013AC RID: 5036
	// (get) Token: 0x0600A5D3 RID: 42451 RVA: 0x003DBB14 File Offset: 0x003D9D14
	public static ReferenceEqualityComparer Instance
	{
		get
		{
			if (ReferenceEqualityComparer.instance == null)
			{
				ReferenceEqualityComparer referenceEqualityComparer = new ReferenceEqualityComparer();
				Interlocked.CompareExchange<ReferenceEqualityComparer>(ref ReferenceEqualityComparer.instance, referenceEqualityComparer, null);
			}
			return ReferenceEqualityComparer.instance;
		}
	}

	// Token: 0x0600A5D4 RID: 42452 RVA: 0x003DBB40 File Offset: 0x003D9D40
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IEqualityComparer.Equals(object _x, object _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A5D5 RID: 42453 RVA: 0x003DBB46 File Offset: 0x003D9D46
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.IEqualityComparer.GetHashCode(object _obj)
	{
		if (_obj != null)
		{
			return _obj.GetHashCode();
		}
		return 0;
	}

	// Token: 0x04007B93 RID: 31635
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer instance;
}
