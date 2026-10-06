using System;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CF0 RID: 7408
	public class RichPresence : IRichPresence
	{
		// Token: 0x0600DBFA RID: 56314 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DBFB RID: 56315 RVA: 0x004E7A92 File Offset: 0x004E5C92
		public void UpdateRichPresence(IRichPresence.PresenceStates _state)
		{
			IRichPresence richPresence = PlatformManager.NativePlatform.RichPresence;
			if (richPresence != null)
			{
				richPresence.UpdateRichPresence(_state);
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform == null)
			{
				return;
			}
			IRichPresence richPresence2 = crossplatformPlatform.RichPresence;
			if (richPresence2 == null)
			{
				return;
			}
			richPresence2.UpdateRichPresence(_state);
		}
	}
}
