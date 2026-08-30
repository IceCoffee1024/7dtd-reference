using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.U2D
{
	// Token: 0x020004B8 RID: 1208
	[RequiredByNativeCode]
	[NativeHeader("Runtime/2D/Renderer/SpriteRendererGroup.h")]
	internal struct SpriteIntermediateRendererInfo
	{
		// Token: 0x04000F9F RID: 3999
		public int SpriteID;

		// Token: 0x04000FA0 RID: 4000
		public int TextureID;

		// Token: 0x04000FA1 RID: 4001
		public int MaterialID;

		// Token: 0x04000FA2 RID: 4002
		public Color Color;

		// Token: 0x04000FA3 RID: 4003
		public Matrix4x4 Transform;

		// Token: 0x04000FA4 RID: 4004
		public Bounds Bounds;

		// Token: 0x04000FA5 RID: 4005
		public int Layer;

		// Token: 0x04000FA6 RID: 4006
		public int SortingLayer;

		// Token: 0x04000FA7 RID: 4007
		public int SortingOrder;

		// Token: 0x04000FA8 RID: 4008
		public ulong SceneCullingMask;

		// Token: 0x04000FA9 RID: 4009
		public IntPtr IndexData;

		// Token: 0x04000FAA RID: 4010
		public IntPtr VertexData;

		// Token: 0x04000FAB RID: 4011
		public int IndexCount;

		// Token: 0x04000FAC RID: 4012
		public int VertexCount;

		// Token: 0x04000FAD RID: 4013
		public int ShaderChannelMask;
	}
}
