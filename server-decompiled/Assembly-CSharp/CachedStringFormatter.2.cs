using System;
using System.Collections.Generic;

// Token: 0x020013E2 RID: 5090
public class CachedStringFormatter<T1, T2>
{
	// Token: 0x06009FD5 RID: 40917 RVA: 0x003C58F2 File Offset: 0x003C3AF2
	public CachedStringFormatter(Func<T1, T2, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009FD6 RID: 40918 RVA: 0x003C5918 File Offset: 0x003C3B18
	public string Format(T1 _v1, T2 _v2)
	{
		bool flag;
		return this.Format(_v1, _v2, out flag);
	}

	// Token: 0x06009FD7 RID: 40919 RVA: 0x003C5930 File Offset: 0x003C3B30
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

	// Token: 0x0400790C RID: 30988
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, string> formatter;

	// Token: 0x0400790D RID: 30989
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x0400790E RID: 30990
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x0400790F RID: 30991
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007910 RID: 30992
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x04007911 RID: 30993
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;
}
