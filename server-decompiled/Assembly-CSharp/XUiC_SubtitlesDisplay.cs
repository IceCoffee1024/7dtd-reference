using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001066 RID: 4198
[Preserve]
public class XUiC_SubtitlesDisplay : XUiController
{
	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x06008542 RID: 34114 RVA: 0x0032DEE8 File Offset: 0x0032C0E8
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

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x06008543 RID: 34115 RVA: 0x0032DF21 File Offset: 0x0032C121
	// (set) Token: 0x06008544 RID: 34116 RVA: 0x0032DF28 File Offset: 0x0032C128
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

	// Token: 0x06008545 RID: 34117 RVA: 0x0032DF4C File Offset: 0x0032C14C
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

	// Token: 0x06008546 RID: 34118 RVA: 0x0032DFA8 File Offset: 0x0032C1A8
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

	// Token: 0x06008547 RID: 34119 RVA: 0x0032E028 File Offset: 0x0032C228
	public override void OnClose()
	{
		base.OnClose();
		XUiC_SubtitlesDisplay.IsDisplaying = false;
	}

	// Token: 0x06008548 RID: 34120 RVA: 0x0032E038 File Offset: 0x0032C238
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

	// Token: 0x06008549 RID: 34121 RVA: 0x0032E0A0 File Offset: 0x0032C2A0
	public static void Close(XUi _xui)
	{
		XUiC_SubtitlesDisplay instance = XUiC_SubtitlesDisplay.getInstance(_xui);
		if (instance == null)
		{
			return;
		}
		_xui.playerUI.windowManager.Close(instance.windowGroup, false);
	}

	// Token: 0x0600854A RID: 34122 RVA: 0x0032E0CF File Offset: 0x0032C2CF
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_SubtitlesDisplay getInstance(XUi _xui)
	{
		return _xui.GetChildByType<XUiC_SubtitlesDisplay>();
	}

	// Token: 0x040062E6 RID: 25318
	[PublicizedFrom(EAccessModifier.Private)]
	public const float MinDuration = 3f;

	// Token: 0x040062E7 RID: 25319
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DurationAdd = 1f;

	// Token: 0x040062E8 RID: 25320
	[PublicizedFrom(EAccessModifier.Private)]
	public const int LabelPadding = 28;

	// Token: 0x040062E9 RID: 25321
	[XuiBindComponent("lblSpeaker", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label speakerLabel;

	// Token: 0x040062EA RID: 25322
	[XuiBindComponent("lblSubtitle", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label subtitlesLabel;

	// Token: 0x040062EB RID: 25323
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTime;

	// Token: 0x040062EC RID: 25324
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x040062ED RID: 25325
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetHeight = 64;

	// Token: 0x040062EE RID: 25326
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingUpdate;

	// Token: 0x040062EF RID: 25327
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingText = "";

	// Token: 0x040062F0 RID: 25328
	[PublicizedFrom(EAccessModifier.Private)]
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x040062F1 RID: 25329
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDisplaying;

	// Token: 0x040062F2 RID: 25330
	[PublicizedFrom(EAccessModifier.Private)]
	public static NGUIWindowManager _playerNguiWindowManager;
}
