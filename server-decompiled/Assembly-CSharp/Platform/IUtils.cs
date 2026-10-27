using System;
using InControl;

namespace Platform
{
	// Token: 0x02001C27 RID: 7207
	public interface IUtils
	{
		// Token: 0x0600D62F RID: 54831
		void Init(IPlatform _owner);

		// Token: 0x0600D630 RID: 54832
		bool OpenBrowser(string _url);

		// Token: 0x0600D631 RID: 54833
		void ControllerDisconnected(InputDevice inputDevice);

		// Token: 0x0600D632 RID: 54834
		string GetPlatformLanguage();

		// Token: 0x0600D633 RID: 54835
		string GetAppLanguage();

		// Token: 0x0600D634 RID: 54836
		string GetCountry();

		// Token: 0x0600D635 RID: 54837
		string GetStoreBranchName();

		// Token: 0x0600D636 RID: 54838
		void ClearTempFiles();

		// Token: 0x0600D637 RID: 54839
		string GetTempFileName(string prefix = "", string suffix = "");

		// Token: 0x0600D638 RID: 54840
		bool? IsFamilyShare();

		// Token: 0x0600D639 RID: 54841
		string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform = EPlatformIdentifier.None);
	}
}
