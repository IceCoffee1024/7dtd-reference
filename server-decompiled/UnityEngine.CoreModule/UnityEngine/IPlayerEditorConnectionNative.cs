using System;

namespace UnityEngine
{
	// Token: 0x02000205 RID: 517
	internal interface IPlayerEditorConnectionNative
	{
		// Token: 0x06001768 RID: 5992
		void Initialize();

		// Token: 0x06001769 RID: 5993
		void DisconnectAll();

		// Token: 0x0600176A RID: 5994
		void SendMessage(Guid messageId, byte[] data, int playerId);

		// Token: 0x0600176B RID: 5995
		bool TrySendMessage(Guid messageId, byte[] data, int playerId);

		// Token: 0x0600176C RID: 5996
		void Poll();

		// Token: 0x0600176D RID: 5997
		void RegisterInternal(Guid messageId);

		// Token: 0x0600176E RID: 5998
		void UnregisterInternal(Guid messageId);

		// Token: 0x0600176F RID: 5999
		bool IsConnected();
	}
}
