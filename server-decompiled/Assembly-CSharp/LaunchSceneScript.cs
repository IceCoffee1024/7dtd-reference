using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001276 RID: 4726
public class LaunchSceneScript : MonoBehaviour
{
	// Token: 0x060095F6 RID: 38390 RVA: 0x0038A90E File Offset: 0x00388B0E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Cursor.visible = false;
		base.StartCoroutine(this.GoToNextSceneCo());
	}

	// Token: 0x060095F7 RID: 38391 RVA: 0x0038A923 File Offset: 0x00388B23
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

	// Token: 0x0400705F RID: 28767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string MainSceneName = "SceneGame";

	// Token: 0x04007060 RID: 28768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string SplashSceneName = "SceneSplash";

	// Token: 0x04007061 RID: 28769
	public UIPanel fadeInUIPanel;

	// Token: 0x04007062 RID: 28770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float fadeInDuration = 0.6f;
}
