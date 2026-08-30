using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000134 RID: 308
	[NativeHeader("Modules/IMGUI/GUIStyle.h")]
	[UsedByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class RectOffset : IFormattable
	{
		// Token: 0x06000839 RID: 2105 RVA: 0x0000D965 File Offset: 0x0000BB65
		public RectOffset()
		{
			this.m_Ptr = RectOffset.InternalCreate();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0000D97A File Offset: 0x0000BB7A
		[VisibleToOtherModules(new string[] { "UnityEngine.IMGUIModule" })]
		internal RectOffset(object sourceStyle, IntPtr source)
		{
			this.m_SourceStyle = sourceStyle;
			this.m_Ptr = source;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0000D994 File Offset: 0x0000BB94
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_SourceStyle == null;
				if (flag)
				{
					this.Destroy();
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		public RectOffset(int left, int right, int top, int bottom)
		{
			this.m_Ptr = RectOffset.InternalCreate();
			this.left = left;
			this.right = right;
			this.top = top;
			this.bottom = bottom;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0000DA0C File Offset: 0x0000BC0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0000DA28 File Offset: 0x0000BC28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0000DA44 File Offset: 0x0000BC44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = formatProvider == null;
			if (flag)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("RectOffset (l:{0} r:{1} t:{2} b:{3})", new object[]
			{
				this.left.ToString(format, formatProvider),
				this.right.ToString(format, formatProvider),
				this.top.ToString(format, formatProvider),
				this.bottom.ToString(format, formatProvider)
			});
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0000DAC8 File Offset: 0x0000BCC8
		private void Destroy()
		{
			bool flag = this.m_Ptr != IntPtr.Zero;
			if (flag)
			{
				RectOffset.InternalDestroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
		}

		// Token: 0x06000841 RID: 2113
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InternalCreate();

		// Token: 0x06000842 RID: 2114
		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalDestroy(IntPtr ptr);

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000843 RID: 2115
		// (set) Token: 0x06000844 RID: 2116
		[NativeProperty("left", false, TargetType.Field)]
		public extern int left
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000845 RID: 2117
		// (set) Token: 0x06000846 RID: 2118
		[NativeProperty("right", false, TargetType.Field)]
		public extern int right
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000847 RID: 2119
		// (set) Token: 0x06000848 RID: 2120
		[NativeProperty("top", false, TargetType.Field)]
		public extern int top
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000849 RID: 2121
		// (set) Token: 0x0600084A RID: 2122
		[NativeProperty("bottom", false, TargetType.Field)]
		public extern int bottom
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600084B RID: 2123
		public extern int horizontal
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600084C RID: 2124
		public extern int vertical
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0000DB04 File Offset: 0x0000BD04
		public Rect Add(Rect rect)
		{
			Rect rect2;
			this.Add_Injected(ref rect, out rect2);
			return rect2;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		public Rect Remove(Rect rect)
		{
			Rect rect2;
			this.Remove_Injected(ref rect, out rect2);
			return rect2;
		}

		// Token: 0x0600084F RID: 2127
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Add_Injected(ref Rect rect, out Rect ret);

		// Token: 0x06000850 RID: 2128
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Remove_Injected(ref Rect rect, out Rect ret);

		// Token: 0x040003F9 RID: 1017
		[VisibleToOtherModules(new string[] { "UnityEngine.IMGUIModule" })]
		[NonSerialized]
		internal IntPtr m_Ptr;

		// Token: 0x040003FA RID: 1018
		private readonly object m_SourceStyle;
	}
}
