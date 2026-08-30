using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.SceneManagement
{
	// Token: 0x02000321 RID: 801
	[NativeHeader("Runtime/Export/SceneManager/SceneManager.bindings.h")]
	[NativeHeader("Runtime/SceneManager/SceneManager.h")]
	[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
	internal static class SceneManagerAPIInternal
	{
		// Token: 0x0600206F RID: 8303
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetNumScenesInBuildSettings();

		// Token: 0x06002070 RID: 8304 RVA: 0x00035DB0 File Offset: 0x00033FB0
		[NativeThrows]
		public static Scene GetSceneByBuildIndex(int buildIndex)
		{
			Scene scene;
			SceneManagerAPIInternal.GetSceneByBuildIndex_Injected(buildIndex, out scene);
			return scene;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00035DC6 File Offset: 0x00033FC6
		[NativeThrows]
		public static AsyncOperation LoadSceneAsyncNameIndexInternal(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
		{
			return SceneManagerAPIInternal.LoadSceneAsyncNameIndexInternal_Injected(sceneName, sceneBuildIndex, ref parameters, mustCompleteNextFrame);
		}

		// Token: 0x06002072 RID: 8306
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AsyncOperation UnloadSceneNameIndexInternal(string sceneName, int sceneBuildIndex, bool immediately, UnloadSceneOptions options, out bool outSuccess);

		// Token: 0x06002073 RID: 8307
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSceneByBuildIndex_Injected(int buildIndex, out Scene ret);

		// Token: 0x06002074 RID: 8308
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AsyncOperation LoadSceneAsyncNameIndexInternal_Injected(string sceneName, int sceneBuildIndex, ref LoadSceneParameters parameters, bool mustCompleteNextFrame);
	}
}
