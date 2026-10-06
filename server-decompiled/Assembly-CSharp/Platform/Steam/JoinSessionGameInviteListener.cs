using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CBA RID: 7354
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600DA05 RID: 55813 RVA: 0x004E1AA1 File Offset: 0x004DFCA1
		public void Init(IPlatform _owner)
		{
			if (this.m_friends_serverchange == null)
			{
				this.m_friends_serverchange = Callback<GameServerChangeRequested_t>.Create(new Callback<GameServerChangeRequested_t>.DispatchDelegate(this.Friends_GameServerChangeRequested));
			}
			_owner.Api.ClientApiInitialized += delegate
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				for (int i = 0; i < commandLineArgs.Length - 1; i++)
				{
					ulong num;
					if (commandLineArgs[i] == "+connect_lobby" && ulong.TryParse(commandLineArgs[i + 1], out num))
					{
						this.SetLobby(new CSteamID(num));
					}
				}
			};
		}

		// Token: 0x0600DA06 RID: 55814 RVA: 0x004E1ADC File Offset: 0x004DFCDC
		[return: TupleElementNames(new string[] { "invite", "password" })]
		public ValueTuple<string, string> TakePendingInvite()
		{
			string text = this.pendingInvite;
			this.pendingInvite = null;
			string text2 = this.pendingPassword;
			this.pendingPassword = null;
			return new ValueTuple<string, string>(text, text2);
		}

		// Token: 0x0600DA07 RID: 55815 RVA: 0x004E1B0A File Offset: 0x004DFD0A
		public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
		{
			if (string.IsNullOrEmpty(_invite))
			{
				if (_onFinished != null)
				{
					_onFinished(false);
				}
				yield break;
			}
			if (_invite.StartsWith("Lobby:"))
			{
				ILobbyHost lobbyHost = PlatformManager.NativePlatform.LobbyHost;
				if (lobbyHost != null)
				{
					lobbyHost.JoinLobby(_invite.Substring("Lobby:".Length), null);
				}
				if (_onFinished != null)
				{
					_onFinished(true);
				}
				yield break;
			}
			string[] array = _invite.Split(':', StringSplitOptions.None);
			string text = "";
			int num = 0;
			if (array.Length == 2)
			{
				text = array[0];
				num = Convert.ToInt32(array[1]);
			}
			yield return InviteManager.HandleIpPortInvite(text, num, _password, _onFinished);
			yield break;
		}

		// Token: 0x0600DA08 RID: 55816 RVA: 0x004E1B27 File Offset: 0x004DFD27
		public string GetListenerIdentifier()
		{
			return "STM";
		}

		// Token: 0x0600DA09 RID: 55817 RVA: 0x004E1B2E File Offset: 0x004DFD2E
		public void SetLobby(CSteamID _lobbyId)
		{
			this.pendingInvite = "Lobby:" + _lobbyId.m_SteamID.ToString();
		}

		// Token: 0x0600DA0A RID: 55818 RVA: 0x004E1B4C File Offset: 0x004DFD4C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Friends_GameServerChangeRequested(GameServerChangeRequested_t _value)
		{
			Log.Out("[Steamworks.NET] Friends_GameServerChangeRequested");
			this.pendingInvite = _value.m_rgchServer;
			this.pendingPassword = _value.m_rgchPassword;
		}

		// Token: 0x0400A53E RID: 42302
		[PublicizedFrom(EAccessModifier.Private)]
		public const string LobbyMarker = "Lobby:";

		// Token: 0x0400A53F RID: 42303
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A540 RID: 42304
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A541 RID: 42305
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameServerChangeRequested_t> m_friends_serverchange;
	}
}
