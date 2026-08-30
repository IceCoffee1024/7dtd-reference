using System;

namespace Platform
{
	// Token: 0x02001B7E RID: 7038
	public interface ITextCensor
	{
		// Token: 0x0600D1AF RID: 53679
		void Init(IPlatform _owner);

		// Token: 0x0600D1B0 RID: 53680
		void Update();

		// Token: 0x0600D1B1 RID: 53681
		void CensorProfanity(string _input, PlatformUserIdentifierAbs _author, Action<CensoredTextResult> _censoredCallback);
	}
}
