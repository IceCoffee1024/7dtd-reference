using System;
using InControl;

namespace Platform
{
	// Token: 0x02001B9D RID: 7069
	public interface IUtils
	{
		// Token: 0x0600D217 RID: 53783
		void Init(IPlatform _owner);

		// Token: 0x0600D218 RID: 53784
		bool OpenBrowser(string _url);

		// Token: 0x0600D219 RID: 53785
		void ControllerDisconnected(InputDevice inputDevice);

		// Token: 0x0600D21A RID: 53786
		string GetPlatformLanguage();

		// Token: 0x0600D21B RID: 53787
		string GetAppLanguage();

		// Token: 0x0600D21C RID: 53788
		string GetCountry();

		// Token: 0x0600D21D RID: 53789
		void ClearTempFiles();

		// Token: 0x0600D21E RID: 53790
		string GetTempFileName(string prefix = "", string suffix = "");

		// Token: 0x0600D21F RID: 53791
		string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform = EPlatformIdentifier.None);
	}
}
