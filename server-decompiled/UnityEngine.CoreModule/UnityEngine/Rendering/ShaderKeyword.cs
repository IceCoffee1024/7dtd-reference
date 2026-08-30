using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000488 RID: 1160
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[UsedByNativeCode]
	public struct ShaderKeyword
	{
		// Token: 0x060027D4 RID: 10196
		[FreeFunction("ShaderScripting::GetGlobalKeywordCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetGlobalKeywordCount();

		// Token: 0x060027D5 RID: 10197
		[FreeFunction("ShaderScripting::GetGlobalKeywordIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetGlobalKeywordIndex(string keyword);

		// Token: 0x060027D6 RID: 10198
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetKeywordCount(Shader shader);

		// Token: 0x060027D7 RID: 10199
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetKeywordIndex(Shader shader, string keyword);

		// Token: 0x060027D8 RID: 10200
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetComputeShaderKeywordCount(ComputeShader shader);

		// Token: 0x060027D9 RID: 10201
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetComputeShaderKeywordIndex(ComputeShader shader, string keyword);

		// Token: 0x060027DA RID: 10202
		[FreeFunction("ShaderScripting::CreateGlobalKeyword")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void CreateGlobalKeyword(string keyword);

		// Token: 0x060027DB RID: 10203
		[FreeFunction("ShaderScripting::GetKeywordType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ShaderKeywordType GetGlobalShaderKeywordType(uint keyword);

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x0004468C File Offset: 0x0004288C
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000446A4 File Offset: 0x000428A4
		public static ShaderKeywordType GetGlobalKeywordType(ShaderKeyword index)
		{
			bool flag = index.IsValid();
			ShaderKeywordType shaderKeywordType;
			if (flag)
			{
				shaderKeywordType = ShaderKeyword.GetGlobalShaderKeywordType(index.m_Index);
			}
			else
			{
				shaderKeywordType = ShaderKeywordType.UserDefined;
			}
			return shaderKeywordType;
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000446D4 File Offset: 0x000428D4
		public ShaderKeyword(string keywordName)
		{
			this.m_Name = keywordName;
			this.m_Index = ShaderKeyword.GetGlobalKeywordIndex(keywordName);
			bool flag = this.m_Index >= ShaderKeyword.GetGlobalKeywordCount();
			if (flag)
			{
				ShaderKeyword.CreateGlobalKeyword(keywordName);
				this.m_Index = ShaderKeyword.GetGlobalKeywordIndex(keywordName);
			}
			this.m_IsValid = true;
			this.m_IsLocal = false;
			this.m_IsCompute = false;
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x00044733 File Offset: 0x00042933
		public ShaderKeyword(Shader shader, string keywordName)
		{
			this.m_Name = keywordName;
			this.m_Index = ShaderKeyword.GetKeywordIndex(shader, keywordName);
			this.m_IsValid = this.m_Index < ShaderKeyword.GetKeywordCount(shader);
			this.m_IsLocal = true;
			this.m_IsCompute = false;
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x0004476C File Offset: 0x0004296C
		public ShaderKeyword(ComputeShader shader, string keywordName)
		{
			this.m_Name = keywordName;
			this.m_Index = ShaderKeyword.GetComputeShaderKeywordIndex(shader, keywordName);
			this.m_IsValid = this.m_Index < ShaderKeyword.GetComputeShaderKeywordCount(shader);
			this.m_IsLocal = true;
			this.m_IsCompute = true;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000447A8 File Offset: 0x000429A8
		public static bool IsKeywordLocal(ShaderKeyword keyword)
		{
			return keyword.m_IsLocal;
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000447C0 File Offset: 0x000429C0
		public bool IsValid()
		{
			return this.m_IsValid;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000447D8 File Offset: 0x000429D8
		public bool IsValid(ComputeShader shader)
		{
			return this.m_IsValid;
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x000447F0 File Offset: 0x000429F0
		public bool IsValid(Shader shader)
		{
			return this.m_IsValid;
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060027E5 RID: 10213 RVA: 0x00044808 File Offset: 0x00042A08
		public int index
		{
			get
			{
				return (int)this.m_Index;
			}
		}

		// Token: 0x060027E6 RID: 10214 RVA: 0x00044820 File Offset: 0x00042A20
		public override string ToString()
		{
			return this.m_Name;
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x00044838 File Offset: 0x00042A38
		[Obsolete("GetKeywordType is deprecated. Only global keywords can have a type. This method always returns ShaderKeywordType.UserDefined.")]
		public static ShaderKeywordType GetKeywordType(Shader shader, ShaderKeyword index)
		{
			return ShaderKeywordType.UserDefined;
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x0004484C File Offset: 0x00042A4C
		[Obsolete("GetKeywordType is deprecated. Only global keywords can have a type. This method always returns ShaderKeywordType.UserDefined.")]
		public static ShaderKeywordType GetKeywordType(ComputeShader shader, ShaderKeyword index)
		{
			return ShaderKeywordType.UserDefined;
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x00044860 File Offset: 0x00042A60
		[Obsolete("GetGlobalKeywordName is deprecated. Use the ShaderKeyword.name property instead.")]
		public static string GetGlobalKeywordName(ShaderKeyword index)
		{
			return index.m_Name;
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x00044878 File Offset: 0x00042A78
		[Obsolete("GetKeywordName is deprecated. Use the ShaderKeyword.name property instead.")]
		public static string GetKeywordName(Shader shader, ShaderKeyword index)
		{
			return index.m_Name;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x00044890 File Offset: 0x00042A90
		[Obsolete("GetKeywordName is deprecated. Use the ShaderKeyword.name property instead.")]
		public static string GetKeywordName(ComputeShader shader, ShaderKeyword index)
		{
			return index.m_Name;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000448A8 File Offset: 0x00042AA8
		[Obsolete("GetKeywordType is deprecated. Use ShaderKeyword.name instead.")]
		public ShaderKeywordType GetKeywordType()
		{
			return ShaderKeyword.GetGlobalKeywordType(this);
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000448C8 File Offset: 0x00042AC8
		[Obsolete("GetKeywordName is deprecated. Use ShaderKeyword.name instead.")]
		public string GetKeywordName()
		{
			return ShaderKeyword.GetGlobalKeywordName(this);
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000448E8 File Offset: 0x00042AE8
		[Obsolete("GetName() has been deprecated. Use ShaderKeyword.name instead.")]
		public string GetName()
		{
			return this.GetKeywordName();
		}

		// Token: 0x04000F1A RID: 3866
		internal string m_Name;

		// Token: 0x04000F1B RID: 3867
		internal uint m_Index;

		// Token: 0x04000F1C RID: 3868
		internal bool m_IsLocal;

		// Token: 0x04000F1D RID: 3869
		internal bool m_IsCompute;

		// Token: 0x04000F1E RID: 3870
		internal bool m_IsValid;
	}
}
