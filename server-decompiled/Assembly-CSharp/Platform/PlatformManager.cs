using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform.MultiPlatform;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BD2 RID: 7122
	public static class PlatformManager
	{
		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x0600D3EF RID: 54255 RVA: 0x004C881C File Offset: 0x004C6A1C
		// (set) Token: 0x0600D3F0 RID: 54256 RVA: 0x004C8823 File Offset: 0x004C6A23
		public static EDeviceType DeviceType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x0600D3F1 RID: 54257 RVA: 0x004C882B File Offset: 0x004C6A2B
		// (set) Token: 0x0600D3F2 RID: 54258 RVA: 0x004C8832 File Offset: 0x004C6A32
		public static IPlatform MultiPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x0600D3F3 RID: 54259 RVA: 0x004C883A File Offset: 0x004C6A3A
		// (set) Token: 0x0600D3F4 RID: 54260 RVA: 0x004C8841 File Offset: 0x004C6A41
		public static IPlatform NativePlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x0600D3F5 RID: 54261 RVA: 0x004C8849 File Offset: 0x004C6A49
		// (set) Token: 0x0600D3F6 RID: 54262 RVA: 0x004C8850 File Offset: 0x004C6A50
		public static IPlatform CrossplatformPlatform
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x0600D3F7 RID: 54263 RVA: 0x004C8858 File Offset: 0x004C6A58
		// (set) Token: 0x0600D3F8 RID: 54264 RVA: 0x004C885F File Offset: 0x004C6A5F
		public static ClientLobbyManager ClientLobbyManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x0600D3F9 RID: 54265 RVA: 0x004C8867 File Offset: 0x004C6A67
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

		// Token: 0x0600D3FA RID: 54266 RVA: 0x004C889C File Offset: 0x004C6A9C
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

		// Token: 0x0600D3FB RID: 54267 RVA: 0x004C8BA4 File Offset: 0x004C6DA4
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

		// Token: 0x0600D3FC RID: 54268 RVA: 0x004C8C40 File Offset: 0x004C6E40
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

		// Token: 0x0600D3FD RID: 54269 RVA: 0x004C8CD8 File Offset: 0x004C6ED8
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

		// Token: 0x0600D3FE RID: 54270 RVA: 0x004C8D90 File Offset: 0x004C6F90
		public static string PlatformStringFromEnum(EPlatformIdentifier _platformIdentifier)
		{
			return _platformIdentifier.ToStringCached<EPlatformIdentifier>();
		}

		// Token: 0x0600D3FF RID: 54271 RVA: 0x004C8D98 File Offset: 0x004C6F98
		public static bool TryPlatformIdentifierFromString(string _platformName, out EPlatformIdentifier _platformIdentifier)
		{
			return EnumUtils.TryParse<EPlatformIdentifier>(_platformName, out _platformIdentifier, true);
		}

		// Token: 0x0600D400 RID: 54272 RVA: 0x004C8DA4 File Offset: 0x004C6FA4
		public static IPlatform InstanceForPlatformIdentifier(EPlatformIdentifier _platformIdentifier)
		{
			IPlatform platform;
			if (!PlatformManager.serverPlatforms.TryGetValue(_platformIdentifier, out platform))
			{
				return null;
			}
			return platform;
		}

		// Token: 0x0600D401 RID: 54273 RVA: 0x004C8DC3 File Offset: 0x004C6FC3
		public static bool IsPlatformLoaded(EPlatformIdentifier _platformIdentifier)
		{
			return PlatformManager.serverPlatforms.ContainsKey(_platformIdentifier);
		}

		// Token: 0x0600D402 RID: 54274 RVA: 0x004C8DD0 File Offset: 0x004C6FD0
		public static string GetPlatformDisplayName(EPlatformIdentifier _platformIdentifier)
		{
			return Localization.Get("platformName" + _platformIdentifier.ToStringCached<EPlatformIdentifier>(), false, null);
		}

		// Token: 0x0600D403 RID: 54275 RVA: 0x004C8DEC File Offset: 0x004C6FEC
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

		// Token: 0x0600D404 RID: 54276 RVA: 0x004C8E94 File Offset: 0x004C7094
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

		// Token: 0x0600D405 RID: 54277 RVA: 0x004C8F20 File Offset: 0x004C7120
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

		// Token: 0x0600D406 RID: 54278 RVA: 0x004C8F90 File Offset: 0x004C7190
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

		// Token: 0x0400A14D RID: 41293
		public const string PlatformConfigFileName = "platform.cfg";

		// Token: 0x0400A153 RID: 41299
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, IPlatform> serverPlatforms = new EnumDictionary<EPlatformIdentifier, IPlatform>();

		// Token: 0x0400A154 RID: 41300
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, IPlatform> ServerPlatforms = new ReadOnlyDictionary<EPlatformIdentifier, IPlatform>(PlatformManager.serverPlatforms);

		// Token: 0x0400A155 RID: 41301
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool initialized;

		// Token: 0x0400A156 RID: 41302
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlatformIdentifier, Type> supportedPlatforms = new EnumDictionary<EPlatformIdentifier, Type>();

		// Token: 0x0400A157 RID: 41303
		[PublicizedFrom(EAccessModifier.Private)]
		public static string supportedPlatformsString;

		// Token: 0x0400A158 RID: 41304
		public static readonly Dictionary<EPlatformIdentifier, AbsUserIdentifierFactory> UserIdentifierFactories = new EnumDictionary<EPlatformIdentifier, AbsUserIdentifierFactory>();
	}
}
