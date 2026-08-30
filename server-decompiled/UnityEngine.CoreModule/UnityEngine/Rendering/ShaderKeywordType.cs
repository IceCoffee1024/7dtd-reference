using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000487 RID: 1159
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	public enum ShaderKeywordType
	{
		// Token: 0x04000F14 RID: 3860
		None,
		// Token: 0x04000F15 RID: 3861
		BuiltinDefault = 2,
		// Token: 0x04000F16 RID: 3862
		[Obsolete("Shader keyword type BuiltinExtra is no longer used. Use BuiltinDefault instead. (UnityUpgradable) -> BuiltinDefault")]
		BuiltinExtra = 6,
		// Token: 0x04000F17 RID: 3863
		[Obsolete("Shader keyword type BuiltinAutoStripped is no longer used. Use BuiltinDefault instead. (UnityUpgradable) -> BuiltinDefault")]
		BuiltinAutoStripped = 10,
		// Token: 0x04000F18 RID: 3864
		UserDefined = 16,
		// Token: 0x04000F19 RID: 3865
		Plugin = 32
	}
}
