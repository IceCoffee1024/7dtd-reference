using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E96 RID: 3734
[Preserve]
public class XUiC_VideoPlayer : XUiController
{
	// Token: 0x06007486 RID: 29830 RVA: 0x002BE2AF File Offset: 0x002BC4AF
	public override void Init()
	{
		base.Init();
		XUiC_VideoPlayer.ID = this.windowGroup.Id;
		this.skipPrompt.IsVisible = false;
	}

	// Token: 0x06007487 RID: 29831 RVA: 0x002BE2D3 File Offset: 0x002BC4D3
	public static void PlayVideo(XUi _xui, VideoData _videoData, bool _skippable, XUiC_VideoPlayer.DelegateOnVideoFinished _videoFinishedCallback = null)
	{
		XUiC_VideoPlayer instance = XUiC_VideoPlayer.GetInstance(_xui);
		_xui.playerUI.windowManager.Open("VideoPlayer", true);
		instance.PlayVideo(_videoData, _skippable, _videoFinishedCallback);
	}

	// Token: 0x06007488 RID: 29832 RVA: 0x002BE2F9 File Offset: 0x002BC4F9
	public static void EndVideo(XUiC_VideoPlayer _videoPlayer)
	{
		_videoPlayer.FinishAndClose(true);
	}

	// Token: 0x06007489 RID: 29833 RVA: 0x002BE302 File Offset: 0x002BC502
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

	// Token: 0x0600748A RID: 29834 RVA: 0x002BE334 File Offset: 0x002BC534
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

	// Token: 0x0600748B RID: 29835 RVA: 0x002BE3F4 File Offset: 0x002BC5F4
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

	// Token: 0x0600748C RID: 29836 RVA: 0x002BE590 File Offset: 0x002BC790
	[XuiBindEvent("VideoError", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(XUiV_Video _source)
	{
		this.FinishAndClose(true);
	}

	// Token: 0x0600748D RID: 29837 RVA: 0x002BE599 File Offset: 0x002BC799
	[XuiBindEvent("VideoFinished", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoFinished(XUiV_Video _source)
	{
		this.FinishAndClose(false);
	}

	// Token: 0x0600748E RID: 29838 RVA: 0x002BE5A2 File Offset: 0x002BC7A2
	[PublicizedFrom(EAccessModifier.Private)]
	public void FinishAndClose(bool _skipped)
	{
		this.wasVideoSkipped = _skipped;
		XUiC_SubtitlesDisplay.Close(this.xui);
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600748F RID: 29839 RVA: 0x002BE5D2 File Offset: 0x002BC7D2
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

	// Token: 0x04005642 RID: 22082
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SkipVisibleDuration = 3f;

	// Token: 0x04005643 RID: 22083
	public static string ID = "";

	// Token: 0x04005644 RID: 22084
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Video videoView;

	// Token: 0x04005645 RID: 22085
	[XuiBindComponent("videoBackground", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite backgroundSprite;

	// Token: 0x04005646 RID: 22086
	[XuiBindComponent("skipPrompt", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiView skipPrompt;

	// Token: 0x04005647 RID: 22087
	[XuiBindComponent("lblSkip", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Label skipLabel;

	// Token: 0x04005648 RID: 22088
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VideoPlayer.DelegateOnVideoFinished onVideoFinished;

	// Token: 0x04005649 RID: 22089
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoData currentVideo;

	// Token: 0x0400564A RID: 22090
	[PublicizedFrom(EAccessModifier.Private)]
	public double previousTimestamp;

	// Token: 0x0400564B RID: 22091
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skippable;

	// Token: 0x0400564C RID: 22092
	[PublicizedFrom(EAccessModifier.Private)]
	public float skipVisibleTime;

	// Token: 0x0400564D RID: 22093
	public static bool IsVideoPlaying;

	// Token: 0x0400564E RID: 22094
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasVideoSkipped;

	// Token: 0x0400564F RID: 22095
	[PublicizedFrom(EAccessModifier.Private)]
	public bool subtitlesEnabled;

	// Token: 0x02000E97 RID: 3735
	// (Invoke) Token: 0x06007493 RID: 29843
	public delegate void DelegateOnVideoFinished(bool _skipped);
}
