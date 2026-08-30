using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform.MultiPlatform;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BAB RID: 7083
	public static class PlatformManager
	{
		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x0600D261 RID: 53857 RVA: 0x004C2428 File Offset: 0x004C0628
		// (set) Token: 0x0600D262 RID: 53858 RVA: 0x004C242F File Offset: 0x004C062F
		public static EDeviceType DeviceType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x0600D263 RID: 53859 RVA: 0x004C2437 File Offset: 0x004C0637
		// (set) Token: 0x0600D264 RID: 53860 RVA: 0x004C243E File Offset: 0x004C063E
		public static IPlatform MultiPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x0600D265 RID: 53861 RVA: 0x004C2446 File Offset: 0x004C0646
		// (set) Token: 0x0600D266 RID: 53862 RVA: 0x004C244D File Offset: 0x004C064D
		public static IPlatform NativePlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600D267 RID: 53863 RVA: 0x004C2455 File Offset: 0x004C0655
		// (set) Token: 0x0600D268 RID: 53864 RVA: 0x004C245C File Offset: 0x004C065C
		public static IPlatform CrossplatformPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x0600D269 RID: 53865 RVA: 0x004C2464 File Offset: 0x004C0664
		// (set) Token: 0x0600D26A RID: 53866 RVA: 0x004C246B File Offset: 0x004C066B
		public static ClientLobbyManager ClientLobbyManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x0600D26B RID: 53867 RVA: 0x004C2473 File Offset: 0x004C0673
		public static PlatformUserIdentifierAbs InternalLocalUserIdentifier
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				PlatformUserIdentifierAbs platformUserIdentifierAbs;
				if (crossplatformPlatform == null)
				{
					platformUserIdentifierAbs = null;
				}
				else
				{
					IUserClient user = crossplatformPlatform.User;
					platformUserIdentifierAbs = ((user != null) ? user.PlatformUserId : null);
				}
				return platformUserIdentifierAbs ?? PlatformManager.NativePlatform.User.PlatformUserId;
			}
		}

		// Token: 0x0600D26C RID: 53868 RVA: 0x004C24A8 File Offset: 0x004C06A8
		public static bool Init()
		{
			if (PlatformManager.initialized)
			{
				return true;
			}
			PlatformManager.DeviceType = EDeviceType.PC;
			try
			{
				PlatformManager.initialized = true;
				Log.Out("[Platform] Init");
				PlatformManager.FindSupportedPlatforms();
				PlatformConfiguration platformConfiguration = PlatformManager.DetectPlatform();
				PlatformManager.GetCommandLineOverrides(platformConfiguration);
				IPlatform platform;
				PlatformManager.initPlatformFromIdentifier(platformConfiguration.NativePlatform, "Native", out platform);
				PlatformManager.NativePlatform = platform;
				if (platformConfiguration.CrossPlatform != EPlatformIdentifier.None)
				{
					PlatformManager.initPlatformFromIdentifier(platformConfiguration.CrossPlatform, "Cross", out platform);
					platform.IsCrossplatform = true;
					PlatformManager.CrossplatformPlatform = platform;
				}
				PlatformManager.MultiPlatform = new Factory();
				using (List<EPlatformIdentifier>.Enumerator enumerator = platformConfiguration.ServerPlatforms.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (PlatformManager.initPlatformFromIdentifier(enumerator.Current, "Server", out platform))
						{
							platform.AsServerOnly = true;
						}
					}
				}
				PlatformManager.ClientLobbyManager = new ClientLobbyManager();
				PlatformManager.NativePlatform.CreateInstances();
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (crossplatformPlatform != null)
				{
					crossplatformPlatform.CreateInstances();
				}
				PlatformManager.MultiPlatform.CreateInstances();
				List<EPlatformIdentifier> list = new List<EPlatformIdentifier>();
				foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.serverPlatforms)
				{
					if (keyValuePair.Value.AsServerOnly)
					{
						try
						{
							keyValuePair.Value.CreateInstances();
						}
						catch (NotSupportedException ex)
						{
							Log.Error(string.Format("[Platform] Platform {0} Errored on init, removing from the list of server platforms. Error: {1}.", keyValuePair.Key, ex.Message));
							list.Add(keyValuePair.Key);
						}
					}
				}
				foreach (EPlatformIdentifier eplatformIdentifier in list)
				{
					IPlatform platform2;
					if (PlatformManager.serverPlatforms.TryGetValue(eplatformIdentifier, out platform2))
					{
						platformConfiguration.ServerPlatforms.Remove(eplatformIdentifier);
						platform2.Destroy();
						PlatformManager.serverPlatforms.Remove(eplatformIdentifier);
					}
				}
				list.Clear();
				PlatformManager.NativePlatform.Init();
				IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
				if (crossplatformPlatform2 != null)
				{
					crossplatformPlatform2.Init();
				}
				PlatformManager.MultiPlatform.Init();
				foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair2 in PlatformManager.serverPlatforms)
				{
					if (keyValuePair2.Value.AsServerOnly)
					{
						keyValuePair2.Value.Init();
					}
				}
				PlatformUserManager.Init();
			}
			catch (Exception ex2)
			{
				Log.Error("[Platform] Error while initializing platform code, shutting down.");
				Log.Exception(ex2);
				Application.Quit(1);
				return false;
			}
			return true;
		}

		// Token: 0x0600D26D RID: 53869 RVA: 0x004C27B0 File Offset: 0x004C09B0
		public static void Update()
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform != null)
			{
				nativePlatform.Update();
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null)
			{
				crossplatformPlatform.Update();
			}
			IPlatform multiPlatform = PlatformManager.MultiPlatform;
			if (multiPlatform != null)
			{
				multiPlatform.Update();
			}
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.serverPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly)
				{
					keyValuePair.Value.Update();
				}
			}
			PlatformUserManager.Update();
		}

		// Token: 0x0600D26E RID: 53870 RVA: 0x004C284C File Offset: 0x004C0A4C
		public static void LateUpdate()
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform != null)
			{
				nativePlatform.LateUpdate();
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null)
			{
				crossplatformPlatform.LateUpdate();
			}
			IPlatform multiPlatform = PlatformManager.MultiPlatform;
			if (multiPlatform != null)
			{
				multiPlatform.LateUpdate();
			}
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.serverPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly)
				{
					keyValuePair.Value.LateUpdate();
				}
			}
		}

		// Token: 0x0600D26F RID: 53871 RVA: 0x004C28E4 File Offset: 0x004C0AE4
		public static void Destroy()
		{
			PlatformUserManager.Destroy();
			foreach (KeyValuePair<EPlatformIdentifier, IPlatform> keyValuePair in PlatformManager.serverPlatforms)
			{
				if (keyValuePair.Value.AsServerOnly)
				{
					keyValuePair.Value.Destroy();
				}
			}
			IPlatform multiPlatform = PlatformManager.MultiPlatform;
			if (multiPlatform != null)
			{
				multiPlatform.Destroy();
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null)
			{
				crossplatformPlatform.Destroy();
			}
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform != null)
			{
				nativePlatform.Destroy();
			}
			PlatformManager.serverPlatforms.Clear();
			PlatformManager.MultiPlatform = null;
			PlatformManager.CrossplatformPlatform = null;
			PlatformManager.NativePlatform = null;
		}

		// Token: 0x0600D270 RID: 53872 RVA: 0x004C299C File Offset: 0x004C0B9C
		public static string PlatformStringFromEnum(EPlatformIdentifier _platformIdentifier)
		{
			return _platformIdentifier.ToStringCached<EPlatformIdentifier>();
		}

		// Token: 0x0600D271 RID: 53873 RVA: 0x004C29A4 File Offset: 0x004C0BA4
		public static bool TryPlatformIdentifierFromString(string _platformName, out EPlatformIdentifier _platformIdentifier)
		{
			return EnumUtils.TryParse<EPlatformIdentifier>(_platformName, out _platformIdentifier, true);
		}

		// Token: 0x0600D272 RID: 53874 RVA: 0x004C29B0 File Offset: 0x004C0BB0
		public static IPlatform InstanceForPlatformIdentifier(EPlatformIdentifier _platformIdentifier)
		{
			IPlatform platform;
			if (!PlatformManager.serverPlatforms.TryGetValue(_platformIdentifier, out platform))
			{
				return null;
			}
			return platform;
		}

		// Token: 0x0600D273 RID: 53875 RVA: 0x004C29CF File Offset: 0x004C0BCF
		public static bool IsPlatformLoaded(EPlatformIdentifier _platformIdentifier)
		{
			return PlatformManager.serverPlatforms.ContainsKey(_platformIdentifier);
		}

		// Token: 0x0600D274 RID: 53876 RVA: 0x004C29DC File Offset: 0x004C0BDC
		public static string GetPlatformDisplayName(EPlatformIdentifier _platformIdentifier)
		{
			return Localization.Get("platformName" + _platformIdentifier.ToStringCached<EPlatformIdentifier>(), false, null);
		}

		// Token: 0x0600D275 RID: 53877 RVA: 0x004C29F8 File Offset: 0x004C0BF8
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initPlatformFromIdentifier(EPlatformIdentifier _platformIdentifier, string _logName, out IPlatform _target)
		{
			Type type;
			if (!PlatformManager.supportedPlatforms.TryGetValue(_platformIdentifier, out type))
			{
				throw new NotSupportedException(string.Concat(new string[]
				{
					"[Platform] ",
					_logName,
					" platform ",
					_platformIdentifier.ToStringCached<EPlatformIdentifier>(),
					" not supported. Supported: ",
					PlatformManager.supportedPlatformsString
				}));
			}
			Log.Out("[Platform] Using " + _logName.ToLowerInvariant() + " platform: " + _platformIdentifier.ToStringCached<EPlatformIdentifier>());
			if (PlatformManager.serverPlatforms.ContainsKey(_platformIdentifier))
			{
				_target = null;
				return false;
			}
			_target = ReflectionHelpers.Instantiate<IPlatform>(type);
			PlatformManager.serverPlatforms.Add(_target.PlatformIdentifier, _target);
			return true;
		}

		// Token: 0x0600D276 RID: 53878 RVA: 0x004C2AA0 File Offset: 0x004C0CA0
		[PublicizedFrom(EAccessModifier.Private)]
		public static void FindSupportedPlatforms()
		{
			PlatformManager.supportedPlatforms.Clear();
			PlatformManager.supportedPlatformsString = "";
			Type typeFromHandle = typeof(IPlatform);
			Type attrType = typeof(PlatformFactoryAttribute);
			ReflectionHelpers.FindTypesImplementingBase(typeFromHandle, delegate(Type _type)
			{
				object[] customAttributes = _type.GetCustomAttributes(attrType, false);
				if (customAttributes.Length != 1)
				{
					return;
				}
				PlatformFactoryAttribute platformFactoryAttribute = (PlatformFactoryAttribute)customAttributes[0];
				Type type;
				if (PlatformManager.supportedPlatforms.TryGetValue(platformFactoryAttribute.TargetPlatform, out type))
				{
					Log.Error(string.Concat(new string[]
					{
						"[Platform] Multiple platform providers for platform ",
						platformFactoryAttribute.TargetPlatform.ToStringCached<EPlatformIdentifier>(),
						": Loaded '",
						type.FullName,
						"', found '",
						_type.FullName,
						"'"
					}));
					return;
				}
				PlatformManager.supportedPlatforms.Add(platformFactoryAttribute.TargetPlatform, _type);
				if (PlatformManager.supportedPlatformsString.Length > 0)
				{
					PlatformManager.supportedPlatformsString += ", ";
				}
				PlatformManager.supportedPlatformsString += platformFactoryAttribute.TargetPlatform.ToStringCached<EPlatformIdentifier>();
			}, false);
			PlatformManager.UserIdentifierFactories.Clear();
			Type typeFromHandle2 = typeof(AbsUserIdentifierFactory);
			Type attrType2 = typeof(UserIdentifierFactoryAttribute);
			ReflectionHelpers.FindTypesImplementingBase(typeFromHandle2, delegate(Type _type)
			{
				object[] customAttributes2 = _type.GetCustomAttributes(attrType2, false);
				if (customAttributes2.Length != 1)
				{
					return;
				}
				UserIdentifierFactoryAttribute userIdentifierFactoryAttribute = (UserIdentifierFactoryAttribute)customAttributes2[0];
				AbsUserIdentifierFactory absUserIdentifierFactory;
				if (PlatformManager.UserIdentifierFactories.TryGetValue(userIdentifierFactoryAttribute.TargetPlatform, out absUserIdentifierFactory))
				{
					Log.Error(string.Concat(new string[]
					{
						"[Platform] Multiple user identifier factories for platform ",
						userIdentifierFactoryAttribute.TargetPlatform.ToStringCached<EPlatformIdentifier>(),
						": Loaded '",
						absUserIdentifierFactory.GetType().FullName,
						"', found '",
						_type.FullName,
						"'"
					}));
					return;
				}
				AbsUserIdentifierFactory absUserIdentifierFactory2 = ReflectionHelpers.Instantiate<AbsUserIdentifierFactory>(_type);
				if (absUserIdentifierFactory2 == null)
				{
					return;
				}
				PlatformManager.UserIdentifierFactories.Add(userIdentifierFactoryAttribute.TargetPlatform, absUserIdentifierFactory2);
			}, false);
		}

		// Token: 0x0600D277 RID: 53879 RVA: 0x004C2B2C File Offset: 0x004C0D2C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void GetCommandLineOverrides(PlatformConfiguration _platforms)
		{
			string text = GameUtils.GetLaunchArgument("platform");
			if (!string.IsNullOrEmpty(text))
			{
				_platforms.ParsePlatform("platform", text);
			}
			text = GameUtils.GetLaunchArgument("crossplatform");
			if (!string.IsNullOrEmpty(text))
			{
				_platforms.ParsePlatform("crossplatform", text);
			}
			text = GameUtils.GetLaunchArgument("serverplatforms");
			if (!string.IsNullOrEmpty(text))
			{
				_platforms.ParsePlatform("serverplatforms", text);
			}
		}

		// Token: 0x0600D278 RID: 53880 RVA: 0x004C2B9C File Offset: 0x004C0D9C
		[PublicizedFrom(EAccessModifier.Private)]
		public static PlatformConfiguration DetectPlatform()
		{
			PlatformConfiguration platformConfiguration = null;
			if (PlatformConfiguration.ReadFile(ref platformConfiguration, null))
			{
				return platformConfiguration;
			}
			PlatformConfiguration platformConfiguration2 = new PlatformConfiguration();
			Log.Warning(string.Format("[Platform] No platform config file ({0}) found, defaulting to {1} / {2} without additional server platforms.", "platform.cfg", platformConfiguration2.NativePlatform, platformConfiguration2.CrossPlatform));
			return platformConfiguration2;
		}

		// Token: 0x0400A01C RID: 40988
		public const string PlatformConfigFileName = "platform.cfg";

		// Token: 0x0400A022 RID: 40994
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, IPlatform> serverPlatforms = new EnumDictionary<EPlatformIdentifier, IPlatform>();

		// Token: 0x0400A023 RID: 40995
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, IPlatform> ServerPlatforms = new ReadOnlyDictionary<EPlatformIdentifier, IPlatform>(PlatformManager.serverPlatforms);

		// Token: 0x0400A024 RID: 40996
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A025 RID: 40997
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, Type> supportedPlatforms = new EnumDictionary<EPlatformIdentifier, Type>();

		// Token: 0x0400A026 RID: 40998
		[PublicizedFrom(EAccessModifier.Private)]
		public static string supportedPlatformsString;

		// Token: 0x0400A027 RID: 40999
		public static readonly Dictionary<EPlatformIdentifier, AbsUserIdentifierFactory> UserIdentifierFactories = new EnumDictionary<EPlatformIdentifier, AbsUserIdentifierFactory>();
	}
}
