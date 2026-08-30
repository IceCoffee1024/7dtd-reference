using System;

// Token: 0x020010D9 RID: 4313
public abstract class BindingItem
{
	// Token: 0x060088DE RID: 35038 RVA: 0x00343D75 File Offset: 0x00341F75
	[PublicizedFrom(EAccessModifier.Protected)]
	public BindingItem(string _sourceText)
	{
		this.SourceText = _sourceText;
		this.fieldName = _sourceText.Substring(1, _sourceText.Length - 2);
	}

	// Token: 0x060088DF RID: 35039
	public abstract string GetValue();

	// Token: 0x040065C3 RID: 26051
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fieldName;

	// Token: 0x040065C4 RID: 26052
	public readonly string SourceText;
}
