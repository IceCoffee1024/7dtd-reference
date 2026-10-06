using System;
using System.IO;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Sanctions;
using UnityEngine;

namespace Platform.EOS
{
	// Token: 0x02001D16 RID: 7446
	public class Api : IPlatformApi
	{
		// Token: 0x0600DD0A RID: 56586 RVA: 0x004EBD74 File Offset: 0x004E9F74
		[PublicizedFrom(EAccessModifier.Private)]
		static Api()
		{
			string launchArgument = GameUtils.GetLaunchArgument("debugeos");
			if (launchArgument != null)
			{
				if (launchArgument == "verbose")
				{
					Api.DebugLevel = Api.EDebugLevel.Verbose;
					return;
				}
				Api.DebugLevel = Api.EDebugLevel.Normal;
			}
		}

		// Token: 0x0600DD0B RID: 56587 RVA: 0x004EBDAF File Offset: 0x004E9FAF
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DD0C RID: 56588 RVA: 0x004EBDB8 File Offset: 0x004E9FB8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnApplicationStateChanged(ApplicationState _applicationState)
		{
			if (this.PlatformInterface == null)
			{
				return;
			}
			ApplicationStatus applicationStatus;
			if (_applicationState != ApplicationState.Foreground)
			{
				if (_applicationState != ApplicationState.Suspended)
				{
					throw new ArgumentOutOfRangeException("_applicationState", _applicationState, "[EOS] OnApplicationStateChanged: ApplicationState is missing a conversion to a EOS.ApplicationStatus");
				}
				applicationStatus = ApplicationStatus.BackgroundSuspended;
			}
			else
			{
				applicationStatus = ApplicationStatus.Foreground;
			}
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.PlatformInterface.SetApplicationStatus(applicationStatus);
			}
		}

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x0600DD0D RID: 56589 RVA: 0x004EBE34 File Offset: 0x004EA034
		// (set) Token: 0x0600DD0E RID: 56590 RVA: 0x004EBE3C File Offset: 0x004EA03C
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x0600DD0F RID: 56591 RVA: 0x004EBE48 File Offset: 0x004EA048
		// (remove) Token: 0x0600DD10 RID: 56592 RVA: 0x004EBEA4 File Offset: 0x004EA0A4
		public event Action ClientApiInitialized
		{
			add
			{
				lock (this)
				{
					this.clientApiInitialized = (Action)Delegate.Combine(this.clientApiInitialized, value);
					if (this.ClientApiStatus == EApiStatus.Ok)
					{
						value();
					}
				}
			}
			remove
			{
				lock (this)
				{
					this.clientApiInitialized = (Action)Delegate.Remove(this.clientApiInitialized, value);
				}
			}
		}

		// Token: 0x0600DD11 RID: 56593 RVA: 0x004EBEF0 File Offset: 0x004EA0F0
		public bool InitClientApis()
		{
			if (this.ClientApiStatus == EApiStatus.Ok)
			{
				return true;
			}
			EosCreds eosCreds = (GameManager.IsDedicatedServer ? EosCreds.ServerDeviceIdCredentials : EosCreds.ClientCredentials);
			this.initPlatform(eosCreds, eosCreds.ServerMode);
			return this.ClientApiStatus == EApiStatus.Ok;
		}

		// Token: 0x0600DD12 RID: 56594 RVA: 0x004EBF31 File Offset: 0x004EA131
		public bool InitServerApis()
		{
			return this.InitClientApis();
		}

		// Token: 0x0600DD13 RID: 56595 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600DD14 RID: 56596 RVA: 0x004EBF3C File Offset: 0x004EA13C
		public void Update()
		{
			if (this.ClientApiStatus != EApiStatus.Ok)
			{
				return;
			}
			this.tickDurationStopwatch.Restart();
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.PlatformInterface.Tick();
			}
			long num = this.tickDurationStopwatch.ElapsedMicroseconds / 1000L;
			if (Api.DebugLevel != Api.EDebugLevel.Off && num > 5L)
			{
				Log.Warning(string.Format("[EOS] Tick took exceptionally long: {0} ms", num));
			}
		}

		// Token: 0x0600DD15 RID: 56597 RVA: 0x004EBFC8 File Offset: 0x004EA1C8
		public void Destroy()
		{
			if (this.ClientApiStatus != EApiStatus.Ok)
			{
				return;
			}
			this.ConnectInterface = null;
			object obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.PlatformInterface.Release();
			}
			this.PlatformInterface = null;
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				PlatformInterface.Shutdown();
			}
		}

		// Token: 0x0600DD16 RID: 56598 RVA: 0x00041064 File Offset: 0x0003F264
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600DD17 RID: 56599 RVA: 0x004EC054 File Offset: 0x004EA254
		[PublicizedFrom(EAccessModifier.Private)]
		public void initPlatform(EosCreds _creds, bool _serverMode)
		{
			InitializeOptions initializeOptions = new InitializeOptions
			{
				ProductName = "7 Days To Die",
				ProductVersion = Constants.cVersionInformation.SerializableString
			};
			Result result = Result.NotFound;
			object obj;
			try
			{
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					result = PlatformInterface.Initialize(ref initializeOptions);
				}
			}
			catch (DllNotFoundException ex)
			{
				this.ClientApiStatus = EApiStatus.PermanentError;
				Log.Error("[EOS] Native library or one of its dependencies not found (e.g. no Microsoft Visual C Redistributables 2022)");
				Log.Exception(ex);
				Application.Quit(1);
			}
			Log.Out(string.Format("[EOS] Initialize: {0}", result));
			LogLevel logLevel;
			switch (Api.DebugLevel)
			{
			case Api.EDebugLevel.Off:
				logLevel = LogLevel.Warning;
				break;
			case Api.EDebugLevel.Normal:
				logLevel = LogLevel.Info;
				break;
			case Api.EDebugLevel.Verbose:
				logLevel = LogLevel.VeryVerbose;
				break;
			default:
				throw new ArgumentOutOfRangeException("DebugLevel");
			}
			LogLevel logLevel2 = logLevel;
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				LoggingInterface.SetLogLevel(LogCategory.AllCategories, logLevel2);
				string launchArgument = GameUtils.GetLaunchArgument("debugeac");
				if (launchArgument != null)
				{
					LoggingInterface.SetLogLevel(LogCategory.AntiCheat, (launchArgument == "verbose") ? LogLevel.Verbose : LogLevel.Info);
				}
				else
				{
					LoggingInterface.SetLogLevel(LogCategory.AntiCheat, LogLevel.Warning);
				}
				if (logLevel2 == LogLevel.VeryVerbose)
				{
					LoggingInterface.SetLogLevel(LogCategory.Http, LogLevel.Verbose);
				}
				LoggingInterface.SetLogLevel(LogCategory.Analytics, LogLevel.Error);
				LoggingInterface.SetLogLevel(LogCategory.Messaging, LogLevel.Warning);
				LoggingInterface.SetLogLevel(LogCategory.Ecom, LogLevel.Error);
				LoggingInterface.SetLogLevel(LogCategory.Auth, LogLevel.Error);
				LoggingInterface.SetLogLevel(LogCategory.Presence, LogLevel.Warning);
				LoggingInterface.SetLogLevel(LogCategory.Overlay, LogLevel.Warning);
				LoggingInterface.SetLogLevel(LogCategory.Ui, LogLevel.Warning);
				LoggingInterface.SetCallback(new LogMessageFunc(this.logCallback));
			}
			this.PlatformInterface = this.createPlatformInterface(_creds, _serverMode);
			if (this.PlatformInterface == null)
			{
				this.ClientApiStatus = EApiStatus.PermanentError;
				Log.Error("[EOS] Failed to create platform");
				return;
			}
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (((nativePlatform != null) ? nativePlatform.ApplicationState : null) != null)
			{
				PlatformManager.NativePlatform.ApplicationState.OnApplicationStateChanged += this.OnApplicationStateChanged;
			}
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.ConnectInterface = this.PlatformInterface.GetConnectInterface();
			}
			if (this.ConnectInterface == null)
			{
				this.ClientApiStatus = EApiStatus.PermanentError;
				Log.Error("[EOS] Failed to get connect interface");
				return;
			}
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				this.SanctionsInterface = this.PlatformInterface.GetSanctionsInterface();
			}
			if (this.SanctionsInterface == null)
			{
				this.ClientApiStatus = EApiStatus.PermanentError;
				Log.Error("[EOS] Failed to get sanctions interface");
				return;
			}
			this.ClientApiStatus = EApiStatus.Ok;
			Action action = this.clientApiInitialized;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600DD18 RID: 56600 RVA: 0x004EC37C File Offset: 0x004EA57C
		[PublicizedFrom(EAccessModifier.Private)]
		public PlatformInterface createPlatformInterface(EosCreds _creds, bool _serverMode)
		{
			WindowsOptions windowsOptions = default(WindowsOptions);
			windowsOptions.ProductId = _creds.ProductId;
			windowsOptions.SandboxId = _creds.SandboxId;
			windowsOptions.ClientCredentials = new ClientCredentials
			{
				ClientId = _creds.ClientId,
				ClientSecret = _creds.ClientSecret
			};
			windowsOptions.DeploymentId = _creds.DeploymentId;
			windowsOptions.EncryptionKey = "0000000000000000000000000000000000000000000000000000000000000000";
			windowsOptions.IsServer = _serverMode;
			windowsOptions.Flags = PlatformFlags.DisableOverlay;
			windowsOptions.Flags |= PlatformFlags.DisableSocialOverlay;
			windowsOptions.RTCOptions = null;
			windowsOptions.RTCOptions = new WindowsRTCOptions?(new WindowsRTCOptions
			{
				PlatformSpecificOptions = new WindowsRTCOptionsPlatformSpecificOptions?(new WindowsRTCOptionsPlatformSpecificOptions
				{
					XAudio29DllPath = GameIO.GetGameDir("7DaysToDie_Data/Plugins/x86_64/xaudio2_9redist.dll")
				})
			});
			windowsOptions.CacheDirectory = GameIO.GetDeviceLocalUserGameDataDir();
			if (!Directory.Exists(windowsOptions.CacheDirectory))
			{
				Directory.CreateDirectory(windowsOptions.CacheDirectory);
			}
			object lockObject = AntiCheatCommon.LockObject;
			PlatformInterface platformInterface;
			lock (lockObject)
			{
				platformInterface = PlatformInterface.Create(ref windowsOptions);
			}
			return platformInterface;
		}

		// Token: 0x0600DD19 RID: 56601 RVA: 0x004EC4F0 File Offset: 0x004EA6F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void logCallback(ref LogMessage _message)
		{
			if (_message.Level == LogLevel.Warning && _message.Category == "LogHttp")
			{
				this.httpWarningCount++;
				if (this.httpWarningCount == 50)
				{
					this.httpNextTime = Time.unscaledTime + 600f;
					return;
				}
				if (this.httpWarningCount > 50)
				{
					if (Time.unscaledTime < this.httpNextTime)
					{
						return;
					}
					Log.Out(string.Format("[EOS] [LogHttp - Warning] Skipped {0} warnings within the last {1} seconds!", this.httpWarningCount - 50, 600f));
					this.httpWarningCount = 0;
				}
			}
			if (Api.DebugLevel == Api.EDebugLevel.Off && _message.Level == LogLevel.Warning && _message.Category == "LogEOSRTC" && _message.Message.StartsWith("TickTracker Ticks have been delayed.", StringComparison.Ordinal))
			{
				return;
			}
			string text = string.Format("[EOS] [{0} - {1}] {2}", _message.Category, _message.Level.ToStringCached<LogLevel>(), _message.Message);
			LogLevel level = _message.Level;
			if (level > LogLevel.Error)
			{
				if (level <= LogLevel.Info)
				{
					if (level == LogLevel.Warning)
					{
						Log.Warning(text);
						return;
					}
					if (level != LogLevel.Info)
					{
						goto IL_016D;
					}
				}
				else if (level != LogLevel.Verbose && level != LogLevel.VeryVerbose)
				{
					goto IL_016D;
				}
				Log.Out(text);
				return;
			}
			if (level == LogLevel.Off)
			{
				Log.Error(text);
				throw new ArgumentOutOfRangeException();
			}
			if (level == LogLevel.Fatal || level == LogLevel.Error)
			{
				Log.Error(text);
				return;
			}
			IL_016D:
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x0400A6BE RID: 42686
		public static readonly Api.EDebugLevel DebugLevel = Api.EDebugLevel.Off;

		// Token: 0x0400A6BF RID: 42687
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A6C0 RID: 42688
		public PlatformInterface PlatformInterface;

		// Token: 0x0400A6C1 RID: 42689
		public ConnectInterface ConnectInterface;

		// Token: 0x0400A6C2 RID: 42690
		public SanctionsInterface SanctionsInterface;

		// Token: 0x0400A6C3 RID: 42691
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);

		// Token: 0x0400A6C4 RID: 42692
		[PublicizedFrom(EAccessModifier.Internal)]
		public readonly SanctionsCheck eosSanctionsCheck = new SanctionsCheck();

		// Token: 0x0400A6C6 RID: 42694
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A6C7 RID: 42695
		[PublicizedFrom(EAccessModifier.Private)]
		public const int httpWarningLimit = 50;

		// Token: 0x0400A6C8 RID: 42696
		[PublicizedFrom(EAccessModifier.Private)]
		public const float httpWarningTimeout = 600f;

		// Token: 0x0400A6C9 RID: 42697
		[PublicizedFrom(EAccessModifier.Private)]
		public int httpWarningCount;

		// Token: 0x0400A6CA RID: 42698
		[PublicizedFrom(EAccessModifier.Private)]
		public float httpNextTime;

		// Token: 0x02001D17 RID: 7447
		public enum EDebugLevel
		{
			// Token: 0x0400A6CC RID: 42700
			Off,
			// Token: 0x0400A6CD RID: 42701
			Normal,
			// Token: 0x0400A6CE RID: 42702
			Verbose
		}
	}
}
