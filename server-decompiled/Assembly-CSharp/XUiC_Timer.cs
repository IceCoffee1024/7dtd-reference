using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x0200106B RID: 4203
[Preserve]
public class XUiC_Timer : XUiController
{
	// Token: 0x0600855D RID: 34141 RVA: 0x0032E8F4 File Offset: 0x0032CAF4
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(true);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.CancelButton, "igcoCancel", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
	}

	// Token: 0x0600855E RID: 34142 RVA: 0x0032E95C File Offset: 0x0032CB5C
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

	// Token: 0x0600855F RID: 34143 RVA: 0x0032E9ED File Offset: 0x0032CBED
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTimer(float _timeLeft, float _fillAmount)
	{
		this.currentTimeLeft = _timeLeft;
		this.currentFillAmount = _fillAmount;
		base.RefreshBindings();
	}

	// Token: 0x06008560 RID: 34144 RVA: 0x0032EA04 File Offset: 0x0032CC04
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

	// Token: 0x06008561 RID: 34145 RVA: 0x0032EA7C File Offset: 0x0032CC7C
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

	// Token: 0x06008562 RID: 34146 RVA: 0x0032EAF4 File Offset: 0x0032CCF4
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

	// Token: 0x06008563 RID: 34147 RVA: 0x0032EC64 File Offset: 0x0032CE64
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

	// Token: 0x06008564 RID: 34148 RVA: 0x0032ECD8 File Offset: 0x0032CED8
	public static void OpenTimer(XUi _xui, float _fullTime, TimerEventData _eventData, float _startTime = -1f, string _labelText = "", bool _modal = true)
	{
		XUiC_Timer childByType = _xui.GetChildByType<XUiC_Timer>();
		_xui.playerUI.windowManager.Open(childByType.windowGroup, _modal);
		childByType.setTimer(_fullTime, _eventData, _startTime, _labelText);
	}

	// Token: 0x06008565 RID: 34149 RVA: 0x0032ED10 File Offset: 0x0032CF10
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

	// Token: 0x04006309 RID: 25353
	[PublicizedFrom(EAccessModifier.Private)]
	public float fullTime;

	// Token: 0x0400630A RID: 25354
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeLeft;

	// Token: 0x0400630B RID: 25355
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentOpenEventText;

	// Token: 0x0400630C RID: 25356
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentFillAmount;

	// Token: 0x0400630D RID: 25357
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTimeLeft;

	// Token: 0x0400630E RID: 25358
	[PublicizedFrom(EAccessModifier.Private)]
	public TimerEventData eventData;

	// Token: 0x0400630F RID: 25359
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipCloseEvent;
}
