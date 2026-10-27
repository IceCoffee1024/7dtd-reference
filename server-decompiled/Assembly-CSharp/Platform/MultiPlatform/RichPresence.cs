using System;

namespace Platform.MultiPlatform
{
	// Token: 0x02001D21 RID: 7457
	public class RichPresence : IRichPresence
	{
		// Token: 0x0600DD0E RID: 56590 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DD0F RID: 56591 RVA: 0x004EB632 File Offset: 0x004E9832
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
