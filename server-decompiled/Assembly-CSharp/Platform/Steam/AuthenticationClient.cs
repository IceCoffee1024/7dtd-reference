using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C59 RID: 7257
	public class AuthenticationClient : IAuthenticationClient
	{
		// Token: 0x0600D6C5 RID: 54981 RVA: 0x004D5E9B File Offset: 0x004D409B
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600D6C6 RID: 54982 RVA: 0x004D5EA4 File Offset: 0x004D40A4
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

		// Token: 0x0600D6C7 RID: 54983 RVA: 0x004D5F3E File Offset: 0x004D413E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisconnectFromServer()
		{
			if (this.ticketHandle != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(this.ticketHandle);
				this.ticketHandle = HAuthTicket.Invalid;
			}
		}

		// Token: 0x0600D6C8 RID: 54984 RVA: 0x004D5F68 File Offset: 0x004D4168
		public void AuthenticateServer(ClientAuthenticateServerContext _context)
		{
			_context.Success();
		}

		// Token: 0x0600D6C9 RID: 54985 RVA: 0x004D5F70 File Offset: 0x004D4170
		public void Destroy()
		{
			this.OnDisconnectFromServer();
		}

		// Token: 0x0400A326 RID: 41766
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A327 RID: 41767
		[PublicizedFrom(EAccessModifier.Private)]
		public HAuthTicket ticketHandle = HAuthTicket.Invalid;

		// Token: 0x0400A328 RID: 41768
		[PublicizedFrom(EAccessModifier.Private)]
		public bool registeredDisconnectEvent;
	}
}
