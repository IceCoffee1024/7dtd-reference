using System;
using GUI_2;
using UnityEngine.Scripting;

// Token: 0x02001084 RID: 4228
[Preserve]
public class XUiC_Timer : XUiController
{
	// Token: 0x0600864F RID: 34383 RVA: 0x0033311C File Offset: 0x0033131C
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.playerUI.CursorController.SetCursorHidden(true);
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.CancelButton, "igcoCancel", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
	}

	// Token: 0x06008650 RID: 34384 RVA: 0x00333184 File Offset: 0x00331384
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

	// Token: 0x06008651 RID: 34385 RVA: 0x00333215 File Offset: 0x00331415
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTimer(float _timeLeft, float _fillAmount)
	{
		this.currentTimeLeft = _timeLeft;
		this.currentFillAmount = _fillAmount;
		base.RefreshBindings();
	}

	// Token: 0x06008652 RID: 34386 RVA: 0x0033322C File Offset: 0x0033142C
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

	// Token: 0x06008653 RID: 34387 RVA: 0x003332A4 File Offset: 0x003314A4
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

	// Token: 0x06008654 RID: 34388 RVA: 0x0033331C File Offset: 0x0033151C
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

	// Token: 0x06008655 RID: 34389 RVA: 0x0033348C File Offset: 0x0033168C
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

	// Token: 0x06008656 RID: 34390 RVA: 0x00333500 File Offset: 0x00331700
	public static void OpenTimer(XUi _xui, float _fullTime, TimerEventData _eventData, float _startTime = -1f, string _labelText = "", bool _modal = true)
	{
		XUiC_Timer childByType = _xui.GetChildByType<XUiC_Timer>();
		_xui.playerUI.windowManager.Open(childByType.windowGroup, _modal);
		childByType.setTimer(_fullTime, _eventData, _startTime, _labelText);
	}

	// Token: 0x06008657 RID: 34391 RVA: 0x00333538 File Offset: 0x00331738
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

	// Token: 0x040063D0 RID: 25552
	[PublicizedFrom(EAccessModifier.Private)]
	public float fullTime;

	// Token: 0x040063D1 RID: 25553
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeLeft;

	// Token: 0x040063D2 RID: 25554
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentOpenEventText;

	// Token: 0x040063D3 RID: 25555
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentFillAmount;

	// Token: 0x040063D4 RID: 25556
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTimeLeft;

	// Token: 0x040063D5 RID: 25557
	[PublicizedFrom(EAccessModifier.Private)]
	public TimerEventData eventData;

	// Token: 0x040063D6 RID: 25558
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skipCloseEvent;
}
