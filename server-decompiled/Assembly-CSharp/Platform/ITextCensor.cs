using System;

namespace Platform
{
	// Token: 0x02001BD7 RID: 7127
	public interface ITextCensor
	{
		// Token: 0x0600D4B3 RID: 54451
		void Init(IPlatform _owner);

		// Token: 0x0600D4B4 RID: 54452
		void Update();

		// Token: 0x0600D4B5 RID: 54453
		void CensorProfanity(string _input, PlatformUserIdentifierAbs _author, Action<CensoredTextResult> _censoredCallback);
	}
}
