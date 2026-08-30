using System;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CBE RID: 7358
	public class RichPresence : IRichPresence
	{
		// Token: 0x0600DA80 RID: 55936 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600DA81 RID: 55937 RVA: 0x004E2F4A File Offset: 0x004E114A
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
