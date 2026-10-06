using System;

namespace Platform
{
	// Token: 0x02001BF7 RID: 7159
	public interface IVirtualKeyboard
	{
		// Token: 0x0600D526 RID: 54566
		void Init(IPlatform _owner);

		// Token: 0x0600D527 RID: 54567
		string Open(string _title, string _defaultText, Action<bool, string> _onTextReceived, UIInput.InputType _mode = UIInput.InputType.Standard, bool _multiLine = false, uint singleLineLength = 200U);

		// Token: 0x0600D528 RID: 54568
		void Destroy();
	}
}
