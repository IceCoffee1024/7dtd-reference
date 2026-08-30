using System;
using System.Collections;
using System.Threading;

// Token: 0x0200148D RID: 5261
public class ReferenceEqualityComparer : IEqualityComparer
{
	// Token: 0x0600A524 RID: 42276 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x17001367 RID: 4967
	// (get) Token: 0x0600A525 RID: 42277 RVA: 0x003D6E88 File Offset: 0x003D5088
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

	// Token: 0x0600A526 RID: 42278 RVA: 0x003D6EB4 File Offset: 0x003D50B4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IEqualityComparer.Equals(object _x, object _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A527 RID: 42279 RVA: 0x003D6EBA File Offset: 0x003D50BA
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.IEqualityComparer.GetHashCode(object _obj)
	{
		if (_obj != null)
		{
			return _obj.GetHashCode();
		}
		return 0;
	}

	// Token: 0x04007B18 RID: 31512
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer instance;
}
