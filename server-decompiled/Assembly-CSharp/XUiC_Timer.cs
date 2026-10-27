using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x020010A4 RID: 4260
[Preserve]
public class XUiC_Timer : XUiController
{
	// Token: 0x060086E4 RID: 34532 RVA: 0x00338AD8 File Offset: 0x00336CD8
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(true);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.CancelButton, "igcoCancel", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
	}

	// Token: 0x060086E5 RID: 34533 RVA: 0x00338B40 File Offset: 0x00336D40
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

	// Token: 0x060086E6 RID: 34534 RVA: 0x00338BD1 File Offset: 0x00336DD1
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTimer(float _timeLeft, float _fillAmount)
	{
		this.currentTimeLeft = _timeLeft;
		this.currentFillAmount = _fillAmount;
		base.RefreshBindings();
	}

	// Token: 0x060086E7 RID: 34535 RVA: 0x00338BE8 File Offset: 0x00336DE8
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

	// Token: 0x060086E8 RID: 34536 RVA: 0x00338C60 File Offset: 0x00336E60
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

	// Token: 0x060086E9 RID: 34537 RVA: 0x00338CD8 File Offset: 0x00336ED8
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

	// Token: 0x060086EA RID: 34538 RVA: 0x00338E48 File Offset: 0x00337048
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

	// Token: 0x060086EB RID: 34539 RVA: 0x00338EBC File Offset: 0x003370BC
	public static void OpenTimer(XUi _xui, float _fullTime, TimerEventData _eventData, float _startTime = -1f, string _labelText = "", bool _modal = true)
	{
		XUiC_Timer childByType = _xui.GetChildByType<XUiC_Timer>();
		_xui.playerUI.windowManager.Open(childByType.windowGroup, _modal);
		childByType.setTimer(_fullTime, _eventData, _startTime, _labelText);
	}

	// Token: 0x060086EC RID: 34540 RVA: 0x00338EF4 File Offset: 0x003370F4
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

	// Token: 0x0400644F RID: 25679
	[PublicizedFrom(EAccessModifier.Private)]
	public float fullTime;

	// Token: 0x04006450 RID: 25680
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeLeft;

	// Token: 0x04006451 RID: 25681
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentOpenEventText;

	// Token: 0x04006452 RID: 25682
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentFillAmount;

	// Token: 0x04006453 RID: 25683
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTimeLeft;

	// Token: 0x04006454 RID: 25684
	[PublicizedFrom(EAccessModifier.Private)]
	public TimerEventData eventData;

	// Token: 0x04006455 RID: 25685
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipCloseEvent;
}
