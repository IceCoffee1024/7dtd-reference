using System;

namespace Platform
{
	// Token: 0x02001C28 RID: 7208
	public interface IVirtualKeyboard
	{
		// Token: 0x0600D63A RID: 54842
		void Init(IPlatform _owner);

		// Token: 0x0600D63B RID: 54843
		string Open(string _title, string _defaultText, Action<bool, string> _onTextReceived, UIInput.InputType _mode = UIInput.InputType.Standard, bool _multiLine = false, uint singleLineLength = 200U);

		// Token: 0x0600D63C RID: 54844
		void Destroy();
	}
}
