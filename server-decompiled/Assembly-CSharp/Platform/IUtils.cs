using System;
using InControl;

namespace Platform
{
	// Token: 0x02001BF6 RID: 7158
	public interface IUtils
	{
		// Token: 0x0600D51B RID: 54555
		void Init(IPlatform _owner);

		// Token: 0x0600D51C RID: 54556
		bool OpenBrowser(string _url);

		// Token: 0x0600D51D RID: 54557
		void ControllerDisconnected(InputDevice inputDevice);

		// Token: 0x0600D51E RID: 54558
		string GetPlatformLanguage();

		// Token: 0x0600D51F RID: 54559
		string GetAppLanguage();

		// Token: 0x0600D520 RID: 54560
		string GetCountry();

		// Token: 0x0600D521 RID: 54561
		string GetStoreBranchName();

		// Token: 0x0600D522 RID: 54562
		void ClearTempFiles();

		// Token: 0x0600D523 RID: 54563
		string GetTempFileName(string prefix = "", string suffix = "");

		// Token: 0x0600D524 RID: 54564
		bool? IsFamilyShare();

		// Token: 0x0600D525 RID: 54565
		string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform = EPlatformIdentifier.None);
	}
}
