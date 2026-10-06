using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x020014AE RID: 5294
public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
{
	// Token: 0x0600A5D6 RID: 42454 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public ReferenceEqualityComparer()
	{
	}

	// Token: 0x170013AD RID: 5037
	// (get) Token: 0x0600A5D7 RID: 42455 RVA: 0x003DBB54 File Offset: 0x003D9D54
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

	// Token: 0x0600A5D8 RID: 42456 RVA: 0x003DBB80 File Offset: 0x003D9D80
	[PublicizedFrom(EAccessModifier.Private)]
	public bool System.Collections.Generic.IEqualityComparer<T>.Equals(T _x, T _y)
	{
		return _x == _y;
	}

	// Token: 0x0600A5D9 RID: 42457 RVA: 0x003DBB90 File Offset: 0x003D9D90
	[PublicizedFrom(EAccessModifier.Private)]
	public int System.Collections.Generic.IEqualityComparer<T>.GetHashCode(T _obj)
	{
		if (_obj == null)
		{
			return 0;
		}
		return _obj.GetHashCode();
	}

	// Token: 0x04007B94 RID: 31636
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReferenceEqualityComparer<T> instance;
}
