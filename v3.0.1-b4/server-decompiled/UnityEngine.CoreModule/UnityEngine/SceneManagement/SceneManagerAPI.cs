using System;

namespace UnityEngine.SceneManagement
{
	// Token: 0x02000322 RID: 802
	public class SceneManagerAPI
	{
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x00035DD2 File Offset: 0x00033FD2
		internal static SceneManagerAPI ActiveAPI
		{
			get
			{
				return SceneManagerAPI.overrideAPI ?? SceneManagerAPI.s_DefaultAPI;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x00035DE2 File Offset: 0x00033FE2
		// (set) Token: 0x06002077 RID: 8311 RVA: 0x00035DE9 File Offset: 0x00033FE9
		public static SceneManagerAPI overrideAPI { get; set; }

		// Token: 0x06002078 RID: 8312 RVA: 0x00009E2F File Offset: 0x0000802F
		protected internal SceneManagerAPI()
		{
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00035DF1 File Offset: 0x00033FF1
		protected internal virtual int GetNumScenesInBuildSettings()
		{
			return SceneManagerAPIInternal.GetNumScenesInBuildSettings();
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00035DF8 File Offset: 0x00033FF8
		protected internal virtual Scene GetSceneByBuildIndex(int buildIndex)
		{
			return SceneManagerAPIInternal.GetSceneByBuildIndex(buildIndex);
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00035E00 File Offset: 0x00034000
		protected internal virtual AsyncOperation LoadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
		{
			return SceneManagerAPIInternal.LoadSceneAsyncNameIndexInternal(sceneName, sceneBuildIndex, parameters, mustCompleteNextFrame);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00035E0C File Offset: 0x0003400C
		protected internal virtual AsyncOperation UnloadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, bool immediately, UnloadSceneOptions options, out bool outSuccess)
		{
			return SceneManagerAPIInternal.UnloadSceneNameIndexInternal(sceneName, sceneBuildIndex, immediately, options, out outSuccess);
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00035E1A File Offset: 0x0003401A
		protected internal virtual AsyncOperation LoadFirstScene(bool mustLoadAsync)
		{
			return null;
		}

		// Token: 0x04000AC0 RID: 2752
		private static SceneManagerAPI s_DefaultAPI = new SceneManagerAPI();
	}
}
