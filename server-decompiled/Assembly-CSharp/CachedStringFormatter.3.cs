using System;
using System.Collections.Generic;

// Token: 0x020013D1 RID: 5073
public class CachedStringFormatter<T1, T2, T3>
{
	// Token: 0x06009F80 RID: 40832 RVA: 0x003C359D File Offset: 0x003C179D
	public CachedStringFormatter(Func<T1, T2, T3, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009F81 RID: 40833 RVA: 0x003C35D0 File Offset: 0x003C17D0
	public string Format(T1 _v1, T2 _v2, T3 _v3)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, out flag);
	}

	// Token: 0x06009F82 RID: 40834 RVA: 0x003C35E8 File Offset: 0x003C17E8
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

	// Token: 0x040078D1 RID: 30929
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, string> formatter;

	// Token: 0x040078D2 RID: 30930
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x040078D3 RID: 30931
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x040078D4 RID: 30932
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x040078D5 RID: 30933
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x040078D6 RID: 30934
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x040078D7 RID: 30935
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x040078D8 RID: 30936
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;
}
