using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B56 RID: 6998
	public interface IAntiCheatServer : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D1B4 RID: 53684
		void Init(IPlatform _owner);

		// Token: 0x0600D1B5 RID: 53685
		void Update();

		// Token: 0x0600D1B6 RID: 53686
		bool StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D1B7 RID: 53687
		bool RegisterUser(ClientInfo _client);

		// Token: 0x0600D1B8 RID: 53688
		void FreeUser(ClientInfo _client);

		// Token: 0x0600D1B9 RID: 53689
		void HandleMessageFromClient(ClientInfo _cInfo, byte[] _data);

		// Token: 0x0600D1BA RID: 53690
		void StopServer();

		// Token: 0x0600D1BB RID: 53691
		void Destroy();

		// Token: 0x0600D1BC RID: 53692
		bool ServerEacEnabled();

		// Token: 0x0600D1BD RID: 53693
		bool ServerEacAvailable();

		// Token: 0x0600D1BE RID: 53694
		bool GetHostUserIdAndToken([TupleElementNames(new string[] { "userId", "token" })] out ValueTuple<PlatformUserIdentifierAbs, string> _hostUserIdAndToken);
	}
}
