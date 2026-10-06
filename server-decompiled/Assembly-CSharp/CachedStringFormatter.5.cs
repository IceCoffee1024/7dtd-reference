using System;
using System.Collections.Generic;

// Token: 0x020013D6 RID: 5078
public class CachedStringFormatter<T1, T2, T3, T4, T5>
{
	// Token: 0x06009F92 RID: 40850 RVA: 0x003C378C File Offset: 0x003C198C
	public CachedStringFormatter(global::Func<T1, T2, T3, T4, T5, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009F93 RID: 40851 RVA: 0x003C37E0 File Offset: 0x003C19E0
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, T5 _v5)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, _v5, out flag);
	}

	// Token: 0x06009F94 RID: 40852 RVA: 0x003C37FC File Offset: 0x003C19FC
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

	// Token: 0x040078E3 RID: 30947
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly global::Func<T1, T2, T3, T4, T5, string> formatter;

	// Token: 0x040078E4 RID: 30948
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x040078E5 RID: 30949
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x040078E6 RID: 30950
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x040078E7 RID: 30951
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x040078E8 RID: 30952
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x040078E9 RID: 30953
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x040078EA RID: 30954
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x040078EB RID: 30955
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x040078EC RID: 30956
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;

	// Token: 0x040078ED RID: 30957
	[PublicizedFrom(EAccessModifier.Private)]
	public T5 oldValue5;

	// Token: 0x040078EE RID: 30958
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T5> comparer5 = EqualityComparer<T5>.Default;
}
