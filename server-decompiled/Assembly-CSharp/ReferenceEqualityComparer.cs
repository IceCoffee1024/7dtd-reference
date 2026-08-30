using System;
using System.Collections;
using System.Threading;

// Token: 0x02001472 RID: 5234
public class ReferenceEqualityComparer : IEqualityComparer
{
	// Token: 0x0600A427 RID: 42023 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x1700132E RID: 4910
	// (get) Token: 0x0600A428 RID: 42024 RVA: 0x003D1B44 File Offset: 0x003CFD44
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

	// Token: 0x0600A429 RID: 42025 RVA: 0x003D1B70 File Offset: 0x003CFD70
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.IEqualityComparer.Equals(object _x, object _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A42A RID: 42026 RVA: 0x003D1B76 File Offset: 0x003CFD76
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.IEqualityComparer.GetHashCode(object _obj)
	{
		if (_obj != null)
		{
			return _obj.GetHashCode();
		}
		return 0;
	}

	// Token: 0x04007A3F RID: 31295
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer instance;
}
