using System;
using UnityEngine.Scripting;

// Token: 0x02001093 RID: 4243
[Preserve]
public class XUiC_ThrowPower : XUiController
{
	// Token: 0x17000FDA RID: 4058
	// (get) Token: 0x0600868A RID: 34442 RVA: 0x003369A6 File Offset: 0x00334BA6
	// (set) Token: 0x0600868B RID: 34443 RVA: 0x003369AE File Offset: 0x00334BAE
	public float CurrentPower
	{
		get
		{
			return this.currentPower;
		}
		set
		{
			if (value != this.currentPower)
			{
				this.currentPower = value;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x0600868C RID: 34444 RVA: 0x002AE7EF File Offset: 0x002AC9EF
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x0600868D RID: 34445 RVA: 0x003369C6 File Offset: 0x00334BC6
	public override void OnClose()
	{
		base.OnClose();
		this.CurrentPower = 0f;
	}

	// Token: 0x0600868E RID: 34446 RVA: 0x003369D9 File Offset: 0x00334BD9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "fill")
		{
			_value = this.currentPower.ToCultureInvariantString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x0600868F RID: 34447 RVA: 0x00336A00 File Offset: 0x00334C00
	public static void Status(LocalPlayerUI _playerUi, float _currentPower = -1f)
	{
		XUiC_ThrowPower windowByType = _playerUi.xui.GetWindowByType<XUiC_ThrowPower>();
		if (windowByType != null)
		{
			windowByType.CurrentPower = _currentPower;
			if (_currentPower >= 0f)
			{
				_playerUi.windowManager.Open(windowByType.windowGroup, false, true);
				return;
			}
			_playerUi.windowManager.Close(windowByType.windowGroup, false);
		}
	}

	// Token: 0x0400640F RID: 25615
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentPower;
}
