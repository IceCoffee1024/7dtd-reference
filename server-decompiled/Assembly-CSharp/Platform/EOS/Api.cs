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
	// Token: 0x02001CE4 RID: 7396
	public class Api : IPlatformApi
	{
		// Token: 0x0600DB8F RID: 56207 RVA: 0x004E722C File Offset: 0x004E542C
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

		// Token: 0x0600DB90 RID: 56208 RVA: 0x004E7267 File Offset: 0x004E5467
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
		}

		// Token: 0x0600DB91 RID: 56209 RVA: 0x004E7270 File Offset: 0x004E5470
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

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x0600DB92 RID: 56210 RVA: 0x004E72EC File Offset: 0x004E54EC
		// (set) Token: 0x0600DB93 RID: 56211 RVA: 0x004E72F4 File Offset: 0x004E54F4
		public EApiStatus ClientApiStatus
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = EApiStatus.Uninitialized;

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x0600DB94 RID: 56212 RVA: 0x004E7300 File Offset: 0x004E5500
		// (remove) Token: 0x0600DB95 RID: 56213 RVA: 0x004E735C File Offset: 0x004E555C
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

		// Token: 0x0600DB96 RID: 56214 RVA: 0x004E73A8 File Offset: 0x004E55A8
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

		// Token: 0x0600DB97 RID: 56215 RVA: 0x004E73E9 File Offset: 0x004E55E9
		public bool InitServerApis()
		{
			return this.InitClientApis();
		}

		// Token: 0x0600DB98 RID: 56216 RVA: 0x000027FC File Offset: 0x000009FC
		public void ServerApiLoaded()
		{
		}

		// Token: 0x0600DB99 RID: 56217 RVA: 0x004E73F4 File Offset: 0x004E55F4
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

		// Token: 0x0600DB9A RID: 56218 RVA: 0x004E7480 File Offset: 0x004E5680
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

		// Token: 0x0600DB9B RID: 56219 RVA: 0x00040F84 File Offset: 0x0003F184
		public float GetScreenBoundsValueFromSystem()
		{
			return 1f;
		}

		// Token: 0x0600DB9C RID: 56220 RVA: 0x004E750C File Offset: 0x004E570C
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

		// Token: 0x0600DB9D RID: 56221 RVA: 0x004E7834 File Offset: 0x004E5A34
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

		// Token: 0x0600DB9E RID: 56222 RVA: 0x004E79A8 File Offset: 0x004E5BA8
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

		// Token: 0x0400A5E9 RID: 42473
		public static readonly Api.EDebugLevel DebugLevel = Api.EDebugLevel.Off;

		// Token: 0x0400A5EA RID: 42474
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A5EB RID: 42475
		public PlatformInterface PlatformInterface;

		// Token: 0x0400A5EC RID: 42476
		public ConnectInterface ConnectInterface;

		// Token: 0x0400A5ED RID: 42477
		public SanctionsInterface SanctionsInterface;

		// Token: 0x0400A5EE RID: 42478
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MicroStopwatch tickDurationStopwatch = new MicroStopwatch(false);

		// Token: 0x0400A5EF RID: 42479
		[PublicizedFrom(EAccessModifier.Internal)]
		public readonly SanctionsCheck eosSanctionsCheck = new SanctionsCheck();

		// Token: 0x0400A5F1 RID: 42481
		[PublicizedFrom(EAccessModifier.Private)]
		public Action clientApiInitialized;

		// Token: 0x0400A5F2 RID: 42482
		[PublicizedFrom(EAccessModifier.Private)]
		public const int httpWarningLimit = 50;

		// Token: 0x0400A5F3 RID: 42483
		[PublicizedFrom(EAccessModifier.Private)]
		public const float httpWarningTimeout = 600f;

		// Token: 0x0400A5F4 RID: 42484
		[PublicizedFrom(EAccessModifier.Private)]
		public int httpWarningCount;

		// Token: 0x0400A5F5 RID: 42485
		[PublicizedFrom(EAccessModifier.Private)]
		public float httpNextTime;

		// Token: 0x02001CE5 RID: 7397
		public enum EDebugLevel
		{
			// Token: 0x0400A5F7 RID: 42487
			Off,
			// Token: 0x0400A5F8 RID: 42488
			Normal,
			// Token: 0x0400A5F9 RID: 42489
			Verbose
		}
	}
}
