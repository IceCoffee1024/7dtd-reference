using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B54 RID: 6996
	public interface IAntiCheatClient : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D1AB RID: 53675
		void Init(IPlatform _owner);

		// Token: 0x0600D1AC RID: 53676
		bool ClientAntiCheatEnabled();

		// Token: 0x0600D1AD RID: 53677
		bool GetUnhandledViolationMessage(out string _message);

		// Token: 0x0600D1AE RID: 53678
		void WaitForRemoteAuth(Action onRemoteAuthSkippedOrComplete);

		// Token: 0x0600D1AF RID: 53679
		void ConnectToServer([TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> hostUserAndToken, Action onNoAntiCheatOrConnectionComplete, Action<string> onConnectionFailed);

		// Token: 0x0600D1B0 RID: 53680
		void HandleMessageFromServer(byte[] _data);

		// Token: 0x0600D1B1 RID: 53681
		void DisconnectFromServer();

		// Token: 0x0600D1B2 RID: 53682
		void Destroy();
	}
}
