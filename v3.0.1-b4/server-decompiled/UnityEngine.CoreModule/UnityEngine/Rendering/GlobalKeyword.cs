using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000482 RID: 1154
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	public readonly struct GlobalKeyword
	{
		// Token: 0x06002797 RID: 10135
		[FreeFunction("ShaderScripting::GetGlobalKeywordCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetGlobalKeywordCount();

		// Token: 0x06002798 RID: 10136
		[FreeFunction("ShaderScripting::GetGlobalKeywordIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetGlobalKeywordIndex(string keyword);

		// Token: 0x06002799 RID: 10137
		[FreeFunction("ShaderScripting::CreateGlobalKeyword")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateGlobalKeyword(string keyword);

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000440B0 File Offset: 0x000422B0
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000440C8 File Offset: 0x000422C8
		public static GlobalKeyword Create(string name)
		{
			GlobalKeyword.CreateGlobalKeyword(name);
			return new GlobalKeyword(name);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000440E8 File Offset: 0x000422E8
		public GlobalKeyword(string name)
		{
			this.m_Name = name;
			this.m_Index = GlobalKeyword.GetGlobalKeywordIndex(name);
			bool flag = this.m_Index >= GlobalKeyword.GetGlobalKeywordCount();
			if (flag)
			{
				Debug.LogErrorFormat("Global keyword {0} doesn't exist.", new object[] { name });
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00044134 File Offset: 0x00042334
		public override string ToString()
		{
			return this.m_Name;
		}

		// Token: 0x04000F09 RID: 3849
		internal readonly string m_Name;

		// Token: 0x04000F0A RID: 3850
		internal readonly uint m_Index;
	}
}
