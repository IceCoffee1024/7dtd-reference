using System;
using System.Collections.Generic;

// Token: 0x020013D2 RID: 5074
public class CachedStringFormatter<T1, T2, T3, T4>
{
	// Token: 0x06009F83 RID: 40835 RVA: 0x003C367D File Offset: 0x003C187D
	public CachedStringFormatter(Func<T1, T2, T3, T4, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009F84 RID: 40836 RVA: 0x003C36B8 File Offset: 0x003C18B8
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4)
	{
		bool flag;
		return this.Format(_v1, _v2, _v3, _v4, out flag);
	}

	// Token: 0x06009F85 RID: 40837 RVA: 0x003C36D4 File Offset: 0x003C18D4
	public string Format(T1 _v1, T2 _v2, T3 _v3, T4 _v4, out bool _valueChanged)
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
		if (_valueChanged)
		{
			this.cachedResult = this.formatter(_v1, _v2, _v3, _v4);
		}
		return this.cachedResult;
	}

	// Token: 0x040078D9 RID: 30937
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, T3, T4, string> formatter;

	// Token: 0x040078DA RID: 30938
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x040078DB RID: 30939
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x040078DC RID: 30940
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x040078DD RID: 30941
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x040078DE RID: 30942
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;

	// Token: 0x040078DF RID: 30943
	[PublicizedFrom(EAccessModifier.Private)]
	public T3 oldValue3;

	// Token: 0x040078E0 RID: 30944
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T3> comparer3 = EqualityComparer<T3>.Default;

	// Token: 0x040078E1 RID: 30945
	[PublicizedFrom(EAccessModifier.Private)]
	public T4 oldValue4;

	// Token: 0x040078E2 RID: 30946
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T4> comparer4 = EqualityComparer<T4>.Default;
}
