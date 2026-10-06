using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform.MultiPlatform;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001C04 RID: 7172
	public static class PlatformManager
	{
		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x0600D567 RID: 54631 RVA: 0x004CD304 File Offset: 0x004CB504
		// (set) Token: 0x0600D568 RID: 54632 RVA: 0x004CD30B File Offset: 0x004CB50B
		public static EDeviceType DeviceType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x0600D569 RID: 54633 RVA: 0x004CD313 File Offset: 0x004CB513
		// (set) Token: 0x0600D56A RID: 54634 RVA: 0x004CD31A File Offset: 0x004CB51A
		public static IPlatform MultiPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600D56B RID: 54635 RVA: 0x004CD322 File Offset: 0x004CB522
		// (set) Token: 0x0600D56C RID: 54636 RVA: 0x004CD329 File Offset: 0x004CB529
		public static IPlatform NativePlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600D56D RID: 54637 RVA: 0x004CD331 File Offset: 0x004CB531
		// (set) Token: 0x0600D56E RID: 54638 RVA: 0x004CD338 File Offset: 0x004CB538
		public static IPlatform CrossplatformPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x0600D56F RID: 54639 RVA: 0x004CD340 File Offset: 0x004CB540
		// (set) Token: 0x0600D570 RID: 54640 RVA: 0x004CD347 File Offset: 0x004CB547
		public static ClientLobbyManager ClientLobbyManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x0600D571 RID: 54641 RVA: 0x004CD34F File Offset: 0x004CB54F
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

		// Token: 0x0600D572 RID: 54642 RVA: 0x004CD384 File Offset: 0x004CB584
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

		// Token: 0x0600D573 RID: 54643 RVA: 0x004CD68C File Offset: 0x004CB88C
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

		// Token: 0x0600D574 RID: 54644 RVA: 0x004CD728 File Offset: 0x004CB928
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

		// Token: 0x0600D575 RID: 54645 RVA: 0x004CD7C0 File Offset: 0x004CB9C0
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

		// Token: 0x0600D576 RID: 54646 RVA: 0x004CD878 File Offset: 0x004CBA78
		public static string PlatformStringFromEnum(EPlatformIdentifier _platformIdentifier)
		{
			return _platformIdentifier.ToStringCached<EPlatformIdentifier>();
		}

		// Token: 0x0600D577 RID: 54647 RVA: 0x004CD880 File Offset: 0x004CBA80
		public static bool TryPlatformIdentifierFromString(string _platformName, out EPlatformIdentifier _platformIdentifier)
		{
			return EnumUtils.TryParse<EPlatformIdentifier>(_platformName, out _platformIdentifier, true);
		}

		// Token: 0x0600D578 RID: 54648 RVA: 0x004CD88C File Offset: 0x004CBA8C
		public static IPlatform InstanceForPlatformIdentifier(EPlatformIdentifier _platformIdentifier)
		{
			IPlatform platform;
			if (!PlatformManager.serverPlatforms.TryGetValue(_platformIdentifier, out platform))
			{
				return null;
			}
			return platform;
		}

		// Token: 0x0600D579 RID: 54649 RVA: 0x004CD8AB File Offset: 0x004CBAAB
		public static bool IsPlatformLoaded(EPlatformIdentifier _platformIdentifier)
		{
			return PlatformManager.serverPlatforms.ContainsKey(_platformIdentifier);
		}

		// Token: 0x0600D57A RID: 54650 RVA: 0x004CD8B8 File Offset: 0x004CBAB8
		public static string GetPlatformDisplayName(EPlatformIdentifier _platformIdentifier)
		{
			return Localization.Get("platformName" + _platformIdentifier.ToStringCached<EPlatformIdentifier>(), false, null);
		}

		// Token: 0x0600D57B RID: 54651 RVA: 0x004CD8D4 File Offset: 0x004CBAD4
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

		// Token: 0x0600D57C RID: 54652 RVA: 0x004CD97C File Offset: 0x004CBB7C
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

		// Token: 0x0600D57D RID: 54653 RVA: 0x004CDA08 File Offset: 0x004CBC08
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

		// Token: 0x0600D57E RID: 54654 RVA: 0x004CDA78 File Offset: 0x004CBC78
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

		// Token: 0x0400A222 RID: 41506
		public const string PlatformConfigFileName = "platform.cfg";

		// Token: 0x0400A228 RID: 41512
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, IPlatform> serverPlatforms = new EnumDictionary<EPlatformIdentifier, IPlatform>();

		// Token: 0x0400A229 RID: 41513
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, IPlatform> ServerPlatforms = new ReadOnlyDictionary<EPlatformIdentifier, IPlatform>(PlatformManager.serverPlatforms);

		// Token: 0x0400A22A RID: 41514
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A22B RID: 41515
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, Type> supportedPlatforms = new EnumDictionary<EPlatformIdentifier, Type>();

		// Token: 0x0400A22C RID: 41516
		[PublicizedFrom(EAccessModifier.Private)]
		public static string supportedPlatformsString;

		// Token: 0x0400A22D RID: 41517
		public static readonly Dictionary<EPlatformIdentifier, AbsUserIdentifierFactory> UserIdentifierFactories = new EnumDictionary<EPlatformIdentifier, AbsUserIdentifierFactory>();
	}
}
