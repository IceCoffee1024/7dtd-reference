using System;

namespace Platform
{
	// Token: 0x02001C00 RID: 7168
	public interface IRichPresence
	{
		// Token: 0x0600D5B0 RID: 54704
		void Init(IPlatform _owner);

		// Token: 0x0600D5B1 RID: 54705
		void UpdateRichPresence(IRichPresence.PresenceStates _state);

		// Token: 0x02001C01 RID: 7169
		public enum PresenceStates
		{
			// Token: 0x0400A29C RID: 41628
			Menu,
			// Token: 0x0400A29D RID: 41629
			Loading,
			// Token: 0x0400A29E RID: 41630
			Connecting,
			// Token: 0x0400A29F RID: 41631
			InGame
		}
	}
}
