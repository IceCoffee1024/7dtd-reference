using System;

namespace Platform
{
	// Token: 0x02001B44 RID: 6980
	public interface IMultiplayerInvitationDialog
	{
		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x0600D089 RID: 53385
		bool CanShow { get; }

		// Token: 0x0600D08A RID: 53386
		void Init(IPlatform owner);

		// Token: 0x0600D08B RID: 53387
		void ShowInviteDialog();
	}
}
