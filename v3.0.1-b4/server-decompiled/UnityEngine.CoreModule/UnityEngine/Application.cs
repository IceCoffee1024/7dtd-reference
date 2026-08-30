using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine.Bindings;
using UnityEngine.Diagnostics;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020000E8 RID: 232
	[NativeHeader("Runtime/Input/GetInput.h")]
	[NativeHeader("Runtime/Misc/PlayerSettings.h")]
	[NativeHeader("Runtime/Utilities/URLUtility.h")]
	[NativeHeader("Runtime/PreloadManager/PreloadManager.h")]
	[NativeHeader("Runtime/Input/InputManager.h")]
	[NativeHeader("Runtime/Misc/SystemInfo.h")]
	[NativeHeader("Runtime/Network/NetworkUtility.h")]
	[NativeHeader("Runtime/Misc/Player.h")]
	[NativeHeader("Runtime/Misc/BuildSettings.h")]
	[NativeHeader("Runtime/Input/TargetFrameRate.h")]
	[NativeHeader("Runtime/Logging/LogSystem.h")]
	[NativeHeader("Runtime/PreloadManager/LoadSceneOperation.h")]
	[NativeHeader("Runtime/Export/Application/Application.bindings.h")]
	[NativeHeader("Runtime/Utilities/Argv.h")]
	[NativeHeader("Runtime/Application/ApplicationInfo.h")]
	[NativeHeader("Runtime/Application/AdsIdHandler.h")]
	[NativeHeader("Runtime/File/ApplicationSpecificPersistentDataPath.h")]
	[NativeHeader("Runtime/BaseClasses/IsPlaying.h")]
	public class Application
	{
		// Token: 0x0600043D RID: 1085
		[FreeFunction("GetInputManager().QuitApplication")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Quit(int exitCode);

		// Token: 0x0600043E RID: 1086 RVA: 0x00007237 File Offset: 0x00005437
		public static void Quit()
		{
			Application.Quit(0);
		}

		// Token: 0x0600043F RID: 1087
		[FreeFunction("GetInputManager().CancelQuitApplication")]
		[Obsolete("CancelQuit is deprecated. Use the wantsToQuit event instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CancelQuit();

		// Token: 0x06000440 RID: 1088
		[FreeFunction("Application_Bindings::Unload")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Unload();

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000441 RID: 1089
		[Obsolete("This property is deprecated, please use LoadLevelAsync to detect if a specific scene is currently loading.")]
		public static extern bool isLoadingLevel
		{
			[FreeFunction("GetPreloadManager().IsLoadingOrQueued")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000442 RID: 1090
		[FreeFunction("UpdateMemoryUsage")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SimulateMemoryUsage(ApplicationMemoryUsage usage);

		// Token: 0x06000443 RID: 1091 RVA: 0x00007244 File Offset: 0x00005444
		[Obsolete("Streaming was a Unity Web Player feature, and is removed. This function is deprecated and always returns 1.0 for valid level indices.")]
		public static float GetStreamProgressForLevel(int levelIndex)
		{
			bool flag = levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings;
			float num;
			if (flag)
			{
				num = 1f;
			}
			else
			{
				num = 0f;
			}
			return num;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00007278 File Offset: 0x00005478
		[Obsolete("Streaming was a Unity Web Player feature, and is removed. This function is deprecated and always returns 1.0.")]
		public static float GetStreamProgressForLevel(string levelName)
		{
			return 1f;
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00007290 File Offset: 0x00005490
		[Obsolete("Streaming was a Unity Web Player feature, and is removed. This property is deprecated and always returns 0.")]
		public static int streamedBytes
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x000072A4 File Offset: 0x000054A4
		[Obsolete("Application.webSecurityEnabled is no longer supported, since the Unity Web Player is no longer supported by Unity", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool webSecurityEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000072B8 File Offset: 0x000054B8
		public static bool CanStreamedLevelBeLoaded(int levelIndex)
		{
			return levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings;
		}

		// Token: 0x06000448 RID: 1096
		[FreeFunction("Application_Bindings::CanStreamedLevelBeLoaded")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CanStreamedLevelBeLoaded(string levelName);

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000449 RID: 1097
		public static extern bool isPlaying
		{
			[FreeFunction("IsWorldPlaying")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600044A RID: 1098
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsPlaying([NotNull("NullExceptionObject")] Object obj);

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600044B RID: 1099
		public static extern bool isFocused
		{
			[FreeFunction("IsPlayerFocused")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600044C RID: 1100
		[FreeFunction("GetBuildSettings().GetBuildTags")]
		[Obsolete("Application.GetBuildTags is no longer supported and will be removed.", false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetBuildTags();

		// Token: 0x0600044D RID: 1101
		[Obsolete("Application.SetBuildTags is no longer supported and will be removed.", false)]
		[FreeFunction("GetBuildSettings().SetBuildTags")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBuildTags(string[] buildTags);

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600044E RID: 1102
		public static extern string buildGUID
		{
			[FreeFunction("Application_Bindings::GetBuildGUID")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600044F RID: 1103
		// (set) Token: 0x06000450 RID: 1104
		public static extern bool runInBackground
		{
			[FreeFunction("GetPlayerSettingsRunInBackground")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetPlayerSettingsRunInBackground")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000451 RID: 1105
		[FreeFunction("GetBuildSettings().GetHasPROVersion")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasProLicense();

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000452 RID: 1106
		public static extern bool isBatchMode
		{
			[FreeFunction("::IsBatchmode")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000453 RID: 1107
		internal static extern bool isTestRun
		{
			[FreeFunction("::IsTestRun")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000454 RID: 1108
		internal static extern bool isHumanControllingUs
		{
			[FreeFunction("::IsHumanControllingUs")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000455 RID: 1109
		[FreeFunction("HasARGV")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasARGV(string name);

		// Token: 0x06000456 RID: 1110
		[FreeFunction("GetFirstValueForARGV")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetValueForARGV(string name);

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000457 RID: 1111
		public static extern string dataPath
		{
			[FreeFunction("GetAppDataPath", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000458 RID: 1112
		public static extern string streamingAssetsPath
		{
			[FreeFunction("GetStreamingAssetsPath", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000459 RID: 1113
		public static extern string persistentDataPath
		{
			[FreeFunction("GetPersistentDataPathApplicationSpecific")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600045A RID: 1114
		public static extern string temporaryCachePath
		{
			[FreeFunction("GetTemporaryCachePathApplicationSpecific")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600045B RID: 1115
		public static extern string absoluteURL
		{
			[FreeFunction("GetPlayerSettings().GetAbsoluteURL")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000072DC File Offset: 0x000054DC
		[Obsolete("Application.ExternalEval is deprecated. See https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html for alternatives.")]
		public static void ExternalEval(string script)
		{
			bool flag = script.Length > 0 && script[script.Length - 1] != ';';
			if (flag)
			{
				script += ";";
			}
			Application.Internal_ExternalCall(script);
		}

		// Token: 0x0600045D RID: 1117
		[FreeFunction("Application_Bindings::ExternalCall")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ExternalCall(string script);

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600045E RID: 1118
		public static extern string unityVersion
		{
			[FreeFunction("Application_Bindings::GetUnityVersion", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600045F RID: 1119
		internal static extern int unityVersionVer
		{
			[FreeFunction("Application_Bindings::GetUnityVersionVer", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000460 RID: 1120
		internal static extern int unityVersionMaj
		{
			[FreeFunction("Application_Bindings::GetUnityVersionMaj", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000461 RID: 1121
		internal static extern int unityVersionMin
		{
			[FreeFunction("Application_Bindings::GetUnityVersionMin", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000462 RID: 1122
		public static extern string version
		{
			[FreeFunction("GetApplicationInfo().GetVersion")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000463 RID: 1123
		public static extern string installerName
		{
			[FreeFunction("GetApplicationInfo().GetInstallerName")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000464 RID: 1124
		public static extern string identifier
		{
			[FreeFunction("GetApplicationInfo().GetApplicationIdentifier")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000465 RID: 1125
		public static extern ApplicationInstallMode installMode
		{
			[FreeFunction("GetApplicationInfo().GetInstallMode")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000466 RID: 1126
		public static extern ApplicationSandboxType sandboxType
		{
			[FreeFunction("GetApplicationInfo().GetSandboxType")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000467 RID: 1127
		public static extern string productName
		{
			[FreeFunction("GetPlayerSettings().GetProductName")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000468 RID: 1128
		public static extern string companyName
		{
			[FreeFunction("GetPlayerSettings().GetCompanyName")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000469 RID: 1129
		public static extern string cloudProjectId
		{
			[FreeFunction("GetPlayerSettings().GetCloudProjectId")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600046A RID: 1130
		[FreeFunction("GetAdsIdHandler().RequestAdsIdAsync")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool RequestAdvertisingIdentifierAsync(Application.AdvertisingIdentifierCallback delegateMethod);

		// Token: 0x0600046B RID: 1131
		[FreeFunction("OpenURL")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void OpenURL(string url);

		// Token: 0x0600046C RID: 1132 RVA: 0x00007323 File Offset: 0x00005523
		[Obsolete("Use UnityEngine.Diagnostics.Utils.ForceCrash")]
		public static void ForceCrash(int mode)
		{
			Utils.ForceCrash((ForcedCrashCategory)mode);
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600046D RID: 1133
		// (set) Token: 0x0600046E RID: 1134
		public static extern int targetFrameRate
		{
			[FreeFunction("GetTargetFrameRate")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetTargetFrameRate")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600046F RID: 1135
		[FreeFunction("Application_Bindings::SetLogCallbackDefined")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLogCallbackDefined(bool defined);

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000470 RID: 1136
		// (set) Token: 0x06000471 RID: 1137
		[Obsolete("Use SetStackTraceLogType/GetStackTraceLogType instead")]
		public static extern StackTraceLogType stackTraceLogType
		{
			[FreeFunction("Application_Bindings::GetStackTraceLogType")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Application_Bindings::SetStackTraceLogType")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000472 RID: 1138
		[FreeFunction("GetStackTraceLogType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern StackTraceLogType GetStackTraceLogType(LogType logType);

		// Token: 0x06000473 RID: 1139
		[FreeFunction("SetStackTraceLogType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStackTraceLogType(LogType logType, StackTraceLogType stackTraceType);

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000474 RID: 1140
		public static extern string consoleLogPath
		{
			[FreeFunction("GetConsoleLogPath")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000475 RID: 1141
		// (set) Token: 0x06000476 RID: 1142
		public static extern ThreadPriority backgroundLoadingPriority
		{
			[FreeFunction("GetPreloadManager().GetThreadPriority")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("GetPreloadManager().SetThreadPriority")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000477 RID: 1143
		public static extern bool genuine
		{
			[FreeFunction("IsApplicationGenuine")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000478 RID: 1144
		public static extern bool genuineCheckAvailable
		{
			[FreeFunction("IsApplicationGenuineAvailable")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000479 RID: 1145
		[FreeFunction("Application_Bindings::RequestUserAuthorization")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AsyncOperation RequestUserAuthorization(UserAuthorization mode);

		// Token: 0x0600047A RID: 1146
		[FreeFunction("Application_Bindings::HasUserAuthorization")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasUserAuthorization(UserAuthorization mode);

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600047B RID: 1147
		internal static extern bool submitAnalytics
		{
			[FreeFunction("GetPlayerSettings().GetSubmitAnalytics")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x00007330 File Offset: 0x00005530
		[Obsolete("This property is deprecated, please use SplashScreen.isFinished instead")]
		public static bool isShowingSplashScreen
		{
			get
			{
				return !SplashScreen.isFinished;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600047D RID: 1149
		public static extern RuntimePlatform platform
		{
			[FreeFunction("systeminfo::GetRuntimePlatform", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000734C File Offset: 0x0000554C
		public static bool isMobilePlatform
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				RuntimePlatform runtimePlatform = platform;
				if (runtimePlatform <= RuntimePlatform.Android)
				{
					if (runtimePlatform != RuntimePlatform.IPhonePlayer && runtimePlatform != RuntimePlatform.Android)
					{
						goto IL_003A;
					}
				}
				else
				{
					if (runtimePlatform - RuntimePlatform.MetroPlayerX86 <= 2)
					{
						return SystemInfo.deviceType == DeviceType.Handheld;
					}
					if (runtimePlatform != RuntimePlatform.VisionOS)
					{
						goto IL_003A;
					}
				}
				return true;
				IL_003A:
				return false;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x00007398 File Offset: 0x00005598
		public static bool isConsolePlatform
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				return platform == RuntimePlatform.GameCoreXboxOne || platform == RuntimePlatform.GameCoreXboxSeries || platform == RuntimePlatform.PS4 || platform == RuntimePlatform.PS5 || platform == RuntimePlatform.Switch || platform == RuntimePlatform.XboxOne;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000480 RID: 1152
		public static extern SystemLanguage systemLanguage
		{
			[FreeFunction("(SystemLanguage)systeminfo::GetSystemLanguage")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000481 RID: 1153
		public static extern NetworkReachability internetReachability
		{
			[FreeFunction("GetInternetReachability")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000482 RID: 1154 RVA: 0x000073D4 File Offset: 0x000055D4
		// (remove) Token: 0x06000483 RID: 1155 RVA: 0x00007408 File Offset: 0x00005608
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Application.LowMemoryCallback lowMemory;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000484 RID: 1156 RVA: 0x0000743C File Offset: 0x0000563C
		// (remove) Token: 0x06000485 RID: 1157 RVA: 0x00007470 File Offset: 0x00005670
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Application.MemoryUsageChangedCallback memoryUsageChanged;

		// Token: 0x06000486 RID: 1158 RVA: 0x000074A4 File Offset: 0x000056A4
		[RequiredByNativeCode]
		internal static void CallLowMemory(ApplicationMemoryUsage usage)
		{
			Application.MemoryUsageChangedCallback memoryUsageChangedCallback = Application.memoryUsageChanged;
			bool flag = memoryUsageChangedCallback != null;
			if (flag)
			{
				ApplicationMemoryUsageChange applicationMemoryUsageChange = new ApplicationMemoryUsageChange(usage);
				memoryUsageChangedCallback(in applicationMemoryUsageChange);
			}
			if (usage > ApplicationMemoryUsage.High)
			{
				if (usage != ApplicationMemoryUsage.Critical)
				{
					throw new Exception(string.Format("Unknown application memory usage: {0}", usage));
				}
				Application.LowMemoryCallback lowMemoryCallback = Application.lowMemory;
				bool flag2 = lowMemoryCallback != null;
				if (flag2)
				{
					lowMemoryCallback();
				}
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000751C File Offset: 0x0000571C
		[RequiredByNativeCode]
		internal static bool HasLogCallback()
		{
			return Application.s_LogCallbackHandler != null || Application.s_LogCallbackHandlerThreaded != null;
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000488 RID: 1160 RVA: 0x00007540 File Offset: 0x00005740
		// (remove) Token: 0x06000489 RID: 1161 RVA: 0x0000755F File Offset: 0x0000575F
		public static event Application.LogCallback logMessageReceived
		{
			add
			{
				Application.s_LogCallbackHandler = (Application.LogCallback)Delegate.Combine(Application.s_LogCallbackHandler, value);
				Application.SetLogCallbackDefined(true);
			}
			remove
			{
				Application.s_LogCallbackHandler = (Application.LogCallback)Delegate.Remove(Application.s_LogCallbackHandler, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600048A RID: 1162 RVA: 0x00007577 File Offset: 0x00005777
		// (remove) Token: 0x0600048B RID: 1163 RVA: 0x00007596 File Offset: 0x00005796
		public static event Application.LogCallback logMessageReceivedThreaded
		{
			add
			{
				Application.s_LogCallbackHandlerThreaded = (Application.LogCallback)Delegate.Combine(Application.s_LogCallbackHandlerThreaded, value);
				Application.SetLogCallbackDefined(true);
			}
			remove
			{
				Application.s_LogCallbackHandlerThreaded = (Application.LogCallback)Delegate.Remove(Application.s_LogCallbackHandlerThreaded, value);
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000075B0 File Offset: 0x000057B0
		[RequiredByNativeCode]
		private static void CallLogCallback(string logString, string stackTrace, LogType type, bool invokedOnMainThread)
		{
			if (invokedOnMainThread)
			{
				Application.LogCallback logCallback = Application.s_LogCallbackHandler;
				bool flag = logCallback != null;
				if (flag)
				{
					logCallback(logString, stackTrace, type);
				}
			}
			Application.LogCallback logCallback2 = Application.s_LogCallbackHandlerThreaded;
			bool flag2 = logCallback2 != null;
			if (flag2)
			{
				logCallback2(logString, stackTrace, type);
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000075F8 File Offset: 0x000057F8
		internal static void InvokeOnAdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled)
		{
			bool flag = Application.OnAdvertisingIdentifierCallback != null;
			if (flag)
			{
				Application.OnAdvertisingIdentifierCallback(advertisingId, trackingEnabled, string.Empty);
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00007624 File Offset: 0x00005824
		private static string ObjectToJSString(object o)
		{
			bool flag = o == null;
			string text;
			if (flag)
			{
				text = "null";
			}
			else
			{
				bool flag2 = o is string;
				if (flag2)
				{
					string text2 = o.ToString().Replace("\\", "\\\\");
					text2 = text2.Replace("\"", "\\\"");
					text2 = text2.Replace("\n", "\\n");
					text2 = text2.Replace("\r", "\\r");
					text2 = text2.Replace("\0", "");
					text2 = text2.Replace("\u2028", "");
					text2 = text2.Replace("\u2029", "");
					text = "\"" + text2 + "\"";
				}
				else
				{
					bool flag3 = o is int || o is short || o is uint || o is ushort || o is byte;
					if (flag3)
					{
						text = o.ToString();
					}
					else
					{
						bool flag4 = o is float;
						if (flag4)
						{
							NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
							text = ((float)o).ToString(numberFormat);
						}
						else
						{
							bool flag5 = o is double;
							if (flag5)
							{
								NumberFormatInfo numberFormat2 = CultureInfo.InvariantCulture.NumberFormat;
								text = ((double)o).ToString(numberFormat2);
							}
							else
							{
								bool flag6 = o is char;
								if (flag6)
								{
									bool flag7 = (char)o == '"';
									if (flag7)
									{
										text = "\"\\\"\"";
									}
									else
									{
										text = "\"" + o.ToString() + "\"";
									}
								}
								else
								{
									bool flag8 = o is IList;
									if (flag8)
									{
										IList list = (IList)o;
										StringBuilder stringBuilder = new StringBuilder();
										stringBuilder.Append("new Array(");
										int count = list.Count;
										for (int i = 0; i < count; i++)
										{
											bool flag9 = i != 0;
											if (flag9)
											{
												stringBuilder.Append(", ");
											}
											stringBuilder.Append(Application.ObjectToJSString(list[i]));
										}
										stringBuilder.Append(")");
										text = stringBuilder.ToString();
									}
									else
									{
										text = Application.ObjectToJSString(o.ToString());
									}
								}
							}
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000787A File Offset: 0x00005A7A
		[Obsolete("Application.ExternalCall is deprecated. See https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html for alternatives.")]
		public static void ExternalCall(string functionName, params object[] args)
		{
			Application.Internal_ExternalCall(Application.BuildInvocationForArguments(functionName, args));
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000788C File Offset: 0x00005A8C
		private static string BuildInvocationForArguments(string functionName, params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			int num = args.Length;
			for (int i = 0; i < num; i++)
			{
				bool flag = i != 0;
				if (flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(Application.ObjectToJSString(args[i]));
			}
			stringBuilder.Append(')');
			stringBuilder.Append(';');
			return stringBuilder.ToString();
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000790C File Offset: 0x00005B0C
		[Obsolete("use Application.isEditor instead")]
		public static bool isPlayer
		{
			get
			{
				return !Application.isEditor;
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00007928 File Offset: 0x00005B28
		[Obsolete("Use Object.DontDestroyOnLoad instead")]
		public static void DontDestroyOnLoad(Object o)
		{
			bool flag = o != null;
			if (flag)
			{
				Object.DontDestroyOnLoad(o);
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00007948 File Offset: 0x00005B48
		[Obsolete("Application.CaptureScreenshot is obsolete. Use ScreenCapture.CaptureScreenshot instead (UnityUpgradable) -> [UnityEngine] UnityEngine.ScreenCapture.CaptureScreenshot(*)", true)]
		public static void CaptureScreenshot(string filename, int superSize)
		{
			throw new NotSupportedException("Application.CaptureScreenshot is obsolete. Use ScreenCapture.CaptureScreenshot instead.");
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00007948 File Offset: 0x00005B48
		[Obsolete("Application.CaptureScreenshot is obsolete. Use ScreenCapture.CaptureScreenshot instead (UnityUpgradable) -> [UnityEngine] UnityEngine.ScreenCapture.CaptureScreenshot(*)", true)]
		public static void CaptureScreenshot(string filename)
		{
			throw new NotSupportedException("Application.CaptureScreenshot is obsolete. Use ScreenCapture.CaptureScreenshot instead.");
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000495 RID: 1173 RVA: 0x00007955 File Offset: 0x00005B55
		// (remove) Token: 0x06000496 RID: 1174 RVA: 0x0000795F File Offset: 0x00005B5F
		public static event UnityAction onBeforeRender
		{
			add
			{
				BeforeRenderHelper.RegisterCallback(value);
			}
			remove
			{
				BeforeRenderHelper.UnregisterCallback(value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000497 RID: 1175 RVA: 0x0000796C File Offset: 0x00005B6C
		// (remove) Token: 0x06000498 RID: 1176 RVA: 0x000079A0 File Offset: 0x00005BA0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<bool> focusChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000499 RID: 1177 RVA: 0x000079D4 File Offset: 0x00005BD4
		// (remove) Token: 0x0600049A RID: 1178 RVA: 0x00007A08 File Offset: 0x00005C08
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<string> deepLinkActivated;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600049B RID: 1179 RVA: 0x00007A3C File Offset: 0x00005C3C
		// (remove) Token: 0x0600049C RID: 1180 RVA: 0x00007A70 File Offset: 0x00005C70
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Func<bool> wantsToQuit;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600049D RID: 1181 RVA: 0x00007AA4 File Offset: 0x00005CA4
		// (remove) Token: 0x0600049E RID: 1182 RVA: 0x00007AD8 File Offset: 0x00005CD8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action quitting;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600049F RID: 1183 RVA: 0x00007B0C File Offset: 0x00005D0C
		// (remove) Token: 0x060004A0 RID: 1184 RVA: 0x00007B40 File Offset: 0x00005D40
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action unloading;

		// Token: 0x060004A1 RID: 1185 RVA: 0x00007B74 File Offset: 0x00005D74
		[RequiredByNativeCode]
		private static bool Internal_ApplicationWantsToQuit()
		{
			bool flag = Application.wantsToQuit != null;
			if (flag)
			{
				foreach (Func<bool> func in Application.wantsToQuit.GetInvocationList())
				{
					try
					{
						bool flag2 = !func();
						if (flag2)
						{
							return false;
						}
					}
					catch (Exception ex)
					{
						Debug.LogException(ex);
					}
				}
			}
			return true;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public static CancellationToken exitCancellationToken
		{
			get
			{
				return Application.s_currentCancellationTokenSource.Token;
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00007C00 File Offset: 0x00005E00
		[RequiredByNativeCode]
		private static void Internal_ApplicationInit()
		{
			Application.s_currentCancellationTokenSource = new CancellationTokenSource();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00007C10 File Offset: 0x00005E10
		[RequiredByNativeCode]
		private static void Internal_ApplicationQuit()
		{
			Application.s_currentCancellationTokenSource.Cancel();
			bool flag = Application.quitting != null;
			if (flag)
			{
				Application.quitting();
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00007C40 File Offset: 0x00005E40
		[RequiredByNativeCode]
		private static void Internal_ApplicationUnload()
		{
			bool flag = Application.unloading != null;
			if (flag)
			{
				Application.unloading();
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00007C65 File Offset: 0x00005E65
		[RequiredByNativeCode]
		internal static void InvokeOnBeforeRender()
		{
			BeforeRenderHelper.Invoke();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00007C70 File Offset: 0x00005E70
		[RequiredByNativeCode]
		internal static void InvokeFocusChanged(bool focus)
		{
			bool flag = Application.focusChanged != null;
			if (flag)
			{
				Application.focusChanged(focus);
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00007C98 File Offset: 0x00005E98
		[RequiredByNativeCode]
		internal static void InvokeDeepLinkActivated(string url)
		{
			bool flag = Application.deepLinkActivated != null;
			if (flag)
			{
				Application.deepLinkActivated(url);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00007CBE File Offset: 0x00005EBE
		[Obsolete("Application.RegisterLogCallback is deprecated. Use Application.logMessageReceived instead.")]
		public static void RegisterLogCallback(Application.LogCallback handler)
		{
			Application.RegisterLogCallback(handler, false);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00007CC9 File Offset: 0x00005EC9
		[Obsolete("Application.RegisterLogCallbackThreaded is deprecated. Use Application.logMessageReceivedThreaded instead.")]
		public static void RegisterLogCallbackThreaded(Application.LogCallback handler)
		{
			Application.RegisterLogCallback(handler, true);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00007CD4 File Offset: 0x00005ED4
		private static void RegisterLogCallback(Application.LogCallback handler, bool threaded)
		{
			bool flag = Application.s_RegisterLogCallbackDeprecated != null;
			if (flag)
			{
				Application.logMessageReceived -= Application.s_RegisterLogCallbackDeprecated;
				Application.logMessageReceivedThreaded -= Application.s_RegisterLogCallbackDeprecated;
			}
			Application.s_RegisterLogCallbackDeprecated = handler;
			bool flag2 = handler != null;
			if (flag2)
			{
				if (threaded)
				{
					Application.logMessageReceivedThreaded += handler;
				}
				else
				{
					Application.logMessageReceived += handler;
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00007D34 File Offset: 0x00005F34
		[Obsolete("Use SceneManager.sceneCountInBuildSettings")]
		public static int levelCount
		{
			get
			{
				return SceneManager.sceneCountInBuildSettings;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00007D4C File Offset: 0x00005F4C
		[Obsolete("Use SceneManager to determine what scenes have been loaded")]
		public static int loadedLevel
		{
			get
			{
				return SceneManager.GetActiveScene().buildIndex;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00007D6C File Offset: 0x00005F6C
		[Obsolete("Use SceneManager to determine what scenes have been loaded")]
		public static string loadedLevelName
		{
			get
			{
				return SceneManager.GetActiveScene().name;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00007D8B File Offset: 0x00005F8B
		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevel(int index)
		{
			SceneManager.LoadScene(index, LoadSceneMode.Single);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00007D96 File Offset: 0x00005F96
		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevel(string name)
		{
			SceneManager.LoadScene(name, LoadSceneMode.Single);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00007DA1 File Offset: 0x00005FA1
		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevelAdditive(int index)
		{
			SceneManager.LoadScene(index, LoadSceneMode.Additive);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00007DAC File Offset: 0x00005FAC
		[Obsolete("Use SceneManager.LoadScene")]
		public static void LoadLevelAdditive(string name)
		{
			SceneManager.LoadScene(name, LoadSceneMode.Additive);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00007DB8 File Offset: 0x00005FB8
		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAsync(int index)
		{
			return SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00007DD4 File Offset: 0x00005FD4
		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAsync(string levelName)
		{
			return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00007DF0 File Offset: 0x00005FF0
		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAdditiveAsync(int index)
		{
			return SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00007E0C File Offset: 0x0000600C
		[Obsolete("Use SceneManager.LoadSceneAsync")]
		public static AsyncOperation LoadLevelAdditiveAsync(string levelName)
		{
			return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00007E28 File Offset: 0x00006028
		[Obsolete("Use SceneManager.UnloadScene")]
		public static bool UnloadLevel(int index)
		{
			return SceneManager.UnloadScene(index);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00007E40 File Offset: 0x00006040
		[Obsolete("Use SceneManager.UnloadScene")]
		public static bool UnloadLevel(string scenePath)
		{
			return SceneManager.UnloadScene(scenePath);
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00007E58 File Offset: 0x00006058
		public static bool isEditor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000290 RID: 656
		private static Application.LogCallback s_LogCallbackHandler;

		// Token: 0x04000291 RID: 657
		private static Application.LogCallback s_LogCallbackHandlerThreaded;

		// Token: 0x04000292 RID: 658
		internal static Application.AdvertisingIdentifierCallback OnAdvertisingIdentifierCallback;

		// Token: 0x04000298 RID: 664
		private static CancellationTokenSource s_currentCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04000299 RID: 665
		private static volatile Application.LogCallback s_RegisterLogCallbackDeprecated;

		// Token: 0x020000E9 RID: 233
		// (Invoke) Token: 0x060004BD RID: 1213
		public delegate void AdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled, string errorMsg);

		// Token: 0x020000EA RID: 234
		// (Invoke) Token: 0x060004C1 RID: 1217
		public delegate void LowMemoryCallback();

		// Token: 0x020000EB RID: 235
		// (Invoke) Token: 0x060004C5 RID: 1221
		public delegate void MemoryUsageChangedCallback(in ApplicationMemoryUsageChange usage);

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x060004C9 RID: 1225
		public delegate void LogCallback(string condition, string stackTrace, LogType type);
	}
}
