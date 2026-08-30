using System;
using System.Collections.Generic;

// Token: 0x020013B2 RID: 5042
public class CachedStringFormatter<T1, T2, T3, T4>
{
	// Token: 0x06009ED8 RID: 40664 RVA: 0x003BEAE1 File Offset: 0x003BCCE1
	public CachedStringFormatter(Func<T1, T2, T3, T4, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009ED9 RID: 40665 RVA: 0x003BEB1C File Offset: 0x003BCD1C
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, out flag);
	}

	// Token: 0x06009EDA RID: 40666 RVA: 0x003BEB38 File Offset: 0x003BCD38
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

	// Token: 0x0400785E RID: 30814
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, T4, string> formatter;

	// Token: 0x0400785F RID: 30815
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007860 RID: 30816
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007861 RID: 30817
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007862 RID: 30818
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x04007863 RID: 30819
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x04007864 RID: 30820
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x04007865 RID: 30821
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x04007866 RID: 30822
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x04007867 RID: 30823
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;
}
