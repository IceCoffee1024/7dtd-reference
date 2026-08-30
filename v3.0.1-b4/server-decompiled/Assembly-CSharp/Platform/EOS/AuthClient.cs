using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001CBF RID: 7359
	public class AuthClient : IAuthenticationClient
	{
		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x0600DA05 RID: 55813 RVA: 0x004E1539 File Offset: 0x004DF739
		public ConnectInterface connectInterface
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return ((Api)this.owner.Api).ConnectInterface;
			}
		}

		// Token: 0x0600DA06 RID: 55814 RVA: 0x004E1550 File Offset: 0x004DF750
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DA07 RID: 55815 RVA: 0x004E155C File Offset: 0x004DF75C
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

		// Token: 0x0600DA08 RID: 55816 RVA: 0x004E1618 File Offset: 0x004DF818
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

		// Token: 0x0600DA09 RID: 55817 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0400A4C7 RID: 42183
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;
	}
}
