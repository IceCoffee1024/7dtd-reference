using System;
using InControl;

namespace Platform
{
	// Token: 0x02001BC4 RID: 7108
	public interface IUtils
	{
		// Token: 0x0600D3A3 RID: 54179
		void Init(IPlatform _owner);

		// Token: 0x0600D3A4 RID: 54180
		bool OpenBrowser(string _url);

		// Token: 0x0600D3A5 RID: 54181
		void ControllerDisconnected(InputDevice inputDevice);

		// Token: 0x0600D3A6 RID: 54182
		string GetPlatformLanguage();

		// Token: 0x0600D3A7 RID: 54183
		string GetAppLanguage();

		// Token: 0x0600D3A8 RID: 54184
		string GetCountry();

		// Token: 0x0600D3A9 RID: 54185
		string GetStoreBranchName();

		// Token: 0x0600D3AA RID: 54186
		void ClearTempFiles();

		// Token: 0x0600D3AB RID: 54187
		string GetTempFileName(string prefix = "", string suffix = "");

		// Token: 0x0600D3AC RID: 54188
		bool? IsFamilyShare();

		// Token: 0x0600D3AD RID: 54189
		string GetCrossplayPlayerIcon(EPlayGroup _playGroup, bool _fetchGenericIcons, EPlatformIdentifier _nativePlatform = EPlatformIdentifier.None);
	}
}
