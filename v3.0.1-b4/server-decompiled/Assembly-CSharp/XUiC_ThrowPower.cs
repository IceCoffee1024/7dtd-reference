using System;
using UnityEngine.Scripting;

// Token: 0x0200106A RID: 4202
[Preserve]
public class XUiC_ThrowPower : XUiController
{
	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x06008556 RID: 34134 RVA: 0x0032E846 File Offset: 0x0032CA46
	// (set) Token: 0x06008557 RID: 34135 RVA: 0x0032E84E File Offset: 0x0032CA4E
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

	// Token: 0x06008558 RID: 34136 RVA: 0x002A7F33 File Offset: 0x002A6133
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x06008559 RID: 34137 RVA: 0x0032E866 File Offset: 0x0032CA66
	public override void OnClose()
	{
		base.OnClose();
		this.CurrentPower = 0f;
	}

	// Token: 0x0600855A RID: 34138 RVA: 0x0032E879 File Offset: 0x0032CA79
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

	// Token: 0x0600855B RID: 34139 RVA: 0x0032E8A0 File Offset: 0x0032CAA0
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

	// Token: 0x04006308 RID: 25352
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentPower;
}
