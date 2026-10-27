using System;
using System.Collections.Generic;

// Token: 0x020013E8 RID: 5096
public class CachedStringFormatter<T1, T2, T3, T4, T5>
{
	// Token: 0x06009FEA RID: 40938 RVA: 0x003C5B90 File Offset: 0x003C3D90
	public CachedStringFormatter(global::Func<T1, T2, T3, T4, T5, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009FEB RID: 40939 RVA: 0x003C5BE4 File Offset: 0x003C3DE4
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, T5 _v5)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, _v5, out flag);
	}

	// Token: 0x06009FEC RID: 40940 RVA: 0x003C5C00 File Offset: 0x003C3E00
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, T5 _v5, out bool _valueChanged)
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
		if (!this.comparer4.Equals(this.oldValue4, _v4))
		{
			this.oldValue4 = _v4;
			_valueChanged = true;
		}
		if (!this.comparer5.Equals(this.oldValue5, _v5))
		{
			this.oldValue5 = _v5;
			_valueChanged = true;
		}
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1, _v2, _v3, _v4, _v5);
		}
		return this.cachedResult;
	}

	// Token: 0x04007924 RID: 31012
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly global::Func<T1, T2, T3, T4, T5, string> formatter;

	// Token: 0x04007925 RID: 31013
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007926 RID: 31014
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x04007927 RID: 31015
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x04007928 RID: 31016
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x04007929 RID: 31017
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x0400792A RID: 31018
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x0400792B RID: 31019
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x0400792C RID: 31020
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x0400792D RID: 31021
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;

	// Token: 0x0400792E RID: 31022
	[PublicizedFrom(EAccessModifier.Private)]
	public T5 oldValue5;

	// Token: 0x0400792F RID: 31023
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T5> comparer5 = EqualityComparer<T5>.Default;
}
