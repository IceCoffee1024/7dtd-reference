using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000486 RID: 1158
	[UsedByNativeCode]
	public struct PlatformKeywordSet
	{
		// Token: 0x060027D0 RID: 10192 RVA: 0x0004461C File Offset: 0x0004281C
		private ulong ComputeKeywordMask(BuiltinShaderDefine define)
		{
			return (ulong)(1L << (int)((define % (BuiltinShaderDefine)64) & BuiltinShaderDefine.SHADER_API_GLES30));
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x00044638 File Offset: 0x00042838
		public bool IsEnabled(BuiltinShaderDefine define)
		{
			return (this.m_Bits & this.ComputeKeywordMask(define)) > 0UL;
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x0004465C File Offset: 0x0004285C
		public void Enable(BuiltinShaderDefine define)
		{
			this.m_Bits |= this.ComputeKeywordMask(define);
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x00044673 File Offset: 0x00042873
		public void Disable(BuiltinShaderDefine define)
		{
			this.m_Bits &= ~this.ComputeKeywordMask(define);
		}

		// Token: 0x04000F11 RID: 3857
		private const int k_SizeInBits = 64;

		// Token: 0x04000F12 RID: 3858
		internal ulong m_Bits;
	}
}
