using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.SceneManagement
{
	// Token: 0x02000329 RID: 809
	[NativeHeader("Runtime/Export/SceneManager/SceneUtility.bindings.h")]
	public static class SceneUtility
	{
		// Token: 0x060020CA RID: 8394
		[StaticAccessor("SceneUtilityBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetScenePathByBuildIndex(int buildIndex);

		// Token: 0x060020CB RID: 8395
		[StaticAccessor("SceneUtilityBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetBuildIndexByScenePath(string scenePath);
	}
}
