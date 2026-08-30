using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EAB RID: 3755
[Preserve]
public class XUiC_VideoPlayer : XUiController
{
	// Token: 0x06007526 RID: 29990 RVA: 0x002C17DB File Offset: 0x002BF9DB
	public override void Init()
	{
		base.Init();
		XUiC_VideoPlayer.ID = this.windowGroup.Id;
		this.skipPrompt.IsVisible = false;
	}

	// Token: 0x06007527 RID: 29991 RVA: 0x002C17FF File Offset: 0x002BF9FF
	public static void PlayVideo(XUi _xui, VideoData _videoData, bool _skippable, XUiC_VideoPlayer.DelegateOnVideoFinished _videoFinishedCallback = null)
	{
		XUiC_VideoPlayer instance = XUiC_VideoPlayer.GetInstance(_xui);
		_xui.playerUI.windowManager.Open("VideoPlayer", true);
		instance.PlayVideo(_videoData, _skippable, _videoFinishedCallback);
	}

	// Token: 0x06007528 RID: 29992 RVA: 0x002C1825 File Offset: 0x002BFA25
	public static void EndVideo(XUiC_VideoPlayer _videoPlayer)
	{
		_videoPlayer.FinishAndClose(true);
	}

	// Token: 0x06007529 RID: 29993 RVA: 0x002C182E File Offset: 0x002BFA2E
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

	// Token: 0x0600752A RID: 29994 RVA: 0x002C1860 File Offset: 0x002BFA60
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

	// Token: 0x0600752B RID: 29995 RVA: 0x002C1920 File Offset: 0x002BFB20
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

	// Token: 0x0600752C RID: 29996 RVA: 0x002C1ABC File Offset: 0x002BFCBC
	[XuiBindEvent("VideoError", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(XUiV_Video _source)
	{
		this.FinishAndClose(true);
	}

	// Token: 0x0600752D RID: 29997 RVA: 0x002C1AC5 File Offset: 0x002BFCC5
	[XuiBindEvent("VideoFinished", "videoView")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoFinished(XUiV_Video _source)
	{
		this.FinishAndClose(false);
	}

	// Token: 0x0600752E RID: 29998 RVA: 0x002C1ACE File Offset: 0x002BFCCE
	[PublicizedFrom(EAccessModifier.Private)]
	public void FinishAndClose(bool _skipped)
	{
		this.wasVideoSkipped = _skipped;
		XUiC_SubtitlesDisplay.Close(this.xui);
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600752F RID: 29999 RVA: 0x002C1AFE File Offset: 0x002BFCFE
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

	// Token: 0x040056E6 RID: 22246
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SkipVisibleDuration = 3f;

	// Token: 0x040056E7 RID: 22247
	public static string ID = "";

	// Token: 0x040056E8 RID: 22248
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Video videoView;

	// Token: 0x040056E9 RID: 22249
	[XuiBindComponent("videoBackground", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Sprite backgroundSprite;

	// Token: 0x040056EA RID: 22250
	[XuiBindComponent("skipPrompt", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiView skipPrompt;

	// Token: 0x040056EB RID: 22251
	[XuiBindComponent("lblSkip", true)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiV_Label skipLabel;

	// Token: 0x040056EC RID: 22252
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_VideoPlayer.DelegateOnVideoFinished onVideoFinished;

	// Token: 0x040056ED RID: 22253
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoData currentVideo;

	// Token: 0x040056EE RID: 22254
	[PublicizedFrom(EAccessModifier.Private)]
	public double previousTimestamp;

	// Token: 0x040056EF RID: 22255
	[PublicizedFrom(EAccessModifier.Private)]
	public bool skippable;

	// Token: 0x040056F0 RID: 22256
	[PublicizedFrom(EAccessModifier.Private)]
	public float skipVisibleTime;

	// Token: 0x040056F1 RID: 22257
	public static bool IsVideoPlaying;

	// Token: 0x040056F2 RID: 22258
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasVideoSkipped;

	// Token: 0x040056F3 RID: 22259
	[PublicizedFrom(EAccessModifier.Private)]
	public bool subtitlesEnabled;

	// Token: 0x02000EAC RID: 3756
	// (Invoke) Token: 0x06007533 RID: 30003
	public delegate void DelegateOnVideoFinished(bool _skipped);
}
