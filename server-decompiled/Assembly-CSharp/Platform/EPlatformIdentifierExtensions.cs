using System;

namespace Platform
{
	// Token: 0x02001B26 RID: 6950
	public static class EPlatformIdentifierExtensions
	{
		// Token: 0x0600CFB9 RID: 53177 RVA: 0x004BF6C4 File Offset: 0x004BD8C4
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

		// Token: 0x0600CFBA RID: 53178 RVA: 0x004BF734 File Offset: 0x004BD934
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

		// Token: 0x0600CFBB RID: 53179 RVA: 0x004BF7A4 File Offset: 0x004BD9A4
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

		// Token: 0x0600CFBC RID: 53180 RVA: 0x004BF814 File Offset: 0x004BDA14
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
