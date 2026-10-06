using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EBA RID: 3770
[Preserve]
public class XUiC_VideoPlayer : XUiController
{
	// Token: 0x0600751E RID: 29982 RVA: 0x002C4B93 File Offset: 0x002C2D93
	public override void Init()
	{
		base.Init();
		XUiC_VideoPlayer.ID = this.windowGroup.Id;
		this.skipPrompt.IsVisible = false;
	}

	// Token: 0x0600751F RID: 29983 RVA: 0x002C4BB7 File Offset: 0x002C2DB7
	public static void PlayVideo(XUi _xui, VideoData _videoData, bool _skippable, XUiC_VideoPlayer.DelegateOnVideoFinished _videoFinishedCallback = null)
	{
		XUiC_VideoPlayer instance = XUiC_VideoPlayer.GetInstance(_xui);
		_xui.playerUI.windowManager.Open("VideoPlayer", true);
		instance.PlayVideo(_videoData, _skippable, _videoFinishedCallback);
	}

	// Token: 0x06007520 RID: 29984 RVA: 0x002C4BDD File Offset: 0x002C2DDD
	public static void EndVideo(XUiC_VideoPlayer _videoPlayer)
	{
		_videoPlayer.FinishAndClose(true);
	}

	// Token: 0x06007521 RID: 29985 RVA: 0x002C4BE6 File Offset: 0x002C2DE6
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

	// Token: 0x06007522 RID: 29986 RVA: 0x002C4C18 File Offset: 0x002C2E18
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

	// Token: 0x06007523 RID: 29987 RVA: 0x002C4CD8 File Offset: 0x002C2ED8
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

	// Token: 0x06007524 RID: 29988 RVA: 0x002C4E74 File Offset: 0x002C3074
	[XuiBindEvent("VideoError", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(XUiV_Video _source)
	{
		this.FinishAndClose(true);
	}

	// Token: 0x06007525 RID: 29989 RVA: 0x002C4E7D File Offset: 0x002C307D
	[XuiBindEvent("VideoFinished", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoFinished(XUiV_Video _source)
	{
		this.FinishAndClose(false);
	}

	// Token: 0x06007526 RID: 29990 RVA: 0x002C4E86 File Offset: 0x002C3086
	[PublicizedFrom(EAccessModifier.Private)]
	public void FinishAndClose(bool _skipped)
	{
		this.wasVideoSkipped = _skipped;
		XUiC_SubtitlesDisplay.Close(this.xui);
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007527 RID: 29991 RVA: 0x002C4EB6 File Offset: 0x002C30B6
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

	// Token: 0x0400571D RID: 22301
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SkipVisibleDuration = 3f;

	// Token: 0x0400571E RID: 22302
	public static string ID = "";

	// Token: 0x0400571F RID: 22303
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Video videoView;

	// Token: 0x04005720 RID: 22304
	[XuiBindComponent("videoBackground", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite backgroundSprite;

	// Token: 0x04005721 RID: 22305
	[XuiBindComponent("skipPrompt", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiView skipPrompt;

	// Token: 0x04005722 RID: 22306
	[XuiBindComponent("lblSkip", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Label skipLabel;

	// Token: 0x04005723 RID: 22307
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VideoPlayer.DelegateOnVideoFinished onVideoFinished;

	// Token: 0x04005724 RID: 22308
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoData currentVideo;

	// Token: 0x04005725 RID: 22309
	[PublicizedFrom(EAccessModifier.Private)]
	public double previousTimestamp;

	// Token: 0x04005726 RID: 22310
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skippable;

	// Token: 0x04005727 RID: 22311
	[PublicizedFrom(EAccessModifier.Private)]
	public float skipVisibleTime;

	// Token: 0x04005728 RID: 22312
	public static bool IsVideoPlaying;

	// Token: 0x04005729 RID: 22313
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasVideoSkipped;

	// Token: 0x0400572A RID: 22314
	[PublicizedFrom(EAccessModifier.Private)]
	public bool subtitlesEnabled;

	// Token: 0x02000EBB RID: 3771
	// (Invoke) Token: 0x0600752B RID: 29995
	public delegate void DelegateOnVideoFinished(bool _skipped);
}
