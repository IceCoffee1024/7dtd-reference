using System;

namespace Platform
{
	// Token: 0x02001B9E RID: 7070
	public interface IVirtualKeyboard
	{
		// Token: 0x0600D220 RID: 53792
		void Init(IPlatform _owner);

		// Token: 0x0600D221 RID: 53793
		string Open(string _title, string _defaultText, Action<bool, string> _onTextReceived, UIInput.InputType _mode = UIInput.InputType.Standard, bool _multiLine = false, uint singleLineLength = 200U);

		// Token: 0x0600D222 RID: 53794
		void Destroy();
	}
}
