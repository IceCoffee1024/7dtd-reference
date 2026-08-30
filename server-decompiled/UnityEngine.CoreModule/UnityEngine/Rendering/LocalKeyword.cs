using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000483 RID: 1155
	[UsedByNativeCode]
	[NativeHeader("Runtime/Graphics/ShaderScriptBindings.h")]
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	public readonly struct LocalKeyword : IEquatable<LocalKeyword>
	{
		// Token: 0x0600279E RID: 10142 RVA: 0x0004414C File Offset: 0x0004234C
		[FreeFunction("keywords::IsKeywordDynamic")]
		private static bool IsDynamic(LocalKeyword kw)
		{
			return LocalKeyword.IsDynamic_Injected(ref kw);
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x00044155 File Offset: 0x00042355
		[FreeFunction("keywords::IsKeywordOverridable")]
		private static bool IsOverridable(LocalKeyword kw)
		{
			return LocalKeyword.IsOverridable_Injected(ref kw);
		}

		// Token: 0x060027A0 RID: 10144
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetShaderKeywordCount(Shader shader);

		// Token: 0x060027A1 RID: 10145
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetShaderKeywordIndex(Shader shader, string keyword);

		// Token: 0x060027A2 RID: 10146
		[FreeFunction("ShaderScripting::GetKeywordCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetComputeShaderKeywordCount(ComputeShader shader);

		// Token: 0x060027A3 RID: 10147
		[FreeFunction("ShaderScripting::GetKeywordIndex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetComputeShaderKeywordIndex(ComputeShader shader, string keyword);

		// Token: 0x060027A4 RID: 10148 RVA: 0x0004415E File Offset: 0x0004235E
		[FreeFunction("keywords::GetKeywordType")]
		private static ShaderKeywordType GetKeywordType(LocalKeywordSpace spaceInfo, uint keyword)
		{
			return LocalKeyword.GetKeywordType_Injected(ref spaceInfo, keyword);
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x00044168 File Offset: 0x00042368
		[FreeFunction("keywords::IsKeywordValid")]
		private static bool IsValid(LocalKeywordSpace spaceInfo, uint keyword)
		{
			return LocalKeyword.IsValid_Injected(ref spaceInfo, keyword);
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x00044174 File Offset: 0x00042374
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x0004418C File Offset: 0x0004238C
		public bool isDynamic
		{
			get
			{
				return LocalKeyword.IsDynamic(this);
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000441AC File Offset: 0x000423AC
		public bool isOverridable
		{
			get
			{
				return LocalKeyword.IsOverridable(this);
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060027A9 RID: 10153 RVA: 0x000441CC File Offset: 0x000423CC
		public bool isValid
		{
			get
			{
				return LocalKeyword.IsValid(this.m_SpaceInfo, this.m_Index);
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x000441F0 File Offset: 0x000423F0
		public ShaderKeywordType type
		{
			get
			{
				return LocalKeyword.GetKeywordType(this.m_SpaceInfo, this.m_Index);
			}
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x00044214 File Offset: 0x00042414
		public LocalKeyword(Shader shader, string name)
		{
			bool flag = shader == null;
			if (flag)
			{
				Debug.LogError("Cannot initialize a LocalKeyword with a null Shader.");
			}
			this.m_SpaceInfo = shader.keywordSpace;
			this.m_Name = name;
			this.m_Index = LocalKeyword.GetShaderKeywordIndex(shader, name);
			bool flag2 = this.m_Index >= LocalKeyword.GetShaderKeywordCount(shader);
			if (flag2)
			{
				Debug.LogErrorFormat("Local keyword {0} doesn't exist in the shader.", new object[] { name });
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x00044284 File Offset: 0x00042484
		public LocalKeyword(ComputeShader shader, string name)
		{
			bool flag = shader == null;
			if (flag)
			{
				Debug.LogError("Cannot initialize a LocalKeyword with a null ComputeShader.");
			}
			this.m_SpaceInfo = shader.keywordSpace;
			this.m_Name = name;
			this.m_Index = LocalKeyword.GetComputeShaderKeywordIndex(shader, name);
			bool flag2 = this.m_Index >= LocalKeyword.GetComputeShaderKeywordCount(shader);
			if (flag2)
			{
				Debug.LogErrorFormat("Local keyword {0} doesn't exist in the compute shader.", new object[] { name });
			}
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000442F4 File Offset: 0x000424F4
		public override string ToString()
		{
			return this.m_Name;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x0004430C File Offset: 0x0004250C
		public override bool Equals(object o)
		{
			bool flag;
			if (o is LocalKeyword)
			{
				LocalKeyword localKeyword = (LocalKeyword)o;
				flag = this.Equals(localKeyword);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x00044338 File Offset: 0x00042538
		public bool Equals(LocalKeyword rhs)
		{
			return this.m_SpaceInfo == rhs.m_SpaceInfo && this.m_Index == rhs.m_Index;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x00044370 File Offset: 0x00042570
		public static bool operator ==(LocalKeyword lhs, LocalKeyword rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x0004438C File Offset: 0x0004258C
		public static bool operator !=(LocalKeyword lhs, LocalKeyword rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000443A8 File Offset: 0x000425A8
		public override int GetHashCode()
		{
			return this.m_Index.GetHashCode() ^ this.m_SpaceInfo.GetHashCode();
		}

		// Token: 0x060027B3 RID: 10163
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDynamic_Injected(ref LocalKeyword kw);

		// Token: 0x060027B4 RID: 10164
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsOverridable_Injected(ref LocalKeyword kw);

		// Token: 0x060027B5 RID: 10165
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderKeywordType GetKeywordType_Injected(ref LocalKeywordSpace spaceInfo, uint keyword);

		// Token: 0x060027B6 RID: 10166
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsValid_Injected(ref LocalKeywordSpace spaceInfo, uint keyword);

		// Token: 0x04000F0B RID: 3851
		internal readonly LocalKeywordSpace m_SpaceInfo;

		// Token: 0x04000F0C RID: 3852
		internal readonly string m_Name;

		// Token: 0x04000F0D RID: 3853
		internal readonly uint m_Index;
	}
}
