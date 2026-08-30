using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Token: 0x020012BB RID: 4795
public class SplashScreenScript : MonoBehaviour
{
	// Token: 0x0600981E RID: 38942 RVA: 0x0039B5AC File Offset: 0x003997AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		if (!GameEntrypoint.EntrypointSuccess)
		{
			return;
		}
		if (GameManager.IsDedicatedServer)
		{
			SceneManager.LoadScene(SplashScreenScript.MainSceneName);
			return;
		}
		if (GameUtils.GetLaunchArgument("skipintro") != null || !string.IsNullOrEmpty(LaunchPrefs.RunAutomation.Value))
		{
			SceneManager.LoadScene(SplashScreenScript.MainSceneName);
			return;
		}
		GameOptionsManager.ApplyTextureQuality(-1);
		this.labelEaWarning.text = Localization.Get("splashMessageEarlyAccessWarning", false, null);
		this.videoPlayer.prepareCompleted += this.OnVideoPrepared;
		this.videoPlayer.loopPointReached += this.OnVideoFinished;
		this.videoPlayer.errorReceived += this.OnVideoErrorReceived;
		this.videoPlayer.url = Application.streamingAssetsPath + "/Video/TFP_Intro.webm";
		this.videoPlayer.Prepare();
	}

	// Token: 0x0600981F RID: 38943 RVA: 0x0039B682 File Offset: 0x00399882
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if ((this.videoPlayer.isPlaying && Input.anyKey) || this.videoFinished)
		{
			SceneManager.LoadScene(SplashScreenScript.MainSceneName);
		}
	}

	// Token: 0x06009820 RID: 38944 RVA: 0x0039B6AA File Offset: 0x003998AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoPrepared(VideoPlayer player)
	{
		base.StartCoroutine(this.DelayVideoRoutine());
	}

	// Token: 0x06009821 RID: 38945 RVA: 0x0039B6B9 File Offset: 0x003998B9
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DelayVideoRoutine()
	{
		yield return new WaitForSecondsRealtime(0.3f);
		this.videoPlayer.Play();
		yield break;
	}

	// Token: 0x06009822 RID: 38946 RVA: 0x0039B6C8 File Offset: 0x003998C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoFinished(VideoPlayer player)
	{
		this.videoFinished = true;
	}

	// Token: 0x06009823 RID: 38947 RVA: 0x0039B6D1 File Offset: 0x003998D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(VideoPlayer player, string message)
	{
		Log.Error("SplashScreen video error: " + message);
		this.videoFinished = true;
	}

	// Token: 0x06009824 RID: 38948 RVA: 0x0039B6EA File Offset: 0x003998EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		GUI.contentColor = new Color(0f, 0f, 0f, 0f);
		GUILayout.Label("Test", Array.Empty<GUILayoutOption>());
	}

	// Token: 0x04007244 RID: 29252
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string MainSceneName = "SceneGame";

	// Token: 0x04007245 RID: 29253
	public Transform wdwSplashScreen;

	// Token: 0x04007246 RID: 29254
	public UILabel labelEaWarning;

	// Token: 0x04007247 RID: 29255
	public VideoPlayer videoPlayer;

	// Token: 0x04007248 RID: 29256
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool videoFinished;
}
