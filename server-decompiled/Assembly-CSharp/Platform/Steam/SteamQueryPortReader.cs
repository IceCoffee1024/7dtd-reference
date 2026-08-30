using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C91 RID: 7313
	public class SteamQueryPortReader
	{
		// Token: 0x14000136 RID: 310
		// (add) Token: 0x0600D8D1 RID: 55505 RVA: 0x004DD954 File Offset: 0x004DBB54
		// (remove) Token: 0x0600D8D2 RID: 55506 RVA: 0x004DD98C File Offset: 0x004DBB8C
		[method: PublicizedFrom(EAccessModifier.Private)]
		public event GameServerDetailsCallback GameServerDetailsEvent;

		// Token: 0x0600D8D3 RID: 55507 RVA: 0x004DD9C4 File Offset: 0x004DBBC4
		public void Init(IPlatform _owner)
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			if (this.matchmakingRulesResponse != null)
			{
				return;
			}
			this.matchmakingRulesResponse = new ISteamMatchmakingRulesResponse(new ISteamMatchmakingRulesResponse.RulesResponded(this.RulesResponded), new ISteamMatchmakingRulesResponse.RulesFailedToRespond(this.RulesFailedToRespond), new ISteamMatchmakingRulesResponse.RulesRefreshComplete(this.RulesRefreshComplete));
		}

		// Token: 0x0600D8D4 RID: 55508 RVA: 0x004DDA11 File Offset: 0x004DBC11
		public void Disconnect()
		{
			if (this.rulesRequestHandle != HServerQuery.Invalid)
			{
				SteamMatchmakingServers.CancelServerQuery(this.rulesRequestHandle);
				this.rulesRequestHandle = HServerQuery.Invalid;
			}
			this.GameServerDetailsEvent = null;
		}

		// Token: 0x0600D8D5 RID: 55509 RVA: 0x004DDA42 File Offset: 0x004DBC42
		public void RegisterGameServerCallbacks(GameServerDetailsCallback _details)
		{
			this.GameServerDetailsEvent = _details;
		}

		// Token: 0x0600D8D6 RID: 55510 RVA: 0x004DDA4B File Offset: 0x004DBC4B
		[PublicizedFrom(EAccessModifier.Private)]
		public void RunGameServerDetailsEvent(GameServerInfo _info, bool _success)
		{
			GameServerDetailsCallback gameServerDetailsEvent = this.GameServerDetailsEvent;
			if (gameServerDetailsEvent == null)
			{
				return;
			}
			gameServerDetailsEvent(_info, _success);
		}

		// Token: 0x0600D8D7 RID: 55511 RVA: 0x004DDA60 File Offset: 0x004DBC60
		public void GetGameServerInfo(GameServerInfo _gameInfo)
		{
			if (_gameInfo.IsLobby)
			{
				this.RunGameServerDetailsEvent(_gameInfo, true);
				return;
			}
			if (_gameInfo.IsNoResponse)
			{
				this.RunGameServerDetailsEvent(_gameInfo, true);
			}
			string text = _gameInfo.GetValue(GameInfoString.IP);
			long num;
			if (!long.TryParse(text.Replace(".", ""), out num))
			{
				try
				{
					IPHostEntry hostEntry = Dns.GetHostEntry(text);
					if (hostEntry.AddressList.Length == 0)
					{
						Log.Out("Steamworks.NET] No valid IP for server found");
						this.RunGameServerDetailsEvent(_gameInfo, false);
						return;
					}
					text = hostEntry.AddressList[0].ToString();
				}
				catch (SocketException ex)
				{
					string text2 = "Steamworks.NET] No such hostname: \"";
					string text3 = text;
					string text4 = "\": ";
					SocketException ex2 = ex;
					Log.Out(text2 + text3 + text4 + ((ex2 != null) ? ex2.ToString() : null));
					this.RunGameServerDetailsEvent(_gameInfo, false);
					return;
				}
			}
			SteamQueryPortReader.RulesRequest rulesRequest = new SteamQueryPortReader.RulesRequest
			{
				GameInfo = _gameInfo,
				Ip = NetworkUtils.ToInt(text),
				Port = (ushort)_gameInfo.GetValue(GameInfoInt.Port)
			};
			this.rulesRequests.Enqueue(rulesRequest);
			if (this.rulesRequestHandle == HServerQuery.Invalid)
			{
				this.StartNextRulesRequest();
			}
		}

		// Token: 0x0600D8D8 RID: 55512 RVA: 0x004DDB70 File Offset: 0x004DBD70
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartNextRulesRequest()
		{
			this.currentRulesRequest = null;
			this.rulesRequestHandle = HServerQuery.Invalid;
			if (this.rulesRequests.Count > 0)
			{
				this.currentRulesRequest = this.rulesRequests.Dequeue();
				this.currentRulesRequest.GameInfoClone = new GameServerInfo(this.currentRulesRequest.GameInfo);
				this.rulesRequestHandle = SteamMatchmakingServers.ServerRules(this.currentRulesRequest.Ip, this.currentRulesRequest.Port, this.matchmakingRulesResponse);
			}
		}

		// Token: 0x0600D8D9 RID: 55513 RVA: 0x004DDBF0 File Offset: 0x004DBDF0
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesFailedToRespond()
		{
			this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, false);
			this.StartNextRulesRequest();
		}

		// Token: 0x0600D8DA RID: 55514 RVA: 0x004DDC0C File Offset: 0x004DBE0C
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesRefreshComplete()
		{
			if (!this.currentRulesRequest.DataErrors && this.currentRulesRequest.GameInfoClone.GetValue(GameInfoString.GameName).Length > 0)
			{
				this.currentRulesRequest.GameInfo.Merge(this.currentRulesRequest.GameInfoClone, this.currentRulesRequest.GameInfo.IsLAN ? EServerRelationType.LAN : EServerRelationType.Internet);
				this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, true);
			}
			else
			{
				if (this.currentRulesRequest.DataErrors)
				{
					this.currentRulesRequest.GameInfo.SetValue(GameInfoString.ServerDescription, Localization.Get("xuiServerBrowserFailedRetrievingData", false, null));
				}
				this.RunGameServerDetailsEvent(this.currentRulesRequest.GameInfo, false);
			}
			this.StartNextRulesRequest();
		}

		// Token: 0x0600D8DB RID: 55515 RVA: 0x004DDCC8 File Offset: 0x004DBEC8
		[PublicizedFrom(EAccessModifier.Private)]
		public void RulesResponded(string _rule, string _value)
		{
			SteamQueryPortReader.RulesRequest rulesRequest = this.currentRulesRequest;
			if (rulesRequest.DataErrors)
			{
				return;
			}
			if (_rule.EqualsCaseInsensitive("gameinfo") || _rule.EqualsCaseInsensitive("ping"))
			{
				return;
			}
			if (rulesRequest.GameInfoClone.IsLAN && _rule.EqualsCaseInsensitive("ip"))
			{
				return;
			}
			if (!rulesRequest.GameInfoClone.ParseAny(_rule, _value))
			{
				rulesRequest.DataErrors = true;
			}
		}

		// Token: 0x0400A487 RID: 42119
		[PublicizedFrom(EAccessModifier.Private)]
		public ISteamMatchmakingRulesResponse matchmakingRulesResponse;

		// Token: 0x0400A488 RID: 42120
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Queue<SteamQueryPortReader.RulesRequest> rulesRequests = new Queue<SteamQueryPortReader.RulesRequest>();

		// Token: 0x0400A489 RID: 42121
		[PublicizedFrom(EAccessModifier.Private)]
		public SteamQueryPortReader.RulesRequest currentRulesRequest;

		// Token: 0x0400A48A RID: 42122
		[PublicizedFrom(EAccessModifier.Private)]
		public HServerQuery rulesRequestHandle = HServerQuery.Invalid;

		// Token: 0x02001C92 RID: 7314
		[PublicizedFrom(EAccessModifier.Private)]
		public class RulesRequest
		{
			// Token: 0x0400A48B RID: 42123
			public uint Ip;

			// Token: 0x0400A48C RID: 42124
			public ushort Port;

			// Token: 0x0400A48D RID: 42125
			public GameServerInfo GameInfo;

			// Token: 0x0400A48E RID: 42126
			public GameServerInfo GameInfoClone;

			// Token: 0x0400A48F RID: 42127
			public bool DataErrors;
		}
	}
}
