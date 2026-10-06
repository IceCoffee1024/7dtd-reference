using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B81 RID: 7041
	public static class EPlayGroupExtensions
	{
		// Token: 0x0600D2BD RID: 53949 RVA: 0x004CA749 File Offset: 0x004C8949
		public static bool IsCurrent(this EPlayGroup group)
		{
			return group == EPlayGroupExtensions.Current;
		}

		// Token: 0x0600D2BE RID: 53950 RVA: 0x004CA753 File Offset: 0x004C8953
		[PublicizedFrom(EAccessModifier.Private)]
		public static EPlayGroup GetCurrentPlayGroup()
		{
			if (DeviceFlag.PS5.IsCurrent())
			{
				return EPlayGroup.PS5;
			}
			if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent())
			{
				return EPlayGroup.XBS;
			}
			return EPlayGroup.Standalone;
		}

		// Token: 0x0600D2BF RID: 53951 RVA: 0x004CA76C File Offset: 0x004C896C
		public static EPlayGroup ToPlayGroup(this DeviceFlag device)
		{
			if (device <= DeviceFlag.XBoxSeriesS)
			{
				switch (device)
				{
				case DeviceFlag.StandaloneWindows:
					return EPlayGroup.Standalone;
				case DeviceFlag.StandaloneLinux:
					return EPlayGroup.Standalone;
				case DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux:
					break;
				case DeviceFlag.StandaloneOSX:
					return EPlayGroup.Standalone;
				default:
					if (device == DeviceFlag.XBoxSeriesS)
					{
						return EPlayGroup.XBS;
					}
					break;
				}
			}
			else
			{
				if (device == DeviceFlag.XBoxSeriesX)
				{
					return EPlayGroup.XBS;
				}
				if (device == DeviceFlag.PS5)
				{
					return EPlayGroup.PS5;
				}
			}
			throw new ArgumentOutOfRangeException("device", device, string.Format("Missing play group mapping for {0}.", device));
		}

		// Token: 0x0600D2C0 RID: 53952 RVA: 0x004CA7E4 File Offset: 0x004C89E4
		public static EPlayGroup ToPlayGroup(this ClientInfo.EDeviceType deviceType)
		{
			EPlayGroup eplayGroup;
			switch (deviceType)
			{
			case ClientInfo.EDeviceType.Linux:
				eplayGroup = EPlayGroup.Standalone;
				break;
			case ClientInfo.EDeviceType.Mac:
				eplayGroup = EPlayGroup.Standalone;
				break;
			case ClientInfo.EDeviceType.Windows:
				eplayGroup = EPlayGroup.Standalone;
				break;
			case ClientInfo.EDeviceType.PlayStation:
				eplayGroup = EPlayGroup.PS5;
				break;
			case ClientInfo.EDeviceType.Xbox:
				eplayGroup = EPlayGroup.XBS;
				break;
			case ClientInfo.EDeviceType.Unknown:
				eplayGroup = EPlayGroup.Standalone;
				break;
			default:
				throw new ArgumentOutOfRangeException("deviceType", deviceType, string.Format("Missing play group mapping for {0}.", deviceType));
			}
			return eplayGroup;
		}

		// Token: 0x0600D2C1 RID: 53953 RVA: 0x004CA84B File Offset: 0x004C8A4B
		public static uint[] GetCurrentlyAllowedPlatformIds()
		{
			if (PermissionsManager.IsCrossplayAllowed())
			{
				return null;
			}
			return EPlayGroupExtensions.s_playGroupToAllowedPlatformIds[EPlayGroupExtensions.Current];
		}

		// Token: 0x0400A121 RID: 41249
		public static readonly EPlayGroup Current = EPlayGroupExtensions.GetCurrentPlayGroup();

		// Token: 0x0400A122 RID: 41250
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<EPlayGroup, uint[]> s_playGroupToAllowedPlatformIds = new Dictionary<EPlayGroup, uint[]>
		{
			{
				EPlayGroup.Standalone,
				null
			},
			{
				EPlayGroup.XBS,
				null
			},
			{
				EPlayGroup.PS5,
				null
			}
		};
	}
}
