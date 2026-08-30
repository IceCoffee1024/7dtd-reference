using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001DA RID: 474
	[NativeHeader("Runtime/Utilities/Hash128.h")]
	[NativeHeader("Runtime/Export/Hashing/Hash128.bindings.h")]
	[UsedByNativeCode]
	[Serializable]
	public struct Hash128 : IComparable, IComparable<Hash128>, IEquatable<Hash128>
	{
		// Token: 0x0600146E RID: 5230 RVA: 0x0001CD05 File Offset: 0x0001AF05
		public Hash128(uint u32_0, uint u32_1, uint u32_2, uint u32_3)
		{
			this.u64_0 = ((ulong)u32_1 << 32) | (ulong)u32_0;
			this.u64_1 = ((ulong)u32_3 << 32) | (ulong)u32_2;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0001CD25 File Offset: 0x0001AF25
		public Hash128(ulong u64_0, ulong u64_1)
		{
			this.u64_0 = u64_0;
			this.u64_1 = u64_1;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0001CD36 File Offset: 0x0001AF36
		public bool isValid
		{
			get
			{
				return this.u64_0 != 0UL || this.u64_1 > 0UL;
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0001CD50 File Offset: 0x0001AF50
		public int CompareTo(Hash128 rhs)
		{
			bool flag = this < rhs;
			int num;
			if (flag)
			{
				num = -1;
			}
			else
			{
				bool flag2 = this > rhs;
				if (flag2)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
			}
			return num;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0001CD8C File Offset: 0x0001AF8C
		public override string ToString()
		{
			return Hash128.Hash128ToStringImpl(this);
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0001CDAC File Offset: 0x0001AFAC
		[FreeFunction("StringToHash128", IsThreadSafe = true)]
		public static Hash128 Parse(string hashString)
		{
			Hash128 hash;
			Hash128.Parse_Injected(hashString, out hash);
			return hash;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0001CDC2 File Offset: 0x0001AFC2
		[FreeFunction("Hash128ToString", IsThreadSafe = true)]
		private static string Hash128ToStringImpl(Hash128 hash)
		{
			return Hash128.Hash128ToStringImpl_Injected(ref hash);
		}

		// Token: 0x06001475 RID: 5237
		[FreeFunction("ComputeHash128FromScriptString", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ComputeFromString(string data, ref Hash128 hash);

		// Token: 0x06001476 RID: 5238
		[FreeFunction("ComputeHash128FromScriptPointer", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ComputeFromPtr(IntPtr data, int start, int count, int elemSize, ref Hash128 hash);

		// Token: 0x06001477 RID: 5239
		[FreeFunction("ComputeHash128FromScriptArray", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ComputeFromArray(Array data, int start, int count, int elemSize, ref Hash128 hash);

		// Token: 0x06001478 RID: 5240 RVA: 0x0001CDCC File Offset: 0x0001AFCC
		public static Hash128 Compute(string data)
		{
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromString(data, ref hash);
			return hash;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0001CDF0 File Offset: 0x0001AFF0
		public static Hash128 Compute<T>(NativeArray<T> data) where T : struct
		{
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, data.Length, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0001CE2C File Offset: 0x0001B02C
		public static Hash128 Compute<T>(NativeArray<T> data, int start, int count) where T : struct
		{
			bool flag = start < 0 || count < 0 || start + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), start, count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0001CE98 File Offset: 0x0001B098
		public static Hash128 Compute<T>(T[] data) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Compute must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(data, 0, data.Length, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0001CEE8 File Offset: 0x0001B0E8
		public static Hash128 Compute<T>(T[] data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Compute must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Length;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(data, start, count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0001CF6C File Offset: 0x0001B16C
		public static Hash128 Compute<T>(List<T> data) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Compute", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), 0, data.Count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0001CFD4 File Offset: 0x0001B1D4
		public static Hash128 Compute<T>(List<T> data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Compute", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Count;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), start, count, UnsafeUtility.SizeOf<T>(), ref hash);
			return hash;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0001D06C File Offset: 0x0001B26C
		public unsafe static Hash128 Compute<[IsUnmanaged] T>(ref T val) where T : struct, ValueType
		{
			fixed (T* ptr = &val)
			{
				void* ptr2 = (void*)ptr;
				Hash128 hash = default(Hash128);
				Hash128.ComputeFromPtr((IntPtr)ptr2, 0, 1, UnsafeUtility.SizeOf<T>(), ref hash);
				return hash;
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0001D0A4 File Offset: 0x0001B2A4
		public static Hash128 Compute(int val)
		{
			Hash128 hash = default(Hash128);
			hash.Append(val);
			return hash;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0001D0C8 File Offset: 0x0001B2C8
		public static Hash128 Compute(float val)
		{
			Hash128 hash = default(Hash128);
			hash.Append(val);
			return hash;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0001D0EC File Offset: 0x0001B2EC
		public unsafe static Hash128 Compute(void* data, ulong size)
		{
			Hash128 hash = default(Hash128);
			Hash128.ComputeFromPtr(new IntPtr(data), 0, (int)size, 1, ref hash);
			return hash;
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0001D119 File Offset: 0x0001B319
		public void Append(string data)
		{
			Hash128.ComputeFromString(data, ref this);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0001D124 File Offset: 0x0001B324
		public void Append<T>(NativeArray<T> data) where T : struct
		{
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, data.Length, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0001D148 File Offset: 0x0001B348
		public void Append<T>(NativeArray<T> data, int start, int count) where T : struct
		{
			bool flag = start < 0 || count < 0 || start + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128.ComputeFromPtr((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), start, count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0001D1A8 File Offset: 0x0001B3A8
		public void Append<T>(T[] data) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Append must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			Hash128.ComputeFromArray(data, 0, data.Length, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0001D1EC File Offset: 0x0001B3EC
		public void Append<T>(T[] data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsArrayBlittable(data);
			if (flag)
			{
				throw new ArgumentException("Array passed to Append must be blittable.\n" + UnsafeUtility.GetReasonForArrayNonBlittable(data));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Length;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128.ComputeFromArray(data, start, count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0001D260 File Offset: 0x0001B460
		public void Append<T>(List<T> data) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Append", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), 0, data.Count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0001D2B8 File Offset: 0x0001B4B8
		public void Append<T>(List<T> data, int start, int count) where T : struct
		{
			bool flag = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to {1} must be blittable.\n{2}", typeof(T), "Append", UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag2 = start < 0 || count < 0 || start + count > data.Count;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad start/count arguments (start:{0} count:{1})", start, count));
			}
			Hash128.ComputeFromArray(NoAllocHelpers.ExtractArrayFromList(data), start, count, UnsafeUtility.SizeOf<T>(), ref this);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0001D340 File Offset: 0x0001B540
		public unsafe void Append<[IsUnmanaged] T>(ref T val) where T : struct, ValueType
		{
			fixed (T* ptr = &val)
			{
				void* ptr2 = (void*)ptr;
				Hash128.ComputeFromPtr((IntPtr)ptr2, 0, 1, UnsafeUtility.SizeOf<T>(), ref this);
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0001D36C File Offset: 0x0001B56C
		public void Append(int val)
		{
			this.ShortHash4((uint)val);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0001D377 File Offset: 0x0001B577
		public unsafe void Append(float val)
		{
			this.ShortHash4(*(uint*)(&val));
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x0001D385 File Offset: 0x0001B585
		public unsafe void Append(void* data, ulong size)
		{
			Hash128.ComputeFromPtr(new IntPtr(data), 0, (int)size, 1, ref this);
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x0001D39C File Offset: 0x0001B59C
		public override bool Equals(object obj)
		{
			return obj is Hash128 && this == (Hash128)obj;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x0001D3CC File Offset: 0x0001B5CC
		public bool Equals(Hash128 obj)
		{
			return this == obj;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0001D3EC File Offset: 0x0001B5EC
		public override int GetHashCode()
		{
			return this.u64_0.GetHashCode() ^ this.u64_1.GetHashCode();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0001D418 File Offset: 0x0001B618
		public int CompareTo(object obj)
		{
			bool flag = obj == null || !(obj is Hash128);
			int num;
			if (flag)
			{
				num = 1;
			}
			else
			{
				Hash128 hash = (Hash128)obj;
				num = this.CompareTo(hash);
			}
			return num;
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0001D454 File Offset: 0x0001B654
		public static bool operator ==(Hash128 hash1, Hash128 hash2)
		{
			return hash1.u64_0 == hash2.u64_0 && hash1.u64_1 == hash2.u64_1;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0001D488 File Offset: 0x0001B688
		public static bool operator !=(Hash128 hash1, Hash128 hash2)
		{
			return !(hash1 == hash2);
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
		public static bool operator <(Hash128 x, Hash128 y)
		{
			bool flag = x.u64_0 != y.u64_0;
			bool flag2;
			if (flag)
			{
				flag2 = x.u64_0 < y.u64_0;
			}
			else
			{
				flag2 = x.u64_1 < y.u64_1;
			}
			return flag2;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0001D4EC File Offset: 0x0001B6EC
		public static bool operator >(Hash128 x, Hash128 y)
		{
			bool flag = x < y;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = x == y;
				flag2 = !flag3;
			}
			return flag2;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0001D520 File Offset: 0x0001B720
		private void ShortHash4(uint data)
		{
			ulong num = this.u64_0;
			ulong num2 = this.u64_1;
			ulong num3 = 16045690984833335023UL;
			ulong num4 = 16045690984833335023UL;
			num4 += 288230376151711744UL;
			num3 += (ulong)data;
			Hash128.ShortEnd(ref num, ref num2, ref num3, ref num4);
			this.u64_0 = num;
			this.u64_1 = num2;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0001D580 File Offset: 0x0001B780
		private static void ShortEnd(ref ulong h0, ref ulong h1, ref ulong h2, ref ulong h3)
		{
			h3 ^= h2;
			Hash128.Rot64(ref h2, 15);
			h3 += h2;
			h0 ^= h3;
			Hash128.Rot64(ref h3, 52);
			h0 += h3;
			h1 ^= h0;
			Hash128.Rot64(ref h0, 26);
			h1 += h0;
			h2 ^= h1;
			Hash128.Rot64(ref h1, 51);
			h2 += h1;
			h3 ^= h2;
			Hash128.Rot64(ref h2, 28);
			h3 += h2;
			h0 ^= h3;
			Hash128.Rot64(ref h3, 9);
			h0 += h3;
			h1 ^= h0;
			Hash128.Rot64(ref h0, 47);
			h1 += h0;
			h2 ^= h1;
			Hash128.Rot64(ref h1, 54);
			h2 += h1;
			h3 ^= h2;
			Hash128.Rot64(ref h2, 32);
			h3 += h2;
			h0 ^= h3;
			Hash128.Rot64(ref h3, 25);
			h0 += h3;
			h1 ^= h0;
			Hash128.Rot64(ref h0, 63);
			h1 += h0;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0001D68B File Offset: 0x0001B88B
		private static void Rot64(ref ulong x, int k)
		{
			x = (x << k) | (x >> 64 - k);
		}

		// Token: 0x06001499 RID: 5273
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Parse_Injected(string hashString, out Hash128 ret);

		// Token: 0x0600149A RID: 5274
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Hash128ToStringImpl_Injected(ref Hash128 hash);

		// Token: 0x04000670 RID: 1648
		internal ulong u64_0;

		// Token: 0x04000671 RID: 1649
		internal ulong u64_1;

		// Token: 0x04000672 RID: 1650
		private const ulong kConst = 16045690984833335023UL;
	}
}
