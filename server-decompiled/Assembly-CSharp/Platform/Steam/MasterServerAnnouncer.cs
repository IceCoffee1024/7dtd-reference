using System;
using System.Collections;
using System.Net;
using Steamworks;
using UnityEngine;

namespace Platform.Steam
{
	// Token: 0x02001CC6 RID: 7366
	public class MasterServerAnnouncer : IMasterServerAnnouncer
	{
		// Token: 0x0600DA6D RID: 55917 RVA: 0x004E2F1D File Offset: 0x004E111D
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DA6E RID: 55918 RVA: 0x004E2F28 File Offset: 0x004E1128
		public void Update()
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			this.tickDurationStopwatch.Restart();
			GameServer.RunCallbacks();
			long num = this.tickDurationStopwatch.ElapsedMicroseconds / 1000L;
			if (num > 25L)
			{
				Log.Warning(string.Format("[SteamServer] Tick took exceptionally long: {0} ms", num));
			}
			if (this.localGameServerId == CSteamID.Nil)
			{
				return;
			}
			if (this.updateTagsCountdown.HasPassed())
			{
				this.updateTagsCountdown.Reset();
				SteamGameServer.SetGameTags(NetworkUtils.BuildGameTags(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo));
			}
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x0600DA6F RID: 55919 RVA: 0x004E2FBB File Offset: 0x004E11BB
		// (set) Token: 0x0600DA70 RID: 55920 RVA: 0x004E2FC3 File Offset: 0x004E11C3
		public bool GameServerInitialized
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DA71 RID: 55921 RVA: 0x004E2FCC File Offset: 0x004E11CC
		public void AdvertiseServer(Action _onServerRegistered)
		{
			if (this.owner.User.UserStatus == EUserStatus.OfflineMode)
			{
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			this.onServerRegistered = _onServerRegistered;
			this.registerGameCoroutine = this.RegisterGame();
			ThreadManager.StartCoroutine(this.registerGameCoroutine);
		}

		// Token: 0x0600DA72 RID: 55922 RVA: 0x004E300C File Offset: 0x004E120C
		public void StopServer()
		{
			Log.Out("[Steamworks.NET] Stopping server");
			if (this.registerGameCoroutine != null)
			{
				ThreadManager.StopCoroutine(this.registerGameCoroutine);
				this.registerGameCoroutine = null;
			}
			if (this.localGameServerId != CSteamID.Nil)
			{
				if (!GameManager.IsDedicatedServer && !this.owner.AsServerOnly)
				{
					SteamUser.AdvertiseGame(CSteamID.Nil, 0U, 0);
				}
				SteamGameServer.SetAdvertiseServerActive(false);
				SteamGameServer.LogOff();
			}
			if (this.GameServerInitialized)
			{
				this.GameServerInitialized = false;
				GameServer.Shutdown();
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance != null && SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo != null)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedString -= this.updateSteamKeysString;
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedInt -= this.updateSteamKeysInt;
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedBool -= this.updateSteamKeysBool;
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedAny -= this.updateSteamKeys;
			}
			this.localGameServerId = CSteamID.Nil;
			this.onServerRegistered = null;
		}

		// Token: 0x0600DA73 RID: 55923 RVA: 0x004E3125 File Offset: 0x004E1325
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator RegisterGame()
		{
			yield return null;
			if (!GameServer.Init(NetworkUtils.ToInt(IPAddress.Any.ToString()), (ushort)SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoInt.Port), (ushort)SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoInt.Port), EServerMode.eServerModeAuthentication, Platform.Steam.Constants.SteamVersionNr))
			{
				Log.Error("[Steamworks.NET] Could not initialize GameServer");
				Action action = this.onServerRegistered;
				if (action != null)
				{
					action();
				}
				this.onServerRegistered = null;
				yield break;
			}
			yield return null;
			this.GameServerInitialized = true;
			this.owner.Api.ServerApiLoaded();
			Log.Out("[Steamworks.NET] GameServer.Init successful");
			SteamGameServer.SetDedicatedServer(GameManager.IsDedicatedServer);
			SteamGameServer.SetModDir("7DTD");
			SteamGameServer.SetProduct("7DTD");
			SteamGameServer.SetGameDescription("7 Days To Die");
			SteamGameServer.SetMaxPlayerCount(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoInt.MaxPlayers));
			SteamGameServer.SetBotPlayerCount(0);
			SteamGameServer.SetPasswordProtected(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoBool.IsPasswordProtected));
			SteamGameServer.SetMapName(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoString.LevelName));
			SteamGameServer.SetServerName(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoString.GameHost));
			SteamGameServer.SetGameTags(NetworkUtils.BuildGameTags(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo));
			SteamGameServer.LogOnAnonymous();
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoInt.ServerVisibility) == 2)
			{
				Log.Out("[Steamworks.NET] Making server public");
				SteamGameServer.SetAdvertiseServerActive(true);
			}
			float loginTimeout = 30f;
			while (!SteamGameServer.BLoggedOn() && loginTimeout > 0f)
			{
				yield return null;
				loginTimeout -= Time.unscaledDeltaTime;
			}
			if (SteamGameServer.BLoggedOn())
			{
				string text = SteamGameServer.GetPublicIP().ToString();
				this.localGameServerId = SteamGameServer.GetSteamID();
				Log.Out("[Steamworks.NET] GameServer.LogOn successful, SteamID=" + this.localGameServerId.m_SteamID.ToString() + ", public IP=" + Utils.MaskIp(text));
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoString.SteamID, this.localGameServerId.m_SteamID.ToString());
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoString.IP, text);
				if (PlatformManager.CrossplatformPlatform == null)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.SetValue(GameInfoString.UniqueId, this.localGameServerId.ToString());
				}
				GamePrefs.Set(EnumGamePrefs.ServerIP, text);
				this.SetGameServerInfo(SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance != null && SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo != null)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedString += this.updateSteamKeysString;
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedInt += this.updateSteamKeysInt;
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedBool += this.updateSteamKeysBool;
					SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedAny += this.updateSteamKeys;
				}
			}
			else
			{
				Log.Error("[Steamworks.NET] GameServer.LogOn timed out");
			}
			if (this.onServerRegistered != null)
			{
				this.onServerRegistered();
				this.onServerRegistered = null;
			}
			this.registerGameCoroutine = null;
			yield break;
		}

		// Token: 0x0600DA74 RID: 55924 RVA: 0x004E3134 File Offset: 0x004E1334
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetGameServerInfo(GameServerInfo _gameServerInfo)
		{
			SteamGameServer.ClearAllKeyValues();
			foreach (GameInfoString gameInfoString in EnumUtils.Values<GameInfoString>())
			{
				SteamGameServer.SetKeyValue(gameInfoString.ToStringCached<GameInfoString>(), _gameServerInfo.GetValue(gameInfoString));
			}
			foreach (GameInfoInt gameInfoInt in EnumUtils.Values<GameInfoInt>())
			{
				SteamGameServer.SetKeyValue(gameInfoInt.ToStringCached<GameInfoInt>(), _gameServerInfo.GetValue(gameInfoInt).ToString());
			}
			foreach (GameInfoBool gameInfoBool in EnumUtils.Values<GameInfoBool>())
			{
				SteamGameServer.SetKeyValue(gameInfoBool.ToStringCached<GameInfoBool>(), _gameServerInfo.GetValue(gameInfoBool).ToString());
			}
		}

		// Token: 0x0600DA75 RID: 55925 RVA: 0x004E3238 File Offset: 0x004E1438
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSteamKeysString(GameServerInfo _gameServerInfo, GameInfoString _gameInfoString)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (this.localGameServerId == CSteamID.Nil)
			{
				return;
			}
			SteamGameServer.SetKeyValue(_gameInfoString.ToStringCached<GameInfoString>(), _gameServerInfo.GetValue(_gameInfoString));
			if (!this.updateTagsCountdown.IsRunning)
			{
				this.updateTagsCountdown.ResetAndRestart();
			}
		}

		// Token: 0x0600DA76 RID: 55926 RVA: 0x004E328C File Offset: 0x004E148C
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSteamKeysInt(GameServerInfo _gameServerInfo, GameInfoInt _gameInfoInt)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (this.localGameServerId == CSteamID.Nil)
			{
				return;
			}
			SteamGameServer.SetKeyValue(_gameInfoInt.ToStringCached<GameInfoInt>(), _gameServerInfo.GetValue(_gameInfoInt).ToString());
			if (!this.updateTagsCountdown.IsRunning)
			{
				this.updateTagsCountdown.ResetAndRestart();
			}
		}

		// Token: 0x0600DA77 RID: 55927 RVA: 0x004E32E8 File Offset: 0x004E14E8
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSteamKeysBool(GameServerInfo _gameServerInfo, GameInfoBool _gameInfoBool)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (this.localGameServerId == CSteamID.Nil)
			{
				return;
			}
			SteamGameServer.SetKeyValue(_gameInfoBool.ToStringCached<GameInfoBool>(), _gameServerInfo.GetValue(_gameInfoBool).ToString());
			if (!this.updateTagsCountdown.IsRunning)
			{
				this.updateTagsCountdown.ResetAndRestart();
			}
		}

		// Token: 0x0600DA78 RID: 55928 RVA: 0x004E3344 File Offset: 0x004E1544
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSteamKeys(GameServerInfo _gameServerInfo)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (this.localGameServerId == CSteamID.Nil)
			{
				return;
			}
			SteamGameServer.SetMapName(_gameServerInfo.GetValue(GameInfoString.LevelName));
			if (!this.updateTagsCountdown.IsRunning)
			{
				this.updateTagsCountdown.ResetAndRestart();
			}
		}

		// Token: 0x0600DA79 RID: 55929 RVA: 0x004E3394 File Offset: 0x004E1594
		public string GetServerPorts()
		{
			return GamePrefs.GetInt(EnumGamePrefs.ServerPort).ToString() + "/UDP, " + (GamePrefs.GetInt(EnumGamePrefs.ServerPort) + 1).ToString() + "/UDP";
		}

		// Token: 0x0400A570 RID: 42352
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A571 RID: 42353
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CountdownTimer updateTagsCountdown = new CountdownTimer(300f, false);

		// Token: 0x0400A572 RID: 42354
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator registerGameCoroutine;

		// Token: 0x0400A573 RID: 42355
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onServerRegistered;

		// Token: 0x0400A574 RID: 42356
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);

		// Token: 0x0400A576 RID: 42358
		[PublicizedFrom(EAccessModifier.Private)]
		public CSteamID localGameServerId = CSteamID.Nil;
	}
}
