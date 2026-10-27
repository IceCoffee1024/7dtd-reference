using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BB2 RID: 7090
	public static class EPlayGroupExtensions
	{
		// Token: 0x0600D3D1 RID: 54225 RVA: 0x004CE271 File Offset: 0x004CC471
		public static bool IsCurrent(this EPlayGroup group)
		{
			return group == EPlayGroupExtensions.Current;
		}

		// Token: 0x0600D3D2 RID: 54226 RVA: 0x004CE27B File Offset: 0x004CC47B
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

		// Token: 0x0600D3D3 RID: 54227 RVA: 0x004CE294 File Offset: 0x004CC494
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

		// Token: 0x0600D3D4 RID: 54228 RVA: 0x004CE30C File Offset: 0x004CC50C
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

		// Token: 0x0600D3D5 RID: 54229 RVA: 0x004CE373 File Offset: 0x004CC573
		public static uint[] GetCurrentlyAllowedPlatformIds()
		{
			if (PermissionsManager.IsCrossplayAllowed())
			{
				return null;
			}
			return EPlayGroupExtensions.s_playGroupToAllowedPlatformIds[EPlayGroupExtensions.Current];
		}

		// Token: 0x0400A20A RID: 41482
		public static readonly EPlayGroup Current = EPlayGroupExtensions.GetCurrentPlayGroup();

		// Token: 0x0400A20B RID: 41483
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
