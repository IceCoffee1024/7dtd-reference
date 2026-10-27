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
	// Token: 0x02001D47 RID: 7495
	public class Api : IPlatformApi
	{
		// Token: 0x0600DE1E RID: 56862 RVA: 0x004EF914 File Offset: 0x004EDB14
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

		// Token: 0x0600DE1F RID: 56863 RVA: 0x004EF94F File Offset: 0x004EDB4F
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DE20 RID: 56864 RVA: 0x004EF958 File Offset: 0x004EDB58
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

		// Token: 0x17001C55 RID: 7253
		// (get) Token: 0x0600DE21 RID: 56865 RVA: 0x004EF9D4 File Offset: 0x004EDBD4
		// (set) Token: 0x0600DE22 RID: 56866 RVA: 0x004EF9DC File Offset: 0x004EDBDC
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x0600DE23 RID: 56867 RVA: 0x004EF9E8 File Offset: 0x004EDBE8
		// (remove) Token: 0x0600DE24 RID: 56868 RVA: 0x004EFA44 File Offset: 0x004EDC44
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

		// Token: 0x0600DE25 RID: 56869 RVA: 0x004EFA90 File Offset: 0x004EDC90
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

		// Token: 0x0600DE26 RID: 56870 RVA: 0x004EFAD1 File Offset: 0x004EDCD1
		public bool InitServerApis()
		{
			return this.InitClientApis();
		}

		// Token: 0x0600DE27 RID: 56871 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600DE28 RID: 56872 RVA: 0x004EFADC File Offset: 0x004EDCDC
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

		// Token: 0x0600DE29 RID: 56873 RVA: 0x004EFB68 File Offset: 0x004EDD68
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

		// Token: 0x0600DE2A RID: 56874 RVA: 0x00041024 File Offset: 0x0003F224
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600DE2B RID: 56875 RVA: 0x004EFBF4 File Offset: 0x004EDDF4
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

		// Token: 0x0600DE2C RID: 56876 RVA: 0x004EFF1C File Offset: 0x004EE11C
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

		// Token: 0x0600DE2D RID: 56877 RVA: 0x004F0090 File Offset: 0x004EE290
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

		// Token: 0x0400A7A7 RID: 42919
		public static readonly Api.EDebugLevel DebugLevel = Api.EDebugLevel.Off;

		// Token: 0x0400A7A8 RID: 42920
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A7A9 RID: 42921
		public PlatformInterface PlatformInterface;

		// Token: 0x0400A7AA RID: 42922
		public ConnectInterface ConnectInterface;

		// Token: 0x0400A7AB RID: 42923
		public SanctionsInterface SanctionsInterface;

		// Token: 0x0400A7AC RID: 42924
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);

		// Token: 0x0400A7AD RID: 42925
		[PublicizedFrom(EAccessModifier.Internal)]
		public readonly SanctionsCheck eosSanctionsCheck = new SanctionsCheck();

		// Token: 0x0400A7AF RID: 42927
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A7B0 RID: 42928
		[PublicizedFrom(EAccessModifier.Private)]
		public const int httpWarningLimit = 50;

		// Token: 0x0400A7B1 RID: 42929
		[PublicizedFrom(EAccessModifier.Private)]
		public const float httpWarningTimeout = 600f;

		// Token: 0x0400A7B2 RID: 42930
		[PublicizedFrom(EAccessModifier.Private)]
		public int httpWarningCount;

		// Token: 0x0400A7B3 RID: 42931
		[PublicizedFrom(EAccessModifier.Private)]
		public float httpNextTime;

		// Token: 0x02001D48 RID: 7496
		public enum EDebugLevel
		{
			// Token: 0x0400A7B5 RID: 42933
			Off,
			// Token: 0x0400A7B6 RID: 42934
			Normal,
			// Token: 0x0400A7B7 RID: 42935
			Verbose
		}
	}
}
