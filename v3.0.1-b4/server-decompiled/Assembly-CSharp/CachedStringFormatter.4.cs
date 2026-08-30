using System;
using System.Collections.Generic;

// Token: 0x02001397 RID: 5015
public class CachedStringFormatter<T1, T2, T3, T4>
{
	// Token: 0x06009DDB RID: 40411 RVA: 0x003B979D File Offset: 0x003B799D
	public CachedStringFormatter(Func<T1, T2, T3, T4, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009DDC RID: 40412 RVA: 0x003B97D8 File Offset: 0x003B79D8
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, out flag);
	}

	// Token: 0x06009DDD RID: 40413 RVA: 0x003B97F4 File Offset: 0x003B79F4
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, out bool _valueChanged)
	{
		_valueChanged = this.cachedResult == null;
		if (!this.comparer1.Equals(this.oldValue1, _v1))
		{
			this.oldValue1 = _v1;
			_valueChanged = true;
		}
		if (!this.comparer2.Equals(this.oldValue2, _v2))
		{
			this.oldValue2 = _v2;
			_valueChanged = true;
		}
		if (!this.comparer3.Equals(this.oldValue3, _v3))
		{
			this.oldValue3 = _v3;
			_valueChanged = true;
		}
		if (!this.comparer4.Equals(this.oldValue4, _v4))
		{
			this.oldValue4 = _v4;
			_valueChanged = true;
		}
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1, _v2, _v3, _v4);
		}
		return this.cachedResult;
	}

	// Token: 0x04007785 RID: 30597
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, T4, string> formatter;

	// Token: 0x04007786 RID: 30598
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007787 RID: 30599
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007788 RID: 30600
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007789 RID: 30601
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x0400778A RID: 30602
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x0400778B RID: 30603
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x0400778C RID: 30604
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x0400778D RID: 30605
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x0400778E RID: 30606
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;
}
