using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200107F RID: 4223
[Preserve]
public class XUiC_SubtitlesDisplay : XUiController
{
	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x06008634 RID: 34356 RVA: 0x00332710 File Offset: 0x00330910
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

	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x06008635 RID: 34357 RVA: 0x00332749 File Offset: 0x00330949
	// (set) Token: 0x06008636 RID: 34358 RVA: 0x00332750 File Offset: 0x00330950
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

	// Token: 0x06008637 RID: 34359 RVA: 0x00332774 File Offset: 0x00330974
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

	// Token: 0x06008638 RID: 34360 RVA: 0x003327D0 File Offset: 0x003309D0
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

	// Token: 0x06008639 RID: 34361 RVA: 0x00332850 File Offset: 0x00330A50
	public override void OnClose()
	{
		base.OnClose();
		XUiC_SubtitlesDisplay.IsDisplaying = false;
	}

	// Token: 0x0600863A RID: 34362 RVA: 0x00332860 File Offset: 0x00330A60
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

	// Token: 0x0600863B RID: 34363 RVA: 0x003328C8 File Offset: 0x00330AC8
	public static void Close(XUi _xui)
	{
		XUiC_SubtitlesDisplay instance = XUiC_SubtitlesDisplay.getInstance(_xui);
		if (instance == null)
		{
			return;
		}
		_xui.playerUI.windowManager.Close(instance.windowGroup, false);
	}

	// Token: 0x0600863C RID: 34364 RVA: 0x003328F7 File Offset: 0x00330AF7
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_SubtitlesDisplay getInstance(XUi _xui)
	{
		return _xui.GetChildByType<XUiC_SubtitlesDisplay>();
	}

	// Token: 0x040063AD RID: 25517
	[PublicizedFrom(EAccessModifier.Private)]
	public const float MinDuration = 3f;

	// Token: 0x040063AE RID: 25518
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DurationAdd = 1f;

	// Token: 0x040063AF RID: 25519
	[PublicizedFrom(EAccessModifier.Private)]
	public const int LabelPadding = 28;

	// Token: 0x040063B0 RID: 25520
	[XuiBindComponent("lblSpeaker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label speakerLabel;

	// Token: 0x040063B1 RID: 25521
	[XuiBindComponent("lblSubtitle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label subtitlesLabel;

	// Token: 0x040063B2 RID: 25522
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTime;

	// Token: 0x040063B3 RID: 25523
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x040063B4 RID: 25524
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetHeight = 64;

	// Token: 0x040063B5 RID: 25525
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingUpdate;

	// Token: 0x040063B6 RID: 25526
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingText = "";

	// Token: 0x040063B7 RID: 25527
	[PublicizedFrom(EAccessModifier.Private)]
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x040063B8 RID: 25528
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDisplaying;

	// Token: 0x040063B9 RID: 25529
	[PublicizedFrom(EAccessModifier.Private)]
	public static NGUIWindowManager _playerNguiWindowManager;
}
