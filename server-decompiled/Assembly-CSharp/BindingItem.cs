using System;

// Token: 0x02001113 RID: 4371
public abstract class BindingItem
{
	// Token: 0x06008A74 RID: 35444 RVA: 0x0034E5D9 File Offset: 0x0034C7D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public BindingItem(string _sourceText)
	{
		this.SourceText = _sourceText;
		this.fieldName = _sourceText.Substring(1, _sourceText.Length - 2);
	}

	// Token: 0x06008A75 RID: 35445
	public abstract string GetValue();

	// Token: 0x06008A76 RID: 35446 RVA: 0x0034E5FD File Offset: 0x0034C7FD
	public virtual bool TryGetTypedValue(out object _value)
	{
		_value = null;
		return false;
	}

	// Token: 0x04006711 RID: 26385
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fieldName;

	// Token: 0x04006712 RID: 26386
	public readonly string SourceText;
}
