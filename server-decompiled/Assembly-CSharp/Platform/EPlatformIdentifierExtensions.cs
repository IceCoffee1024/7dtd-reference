using System;

namespace Platform
{
	// Token: 0x02001B7F RID: 7039
	public static class EPlatformIdentifierExtensions
	{
		// Token: 0x0600D2B9 RID: 53945 RVA: 0x004CA5A0 File Offset: 0x004C87A0
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

		// Token: 0x0600D2BA RID: 53946 RVA: 0x004CA610 File Offset: 0x004C8810
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

		// Token: 0x0600D2BB RID: 53947 RVA: 0x004CA680 File Offset: 0x004C8880
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

		// Token: 0x0600D2BC RID: 53948 RVA: 0x004CA6F0 File Offset: 0x004C88F0
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
