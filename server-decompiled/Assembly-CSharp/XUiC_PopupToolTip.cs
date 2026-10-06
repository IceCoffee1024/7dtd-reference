using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC0 RID: 4032
[Preserve]
public class XUiC_PopupToolTip : XUiController
{
	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x06007EE8 RID: 32488 RVA: 0x0030517D File Offset: 0x0030337D
	// (set) Token: 0x06007EE7 RID: 32487 RVA: 0x0030516D File Offset: 0x0030336D
	public float TextAlphaCurrent
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.textAlphaCurrent;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.textAlphaCurrent = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000EF7 RID: 3831
	// (set) Token: 0x06007EE9 RID: 32489 RVA: 0x00305185 File Offset: 0x00303385
	public string TooltipText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.tooltipText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007EEA RID: 32490 RVA: 0x00305195 File Offset: 0x00303395
	public override void Init()
	{
		base.Init();
		XUiC_PopupToolTip.ID = base.WindowGroup.Id;
		this.toolbelt = this.xui.GetChildByType<XUiC_Toolbelt>();
	}

	// Token: 0x06007EEB RID: 32491 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007EEC RID: 32492 RVA: 0x003051C0 File Offset: 0x003033C0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (GameStats.GetInt(EnumGameStats.GameState) == 1)
		{
			if ((this.xui.playerUI != null && this.xui.playerUI.entityPlayer != null && this.xui.playerUI.entityPlayer.IsDead()) || !this.xui.playerUI.windowManager.IsHUDEnabled())
			{
				this.ClearTooltipsInternal();
			}
			if (!this.pauseToolTips)
			{
				this.TextAlphaCurrent = Mathf.Lerp(this.textAlphaCurrent, this.textAlphaTarget, _dt * 3f);
				if (this.countdownTooltip.HasPassed() && this.xui.IsReady && !XUiC_SubtitlesDisplay.IsDisplaying)
				{
					this.DisplayTooltipText();
				}
			}
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06007EED RID: 32493 RVA: 0x003052A4 File Offset: 0x003034A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "text")
		{
			_value = this.tooltipText;
			return true;
		}
		if (_bindingName == "textalpha")
		{
			_value = this.textalphaFormatter.Format((int)(255f * this.textAlphaCurrent));
			return true;
		}
		if (!(_bindingName == "yoffset_secondrow"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.yoffsetFormatter.Format((this.toolbelt != null && ToolbeltConfig.CurrentConfig.BarsPerPage > 1) ? this.yOffsetSecondRow : 0);
		return true;
	}

	// Token: 0x06007EEE RID: 32494 RVA: 0x00305335 File Offset: 0x00303535
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "yoffset_second_row")
		{
			this.yOffsetSecondRow = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007EEF RID: 32495 RVA: 0x0030535D File Offset: 0x0030355D
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearTooltipsInternal()
	{
		this.tooltipQueue.Clear();
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007EF0 RID: 32496 RVA: 0x00305380 File Offset: 0x00303580
	[PublicizedFrom(EAccessModifier.Private)]
	public void QueueTooltipInternal(string _text, string[] _args, string _alertSound, ToolTipEvent _eventHandler, bool _showImmediately, bool _pinTooltip, float _timeout)
	{
		if (string.IsNullOrEmpty(_text) && string.IsNullOrEmpty(_alertSound) && _eventHandler == null)
		{
			return;
		}
		string text = Localization.Get(_text, false, null);
		if (_args != null && _args.Length != 0)
		{
			text = string.Format(text, _args);
		}
		XUiC_PopupToolTip.Tooltip tooltip = new XUiC_PopupToolTip.Tooltip(_text, text, _alertSound, _eventHandler, _timeout, _pinTooltip);
		if (_pinTooltip)
		{
			this.immediateTip = tooltip;
			if (!this.tooltipQueue.Contains(tooltip))
			{
				this.tooltipQueue.Enqueue(tooltip);
			}
			this.DisplayTooltipText();
			return;
		}
		if (_showImmediately)
		{
			this.immediateTip = tooltip;
			this.DisplayTooltipText();
			return;
		}
		if (!this.tooltipQueue.Contains(tooltip))
		{
			this.tooltipQueue.Enqueue(tooltip);
		}
	}

	// Token: 0x06007EF1 RID: 32497 RVA: 0x00305424 File Offset: 0x00303624
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemovePinnedTooltipInternal(string _key)
	{
		foreach (XUiC_PopupToolTip.Tooltip tooltip in this.tooltipQueue)
		{
			if (tooltip.Pinned && tooltip.Key == _key)
			{
				tooltip.RemoveOnDequeue = true;
				break;
			}
		}
	}

	// Token: 0x06007EF2 RID: 32498 RVA: 0x00305490 File Offset: 0x00303690
	[PublicizedFrom(EAccessModifier.Private)]
	public void DisplayTooltipText()
	{
		if (this.tooltipQueue.Count == 0 && this.immediateTip == null)
		{
			this.TextAlphaCurrent = 0f;
			this.TooltipText = string.Empty;
			return;
		}
		XUiC_PopupToolTip.Tooltip tooltip = null;
		bool flag = false;
		if (this.immediateTip != null)
		{
			tooltip = this.immediateTip;
			this.immediateTip = null;
		}
		else
		{
			while (this.tooltipQueue.Count > 0)
			{
				tooltip = this.tooltipQueue.Dequeue();
				if (tooltip.RemoveOnDequeue)
				{
					tooltip = null;
				}
				else
				{
					if (tooltip.Pinned)
					{
						if (this.tooltipQueue.Count == 0)
						{
							flag = true;
						}
						this.tooltipQueue.Enqueue(tooltip);
						break;
					}
					if (tooltip.Timeout <= 0f || Time.time - tooltip.CreationTime < tooltip.Timeout)
					{
						break;
					}
					tooltip = null;
				}
			}
		}
		if (tooltip == null)
		{
			this.TextAlphaCurrent = 0f;
			this.TooltipText = string.Empty;
			return;
		}
		if (!string.IsNullOrEmpty(tooltip.AlertSoundName))
		{
			Manager.PlayInsidePlayerHead(tooltip.AlertSoundName, -1, 0f, false, false);
		}
		ToolTipEvent @event = tooltip.Event;
		if (@event != null)
		{
			@event.HandleEvent();
		}
		this.TextAlphaCurrent = (flag ? this.TextAlphaCurrent : 0f);
		if (!string.IsNullOrEmpty(tooltip.Text))
		{
			this.textAlphaTarget = 1f;
			this.TooltipText = tooltip.Text;
		}
		else
		{
			this.textAlphaTarget = 0f;
			this.TooltipText = "";
		}
		this.countdownTooltip.ResetAndRestart();
	}

	// Token: 0x06007EF3 RID: 32499 RVA: 0x003055FB File Offset: 0x003037FB
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetToolTipPauseInternal(bool _isPaused)
	{
		this.pauseToolTips = _isPaused;
		if (_isPaused)
		{
			this.TextAlphaCurrent = 0f;
			return;
		}
		this.TextAlphaCurrent = this.textAlphaTarget;
	}

	// Token: 0x06007EF4 RID: 32500 RVA: 0x0030561F File Offset: 0x0030381F
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearCurrentTooltipInternal()
	{
		this.TooltipText = "";
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007EF5 RID: 32501 RVA: 0x00305642 File Offset: 0x00303842
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_PopupToolTip GetInstance(XUi _xui)
	{
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)_xui.playerUI.windowManager.GetWindow(XUiC_PopupToolTip.ID);
		if (xuiWindowGroup == null)
		{
			return null;
		}
		XUiController controller = xuiWindowGroup.Controller;
		if (controller == null)
		{
			return null;
		}
		return controller.GetChildByType<XUiC_PopupToolTip>();
	}

	// Token: 0x06007EF6 RID: 32502 RVA: 0x00305674 File Offset: 0x00303874
	public static void ClearTooltips(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearTooltipsInternal();
	}

	// Token: 0x06007EF7 RID: 32503 RVA: 0x00305686 File Offset: 0x00303886
	public static void QueueTooltip(XUi _xui, string _text, string[] _args, string _alertSound, ToolTipEvent _eventHandler, bool _showImmediately, bool _pinTooltip, float _timeout = 0f)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.QueueTooltipInternal(_text, _args, _alertSound, _eventHandler, _showImmediately, _pinTooltip, _timeout);
	}

	// Token: 0x06007EF8 RID: 32504 RVA: 0x003056A3 File Offset: 0x003038A3
	public static void SetToolTipPause(XUi _xui, bool _isPaused)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.SetToolTipPauseInternal(_isPaused);
	}

	// Token: 0x06007EF9 RID: 32505 RVA: 0x003056B6 File Offset: 0x003038B6
	public static void ClearCurrentTooltip(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearCurrentTooltipInternal();
	}

	// Token: 0x06007EFA RID: 32506 RVA: 0x003056C8 File Offset: 0x003038C8
	public static void RemovePinnedTooltip(XUi _xui, string _key)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.RemovePinnedTooltipInternal(_key);
	}

	// Token: 0x04005E41 RID: 24129
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005E42 RID: 24130
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffsetSecondRow = 75;

	// Token: 0x04005E43 RID: 24131
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Toolbelt toolbelt;

	// Token: 0x04005E44 RID: 24132
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipText = string.Empty;

	// Token: 0x04005E45 RID: 24133
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaTarget;

	// Token: 0x04005E46 RID: 24134
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaCurrent;

	// Token: 0x04005E47 RID: 24135
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CountdownTimer countdownTooltip = new CountdownTimer(5f, true);

	// Token: 0x04005E48 RID: 24136
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pauseToolTips;

	// Token: 0x04005E49 RID: 24137
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<XUiC_PopupToolTip.Tooltip> tooltipQueue = new Queue<XUiC_PopupToolTip.Tooltip>();

	// Token: 0x04005E4A RID: 24138
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupToolTip.Tooltip immediateTip;

	// Token: 0x04005E4B RID: 24139
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt textalphaFormatter = new CachedStringFormatterInt();

	// Token: 0x04005E4C RID: 24140
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt yoffsetFormatter = new CachedStringFormatterInt();

	// Token: 0x02000FC1 RID: 4033
	[PublicizedFrom(EAccessModifier.Private)]
	public class Tooltip : IEquatable<XUiC_PopupToolTip.Tooltip>
	{
		// Token: 0x06007EFD RID: 32509 RVA: 0x00305740 File Offset: 0x00303940
		public Tooltip(string _key, string _text, string _alertSoundName, ToolTipEvent _event, float _timeout = 0f, bool _pinned = false)
		{
			this.Key = _key;
			this.Text = _text;
			this.AlertSoundName = _alertSoundName;
			this.Event = _event;
			this.Timeout = _timeout;
			this.Pinned = _pinned;
			this.CreationTime = Time.time;
		}

		// Token: 0x06007EFE RID: 32510 RVA: 0x00305796 File Offset: 0x00303996
		public bool Equals(XUiC_PopupToolTip.Tooltip _other)
		{
			return _other != null && (this == _other || this.Text == _other.Text);
		}

		// Token: 0x06007EFF RID: 32511 RVA: 0x003057B4 File Offset: 0x003039B4
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((XUiC_PopupToolTip.Tooltip)_obj)));
		}

		// Token: 0x06007F00 RID: 32512 RVA: 0x003057E2 File Offset: 0x003039E2
		public override int GetHashCode()
		{
			if (this.Text == null)
			{
				return 0;
			}
			return this.Text.GetHashCode();
		}

		// Token: 0x04005E4D RID: 24141
		public readonly string Key;

		// Token: 0x04005E4E RID: 24142
		public readonly string Text;

		// Token: 0x04005E4F RID: 24143
		public readonly string AlertSoundName;

		// Token: 0x04005E50 RID: 24144
		public readonly ToolTipEvent Event;

		// Token: 0x04005E51 RID: 24145
		public readonly float Timeout = -1f;

		// Token: 0x04005E52 RID: 24146
		public readonly bool Pinned;

		// Token: 0x04005E53 RID: 24147
		public bool RemoveOnDequeue;

		// Token: 0x04005E54 RID: 24148
		public readonly float CreationTime;
	}
}
