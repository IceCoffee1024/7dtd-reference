using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x0200021F RID: 543
	[NativeHeader("Runtime/Misc/ResourceManagerUtility.h")]
	[NativeHeader("Runtime/Export/Resources/Resources.bindings.h")]
	internal static class ResourcesAPIInternal
	{
		// Token: 0x060017E2 RID: 6114
		[FreeFunction("Resources_Bindings::FindObjectsOfTypeAll")]
		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindObjectsOfTypeAll(Type type);

		// Token: 0x060017E3 RID: 6115
		[FreeFunction("GetShaderNameRegistry().FindShader")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Shader FindShaderByName(string name);

		// Token: 0x060017E4 RID: 6116
		[FreeFunction("Resources_Bindings::Load")]
		[NativeThrows]
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object Load(string path, [NotNull("ArgumentNullException")] Type systemTypeInstance);

		// Token: 0x060017E5 RID: 6117
		[NativeThrows]
		[FreeFunction("Resources_Bindings::LoadAll")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] LoadAll([NotNull("ArgumentNullException")] string path, [NotNull("ArgumentNullException")] Type systemTypeInstance);

		// Token: 0x060017E6 RID: 6118
		[FreeFunction("Resources_Bindings::LoadAsyncInternal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ResourceRequest LoadAsyncInternal(string path, Type type);

		// Token: 0x060017E7 RID: 6119
		[FreeFunction("Scripting::UnloadAssetFromScripting")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnloadAsset(Object assetToUnload);

		// Token: 0x02000220 RID: 544
		internal static class EntitiesAssetGC
		{
			// Token: 0x060017E8 RID: 6120
			[FreeFunction("Resources_Bindings::MarkInstanceIDsAsRoot")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			internal static extern void MarkInstanceIDsAsRoot(IntPtr instanceIDs, int count, IntPtr state);

			// Token: 0x060017E9 RID: 6121
			[FreeFunction("Resources_Bindings::EnableEntitiesAssetGCCallback")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			internal static extern void EnableEntitiesAssetGCCallback();

			// Token: 0x060017EA RID: 6122 RVA: 0x00027A7C File Offset: 0x00025C7C
			internal static void RegisterAdditionalRootsHandler(ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandlerDelegate newAdditionalRootsHandler)
			{
				bool flag = ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandler == null;
				if (flag)
				{
					ResourcesAPIInternal.EntitiesAssetGC.EnableEntitiesAssetGCCallback();
					ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandler = newAdditionalRootsHandler;
				}
				else
				{
					Debug.LogWarning("Attempting to register more than one AdditionalRootsHandlerDelegate! Only one may be registered at a time.");
				}
			}

			// Token: 0x060017EB RID: 6123 RVA: 0x00027AB4 File Offset: 0x00025CB4
			[UsedByNativeCode]
			private static void GetAdditionalRoots(IntPtr state)
			{
				bool flag = ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandler != null;
				if (flag)
				{
					ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandler(state);
				}
			}

			// Token: 0x0400088D RID: 2189
			internal static ResourcesAPIInternal.EntitiesAssetGC.AdditionalRootsHandlerDelegate AdditionalRootsHandler;

			// Token: 0x02000221 RID: 545
			// (Invoke) Token: 0x060017ED RID: 6125
			internal delegate void AdditionalRootsHandlerDelegate(IntPtr state);
		}
	}
}
