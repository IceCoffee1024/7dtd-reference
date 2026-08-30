using System;
using System.Collections.Generic;

// Token: 0x02001396 RID: 5014
public class CachedStringFormatter<T1, T2, T3>
{
	// Token: 0x06009DD8 RID: 40408 RVA: 0x003B96BD File Offset: 0x003B78BD
	public CachedStringFormatter(Func<T1, T2, T3, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009DD9 RID: 40409 RVA: 0x003B96F0 File Offset: 0x003B78F0
	public string Format(T1 _v1, T2 _v2, T3 _v3)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, out flag);
	}

	// Token: 0x06009DDA RID: 40410 RVA: 0x003B9708 File Offset: 0x003B7908
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

	// Token: 0x0400777D RID: 30589
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, string> formatter;

	// Token: 0x0400777E RID: 30590
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x0400777F RID: 30591
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007780 RID: 30592
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007781 RID: 30593
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x04007782 RID: 30594
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x04007783 RID: 30595
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x04007784 RID: 30596
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;
}
