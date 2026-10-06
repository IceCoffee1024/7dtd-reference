using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02001094 RID: 4244
[Preserve]
public class XUiC_Timer : XUiController
{
	// Token: 0x06008691 RID: 34449 RVA: 0x00336A54 File Offset: 0x00334C54
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(true);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.CancelButton, "igcoCancel", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
	}

	// Token: 0x06008692 RID: 34450 RVA: 0x00336ABC File Offset: 0x00334CBC
	public override void OnClose()
	{
		base.OnClose();
		this.updateTimer(0f, 1f);
		this.fullTime = 0f;
		this.xui.playerUI.CursorController.SetCursorHidden(false);
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		if (!this.skipCloseEvent)
		{
			TimerEventData timerEventData = this.eventData;
			if (timerEventData != null)
			{
				timerEventData.HandleClosed();
			}
		}
		this.eventData = null;
		this.xui.playerUI.entityPlayer.SetControllable(true);
		CursorControllerAbs.SetCursor(CursorControllerAbs.ECursorType.Default);
	}

	// Token: 0x06008693 RID: 34451 RVA: 0x00336B4D File Offset: 0x00334D4D
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTimer(float _timeLeft, float _fillAmount)
	{
		this.currentTimeLeft = _timeLeft;
		this.currentFillAmount = _fillAmount;
		base.RefreshBindings();
	}

	// Token: 0x06008694 RID: 34452 RVA: 0x00336B64 File Offset: 0x00334D64
	[PublicizedFrom(EAccessModifier.Private)]
	public void timeReachedNull()
	{
		this.fullTime = 0f;
		this.timeLeft = 0f;
		this.skipCloseEvent = true;
		TimerEventData timerEventData = this.eventData;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.DragAndDropWindow.InMenu = false;
		if (timerEventData != null)
		{
			timerEventData.HandleFullTimeFinished();
		}
		this.skipCloseEvent = false;
		this.eventData = null;
	}

	// Token: 0x06008695 RID: 34453 RVA: 0x00336BDC File Offset: 0x00334DDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void alternateTimeReached()
	{
		this.fullTime = 0f;
		this.timeLeft = 0f;
		this.skipCloseEvent = true;
		TimerEventData timerEventData = this.eventData;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.DragAndDropWindow.InMenu = false;
		if (timerEventData != null)
		{
			timerEventData.HandleAlternateEvent();
		}
		this.skipCloseEvent = false;
		this.eventData = null;
	}

	// Token: 0x06008696 RID: 34454 RVA: 0x00336C54 File Offset: 0x00334E54
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.fullTime <= 0f)
		{
			return;
		}
		if (this.eventData.CloseOnHit && this.xui.playerUI.entityPlayer != null && this.xui.playerUI.entityPlayer.hasBeenAttackedTime > 0)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			return;
		}
		this.timeLeft -= _dt;
		if (this.timeLeft < 0f)
		{
			this.timeLeft = 0f;
		}
		float num = this.timeLeft / this.fullTime;
		this.updateTimer(this.timeLeft, num);
		this.eventData.Completion = (this.fullTime - this.timeLeft) / this.fullTime;
		if (this.timeLeft <= 0f)
		{
			this.timeReachedNull();
			return;
		}
		if (this.eventData.AlternateTime >= 0f && this.fullTime - this.timeLeft > this.eventData.AlternateTime)
		{
			this.alternateTimeReached();
			return;
		}
		if (this.eventData.CancelWithActivateButton && this.xui.playerUI.playerInput.PermanentActions.Activate.WasPressed)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x06008697 RID: 34455 RVA: 0x00336DC4 File Offset: 0x00334FC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setTimer(float _fullTime, TimerEventData _eventData, float _startTime = -1f, string _labelText = "")
	{
		this.currentOpenEventText = _labelText;
		this.fullTime = _fullTime;
		this.timeLeft = ((_startTime < 0f) ? this.fullTime : _startTime);
		float num = 1f;
		if (this.fullTime > _startTime && this.fullTime > 0f)
		{
			num = this.timeLeft / this.fullTime;
		}
		this.eventData = _eventData;
		this.updateTimer(this.timeLeft, num);
	}

	// Token: 0x06008698 RID: 34456 RVA: 0x00336E38 File Offset: 0x00335038
	public static void OpenTimer(XUi _xui, float _fullTime, TimerEventData _eventData, float _startTime = -1f, string _labelText = "", bool _modal = true)
	{
		XUiC_Timer childByType = _xui.GetChildByType<XUiC_Timer>();
		_xui.playerUI.windowManager.Open(childByType.windowGroup, _modal);
		childByType.setTimer(_fullTime, _eventData, _startTime, _labelText);
	}

	// Token: 0x06008699 RID: 34457 RVA: 0x00336E70 File Offset: 0x00335070
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "timeleft")
		{
			_value = this.currentTimeLeft.ToCultureInvariantString("0.0");
			return true;
		}
		if (_bindingName == "percent")
		{
			_value = this.currentFillAmount.ToCultureInvariantString();
			return true;
		}
		if (!(_bindingName == "caption"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.currentOpenEventText ?? "";
		return true;
	}

	// Token: 0x04006410 RID: 25616
	[PublicizedFrom(EAccessModifier.Private)]
	public float fullTime;

	// Token: 0x04006411 RID: 25617
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeLeft;

	// Token: 0x04006412 RID: 25618
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentOpenEventText;

	// Token: 0x04006413 RID: 25619
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentFillAmount;

	// Token: 0x04006414 RID: 25620
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTimeLeft;

	// Token: 0x04006415 RID: 25621
	[PublicizedFrom(EAccessModifier.Private)]
	public TimerEventData eventData;

	// Token: 0x04006416 RID: 25622
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipCloseEvent;
}
