using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000051 RID: 81
[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	// Token: 0x060001B2 RID: 434 RVA: 0x000102D6 File Offset: 0x0000E4D6
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			SceneManager.LoadScene(this.levelName);
		}
	}

	// Token: 0x0400025E RID: 606
	public string levelName;
}
