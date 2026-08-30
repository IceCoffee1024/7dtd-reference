using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Token: 0x020012D5 RID: 4821
public class SplashScreenScript : MonoBehaviour
{
	// Token: 0x06009919 RID: 39193 RVA: 0x003A07FC File Offset: 0x0039E9FC
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

	// Token: 0x0600991A RID: 39194 RVA: 0x003A08D2 File Offset: 0x0039EAD2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if ((this.videoPlayer.isPlaying && Input.anyKey) || this.videoFinished)
		{
			SceneManager.LoadScene(SplashScreenScript.MainSceneName);
		}
	}

	// Token: 0x0600991B RID: 39195 RVA: 0x003A08FA File Offset: 0x0039EAFA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoPrepared(VideoPlayer player)
	{
		base.StartCoroutine(this.DelayVideoRoutine());
	}

	// Token: 0x0600991C RID: 39196 RVA: 0x003A0909 File Offset: 0x0039EB09
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DelayVideoRoutine()
	{
		yield return new WaitForSecondsRealtime(0.3f);
		this.videoPlayer.Play();
		yield break;
	}

	// Token: 0x0600991D RID: 39197 RVA: 0x003A0918 File Offset: 0x0039EB18
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoFinished(VideoPlayer player)
	{
		this.videoFinished = true;
	}

	// Token: 0x0600991E RID: 39198 RVA: 0x003A0921 File Offset: 0x0039EB21
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(VideoPlayer player, string message)
	{
		Log.Error("SplashScreen video error: " + message);
		this.videoFinished = true;
	}

	// Token: 0x0600991F RID: 39199 RVA: 0x003A093A File Offset: 0x0039EB3A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		GUI.contentColor = new Color(0f, 0f, 0f, 0f);
		GUILayout.Label("Test", Array.Empty<GUILayoutOption>());
	}

	// Token: 0x0400731B RID: 29467
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string MainSceneName = "SceneGame";

	// Token: 0x0400731C RID: 29468
	public Transform wdwSplashScreen;

	// Token: 0x0400731D RID: 29469
	public UILabel labelEaWarning;

	// Token: 0x0400731E RID: 29470
	public VideoPlayer videoPlayer;

	// Token: 0x0400731F RID: 29471
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool videoFinished;
}
