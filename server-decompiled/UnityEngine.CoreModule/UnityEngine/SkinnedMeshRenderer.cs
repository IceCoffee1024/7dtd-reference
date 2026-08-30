using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001BE RID: 446
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Graphics/Mesh/SkinnedMeshRenderer.h")]
	public class SkinnedMeshRenderer : Renderer
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001016 RID: 4118
		// (set) Token: 0x06001017 RID: 4119
		public extern SkinQuality quality
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001018 RID: 4120
		// (set) Token: 0x06001019 RID: 4121
		public extern bool updateWhenOffscreen
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600101A RID: 4122
		// (set) Token: 0x0600101B RID: 4123
		public extern bool forceMatrixRecalculationPerRender
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600101C RID: 4124
		// (set) Token: 0x0600101D RID: 4125
		public extern Transform rootBone
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600101E RID: 4126
		// (set) Token: 0x0600101F RID: 4127
		public extern Transform[] bones
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001020 RID: 4128
		// (set) Token: 0x06001021 RID: 4129
		[NativeProperty("Mesh")]
		public extern Mesh sharedMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001022 RID: 4130
		// (set) Token: 0x06001023 RID: 4131
		[NativeProperty("SkinnedMeshMotionVectors")]
		public extern bool skinnedMotionVectors
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001024 RID: 4132
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetBlendShapeWeight(int index);

		// Token: 0x06001025 RID: 4133
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBlendShapeWeight(int index, float value);

		// Token: 0x06001026 RID: 4134 RVA: 0x00015D91 File Offset: 0x00013F91
		public void BakeMesh(Mesh mesh)
		{
			this.BakeMesh(mesh, false);
		}

		// Token: 0x06001027 RID: 4135
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BakeMesh([NotNull("NullExceptionObject")] Mesh mesh, bool useScale);

		// Token: 0x06001028 RID: 4136 RVA: 0x00015DA0 File Offset: 0x00013FA0
		public GraphicsBuffer GetVertexBuffer()
		{
			bool flag = this == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			return this.GetVertexBufferImpl();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00015DCC File Offset: 0x00013FCC
		public GraphicsBuffer GetPreviousVertexBuffer()
		{
			bool flag = this == null;
			if (flag)
			{
				throw new NullReferenceException();
			}
			return this.GetPreviousVertexBufferImpl();
		}

		// Token: 0x0600102A RID: 4138
		[FreeFunction(Name = "SkinnedMeshRendererScripting::GetVertexBufferPtr", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern GraphicsBuffer GetVertexBufferImpl();

		// Token: 0x0600102B RID: 4139
		[FreeFunction(Name = "SkinnedMeshRendererScripting::GetPreviousVertexBufferPtr", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern GraphicsBuffer GetPreviousVertexBufferImpl();

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x0600102C RID: 4140
		// (set) Token: 0x0600102D RID: 4141
		public extern GraphicsBuffer.Target vertexBufferTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
