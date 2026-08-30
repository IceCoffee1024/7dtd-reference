using System;
using UnityEngine.Scripting;

// Token: 0x02001083 RID: 4227
[Preserve]
public class XUiC_ThrowPower : XUiController
{
	// Token: 0x17000FA5 RID: 4005
	// (get) Token: 0x06008648 RID: 34376 RVA: 0x0033306E File Offset: 0x0033126E
	// (set) Token: 0x06008649 RID: 34377 RVA: 0x00333076 File Offset: 0x00331276
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

	// Token: 0x0600864A RID: 34378 RVA: 0x002AB447 File Offset: 0x002A9647
	public override void OnOpen()
	{
		base.OnOpen();
		((XUiV_Window)this.viewComponent).ForceVisible(1f);
	}

	// Token: 0x0600864B RID: 34379 RVA: 0x0033308E File Offset: 0x0033128E
	public override void OnClose()
	{
		base.OnClose();
		this.CurrentPower = 0f;
	}

	// Token: 0x0600864C RID: 34380 RVA: 0x003330A1 File Offset: 0x003312A1
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

	// Token: 0x0600864D RID: 34381 RVA: 0x003330C8 File Offset: 0x003312C8
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

	// Token: 0x040063CF RID: 25551
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentPower;
}
