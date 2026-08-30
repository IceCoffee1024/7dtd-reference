using System;

namespace Platform
{
	// Token: 0x02001B76 RID: 7030
	public interface IRichPresence
	{
		// Token: 0x0600D198 RID: 53656
		void Init(IPlatform _owner);

		// Token: 0x0600D199 RID: 53657
		void UpdateRichPresence(IRichPresence.PresenceStates _state);

		// Token: 0x02001B77 RID: 7031
		public enum PresenceStates
		{
			// Token: 0x04009FAD RID: 40877
			Menu,
			// Token: 0x04009FAE RID: 40878
			Loading,
			// Token: 0x04009FAF RID: 40879
			Connecting,
			// Token: 0x04009FB0 RID: 40880
			InGame
		}
	}
}
