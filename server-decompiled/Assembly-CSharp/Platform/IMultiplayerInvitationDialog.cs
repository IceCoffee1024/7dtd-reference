using System;

namespace Platform
{
	// Token: 0x02001BCE RID: 7118
	public interface IMultiplayerInvitationDialog
	{
		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600D4A0 RID: 54432
		bool CanShow { get; }

		// Token: 0x0600D4A1 RID: 54433
		void Init(IPlatform owner);

		// Token: 0x0600D4A2 RID: 54434
		void ShowInviteDialog();
	}
}
