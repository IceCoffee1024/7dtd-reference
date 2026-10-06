using System;
using System.Collections.Generic;

// Token: 0x020013CF RID: 5071
public class CachedStringFormatter<T1>
{
	// Token: 0x06009F7A RID: 40826 RVA: 0x003C3469 File Offset: 0x003C1669
	public CachedStringFormatter(Func<T1, string> _formatterFunc)
	{
		this.formatter = _formatterFunc;
	}

	// Token: 0x06009F7B RID: 40827 RVA: 0x003C3484 File Offset: 0x003C1684
	public string Format(T1 _v1)
	{
		bool flag;
		return this.Format(_v1, out flag);
	}

	// Token: 0x06009F7C RID: 40828 RVA: 0x003C349C File Offset: 0x003C169C
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

	// Token: 0x040078C7 RID: 30919
	[PublicizedFrom(EAccessModifier.Protected)]
	public Func<T1, string> formatter;

	// Token: 0x040078C8 RID: 30920
	[PublicizedFrom(EAccessModifier.Private)]
	public string cachedResult;

	// Token: 0x040078C9 RID: 30921
	[PublicizedFrom(EAccessModifier.Private)]
	public T1 oldValue1;

	// Token: 0x040078CA RID: 30922
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEqualityComparer<T1> comparer1 = EqualityComparer<T1>.Default;
}
