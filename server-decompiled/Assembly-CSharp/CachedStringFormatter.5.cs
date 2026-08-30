using System;
using System.Collections.Generic;

// Token: 0x020013B6 RID: 5046
public class CachedStringFormatter<T1, T2, T3, T4, T5>
{
	// Token: 0x06009EE7 RID: 40679 RVA: 0x003BEBF0 File Offset: 0x003BCDF0
	public CachedStringFormatter(global::Func<T1, T2, T3, T4, T5, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009EE8 RID: 40680 RVA: 0x003BEC44 File Offset: 0x003BCE44
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, T5 _v5)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, _v5, out flag);
	}

	// Token: 0x06009EE9 RID: 40681 RVA: 0x003BEC60 File Offset: 0x003BCE60
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, T5 _v5, out bool _valueChanged)
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
		if (!this.comparer5.Equals(this.oldValue5, _v5))
		{
			this.oldValue5 = _v5;
			_valueChanged = true;
		}
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1, _v2, _v3, _v4, _v5);
		}
		return this.cachedResult;
	}

	// Token: 0x04007868 RID: 30824
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly global::Func<T1, T2, T3, T4, T5, string> formatter;

	// Token: 0x04007869 RID: 30825
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x0400786A RID: 30826
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x0400786B RID: 30827
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x0400786C RID: 30828
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x0400786D RID: 30829
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x0400786E RID: 30830
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x0400786F RID: 30831
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x04007870 RID: 30832
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x04007871 RID: 30833
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;

	// Token: 0x04007872 RID: 30834
	[PublicizedFrom(EAccessModifier.Private)]
	public T5 oldValue5;

	// Token: 0x04007873 RID: 30835
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T5> comparer5 = EqualityComparer<T5>.Default;
}
