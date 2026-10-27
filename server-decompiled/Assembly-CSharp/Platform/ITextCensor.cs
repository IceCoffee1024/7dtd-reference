using System;

namespace Platform
{
	// Token: 0x02001C08 RID: 7176
	public interface ITextCensor
	{
		// Token: 0x0600D5C7 RID: 54727
		void Init(IPlatform _owner);

		// Token: 0x0600D5C8 RID: 54728
		void Update();

		// Token: 0x0600D5C9 RID: 54729
		void CensorProfanity(string _input, PlatformUserIdentifierAbs _author, Action<CensoredTextResult> _censoredCallback);
	}
}
