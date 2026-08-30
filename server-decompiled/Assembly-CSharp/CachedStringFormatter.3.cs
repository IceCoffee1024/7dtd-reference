using System;
using System.Collections.Generic;

// Token: 0x020013B1 RID: 5041
public class CachedStringFormatter<T1, T2, T3>
{
	// Token: 0x06009ED5 RID: 40661 RVA: 0x003BEA01 File Offset: 0x003BCC01
	public CachedStringFormatter(Func<T1, T2, T3, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009ED6 RID: 40662 RVA: 0x003BEA34 File Offset: 0x003BCC34
	public string Format(T1 _v1, T2 _v2, T3 _v3)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, out flag);
	}

	// Token: 0x06009ED7 RID: 40663 RVA: 0x003BEA4C File Offset: 0x003BCC4C
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

	// Token: 0x04007856 RID: 30806
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, string> formatter;

	// Token: 0x04007857 RID: 30807
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007858 RID: 30808
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007859 RID: 30809
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x0400785A RID: 30810
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x0400785B RID: 30811
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x0400785C RID: 30812
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x0400785D RID: 30813
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;
}
