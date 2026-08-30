using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x02000484 RID: 1156
	[NativeHeader("Runtime/Shaders/Keywords/KeywordSpaceScriptBindings.h")]
	public readonly struct LocalKeywordSpace : IEquatable<LocalKeywordSpace>
	{
		// Token: 0x060027B7 RID: 10167 RVA: 0x000443D7 File Offset: 0x000425D7
		[FreeFunction("keywords::GetKeywords", HasExplicitThis = true)]
		private LocalKeyword[] GetKeywords()
		{
			return LocalKeywordSpace.GetKeywords_Injected(ref this);
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x000443DF File Offset: 0x000425DF
		[FreeFunction("keywords::GetKeywordNames", HasExplicitThis = true)]
		private string[] GetKeywordNames()
		{
			return LocalKeywordSpace.GetKeywordNames_Injected(ref this);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000443E7 File Offset: 0x000425E7
		[FreeFunction("keywords::GetKeywordCount", HasExplicitThis = true)]
		private uint GetKeywordCount()
		{
			return LocalKeywordSpace.GetKeywordCount_Injected(ref this);
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000443F0 File Offset: 0x000425F0
		[FreeFunction("keywords::GetKeyword", HasExplicitThis = true)]
		private LocalKeyword GetKeyword(string name)
		{
			LocalKeyword localKeyword;
			LocalKeywordSpace.GetKeyword_Injected(ref this, name, out localKeyword);
			return localKeyword;
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x00044408 File Offset: 0x00042608
		public LocalKeyword[] keywords
		{
			get
			{
				return this.GetKeywords();
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x00044420 File Offset: 0x00042620
		public string[] keywordNames
		{
			get
			{
				return this.GetKeywordNames();
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060027BD RID: 10173 RVA: 0x00044438 File Offset: 0x00042638
		public uint keywordCount
		{
			get
			{
				return this.GetKeywordCount();
			}
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x00044450 File Offset: 0x00042650
		public LocalKeyword FindKeyword(string name)
		{
			return this.GetKeyword(name);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x0004446C File Offset: 0x0004266C
		public override bool Equals(object o)
		{
			bool flag;
			if (o is LocalKeywordSpace)
			{
				LocalKeywordSpace localKeywordSpace = (LocalKeywordSpace)o;
				flag = this.Equals(localKeywordSpace);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x00044498 File Offset: 0x00042698
		public bool Equals(LocalKeywordSpace rhs)
		{
			return this.m_KeywordSpace == rhs.m_KeywordSpace;
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000444BC File Offset: 0x000426BC
		public static bool operator ==(LocalKeywordSpace lhs, LocalKeywordSpace rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000444D8 File Offset: 0x000426D8
		public static bool operator !=(LocalKeywordSpace lhs, LocalKeywordSpace rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000444F4 File Offset: 0x000426F4
		public override int GetHashCode()
		{
			return this.m_KeywordSpace.GetHashCode();
		}

		// Token: 0x060027C4 RID: 10180
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LocalKeyword[] GetKeywords_Injected(ref LocalKeywordSpace _unity_self);

		// Token: 0x060027C5 RID: 10181
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetKeywordNames_Injected(ref LocalKeywordSpace _unity_self);

		// Token: 0x060027C6 RID: 10182
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetKeywordCount_Injected(ref LocalKeywordSpace _unity_self);

		// Token: 0x060027C7 RID: 10183
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetKeyword_Injected(ref LocalKeywordSpace _unity_self, string name, out LocalKeyword ret);

		// Token: 0x04000F0E RID: 3854
		private readonly IntPtr m_KeywordSpace;
	}
}
