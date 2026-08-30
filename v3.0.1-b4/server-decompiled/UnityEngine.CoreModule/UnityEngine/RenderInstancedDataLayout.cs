using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	// Token: 0x02000159 RID: 345
	internal readonly struct RenderInstancedDataLayout
	{
		// Token: 0x06000B01 RID: 2817 RVA: 0x00011BA0 File Offset: 0x0000FDA0
		public RenderInstancedDataLayout(Type t)
		{
			this.size = Marshal.SizeOf(t);
			this.offsetObjectToWorld = ((t == typeof(Matrix4x4)) ? 0 : Marshal.OffsetOf(t, "objectToWorld").ToInt32());
			try
			{
				this.offsetPrevObjectToWorld = Marshal.OffsetOf(t, "prevObjectToWorld").ToInt32();
			}
			catch (ArgumentException)
			{
				this.offsetPrevObjectToWorld = -1;
			}
			try
			{
				this.offsetRenderingLayerMask = Marshal.OffsetOf(t, "renderingLayerMask").ToInt32();
			}
			catch (ArgumentException)
			{
				this.offsetRenderingLayerMask = -1;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00011C58 File Offset: 0x0000FE58
		public int size { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00011C60 File Offset: 0x0000FE60
		public int offsetObjectToWorld { get; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00011C68 File Offset: 0x0000FE68
		public int offsetPrevObjectToWorld { get; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00011C70 File Offset: 0x0000FE70
		public int offsetRenderingLayerMask { get; }
	}
}
