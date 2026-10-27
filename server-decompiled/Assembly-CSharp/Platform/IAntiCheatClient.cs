using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BB7 RID: 7095
	public interface IAntiCheatClient : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D436 RID: 54326
		void Init(IPlatform _owner);

		// Token: 0x0600D437 RID: 54327
		bool ClientAntiCheatEnabled();

		// Token: 0x0600D438 RID: 54328
		bool GetUnhandledViolationMessage(out string _message);

		// Token: 0x0600D439 RID: 54329
		void WaitForRemoteAuth(Action onRemoteAuthSkippedOrComplete);

		// Token: 0x0600D43A RID: 54330
		void ConnectToServer([TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> hostUserAndToken, Action onNoAntiCheatOrConnectionComplete, Action<string> onConnectionFailed);

		// Token: 0x0600D43B RID: 54331
		void HandleMessageFromServer(byte[] _data);

		// Token: 0x0600D43C RID: 54332
		void DisconnectFromServer();

		// Token: 0x0600D43D RID: 54333
		void Destroy();
	}
}
