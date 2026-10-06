using System;

// Token: 0x02001103 RID: 4355
public abstract class BindingItem
{
	// Token: 0x06008A20 RID: 35360 RVA: 0x0034C529 File Offset: 0x0034A729
	[PublicizedFrom(EAccessModifier.Protected)]
	public BindingItem(string _sourceText)
	{
		this.SourceText = _sourceText;
		this.fieldName = _sourceText.Substring(1, _sourceText.Length - 2);
	}

	// Token: 0x06008A21 RID: 35361
	public abstract string GetValue();

	// Token: 0x06008A22 RID: 35362 RVA: 0x0034C54D File Offset: 0x0034A74D
	public virtual bool TryGetTypedValue(out object _value)
	{
		_value = null;
		return false;
	}

	// Token: 0x040066D1 RID: 26321
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fieldName;

	// Token: 0x040066D2 RID: 26322
	public readonly string SourceText;
}
