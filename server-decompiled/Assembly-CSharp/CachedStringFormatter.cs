using System;
using System.Collections.Generic;

// Token: 0x020013E1 RID: 5089
public class CachedStringFormatter<T1>
{
	// Token: 0x06009FD2 RID: 40914 RVA: 0x003C586D File Offset: 0x003C3A6D
	public CachedStringFormatter(Func<T1, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009FD3 RID: 40915 RVA: 0x003C5888 File Offset: 0x003C3A88
	public string Format(T1 _v1)
	{
		bool flag;
		return this.Format(_v1, out flag);
	}

	// Token: 0x06009FD4 RID: 40916 RVA: 0x003C58A0 File Offset: 0x003C3AA0
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

	// Token: 0x04007908 RID: 30984
	[PublicizedFrom(EAccessModifier.Protected)]
	public Func<T1, string> formatter;

	// Token: 0x04007909 RID: 30985
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x0400790A RID: 30986
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x0400790B RID: 30987
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;
}
