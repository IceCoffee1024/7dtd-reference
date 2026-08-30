using System;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000157 RID: 343
	public struct RenderTargetSetup
	{
		// Token: 0x06000ADC RID: 2780 RVA: 0x00011884 File Offset: 0x0000FA84
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, RenderBufferLoadAction[] colorLoad, RenderBufferStoreAction[] colorStore, RenderBufferLoadAction depthLoad, RenderBufferStoreAction depthStore)
		{
			this.color = color;
			this.depth = depth;
			this.mipLevel = mip;
			this.cubemapFace = face;
			this.depthSlice = 0;
			this.colorLoad = colorLoad;
			this.colorStore = colorStore;
			this.depthLoad = depthLoad;
			this.depthStore = depthStore;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000118D8 File Offset: 0x0000FAD8
		internal static RenderBufferLoadAction[] LoadActions(RenderBuffer[] buf)
		{
			RenderBufferLoadAction[] array = new RenderBufferLoadAction[buf.Length];
			for (int i = 0; i < buf.Length; i++)
			{
				array[i] = buf[i].loadAction;
				buf[i].loadAction = RenderBufferLoadAction.Load;
			}
			return array;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00011928 File Offset: 0x0000FB28
		internal static RenderBufferStoreAction[] StoreActions(RenderBuffer[] buf)
		{
			RenderBufferStoreAction[] array = new RenderBufferStoreAction[buf.Length];
			for (int i = 0; i < buf.Length; i++)
			{
				array[i] = buf[i].storeAction;
				buf[i].storeAction = RenderBufferStoreAction.Store;
			}
			return array;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00011975 File Offset: 0x0000FB75
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0001198E File Offset: 0x0000FB8E
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth, mipLevel);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000119A8 File Offset: 0x0000FBA8
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth, mipLevel, face);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000119C4 File Offset: 0x0000FBC4
		public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face, int depthSlice)
		{
			this = new RenderTargetSetup(new RenderBuffer[] { color }, depth, mipLevel, face);
			this.depthSlice = depthSlice;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000119E8 File Offset: 0x0000FBE8
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth)
		{
			this = new RenderTargetSetup(color, depth, 0, CubemapFace.Unknown);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000119F6 File Offset: 0x0000FBF6
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mipLevel)
		{
			this = new RenderTargetSetup(color, depth, mipLevel, CubemapFace.Unknown);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00011A04 File Offset: 0x0000FC04
		public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face)
		{
			this = new RenderTargetSetup(color, depth, mip, face, RenderTargetSetup.LoadActions(color), RenderTargetSetup.StoreActions(color), depth.loadAction, depth.storeAction);
		}

		// Token: 0x0400044A RID: 1098
		public RenderBuffer[] color;

		// Token: 0x0400044B RID: 1099
		public RenderBuffer depth;

		// Token: 0x0400044C RID: 1100
		public int mipLevel;

		// Token: 0x0400044D RID: 1101
		public CubemapFace cubemapFace;

		// Token: 0x0400044E RID: 1102
		public int depthSlice;

		// Token: 0x0400044F RID: 1103
		public RenderBufferLoadAction[] colorLoad;

		// Token: 0x04000450 RID: 1104
		public RenderBufferStoreAction[] colorStore;

		// Token: 0x04000451 RID: 1105
		public RenderBufferLoadAction depthLoad;

		// Token: 0x04000452 RID: 1106
		public RenderBufferStoreAction depthStore;
	}
}
