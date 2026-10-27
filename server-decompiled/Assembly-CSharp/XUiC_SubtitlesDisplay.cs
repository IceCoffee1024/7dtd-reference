using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200109F RID: 4255
[Preserve]
public class XUiC_SubtitlesDisplay : XUiController
{
	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x060086C9 RID: 34505 RVA: 0x003380CC File Offset: 0x003362CC
	public static NGUIWindowManager PlayerNguiWindowManager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (XUiC_SubtitlesDisplay._playerNguiWindowManager == null)
			{
				LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
				if (uiforPrimaryPlayer != null)
				{
					XUiC_SubtitlesDisplay._playerNguiWindowManager = uiforPrimaryPlayer.nguiWindowManager;
				}
			}
			return XUiC_SubtitlesDisplay._playerNguiWindowManager;
		}
	}

	// Token: 0x17000FE3 RID: 4067
	// (get) Token: 0x060086CA RID: 34506 RVA: 0x00338105 File Offset: 0x00336305
	// (set) Token: 0x060086CB RID: 34507 RVA: 0x0033810C File Offset: 0x0033630C
	public static bool IsDisplaying
	{
		get
		{
			return XUiC_SubtitlesDisplay.isDisplaying;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			XUiC_SubtitlesDisplay.isDisplaying = value;
			if (XUiC_SubtitlesDisplay.PlayerNguiWindowManager)
			{
				GameManager.Instance.SetToolTipPause(XUiC_SubtitlesDisplay.PlayerNguiWindowManager, value);
			}
		}
	}

	// Token: 0x060086CC RID: 34508 RVA: 0x00338130 File Offset: 0x00336330
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSubtitle(string _speaker, string _content, float _duration, bool _centerAlign)
	{
		this.speakerLabel.Text = _speaker;
		this.subtitlesLabel.Text = _content;
		this.alignment = (_centerAlign ? NGUIText.Alignment.Center : NGUIText.Alignment.Left);
		this.subtitlesLabel.Alignment = this.alignment;
		this.openTime = Time.time;
		this.duration = _duration;
		XUiC_SubtitlesDisplay.IsDisplaying = true;
	}

	// Token: 0x060086CD RID: 34509 RVA: 0x0033818C File Offset: 0x0033638C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.subtitlesLabel.Alignment = this.alignment;
		this.subtitlesLabel.Size = new Vector2i(610, this.subtitlesLabel.Size.y);
		if (Time.time - this.openTime >= this.duration)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			XUiC_SubtitlesDisplay.IsDisplaying = false;
		}
	}

	// Token: 0x060086CE RID: 34510 RVA: 0x0033820C File Offset: 0x0033640C
	public override void OnClose()
	{
		base.OnClose();
		XUiC_SubtitlesDisplay.IsDisplaying = false;
	}

	// Token: 0x060086CF RID: 34511 RVA: 0x0033821C File Offset: 0x0033641C
	public static void DisplaySubtitle(XUi _xui, string _speakerText, string _contentText, float _duration = 3f, bool _centerAlign = false)
	{
		XUiC_SubtitlesDisplay instance = XUiC_SubtitlesDisplay.getInstance(_xui);
		if (instance == null)
		{
			return;
		}
		_xui.playerUI.windowManager.Open(instance.windowGroup, false);
		instance.setSubtitle(_speakerText, _contentText, Mathf.Max(_duration, 3f) + 1f, _centerAlign);
		if (XUiC_SubtitlesDisplay.PlayerNguiWindowManager != null)
		{
			GameManager.Instance.ClearCurrentTooltip(XUiC_SubtitlesDisplay.PlayerNguiWindowManager);
		}
	}

	// Token: 0x060086D0 RID: 34512 RVA: 0x00338284 File Offset: 0x00336484
	public static void Close(XUi _xui)
	{
		XUiC_SubtitlesDisplay instance = XUiC_SubtitlesDisplay.getInstance(_xui);
		if (instance == null)
		{
			return;
		}
		_xui.playerUI.windowManager.Close(instance.windowGroup, false);
	}

	// Token: 0x060086D1 RID: 34513 RVA: 0x003382B3 File Offset: 0x003364B3
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_SubtitlesDisplay getInstance(XUi _xui)
	{
		return _xui.GetChildByType<XUiC_SubtitlesDisplay>();
	}

	// Token: 0x0400642C RID: 25644
	[PublicizedFrom(EAccessModifier.Private)]
	public const float MinDuration = 3f;

	// Token: 0x0400642D RID: 25645
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DurationAdd = 1f;

	// Token: 0x0400642E RID: 25646
	[PublicizedFrom(EAccessModifier.Private)]
	public const int LabelPadding = 28;

	// Token: 0x0400642F RID: 25647
	[XuiBindComponent("lblSpeaker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label speakerLabel;

	// Token: 0x04006430 RID: 25648
	[XuiBindComponent("lblSubtitle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label subtitlesLabel;

	// Token: 0x04006431 RID: 25649
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTime;

	// Token: 0x04006432 RID: 25650
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x04006433 RID: 25651
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetHeight = 64;

	// Token: 0x04006434 RID: 25652
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingUpdate;

	// Token: 0x04006435 RID: 25653
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingText = "";

	// Token: 0x04006436 RID: 25654
	[PublicizedFrom(EAccessModifier.Private)]
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x04006437 RID: 25655
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDisplaying;

	// Token: 0x04006438 RID: 25656
	[PublicizedFrom(EAccessModifier.Private)]
	public static NGUIWindowManager _playerNguiWindowManager;
}
