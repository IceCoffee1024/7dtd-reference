using System;
using System.Collections.Generic;

// Token: 0x020013E3 RID: 5091
public class CachedStringFormatter<T1, T2, T3>
{
	// Token: 0x06009FD8 RID: 40920 RVA: 0x003C59A1 File Offset: 0x003C3BA1
	public CachedStringFormatter(Func<T1, T2, T3, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009FD9 RID: 40921 RVA: 0x003C59D4 File Offset: 0x003C3BD4
	public string Format(T1 _v1, T2 _v2, T3 _v3)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, out flag);
	}

	// Token: 0x06009FDA RID: 40922 RVA: 0x003C59EC File Offset: 0x003C3BEC
	public string Format(T1 _v1, T2 _v2, T3 _v3, out bool _valueChanged)
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
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1, _v2, _v3);
		}
		return this.cachedResult;
	}

	// Token: 0x04007912 RID: 30994
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, string> formatter;

	// Token: 0x04007913 RID: 30995
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007914 RID: 30996
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007915 RID: 30997
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007916 RID: 30998
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x04007917 RID: 30999
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x04007918 RID: 31000
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x04007919 RID: 31001
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;
}
