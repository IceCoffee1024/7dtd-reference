using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB1 RID: 4017
[Preserve]
public class XUiC_PopupToolTip : XUiController
{
	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x06007ED9 RID: 32473 RVA: 0x003021A5 File Offset: 0x003003A5
	// (set) Token: 0x06007ED8 RID: 32472 RVA: 0x00302195 File Offset: 0x00300395
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

	// Token: 0x17000ED0 RID: 3792
	// (set) Token: 0x06007EDA RID: 32474 RVA: 0x003021AD File Offset: 0x003003AD
	public string TooltipText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.tooltipText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007EDB RID: 32475 RVA: 0x003021BD File Offset: 0x003003BD
	public override void Init()
	{
		base.Init();
		XUiC_PopupToolTip.ID = base.WindowGroup.Id;
		this.toolbelt = this.xui.GetChildByType<XUiC_Toolbelt>();
	}

	// Token: 0x06007EDC RID: 32476 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007EDD RID: 32477 RVA: 0x003021E8 File Offset: 0x003003E8
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

	// Token: 0x06007EDE RID: 32478 RVA: 0x003022CC File Offset: 0x003004CC
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
		_value = this.yoffsetFormatter.Format((this.toolbelt != null && this.toolbelt.HasSecondRow) ? this.yOffsetSecondRow : 0);
		return true;
	}

	// Token: 0x06007EDF RID: 32479 RVA: 0x0030235D File Offset: 0x0030055D
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "yoffset_second_row")
		{
			this.yOffsetSecondRow = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007EE0 RID: 32480 RVA: 0x00302385 File Offset: 0x00300585
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearTooltipsInternal()
	{
		this.tooltipQueue.Clear();
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007EE1 RID: 32481 RVA: 0x003023A8 File Offset: 0x003005A8
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

	// Token: 0x06007EE2 RID: 32482 RVA: 0x0030244C File Offset: 0x0030064C
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

	// Token: 0x06007EE3 RID: 32483 RVA: 0x003024B8 File Offset: 0x003006B8
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

	// Token: 0x06007EE4 RID: 32484 RVA: 0x00302623 File Offset: 0x00300823
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

	// Token: 0x06007EE5 RID: 32485 RVA: 0x00302647 File Offset: 0x00300847
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearCurrentTooltipInternal()
	{
		this.TooltipText = "";
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007EE6 RID: 32486 RVA: 0x0030266A File Offset: 0x0030086A
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

	// Token: 0x06007EE7 RID: 32487 RVA: 0x0030269C File Offset: 0x0030089C
	public static void ClearTooltips(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearTooltipsInternal();
	}

	// Token: 0x06007EE8 RID: 32488 RVA: 0x003026AE File Offset: 0x003008AE
	public static void QueueTooltip(XUi _xui, string _text, string[] _args, string _alertSound, ToolTipEvent _eventHandler, bool _showImmediately, bool _pinTooltip, float _timeout = 0f)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.QueueTooltipInternal(_text, _args, _alertSound, _eventHandler, _showImmediately, _pinTooltip, _timeout);
	}

	// Token: 0x06007EE9 RID: 32489 RVA: 0x003026CB File Offset: 0x003008CB
	public static void SetToolTipPause(XUi _xui, bool _isPaused)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.SetToolTipPauseInternal(_isPaused);
	}

	// Token: 0x06007EEA RID: 32490 RVA: 0x003026DE File Offset: 0x003008DE
	public static void ClearCurrentTooltip(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearCurrentTooltipInternal();
	}

	// Token: 0x06007EEB RID: 32491 RVA: 0x003026F0 File Offset: 0x003008F0
	public static void RemovePinnedTooltip(XUi _xui, string _key)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.RemovePinnedTooltipInternal(_key);
	}

	// Token: 0x04005E18 RID: 24088
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005E19 RID: 24089
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffsetSecondRow = 75;

	// Token: 0x04005E1A RID: 24090
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Toolbelt toolbelt;

	// Token: 0x04005E1B RID: 24091
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipText = string.Empty;

	// Token: 0x04005E1C RID: 24092
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaTarget;

	// Token: 0x04005E1D RID: 24093
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaCurrent;

	// Token: 0x04005E1E RID: 24094
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CountdownTimer countdownTooltip = new CountdownTimer(5f, true);

	// Token: 0x04005E1F RID: 24095
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pauseToolTips;

	// Token: 0x04005E20 RID: 24096
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<XUiC_PopupToolTip.Tooltip> tooltipQueue = new Queue<XUiC_PopupToolTip.Tooltip>();

	// Token: 0x04005E21 RID: 24097
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupToolTip.Tooltip immediateTip;

	// Token: 0x04005E22 RID: 24098
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt textalphaFormatter = new CachedStringFormatterInt();

	// Token: 0x04005E23 RID: 24099
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt yoffsetFormatter = new CachedStringFormatterInt();

	// Token: 0x02000FB2 RID: 4018
	[PublicizedFrom(EAccessModifier.Private)]
	public class Tooltip : IEquatable<XUiC_PopupToolTip.Tooltip>
	{
		// Token: 0x06007EEE RID: 32494 RVA: 0x00302768 File Offset: 0x00300968
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

		// Token: 0x06007EEF RID: 32495 RVA: 0x003027BE File Offset: 0x003009BE
		public bool Equals(XUiC_PopupToolTip.Tooltip _other)
		{
			return _other != null && (this == _other || this.Text == _other.Text);
		}

		// Token: 0x06007EF0 RID: 32496 RVA: 0x003027DC File Offset: 0x003009DC
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((XUiC_PopupToolTip.Tooltip)_obj)));
		}

		// Token: 0x06007EF1 RID: 32497 RVA: 0x0030280A File Offset: 0x00300A0A
		public override int GetHashCode()
		{
			if (this.Text == null)
			{
				return 0;
			}
			return this.Text.GetHashCode();
		}

		// Token: 0x04005E24 RID: 24100
		public readonly string Key;

		// Token: 0x04005E25 RID: 24101
		public readonly string Text;

		// Token: 0x04005E26 RID: 24102
		public readonly string AlertSoundName;

		// Token: 0x04005E27 RID: 24103
		public readonly ToolTipEvent Event;

		// Token: 0x04005E28 RID: 24104
		public readonly float Timeout = -1f;

		// Token: 0x04005E29 RID: 24105
		public readonly bool Pinned;

		// Token: 0x04005E2A RID: 24106
		public bool RemoveOnDequeue;

		// Token: 0x04005E2B RID: 24107
		public readonly float CreationTime;
	}
}
