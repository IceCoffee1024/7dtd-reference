using System;

namespace Platform
{
	// Token: 0x02001B4D RID: 6989
	public static class EPlatformIdentifierExtensions
	{
		// Token: 0x0600D142 RID: 53570 RVA: 0x004C5AB8 File Offset: 0x004C3CB8
		public static bool IsNative(this EPlatformIdentifier platformIdentifier)
		{
			bool flag;
			switch (platformIdentifier)
			{
			case EPlatformIdentifier.None:
				flag = false;
				break;
			case EPlatformIdentifier.Local:
				flag = true;
				break;
			case EPlatformIdentifier.EOS:
				flag = false;
				break;
			case EPlatformIdentifier.Steam:
				flag = true;
				break;
			case EPlatformIdentifier.XBL:
				flag = true;
				break;
			case EPlatformIdentifier.PSN:
				flag = true;
				break;
			case EPlatformIdentifier.EGS:
				flag = true;
				break;
			case EPlatformIdentifier.LAN:
				flag = false;
				break;
			case EPlatformIdentifier.Count:
				flag = false;
				break;
			default:
				throw new ArgumentOutOfRangeException("platformIdentifier", platformIdentifier, null);
			}
			return flag;
		}

		// Token: 0x0600D143 RID: 53571 RVA: 0x004C5B28 File Offset: 0x004C3D28
		public static bool IsCross(this EPlatformIdentifier platformIdentifier)
		{
			bool flag;
			switch (platformIdentifier)
			{
			case EPlatformIdentifier.None:
				flag = true;
				break;
			case EPlatformIdentifier.Local:
				flag = false;
				break;
			case EPlatformIdentifier.EOS:
				flag = true;
				break;
			case EPlatformIdentifier.Steam:
				flag = false;
				break;
			case EPlatformIdentifier.XBL:
				flag = false;
				break;
			case EPlatformIdentifier.PSN:
				flag = false;
				break;
			case EPlatformIdentifier.EGS:
				flag = false;
				break;
			case EPlatformIdentifier.LAN:
				flag = false;
				break;
			case EPlatformIdentifier.Count:
				flag = false;
				break;
			default:
				throw new ArgumentOutOfRangeException("platformIdentifier", platformIdentifier, null);
			}
			return flag;
		}

		// Token: 0x0600D144 RID: 53572 RVA: 0x004C5B98 File Offset: 0x004C3D98
		public static bool IsServer(this EPlatformIdentifier platformIdentifier)
		{
			bool flag;
			switch (platformIdentifier)
			{
			case EPlatformIdentifier.None:
				flag = false;
				break;
			case EPlatformIdentifier.Local:
				flag = false;
				break;
			case EPlatformIdentifier.EOS:
				flag = false;
				break;
			case EPlatformIdentifier.Steam:
				flag = true;
				break;
			case EPlatformIdentifier.XBL:
				flag = true;
				break;
			case EPlatformIdentifier.PSN:
				flag = true;
				break;
			case EPlatformIdentifier.EGS:
				flag = true;
				break;
			case EPlatformIdentifier.LAN:
				flag = true;
				break;
			case EPlatformIdentifier.Count:
				flag = false;
				break;
			default:
				throw new ArgumentOutOfRangeException("platformIdentifier", platformIdentifier, null);
			}
			return flag;
		}

		// Token: 0x0600D145 RID: 53573 RVA: 0x004C5C08 File Offset: 0x004C3E08
		public static bool IsServerValid(this EPlatformIdentifier serverPlatform, EPlatformIdentifier nativePlatform, EPlatformIdentifier crossPlatform)
		{
			bool flag = serverPlatform.IsServer();
			if (flag)
			{
				bool flag2;
				switch (serverPlatform)
				{
				case EPlatformIdentifier.Steam:
					flag2 = true;
					break;
				case EPlatformIdentifier.XBL:
				case EPlatformIdentifier.PSN:
				case EPlatformIdentifier.EGS:
					flag2 = crossPlatform == EPlatformIdentifier.EOS;
					break;
				case EPlatformIdentifier.LAN:
					flag2 = true;
					break;
				default:
					throw new ArgumentOutOfRangeException("serverPlatform", serverPlatform, null);
				}
				flag = flag2;
			}
			return flag;
		}
	}
}
