using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x0200117E RID: 4478
public class XUiV_Video : XUiV_TextureBased
{
	// Token: 0x140000EC RID: 236
	// (add) Token: 0x06008EEB RID: 36587 RVA: 0x0035E1A0 File Offset: 0x0035C3A0
	// (remove) Token: 0x06008EEC RID: 36588 RVA: 0x0035E1D8 File Offset: 0x0035C3D8
	public event XUiV_Video.VideoErrorDelegate VideoError;

	// Token: 0x140000ED RID: 237
	// (add) Token: 0x06008EED RID: 36589 RVA: 0x0035E210 File Offset: 0x0035C410
	// (remove) Token: 0x06008EEE RID: 36590 RVA: 0x0035E248 File Offset: 0x0035C448
	public event XUiV_Video.VideoFinishedDelegate VideoFinished;

	// Token: 0x17001142 RID: 4418
	// (get) Token: 0x06008EEF RID: 36591 RVA: 0x0035E27D File Offset: 0x0035C47D
	// (set) Token: 0x06008EF0 RID: 36592 RVA: 0x0035E285 File Offset: 0x0035C485
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

	// Token: 0x17001143 RID: 4419
	// (get) Token: 0x06008EF1 RID: 36593 RVA: 0x0035E29E File Offset: 0x0035C49E
	// (set) Token: 0x06008EF2 RID: 36594 RVA: 0x0035E2A6 File Offset: 0x0035C4A6
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

	// Token: 0x17001144 RID: 4420
	// (get) Token: 0x06008EF3 RID: 36595 RVA: 0x0035E2CB File Offset: 0x0035C4CB
	// (set) Token: 0x06008EF4 RID: 36596 RVA: 0x0035E2D3 File Offset: 0x0035C4D3
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

	// Token: 0x17001145 RID: 4421
	// (get) Token: 0x06008EF5 RID: 36597 RVA: 0x0035E2EC File Offset: 0x0035C4EC
	// (set) Token: 0x06008EF6 RID: 36598 RVA: 0x0035E2F9 File Offset: 0x0035C4F9
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

	// Token: 0x17001146 RID: 4422
	// (get) Token: 0x06008EF7 RID: 36599 RVA: 0x0035E324 File Offset: 0x0035C524
	// (set) Token: 0x06008EF8 RID: 36600 RVA: 0x0035E32C File Offset: 0x0035C52C
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

	// Token: 0x17001147 RID: 4423
	// (get) Token: 0x06008EF9 RID: 36601 RVA: 0x0035E345 File Offset: 0x0035C545
	// (set) Token: 0x06008EFA RID: 36602 RVA: 0x0035E352 File Offset: 0x0035C552
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

	// Token: 0x06008EFB RID: 36603 RVA: 0x0035E360 File Offset: 0x0035C560
	public XUiV_Video(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EFC RID: 36604 RVA: 0x0035E378 File Offset: 0x0035C578
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<VideoPlayer>();
	}

	// Token: 0x06008EFD RID: 36605 RVA: 0x0035E388 File Offset: 0x0035C588
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.videoPlayer = this.uiTransform.gameObject.GetComponent<VideoPlayer>();
	}

	// Token: 0x06008EFE RID: 36606 RVA: 0x0035E3A8 File Offset: 0x0035C5A8
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

	// Token: 0x06008EFF RID: 36607 RVA: 0x0035E429 File Offset: 0x0035C629
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		this.destroyRenderTexture();
	}

	// Token: 0x06008F00 RID: 36608 RVA: 0x0035E448 File Offset: 0x0035C648
	public override void OnOpen()
	{
		base.OnOpen();
		ThreadManager.StartCoroutine(this.startVideoEndOfFrame());
	}

	// Token: 0x06008F01 RID: 36609 RVA: 0x0035E45C File Offset: 0x0035C65C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startVideoEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.startVideo();
		yield break;
	}

	// Token: 0x06008F02 RID: 36610 RVA: 0x0035E46B File Offset: 0x0035C66B
	public override void OnClose()
	{
		base.OnClose();
		this.destroyRenderTexture();
	}

	// Token: 0x06008F03 RID: 36611 RVA: 0x0035E479 File Offset: 0x0035C679
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref != this.VolumeSetting)
		{
			return;
		}
		this.updateVolumeLevel();
	}

	// Token: 0x06008F04 RID: 36612 RVA: 0x0035E48C File Offset: 0x0035C68C
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

	// Token: 0x06008F05 RID: 36613 RVA: 0x0035E633 File Offset: 0x0035C833
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

	// Token: 0x06008F06 RID: 36614 RVA: 0x0035E65A File Offset: 0x0035C85A
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateRenderTexture();
	}

	// Token: 0x06008F07 RID: 36615 RVA: 0x0035E66C File Offset: 0x0035C86C
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

	// Token: 0x06008F08 RID: 36616 RVA: 0x0035E6C1 File Offset: 0x0035C8C1
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

	// Token: 0x06008F09 RID: 36617 RVA: 0x0035E6D0 File Offset: 0x0035C8D0
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

	// Token: 0x06008F0A RID: 36618 RVA: 0x0035E714 File Offset: 0x0035C914
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

	// Token: 0x06008F0B RID: 36619 RVA: 0x0035E7FC File Offset: 0x0035C9FC
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

	// Token: 0x06008F0C RID: 36620 RVA: 0x0035E84A File Offset: 0x0035CA4A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoPrepared(VideoPlayer _source)
	{
		ThreadManager.StartCoroutine(this.playCo());
	}

	// Token: 0x06008F0D RID: 36621 RVA: 0x0035E858 File Offset: 0x0035CA58
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

	// Token: 0x06008F0E RID: 36622 RVA: 0x0035E867 File Offset: 0x0035CA67
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

	// Token: 0x06008F0F RID: 36623 RVA: 0x0035E88A File Offset: 0x0035CA8A
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

	// Token: 0x040068CB RID: 26827
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoPlayer videoPlayer;

	// Token: 0x040068CC RID: 26828
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x040068CD RID: 26829
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint FrameDelay = 2U;

	// Token: 0x040068CE RID: 26830
	[PublicizedFrom(EAccessModifier.Private)]
	public bool loop;

	// Token: 0x040068CF RID: 26831
	[PublicizedFrom(EAccessModifier.Private)]
	public string videoUri;

	// Token: 0x040068D0 RID: 26832
	[PublicizedFrom(EAccessModifier.Private)]
	public bool videoUriChanged;

	// Token: 0x040068D1 RID: 26833
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoAspectRatio videoAspect = VideoAspectRatio.FitInside;

	// Token: 0x040068D2 RID: 26834
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs volumeSetting = EnumGamePrefs.OptionsMenuMusicVolumeLevel;

	// Token: 0x0200117F RID: 4479
	// (Invoke) Token: 0x06008F11 RID: 36625
	public delegate void VideoErrorDelegate(XUiV_Video _sender);

	// Token: 0x02001180 RID: 4480
	// (Invoke) Token: 0x06008F15 RID: 36629
	public delegate void VideoFinishedDelegate(XUiV_Video _sender);
}
