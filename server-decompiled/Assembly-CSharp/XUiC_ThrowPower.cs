using System;
using UnityEngine.Scripting;

// Token: 0x020010A3 RID: 4259
[Preserve]
public class XUiC_ThrowPower : XUiController
{
	// Token: 0x17000FE5 RID: 4069
	// (get) Token: 0x060086DD RID: 34525 RVA: 0x00338A2A File Offset: 0x00336C2A
	// (set) Token: 0x060086DE RID: 34526 RVA: 0x00338A32 File Offset: 0x00336C32
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

	// Token: 0x060086DF RID: 34527 RVA: 0x002AFD5B File Offset: 0x002ADF5B
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x060086E0 RID: 34528 RVA: 0x00338A4A File Offset: 0x00336C4A
	public override void OnClose()
	{
		base.OnClose();
		this.CurrentPower = 0f;
	}

	// Token: 0x060086E1 RID: 34529 RVA: 0x00338A5D File Offset: 0x00336C5D
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

	// Token: 0x060086E2 RID: 34530 RVA: 0x00338A84 File Offset: 0x00336C84
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

	// Token: 0x0400644E RID: 25678
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentPower;
}
