using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CB2 RID: 7346
	public class AuthenticationClient : IAuthenticationClient
	{
		// Token: 0x0600D9CE RID: 55758 RVA: 0x004E0DFB File Offset: 0x004DEFFB
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600D9CF RID: 55759 RVA: 0x004E0E04 File Offset: 0x004DF004
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

		// Token: 0x0600D9D0 RID: 55760 RVA: 0x004E0E9E File Offset: 0x004DF09E
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisconnectFromServer()
		{
			if (this.ticketHandle != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(this.ticketHandle);
				this.ticketHandle = HAuthTicket.Invalid;
			}
		}

		// Token: 0x0600D9D1 RID: 55761 RVA: 0x004E0EC8 File Offset: 0x004DF0C8
		public void AuthenticateServer(ClientAuthenticateServerContext _context)
		{
			_context.Success();
		}

		// Token: 0x0600D9D2 RID: 55762 RVA: 0x004E0ED0 File Offset: 0x004DF0D0
		public void Destroy()
		{
			this.OnDisconnectFromServer();
		}

		// Token: 0x0400A52C RID: 42284
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A52D RID: 42285
		[PublicizedFrom(EAccessModifier.Private)]
		public HAuthTicket ticketHandle = HAuthTicket.Invalid;

		// Token: 0x0400A52E RID: 42286
		[PublicizedFrom(EAccessModifier.Private)]
		public bool registeredDisconnectEvent;
	}
}
