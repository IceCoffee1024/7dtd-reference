using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000145 RID: 325
	[NativeType("Runtime/Graphics/RefreshRate.h")]
	public struct RefreshRate : IEquatable<RefreshRate>, IComparable<RefreshRate>
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0000EBA9 File Offset: 0x0000CDA9
		public double value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.numerator / this.denominator;
			}
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0000EBBC File Offset: 0x0000CDBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(RefreshRate other)
		{
			bool flag = this.denominator == 0U;
			bool flag2;
			if (flag)
			{
				flag2 = other.denominator == 0U;
			}
			else
			{
				bool flag3 = other.denominator == 0U;
				flag2 = !flag3 && (ulong)this.numerator * (ulong)other.denominator == (ulong)this.denominator * (ulong)other.numerator;
			}
			return flag2;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0000EC18 File Offset: 0x0000CE18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(RefreshRate other)
		{
			bool flag = this.denominator == 0U;
			int num;
			if (flag)
			{
				num = ((other.denominator == 0U) ? 0 : 1);
			}
			else
			{
				bool flag2 = other.denominator == 0U;
				if (flag2)
				{
					num = -1;
				}
				else
				{
					num = ((ulong)this.numerator * (ulong)other.denominator).CompareTo((ulong)this.denominator * (ulong)other.numerator);
				}
			}
			return num;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000EC80 File Offset: 0x0000CE80
		public override string ToString()
		{
			return this.value.ToString(CultureInfo.InvariantCulture.NumberFormat);
		}

		// Token: 0x04000416 RID: 1046
		[RequiredMember]
		public uint numerator;

		// Token: 0x04000417 RID: 1047
		[RequiredMember]
		public uint denominator;
	}
}
