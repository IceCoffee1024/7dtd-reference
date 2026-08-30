using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B2D RID: 6957
	public interface IAntiCheatClient : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D022 RID: 53282
		void Init(IPlatform _owner);

		// Token: 0x0600D023 RID: 53283
		bool ClientAntiCheatEnabled();

		// Token: 0x0600D024 RID: 53284
		bool GetUnhandledViolationMessage(out string _message);

		// Token: 0x0600D025 RID: 53285
		void WaitForRemoteAuth(Action onRemoteAuthSkippedOrComplete);

		// Token: 0x0600D026 RID: 53286
		void ConnectToServer([TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> hostUserAndToken, Action onNoAntiCheatOrConnectionComplete, Action<string> onConnectionFailed);

		// Token: 0x0600D027 RID: 53287
		void HandleMessageFromServer(byte[] _data);

		// Token: 0x0600D028 RID: 53288
		void DisconnectFromServer();

		// Token: 0x0600D029 RID: 53289
		void Destroy();
	}
}
