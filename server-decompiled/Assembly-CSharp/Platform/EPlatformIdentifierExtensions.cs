using System;

namespace Platform
{
	// Token: 0x02001BB0 RID: 7088
	public static class EPlatformIdentifierExtensions
	{
		// Token: 0x0600D3CD RID: 54221 RVA: 0x004CE0C8 File Offset: 0x004CC2C8
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

		// Token: 0x0600D3CE RID: 54222 RVA: 0x004CE138 File Offset: 0x004CC338
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

		// Token: 0x0600D3CF RID: 54223 RVA: 0x004CE1A8 File Offset: 0x004CC3A8
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

		// Token: 0x0600D3D0 RID: 54224 RVA: 0x004CE218 File Offset: 0x004CC418
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
