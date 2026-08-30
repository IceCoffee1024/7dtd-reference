using System;

namespace Platform
{
	// Token: 0x02001B6B RID: 7019
	public interface IMultiplayerInvitationDialog
	{
		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x0600D214 RID: 53780
		bool CanShow { get; }

		// Token: 0x0600D215 RID: 53781
		void Init(IPlatform owner);

		// Token: 0x0600D216 RID: 53782
		void ShowInviteDialog();
	}
}
