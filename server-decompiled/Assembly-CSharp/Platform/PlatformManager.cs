using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform.MultiPlatform;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001C35 RID: 7221
	public static class PlatformManager
	{
		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x0600D67B RID: 54907 RVA: 0x004D0E2C File Offset: 0x004CF02C
		// (set) Token: 0x0600D67C RID: 54908 RVA: 0x004D0E33 File Offset: 0x004CF033
		public static EDeviceType DeviceType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x0600D67D RID: 54909 RVA: 0x004D0E3B File Offset: 0x004CF03B
		// (set) Token: 0x0600D67E RID: 54910 RVA: 0x004D0E42 File Offset: 0x004CF042
		public static IPlatform MultiPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600D67F RID: 54911 RVA: 0x004D0E4A File Offset: 0x004CF04A
		// (set) Token: 0x0600D680 RID: 54912 RVA: 0x004D0E51 File Offset: 0x004CF051
		public static IPlatform NativePlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600D681 RID: 54913 RVA: 0x004D0E59 File Offset: 0x004CF059
		// (set) Token: 0x0600D682 RID: 54914 RVA: 0x004D0E60 File Offset: 0x004CF060
		public static IPlatform CrossplatformPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x0600D683 RID: 54915 RVA: 0x004D0E68 File Offset: 0x004CF068
		// (set) Token: 0x0600D684 RID: 54916 RVA: 0x004D0E6F File Offset: 0x004CF06F
		public static ClientLobbyManager ClientLobbyManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x0600D685 RID: 54917 RVA: 0x004D0E77 File Offset: 0x004CF077
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

		// Token: 0x0600D686 RID: 54918 RVA: 0x004D0EAC File Offset: 0x004CF0AC
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

		// Token: 0x0600D687 RID: 54919 RVA: 0x004D11B4 File Offset: 0x004CF3B4
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

		// Token: 0x0600D688 RID: 54920 RVA: 0x004D1250 File Offset: 0x004CF450
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

		// Token: 0x0600D689 RID: 54921 RVA: 0x004D12E8 File Offset: 0x004CF4E8
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

		// Token: 0x0600D68A RID: 54922 RVA: 0x004D13A0 File Offset: 0x004CF5A0
		public static string PlatformStringFromEnum(EPlatformIdentifier _platformIdentifier)
		{
			return _platformIdentifier.ToStringCached<EPlatformIdentifier>();
		}

		// Token: 0x0600D68B RID: 54923 RVA: 0x004D13A8 File Offset: 0x004CF5A8
		public static bool TryPlatformIdentifierFromString(string _platformName, out EPlatformIdentifier _platformIdentifier)
		{
			return EnumUtils.TryParse<EPlatformIdentifier>(_platformName, out _platformIdentifier, true);
		}

		// Token: 0x0600D68C RID: 54924 RVA: 0x004D13B4 File Offset: 0x004CF5B4
		public static IPlatform InstanceForPlatformIdentifier(EPlatformIdentifier _platformIdentifier)
		{
			IPlatform platform;
			if (!PlatformManager.serverPlatforms.TryGetValue(_platformIdentifier, out platform))
			{
				return null;
			}
			return platform;
		}

		// Token: 0x0600D68D RID: 54925 RVA: 0x004D13D3 File Offset: 0x004CF5D3
		public static bool IsPlatformLoaded(EPlatformIdentifier _platformIdentifier)
		{
			return PlatformManager.serverPlatforms.ContainsKey(_platformIdentifier);
		}

		// Token: 0x0600D68E RID: 54926 RVA: 0x004D13E0 File Offset: 0x004CF5E0
		public static string GetPlatformDisplayName(EPlatformIdentifier _platformIdentifier)
		{
			return Localization.Get("platformName" + _platformIdentifier.ToStringCached<EPlatformIdentifier>(), false, null);
		}

		// Token: 0x0600D68F RID: 54927 RVA: 0x004D13FC File Offset: 0x004CF5FC
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

		// Token: 0x0600D690 RID: 54928 RVA: 0x004D14A4 File Offset: 0x004CF6A4
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

		// Token: 0x0600D691 RID: 54929 RVA: 0x004D1530 File Offset: 0x004CF730
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

		// Token: 0x0600D692 RID: 54930 RVA: 0x004D15A0 File Offset: 0x004CF7A0
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

		// Token: 0x0400A30B RID: 41739
		public const string PlatformConfigFileName = "platform.cfg";

		// Token: 0x0400A311 RID: 41745
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, IPlatform> serverPlatforms = new EnumDictionary<EPlatformIdentifier, IPlatform>();

		// Token: 0x0400A312 RID: 41746
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, IPlatform> ServerPlatforms = new ReadOnlyDictionary<EPlatformIdentifier, IPlatform>(PlatformManager.serverPlatforms);

		// Token: 0x0400A313 RID: 41747
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A314 RID: 41748
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, Type> supportedPlatforms = new EnumDictionary<EPlatformIdentifier, Type>();

		// Token: 0x0400A315 RID: 41749
		[PublicizedFrom(EAccessModifier.Private)]
		public static string supportedPlatformsString;

		// Token: 0x0400A316 RID: 41750
		public static readonly Dictionary<EPlatformIdentifier, AbsUserIdentifierFactory> UserIdentifierFactories = new EnumDictionary<EPlatformIdentifier, AbsUserIdentifierFactory>();
	}
}
