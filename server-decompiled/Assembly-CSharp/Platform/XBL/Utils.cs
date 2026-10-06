using System;
using Platform.Shared;

namespace Platform.XBL
{
	// Token: 0x02001C6A RID: 7274
	public class Utils : Platform.Shared.Utils
	{
		// Token: 0x0600D7C3 RID: 55235 RVA: 0x004D64C6 File Offset: 0x004D46C6
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
