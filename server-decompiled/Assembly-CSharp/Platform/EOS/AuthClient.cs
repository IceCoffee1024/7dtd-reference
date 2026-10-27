using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D49 RID: 7497
	public class AuthClient : IAuthenticationClient
	{
		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x0600DE2F RID: 56879 RVA: 0x004F0235 File Offset: 0x004EE435
		public ConnectInterface connectInterface
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((Api)this.owner.Api).ConnectInterface;
			}
		}

		// Token: 0x0600DE30 RID: 56880 RVA: 0x004F024C File Offset: 0x004EE44C
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DE31 RID: 56881 RVA: 0x004F0258 File Offset: 0x004EE458
		public string GetAuthTicket()
		{
			EosHelpers.AssertMainThread("ACl.Get");
			CopyIdTokenOptions copyIdTokenOptions = new CopyIdTokenOptions
			{
				LocalUserId = ((UserIdentifierEos)this.owner.User.PlatformUserId).ProductUserId
			};
			object lockObject = AntiCheatCommon.LockObject;
			IdToken? idToken;
			Result result;
			lock (lockObject)
			{
				result = this.connectInterface.CopyIdToken(ref copyIdTokenOptions, out idToken);
			}
			Log.Out(string.Format("[EOS] CopyIdToken result: {0}", result));
			return (idToken != null) ? idToken.GetValueOrDefault().JsonWebToken : null;
		}

		// Token: 0x0600DE32 RID: 56882 RVA: 0x004F0314 File Offset: 0x004EE514
		public void AuthenticateServer(ClientAuthenticateServerContext _context)
		{
			AuthClient.<>c__DisplayClass5_0 CS$<>8__locals1 = new AuthClient.<>c__DisplayClass5_0();
			CS$<>8__locals1._context = _context;
			EosHelpers.AssertMainThread("ACl.Auth");
			if (PermissionsManager.IsCrossplayAllowed())
			{
				CS$<>8__locals1._context.Success();
				return;
			}
			if (CS$<>8__locals1._context.GameServerInfo.AllowsCrossplay)
			{
				Log.Error("[EOS] [ACl.Auth] Cannot join server that has crossplay when we do not have crossplay permissions.");
				CS$<>8__locals1._context.DisconnectNoCrossplay();
				return;
			}
			if (EPlayGroupExtensions.Current == EPlayGroup.Standalone && (CS$<>8__locals1._context.GameServerInfo.PlayGroup == EPlayGroup.Standalone || CS$<>8__locals1._context.GameServerInfo.IsDedicated))
			{
				CS$<>8__locals1._context.Success();
				return;
			}
			PlatformUserIdentifierAbs crossplatformUserId = CS$<>8__locals1._context.CrossplatformUserId;
			CS$<>8__locals1.identifierEos = crossplatformUserId as UserIdentifierEos;
			if (CS$<>8__locals1.identifierEos == null)
			{
				Log.Warning(string.Format("[EOS] [ACl.Auth] Expected EOS Crossplatform ID? But got: {0}", CS$<>8__locals1._context.CrossplatformUserId));
				CS$<>8__locals1._context.DisconnectNoCrossplay();
				return;
			}
			IdToken idToken = new IdToken
			{
				JsonWebToken = CS$<>8__locals1.identifierEos.Ticket,
				ProductUserId = CS$<>8__locals1.identifierEos.ProductUserId
			};
			VerifyIdTokenOptions verifyIdTokenOptions = new VerifyIdTokenOptions
			{
				IdToken = new IdToken?(idToken)
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.connectInterface.VerifyIdToken(ref verifyIdTokenOptions, null, new OnVerifyIdTokenCallback(CS$<>8__locals1.<AuthenticateServer>g__VerifyIdTokenCallback|0));
			}
		}

		// Token: 0x0600DE33 RID: 56883 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A7B8 RID: 42936
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;
	}
}
