using System;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000489 RID: 1161
	[NativeHeader("Editor/Src/Graphics/ShaderCompilerData.h")]
	[UsedByNativeCode]
	public struct ShaderKeywordSet
	{
		// Token: 0x060027EF RID: 10223 RVA: 0x00044900 File Offset: 0x00042B00
		[FreeFunction("keywords::IsKeywordEnabled")]
		private static bool IsGlobalKeywordEnabled(ShaderKeywordSet state, uint index)
		{
			return ShaderKeywordSet.IsGlobalKeywordEnabled_Injected(ref state, index);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x0004490A File Offset: 0x00042B0A
		[FreeFunction("keywords::IsKeywordEnabled")]
		private static bool IsKeywordEnabled(ShaderKeywordSet state, LocalKeywordSpace keywordSpace, uint index)
		{
			return ShaderKeywordSet.IsKeywordEnabled_Injected(ref state, ref keywordSpace, index);
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x00044916 File Offset: 0x00042B16
		[FreeFunction("keywords::IsKeywordEnabled")]
		private static bool IsKeywordNameEnabled(ShaderKeywordSet state, string name)
		{
			return ShaderKeywordSet.IsKeywordNameEnabled_Injected(ref state, name);
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x00044920 File Offset: 0x00042B20
		[FreeFunction("keywords::EnableKeyword")]
		private static void EnableGlobalKeyword(ShaderKeywordSet state, uint index)
		{
			ShaderKeywordSet.EnableGlobalKeyword_Injected(ref state, index);
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x0004492A File Offset: 0x00042B2A
		[FreeFunction("keywords::EnableKeyword")]
		private static void EnableKeywordName(ShaderKeywordSet state, string name)
		{
			ShaderKeywordSet.EnableKeywordName_Injected(ref state, name);
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x00044934 File Offset: 0x00042B34
		[FreeFunction("keywords::DisableKeyword")]
		private static void DisableGlobalKeyword(ShaderKeywordSet state, uint index)
		{
			ShaderKeywordSet.DisableGlobalKeyword_Injected(ref state, index);
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x0004493E File Offset: 0x00042B3E
		[FreeFunction("keywords::DisableKeyword")]
		private static void DisableKeywordName(ShaderKeywordSet state, string name)
		{
			ShaderKeywordSet.DisableKeywordName_Injected(ref state, name);
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x00044948 File Offset: 0x00042B48
		[FreeFunction("keywords::GetEnabledKeywords")]
		private static ShaderKeyword[] GetEnabledKeywords(ShaderKeywordSet state)
		{
			return ShaderKeywordSet.GetEnabledKeywords_Injected(ref state);
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x00044954 File Offset: 0x00042B54
		private void CheckKeywordCompatible(ShaderKeyword keyword)
		{
			bool isLocal = keyword.m_IsLocal;
			if (isLocal)
			{
				bool flag = this.m_Shader != IntPtr.Zero;
				if (flag)
				{
					Assert.IsTrue(!keyword.m_IsCompute, "Trying to use a keyword that comes from a different shader.");
				}
				else
				{
					Assert.IsTrue(keyword.m_IsCompute, "Trying to use a keyword that comes from a different shader.");
				}
			}
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000449AC File Offset: 0x00042BAC
		public bool IsEnabled(ShaderKeyword keyword)
		{
			this.CheckKeywordCompatible(keyword);
			return ShaderKeywordSet.IsKeywordNameEnabled(this, keyword.m_Name);
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000449D8 File Offset: 0x00042BD8
		public bool IsEnabled(GlobalKeyword keyword)
		{
			return ShaderKeywordSet.IsGlobalKeywordEnabled(this, keyword.m_Index);
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000449FC File Offset: 0x00042BFC
		public bool IsEnabled(LocalKeyword keyword)
		{
			return ShaderKeywordSet.IsKeywordEnabled(this, keyword.m_SpaceInfo, keyword.m_Index);
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x00044A28 File Offset: 0x00042C28
		public void Enable(ShaderKeyword keyword)
		{
			this.CheckKeywordCompatible(keyword);
			bool flag = keyword.m_IsLocal || !keyword.IsValid();
			if (flag)
			{
				ShaderKeywordSet.EnableKeywordName(this, keyword.m_Name);
			}
			else
			{
				ShaderKeywordSet.EnableGlobalKeyword(this, keyword.m_Index);
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x00044A80 File Offset: 0x00042C80
		public void Disable(ShaderKeyword keyword)
		{
			bool flag = keyword.m_IsLocal || !keyword.IsValid();
			if (flag)
			{
				ShaderKeywordSet.DisableKeywordName(this, keyword.m_Name);
			}
			else
			{
				ShaderKeywordSet.DisableGlobalKeyword(this, keyword.m_Index);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x00044AD0 File Offset: 0x00042CD0
		public ShaderKeyword[] GetShaderKeywords()
		{
			return ShaderKeywordSet.GetEnabledKeywords(this);
		}

		// Token: 0x060027FE RID: 10238
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsGlobalKeywordEnabled_Injected(ref ShaderKeywordSet state, uint index);

		// Token: 0x060027FF RID: 10239
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsKeywordEnabled_Injected(ref ShaderKeywordSet state, ref LocalKeywordSpace keywordSpace, uint index);

		// Token: 0x06002800 RID: 10240
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsKeywordNameEnabled_Injected(ref ShaderKeywordSet state, string name);

		// Token: 0x06002801 RID: 10241
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableGlobalKeyword_Injected(ref ShaderKeywordSet state, uint index);

		// Token: 0x06002802 RID: 10242
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableKeywordName_Injected(ref ShaderKeywordSet state, string name);

		// Token: 0x06002803 RID: 10243
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableGlobalKeyword_Injected(ref ShaderKeywordSet state, uint index);

		// Token: 0x06002804 RID: 10244
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableKeywordName_Injected(ref ShaderKeywordSet state, string name);

		// Token: 0x06002805 RID: 10245
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderKeyword[] GetEnabledKeywords_Injected(ref ShaderKeywordSet state);

		// Token: 0x04000F1F RID: 3871
		private IntPtr m_KeywordState;

		// Token: 0x04000F20 RID: 3872
		private IntPtr m_Shader;

		// Token: 0x04000F21 RID: 3873
		private IntPtr m_ComputeShader;

		// Token: 0x04000F22 RID: 3874
		private ulong m_StateIndex;
	}
}
