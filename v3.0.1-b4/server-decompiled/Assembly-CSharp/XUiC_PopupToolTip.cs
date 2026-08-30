using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F98 RID: 3992
[Preserve]
public class XUiC_PopupToolTip : XUiController
{
	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x06007DE7 RID: 32231 RVA: 0x002FD9D1 File Offset: 0x002FBBD1
	// (set) Token: 0x06007DE6 RID: 32230 RVA: 0x002FD9C1 File Offset: 0x002FBBC1
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

	// Token: 0x17000E97 RID: 3735
	// (set) Token: 0x06007DE8 RID: 32232 RVA: 0x002FD9D9 File Offset: 0x002FBBD9
	public string TooltipText
	{
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.tooltipText = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007DE9 RID: 32233 RVA: 0x002FD9E9 File Offset: 0x002FBBE9
	public override void Init()
	{
		base.Init();
		XUiC_PopupToolTip.ID = base.WindowGroup.Id;
		this.toolbelt = this.xui.GetChildByType<XUiC_Toolbelt>();
	}

	// Token: 0x06007DEA RID: 32234 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007DEB RID: 32235 RVA: 0x002FDA14 File Offset: 0x002FBC14
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

	// Token: 0x06007DEC RID: 32236 RVA: 0x002FDAF8 File Offset: 0x002FBCF8
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

	// Token: 0x06007DED RID: 32237 RVA: 0x002FDB89 File Offset: 0x002FBD89
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "yoffset_second_row")
		{
			this.yOffsetSecondRow = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x06007DEE RID: 32238 RVA: 0x002FDBB1 File Offset: 0x002FBDB1
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearTooltipsInternal()
	{
		this.tooltipQueue.Clear();
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007DEF RID: 32239 RVA: 0x002FDBD4 File Offset: 0x002FBDD4
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

	// Token: 0x06007DF0 RID: 32240 RVA: 0x002FDC78 File Offset: 0x002FBE78
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

	// Token: 0x06007DF1 RID: 32241 RVA: 0x002FDCE4 File Offset: 0x002FBEE4
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

	// Token: 0x06007DF2 RID: 32242 RVA: 0x002FDE4F File Offset: 0x002FC04F
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

	// Token: 0x06007DF3 RID: 32243 RVA: 0x002FDE73 File Offset: 0x002FC073
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearCurrentTooltipInternal()
	{
		this.TooltipText = "";
		this.TextAlphaCurrent = 0f;
		this.textAlphaTarget = 0f;
	}

	// Token: 0x06007DF4 RID: 32244 RVA: 0x002FDE96 File Offset: 0x002FC096
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

	// Token: 0x06007DF5 RID: 32245 RVA: 0x002FDEC8 File Offset: 0x002FC0C8
	public static void ClearTooltips(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearTooltipsInternal();
	}

	// Token: 0x06007DF6 RID: 32246 RVA: 0x002FDEDA File Offset: 0x002FC0DA
	public static void QueueTooltip(XUi _xui, string _text, string[] _args, string _alertSound, ToolTipEvent _eventHandler, bool _showImmediately, bool _pinTooltip, float _timeout = 0f)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.QueueTooltipInternal(_text, _args, _alertSound, _eventHandler, _showImmediately, _pinTooltip, _timeout);
	}

	// Token: 0x06007DF7 RID: 32247 RVA: 0x002FDEF7 File Offset: 0x002FC0F7
	public static void SetToolTipPause(XUi _xui, bool _isPaused)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.SetToolTipPauseInternal(_isPaused);
	}

	// Token: 0x06007DF8 RID: 32248 RVA: 0x002FDF0A File Offset: 0x002FC10A
	public static void ClearCurrentTooltip(XUi _xui)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.ClearCurrentTooltipInternal();
	}

	// Token: 0x06007DF9 RID: 32249 RVA: 0x002FDF1C File Offset: 0x002FC11C
	public static void RemovePinnedTooltip(XUi _xui, string _key)
	{
		XUiC_PopupToolTip instance = XUiC_PopupToolTip.GetInstance(_xui);
		if (instance == null)
		{
			return;
		}
		instance.RemovePinnedTooltipInternal(_key);
	}

	// Token: 0x04005D51 RID: 23889
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ID = "";

	// Token: 0x04005D52 RID: 23890
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffsetSecondRow = 75;

	// Token: 0x04005D53 RID: 23891
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Toolbelt toolbelt;

	// Token: 0x04005D54 RID: 23892
	[PublicizedFrom(EAccessModifier.Private)]
	public string tooltipText = string.Empty;

	// Token: 0x04005D55 RID: 23893
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaTarget;

	// Token: 0x04005D56 RID: 23894
	[PublicizedFrom(EAccessModifier.Private)]
	public float textAlphaCurrent;

	// Token: 0x04005D57 RID: 23895
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CountdownTimer countdownTooltip = new CountdownTimer(5f, true);

	// Token: 0x04005D58 RID: 23896
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pauseToolTips;

	// Token: 0x04005D59 RID: 23897
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Queue<XUiC_PopupToolTip.Tooltip> tooltipQueue = new Queue<XUiC_PopupToolTip.Tooltip>();

	// Token: 0x04005D5A RID: 23898
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PopupToolTip.Tooltip immediateTip;

	// Token: 0x04005D5B RID: 23899
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt textalphaFormatter = new CachedStringFormatterInt();

	// Token: 0x04005D5C RID: 23900
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt yoffsetFormatter = new CachedStringFormatterInt();

	// Token: 0x02000F99 RID: 3993
	[PublicizedFrom(EAccessModifier.Private)]
	public class Tooltip : IEquatable<XUiC_PopupToolTip.Tooltip>
	{
		// Token: 0x06007DFC RID: 32252 RVA: 0x002FDF94 File Offset: 0x002FC194
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

		// Token: 0x06007DFD RID: 32253 RVA: 0x002FDFEA File Offset: 0x002FC1EA
		public bool Equals(XUiC_PopupToolTip.Tooltip _other)
		{
			return _other != null && (this == _other || this.Text == _other.Text);
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x002FE008 File Offset: 0x002FC208
		public override bool Equals(object _obj)
		{
			return _obj != null && (this == _obj || (!(_obj.GetType() != base.GetType()) && this.Equals((XUiC_PopupToolTip.Tooltip)_obj)));
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x002FE036 File Offset: 0x002FC236
		public override int GetHashCode()
		{
			if (this.Text == null)
			{
				return 0;
			}
			return this.Text.GetHashCode();
		}

		// Token: 0x04005D5D RID: 23901
		public readonly string Key;

		// Token: 0x04005D5E RID: 23902
		public readonly string Text;

		// Token: 0x04005D5F RID: 23903
		public readonly string AlertSoundName;

		// Token: 0x04005D60 RID: 23904
		public readonly ToolTipEvent Event;

		// Token: 0x04005D61 RID: 23905
		public readonly float Timeout = -1f;

		// Token: 0x04005D62 RID: 23906
		public readonly bool Pinned;

		// Token: 0x04005D63 RID: 23907
		public bool RemoveOnDequeue;

		// Token: 0x04005D64 RID: 23908
		public readonly float CreationTime;
	}
}
