using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200013B RID: 315
	[NativeHeader("Runtime/Graphics/Billboard/BillboardAsset.h")]
	[NativeHeader("Runtime/Export/Graphics/BillboardRenderer.bindings.h")]
	public sealed class BillboardAsset : Object
	{
		// Token: 0x060008AF RID: 2223 RVA: 0x0000E1F0 File Offset: 0x0000C3F0
		public BillboardAsset()
		{
			BillboardAsset.Internal_Create(this);
		}

		// Token: 0x060008B0 RID: 2224
		[FreeFunction(Name = "BillboardRenderer_Bindings::Internal_Create")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] BillboardAsset obj);

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008B1 RID: 2225
		// (set) Token: 0x060008B2 RID: 2226
		public extern float width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008B3 RID: 2227
		// (set) Token: 0x060008B4 RID: 2228
		public extern float height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008B5 RID: 2229
		// (set) Token: 0x060008B6 RID: 2230
		public extern float bottom
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008B7 RID: 2231
		public extern int imageCount
		{
			[NativeMethod("GetNumImages")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008B8 RID: 2232
		public extern int vertexCount
		{
			[NativeMethod("GetNumVertices")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008B9 RID: 2233
		public extern int indexCount
		{
			[NativeMethod("GetNumIndices")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008BA RID: 2234
		// (set) Token: 0x060008BB RID: 2235
		public extern Material material
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0000E204 File Offset: 0x0000C404
		public void GetImageTexCoords(List<Vector4> imageTexCoords)
		{
			bool flag = imageTexCoords == null;
			if (flag)
			{
				throw new ArgumentNullException("imageTexCoords");
			}
			this.GetImageTexCoordsInternal(imageTexCoords);
		}

		// Token: 0x060008BD RID: 2237
		[NativeMethod("GetBillboardDataReadonly().GetImageTexCoords")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector4[] GetImageTexCoords();

		// Token: 0x060008BE RID: 2238
		[FreeFunction(Name = "BillboardRenderer_Bindings::GetImageTexCoordsInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetImageTexCoordsInternal(object list);

		// Token: 0x060008BF RID: 2239 RVA: 0x0000E230 File Offset: 0x0000C430
		public void SetImageTexCoords(List<Vector4> imageTexCoords)
		{
			bool flag = imageTexCoords == null;
			if (flag)
			{
				throw new ArgumentNullException("imageTexCoords");
			}
			this.SetImageTexCoordsInternalList(imageTexCoords);
		}

		// Token: 0x060008C0 RID: 2240
		[FreeFunction(Name = "BillboardRenderer_Bindings::SetImageTexCoords", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetImageTexCoords([NotNull("ArgumentNullException")] [Unmarshalled] Vector4[] imageTexCoords);

		// Token: 0x060008C1 RID: 2241
		[FreeFunction(Name = "BillboardRenderer_Bindings::SetImageTexCoordsInternalList", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetImageTexCoordsInternalList(object list);

		// Token: 0x060008C2 RID: 2242 RVA: 0x0000E25C File Offset: 0x0000C45C
		public void GetVertices(List<Vector2> vertices)
		{
			bool flag = vertices == null;
			if (flag)
			{
				throw new ArgumentNullException("vertices");
			}
			this.GetVerticesInternal(vertices);
		}

		// Token: 0x060008C3 RID: 2243
		[NativeMethod("GetBillboardDataReadonly().GetVertices")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector2[] GetVertices();

		// Token: 0x060008C4 RID: 2244
		[FreeFunction(Name = "BillboardRenderer_Bindings::GetVerticesInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetVerticesInternal(object list);

		// Token: 0x060008C5 RID: 2245 RVA: 0x0000E288 File Offset: 0x0000C488
		public void SetVertices(List<Vector2> vertices)
		{
			bool flag = vertices == null;
			if (flag)
			{
				throw new ArgumentNullException("vertices");
			}
			this.SetVerticesInternalList(vertices);
		}

		// Token: 0x060008C6 RID: 2246
		[FreeFunction(Name = "BillboardRenderer_Bindings::SetVertices", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetVertices([Unmarshalled] [NotNull("ArgumentNullException")] Vector2[] vertices);

		// Token: 0x060008C7 RID: 2247
		[FreeFunction(Name = "BillboardRenderer_Bindings::SetVerticesInternalList", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetVerticesInternalList(object list);

		// Token: 0x060008C8 RID: 2248 RVA: 0x0000E2B4 File Offset: 0x0000C4B4
		public void GetIndices(List<ushort> indices)
		{
			bool flag = indices == null;
			if (flag)
			{
				throw new ArgumentNullException("indices");
			}
			this.GetIndicesInternal(indices);
		}

		// Token: 0x060008C9 RID: 2249
		[NativeMethod("GetBillboardDataReadonly().GetIndices")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ushort[] GetIndices();

		// Token: 0x060008CA RID: 2250
		[FreeFunction(Name = "BillboardRenderer_Bindings::GetIndicesInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetIndicesInternal(object list);

		// Token: 0x060008CB RID: 2251 RVA: 0x0000E2E0 File Offset: 0x0000C4E0
		public void SetIndices(List<ushort> indices)
		{
			bool flag = indices == null;
			if (flag)
			{
				throw new ArgumentNullException("indices");
			}
			this.SetIndicesInternalList(indices);
		}

		// Token: 0x060008CC RID: 2252
		[FreeFunction(Name = "BillboardRenderer_Bindings::SetIndices", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetIndices([Unmarshalled] [NotNull("ArgumentNullException")] ushort[] indices);

		// Token: 0x060008CD RID: 2253
		[FreeFunction(Name = "BillboardRenderer_Bindings::SetIndicesInternalList", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetIndicesInternalList(object list);

		// Token: 0x060008CE RID: 2254
		[FreeFunction(Name = "BillboardRenderer_Bindings::MakeMaterialProperties", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MakeMaterialProperties(MaterialPropertyBlock properties, Camera camera);
	}
}
