using System;

// Token: 0x02000C62 RID: 3170
public interface ITileEntitySignable : ITileEntity, ILockTarget
{
	// Token: 0x06006076 RID: 24694
	void SetText(AuthoredText _authoredText, bool _syncData = true);

	// Token: 0x06006077 RID: 24695
	void SetText(string _text, bool _syncData = true, PlatformUserIdentifierAbs _signingPlayer = null);

	// Token: 0x06006078 RID: 24696
	AuthoredText GetAuthoredText();

	// Token: 0x06006079 RID: 24697
	bool CanRenderString(string _text);
}
