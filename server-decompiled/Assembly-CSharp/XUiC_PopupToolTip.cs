using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FD0 RID: 4048
[Preserve]
public class XUiC_PopupToolTip : XUiController
{
	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x06007F37 RID: 32567 RVA: 0x00306C99 File Offset: 0x00304E99
	// (set) Token: 0x06007F36 RID: 32566 RVA: 0x00306C89 File Offset: 0x00304E89
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

	// Token: 0x17000F02 RID: 3842
	// (set) Token: 0x06007F38 RID: 32568 RVA: 0x00306CA1 File Offset: 0x00304EA1
	public string TooltipText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.tooltipText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007F39 RID: 32569 RVA: 0x00306CB1 File Offset: 0x00304EB1
	public override void Init()
	{
		base.Init();
		XUiC_PopupToolTip.ID = base.WindowGroup.Id;
		this.toolbelt = this.xui.GetChildByType<XUiC_Toolbelt>();
	}

	// Token: 0x06007F3A RID: 32570 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007F3B RID: 32571 RVA: 0x00306CDC File Offset: 0x00304EDC
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

	// Token: 0x06007F3C RID: 32572 RVA: 0x00306DC0 File Offset: 0x00304FC0
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

	// Token: 0x06007F3D RID: 32573 RVA: 0x00306E51 File Offset: 0x00305051
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "yoffset_second_row")
		{
			this.yOffsetSecondRow = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007F3E RID: 32574 RVA: 0x00306E79 File Offset: 0x00305079
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearTooltipsInternal()
	{
		this.tooltipQueue.Clear();
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007F3F RID: 32575 RVA: 0x00306E9C File Offset: 0x0030509C
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

	// Token: 0x06007F40 RID: 32576 RVA: 0x00306F40 File Offset: 0x00305140
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

	// Token: 0x06007F41 RID: 32577 RVA: 0x00306FAC File Offset: 0x003051AC
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

	// Token: 0x06007F42 RID: 32578 RVA: 0x00307117 File Offset: 0x00305317
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

	// Token: 0x06007F43 RID: 32579 RVA: 0x0030713B File Offset: 0x0030533B
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearCurrentTooltipInternal()
	{
		this.TooltipText = "";
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007F44 RID: 32580 RVA: 0x0030715E File Offset: 0x0030535E
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

	// Token: 0x06007F45 RID: 32581 RVA: 0x00307190 File Offset: 0x00305390
	public static void ClearTooltips(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearTooltipsInternal();
	}

	// Token: 0x06007F46 RID: 32582 RVA: 0x003071A2 File Offset: 0x003053A2
	public static void QueueTooltip(XUi _xui, string _text, string[] _args, string _alertSound, ToolTipEvent _eventHandler, bool _showImmediately, bool _pinTooltip, float _timeout = 0f)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.QueueTooltipInternal(_text, _args, _alertSound, _eventHandler, _showImmediately, _pinTooltip, _timeout);
	}

	// Token: 0x06007F47 RID: 32583 RVA: 0x003071BF File Offset: 0x003053BF
	public static void SetToolTipPause(XUi _xui, bool _isPaused)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.SetToolTipPauseInternal(_isPaused);
	}

	// Token: 0x06007F48 RID: 32584 RVA: 0x003071D2 File Offset: 0x003053D2
	public static void ClearCurrentTooltip(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearCurrentTooltipInternal();
	}

	// Token: 0x06007F49 RID: 32585 RVA: 0x003071E4 File Offset: 0x003053E4
	public static void RemovePinnedTooltip(XUi _xui, string _key)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.RemovePinnedTooltipInternal(_key);
	}

	// Token: 0x04005E7C RID: 24188
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005E7D RID: 24189
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffsetSecondRow = 75;

	// Token: 0x04005E7E RID: 24190
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Toolbelt toolbelt;

	// Token: 0x04005E7F RID: 24191
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipText = string.Empty;

	// Token: 0x04005E80 RID: 24192
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaTarget;

	// Token: 0x04005E81 RID: 24193
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaCurrent;

	// Token: 0x04005E82 RID: 24194
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CountdownTimer countdownTooltip = new CountdownTimer(5f, true);

	// Token: 0x04005E83 RID: 24195
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pauseToolTips;

	// Token: 0x04005E84 RID: 24196
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<XUiC_PopupToolTip.Tooltip> tooltipQueue = new Queue<XUiC_PopupToolTip.Tooltip>();

	// Token: 0x04005E85 RID: 24197
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupToolTip.Tooltip immediateTip;

	// Token: 0x04005E86 RID: 24198
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt textalphaFormatter = new CachedStringFormatterInt();

	// Token: 0x04005E87 RID: 24199
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt yoffsetFormatter = new CachedStringFormatterInt();

	// Token: 0x02000FD1 RID: 4049
	[PublicizedFrom(EAccessModifier.Private)]
	public class Tooltip : IEquatable<XUiC_PopupToolTip.Tooltip>
	{
		// Token: 0x06007F4C RID: 32588 RVA: 0x0030725C File Offset: 0x0030545C
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

		// Token: 0x06007F4D RID: 32589 RVA: 0x003072B2 File Offset: 0x003054B2
		public bool Equals(XUiC_PopupToolTip.Tooltip _other)
		{
			return _other != null && (this == _other || this.Text == _other.Text);
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x003072D0 File Offset: 0x003054D0
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((XUiC_PopupToolTip.Tooltip)_obj)));
		}

		// Token: 0x06007F4F RID: 32591 RVA: 0x003072FE File Offset: 0x003054FE
		public override int GetHashCode()
		{
			if (this.Text == null)
			{
				return 0;
			}
			return this.Text.GetHashCode();
		}

		// Token: 0x04005E88 RID: 24200
		public readonly string Key;

		// Token: 0x04005E89 RID: 24201
		public readonly string Text;

		// Token: 0x04005E8A RID: 24202
		public readonly string AlertSoundName;

		// Token: 0x04005E8B RID: 24203
		public readonly ToolTipEvent Event;

		// Token: 0x04005E8C RID: 24204
		public readonly float Timeout = -1f;

		// Token: 0x04005E8D RID: 24205
		public readonly bool Pinned;

		// Token: 0x04005E8E RID: 24206
		public bool RemoveOnDequeue;

		// Token: 0x04005E8F RID: 24207
		public readonly float CreationTime;
	}
}
