using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC5 RID: 3781
[Preserve]
public class XUiC_VideoPlayer : XUiController
{
	// Token: 0x06007543 RID: 30019 RVA: 0x002C60FF File Offset: 0x002C42FF
	public override void Init()
	{
		base.Init();
		XUiC_VideoPlayer.ID = this.windowGroup.Id;
		this.skipPrompt.IsVisible = false;
	}

	// Token: 0x06007544 RID: 30020 RVA: 0x002C6123 File Offset: 0x002C4323
	public static void PlayVideo(XUi _xui, VideoData _videoData, bool _skippable, XUiC_VideoPlayer.DelegateOnVideoFinished _videoFinishedCallback = null)
	{
		XUiC_VideoPlayer instance = XUiC_VideoPlayer.GetInstance(_xui);
		_xui.playerUI.windowManager.Open("VideoPlayer", true);
		instance.PlayVideo(_videoData, _skippable, _videoFinishedCallback);
	}

	// Token: 0x06007545 RID: 30021 RVA: 0x002C6149 File Offset: 0x002C4349
	public static void EndVideo(XUiC_VideoPlayer _videoPlayer)
	{
		_videoPlayer.FinishAndClose(true);
	}

	// Token: 0x06007546 RID: 30022 RVA: 0x002C6152 File Offset: 0x002C4352
	public static XUiC_VideoPlayer GetInstance(XUi _xui)
	{
		XUiWindowGroup xuiWindowGroup = (XUiWindowGroup)_xui.playerUI.windowManager.GetWindow(XUiC_VideoPlayer.ID);
		if (xuiWindowGroup == null)
		{
			return null;
		}
		XUiController controller = xuiWindowGroup.Controller;
		if (controller == null)
		{
			return null;
		}
		return controller.GetChildByType<XUiC_VideoPlayer>();
	}

	// Token: 0x06007547 RID: 30023 RVA: 0x002C6184 File Offset: 0x002C4384
	public void PlayVideo(VideoData _videoData, bool _skippable, XUiC_VideoPlayer.DelegateOnVideoFinished _videoFinishedCallback = null)
	{
		this.currentVideo = _videoData;
		this.subtitlesEnabled = GamePrefs.GetBool(EnumGamePrefs.OptionsSubtitlesEnabled);
		this.wasVideoSkipped = false;
		this.skippable = _skippable;
		this.skipPrompt.IsVisible = false;
		if (_videoFinishedCallback != null)
		{
			this.onVideoFinished = (XUiC_VideoPlayer.DelegateOnVideoFinished)Delegate.Combine(this.onVideoFinished, _videoFinishedCallback);
		}
		string bindingXuiMarkupString = this.xui.playerUI.playerInput.GUIActions.Cancel.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		this.skipLabel.Text = string.Format(Localization.Get("ui_video_skip", false, null), bindingXuiMarkupString);
		this.previousTimestamp = 0.0;
		this.videoView.VideoUri = _videoData.url;
		XUiC_VideoPlayer.IsVideoPlaying = true;
	}

	// Token: 0x06007548 RID: 30024 RVA: 0x002C6244 File Offset: 0x002C4444
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.subtitlesEnabled && this.videoView.Playing)
		{
			double currentTime = this.videoView.CurrentTime;
			foreach (VideoSubtitle videoSubtitle in this.currentVideo.subtitles)
			{
				if (videoSubtitle.timestamp >= this.previousTimestamp && videoSubtitle.timestamp <= currentTime)
				{
					GameManager.ShowSubtitle(LocalPlayerUI.primaryUI.xui, Manager.GetFormattedSubtitleSpeaker(videoSubtitle.subtitleId), Manager.GetFormattedSubtitle(videoSubtitle.subtitleId), videoSubtitle.duration, true);
					break;
				}
			}
			this.previousTimestamp = currentTime;
		}
		this.backgroundSprite.Color = Color.black;
		if (this.skippable && this.xui.playerUI.playerInput != null)
		{
			if (!this.skipPrompt.IsVisible && this.xui.playerUI.playerInput.AnyGUIActionPressed())
			{
				this.skipPrompt.IsVisible = true;
				this.skipVisibleTime = Time.time;
			}
			else if (this.skipPrompt.IsVisible && this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed)
			{
				this.FinishAndClose(true);
			}
			if (this.skipPrompt.IsVisible && Time.time - this.skipVisibleTime >= 3f)
			{
				this.skipPrompt.IsVisible = false;
			}
		}
	}

	// Token: 0x06007549 RID: 30025 RVA: 0x002C63E0 File Offset: 0x002C45E0
	[XuiBindEvent("VideoError", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(XUiV_Video _source)
	{
		this.FinishAndClose(true);
	}

	// Token: 0x0600754A RID: 30026 RVA: 0x002C63E9 File Offset: 0x002C45E9
	[XuiBindEvent("VideoFinished", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoFinished(XUiV_Video _source)
	{
		this.FinishAndClose(false);
	}

	// Token: 0x0600754B RID: 30027 RVA: 0x002C63F2 File Offset: 0x002C45F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void FinishAndClose(bool _skipped)
	{
		this.wasVideoSkipped = _skipped;
		XUiC_SubtitlesDisplay.Close(this.xui);
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600754C RID: 30028 RVA: 0x002C6422 File Offset: 0x002C4622
	public override void OnClose()
	{
		base.OnClose();
		XUiC_VideoPlayer.IsVideoPlaying = false;
		XUiC_VideoPlayer.DelegateOnVideoFinished delegateOnVideoFinished = this.onVideoFinished;
		if (delegateOnVideoFinished == null)
		{
			return;
		}
		delegateOnVideoFinished(this.wasVideoSkipped);
	}

	// Token: 0x04005742 RID: 22338
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SkipVisibleDuration = 3f;

	// Token: 0x04005743 RID: 22339
	public static string ID = "";

	// Token: 0x04005744 RID: 22340
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Video videoView;

	// Token: 0x04005745 RID: 22341
	[XuiBindComponent("videoBackground", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite backgroundSprite;

	// Token: 0x04005746 RID: 22342
	[XuiBindComponent("skipPrompt", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiView skipPrompt;

	// Token: 0x04005747 RID: 22343
	[XuiBindComponent("lblSkip", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Label skipLabel;

	// Token: 0x04005748 RID: 22344
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VideoPlayer.DelegateOnVideoFinished onVideoFinished;

	// Token: 0x04005749 RID: 22345
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoData currentVideo;

	// Token: 0x0400574A RID: 22346
	[PublicizedFrom(EAccessModifier.Private)]
	public double previousTimestamp;

	// Token: 0x0400574B RID: 22347
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skippable;

	// Token: 0x0400574C RID: 22348
	[PublicizedFrom(EAccessModifier.Private)]
	public float skipVisibleTime;

	// Token: 0x0400574D RID: 22349
	public static bool IsVideoPlaying;

	// Token: 0x0400574E RID: 22350
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasVideoSkipped;

	// Token: 0x0400574F RID: 22351
	[PublicizedFrom(EAccessModifier.Private)]
	public bool subtitlesEnabled;

	// Token: 0x02000EC6 RID: 3782
	// (Invoke) Token: 0x06007550 RID: 30032
	public delegate void DelegateOnVideoFinished(bool _skipped);
}
