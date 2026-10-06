using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Token: 0x020012F5 RID: 4853
public class SplashScreenScript : MonoBehaviour
{
	// Token: 0x060099C5 RID: 39365 RVA: 0x003A53AC File Offset: 0x003A35AC
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

	// Token: 0x060099C6 RID: 39366 RVA: 0x003A5482 File Offset: 0x003A3682
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if ((this.videoPlayer.isPlaying && Input.anyKey) || this.videoFinished)
		{
			SceneManager.LoadScene(SplashScreenScript.MainSceneName);
		}
	}

	// Token: 0x060099C7 RID: 39367 RVA: 0x003A54AA File Offset: 0x003A36AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoPrepared(VideoPlayer player)
	{
		base.StartCoroutine(this.DelayVideoRoutine());
	}

	// Token: 0x060099C8 RID: 39368 RVA: 0x003A54B9 File Offset: 0x003A36B9
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DelayVideoRoutine()
	{
		yield return new WaitForSecondsRealtime(0.3f);
		this.videoPlayer.Play();
		yield break;
	}

	// Token: 0x060099C9 RID: 39369 RVA: 0x003A54C8 File Offset: 0x003A36C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnVideoFinished(VideoPlayer player)
	{
		this.videoFinished = true;
	}

	// Token: 0x060099CA RID: 39370 RVA: 0x003A54D1 File Offset: 0x003A36D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVideoErrorReceived(VideoPlayer player, string message)
	{
		Log.Error("SplashScreen video error: " + message);
		this.videoFinished = true;
	}

	// Token: 0x060099CB RID: 39371 RVA: 0x003A54EA File Offset: 0x003A36EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		GUI.contentColor = new Color(0f, 0f, 0f, 0f);
		GUILayout.Label("Test", Array.Empty<GUILayoutOption>());
	}

	// Token: 0x04007396 RID: 29590
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string MainSceneName = "SceneGame";

	// Token: 0x04007397 RID: 29591
	public Transform wdwSplashScreen;

	// Token: 0x04007398 RID: 29592
	public UILabel labelEaWarning;

	// Token: 0x04007399 RID: 29593
	public VideoPlayer videoPlayer;

	// Token: 0x0400739A RID: 29594
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool videoFinished;
}
