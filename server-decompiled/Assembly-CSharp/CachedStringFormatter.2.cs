using System;
using System.Collections.Generic;

// Token: 0x020013B0 RID: 5040
public class CachedStringFormatter<T1, T2>
{
	// Token: 0x06009ED2 RID: 40658 RVA: 0x003BE952 File Offset: 0x003BCB52
	public CachedStringFormatter(Func<T1, T2, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009ED3 RID: 40659 RVA: 0x003BE978 File Offset: 0x003BCB78
	public string Format(T1 _v1, T2 _v2)
	{
		bool flag;
		return this.Format(_v1, _v2, out flag);
	}

	// Token: 0x06009ED4 RID: 40660 RVA: 0x003BE990 File Offset: 0x003BCB90
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

	// Token: 0x04007850 RID: 30800
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, string> formatter;

	// Token: 0x04007851 RID: 30801
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007852 RID: 30802
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007853 RID: 30803
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007854 RID: 30804
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x04007855 RID: 30805
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;
}
