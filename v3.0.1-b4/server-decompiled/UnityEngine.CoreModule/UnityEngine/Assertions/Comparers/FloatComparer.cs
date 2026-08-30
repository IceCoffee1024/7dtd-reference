using System;
using System.Collections.Generic;

namespace UnityEngine.Assertions.Comparers
{
	// Token: 0x020004F8 RID: 1272
	public class FloatComparer : IEqualityComparer<float>
	{
		// Token: 0x06002C64 RID: 11364 RVA: 0x0004A862 File Offset: 0x00048A62
		public FloatComparer()
			: this(1E-05f, false)
		{
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x0004A872 File Offset: 0x00048A72
		public FloatComparer(bool relative)
			: this(1E-05f, relative)
		{
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x0004A882 File Offset: 0x00048A82
		public FloatComparer(float error)
			: this(error, false)
		{
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x0004A88E File Offset: 0x00048A8E
		public FloatComparer(float error, bool relative)
		{
			this.m_Error = error;
			this.m_Relative = relative;
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x0004A8A8 File Offset: 0x00048AA8
		public bool Equals(float a, float b)
		{
			return this.m_Relative ? FloatComparer.AreEqualRelative(a, b, this.m_Error) : FloatComparer.AreEqual(a, b, this.m_Error);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x0004A8E0 File Offset: 0x00048AE0
		public int GetHashCode(float obj)
		{
			return base.GetHashCode();
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x0004A8F8 File Offset: 0x00048AF8
		public static bool AreEqual(float expected, float actual, float error)
		{
			return Math.Abs(actual - expected) <= error;
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x0004A918 File Offset: 0x00048B18
		public static bool AreEqualRelative(float expected, float actual, float error)
		{
			bool flag = expected == actual;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				float num = Math.Abs(expected);
				float num2 = Math.Abs(actual);
				float num3 = Math.Abs((actual - expected) / ((num > num2) ? num : num2));
				flag2 = num3 <= error;
			}
			return flag2;
		}

		// Token: 0x04001131 RID: 4401
		private readonly float m_Error;

		// Token: 0x04001132 RID: 4402
		private readonly bool m_Relative;

		// Token: 0x04001133 RID: 4403
		public static readonly FloatComparer s_ComparerWithDefaultTolerance = new FloatComparer(1E-05f);

		// Token: 0x04001134 RID: 4404
		public const float kEpsilon = 1E-05f;
	}
}
