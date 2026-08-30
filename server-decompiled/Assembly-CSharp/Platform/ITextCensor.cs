using System;

namespace Platform
{
	// Token: 0x02001BA5 RID: 7077
	public interface ITextCensor
	{
		// Token: 0x0600D33B RID: 54075
		void Init(IPlatform _owner);

		// Token: 0x0600D33C RID: 54076
		void Update();

		// Token: 0x0600D33D RID: 54077
		void CensorProfanity(string _input, PlatformUserIdentifierAbs _author, Action<CensoredTextResult> _censoredCallback);
	}
}
