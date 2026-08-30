using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001EE RID: 494
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Export/Math/Gradient.bindings.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class Gradient : IEquatable<Gradient>
	{
		// Token: 0x06001528 RID: 5416
		[FreeFunction(Name = "Gradient_Bindings::Init", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Init();

		// Token: 0x06001529 RID: 5417
		[FreeFunction(Name = "Gradient_Bindings::Cleanup", IsThreadSafe = true, HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		// Token: 0x0600152A RID: 5418
		[FreeFunction("Gradient_Bindings::Internal_Equals", IsThreadSafe = true, HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_Equals(IntPtr other);

		// Token: 0x0600152B RID: 5419 RVA: 0x0001FB4B File Offset: 0x0001DD4B
		[RequiredByNativeCode]
		public Gradient()
		{
			this.m_Ptr = Gradient.Init();
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0001FB60 File Offset: 0x0001DD60
		~Gradient()
		{
			this.Cleanup();
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0001FB90 File Offset: 0x0001DD90
		[FreeFunction(Name = "Gradient_Bindings::Evaluate", IsThreadSafe = true, HasExplicitThis = true)]
		public Color Evaluate(float time)
		{
			Color color;
			this.Evaluate_Injected(time, out color);
			return color;
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x0600152E RID: 5422
		// (set) Token: 0x0600152F RID: 5423
		public extern GradientColorKey[] colorKeys
		{
			[FreeFunction("Gradient_Bindings::GetColorKeys", IsThreadSafe = true, HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Gradient_Bindings::SetColorKeys", IsThreadSafe = true, HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[param: Unmarshalled]
			set;
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001530 RID: 5424
		// (set) Token: 0x06001531 RID: 5425
		public extern GradientAlphaKey[] alphaKeys
		{
			[FreeFunction("Gradient_Bindings::GetAlphaKeys", IsThreadSafe = true, HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Gradient_Bindings::SetAlphaKeys", IsThreadSafe = true, HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[param: Unmarshalled]
			set;
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001532 RID: 5426
		// (set) Token: 0x06001533 RID: 5427
		[NativeProperty(IsThreadSafe = true)]
		public extern GradientMode mode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001534 RID: 5428
		// (set) Token: 0x06001535 RID: 5429
		[NativeProperty(IsThreadSafe = true)]
		public extern ColorSpace colorSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001536 RID: 5430
		[FreeFunction(Name = "Gradient_Bindings::SetKeys", IsThreadSafe = true, HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetKeys([Unmarshalled] GradientColorKey[] colorKeys, [Unmarshalled] GradientAlphaKey[] alphaKeys);

		// Token: 0x06001537 RID: 5431 RVA: 0x0001FBA8 File Offset: 0x0001DDA8
		public override bool Equals(object o)
		{
			bool flag = o == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this == o;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = o.GetType() != base.GetType();
					flag2 = !flag4 && this.Equals((Gradient)o);
				}
			}
			return flag2;
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0001FBFC File Offset: 0x0001DDFC
		public bool Equals(Gradient other)
		{
			bool flag = other == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this == other;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = this.m_Ptr.Equals(other.m_Ptr);
					flag2 = flag4 || this.Internal_Equals(other.m_Ptr);
				}
			}
			return flag2;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0001FC54 File Offset: 0x0001DE54
		public override int GetHashCode()
		{
			return this.m_Ptr.GetHashCode();
		}

		// Token: 0x0600153A RID: 5434
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Evaluate_Injected(float time, out Color ret);

		// Token: 0x040007EC RID: 2028
		internal IntPtr m_Ptr;
	}
}
