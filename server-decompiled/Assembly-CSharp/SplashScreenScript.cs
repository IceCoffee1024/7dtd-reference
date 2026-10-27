using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Token: 0x02001307 RID: 4871
public class SplashScreenScript : MonoBehaviour
{
	// Token: 0x06009A1D RID: 39453 RVA: 0x003A77B0 File Offset: 0x003A59B0
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

	// Token: 0x06009A1E RID: 39454 RVA: 0x003A7886 File Offset: 0x003A5A86
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if ((this.videoPlayer.isPlaying && Input.anyKey) || this.videoFinished)
		{
			SceneManager.LoadScene(SplashScreenScript.MainSceneName);
		}
	}

	// Token: 0x06009A1F RID: 39455 RVA: 0x003A78AE File Offset: 0x003A5AAE
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoPrepared(VideoPlayer player)
	{
		base.StartCoroutine(this.DelayVideoRoutine());
	}

	// Token: 0x06009A20 RID: 39456 RVA: 0x003A78BD File Offset: 0x003A5ABD
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DelayVideoRoutine()
	{
		yield return new WaitForSecondsRealtime(0.3f);
		this.videoPlayer.Play();
		yield break;
	}

	// Token: 0x06009A21 RID: 39457 RVA: 0x003A78CC File Offset: 0x003A5ACC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoFinished(VideoPlayer player)
	{
		this.videoFinished = true;
	}

	// Token: 0x06009A22 RID: 39458 RVA: 0x003A78D5 File Offset: 0x003A5AD5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(VideoPlayer player, string message)
	{
		Log.Error("SplashScreen video error: " + message);
		this.videoFinished = true;
	}

	// Token: 0x06009A23 RID: 39459 RVA: 0x003A78EE File Offset: 0x003A5AEE
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		GUI.contentColor = new Color(0f, 0f, 0f, 0f);
		GUILayout.Label("Test", Array.Empty<GUILayoutOption>());
	}

	// Token: 0x040073D7 RID: 29655
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string MainSceneName = "SceneGame";

	// Token: 0x040073D8 RID: 29656
	public Transform wdwSplashScreen;

	// Token: 0x040073D9 RID: 29657
	public UILabel labelEaWarning;

	// Token: 0x040073DA RID: 29658
	public VideoPlayer videoPlayer;

	// Token: 0x040073DB RID: 29659
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool videoFinished;
}
