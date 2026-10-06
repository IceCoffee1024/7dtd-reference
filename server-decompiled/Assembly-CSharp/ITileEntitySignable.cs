using System;

// Token: 0x02000C73 RID: 3187
public interface ITileEntitySignable : ITileEntity, ILockTarget, IHasWorldAddress
{
	// Token: 0x06006074 RID: 24692
	void SetText(AuthoredText _authoredText, bool _syncData = true);

	// Token: 0x06006075 RID: 24693
	void SetText(string _text, bool _syncData = true, PlatformUserIdentifierAbs _signingPlayer = null);

	// Token: 0x06006076 RID: 24694
	AuthoredText GetAuthoredText();

	// Token: 0x06006077 RID: 24695
	bool CanRenderString(string _text);
}
