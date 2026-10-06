using System;

namespace Platform
{
	// Token: 0x02001BCF RID: 7119
	public interface IRichPresence
	{
		// Token: 0x0600D49C RID: 54428
		void Init(IPlatform _owner);

		// Token: 0x0600D49D RID: 54429
		void UpdateRichPresence(IRichPresence.PresenceStates _state);

		// Token: 0x02001BD0 RID: 7120
		public enum PresenceStates
		{
			// Token: 0x0400A1B3 RID: 41395
			Menu,
			// Token: 0x0400A1B4 RID: 41396
			Loading,
			// Token: 0x0400A1B5 RID: 41397
			Connecting,
			// Token: 0x0400A1B6 RID: 41398
			InGame
		}
	}
}
