using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x0200114D RID: 4429
public class XUiV_Video : XUiV_TextureBased
{
	// Token: 0x140000EC RID: 236
	// (add) Token: 0x06008D6E RID: 36206 RVA: 0x003551A8 File Offset: 0x003533A8
	// (remove) Token: 0x06008D6F RID: 36207 RVA: 0x003551E0 File Offset: 0x003533E0
	public event XUiV_Video.VideoErrorDelegate VideoError;

	// Token: 0x140000ED RID: 237
	// (add) Token: 0x06008D70 RID: 36208 RVA: 0x00355218 File Offset: 0x00353418
	// (remove) Token: 0x06008D71 RID: 36209 RVA: 0x00355250 File Offset: 0x00353450
	public event XUiV_Video.VideoFinishedDelegate VideoFinished;

	// Token: 0x170010C5 RID: 4293
	// (get) Token: 0x06008D72 RID: 36210 RVA: 0x00355285 File Offset: 0x00353485
	// (set) Token: 0x06008D73 RID: 36211 RVA: 0x0035528D File Offset: 0x0035348D
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

	// Token: 0x170010C6 RID: 4294
	// (get) Token: 0x06008D74 RID: 36212 RVA: 0x003552A6 File Offset: 0x003534A6
	// (set) Token: 0x06008D75 RID: 36213 RVA: 0x003552AE File Offset: 0x003534AE
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

	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06008D76 RID: 36214 RVA: 0x003552D3 File Offset: 0x003534D3
	// (set) Token: 0x06008D77 RID: 36215 RVA: 0x003552DB File Offset: 0x003534DB
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

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06008D78 RID: 36216 RVA: 0x003552F4 File Offset: 0x003534F4
	// (set) Token: 0x06008D79 RID: 36217 RVA: 0x00355301 File Offset: 0x00353501
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

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x06008D7A RID: 36218 RVA: 0x0035532C File Offset: 0x0035352C
	// (set) Token: 0x06008D7B RID: 36219 RVA: 0x00355334 File Offset: 0x00353534
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

	// Token: 0x170010CA RID: 4298
	// (get) Token: 0x06008D7C RID: 36220 RVA: 0x0035534D File Offset: 0x0035354D
	// (set) Token: 0x06008D7D RID: 36221 RVA: 0x0035535A File Offset: 0x0035355A
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

	// Token: 0x06008D7E RID: 36222 RVA: 0x00355368 File Offset: 0x00353568
	public XUiV_Video(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D7F RID: 36223 RVA: 0x00355380 File Offset: 0x00353580
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<VideoPlayer>();
	}

	// Token: 0x06008D80 RID: 36224 RVA: 0x00355390 File Offset: 0x00353590
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.videoPlayer = this.uiTransform.gameObject.GetComponent<VideoPlayer>();
	}

	// Token: 0x06008D81 RID: 36225 RVA: 0x003553B0 File Offset: 0x003535B0
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

	// Token: 0x06008D82 RID: 36226 RVA: 0x00355431 File Offset: 0x00353631
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		this.destroyRenderTexture();
	}

	// Token: 0x06008D83 RID: 36227 RVA: 0x00355450 File Offset: 0x00353650
	public override void OnOpen()
	{
		base.OnOpen();
		ThreadManager.StartCoroutine(this.startVideoEndOfFrame());
	}

	// Token: 0x06008D84 RID: 36228 RVA: 0x00355464 File Offset: 0x00353664
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator startVideoEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		this.startVideo();
		yield break;
	}

	// Token: 0x06008D85 RID: 36229 RVA: 0x00355473 File Offset: 0x00353673
	public override void OnClose()
	{
		base.OnClose();
		this.destroyRenderTexture();
	}

	// Token: 0x06008D86 RID: 36230 RVA: 0x00355481 File Offset: 0x00353681
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref != this.VolumeSetting)
		{
			return;
		}
		this.updateVolumeLevel();
	}

	// Token: 0x06008D87 RID: 36231 RVA: 0x00355494 File Offset: 0x00353694
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

	// Token: 0x06008D88 RID: 36232 RVA: 0x0035563B File Offset: 0x0035383B
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

	// Token: 0x06008D89 RID: 36233 RVA: 0x00355662 File Offset: 0x00353862
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateRenderTexture();
	}

	// Token: 0x06008D8A RID: 36234 RVA: 0x00355674 File Offset: 0x00353874
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

	// Token: 0x06008D8B RID: 36235 RVA: 0x003556C9 File Offset: 0x003538C9
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

	// Token: 0x06008D8C RID: 36236 RVA: 0x003556D8 File Offset: 0x003538D8
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

	// Token: 0x06008D8D RID: 36237 RVA: 0x0035571C File Offset: 0x0035391C
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

	// Token: 0x06008D8E RID: 36238 RVA: 0x00355804 File Offset: 0x00353A04
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

	// Token: 0x06008D8F RID: 36239 RVA: 0x00355852 File Offset: 0x00353A52
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoPrepared(VideoPlayer _source)
	{
		ThreadManager.StartCoroutine(this.playCo());
	}

	// Token: 0x06008D90 RID: 36240 RVA: 0x00355860 File Offset: 0x00353A60
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

	// Token: 0x06008D91 RID: 36241 RVA: 0x0035586F File Offset: 0x00353A6F
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

	// Token: 0x06008D92 RID: 36242 RVA: 0x00355892 File Offset: 0x00353A92
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

	// Token: 0x0400679B RID: 26523
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoPlayer videoPlayer;

	// Token: 0x0400679C RID: 26524
	[PublicizedFrom(EAccessModifier.Private)]
	public RenderTexture rt;

	// Token: 0x0400679D RID: 26525
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint FrameDelay = 2U;

	// Token: 0x0400679E RID: 26526
	[PublicizedFrom(EAccessModifier.Private)]
	public bool loop;

	// Token: 0x0400679F RID: 26527
	[PublicizedFrom(EAccessModifier.Private)]
	public string videoUri;

	// Token: 0x040067A0 RID: 26528
	[PublicizedFrom(EAccessModifier.Private)]
	public bool videoUriChanged;

	// Token: 0x040067A1 RID: 26529
	[PublicizedFrom(EAccessModifier.Private)]
	public VideoAspectRatio videoAspect = VideoAspectRatio.FitInside;

	// Token: 0x040067A2 RID: 26530
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumGamePrefs volumeSetting = EnumGamePrefs.OptionsMenuMusicVolumeLevel;

	// Token: 0x0200114E RID: 4430
	// (Invoke) Token: 0x06008D94 RID: 36244
	public delegate void VideoErrorDelegate(XUiV_Video _sender);

	// Token: 0x0200114F RID: 4431
	// (Invoke) Token: 0x06008D98 RID: 36248
	public delegate void VideoFinishedDelegate(XUiV_Video _sender);
}
