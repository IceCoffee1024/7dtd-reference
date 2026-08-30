using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004ED RID: 1261
	public sealed class RayTracingAccelerationStructure : IDisposable
	{
		// Token: 0x06002BAC RID: 11180 RVA: 0x0004990C File Offset: 0x00047B0C
		~RayTracingAccelerationStructure()
		{
			this.Dispose(false);
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x00049940 File Offset: 0x00047B40
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x00049954 File Offset: 0x00047B54
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				RayTracingAccelerationStructure.Destroy(this);
			}
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x0004997B File Offset: 0x00047B7B
		public RayTracingAccelerationStructure(RayTracingAccelerationStructure.RASSettings settings)
		{
			this.m_Ptr = RayTracingAccelerationStructure.Create(settings);
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x00049994 File Offset: 0x00047B94
		public RayTracingAccelerationStructure()
		{
			this.m_Ptr = RayTracingAccelerationStructure.Create(new RayTracingAccelerationStructure.RASSettings
			{
				rayTracingModeMask = RayTracingAccelerationStructure.RayTracingModeMask.Everything,
				managementMode = RayTracingAccelerationStructure.ManagementMode.Manual,
				layerMask = -1
			});
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000499D6 File Offset: 0x00047BD6
		[FreeFunction("RayTracingAccelerationStructure_Bindings::Create")]
		private static IntPtr Create(RayTracingAccelerationStructure.RASSettings desc)
		{
			return RayTracingAccelerationStructure.Create_Injected(ref desc);
		}

		// Token: 0x06002BB2 RID: 11186
		[FreeFunction("RayTracingAccelerationStructure_Bindings::Destroy")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(RayTracingAccelerationStructure accelStruct);

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000499DF File Offset: 0x00047BDF
		public void Release()
		{
			this.Dispose();
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000499E9 File Offset: 0x00047BE9
		public void Build()
		{
			this.Build(Vector3.zero);
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000499F8 File Offset: 0x00047BF8
		public void AddInstance(Renderer targetRenderer, RayTracingSubMeshFlags[] subMeshFlags, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, uint id = 4294967295U)
		{
			this.AddInstanceSubMeshFlagsArray(targetRenderer, subMeshFlags, enableTriangleCulling, frontTriangleCounterClockwise, mask, id);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x00049A0C File Offset: 0x00047C0C
		public int AddInstance(GraphicsBuffer aabbBuffer, uint aabbCount, bool dynamicData, Matrix4x4 matrix, Material material, bool opaqueMaterial, MaterialPropertyBlock properties, uint mask = 255U, uint id = 4294967295U)
		{
			return this.AddInstance_Procedural(aabbBuffer, aabbCount, dynamicData, matrix, material, opaqueMaterial, properties, mask, id);
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x00049A33 File Offset: 0x00047C33
		public void RemoveInstance(Renderer targetRenderer)
		{
			this.RemoveInstance_Renderer(targetRenderer);
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x00049A3E File Offset: 0x00047C3E
		public void RemoveInstance(int handle)
		{
			this.RemoveInstance_InstanceID(handle);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x00049A49 File Offset: 0x00047C49
		public void UpdateInstanceTransform(Renderer renderer)
		{
			this.UpdateInstanceTransform_Renderer(renderer);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x00049A54 File Offset: 0x00047C54
		public void UpdateInstanceTransform(int handle, Matrix4x4 matrix)
		{
			this.UpdateInstanceTransform_InstanceID(handle, matrix);
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000499E9 File Offset: 0x00047BE9
		[Obsolete("Method Update has been deprecated. Use Build instead (UnityUpgradable) -> Build()", true)]
		public void Update()
		{
			this.Build(Vector3.zero);
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x00049A60 File Offset: 0x00047C60
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Update", HasExplicitThis = true)]
		[Obsolete("Method Update has been deprecated. Use Build instead (UnityUpgradable) -> Build(*)", true)]
		public void Update(Vector3 relativeOrigin)
		{
			this.Update_Injected(ref relativeOrigin);
		}

		// Token: 0x06002BBD RID: 11197
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceDeprecated", HasExplicitThis = true)]
		[Obsolete("This AddInstance method has been deprecated and will be removed in a future version. Please use the alternate method for adding Renderers to the acceleration structure.", false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddInstance([NotNull("ArgumentNullException")] Renderer targetRenderer, bool[] subMeshMask = null, bool[] subMeshTransparencyFlags = null, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, uint id = 4294967295U);

		// Token: 0x06002BBE RID: 11198 RVA: 0x00049A6C File Offset: 0x00047C6C
		[Obsolete("This AddInstance method has been deprecated and will be removed in a future version. Please use the alternate method for adding procedural geometry (AABBs) to the acceleration structure.", false)]
		public void AddInstance(GraphicsBuffer aabbBuffer, uint numElements, Material material, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U)
		{
			this.AddInstance_Procedural_Deprecated(aabbBuffer, numElements, material, Matrix4x4.identity, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x00049A98 File Offset: 0x00047C98
		[Obsolete("This AddInstance method has been deprecated and will be removed in a future version. Please use the alternate method for adding procedural geometry (AABBs) to the acceleration structure.", false)]
		public void AddInstance(GraphicsBuffer aabbBuffer, uint numElements, Material material, Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U)
		{
			this.AddInstance_Procedural_Deprecated(aabbBuffer, numElements, material, instanceTransform, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x00049ABE File Offset: 0x00047CBE
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Build", HasExplicitThis = true)]
		public void Build(Vector3 relativeOrigin)
		{
			this.Build_Injected(ref relativeOrigin);
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00049AC8 File Offset: 0x00047CC8
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceDeprecated", HasExplicitThis = true)]
		private void AddInstance_Procedural_Deprecated([NotNull("ArgumentNullException")] GraphicsBuffer aabbBuffer, uint numElements, [NotNull("ArgumentNullException")] Material material, Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U)
		{
			this.AddInstance_Procedural_Deprecated_Injected(aabbBuffer, numElements, material, ref instanceTransform, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00049AEC File Offset: 0x00047CEC
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstance", HasExplicitThis = true)]
		private int AddInstance_Procedural([NotNull("ArgumentNullException")] GraphicsBuffer aabbBuffer, uint aabbCount, bool dynamicData, Matrix4x4 matrix, [NotNull("ArgumentNullException")] Material material, bool opaqueMaterial, MaterialPropertyBlock properties, uint mask = 255U, uint id = 4294967295U)
		{
			return this.AddInstance_Procedural_Injected(aabbBuffer, aabbCount, dynamicData, ref matrix, material, opaqueMaterial, properties, mask, id);
		}

		// Token: 0x06002BC3 RID: 11203
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::RemoveInstance", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveInstance_Renderer([NotNull("ArgumentNullException")] Renderer targetRenderer);

		// Token: 0x06002BC4 RID: 11204
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::RemoveInstance", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveInstance_InstanceID(int instanceID);

		// Token: 0x06002BC5 RID: 11205
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceTransform", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UpdateInstanceTransform_Renderer([NotNull("ArgumentNullException")] Renderer renderer);

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00049B0E File Offset: 0x00047D0E
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceTransform", HasExplicitThis = true)]
		private void UpdateInstanceTransform_InstanceID(int instanceID, Matrix4x4 matrix)
		{
			this.UpdateInstanceTransform_InstanceID_Injected(instanceID, ref matrix);
		}

		// Token: 0x06002BC7 RID: 11207
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceMask", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateInstanceMask([NotNull("ArgumentNullException")] Renderer renderer, uint mask);

		// Token: 0x06002BC8 RID: 11208
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceID", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateInstanceID([NotNull("ArgumentNullException")] Renderer renderer, uint instanceID);

		// Token: 0x06002BC9 RID: 11209
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstancePropertyBlock", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateInstancePropertyBlock(int handle, MaterialPropertyBlock properties);

		// Token: 0x06002BCA RID: 11210
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetSize", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ulong GetSize();

		// Token: 0x06002BCB RID: 11211
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetInstanceCount", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetInstanceCount();

		// Token: 0x06002BCC RID: 11212
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::ClearInstances", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearInstances();

		// Token: 0x06002BCD RID: 11213 RVA: 0x00049B1C File Offset: 0x00047D1C
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::CullInstances", HasExplicitThis = true)]
		public RayTracingInstanceCullingResults CullInstances(ref RayTracingInstanceCullingConfig cullingConfig)
		{
			RayTracingInstanceCullingResults rayTracingInstanceCullingResults;
			this.CullInstances_Injected(ref cullingConfig, out rayTracingInstanceCullingResults);
			return rayTracingInstanceCullingResults;
		}

		// Token: 0x06002BCE RID: 11214
		[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceSubMeshFlagsArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddInstanceSubMeshFlagsArray([NotNull("ArgumentNullException")] Renderer targetRenderer, RayTracingSubMeshFlags[] subMeshFlags, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, uint id = 4294967295U);

		// Token: 0x06002BCF RID: 11215
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create_Injected(ref RayTracingAccelerationStructure.RASSettings desc);

		// Token: 0x06002BD0 RID: 11216
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Update_Injected(ref Vector3 relativeOrigin);

		// Token: 0x06002BD1 RID: 11217
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Build_Injected(ref Vector3 relativeOrigin);

		// Token: 0x06002BD2 RID: 11218
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddInstance_Procedural_Deprecated_Injected(GraphicsBuffer aabbBuffer, uint numElements, Material material, ref Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255U, bool reuseBounds = false, uint id = 4294967295U);

		// Token: 0x06002BD3 RID: 11219
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int AddInstance_Procedural_Injected(GraphicsBuffer aabbBuffer, uint aabbCount, bool dynamicData, ref Matrix4x4 matrix, Material material, bool opaqueMaterial, MaterialPropertyBlock properties, uint mask = 255U, uint id = 4294967295U);

		// Token: 0x06002BD4 RID: 11220
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UpdateInstanceTransform_InstanceID_Injected(int instanceID, ref Matrix4x4 matrix);

		// Token: 0x06002BD5 RID: 11221
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CullInstances_Injected(ref RayTracingInstanceCullingConfig cullingConfig, out RayTracingInstanceCullingResults ret);

		// Token: 0x0400111E RID: 4382
		internal IntPtr m_Ptr;

		// Token: 0x020004EE RID: 1262
		[Flags]
		public enum RayTracingModeMask
		{
			// Token: 0x04001120 RID: 4384
			Nothing = 0,
			// Token: 0x04001121 RID: 4385
			Static = 2,
			// Token: 0x04001122 RID: 4386
			DynamicTransform = 4,
			// Token: 0x04001123 RID: 4387
			DynamicGeometry = 8,
			// Token: 0x04001124 RID: 4388
			Everything = 14
		}

		// Token: 0x020004EF RID: 1263
		public enum ManagementMode
		{
			// Token: 0x04001126 RID: 4390
			Manual,
			// Token: 0x04001127 RID: 4391
			Automatic
		}

		// Token: 0x020004F0 RID: 1264
		public struct RASSettings
		{
			// Token: 0x06002BD6 RID: 11222 RVA: 0x00049B33 File Offset: 0x00047D33
			public RASSettings(RayTracingAccelerationStructure.ManagementMode sceneManagementMode, RayTracingAccelerationStructure.RayTracingModeMask rayTracingModeMask, int layerMask)
			{
				this.managementMode = sceneManagementMode;
				this.rayTracingModeMask = rayTracingModeMask;
				this.layerMask = layerMask;
			}

			// Token: 0x04001128 RID: 4392
			public RayTracingAccelerationStructure.ManagementMode managementMode;

			// Token: 0x04001129 RID: 4393
			public RayTracingAccelerationStructure.RayTracingModeMask rayTracingModeMask;

			// Token: 0x0400112A RID: 4394
			public int layerMask;
		}
	}
}
