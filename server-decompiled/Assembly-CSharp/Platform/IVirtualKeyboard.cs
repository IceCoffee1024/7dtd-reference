using System;

namespace Platform
{
	// Token: 0x02001BC5 RID: 7109
	public interface IVirtualKeyboard
	{
		// Token: 0x0600D3AE RID: 54190
		void Init(IPlatform _owner);

		// Token: 0x0600D3AF RID: 54191
		string Open(string _title, string _defaultText, Action<bool, string> _onTextReceived, UIInput.InputType _mode = UIInput.InputType.Standard, bool _multiLine = false, uint singleLineLength = 200U);

		// Token: 0x0600D3B0 RID: 54192
		void Destroy();
	}
}
