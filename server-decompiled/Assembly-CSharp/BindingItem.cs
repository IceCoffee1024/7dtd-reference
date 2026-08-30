using System;

// Token: 0x020010F2 RID: 4338
public abstract class BindingItem
{
	// Token: 0x060089D0 RID: 35280 RVA: 0x00348599 File Offset: 0x00346799
	[PublicizedFrom(EAccessModifier.Protected)]
	public BindingItem(string _sourceText)
	{
		this.SourceText = _sourceText;
		this.fieldName = _sourceText.Substring(1, _sourceText.Length - 2);
	}

	// Token: 0x060089D1 RID: 35281
	public abstract string GetValue();

	// Token: 0x0400668A RID: 26250
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fieldName;

	// Token: 0x0400668B RID: 26251
	public readonly string SourceText;
}
