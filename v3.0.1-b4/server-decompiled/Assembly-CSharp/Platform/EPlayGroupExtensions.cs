using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B28 RID: 6952
	public static class EPlayGroupExtensions
	{
		// Token: 0x0600CFBD RID: 53181 RVA: 0x004BF86D File Offset: 0x004BDA6D
		public static bool IsCurrent(this EPlayGroup group)
		{
			return group == EPlayGroupExtensions.Current;
		}

		// Token: 0x0600CFBE RID: 53182 RVA: 0x004BF877 File Offset: 0x004BDA77
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

		// Token: 0x0600CFBF RID: 53183 RVA: 0x004BF890 File Offset: 0x004BDA90
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

		// Token: 0x0600CFC0 RID: 53184 RVA: 0x004BF908 File Offset: 0x004BDB08
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

		// Token: 0x0600CFC1 RID: 53185 RVA: 0x004BF96F File Offset: 0x004BDB6F
		public static uint[] GetCurrentlyAllowedPlatformIds()
		{
			if (PermissionsManager.IsCrossplayAllowed())
			{
				return null;
			}
			return EPlayGroupExtensions.s_playGroupToAllowedPlatformIds[EPlayGroupExtensions.Current];
		}

		// Token: 0x04009F1B RID: 40731
		public static readonly EPlayGroup Current = EPlayGroupExtensions.GetCurrentPlayGroup();

		// Token: 0x04009F1C RID: 40732
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
