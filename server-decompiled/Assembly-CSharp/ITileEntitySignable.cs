using System;

// Token: 0x02000C7E RID: 3198
public interface ITileEntitySignable : ITileEntity, ILockTarget, IHasWorldAddress
{
	// Token: 0x06006099 RID: 24729
	void SetText(AuthoredText _authoredText, bool _syncData = true);

	// Token: 0x0600609A RID: 24730
	void SetText(string _text, bool _syncData = true, PlatformUserIdentifierAbs _signingPlayer = null);

	// Token: 0x0600609B RID: 24731
	AuthoredText GetAuthoredText();

	// Token: 0x0600609C RID: 24732
	bool CanRenderString(string _text);
}
