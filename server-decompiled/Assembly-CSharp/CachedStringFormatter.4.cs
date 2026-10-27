using System;
using System.Collections.Generic;

// Token: 0x020013E4 RID: 5092
public class CachedStringFormatter<T1, T2, T3, T4>
{
	// Token: 0x06009FDB RID: 40923 RVA: 0x003C5A81 File Offset: 0x003C3C81
	public CachedStringFormatter(Func<T1, T2, T3, T4, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009FDC RID: 40924 RVA: 0x003C5ABC File Offset: 0x003C3CBC
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, out flag);
	}

	// Token: 0x06009FDD RID: 40925 RVA: 0x003C5AD8 File Offset: 0x003C3CD8
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

	// Token: 0x0400791A RID: 31002
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, T4, string> formatter;

	// Token: 0x0400791B RID: 31003
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x0400791C RID: 31004
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x0400791D RID: 31005
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x0400791E RID: 31006
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x0400791F RID: 31007
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x04007920 RID: 31008
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x04007921 RID: 31009
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x04007922 RID: 31010
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x04007923 RID: 31011
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;
}
