using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200108F RID: 4239
[Preserve]
public class XUiC_SubtitlesDisplay : XUiController
{
	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x06008676 RID: 34422 RVA: 0x00336048 File Offset: 0x00334248
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

	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x06008677 RID: 34423 RVA: 0x00336081 File Offset: 0x00334281
	// (set) Token: 0x06008678 RID: 34424 RVA: 0x00336088 File Offset: 0x00334288
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

	// Token: 0x06008679 RID: 34425 RVA: 0x003360AC File Offset: 0x003342AC
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

	// Token: 0x0600867A RID: 34426 RVA: 0x00336108 File Offset: 0x00334308
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

	// Token: 0x0600867B RID: 34427 RVA: 0x00336188 File Offset: 0x00334388
	public override void OnClose()
	{
		base.OnClose();
		XUiC_SubtitlesDisplay.IsDisplaying = false;
	}

	// Token: 0x0600867C RID: 34428 RVA: 0x00336198 File Offset: 0x00334398
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

	// Token: 0x0600867D RID: 34429 RVA: 0x00336200 File Offset: 0x00334400
	public static void Close(XUi _xui)
	{
		XUiC_SubtitlesDisplay instance = XUiC_SubtitlesDisplay.getInstance(_xui);
		if (instance == null)
		{
			return;
		}
		_xui.playerUI.windowManager.Close(instance.windowGroup, false);
	}

	// Token: 0x0600867E RID: 34430 RVA: 0x0033622F File Offset: 0x0033442F
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_SubtitlesDisplay getInstance(XUi _xui)
	{
		return _xui.GetChildByType<XUiC_SubtitlesDisplay>();
	}

	// Token: 0x040063ED RID: 25581
	[PublicizedFrom(EAccessModifier.Private)]
	public const float MinDuration = 3f;

	// Token: 0x040063EE RID: 25582
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DurationAdd = 1f;

	// Token: 0x040063EF RID: 25583
	[PublicizedFrom(EAccessModifier.Private)]
	public const int LabelPadding = 28;

	// Token: 0x040063F0 RID: 25584
	[XuiBindComponent("lblSpeaker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label speakerLabel;

	// Token: 0x040063F1 RID: 25585
	[XuiBindComponent("lblSubtitle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label subtitlesLabel;

	// Token: 0x040063F2 RID: 25586
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTime;

	// Token: 0x040063F3 RID: 25587
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x040063F4 RID: 25588
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetHeight = 64;

	// Token: 0x040063F5 RID: 25589
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingUpdate;

	// Token: 0x040063F6 RID: 25590
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingText = "";

	// Token: 0x040063F7 RID: 25591
	[PublicizedFrom(EAccessModifier.Private)]
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x040063F8 RID: 25592
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDisplaying;

	// Token: 0x040063F9 RID: 25593
	[PublicizedFrom(EAccessModifier.Private)]
	public static NGUIWindowManager _playerNguiWindowManager;
}
