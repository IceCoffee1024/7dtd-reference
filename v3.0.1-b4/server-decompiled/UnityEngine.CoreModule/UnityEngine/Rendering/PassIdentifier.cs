using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000485 RID: 1157
	[NativeHeader("Runtime/Shaders/PassIdentifier.h")]
	[UsedByNativeCode]
	public readonly struct PassIdentifier : IEquatable<PassIdentifier>
	{
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060027C8 RID: 10184 RVA: 0x00044514 File Offset: 0x00042714
		public uint SubshaderIndex
		{
			get
			{
				return this.m_SubShaderIndex;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060027C9 RID: 10185 RVA: 0x0004452C File Offset: 0x0004272C
		public uint PassIndex
		{
			get
			{
				return this.m_PassIndex;
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00044544 File Offset: 0x00042744
		public PassIdentifier(uint subshaderIndex, uint passIndex)
		{
			this.m_SubShaderIndex = subshaderIndex;
			this.m_PassIndex = passIndex;
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00044558 File Offset: 0x00042758
		public override bool Equals(object o)
		{
			bool flag;
			if (o is PassIdentifier)
			{
				PassIdentifier passIdentifier = (PassIdentifier)o;
				flag = this.Equals(passIdentifier);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00044584 File Offset: 0x00042784
		public bool Equals(PassIdentifier rhs)
		{
			return this.m_SubShaderIndex == rhs.m_SubShaderIndex && this.m_PassIndex == rhs.m_PassIndex;
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000445B8 File Offset: 0x000427B8
		public static bool operator ==(PassIdentifier lhs, PassIdentifier rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000445D4 File Offset: 0x000427D4
		public static bool operator !=(PassIdentifier lhs, PassIdentifier rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000445F0 File Offset: 0x000427F0
		public override int GetHashCode()
		{
			return this.m_SubShaderIndex.GetHashCode() ^ this.m_PassIndex.GetHashCode();
		}

		// Token: 0x04000F0F RID: 3855
		internal readonly uint m_SubShaderIndex;

		// Token: 0x04000F10 RID: 3856
		internal readonly uint m_PassIndex;
	}
}
