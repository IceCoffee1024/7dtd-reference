using System;
using System.Collections.Generic;

// Token: 0x020013AF RID: 5039
public class CachedStringFormatter<T1>
{
	// Token: 0x06009ECF RID: 40655 RVA: 0x003BE8CD File Offset: 0x003BCACD
	public CachedStringFormatter(Func<T1, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009ED0 RID: 40656 RVA: 0x003BE8E8 File Offset: 0x003BCAE8
	public string Format(T1 _v1)
	{
		bool flag;
		return this.Format(_v1, out flag);
	}

	// Token: 0x06009ED1 RID: 40657 RVA: 0x003BE900 File Offset: 0x003BCB00
	public string Format(T1 _v1, out bool _valueChanged)
	{
		_valueChanged = this.cachedResult == null;
		if (!this.comparer1.Equals(this.oldValue1, _v1))
		{
			this.oldValue1 = _v1;
			_valueChanged = true;
		}
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1);
		}
		return this.cachedResult;
	}

	// Token: 0x0400784C RID: 30796
	[PublicizedFrom(EAccessModifier.Protected)]
	public Func<T1, string> formatter;

	// Token: 0x0400784D RID: 30797
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x0400784E RID: 30798
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x0400784F RID: 30799
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;
}
