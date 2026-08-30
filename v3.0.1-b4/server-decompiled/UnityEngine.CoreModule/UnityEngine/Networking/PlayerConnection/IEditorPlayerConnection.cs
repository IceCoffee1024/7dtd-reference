using System;
using UnityEngine.Events;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x020003CF RID: 975
	public interface IEditorPlayerConnection
	{
		// Token: 0x06002117 RID: 8471
		void Register(Guid messageId, UnityAction<MessageEventArgs> callback);

		// Token: 0x06002118 RID: 8472
		void Unregister(Guid messageId, UnityAction<MessageEventArgs> callback);

		// Token: 0x06002119 RID: 8473
		void DisconnectAll();

		// Token: 0x0600211A RID: 8474
		void RegisterConnection(UnityAction<int> callback);

		// Token: 0x0600211B RID: 8475
		void RegisterDisconnection(UnityAction<int> callback);

		// Token: 0x0600211C RID: 8476
		void UnregisterConnection(UnityAction<int> callback);

		// Token: 0x0600211D RID: 8477
		void UnregisterDisconnection(UnityAction<int> callback);

		// Token: 0x0600211E RID: 8478
		void Send(Guid messageId, byte[] data);

		// Token: 0x0600211F RID: 8479
		bool TrySend(Guid messageId, byte[] data);
	}
}
