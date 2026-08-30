using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x02000447 RID: 1095
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	public struct ThreadedBatchContext
	{
		// Token: 0x06002469 RID: 9321 RVA: 0x0003D2E8 File Offset: 0x0003B4E8
		[FreeFunction("BatchRendererGroup::AddDrawCommandBatch_Threaded", IsThreadSafe = true)]
		private static BatchID AddDrawCommandBatch(IntPtr brg, IntPtr values, int count, GraphicsBufferHandle buffer, uint bufferOffset, uint windowSize)
		{
			BatchID batchID;
			ThreadedBatchContext.AddDrawCommandBatch_Injected(brg, values, count, ref buffer, bufferOffset, windowSize, out batchID);
			return batchID;
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0003D306 File Offset: 0x0003B506
		[FreeFunction("BatchRendererGroup::SetDrawCommandBatchBuffer_Threaded", IsThreadSafe = true)]
		private static void SetDrawCommandBatchBuffer(IntPtr brg, BatchID batchID, GraphicsBufferHandle buffer)
		{
			ThreadedBatchContext.SetDrawCommandBatchBuffer_Injected(brg, ref batchID, ref buffer);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0003D312 File Offset: 0x0003B512
		[FreeFunction("BatchRendererGroup::RemoveDrawCommandBatch_Threaded", IsThreadSafe = true)]
		private static void RemoveDrawCommandBatch(IntPtr brg, BatchID batchID)
		{
			ThreadedBatchContext.RemoveDrawCommandBatch_Injected(brg, ref batchID);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0003D31C File Offset: 0x0003B51C
		public BatchID AddBatch(NativeArray<MetadataValue> batchMetadata, GraphicsBufferHandle buffer)
		{
			return ThreadedBatchContext.AddDrawCommandBatch(this.batchRendererGroup, (IntPtr)batchMetadata.GetUnsafeReadOnlyPtr<MetadataValue>(), batchMetadata.Length, buffer, 0U, 0U);
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0003D350 File Offset: 0x0003B550
		public BatchID AddBatch(NativeArray<MetadataValue> batchMetadata, GraphicsBufferHandle buffer, uint bufferOffset, uint windowSize)
		{
			return ThreadedBatchContext.AddDrawCommandBatch(this.batchRendererGroup, (IntPtr)batchMetadata.GetUnsafeReadOnlyPtr<MetadataValue>(), batchMetadata.Length, buffer, bufferOffset, windowSize);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0003D383 File Offset: 0x0003B583
		public void SetBatchBuffer(BatchID batchID, GraphicsBufferHandle buffer)
		{
			ThreadedBatchContext.SetDrawCommandBatchBuffer(this.batchRendererGroup, batchID, buffer);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0003D394 File Offset: 0x0003B594
		public void RemoveBatch(BatchID batchID)
		{
			ThreadedBatchContext.RemoveDrawCommandBatch(this.batchRendererGroup, batchID);
		}

		// Token: 0x06002470 RID: 9328
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddDrawCommandBatch_Injected(IntPtr brg, IntPtr values, int count, ref GraphicsBufferHandle buffer, uint bufferOffset, uint windowSize, out BatchID ret);

		// Token: 0x06002471 RID: 9329
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetDrawCommandBatchBuffer_Injected(IntPtr brg, ref BatchID batchID, ref GraphicsBufferHandle buffer);

		// Token: 0x06002472 RID: 9330
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveDrawCommandBatch_Injected(IntPtr brg, ref BatchID batchID);

		// Token: 0x04000DA2 RID: 3490
		public IntPtr batchRendererGroup;
	}
}
