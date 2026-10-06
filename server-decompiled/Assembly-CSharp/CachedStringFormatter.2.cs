using System;
using System.Collections.Generic;

// Token: 0x020013D0 RID: 5072
public class CachedStringFormatter<T1, T2>
{
	// Token: 0x06009F7D RID: 40829 RVA: 0x003C34EE File Offset: 0x003C16EE
	public CachedStringFormatter(Func<T1, T2, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009F7E RID: 40830 RVA: 0x003C3514 File Offset: 0x003C1714
	public string Format(T1 _v1, T2 _v2)
	{
		bool flag;
		return this.Format(_v1, _v2, out flag);
	}

	// Token: 0x06009F7F RID: 40831 RVA: 0x003C352C File Offset: 0x003C172C
	public string Format(T1 _v1, T2 _v2, out bool _valueChanged)
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
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1, _v2);
		}
		return this.cachedResult;
	}

	// Token: 0x040078CB RID: 30923
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, string> formatter;

	// Token: 0x040078CC RID: 30924
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x040078CD RID: 30925
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x040078CE RID: 30926
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x040078CF RID: 30927
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x040078D0 RID: 30928
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;
}
