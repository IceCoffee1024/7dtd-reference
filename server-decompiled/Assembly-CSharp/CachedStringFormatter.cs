using System;
using System.Collections.Generic;

// Token: 0x02001394 RID: 5012
public class CachedStringFormatter<T1>
{
	// Token: 0x06009DD2 RID: 40402 RVA: 0x003B9589 File Offset: 0x003B7789
	public CachedStringFormatter(Func<T1, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009DD3 RID: 40403 RVA: 0x003B95A4 File Offset: 0x003B77A4
	public string Format(T1 _v1)
	{
		bool flag;
		return this.Format(_v1, out flag);
	}

	// Token: 0x06009DD4 RID: 40404 RVA: 0x003B95BC File Offset: 0x003B77BC
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

	// Token: 0x04007773 RID: 30579
	[PublicizedFrom(EAccessModifier.Protected)]
	public Func<T1, string> formatter;

	// Token: 0x04007774 RID: 30580
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007775 RID: 30581
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007776 RID: 30582
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;
}
