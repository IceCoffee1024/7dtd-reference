using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x0200118E RID: 4494
public class XUiV_Video : XUiV_TextureBased
{
	// Token: 0x140000EC RID: 236
	// (add) Token: 0x06008F3F RID: 36671 RVA: 0x00360290 File Offset: 0x0035E490
	// (remove) Token: 0x06008F40 RID: 36672 RVA: 0x003602C8 File Offset: 0x0035E4C8
	public event XUiV_Video.VideoErrorDelegate VideoError;

	// Token: 0x140000ED RID: 237
	// (add) Token: 0x06008F41 RID: 36673 RVA: 0x00360300 File Offset: 0x0035E500
	// (remove) Token: 0x06008F42 RID: 36674 RVA: 0x00360338 File Offset: 0x0035E538
	public event XUiV_Video.VideoFinishedDelegate VideoFinished;

	// Token: 0x1700114D RID: 4429
	// (get) Token: 0x06008F43 RID: 36675 RVA: 0x0036036D File Offset: 0x0035E56D
	// (set) Token: 0x06008F44 RID: 36676 RVA: 0x00360375 File Offset: 0x0035E575
	[XuiXmlAttribute("loop", false)]
	public bool Loop
	{
		get
		{
			return this.loop;
		}
		set
		{
			if (this.loop == value)
			{
				return;
			}
			this.loop = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700114E RID: 4430
	// (get) Token: 0x06008F45 RID: 36677 RVA: 0x0036038E File Offset: 0x0035E58E
	// (set) Token: 0x06008F46 RID: 36678 RVA: 0x00360396 File Offset: 0x0035E596
	[XuiXmlAttribute("uri", false)]
	public string VideoUri
	{
		get
		{
			return this.videoUri;
		}
		set
		{
			if (this.videoUri == value)
			{
				return;
			}
			this.videoUri = value;
			this.videoUriChanged = true;
			base.SetDirty();
		}
	}

	// Token: 0x1700114F RID: 4431
	// (get) Token: 0x06008F47 RID: 36679 RVA: 0x003603BB File Offset: 0x0035E5BB
	// (set) Token: 0x06008F48 RID: 36680 RVA: 0x003603C3 File Offset: 0x0035E5C3
	[XuiXmlAttribute("videoaspect", false)]
	public VideoAspectRatio VideoAspect
	{
		get
		{
			return this.videoAspect;
		}
		set
		{
			if (this.videoAspect == value)
			{
				return;
			}
			this.videoAspect = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001150 RID: 4432
	// (get) Token: 0x06008F49 RID: 36681 RVA: 0x003603DC File Offset: 0x0035E5DC
	// (set) Token: 0x06008F4A RID: 36682 RVA: 0x003603E9 File Offset: 0x0035E5E9
	public bool Playing
	{
		get
		{
			return this.videoPlayer.isPlaying;
		}
		set
		{
			if (this.videoPlayer.isPlaying == value)
			{
				return;
			}
			if (value)
			{
				this.videoPlayer.Play();
				return;
			}
			this.videoPlayer.Stop();
		}
	}

	// Token: 0x17001151 RID: 4433
	// (get) Token: 0x06008F4B RID: 36683 RVA: 0x00360414 File Offset: 0x0035E614
	// (set) Token: 0x06008F4C RID: 36684 RVA: 0x0036041C File Offset: 0x0035E61C
	public EnumGamePrefs VolumeSetting
	{
		get
		{
			return this.volumeSetting;
		}
		set
		{
			if (this.volumeSetting == value)
			{
				return;
			}
			this.volumeSetting = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001152 RID: 4434
	// (get) Token: 0x06008F4D RID: 36685 RVA: 0x00360435 File Offset: 0x0035E635
	// (set) Token: 0x06008F4E RID: 36686 RVA: 0x00360442 File Offset: 0x0035E642
	public double CurrentTime
	{
		get
		{
			return this.videoPlayer.time;
		}
		set
		{
			this.videoPlayer.time = value;
		}
	}

	// Token: 0x06008F4F RID: 36687 RVA: 0x00360450 File Offset: 0x0035E650
	public XUiV_Video(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008F50 RID: 36688 RVA: 0x00360468 File Offset: 0x0035E668
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<VideoPlayer>();
	}

	// Token: 0x06008F51 RID: 36689 RVA: 0x00360478 File Offset: 0x0035E678
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.videoPlayer = this.uiTransform.gameObject.GetComponent<VideoPlayer>();
	}

	// Token: 0x06008F52 RID: 36690 RVA: 0x00360498 File Offset: 0x0035E698
	public override void InitView()
	{
		base.InitView();
		this.videoPlayer.playOnAwake = false;
		this.videoPlayer.renderMode = VideoRenderMode.RenderTexture;
		this.videoPlayer.prepareCompleted += this.OnVideoPrepared;
		this.videoPlayer.loopPointReached += this.OnVideoFinished;
		this.videoPlayer.errorReceived += this.OnVideoErrorReceived;
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
	}

	// Token: 0x06008F53 RID: 36691 RVA: 0x00360519 File Offset: 0x0035E719
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		this.destroyRenderTexture();
	}

	// Token: 0x06008F54 RID: 36692 RVA: 0x00360538 File Offset: 0x0035E738
	public override void OnOpen()
	{
		base.OnOpen();
		ThreadManager.StartCoroutine(this.startVideoEndOfFrame());
	}

	// Token: 0x06008F55 RID: 36693 RVA: 0x0036054C File Offset: 0x0035E74C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startVideoEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.startVideo();
		yield break;
	}

	// Token: 0x06008F56 RID: 36694 RVA: 0x0036055B File Offset: 0x0035E75B
	public override void OnClose()
	{
		base.OnClose();
		this.destroyRenderTexture();
	}

	// Token: 0x06008F57 RID: 36695 RVA: 0x00360569 File Offset: 0x0035E769
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref != this.VolumeSetting)
		{
			return;
		}
		this.updateVolumeLevel();
	}

	// Token: 0x06008F58 RID: 36696 RVA: 0x0036057C File Offset: 0x0035E77C
	[PublicizedFrom(EAccessModifier.Private)]
	public void startVideo()
	{
		if (string.IsNullOrEmpty(this.VideoUri))
		{
			this.destroyRenderTexture();
			return;
		}
		this.updateRenderTexture();
		string text = null;
		string text2;
		if (ModManager.TryPatchModPathString(this.VideoUri, out text2))
		{
			text = text2;
		}
		else if (this.VideoUri[0] == '@' && this.VideoUri[1] != ':')
		{
			string text3 = this.VideoUri.Substring(1);
			if (text3.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
			{
				text = text3.Substring(5);
				if (text[0] != '/' && text[0] != '\\')
				{
					text = new Uri(((Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXServer) ? (Application.dataPath + "/../../") : (Application.dataPath + "/../")) + text).AbsoluteUri;
				}
			}
			if (text == null)
			{
				this.videoPlayer.url = text3;
			}
		}
		else
		{
			text = Application.streamingAssetsPath + this.VideoUri;
		}
		if (text != null)
		{
			string extension = Path.GetExtension(text);
			if (string.IsNullOrEmpty(extension) || extension.Length > 5)
			{
				string text4;
				if (Application.platform == RuntimePlatform.PS5)
				{
					text4 = ".mp4";
				}
				else
				{
					text4 = ".webm";
				}
				string text5 = text4;
				if (Application.platform == RuntimePlatform.PS5)
				{
					text4 = ".webm";
				}
				else
				{
					text4 = ".mp4";
				}
				string text6 = text4;
				if (File.Exists(text + text5))
				{
					text += text5;
				}
				else if (File.Exists(text + text6))
				{
					text += text6;
				}
				else
				{
					text = null;
				}
			}
			this.videoPlayer.url = text;
		}
		this.videoPlayer.Prepare();
	}

	// Token: 0x06008F59 RID: 36697 RVA: 0x00360723 File Offset: 0x0035E923
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopVideo()
	{
		if (!this.videoPlayer.isPlaying)
		{
			return;
		}
		this.videoPlayer.Stop();
		this.videoPlayer.url = null;
	}

	// Token: 0x06008F5A RID: 36698 RVA: 0x0036074A File Offset: 0x0035E94A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateRenderTexture();
	}

	// Token: 0x06008F5B RID: 36699 RVA: 0x0036075C File Offset: 0x0035E95C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.updateData();
		this.uiTexture.enabled = true;
		this.videoPlayer.aspectRatio = this.VideoAspect;
		this.videoPlayer.isLooping = this.Loop;
		if (this.videoUriChanged)
		{
			ThreadManager.StartCoroutine(this.videoUriChangedCo());
		}
	}

	// Token: 0x06008F5C RID: 36700 RVA: 0x003607B1 File Offset: 0x0035E9B1
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator videoUriChangedCo()
	{
		this.stopVideo();
		int i = 0;
		while ((long)i <= 2L)
		{
			yield return null;
			int num = i;
			i = num + 1;
		}
		this.startVideo();
		yield break;
	}

	// Token: 0x06008F5D RID: 36701 RVA: 0x003607C0 File Offset: 0x0035E9C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateVolumeLevel()
	{
		if (!this.videoPlayer.isPrepared || this.videoPlayer.audioTrackCount < 1)
		{
			return;
		}
		float @float = GamePrefs.GetFloat(this.VolumeSetting);
		this.videoPlayer.SetDirectAudioVolume(0, @float);
	}

	// Token: 0x06008F5E RID: 36702 RVA: 0x00360804 File Offset: 0x0035EA04
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRenderTexture()
	{
		Vector3[] worldCorners = this.uiTexture.worldCorners;
		Vector3 vector = this.xui.playerUI.camera.WorldToScreenPoint(worldCorners[2]);
		Vector3 vector2 = this.xui.playerUI.camera.WorldToScreenPoint(worldCorners[0]);
		int num = Mathf.RoundToInt(vector.x - vector2.x);
		int num2 = Mathf.RoundToInt(vector.y - vector2.y);
		if (num < 2 || num2 < 2)
		{
			return;
		}
		if (this.rt != null && this.rt.width == num && this.rt.height == num2)
		{
			return;
		}
		this.destroyRenderTexture();
		this.rt = new RenderTexture(num, num2, 16);
		this.rt.Create();
		this.Texture = (this.videoPlayer.targetTexture = this.rt);
	}

	// Token: 0x06008F5F RID: 36703 RVA: 0x003608EC File Offset: 0x0035EAEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void destroyRenderTexture()
	{
		if (this.rt == null)
		{
			return;
		}
		this.Texture = (this.videoPlayer.targetTexture = null);
		this.rt.Release();
		global::UnityEngine.Object.Destroy(this.rt);
		this.rt = null;
	}

	// Token: 0x06008F60 RID: 36704 RVA: 0x0036093A File Offset: 0x0035EB3A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoPrepared(VideoPlayer _source)
	{
		ThreadManager.StartCoroutine(this.playCo());
	}

	// Token: 0x06008F61 RID: 36705 RVA: 0x00360948 File Offset: 0x0035EB48
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator playCo()
	{
		int i = 0;
		while ((long)i <= 2L)
		{
			yield return null;
			int num = i;
			i = num + 1;
		}
		this.videoPlayer.Play();
		this.updateVolumeLevel();
		yield break;
	}

	// Token: 0x06008F62 RID: 36706 RVA: 0x00360957 File Offset: 0x0035EB57
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(VideoPlayer _source, string _message)
	{
		Log.Error("[XUi] Video player encountered an error. Skipping video. Message: " + _message);
		XUiV_Video.VideoErrorDelegate videoError = this.VideoError;
		if (videoError == null)
		{
			return;
		}
		videoError(this);
	}

	// Token: 0x06008F63 RID: 36707 RVA: 0x0036097A File Offset: 0x0035EB7A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoFinished(VideoPlayer _source)
	{
		XUiV_Video.VideoFinishedDelegate videoFinished = this.VideoFinished;
		if (videoFinished == null)
		{
			return;
		}
		videoFinished(this);
	}

	// Token: 0x0400690B RID: 26891
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoPlayer videoPlayer;

	// Token: 0x0400690C RID: 26892
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x0400690D RID: 26893
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint FrameDelay = 2U;

	// Token: 0x0400690E RID: 26894
	[PublicizedFrom(EAccessModifier.Private)]
	public bool loop;

	// Token: 0x0400690F RID: 26895
	[PublicizedFrom(EAccessModifier.Private)]
	public string videoUri;

	// Token: 0x04006910 RID: 26896
	[PublicizedFrom(EAccessModifier.Private)]
	public bool videoUriChanged;

	// Token: 0x04006911 RID: 26897
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoAspectRatio videoAspect = VideoAspectRatio.FitInside;

	// Token: 0x04006912 RID: 26898
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs volumeSetting = EnumGamePrefs.OptionsMenuMusicVolumeLevel;

	// Token: 0x0200118F RID: 4495
	// (Invoke) Token: 0x06008F65 RID: 36709
	public delegate void VideoErrorDelegate(XUiV_Video _sender);

	// Token: 0x02001190 RID: 4496
	// (Invoke) Token: 0x06008F69 RID: 36713
	public delegate void VideoFinishedDelegate(XUiV_Video _sender);
}
