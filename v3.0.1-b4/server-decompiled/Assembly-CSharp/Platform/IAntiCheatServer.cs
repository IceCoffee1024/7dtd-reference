using System;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B2F RID: 6959
	public interface IAntiCheatServer : IAntiCheatEncryption, IEncryptionModule
	{
		// Token: 0x0600D02B RID: 53291
		void Init(IPlatform _owner);

		// Token: 0x0600D02C RID: 53292
		void Update();

		// Token: 0x0600D02D RID: 53293
		bool StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D02E RID: 53294
		bool RegisterUser(ClientInfo _client);

		// Token: 0x0600D02F RID: 53295
		void FreeUser(ClientInfo _client);

		// Token: 0x0600D030 RID: 53296
		void HandleMessageFromClient(ClientInfo _cInfo, byte[] _data);

		// Token: 0x0600D031 RID: 53297
		void StopServer();

		// Token: 0x0600D032 RID: 53298
		void Destroy();

		// Token: 0x0600D033 RID: 53299
		bool ServerEacEnabled();

		// Token: 0x0600D034 RID: 53300
		bool ServerEacAvailable();

		// Token: 0x0600D035 RID: 53301
		bool GetHostUserIdAndToken([TupleElementNames(new string[] { "userId", "token" })] out ValueTuple<PlatformUserIdentifierAbs, string> _hostUserIdAndToken);
	}
}
