using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C80 RID: 7296
	public class AuthenticationClient : IAuthenticationClient
	{
		// Token: 0x0600D855 RID: 55381 RVA: 0x004DC2BF File Offset: 0x004DA4BF
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600D856 RID: 55382 RVA: 0x004DC2C8 File Offset: 0x004DA4C8
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

		// Token: 0x0600D857 RID: 55383 RVA: 0x004DC362 File Offset: 0x004DA562
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDisconnectFromServer()
		{
			if (this.ticketHandle != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(this.ticketHandle);
				this.ticketHandle = HAuthTicket.Invalid;
			}
		}

		// Token: 0x0600D858 RID: 55384 RVA: 0x004DC38C File Offset: 0x004DA58C
		public void AuthenticateServer(ClientAuthenticateServerContext _context)
		{
			_context.Success();
		}

		// Token: 0x0600D859 RID: 55385 RVA: 0x004DC394 File Offset: 0x004DA594
		public void Destroy()
		{
			this.OnDisconnectFromServer();
		}

		// Token: 0x0400A457 RID: 42071
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A458 RID: 42072
		[PublicizedFrom(EAccessModifier.Private)]
		public HAuthTicket ticketHandle = HAuthTicket.Invalid;

		// Token: 0x0400A459 RID: 42073
		[PublicizedFrom(EAccessModifier.Private)]
		public bool registeredDisconnectEvent;
	}
}
