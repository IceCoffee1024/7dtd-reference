using System;

namespace Platform
{
	// Token: 0x02001B9D RID: 7069
	public interface IRichPresence
	{
		// Token: 0x0600D324 RID: 54052
		void Init(IPlatform _owner);

		// Token: 0x0600D325 RID: 54053
		void UpdateRichPresence(IRichPresence.PresenceStates _state);

		// Token: 0x02001B9E RID: 7070
		public enum PresenceStates
		{
			// Token: 0x0400A0DE RID: 41182
			Menu,
			// Token: 0x0400A0DF RID: 41183
			Loading,
			// Token: 0x0400A0E0 RID: 41184
			Connecting,
			// Token: 0x0400A0E1 RID: 41185
			InGame
		}
	}
}
