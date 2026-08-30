using System;

// Token: 0x02000C50 RID: 3152
public interface ITileEntitySignable : ITileEntity, ILockTarget
{
	// Token: 0x06005FEC RID: 24556
	void SetText(AuthoredText _authoredText, bool _syncData = true);

	// Token: 0x06005FED RID: 24557
	void SetText(string _text, bool _syncData = true, PlatformUserIdentifierAbs _signingPlayer = null);

	// Token: 0x06005FEE RID: 24558
	AuthoredText GetAuthoredText();

	// Token: 0x06005FEF RID: 24559
	bool CanRenderString(string _text);
}
