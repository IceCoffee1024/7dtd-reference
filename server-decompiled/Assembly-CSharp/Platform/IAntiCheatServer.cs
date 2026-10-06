using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B88 RID: 7048
	public interface IAntiCheatServer : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D32B RID: 54059
		void Init(IPlatform _owner);

		// Token: 0x0600D32C RID: 54060
		void Update();

		// Token: 0x0600D32D RID: 54061
		bool StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D32E RID: 54062
		bool RegisterUser(ClientInfo _client);

		// Token: 0x0600D32F RID: 54063
		void FreeUser(ClientInfo _client);

		// Token: 0x0600D330 RID: 54064
		void HandleMessageFromClient(ClientInfo _cInfo, byte[] _data);

		// Token: 0x0600D331 RID: 54065
		void StopServer();

		// Token: 0x0600D332 RID: 54066
		void Destroy();

		// Token: 0x0600D333 RID: 54067
		bool ServerEacEnabled();

		// Token: 0x0600D334 RID: 54068
		bool ServerEacAvailable();

		// Token: 0x0600D335 RID: 54069
		bool GetHostUserIdAndToken([TupleElementNames(new string[] { "userId", "token" })] out ValueTuple<PlatformUserIdentifierAbs, string> _hostUserIdAndToken);
	}
}
