using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000448 RID: 1096
	[NativeHeader("Runtime/Math/Matrix4x4.h")]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class BatchRendererGroup : IDisposable
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x0003D3A4 File Offset: 0x0003B5A4
		public unsafe BatchRendererGroup(BatchRendererGroup.OnPerformCulling cullingCallback, IntPtr userContext)
		{
			this.m_PerformCulling = cullingCallback;
			this.m_GroupHandle = BatchRendererGroup.Create(this, (void*)userContext);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0003D3D2 File Offset: 0x0003B5D2
		public void Dispose()
		{
			BatchRendererGroup.Destroy(this.m_GroupHandle);
			this.m_GroupHandle = IntPtr.Zero;
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0003D3EC File Offset: 0x0003B5EC
		public ThreadedBatchContext GetThreadedBatchContext()
		{
			return new ThreadedBatchContext
			{
				batchRendererGroup = this.m_GroupHandle
			};
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0003D414 File Offset: 0x0003B614
		private BatchID AddDrawCommandBatch(IntPtr values, int count, GraphicsBufferHandle buffer, uint bufferOffset, uint windowSize)
		{
			BatchID batchID;
			this.AddDrawCommandBatch_Injected(values, count, ref buffer, bufferOffset, windowSize, out batchID);
			return batchID;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x0003D434 File Offset: 0x0003B634
		public BatchID AddBatch(NativeArray<MetadataValue> batchMetadata, GraphicsBufferHandle buffer)
		{
			return this.AddDrawCommandBatch((IntPtr)batchMetadata.GetUnsafeReadOnlyPtr<MetadataValue>(), batchMetadata.Length, buffer, 0U, 0U);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0003D464 File Offset: 0x0003B664
		public BatchID AddBatch(NativeArray<MetadataValue> batchMetadata, GraphicsBufferHandle buffer, uint bufferOffset, uint windowSize)
		{
			return this.AddDrawCommandBatch((IntPtr)batchMetadata.GetUnsafeReadOnlyPtr<MetadataValue>(), batchMetadata.Length, buffer, bufferOffset, windowSize);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0003D492 File Offset: 0x0003B692
		private void RemoveDrawCommandBatch(BatchID batchID)
		{
			this.RemoveDrawCommandBatch_Injected(ref batchID);
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0003D49C File Offset: 0x0003B69C
		public void RemoveBatch(BatchID batchID)
		{
			this.RemoveDrawCommandBatch(batchID);
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0003D4A7 File Offset: 0x0003B6A7
		private void SetDrawCommandBatchBuffer(BatchID batchID, GraphicsBufferHandle buffer)
		{
			this.SetDrawCommandBatchBuffer_Injected(ref batchID, ref buffer);
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0003D4B3 File Offset: 0x0003B6B3
		public void SetBatchBuffer(BatchID batchID, GraphicsBufferHandle buffer)
		{
			this.SetDrawCommandBatchBuffer(batchID, buffer);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0003D4C0 File Offset: 0x0003B6C0
		public BatchMaterialID RegisterMaterial(Material material)
		{
			BatchMaterialID batchMaterialID;
			this.RegisterMaterial_Injected(material, out batchMaterialID);
			return batchMaterialID;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0003D4D7 File Offset: 0x0003B6D7
		public BatchMaterialID RegisterMaterial(int materialInstanceID)
		{
			return this.RegisterMaterial_InstanceID(materialInstanceID);
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0003D4E0 File Offset: 0x0003B6E0
		private BatchMaterialID RegisterMaterial_InstanceID(int materialInstanceID)
		{
			BatchMaterialID batchMaterialID;
			this.RegisterMaterial_InstanceID_Injected(materialInstanceID, out batchMaterialID);
			return batchMaterialID;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x0003D4F7 File Offset: 0x0003B6F7
		public void UnregisterMaterial(BatchMaterialID material)
		{
			this.UnregisterMaterial_Injected(ref material);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0003D501 File Offset: 0x0003B701
		public Material GetRegisteredMaterial(BatchMaterialID material)
		{
			return this.GetRegisteredMaterial_Injected(ref material);
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0003D50C File Offset: 0x0003B70C
		public BatchMeshID RegisterMesh(Mesh mesh)
		{
			BatchMeshID batchMeshID;
			this.RegisterMesh_Injected(mesh, out batchMeshID);
			return batchMeshID;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0003D523 File Offset: 0x0003B723
		public BatchMeshID RegisterMesh(int meshInstanceID)
		{
			return this.RegisterMesh_InstanceID(meshInstanceID);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0003D52C File Offset: 0x0003B72C
		private BatchMeshID RegisterMesh_InstanceID(int meshInstanceID)
		{
			BatchMeshID batchMeshID;
			this.RegisterMesh_InstanceID_Injected(meshInstanceID, out batchMeshID);
			return batchMeshID;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0003D543 File Offset: 0x0003B743
		public void UnregisterMesh(BatchMeshID mesh)
		{
			this.UnregisterMesh_Injected(ref mesh);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0003D54D File Offset: 0x0003B74D
		public Mesh GetRegisteredMesh(BatchMeshID mesh)
		{
			return this.GetRegisteredMesh_Injected(ref mesh);
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0003D557 File Offset: 0x0003B757
		public void SetGlobalBounds(Bounds bounds)
		{
			this.SetGlobalBounds_Injected(ref bounds);
		}

		// Token: 0x06002488 RID: 9352
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPickingMaterial(Material material);

		// Token: 0x06002489 RID: 9353
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetErrorMaterial(Material material);

		// Token: 0x0600248A RID: 9354
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetLoadingMaterial(Material material);

		// Token: 0x0600248B RID: 9355
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetEnabledViewTypes(BatchCullingViewType[] viewTypes);

		// Token: 0x0600248C RID: 9356
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern BatchBufferTarget GetBufferTarget();

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x0003D561 File Offset: 0x0003B761
		public static BatchBufferTarget BufferTarget
		{
			get
			{
				return BatchRendererGroup.GetBufferTarget();
			}
		}

		// Token: 0x0600248E RID: 9358
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetConstantBufferMaxWindowSize();

		// Token: 0x0600248F RID: 9359
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetConstantBufferOffsetAlignment();

		// Token: 0x06002490 RID: 9360
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr Create(BatchRendererGroup group, void* userContext);

		// Token: 0x06002491 RID: 9361
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(IntPtr groupHandle);

		// Token: 0x06002492 RID: 9362 RVA: 0x0003D568 File Offset: 0x0003B768
		[RequiredByNativeCode]
		private unsafe static void InvokeOnPerformCulling(BatchRendererGroup group, ref BatchRendererCullingOutput context, ref LODParameters lodParameters, IntPtr userContext)
		{
			NativeArray<Plane> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Plane>((void*)context.cullingPlanes, context.cullingPlaneCount, Allocator.Invalid);
			NativeArray<CullingSplit> nativeArray2 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<CullingSplit>((void*)context.cullingSplits, context.cullingSplitCount, Allocator.Invalid);
			NativeArray<BatchCullingOutputDrawCommands> nativeArray3 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<BatchCullingOutputDrawCommands>((void*)context.drawCommands, 1, Allocator.Invalid);
			try
			{
				BatchCullingOutput batchCullingOutput = new BatchCullingOutput
				{
					drawCommands = nativeArray3
				};
				context.cullingJobsFence = group.m_PerformCulling(group, new BatchCullingContext(nativeArray, nativeArray2, lodParameters, context.localToWorldMatrix, context.viewType, context.projectionType, context.cullingFlags, context.viewID, context.cullingLayerMask, context.sceneCullingMask, context.receiverPlaneOffset, context.receiverPlaneCount), batchCullingOutput, userContext);
			}
			finally
			{
				JobHandle.ScheduleBatchedJobs();
			}
		}

		// Token: 0x06002493 RID: 9363
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddDrawCommandBatch_Injected(IntPtr values, int count, ref GraphicsBufferHandle buffer, uint bufferOffset, uint windowSize, out BatchID ret);

		// Token: 0x06002494 RID: 9364
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveDrawCommandBatch_Injected(ref BatchID batchID);

		// Token: 0x06002495 RID: 9365
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetDrawCommandBatchBuffer_Injected(ref BatchID batchID, ref GraphicsBufferHandle buffer);

		// Token: 0x06002496 RID: 9366
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RegisterMaterial_Injected(Material material, out BatchMaterialID ret);

		// Token: 0x06002497 RID: 9367
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RegisterMaterial_InstanceID_Injected(int materialInstanceID, out BatchMaterialID ret);

		// Token: 0x06002498 RID: 9368
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UnregisterMaterial_Injected(ref BatchMaterialID material);

		// Token: 0x06002499 RID: 9369
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material GetRegisteredMaterial_Injected(ref BatchMaterialID material);

		// Token: 0x0600249A RID: 9370
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RegisterMesh_Injected(Mesh mesh, out BatchMeshID ret);

		// Token: 0x0600249B RID: 9371
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RegisterMesh_InstanceID_Injected(int meshInstanceID, out BatchMeshID ret);

		// Token: 0x0600249C RID: 9372
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UnregisterMesh_Injected(ref BatchMeshID mesh);

		// Token: 0x0600249D RID: 9373
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Mesh GetRegisteredMesh_Injected(ref BatchMeshID mesh);

		// Token: 0x0600249E RID: 9374
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalBounds_Injected(ref Bounds bounds);

		// Token: 0x04000DA3 RID: 3491
		private IntPtr m_GroupHandle = IntPtr.Zero;

		// Token: 0x04000DA4 RID: 3492
		private BatchRendererGroup.OnPerformCulling m_PerformCulling;

		// Token: 0x02000449 RID: 1097
		// (Invoke) Token: 0x060024A0 RID: 9376
		public delegate JobHandle OnPerformCulling(BatchRendererGroup rendererGroup, BatchCullingContext cullingContext, BatchCullingOutput cullingOutput, IntPtr userContext);
	}
}
