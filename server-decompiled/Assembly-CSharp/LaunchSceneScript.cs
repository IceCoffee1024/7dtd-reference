using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200124C RID: 4684
public class LaunchSceneScript : MonoBehaviour
{
	// Token: 0x06009511 RID: 38161 RVA: 0x00383CCA File Offset: 0x00381ECA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Cursor.visible = false;
		base.StartCoroutine(this.GoToNextSceneCo());
	}

	// Token: 0x06009512 RID: 38162 RVA: 0x00383CDF File Offset: 0x00381EDF
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

	// Token: 0x04006FB4 RID: 28596
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string MainSceneName = "SceneGame";

	// Token: 0x04006FB5 RID: 28597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string SplashSceneName = "SceneSplash";

	// Token: 0x04006FB6 RID: 28598
	public UIPanel fadeInUIPanel;

	// Token: 0x04006FB7 RID: 28599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float fadeInDuration = 0.6f;
}
