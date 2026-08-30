using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x02001473 RID: 5235
public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
{
	// Token: 0x0600A42B RID: 42027 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x1700132F RID: 4911
	// (get) Token: 0x0600A42C RID: 42028 RVA: 0x003D1B84 File Offset: 0x003CFD84
	public static ReferenceEqualityComparer<T> Instance
	{
		get
		{
			if (ReferenceEqualityComparer<T>.instance == null)
			{
				ReferenceEqualityComparer<T> referenceEqualityComparer = new ReferenceEqualityComparer<T>();
				Interlocked.CompareExchange<ReferenceEqualityComparer<T>>(ref ReferenceEqualityComparer<T>.instance, referenceEqualityComparer, null);
			}
			return ReferenceEqualityComparer<T>.instance;
		}
	}

	// Token: 0x0600A42D RID: 42029 RVA: 0x003D1BB0 File Offset: 0x003CFDB0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IEqualityComparer<T>.Equals(T _x, T _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A42E RID: 42030 RVA: 0x003D1BC0 File Offset: 0x003CFDC0
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.Generic.IEqualityComparer<T>.GetHashCode(T _obj)
	{
		if (_obj == null)
		{
			return 0;
		}
		return _obj.GetHashCode();
	}

	// Token: 0x04007A40 RID: 31296
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer<T> instance;
}
