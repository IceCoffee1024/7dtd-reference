using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C61 RID: 7265
	[PublicizedFrom(EAccessModifier.Internal)]
	public class JoinSessionGameInviteListener : IJoinSessionGameInviteListener
	{
		// Token: 0x0600D6F9 RID: 55033 RVA: 0x004D6A3D File Offset: 0x004D4C3D
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

		// Token: 0x0600D6FA RID: 55034 RVA: 0x004D6A78 File Offset: 0x004D4C78
		[return: TupleElementNames(new string[] { "invite", "password" })]
		public ValueTuple<string, string> TakePendingInvite()
		{
			string text = this.pendingInvite;
			this.pendingInvite = null;
			string text2 = this.pendingPassword;
			this.pendingPassword = null;
			return new ValueTuple<string, string>(text, text2);
		}

		// Token: 0x0600D6FB RID: 55035 RVA: 0x004D6AA6 File Offset: 0x004D4CA6
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

		// Token: 0x0600D6FC RID: 55036 RVA: 0x004D6AC3 File Offset: 0x004D4CC3
		public string GetListenerIdentifier()
		{
			return "STM";
		}

		// Token: 0x0600D6FD RID: 55037 RVA: 0x004D6ACA File Offset: 0x004D4CCA
		public void SetLobby(CSteamID _lobbyId)
		{
			this.pendingInvite = "Lobby:" + _lobbyId.m_SteamID.ToString();
		}

		// Token: 0x0600D6FE RID: 55038 RVA: 0x004D6AE8 File Offset: 0x004D4CE8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Friends_GameServerChangeRequested(GameServerChangeRequested_t _value)
		{
			Log.Out("[Steamworks.NET] Friends_GameServerChangeRequested");
			this.pendingInvite = _value.m_rgchServer;
			this.pendingPassword = _value.m_rgchPassword;
		}

		// Token: 0x0400A337 RID: 41783
		[PublicizedFrom(EAccessModifier.Private)]
		public const string LobbyMarker = "Lobby:";

		// Token: 0x0400A338 RID: 41784
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingInvite;

		// Token: 0x0400A339 RID: 41785
		[PublicizedFrom(EAccessModifier.Private)]
		public string pendingPassword;

		// Token: 0x0400A33A RID: 41786
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GameServerChangeRequested_t> m_friends_serverchange;
	}
}
