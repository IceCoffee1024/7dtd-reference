using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001230 RID: 4656
public class LaunchSceneScript : MonoBehaviour
{
	// Token: 0x0600940D RID: 37901 RVA: 0x0037E9BA File Offset: 0x0037CBBA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Cursor.visible = false;
		base.StartCoroutine(this.GoToNextSceneCo());
	}

	// Token: 0x0600940E RID: 37902 RVA: 0x0037E9CF File Offset: 0x0037CBCF
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator GoToNextSceneCo()
	{
		string nextScene;
		bool flag;
		if (GameStartupHelper.GetCommandLineArgs().ContainsCaseInsensitive("-skipintro"))
		{
			nextScene = "SceneGame";
			flag = true;
		}
		else
		{
			nextScene = "SceneSplash";
			flag = false;
		}
		this.fadeInUIPanel.alpha = 0f;
		if (flag)
		{
			float timer = 0.6f;
			while (timer > 0f)
			{
				this.fadeInUIPanel.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01(timer / 0.6f));
				timer -= Time.deltaTime;
				yield return null;
			}
			this.fadeInUIPanel.alpha = 1f;
		}
		yield return new WaitForEndOfFrame();
		yield return GameEntrypoint.EntrypointCoroutine();
		if (!GameEntrypoint.EntrypointSuccess)
		{
			yield break;
		}
		SceneManager.LoadScene(nextScene);
		yield break;
	}

	// Token: 0x04006EDB RID: 28379
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string MainSceneName = "SceneGame";

	// Token: 0x04006EDC RID: 28380
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string SplashSceneName = "SceneSplash";

	// Token: 0x04006EDD RID: 28381
	public UIPanel fadeInUIPanel;

	// Token: 0x04006EDE RID: 28382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float fadeInDuration = 0.6f;
}
