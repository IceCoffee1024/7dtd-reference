using System;
using System.Collections.Generic;

// Token: 0x02001395 RID: 5013
public class CachedStringFormatter<T1, T2>
{
	// Token: 0x06009DD5 RID: 40405 RVA: 0x003B960E File Offset: 0x003B780E
	public CachedStringFormatter(Func<T1, T2, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009DD6 RID: 40406 RVA: 0x003B9634 File Offset: 0x003B7834
	public string Format(T1 _v1, T2 _v2)
	{
		bool flag;
		return this.Format(_v1, _v2, out flag);
	}

	// Token: 0x06009DD7 RID: 40407 RVA: 0x003B964C File Offset: 0x003B784C
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

	// Token: 0x04007777 RID: 30583
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Func<T1, T2, string> formatter;

	// Token: 0x04007778 RID: 30584
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x04007779 RID: 30585
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x0400777A RID: 30586
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;

	// Token: 0x0400777B RID: 30587
	[PublicizedFrom(EAccessModifier.Private)]
	public T2 oldValue2;

	// Token: 0x0400777C RID: 30588
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T2> comparer2 = EqualityComparer<T2>.Default;
}
