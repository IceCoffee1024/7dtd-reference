using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using Twitch;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200121D RID: 4637
public class GameSparksManager : MonoBehaviour
{
	// Token: 0x170011D2 RID: 4562
	// (get) Token: 0x060093C8 RID: 37832 RVA: 0x0037DD12 File Offset: 0x0037BF12
	public int sessionUpdateIntervalSec
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (this.DebugEnabled ? 2 : 15) * 60;
		}
	}

	// Token: 0x060093C9 RID: 37833 RVA: 0x0037DD24 File Offset: 0x0037BF24
	public static GameSparksManager Instance()
	{
		GameSparksManager.instance != null;
		return GameSparksManager.instance;
	}

	// Token: 0x170011D3 RID: 4563
	// (get) Token: 0x060093CA RID: 37834 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool DebugEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return false;
		}
	}

	// Token: 0x170011D4 RID: 4564
	// (get) Token: 0x060093CB RID: 37835 RVA: 0x000321FF File Offset: 0x000303FF
	public string DeviceId
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return "";
		}
	}

	// Token: 0x060093CC RID: 37836 RVA: 0x00148FD3 File Offset: 0x001471D3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060093CD RID: 37837 RVA: 0x0037DD37 File Offset: 0x0037BF37
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		this.DeviceAuth(new GameSparksManager.OnPlayerAuthenticated(this.PlayerAuthenticated), new GameSparksManager.OnError(this.AuthError));
		yield break;
	}

	// Token: 0x060093CE RID: 37838 RVA: 0x0037DD48 File Offset: 0x0037BF48
	[PublicizedFrom(EAccessModifier.Private)]
	public void AuthError(string _error)
	{
		if (_error.Contains("cannot authenticate this month"))
		{
			Log.Out("[GSM] Skipping me");
			return;
		}
		if (_error.Contains("timeout"))
		{
			Log.Error("[GSM] AuthError TimeOut");
			return;
		}
		if (_error.Contains("UNRECOGNISED"))
		{
			Log.Error("[GSM] AuthError UNRECOGNISED");
			return;
		}
		Log.Error("[GSM] AuthError" + _error);
	}

	// Token: 0x060093CF RID: 37839 RVA: 0x0037DDAD File Offset: 0x0037BFAD
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerAuthenticated(PlayerDetails _playerDetails)
	{
		this.ProgramStarted();
	}

	// Token: 0x060093D0 RID: 37840 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DeviceAuth(GameSparksManager.OnPlayerAuthenticated _onPlayerAuthSuccess, GameSparksManager.OnError _onAuthError)
	{
	}

	// Token: 0x060093D1 RID: 37841 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void PrepareAndSendRequest(GSRequestData _data, string _eventKey)
	{
	}

	// Token: 0x060093D2 RID: 37842 RVA: 0x0037DDB8 File Offset: 0x0037BFB8
	public void ProgramStarted()
	{
		string text = "PROGRAM_START";
		GSRequestData gsrequestData = new GSRequestData();
		gsrequestData.AddString("uniqueID", this.DeviceId);
		gsrequestData.AddBoolean("IsDedicated", GameManager.IsDedicatedServer);
		GSRequestData gsrequestData2 = new GSRequestData();
		gsrequestData2.AddString("Text", Constants.cVersionInformation.ShortString);
		gsrequestData2.AddString("LongText", Constants.cVersionInformation.LongString);
		gsrequestData2.AddString("Type", Constants.cVersionInformation.ReleaseType.ToStringCached<VersionInformation.EGameReleaseType>());
		gsrequestData2.AddNumber("Major", Constants.cVersionInformation.Major);
		gsrequestData2.AddNumber("Minor", Constants.cVersionInformation.Minor);
		gsrequestData2.AddNumber("Build", Constants.cVersionInformation.Build);
		gsrequestData.AddObject("GameVersion", gsrequestData2);
		if (!GameManager.IsDedicatedServer)
		{
			gsrequestData.AddString("BuildPlatform", Application.platform.ToStringCached<RuntimePlatform>());
			gsrequestData.AddString("OperatingSystemFamily", SystemInfo.operatingSystemFamily.ToStringCached<OperatingSystemFamily>());
			gsrequestData.AddString("OperatingSystemFull", SystemInfo.operatingSystem);
			gsrequestData.AddString("ProcessorType", SystemInfo.processorType);
			gsrequestData.AddNumber("ProcessorCount", SystemInfo.processorCount);
			gsrequestData.AddNumber("ProcessorClockMHz", MathUtils.RoundToSignificantDigits((double)SystemInfo.processorFrequency, 2));
			gsrequestData.AddNumber("SystemMemoryMB", MathUtils.TruncateToSignificantDigits((double)SystemInfo.systemMemorySize, 2));
			GSRequestData gsrequestData3 = gsrequestData;
			string text2 = "Country";
			IUtils utils = PlatformManager.NativePlatform.Utils;
			gsrequestData3.AddString(text2, ((utils != null) ? utils.GetCountry() : null) ?? "-n/a-");
			gsrequestData.AddString("Language", Localization.ActiveLanguage.ToLower());
			GSRequestData gsrequestData4 = gsrequestData;
			string text3 = "EacActive";
			IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
			gsrequestData4.AddBoolean(text3, antiCheatClient != null && antiCheatClient.ClientAntiCheatEnabled());
			gsrequestData.AddString("GraphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
			gsrequestData.AddString("GraphicsDeviceName", SystemInfo.graphicsDeviceName);
			gsrequestData.AddNumber("GraphicsMemoryMB", MathUtils.RoundToSignificantDigits((double)SystemInfo.graphicsMemorySize, 2));
			gsrequestData.AddString("GraphicsApi", SystemInfo.graphicsDeviceType.ToStringCached<GraphicsDeviceType>());
			gsrequestData.AddString("GraphicsVersion", SystemInfo.graphicsDeviceVersion);
			gsrequestData.AddNumber("GraphicsShaderLevel", (double)((float)SystemInfo.graphicsShaderLevel / 10f));
			gsrequestData.AddBoolean("GameSenseInstalled", GameSenseManager.GameSenseInstalled);
			Display main = Display.main;
			ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string> valueTuple = XUiC_OptionsVideo.ResolutionInfo.DimensionsToAspectRatio(main.systemWidth, main.systemHeight);
			float item = valueTuple.Item2;
			string item2 = valueTuple.Item3;
			GSRequestData gsrequestData5 = new GSRequestData();
			gsrequestData5.AddString("Text", string.Format("{0}x{1}", main.systemWidth, main.systemHeight));
			gsrequestData5.AddNumber("Width", main.systemWidth);
			gsrequestData5.AddNumber("Height", main.systemHeight);
			gsrequestData5.AddNumber("AspectRatio", (double)item);
			gsrequestData5.AddString("AspectRatioName", item2);
			gsrequestData.AddObject("ScreenResolution", gsrequestData5);
			GSRequestData gsrequestData6 = new GSRequestData();
			ValueTuple<XUiC_OptionsVideo.ResolutionInfo.EAspectRatio, float, string> valueTuple2 = XUiC_OptionsVideo.ResolutionInfo.DimensionsToAspectRatio(Screen.width, Screen.height);
			float item3 = valueTuple2.Item2;
			string item4 = valueTuple2.Item3;
			gsrequestData6.AddString("Text", string.Format("{0}x{1}", Screen.width, Screen.height));
			gsrequestData6.AddNumber("Width", Screen.width);
			gsrequestData6.AddNumber("Height", Screen.height);
			gsrequestData6.AddNumber("AspectRatio", (double)item3);
			gsrequestData6.AddString("AspectRatioName", item4);
			gsrequestData.AddObject("Resolution", gsrequestData6);
			gsrequestData.AddString("FullscreenMode", Screen.fullScreenMode.ToStringCached<FullScreenMode>());
			GSRequestData gsrequestData7 = new GSRequestData();
			foreach (EnumGamePrefs enumGamePrefs in EnumUtils.Values<EnumGamePrefs>())
			{
				string text4 = enumGamePrefs.ToStringCached<EnumGamePrefs>();
				if (text4.StartsWith("Options", StringComparison.Ordinal))
				{
					GamePrefs.EnumType? prefType = GamePrefs.GetPrefType(enumGamePrefs);
					if (prefType != null)
					{
						switch (prefType.GetValueOrDefault())
						{
						case GamePrefs.EnumType.Int:
							gsrequestData7.AddNumber(text4, GamePrefs.GetInt(enumGamePrefs));
							break;
						case GamePrefs.EnumType.Float:
							gsrequestData7.AddNumber(text4, (double)GamePrefs.GetFloat(enumGamePrefs));
							break;
						case GamePrefs.EnumType.String:
							gsrequestData7.AddString(text4, GamePrefs.GetString(enumGamePrefs));
							break;
						case GamePrefs.EnumType.Bool:
							gsrequestData7.AddBoolean(text4, GamePrefs.GetBool(enumGamePrefs));
							break;
						case GamePrefs.EnumType.Binary:
							Log.Warning("Options GamePref with type Binary: " + text4);
							break;
						default:
							Log.Warning("Options GamePref with unknown type: " + text4);
							break;
						}
					}
					else
					{
						Log.Warning("Options GamePref with no declaration entry: " + text4);
					}
				}
			}
			gsrequestData.AddObject("GameSettings", gsrequestData7);
		}
		else
		{
			gsrequestData.AddString("DediBuildPlatform", Application.platform.ToStringCached<RuntimePlatform>());
			gsrequestData.AddString("DediOperatingSystemFamily", SystemInfo.operatingSystemFamily.ToStringCached<OperatingSystemFamily>());
			gsrequestData.AddString("DediOperatingSystemFull", SystemInfo.operatingSystem);
			gsrequestData.AddString("DediProcessorType", SystemInfo.processorType);
			gsrequestData.AddNumber("DediProcessorCount", SystemInfo.processorCount);
			gsrequestData.AddNumber("DediProcessorClockMHz", MathUtils.RoundToSignificantDigits((double)SystemInfo.processorFrequency, 2));
			gsrequestData.AddNumber("DediSystemMemoryMB", MathUtils.TruncateToSignificantDigits((double)SystemInfo.systemMemorySize, 2));
		}
		this.PrepareAndSendRequest(gsrequestData, text);
	}

	// Token: 0x060093D3 RID: 37843 RVA: 0x0037E318 File Offset: 0x0037C518
	public void PrepareNewSession()
	{
		GameSparksCollector.GetSessionUpdateDataAndReset();
		GameSparksCollector.GetSessionTotalData(true);
		PlatformManager.NativePlatform.Input.ResetInputStyleUsage();
	}

	// Token: 0x060093D4 RID: 37844 RVA: 0x0037E338 File Offset: 0x0037C538
	public void SessionStarted(string _world, string _gameMode, bool _isServer)
	{
		GameServerInfo gameServerInfo = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo : SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
		string text = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer ? GameManager.Instance.World.Guid : GamePrefs.GetString(EnumGamePrefs.GameGuidClient));
		int num = 1;
		bool flag = num == 1 || num == 2;
		bool flag2 = false;
		flag2 = gameServerInfo.GetValue(GameInfoBool.ModdedConfig);
		string text2 = "SESSION_START";
		GSRequestData gsrequestData = new GSRequestData();
		gsrequestData.AddString("uniqueID", text);
		gsrequestData.AddBoolean("StockSettings", flag);
		gsrequestData.AddBoolean("VanillaConfig", !flag2);
		string text3;
		if (GameManager.IsDedicatedServer)
		{
			text3 = "DedicatedServer";
		}
		else if (_isServer)
		{
			text3 = "ListenServer";
		}
		else if (gameServerInfo.IsDedicated)
		{
			text3 = "ClientOnDedicated";
		}
		else
		{
			text3 = "ClientOnListen";
		}
		gsrequestData.AddString("PeerType", text3);
		GSRequestData gsrequestData2 = new GSRequestData();
		List<Mod> loadedMods = ModManager.GetLoadedMods();
		if (loadedMods.Count == 0)
		{
			gsrequestData2.AddNumber("_Vanilla_", 1);
		}
		else
		{
			foreach (Mod mod in loadedMods)
			{
				gsrequestData2.AddNumber(mod.Name, 1);
			}
		}
		gsrequestData.AddObject(GameManager.IsDedicatedServer ? "ModsLoadedDedi" : "ModsLoadedClient", gsrequestData2);
		if (_isServer)
		{
			GSRequestData gsrequestData3 = new GSRequestData();
			foreach (KeyValuePair<GameInfoString, string> keyValuePair in gameServerInfo.Strings)
			{
				if (!GameSparksManager.IgnoredGameInfoStrings.Contains(keyValuePair.Key))
				{
					gsrequestData3.AddString(keyValuePair.Key.ToStringCached<GameInfoString>(), keyValuePair.Value);
				}
			}
			foreach (KeyValuePair<GameInfoInt, int> keyValuePair2 in gameServerInfo.Ints)
			{
				if (!GameSparksManager.IgnoredGameInfoInts.Contains(keyValuePair2.Key))
				{
					gsrequestData3.AddNumber(keyValuePair2.Key.ToStringCached<GameInfoInt>(), keyValuePair2.Value);
				}
			}
			foreach (KeyValuePair<GameInfoBool, bool> keyValuePair3 in gameServerInfo.Bools)
			{
				if (!GameSparksManager.IgnoredGameInfoBools.Contains(keyValuePair3.Key))
				{
					gsrequestData3.AddBoolean(keyValuePair3.Key.ToStringCached<GameInfoBool>(), keyValuePair3.Value);
				}
			}
			gsrequestData.AddObject("WorldSettings", gsrequestData3);
		}
		this.PrepareAndSendRequest(gsrequestData, text2);
		this.sessionUpdateCoroutine = null;
		this.endCoroutine = false;
		GameSparksCollector.CollectGamePlayData = flag && !flag2;
		this.sessionUpdateCoroutine = ThreadManager.StartCoroutine(this.SessionUpdate());
		GameSparksCollector.SetValue(GameSparksCollector.GSDataKey.UsedTwitchIntegration, null, 0, false, GameSparksCollector.GSDataCollection.SessionTotal);
		GameSparksCollector.SetValue(GameSparksCollector.GSDataKey.PeakConcurrentClients, null, 0, false, GameSparksCollector.GSDataCollection.SessionTotal);
		GameSparksCollector.SetValue(GameSparksCollector.GSDataKey.PeakConcurrentPlayers, null, GameManager.IsDedicatedServer ? 0 : 1, false, GameSparksCollector.GSDataCollection.SessionTotal);
		this.nextDediSessionEndTransmitTime = Time.unscaledTime + 28800f;
	}

	// Token: 0x060093D5 RID: 37845 RVA: 0x0037E67C File Offset: 0x0037C87C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator SessionUpdate()
	{
		while (!this.endCoroutine)
		{
			yield return new WaitForSecondsRealtime((float)this.sessionUpdateIntervalSec);
			if (this.endCoroutine)
			{
				yield break;
			}
			if (TwitchManager.HasInstance && TwitchManager.Current.IsReady)
			{
				GameSparksCollector.SetValue(GameSparksCollector.GSDataKey.UsedTwitchIntegration, null, 1, false, GameSparksCollector.GSDataCollection.SessionTotal);
			}
			string text = "SESSION_UPDATE";
			GSRequestData sessionUpdateDataAndReset = GameSparksCollector.GetSessionUpdateDataAndReset();
			if (GameManager.IsDedicatedServer && this.nextDediSessionEndTransmitTime <= Time.unscaledTime)
			{
				foreach (KeyValuePair<string, object> keyValuePair in GameSparksCollector.GetSessionTotalData(false).BaseData)
				{
					sessionUpdateDataAndReset.Add(keyValuePair.Key, keyValuePair.Value);
				}
				this.nextDediSessionEndTransmitTime = Time.unscaledTime + 28800f;
			}
			this.PrepareAndSendRequest(sessionUpdateDataAndReset, text);
		}
		yield break;
	}

	// Token: 0x060093D6 RID: 37846 RVA: 0x0037E68C File Offset: 0x0037C88C
	public void SessionEnded()
	{
		if (this.sessionUpdateCoroutine != null)
		{
			this.endCoroutine = true;
			ThreadManager.StopCoroutine(this.sessionUpdateCoroutine);
			this.sessionUpdateCoroutine = null;
		}
		if (TwitchManager.HasInstance && TwitchManager.Current.IsReady)
		{
			GameSparksCollector.SetValue(GameSparksCollector.GSDataKey.UsedTwitchIntegration, null, 1, false, GameSparksCollector.GSDataCollection.SessionTotal);
		}
		string text = "SESSION_END";
		GSRequestData sessionUpdateDataAndReset = GameSparksCollector.GetSessionUpdateDataAndReset();
		sessionUpdateDataAndReset.AddString("uniqueID", this.DeviceId);
		foreach (KeyValuePair<string, object> keyValuePair in GameSparksCollector.GetSessionTotalData(true).BaseData)
		{
			sessionUpdateDataAndReset.Add(keyValuePair.Key, keyValuePair.Value);
		}
		GSRequestData gsrequestData = new GSRequestData();
		sessionUpdateDataAndReset.AddObject("RunningTotals", gsrequestData);
		if (!GameManager.IsDedicatedServer)
		{
			sessionUpdateDataAndReset.AddString("InputDeviceStyle", PlatformManager.NativePlatform.Input.MostUsedInputStyle().ToStringCached<PlayerInputManager.InputStyle>());
			PlatformManager.NativePlatform.Input.ResetInputStyleUsage();
		}
		this.PrepareAndSendRequest(sessionUpdateDataAndReset, text);
	}

	// Token: 0x04006E4D RID: 28237
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int dediSessionEndIntervalSec = 28800;

	// Token: 0x04006E4E RID: 28238
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<GameInfoString> IgnoredGameInfoStrings = new List<GameInfoString>
	{
		GameInfoString.GameHost,
		GameInfoString.GameName,
		GameInfoString.IP,
		GameInfoString.ServerDescription,
		GameInfoString.ServerLoginConfirmationText,
		GameInfoString.ServerWebsiteURL
	};

	// Token: 0x04006E4F RID: 28239
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<GameInfoInt> IgnoredGameInfoInts = new List<GameInfoInt>
	{
		GameInfoInt.CurrentPlayers,
		GameInfoInt.DayCount,
		GameInfoInt.Port,
		GameInfoInt.CurrentServerTime
	};

	// Token: 0x04006E50 RID: 28240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<GameInfoBool> IgnoredGameInfoBools = new List<GameInfoBool> { GameInfoBool.Architecture64 };

	// Token: 0x04006E51 RID: 28241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool authenticated;

	// Token: 0x04006E52 RID: 28242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameSparksManager instance;

	// Token: 0x04006E53 RID: 28243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine sessionUpdateCoroutine;

	// Token: 0x04006E54 RID: 28244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool endCoroutine;

	// Token: 0x04006E55 RID: 28245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextDediSessionEndTransmitTime = -1f;

	// Token: 0x0200121E RID: 4638
	// (Invoke) Token: 0x060093DA RID: 37850
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate void OnError(string _error);

	// Token: 0x0200121F RID: 4639
	// (Invoke) Token: 0x060093DE RID: 37854
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate void OnPlayerAuthenticated(PlayerDetails _playerDetails);
}
