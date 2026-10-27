using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BB9 RID: 7097
	public interface IAntiCheatServer : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D43F RID: 54335
		void Init(IPlatform _owner);

		// Token: 0x0600D440 RID: 54336
		void Update();

		// Token: 0x0600D441 RID: 54337
		bool StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D442 RID: 54338
		bool RegisterUser(ClientInfo _client);

		// Token: 0x0600D443 RID: 54339
		void FreeUser(ClientInfo _client);

		// Token: 0x0600D444 RID: 54340
		void HandleMessageFromClient(ClientInfo _cInfo, byte[] _data);

		// Token: 0x0600D445 RID: 54341
		void StopServer();

		// Token: 0x0600D446 RID: 54342
		void Destroy();

		// Token: 0x0600D447 RID: 54343
		bool ServerEacEnabled();

		// Token: 0x0600D448 RID: 54344
		bool ServerEacAvailable();

		// Token: 0x0600D449 RID: 54345
		bool GetHostUserIdAndToken([TupleElementNames(new string[] { "userId", "token" })] out ValueTuple<PlatformUserIdentifierAbs, string> _hostUserIdAndToken);
	}
}
