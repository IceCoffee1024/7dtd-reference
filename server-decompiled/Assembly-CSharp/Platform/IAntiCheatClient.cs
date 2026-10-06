using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B86 RID: 7046
	public interface IAntiCheatClient : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D322 RID: 54050
		void Init(IPlatform _owner);

		// Token: 0x0600D323 RID: 54051
		bool ClientAntiCheatEnabled();

		// Token: 0x0600D324 RID: 54052
		bool GetUnhandledViolationMessage(out string _message);

		// Token: 0x0600D325 RID: 54053
		void WaitForRemoteAuth(Action onRemoteAuthSkippedOrComplete);

		// Token: 0x0600D326 RID: 54054
		void ConnectToServer([TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> hostUserAndToken, Action onNoAntiCheatOrConnectionComplete, Action<string> onConnectionFailed);

		// Token: 0x0600D327 RID: 54055
		void HandleMessageFromServer(byte[] _data);

		// Token: 0x0600D328 RID: 54056
		void DisconnectFromServer();

		// Token: 0x0600D329 RID: 54057
		void Destroy();
	}
}
