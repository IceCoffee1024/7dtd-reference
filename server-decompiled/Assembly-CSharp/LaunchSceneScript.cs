using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001264 RID: 4708
public class LaunchSceneScript : MonoBehaviour
{
	// Token: 0x0600959F RID: 38303 RVA: 0x00388512 File Offset: 0x00386712
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Cursor.visible = false;
		base.StartCoroutine(this.GoToNextSceneCo());
	}

	// Token: 0x060095A0 RID: 38304 RVA: 0x00388527 File Offset: 0x00386727
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

	// Token: 0x0400701E RID: 28702
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string MainSceneName = "SceneGame";

	// Token: 0x0400701F RID: 28703
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string SplashSceneName = "SceneSplash";

	// Token: 0x04007020 RID: 28704
	public UIPanel fadeInUIPanel;

	// Token: 0x04007021 RID: 28705
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float fadeInDuration = 0.6f;
}
