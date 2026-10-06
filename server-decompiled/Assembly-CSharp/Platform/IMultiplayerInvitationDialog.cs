using System;

namespace Platform
{
	// Token: 0x02001B9D RID: 7069
	public interface IMultiplayerInvitationDialog
	{
		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x0600D38C RID: 54156
		bool CanShow { get; }

		// Token: 0x0600D38D RID: 54157
		void Init(IPlatform owner);

		// Token: 0x0600D38E RID: 54158
		void ShowInviteDialog();
	}
}
