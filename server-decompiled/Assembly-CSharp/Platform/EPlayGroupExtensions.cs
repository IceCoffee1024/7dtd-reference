using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B4F RID: 6991
	public static class EPlayGroupExtensions
	{
		// Token: 0x0600D146 RID: 53574 RVA: 0x004C5C61 File Offset: 0x004C3E61
		public static bool IsCurrent(this EPlayGroup group)
		{
			return group == EPlayGroupExtensions.Current;
		}

		// Token: 0x0600D147 RID: 53575 RVA: 0x004C5C6B File Offset: 0x004C3E6B
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

		// Token: 0x0600D148 RID: 53576 RVA: 0x004C5C84 File Offset: 0x004C3E84
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

		// Token: 0x0600D149 RID: 53577 RVA: 0x004C5CFC File Offset: 0x004C3EFC
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

		// Token: 0x0600D14A RID: 53578 RVA: 0x004C5D63 File Offset: 0x004C3F63
		public static uint[] GetCurrentlyAllowedPlatformIds()
		{
			if (PermissionsManager.IsCrossplayAllowed())
			{
				return null;
			}
			return EPlayGroupExtensions.s_playGroupToAllowedPlatformIds[EPlayGroupExtensions.Current];
		}

		// Token: 0x0400A04C RID: 41036
		public static readonly EPlayGroup Current = EPlayGroupExtensions.GetCurrentPlayGroup();

		// Token: 0x0400A04D RID: 41037
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
