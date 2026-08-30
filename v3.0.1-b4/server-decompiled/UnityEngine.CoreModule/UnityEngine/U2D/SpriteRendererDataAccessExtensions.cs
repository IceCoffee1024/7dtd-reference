using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.U2D
{
	// Token: 0x020002B6 RID: 694
	[NativeHeader("Runtime/2D/Common/SpriteDataAccess.h")]
	[NativeHeader("Runtime/Graphics/Mesh/SpriteRenderer.h")]
	public static class SpriteRendererDataAccessExtensions
	{
		// Token: 0x06001D97 RID: 7575 RVA: 0x00030C04 File Offset: 0x0002EE04
		internal static void SetDeformableBuffer(this SpriteRenderer spriteRenderer, NativeArray<byte> src)
		{
			bool flag = spriteRenderer.sprite == null;
			if (flag)
			{
				throw new ArgumentException(string.Format("spriteRenderer does not have a valid sprite set.", Array.Empty<object>()));
			}
			bool flag2 = src.Length != SpriteDataAccessExtensions.GetPrimaryVertexStreamSize(spriteRenderer.sprite);
			if (flag2)
			{
				throw new InvalidOperationException(string.Format("custom sprite vertex data size must match sprite asset's vertex data size {0} {1}", src.Length, SpriteDataAccessExtensions.GetPrimaryVertexStreamSize(spriteRenderer.sprite)));
			}
			SpriteRendererDataAccessExtensions.SetDeformableBuffer(spriteRenderer, src.GetUnsafeReadOnlyPtr<byte>(), src.Length);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x00030C94 File Offset: 0x0002EE94
		internal static void SetDeformableBuffer(this SpriteRenderer spriteRenderer, NativeArray<Vector3> src)
		{
			bool flag = spriteRenderer.sprite == null;
			if (flag)
			{
				throw new InvalidOperationException("spriteRenderer does not have a valid sprite set.");
			}
			bool flag2 = src.Length != spriteRenderer.sprite.GetVertexCount();
			if (flag2)
			{
				throw new InvalidOperationException(string.Format("The src length {0} must match the vertex count of source Sprite {1}.", src.Length, spriteRenderer.sprite.GetVertexCount()));
			}
			SpriteRendererDataAccessExtensions.SetDeformableBuffer(spriteRenderer, src.GetUnsafeReadOnlyPtr<Vector3>(), src.Length);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x00030D18 File Offset: 0x0002EF18
		internal static void SetBatchDeformableBufferAndLocalAABBArray(SpriteRenderer[] spriteRenderers, NativeArray<IntPtr> buffers, NativeArray<int> bufferSizes, NativeArray<Bounds> bounds)
		{
			int num = spriteRenderers.Length;
			bool flag = num != buffers.Length || num != bufferSizes.Length || num != bounds.Length;
			if (flag)
			{
				throw new ArgumentException("Input array sizes are not the same.");
			}
			SpriteRendererDataAccessExtensions.SetBatchDeformableBufferAndLocalAABBArray(spriteRenderers, buffers.GetUnsafeReadOnlyPtr<IntPtr>(), bufferSizes.GetUnsafeReadOnlyPtr<int>(), bounds.GetUnsafeReadOnlyPtr<Bounds>(), num);
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00030D78 File Offset: 0x0002EF78
		internal unsafe static bool IsUsingDeformableBuffer(this SpriteRenderer spriteRenderer, IntPtr buffer)
		{
			return SpriteRendererDataAccessExtensions.IsUsingDeformableBuffer(spriteRenderer, (void*)buffer);
		}

		// Token: 0x06001D9B RID: 7579
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeactivateDeformableBuffer([NotNull("ArgumentNullException")] this SpriteRenderer renderer);

		// Token: 0x06001D9C RID: 7580 RVA: 0x00030D96 File Offset: 0x0002EF96
		internal static void SetLocalAABB([NotNull("ArgumentNullException")] this SpriteRenderer renderer, Bounds aabb)
		{
			SpriteRendererDataAccessExtensions.SetLocalAABB_Injected(renderer, ref aabb);
		}

		// Token: 0x06001D9D RID: 7581
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetDeformableBuffer([NotNull("ArgumentNullException")] SpriteRenderer spriteRenderer, void* src, int count);

		// Token: 0x06001D9E RID: 7582
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void SetBatchDeformableBufferAndLocalAABBArray(SpriteRenderer[] spriteRenderers, void* buffers, void* bufferSizes, void* bounds, int count);

		// Token: 0x06001D9F RID: 7583
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool IsUsingDeformableBuffer([NotNull("ArgumentNullException")] SpriteRenderer spriteRenderer, void* buffer);

		// Token: 0x06001DA0 RID: 7584
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalAABB_Injected(SpriteRenderer renderer, ref Bounds aabb);
	}
}
