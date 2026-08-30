using System;
using Platform.Shared;

namespace Platform.XBL
{
	// Token: 0x02001C38 RID: 7224
	public class Utils : Platform.Shared.Utils
	{
		// Token: 0x0600D64A RID: 54858 RVA: 0x004D198A File Offset: 0x004CFB8A
		public override string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform = EPlatformIdentifier.None)
		{
			switch (_playGroup)
			{
			case EPlayGroup.Standalone:
				if (_nativePlatform == EPlatformIdentifier.XBL)
				{
					return "ui_platform_xbl";
				}
				if (_fetchGenericIcons)
				{
					return "ui_platform_pc";
				}
				break;
			case EPlayGroup.XBS:
				return "ui_platform_xbl";
			case EPlayGroup.PS5:
				if (_fetchGenericIcons)
				{
					return "ui_platform_console";
				}
				break;
			}
			return string.Empty;
		}
	}
}
