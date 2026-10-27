using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CE3 RID: 7395
	public class AuthenticationClient : IAuthenticationClient
	{
		// Token: 0x0600DAE2 RID: 56034 RVA: 0x004E494B File Offset: 0x004E2B4B
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DAE3 RID: 56035 RVA: 0x004E4954 File Offset: 0x004E2B54
		public string GetAuthTicket()
		{
			if (!this.registeredDisconnectEvent)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.OnDisconnectFromServer += this.OnDisconnectFromServer;
				this.registeredDisconnectEvent = true;
			}
			byte[] array = new byte[1024];
			Log.Out("[Steamworks.NET] Auth.GetAuthTicket()");
			if (this.ticketHandle != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(this.ticketHandle);
				this.ticketHandle = HAuthTicket.Invalid;
			}
			SteamNetworkingIdentity steamNetworkingIdentity = new SteamNetworkingIdentity
			{
				m_eType = ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_Invalid
			};
			uint num;
			this.ticketHandle = SteamUser.GetAuthSessionTicket(array, array.Length, out num, ref steamNetworkingIdentity);
			return Convert.ToBase64String(array);
		}

		// Token: 0x0600DAE4 RID: 56036 RVA: 0x004E49EE File Offset: 0x004E2BEE
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisconnectFromServer()
		{
			if (this.ticketHandle != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(this.ticketHandle);
				this.ticketHandle = HAuthTicket.Invalid;
			}
		}

		// Token: 0x0600DAE5 RID: 56037 RVA: 0x004E4A18 File Offset: 0x004E2C18
		public void AuthenticateServer(ClientAuthenticateServerContext _context)
		{
			_context.Success();
		}

		// Token: 0x0600DAE6 RID: 56038 RVA: 0x004E4A20 File Offset: 0x004E2C20
		public void Destroy()
		{
			this.OnDisconnectFromServer();
		}

		// Token: 0x0400A615 RID: 42517
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A616 RID: 42518
		[PublicizedFrom(EAccessModifier.Private)]
		public HAuthTicket ticketHandle = HAuthTicket.Invalid;

		// Token: 0x0400A617 RID: 42519
		[PublicizedFrom(EAccessModifier.Private)]
		public bool registeredDisconnectEvent;
	}
}
