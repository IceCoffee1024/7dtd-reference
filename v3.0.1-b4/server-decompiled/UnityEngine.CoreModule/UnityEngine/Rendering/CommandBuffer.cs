using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000430 RID: 1072
	[NativeHeader("Runtime/Shaders/ComputeShader.h")]
	[NativeHeader("Runtime/Export/Graphics/RenderingCommandBuffer.bindings.h")]
	[NativeType("Runtime/Graphics/CommandBuffer/RenderingCommandBuffer.h")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/Shaders/RayTracingShader.h")]
	public class CommandBuffer : IDisposable
	{
		// Token: 0x06002236 RID: 8758 RVA: 0x00038D91 File Offset: 0x00036F91
		public void ConvertTexture(RenderTargetIdentifier src, RenderTargetIdentifier dst)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.ConvertTexture_Internal(src, 0, dst, 0);
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00038DA8 File Offset: 0x00036FA8
		public void ConvertTexture(RenderTargetIdentifier src, int srcElement, RenderTargetIdentifier dst, int dstElement)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.ConvertTexture_Internal(src, srcElement, dst, dstElement);
		}

		// Token: 0x06002238 RID: 8760
		[NativeMethod("AddWaitAllAsyncReadbackRequests")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WaitAllAsyncReadbackRequests();

		// Token: 0x06002239 RID: 8761 RVA: 0x00038DC0 File Offset: 0x00036FC0
		public void RequestAsyncReadback(ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_1(src, callback, null);
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x00038DD9 File Offset: 0x00036FD9
		public void RequestAsyncReadback(GraphicsBuffer src, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_8(src, callback, null);
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x00038DF2 File Offset: 0x00036FF2
		public void RequestAsyncReadback(ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_2(src, size, offset, callback, null);
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x00038E0E File Offset: 0x0003700E
		public void RequestAsyncReadback(GraphicsBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_9(src, size, offset, callback, null);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00038E2A File Offset: 0x0003702A
		public void RequestAsyncReadback(Texture src, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_3(src, callback, null);
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00038E43 File Offset: 0x00037043
		public void RequestAsyncReadback(Texture src, int mipIndex, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_4(src, mipIndex, callback, null);
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x00038E5D File Offset: 0x0003705D
		public void RequestAsyncReadback(Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_5(src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback, null);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x00038E86 File Offset: 0x00037086
		public void RequestAsyncReadback(Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_5(src, mipIndex, dstFormat, callback, null);
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x00038EA4 File Offset: 0x000370A4
		public void RequestAsyncReadback(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_6(src, mipIndex, x, width, y, height, z, depth, callback, null);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x00038ED8 File Offset: 0x000370D8
		public void RequestAsyncReadback(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback, null);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00038F18 File Offset: 0x00037118
		public void RequestAsyncReadback(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, dstFormat, callback, null);
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00038F4C File Offset: 0x0003714C
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_1(src, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x00038F80 File Offset: 0x00037180
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_2(src, size, offset, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00038FB8 File Offset: 0x000371B8
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, GraphicsBuffer src, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_8(src, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00038FEC File Offset: 0x000371EC
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, GraphicsBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_9(src, size, offset, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00039024 File Offset: 0x00037224
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_3(src, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00039058 File Offset: 0x00037258
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_4(src, mipIndex, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x0003908C File Offset: 0x0003728C
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_5(src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000390D0 File Offset: 0x000372D0
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_5(src, mipIndex, dstFormat, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00039108 File Offset: 0x00037308
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_6(src, mipIndex, x, width, y, height, z, depth, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00039148 File Offset: 0x00037348
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x00039198 File Offset: 0x00037398
		public unsafe void RequestAsyncReadbackIntoNativeArray<T>(ref NativeArray<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, dstFormat, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000391DC File Offset: 0x000373DC
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_1(src, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x00039210 File Offset: 0x00037410
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_2(src, size, offset, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00039248 File Offset: 0x00037448
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, GraphicsBuffer src, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_8(src, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0003927C File Offset: 0x0003747C
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, GraphicsBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_9(src, size, offset, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000392B4 File Offset: 0x000374B4
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_3(src, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000392E8 File Offset: 0x000374E8
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, int mipIndex, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_4(src, mipIndex, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0003931C File Offset: 0x0003751C
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_5(src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00039360 File Offset: 0x00037560
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_5(src, mipIndex, dstFormat, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00039398 File Offset: 0x00037598
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_6(src, mipIndex, x, width, y, height, z, depth, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000393D8 File Offset: 0x000375D8
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00039428 File Offset: 0x00037628
		public unsafe void RequestAsyncReadbackIntoNativeSlice<T>(ref NativeSlice<T> output, Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback) where T : struct
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			AsyncRequestNativeArrayData asyncRequestNativeArrayData = AsyncRequestNativeArrayData.CreateAndCheckAccess<T>(output);
			this.Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, dstFormat, callback, &asyncRequestNativeArrayData);
		}

		// Token: 0x0600225A RID: 8794
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_1([NotNull("ArgumentNullException")] ComputeBuffer src, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x0600225B RID: 8795
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_2([NotNull("ArgumentNullException")] ComputeBuffer src, int size, int offset, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x0600225C RID: 8796
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_3([NotNull("ArgumentNullException")] Texture src, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x0600225D RID: 8797
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_4([NotNull("ArgumentNullException")] Texture src, int mipIndex, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x0600225E RID: 8798
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_5([NotNull("ArgumentNullException")] Texture src, int mipIndex, GraphicsFormat dstFormat, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x0600225F RID: 8799
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_6([NotNull("ArgumentNullException")] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x06002260 RID: 8800
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_7([NotNull("ArgumentNullException")] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x06002261 RID: 8801
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_8([NotNull("ArgumentNullException")] GraphicsBuffer src, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x06002262 RID: 8802
		[NativeMethod("AddRequestAsyncReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void Internal_RequestAsyncReadback_9([NotNull("ArgumentNullException")] GraphicsBuffer src, int size, int offset, [NotNull("ArgumentNullException")] Action<AsyncGPUReadbackRequest> callback, AsyncRequestNativeArrayData* nativeArrayData = null);

		// Token: 0x06002263 RID: 8803
		[NativeMethod("AddSetInvertCulling")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInvertCulling(bool invertCulling);

		// Token: 0x06002264 RID: 8804 RVA: 0x00039469 File Offset: 0x00037669
		private void ConvertTexture_Internal(RenderTargetIdentifier src, int srcElement, RenderTargetIdentifier dst, int dstElement)
		{
			this.ConvertTexture_Internal_Injected(ref src, srcElement, ref dst, dstElement);
		}

		// Token: 0x06002265 RID: 8805
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetSinglePassStereo", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetSinglePassStereo(SinglePassStereoMode mode);

		// Token: 0x06002266 RID: 8806
		[FreeFunction("RenderingCommandBuffer_Bindings::InitBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InitBuffer();

		// Token: 0x06002267 RID: 8807
		[FreeFunction("RenderingCommandBuffer_Bindings::CreateGPUFence_Internal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr CreateGPUFence_Internal(GraphicsFenceType fenceType, SynchronisationStageFlags stage);

		// Token: 0x06002268 RID: 8808
		[FreeFunction("RenderingCommandBuffer_Bindings::WaitOnGPUFence_Internal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void WaitOnGPUFence_Internal(IntPtr fencePtr, SynchronisationStageFlags stage);

		// Token: 0x06002269 RID: 8809
		[FreeFunction("RenderingCommandBuffer_Bindings::ReleaseBuffer", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseBuffer();

		// Token: 0x0600226A RID: 8810
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeFloatParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetComputeFloatParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, float val);

		// Token: 0x0600226B RID: 8811
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeIntParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetComputeIntParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, int val);

		// Token: 0x0600226C RID: 8812 RVA: 0x00039478 File Offset: 0x00037678
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeVectorParam", HasExplicitThis = true)]
		public void SetComputeVectorParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, Vector4 val)
		{
			this.SetComputeVectorParam_Injected(computeShader, nameID, ref val);
		}

		// Token: 0x0600226D RID: 8813
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeVectorArrayParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetComputeVectorArrayParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, [Unmarshalled] Vector4[] values);

		// Token: 0x0600226E RID: 8814 RVA: 0x00039484 File Offset: 0x00037684
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeMatrixParam", HasExplicitThis = true)]
		public void SetComputeMatrixParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, Matrix4x4 val)
		{
			this.SetComputeMatrixParam_Injected(computeShader, nameID, ref val);
		}

		// Token: 0x0600226F RID: 8815
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeMatrixArrayParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetComputeMatrixArrayParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, [Unmarshalled] Matrix4x4[] values);

		// Token: 0x06002270 RID: 8816
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetComputeFloats", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeFloats([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, [Unmarshalled] float[] values);

		// Token: 0x06002271 RID: 8817
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetComputeInts", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeInts([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, [Unmarshalled] int[] values);

		// Token: 0x06002272 RID: 8818
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetComputeTextureParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeTextureParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, int nameID, ref RenderTargetIdentifier rt, int mipLevel, RenderTextureSubElement element);

		// Token: 0x06002273 RID: 8819
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeBufferParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, int nameID, ComputeBuffer buffer);

		// Token: 0x06002274 RID: 8820 RVA: 0x00039490 File Offset: 0x00037690
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeBufferParam", HasExplicitThis = true)]
		private void Internal_SetComputeGraphicsBufferHandleParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, int nameID, GraphicsBufferHandle bufferHandle)
		{
			this.Internal_SetComputeGraphicsBufferHandleParam_Injected(computeShader, kernelIndex, nameID, ref bufferHandle);
		}

		// Token: 0x06002275 RID: 8821
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeGraphicsBufferParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, int nameID, GraphicsBuffer buffer);

		// Token: 0x06002276 RID: 8822
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeConstantBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeConstantComputeBufferParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, ComputeBuffer buffer, int offset, int size);

		// Token: 0x06002277 RID: 8823
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeConstantBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeConstantGraphicsBufferParam([NotNull("ArgumentNullException")] ComputeShader computeShader, int nameID, GraphicsBuffer buffer, int offset, int size);

		// Token: 0x06002278 RID: 8824
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeParamsFromMaterial", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeParamsFromMaterial([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, Material material);

		// Token: 0x06002279 RID: 8825
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DispatchCompute", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DispatchCompute([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ);

		// Token: 0x0600227A RID: 8826
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DispatchComputeIndirect", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DispatchComputeIndirect([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, ComputeBuffer indirectBuffer, uint argsOffset);

		// Token: 0x0600227B RID: 8827
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DispatchComputeIndirect", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DispatchComputeIndirectGraphicsBuffer([NotNull("ArgumentNullException")] ComputeShader computeShader, int kernelIndex, GraphicsBuffer indirectBuffer, uint argsOffset);

		// Token: 0x0600227C RID: 8828
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingBufferParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, ComputeBuffer buffer);

		// Token: 0x0600227D RID: 8829
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingConstantBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingConstantComputeBufferParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, ComputeBuffer buffer, int offset, int size);

		// Token: 0x0600227E RID: 8830
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingConstantBufferParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingConstantGraphicsBufferParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, GraphicsBuffer buffer, int offset, int size);

		// Token: 0x0600227F RID: 8831
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingTextureParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingTextureParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, ref RenderTargetIdentifier rt);

		// Token: 0x06002280 RID: 8832
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingFloatParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingFloatParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, float val);

		// Token: 0x06002281 RID: 8833
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingIntParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingIntParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, int val);

		// Token: 0x06002282 RID: 8834 RVA: 0x0003949D File Offset: 0x0003769D
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingVectorParam", HasExplicitThis = true)]
		private void Internal_SetRayTracingVectorParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, Vector4 val)
		{
			this.Internal_SetRayTracingVectorParam_Injected(rayTracingShader, nameID, ref val);
		}

		// Token: 0x06002283 RID: 8835
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingVectorArrayParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingVectorArrayParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, [Unmarshalled] Vector4[] values);

		// Token: 0x06002284 RID: 8836 RVA: 0x000394A9 File Offset: 0x000376A9
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingMatrixParam", HasExplicitThis = true)]
		private void Internal_SetRayTracingMatrixParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, Matrix4x4 val)
		{
			this.Internal_SetRayTracingMatrixParam_Injected(rayTracingShader, nameID, ref val);
		}

		// Token: 0x06002285 RID: 8837
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingMatrixArrayParam", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingMatrixArrayParam([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, [Unmarshalled] Matrix4x4[] values);

		// Token: 0x06002286 RID: 8838
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingFloats", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingFloats([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, [Unmarshalled] float[] values);

		// Token: 0x06002287 RID: 8839
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingInts", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingInts([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, [Unmarshalled] int[] values);

		// Token: 0x06002288 RID: 8840 RVA: 0x000394B5 File Offset: 0x000376B5
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_BuildRayTracingAccelerationStructure", HasExplicitThis = true)]
		private void Internal_BuildRayTracingAccelerationStructure([NotNull("ArgumentNullException")] RayTracingAccelerationStructure accelerationStructure, Vector3 relativeOrigin)
		{
			this.Internal_BuildRayTracingAccelerationStructure_Injected(accelerationStructure, ref relativeOrigin);
		}

		// Token: 0x06002289 RID: 8841
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetRayTracingAccelerationStructure", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingAccelerationStructure([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, int nameID, RayTracingAccelerationStructure accelerationStructure);

		// Token: 0x0600228A RID: 8842
		[NativeMethod("AddSetRayTracingShaderPass")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetRayTracingShaderPass([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, string passName);

		// Token: 0x0600228B RID: 8843
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DispatchRays", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DispatchRays([NotNull("ArgumentNullException")] RayTracingShader rayTracingShader, string rayGenShaderName, uint width, uint height, uint depth, Camera camera = null);

		// Token: 0x0600228C RID: 8844 RVA: 0x000394C0 File Offset: 0x000376C0
		[NativeMethod("AddGenerateMips")]
		private void Internal_GenerateMips(RenderTargetIdentifier rt)
		{
			this.Internal_GenerateMips_Injected(ref rt);
		}

		// Token: 0x0600228D RID: 8845
		[NativeMethod("AddResolveAntiAliasedSurface")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_ResolveAntiAliasedSurface(RenderTexture rt, RenderTexture target);

		// Token: 0x0600228E RID: 8846
		[NativeMethod("AddCopyCounterValue")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyCounterValueCC(ComputeBuffer src, ComputeBuffer dst, uint dstOffsetBytes);

		// Token: 0x0600228F RID: 8847
		[NativeMethod("AddCopyCounterValue")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyCounterValueGC(GraphicsBuffer src, ComputeBuffer dst, uint dstOffsetBytes);

		// Token: 0x06002290 RID: 8848
		[NativeMethod("AddCopyCounterValue")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyCounterValueCG(ComputeBuffer src, GraphicsBuffer dst, uint dstOffsetBytes);

		// Token: 0x06002291 RID: 8849
		[NativeMethod("AddCopyCounterValue")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyCounterValueGG(GraphicsBuffer src, GraphicsBuffer dst, uint dstOffsetBytes);

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002292 RID: 8850
		// (set) Token: 0x06002293 RID: 8851
		public extern string name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002294 RID: 8852
		public extern int sizeInBytes
		{
			[NativeMethod("GetBufferSize")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06002295 RID: 8853
		[NativeMethod("ClearCommands")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Clear();

		// Token: 0x06002296 RID: 8854 RVA: 0x000394CA File Offset: 0x000376CA
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMesh", HasExplicitThis = true)]
		private void Internal_DrawMesh([NotNull("ArgumentNullException")] Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties)
		{
			this.Internal_DrawMesh_Injected(mesh, ref matrix, material, submeshIndex, shaderPass, properties);
		}

		// Token: 0x06002297 RID: 8855
		[NativeMethod("AddDrawRenderer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawRenderer([NotNull("ArgumentNullException")] Renderer renderer, Material material, int submeshIndex, int shaderPass);

		// Token: 0x06002298 RID: 8856 RVA: 0x000394DC File Offset: 0x000376DC
		[NativeMethod("AddDrawRendererList")]
		private void Internal_DrawRendererList(RendererList rendererList)
		{
			this.Internal_DrawRendererList_Injected(ref rendererList);
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000394E6 File Offset: 0x000376E6
		private void Internal_DrawRenderer(Renderer renderer, Material material, int submeshIndex)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_DrawRenderer(renderer, material, submeshIndex, -1);
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000394FD File Offset: 0x000376FD
		private void Internal_DrawRenderer(Renderer renderer, Material material)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_DrawRenderer(renderer, material, 0);
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00039513 File Offset: 0x00037713
		[NativeMethod("AddDrawProcedural")]
		private void Internal_DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties)
		{
			this.Internal_DrawProcedural_Injected(ref matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00039528 File Offset: 0x00037728
		[NativeMethod("AddDrawProceduralIndexed")]
		private void Internal_DrawProceduralIndexed(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount, MaterialPropertyBlock properties)
		{
			this.Internal_DrawProceduralIndexed_Injected(indexBuffer, ref matrix, material, shaderPass, topology, indexCount, instanceCount, properties);
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00039549 File Offset: 0x00037749
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawProceduralIndirect", HasExplicitThis = true)]
		private void Internal_DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			this.Internal_DrawProceduralIndirect_Injected(ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00039560 File Offset: 0x00037760
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawProceduralIndexedIndirect", HasExplicitThis = true)]
		private void Internal_DrawProceduralIndexedIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			this.Internal_DrawProceduralIndexedIndirect_Injected(indexBuffer, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00039581 File Offset: 0x00037781
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawProceduralIndirect", HasExplicitThis = true)]
		private void Internal_DrawProceduralIndirectGraphicsBuffer(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			this.Internal_DrawProceduralIndirectGraphicsBuffer_Injected(ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00039598 File Offset: 0x00037798
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawProceduralIndexedIndirect", HasExplicitThis = true)]
		private void Internal_DrawProceduralIndexedIndirectGraphicsBuffer(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			this.Internal_DrawProceduralIndexedIndirectGraphicsBuffer_Injected(indexBuffer, ref matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x060022A1 RID: 8865
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMeshInstanced", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, [Unmarshalled] Matrix4x4[] matrices, int count, MaterialPropertyBlock properties);

		// Token: 0x060022A2 RID: 8866
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMeshInstancedProcedural", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawMeshInstancedProcedural(Mesh mesh, int submeshIndex, Material material, int shaderPass, int count, MaterialPropertyBlock properties);

		// Token: 0x060022A3 RID: 8867
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMeshInstancedIndirect", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		// Token: 0x060022A4 RID: 8868
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMeshInstancedIndirect", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawMeshInstancedIndirectGraphicsBuffer(Mesh mesh, int submeshIndex, Material material, int shaderPass, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		// Token: 0x060022A5 RID: 8869 RVA: 0x000395B9 File Offset: 0x000377B9
		[FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawOcclusionMesh", HasExplicitThis = true)]
		private void Internal_DrawOcclusionMesh(RectInt normalizedCamViewport)
		{
			this.Internal_DrawOcclusionMesh_Injected(ref normalizedCamViewport);
		}

		// Token: 0x060022A6 RID: 8870
		[FreeFunction("RenderingCommandBuffer_Bindings::SetRandomWriteTarget_Texture", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRandomWriteTarget_Texture(int index, ref RenderTargetIdentifier rt);

		// Token: 0x060022A7 RID: 8871
		[FreeFunction("RenderingCommandBuffer_Bindings::SetRandomWriteTarget_Buffer", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRandomWriteTarget_Buffer(int index, ComputeBuffer uav, bool preserveCounterValue);

		// Token: 0x060022A8 RID: 8872
		[FreeFunction("RenderingCommandBuffer_Bindings::SetRandomWriteTarget_Buffer", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRandomWriteTarget_GraphicsBuffer(int index, GraphicsBuffer uav, bool preserveCounterValue);

		// Token: 0x060022A9 RID: 8873
		[FreeFunction("RenderingCommandBuffer_Bindings::ClearRandomWriteTargets", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearRandomWriteTargets();

		// Token: 0x060022AA RID: 8874 RVA: 0x000395C3 File Offset: 0x000377C3
		[FreeFunction("RenderingCommandBuffer_Bindings::SetViewport", HasExplicitThis = true, ThrowsException = true)]
		public void SetViewport(Rect pixelRect)
		{
			this.SetViewport_Injected(ref pixelRect);
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000395CD File Offset: 0x000377CD
		[FreeFunction("RenderingCommandBuffer_Bindings::EnableScissorRect", HasExplicitThis = true, ThrowsException = true)]
		public void EnableScissorRect(Rect scissor)
		{
			this.EnableScissorRect_Injected(ref scissor);
		}

		// Token: 0x060022AC RID: 8876
		[FreeFunction("RenderingCommandBuffer_Bindings::DisableScissorRect", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DisableScissorRect();

		// Token: 0x060022AD RID: 8877
		[FreeFunction("RenderingCommandBuffer_Bindings::CopyTexture_Internal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyTexture_Internal(ref RenderTargetIdentifier src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, ref RenderTargetIdentifier dst, int dstElement, int dstMip, int dstX, int dstY, int mode);

		// Token: 0x060022AE RID: 8878 RVA: 0x000395D8 File Offset: 0x000377D8
		[FreeFunction("RenderingCommandBuffer_Bindings::Blit_Texture", HasExplicitThis = true)]
		private void Blit_Texture(Texture source, ref RenderTargetIdentifier dest, Material mat, int pass, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
		{
			this.Blit_Texture_Injected(source, ref dest, mat, pass, ref scale, ref offset, sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000395F8 File Offset: 0x000377F8
		[FreeFunction("RenderingCommandBuffer_Bindings::Blit_Identifier", HasExplicitThis = true)]
		private void Blit_Identifier(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, Material mat, int pass, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
		{
			this.Blit_Identifier_Injected(ref source, ref dest, mat, pass, ref scale, ref offset, sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x060022B0 RID: 8880
		[FreeFunction("RenderingCommandBuffer_Bindings::GetTemporaryRT", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode, bool useDynamicScale);

		// Token: 0x060022B1 RID: 8881 RVA: 0x00039618 File Offset: 0x00037818
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, antiAliasing, enableRandomWrite, memorylessMode, false);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00039640 File Offset: 0x00037840
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, antiAliasing, enableRandomWrite, RenderTextureMemoryless.None);
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00039664 File Offset: 0x00037864
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, antiAliasing, false, RenderTextureMemoryless.None);
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x00039686 File Offset: 0x00037886
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, 1);
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0003969C File Offset: 0x0003789C
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode, bool useDynamicScale)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, enableRandomWrite, memorylessMode, useDynamicScale);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000396CC File Offset: 0x000378CC
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite, memorylessMode, false);
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000396F4 File Offset: 0x000378F4
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite, RenderTextureMemoryless.None);
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0003971C File Offset: 0x0003791C
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, false);
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00039740 File Offset: 0x00037940
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, 1);
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x00039761 File Offset: 0x00037961
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default));
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0003977A File Offset: 0x0003797A
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, filter, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR));
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00039791 File Offset: 0x00037991
		public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
		{
			this.GetTemporaryRT(nameID, width, height, depthBuffer, FilterMode.Point);
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000397A1 File Offset: 0x000379A1
		public void GetTemporaryRT(int nameID, int width, int height)
		{
			this.GetTemporaryRT(nameID, width, height, 0);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000397AF File Offset: 0x000379AF
		[FreeFunction("RenderingCommandBuffer_Bindings::GetTemporaryRTWithDescriptor", HasExplicitThis = true)]
		private void GetTemporaryRTWithDescriptor(int nameID, RenderTextureDescriptor desc, FilterMode filter)
		{
			this.GetTemporaryRTWithDescriptor_Injected(nameID, ref desc, filter);
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000397BB File Offset: 0x000379BB
		public void GetTemporaryRT(int nameID, RenderTextureDescriptor desc, FilterMode filter)
		{
			this.GetTemporaryRTWithDescriptor(nameID, desc, filter);
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000397C8 File Offset: 0x000379C8
		public void GetTemporaryRT(int nameID, RenderTextureDescriptor desc)
		{
			this.GetTemporaryRT(nameID, desc, FilterMode.Point);
		}

		// Token: 0x060022C1 RID: 8897
		[FreeFunction("RenderingCommandBuffer_Bindings::GetTemporaryRTArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite, bool useDynamicScale);

		// Token: 0x060022C2 RID: 8898 RVA: 0x000397D8 File Offset: 0x000379D8
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, format, antiAliasing, enableRandomWrite, false);
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00039800 File Offset: 0x00037A00
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, format, antiAliasing, false);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00039824 File Offset: 0x00037A24
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, format, 1);
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00039848 File Offset: 0x00037A48
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, enableRandomWrite, false);
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00039874 File Offset: 0x00037A74
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, false);
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000398A0 File Offset: 0x00037AA0
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), 1, false);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000398CC File Offset: 0x00037ACC
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default), 1, false);
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000398F4 File Offset: 0x00037AF4
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR), 1, false);
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x0003991A File Offset: 0x00037B1A
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, FilterMode.Point);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x0003992C File Offset: 0x00037B2C
		public void GetTemporaryRTArray(int nameID, int width, int height, int slices)
		{
			this.GetTemporaryRTArray(nameID, width, height, slices, 0);
		}

		// Token: 0x060022CC RID: 8908
		[FreeFunction("RenderingCommandBuffer_Bindings::ReleaseTemporaryRT", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ReleaseTemporaryRT(int nameID);

		// Token: 0x060022CD RID: 8909 RVA: 0x0003993C File Offset: 0x00037B3C
		[FreeFunction("RenderingCommandBuffer_Bindings::ClearRenderTarget", HasExplicitThis = true)]
		public void ClearRenderTarget(RTClearFlags clearFlags, Color backgroundColor, float depth, uint stencil)
		{
			this.ClearRenderTarget_Injected(clearFlags, ref backgroundColor, depth, stencil);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x0003994A File Offset: 0x00037B4A
		public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.ClearRenderTarget((clearColor ? RTClearFlags.Color : RTClearFlags.None) | (clearDepth ? RTClearFlags.DepthStencil : RTClearFlags.None), backgroundColor, 1f, 0U);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00039973 File Offset: 0x00037B73
		public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.ClearRenderTarget((clearColor ? RTClearFlags.Color : RTClearFlags.None) | (clearDepth ? RTClearFlags.DepthStencil : RTClearFlags.None), backgroundColor, depth, 0U);
		}

		// Token: 0x060022D0 RID: 8912
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalFloat", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalFloat(int nameID, float value);

		// Token: 0x060022D1 RID: 8913
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalInt", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalInt(int nameID, int value);

		// Token: 0x060022D2 RID: 8914
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalInteger", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalInteger(int nameID, int value);

		// Token: 0x060022D3 RID: 8915 RVA: 0x00039999 File Offset: 0x00037B99
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalVector", HasExplicitThis = true)]
		public void SetGlobalVector(int nameID, Vector4 value)
		{
			this.SetGlobalVector_Injected(nameID, ref value);
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000399A4 File Offset: 0x00037BA4
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalColor", HasExplicitThis = true)]
		public void SetGlobalColor(int nameID, Color value)
		{
			this.SetGlobalColor_Injected(nameID, ref value);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000399AF File Offset: 0x00037BAF
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalMatrix", HasExplicitThis = true)]
		public void SetGlobalMatrix(int nameID, Matrix4x4 value)
		{
			this.SetGlobalMatrix_Injected(nameID, ref value);
		}

		// Token: 0x060022D6 RID: 8918
		[FreeFunction("RenderingCommandBuffer_Bindings::EnableShaderKeyword", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EnableShaderKeyword(string keyword);

		// Token: 0x060022D7 RID: 8919 RVA: 0x000399BA File Offset: 0x00037BBA
		[FreeFunction("RenderingCommandBuffer_Bindings::EnableShaderKeyword", HasExplicitThis = true)]
		private void EnableGlobalKeyword(GlobalKeyword keyword)
		{
			this.EnableGlobalKeyword_Injected(ref keyword);
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000399C4 File Offset: 0x00037BC4
		[FreeFunction("RenderingCommandBuffer_Bindings::EnableMaterialKeyword", HasExplicitThis = true)]
		private void EnableMaterialKeyword(Material material, LocalKeyword keyword)
		{
			this.EnableMaterialKeyword_Injected(material, ref keyword);
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000399CF File Offset: 0x00037BCF
		[FreeFunction("RenderingCommandBuffer_Bindings::EnableComputeKeyword", HasExplicitThis = true)]
		private void EnableComputeKeyword(ComputeShader computeShader, LocalKeyword keyword)
		{
			this.EnableComputeKeyword_Injected(computeShader, ref keyword);
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000399DA File Offset: 0x00037BDA
		public void EnableKeyword(in GlobalKeyword keyword)
		{
			this.EnableGlobalKeyword(keyword);
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000399EA File Offset: 0x00037BEA
		public void EnableKeyword(Material material, in LocalKeyword keyword)
		{
			this.EnableMaterialKeyword(material, keyword);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000399FB File Offset: 0x00037BFB
		public void EnableKeyword(ComputeShader computeShader, in LocalKeyword keyword)
		{
			this.EnableComputeKeyword(computeShader, keyword);
		}

		// Token: 0x060022DD RID: 8925
		[FreeFunction("RenderingCommandBuffer_Bindings::DisableShaderKeyword", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DisableShaderKeyword(string keyword);

		// Token: 0x060022DE RID: 8926 RVA: 0x00039A0C File Offset: 0x00037C0C
		[FreeFunction("RenderingCommandBuffer_Bindings::DisableShaderKeyword", HasExplicitThis = true)]
		private void DisableGlobalKeyword(GlobalKeyword keyword)
		{
			this.DisableGlobalKeyword_Injected(ref keyword);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00039A16 File Offset: 0x00037C16
		[FreeFunction("RenderingCommandBuffer_Bindings::DisableMaterialKeyword", HasExplicitThis = true)]
		private void DisableMaterialKeyword(Material material, LocalKeyword keyword)
		{
			this.DisableMaterialKeyword_Injected(material, ref keyword);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00039A21 File Offset: 0x00037C21
		[FreeFunction("RenderingCommandBuffer_Bindings::DisableComputeKeyword", HasExplicitThis = true)]
		private void DisableComputeKeyword(ComputeShader computeShader, LocalKeyword keyword)
		{
			this.DisableComputeKeyword_Injected(computeShader, ref keyword);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00039A2C File Offset: 0x00037C2C
		public void DisableKeyword(in GlobalKeyword keyword)
		{
			this.DisableGlobalKeyword(keyword);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x00039A3C File Offset: 0x00037C3C
		public void DisableKeyword(Material material, in LocalKeyword keyword)
		{
			this.DisableMaterialKeyword(material, keyword);
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x00039A4D File Offset: 0x00037C4D
		public void DisableKeyword(ComputeShader computeShader, in LocalKeyword keyword)
		{
			this.DisableComputeKeyword(computeShader, keyword);
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x00039A5E File Offset: 0x00037C5E
		[FreeFunction("RenderingCommandBuffer_Bindings::SetShaderKeyword", HasExplicitThis = true)]
		private void SetGlobalKeyword(GlobalKeyword keyword, bool value)
		{
			this.SetGlobalKeyword_Injected(ref keyword, value);
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x00039A69 File Offset: 0x00037C69
		[FreeFunction("RenderingCommandBuffer_Bindings::SetMaterialKeyword", HasExplicitThis = true)]
		private void SetMaterialKeyword(Material material, LocalKeyword keyword, bool value)
		{
			this.SetMaterialKeyword_Injected(material, ref keyword, value);
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x00039A75 File Offset: 0x00037C75
		[FreeFunction("RenderingCommandBuffer_Bindings::SetComputeKeyword", HasExplicitThis = true)]
		private void SetComputeKeyword(ComputeShader computeShader, LocalKeyword keyword, bool value)
		{
			this.SetComputeKeyword_Injected(computeShader, ref keyword, value);
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x00039A81 File Offset: 0x00037C81
		public void SetKeyword(in GlobalKeyword keyword, bool value)
		{
			this.SetGlobalKeyword(keyword, value);
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x00039A92 File Offset: 0x00037C92
		public void SetKeyword(Material material, in LocalKeyword keyword, bool value)
		{
			this.SetMaterialKeyword(material, keyword, value);
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x00039AA4 File Offset: 0x00037CA4
		public void SetKeyword(ComputeShader computeShader, in LocalKeyword keyword, bool value)
		{
			this.SetComputeKeyword(computeShader, keyword, value);
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x00039AB6 File Offset: 0x00037CB6
		[FreeFunction("RenderingCommandBuffer_Bindings::SetViewMatrix", HasExplicitThis = true, ThrowsException = true)]
		public void SetViewMatrix(Matrix4x4 view)
		{
			this.SetViewMatrix_Injected(ref view);
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x00039AC0 File Offset: 0x00037CC0
		[FreeFunction("RenderingCommandBuffer_Bindings::SetProjectionMatrix", HasExplicitThis = true, ThrowsException = true)]
		public void SetProjectionMatrix(Matrix4x4 proj)
		{
			this.SetProjectionMatrix_Injected(ref proj);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x00039ACA File Offset: 0x00037CCA
		[FreeFunction("RenderingCommandBuffer_Bindings::SetViewProjectionMatrices", HasExplicitThis = true, ThrowsException = true)]
		public void SetViewProjectionMatrices(Matrix4x4 view, Matrix4x4 proj)
		{
			this.SetViewProjectionMatrices_Injected(ref view, ref proj);
		}

		// Token: 0x060022ED RID: 8941
		[NativeMethod("AddSetGlobalDepthBias")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalDepthBias(float bias, float slopeBias);

		// Token: 0x060022EE RID: 8942
		[FreeFunction("RenderingCommandBuffer_Bindings::SetExecutionFlags", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetExecutionFlags(CommandBufferExecutionFlags flags);

		// Token: 0x060022EF RID: 8943
		[FreeFunction("RenderingCommandBuffer_Bindings::ValidateAgainstExecutionFlags", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool ValidateAgainstExecutionFlags(CommandBufferExecutionFlags requiredFlags, CommandBufferExecutionFlags invalidFlags);

		// Token: 0x060022F0 RID: 8944
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalFloatArrayListImpl", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalFloatArrayListImpl(int nameID, object values);

		// Token: 0x060022F1 RID: 8945
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalVectorArrayListImpl", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalVectorArrayListImpl(int nameID, object values);

		// Token: 0x060022F2 RID: 8946
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalMatrixArrayListImpl", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalMatrixArrayListImpl(int nameID, object values);

		// Token: 0x060022F3 RID: 8947
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalFloatArray", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalFloatArray(int nameID, [Unmarshalled] float[] values);

		// Token: 0x060022F4 RID: 8948
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalVectorArray", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalVectorArray(int nameID, [Unmarshalled] Vector4[] values);

		// Token: 0x060022F5 RID: 8949
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalMatrixArray", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalMatrixArray(int nameID, [Unmarshalled] Matrix4x4[] values);

		// Token: 0x060022F6 RID: 8950
		[FreeFunction("RenderingCommandBuffer_Bindings::SetLateLatchProjectionMatrices", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetLateLatchProjectionMatrices([Unmarshalled] Matrix4x4[] projectionMat);

		// Token: 0x060022F7 RID: 8951
		[FreeFunction("RenderingCommandBuffer_Bindings::MarkLateLatchMatrixShaderPropertyID", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void MarkLateLatchMatrixShaderPropertyID(CameraLateLatchMatrixType matrixPropertyType, int shaderPropertyID);

		// Token: 0x060022F8 RID: 8952
		[FreeFunction("RenderingCommandBuffer_Bindings::UnmarkLateLatchMatrix", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UnmarkLateLatchMatrix(CameraLateLatchMatrixType matrixPropertyType);

		// Token: 0x060022F9 RID: 8953
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalTexture_Impl", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalTexture_Impl(int nameID, ref RenderTargetIdentifier rt, RenderTextureSubElement element);

		// Token: 0x060022FA RID: 8954
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalBufferInternal(int nameID, ComputeBuffer value);

		// Token: 0x060022FB RID: 8955
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalGraphicsBufferInternal(int nameID, GraphicsBuffer value);

		// Token: 0x060022FC RID: 8956
		[FreeFunction("RenderingCommandBuffer_Bindings::SetShadowSamplingMode_Impl", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetShadowSamplingMode_Impl(ref RenderTargetIdentifier shadowmap, ShadowSamplingMode mode);

		// Token: 0x060022FD RID: 8957
		[FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginEventInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IssuePluginEventInternal(IntPtr callback, int eventID);

		// Token: 0x060022FE RID: 8958
		[FreeFunction("RenderingCommandBuffer_Bindings::BeginSample", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BeginSample(string name);

		// Token: 0x060022FF RID: 8959
		[FreeFunction("RenderingCommandBuffer_Bindings::EndSample", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void EndSample(string name);

		// Token: 0x06002300 RID: 8960 RVA: 0x00039AD6 File Offset: 0x00037CD6
		public void BeginSample(CustomSampler sampler)
		{
			this.BeginSample_CustomSampler(sampler);
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x00039AE1 File Offset: 0x00037CE1
		public void EndSample(CustomSampler sampler)
		{
			this.EndSample_CustomSampler(sampler);
		}

		// Token: 0x06002302 RID: 8962
		[FreeFunction("RenderingCommandBuffer_Bindings::BeginSample_CustomSampler", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void BeginSample_CustomSampler([NotNull("ArgumentNullException")] CustomSampler sampler);

		// Token: 0x06002303 RID: 8963
		[FreeFunction("RenderingCommandBuffer_Bindings::EndSample_CustomSampler", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EndSample_CustomSampler([NotNull("ArgumentNullException")] CustomSampler sampler);

		// Token: 0x06002304 RID: 8964 RVA: 0x00039AEC File Offset: 0x00037CEC
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginSample(ProfilerMarker marker)
		{
			this.BeginSample_ProfilerMarker(marker.Handle);
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x00039AFD File Offset: 0x00037CFD
		[Conditional("ENABLE_PROFILER")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndSample(ProfilerMarker marker)
		{
			this.EndSample_ProfilerMarker(marker.Handle);
		}

		// Token: 0x06002306 RID: 8966
		[FreeFunction("RenderingCommandBuffer_Bindings::BeginSample_ProfilerMarker", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void BeginSample_ProfilerMarker(IntPtr markerHandle);

		// Token: 0x06002307 RID: 8967
		[FreeFunction("RenderingCommandBuffer_Bindings::EndSample_ProfilerMarker", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EndSample_ProfilerMarker(IntPtr markerHandle);

		// Token: 0x06002308 RID: 8968
		[FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginEventAndDataInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IssuePluginEventAndDataInternal(IntPtr callback, int eventID, IntPtr data);

		// Token: 0x06002309 RID: 8969
		[FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginEventAndDataWithFlagsInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IssuePluginEventAndDataInternalWithFlags(IntPtr callback, int eventID, CustomMarkerCallbackFlags flags, IntPtr data);

		// Token: 0x0600230A RID: 8970
		[FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginCustomBlitInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IssuePluginCustomBlitInternal(IntPtr callback, uint command, ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, uint commandParam, uint commandFlags);

		// Token: 0x0600230B RID: 8971
		[FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginCustomTextureUpdateInternal", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IssuePluginCustomTextureUpdateInternal(IntPtr callback, Texture targetTexture, uint userData, bool useNewUnityRenderingExtTextureUpdateParamsV2);

		// Token: 0x0600230C RID: 8972
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalConstantBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalConstantBufferInternal(ComputeBuffer buffer, int nameID, int offset, int size);

		// Token: 0x0600230D RID: 8973
		[FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalConstantBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalConstantGraphicsBufferInternal(GraphicsBuffer buffer, int nameID, int offset, int size);

		// Token: 0x0600230E RID: 8974 RVA: 0x00039B0E File Offset: 0x00037D0E
		[FreeFunction("RenderingCommandBuffer_Bindings::IncrementUpdateCount", HasExplicitThis = true)]
		public void IncrementUpdateCount(RenderTargetIdentifier dest)
		{
			this.IncrementUpdateCount_Injected(ref dest);
		}

		// Token: 0x0600230F RID: 8975
		[FreeFunction("RenderingCommandBuffer_Bindings::SetInstanceMultiplier", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInstanceMultiplier(uint multiplier);

		// Token: 0x06002310 RID: 8976
		[FreeFunction("RenderingCommandBuffer_Bindings::SetFoveatedRenderingMode", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFoveatedRenderingMode(FoveatedRenderingMode foveatedRenderingMode);

		// Token: 0x06002311 RID: 8977
		[FreeFunction("RenderingCommandBuffer_Bindings::SetWireframe", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetWireframe(bool enable);

		// Token: 0x06002312 RID: 8978
		[FreeFunction("RenderingCommandBuffer_Bindings::ConfigureFoveatedRendering", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ConfigureFoveatedRendering(IntPtr platformData);

		// Token: 0x06002313 RID: 8979 RVA: 0x00039B18 File Offset: 0x00037D18
		public void SetRenderTarget(RenderTargetIdentifier rt)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.SetRenderTargetSingle_Internal(rt, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x00039B30 File Offset: 0x00037D30
		public void SetRenderTarget(RenderTargetIdentifier rt, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = loadAction == RenderBufferLoadAction.Clear;
			if (flag)
			{
				throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
			}
			this.SetRenderTargetSingle_Internal(rt, loadAction, storeAction, loadAction, storeAction);
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x00039B68 File Offset: 0x00037D68
		public void SetRenderTarget(RenderTargetIdentifier rt, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = colorLoadAction == RenderBufferLoadAction.Clear || depthLoadAction == RenderBufferLoadAction.Clear;
			if (flag)
			{
				throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
			}
			this.SetRenderTargetSingle_Internal(rt, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction);
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x00039BA8 File Offset: 0x00037DA8
		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = mipLevel < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid value for mipLevel ({0})", mipLevel));
			}
			this.SetRenderTargetSingle_Internal(new RenderTargetIdentifier(rt, mipLevel, CubemapFace.Unknown, 0), RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x00039BF4 File Offset: 0x00037DF4
		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = mipLevel < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid value for mipLevel ({0})", mipLevel));
			}
			this.SetRenderTargetSingle_Internal(new RenderTargetIdentifier(rt, mipLevel, cubemapFace, 0), RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x00039C40 File Offset: 0x00037E40
		public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = depthSlice < -1;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid value for depthSlice ({0})", depthSlice));
			}
			bool flag2 = mipLevel < 0;
			if (flag2)
			{
				throw new ArgumentException(string.Format("Invalid value for mipLevel ({0})", mipLevel));
			}
			this.SetRenderTargetSingle_Internal(new RenderTargetIdentifier(rt, mipLevel, cubemapFace, depthSlice), RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x00039CAA File Offset: 0x00037EAA
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.SetRenderTargetColorDepth_Internal(color, depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderTargetFlags.None);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x00039CC4 File Offset: 0x00037EC4
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = mipLevel < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid value for mipLevel ({0})", mipLevel));
			}
			this.SetRenderTargetColorDepth_Internal(new RenderTargetIdentifier(color, mipLevel, CubemapFace.Unknown, 0), depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderTargetFlags.None);
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x00039D10 File Offset: 0x00037F10
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = mipLevel < 0;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid value for mipLevel ({0})", mipLevel));
			}
			this.SetRenderTargetColorDepth_Internal(new RenderTargetIdentifier(color, mipLevel, cubemapFace, 0), depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderTargetFlags.None);
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x00039D5C File Offset: 0x00037F5C
		public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = depthSlice < -1;
			if (flag)
			{
				throw new ArgumentException(string.Format("Invalid value for depthSlice ({0})", depthSlice));
			}
			bool flag2 = mipLevel < 0;
			if (flag2)
			{
				throw new ArgumentException(string.Format("Invalid value for mipLevel ({0})", mipLevel));
			}
			this.SetRenderTargetColorDepth_Internal(new RenderTargetIdentifier(color, mipLevel, cubemapFace, depthSlice), depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderTargetFlags.None);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x00039DCC File Offset: 0x00037FCC
		public void SetRenderTarget(RenderTargetIdentifier color, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderTargetIdentifier depth, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = colorLoadAction == RenderBufferLoadAction.Clear || depthLoadAction == RenderBufferLoadAction.Clear;
			if (flag)
			{
				throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
			}
			this.SetRenderTargetColorDepth_Internal(color, depth, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction, RenderTargetFlags.None);
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x00039E10 File Offset: 0x00038010
		public void SetRenderTarget(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = colors.Length < 1;
			if (flag)
			{
				throw new ArgumentException(string.Format("colors.Length must be at least 1, but was {0}", colors.Length));
			}
			bool flag2 = colors.Length > SystemInfo.supportedRenderTargetCount;
			if (flag2)
			{
				throw new ArgumentException(string.Format("colors.Length is {0} and exceeds the maximum number of supported render targets ({1})", colors.Length, SystemInfo.supportedRenderTargetCount));
			}
			this.SetRenderTargetMulti_Internal(colors, depth, null, null, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderTargetFlags.None);
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x00039E88 File Offset: 0x00038088
		public void SetRenderTarget(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = colors.Length < 1;
			if (flag)
			{
				throw new ArgumentException(string.Format("colors.Length must be at least 1, but was {0}", colors.Length));
			}
			bool flag2 = colors.Length > SystemInfo.supportedRenderTargetCount;
			if (flag2)
			{
				throw new ArgumentException(string.Format("colors.Length is {0} and exceeds the maximum number of supported render targets ({1})", colors.Length, SystemInfo.supportedRenderTargetCount));
			}
			this.SetRenderTargetMultiSubtarget(colors, depth, null, null, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, mipLevel, cubemapFace, depthSlice);
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x00039F04 File Offset: 0x00038104
		public void SetRenderTarget(RenderTargetBinding binding, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = binding.colorRenderTargets.Length < 1;
			if (flag)
			{
				throw new ArgumentException(string.Format("The number of color render targets must be at least 1, but was {0}", binding.colorRenderTargets.Length));
			}
			bool flag2 = binding.colorRenderTargets.Length > SystemInfo.supportedRenderTargetCount;
			if (flag2)
			{
				throw new ArgumentException(string.Format("The number of color render targets ({0}) and exceeds the maximum supported number of render targets ({1})", binding.colorRenderTargets.Length, SystemInfo.supportedRenderTargetCount));
			}
			bool flag3 = binding.colorLoadActions.Length != binding.colorRenderTargets.Length;
			if (flag3)
			{
				throw new ArgumentException(string.Format("The number of color load actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
			}
			bool flag4 = binding.colorStoreActions.Length != binding.colorRenderTargets.Length;
			if (flag4)
			{
				throw new ArgumentException(string.Format("The number of color store actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
			}
			bool flag5 = binding.depthLoadAction == RenderBufferLoadAction.Clear || Array.IndexOf<RenderBufferLoadAction>(binding.colorLoadActions, RenderBufferLoadAction.Clear) > -1;
			if (flag5)
			{
				throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
			}
			bool flag6 = binding.colorRenderTargets.Length == 1;
			if (flag6)
			{
				this.SetRenderTargetColorDepthSubtarget(binding.colorRenderTargets[0], binding.depthRenderTarget, binding.colorLoadActions[0], binding.colorStoreActions[0], binding.depthLoadAction, binding.depthStoreAction, mipLevel, cubemapFace, depthSlice);
			}
			else
			{
				this.SetRenderTargetMultiSubtarget(binding.colorRenderTargets, binding.depthRenderTarget, binding.colorLoadActions, binding.colorStoreActions, binding.depthLoadAction, binding.depthStoreAction, mipLevel, cubemapFace, depthSlice);
			}
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x0003A0CC File Offset: 0x000382CC
		public void SetRenderTarget(RenderTargetBinding binding)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag = binding.colorRenderTargets.Length < 1;
			if (flag)
			{
				throw new ArgumentException(string.Format("The number of color render targets must be at least 1, but was {0}", binding.colorRenderTargets.Length));
			}
			bool flag2 = binding.colorRenderTargets.Length > SystemInfo.supportedRenderTargetCount;
			if (flag2)
			{
				throw new ArgumentException(string.Format("The number of color render targets ({0}) and exceeds the maximum supported number of render targets ({1})", binding.colorRenderTargets.Length, SystemInfo.supportedRenderTargetCount));
			}
			bool flag3 = binding.colorLoadActions.Length != binding.colorRenderTargets.Length;
			if (flag3)
			{
				throw new ArgumentException(string.Format("The number of color load actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
			}
			bool flag4 = binding.colorStoreActions.Length != binding.colorRenderTargets.Length;
			if (flag4)
			{
				throw new ArgumentException(string.Format("The number of color store actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
			}
			bool flag5 = binding.depthLoadAction == RenderBufferLoadAction.Clear || Array.IndexOf<RenderBufferLoadAction>(binding.colorLoadActions, RenderBufferLoadAction.Clear) > -1;
			if (flag5)
			{
				throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
			}
			bool flag6 = binding.colorRenderTargets.Length == 1;
			if (flag6)
			{
				this.SetRenderTargetColorDepth_Internal(binding.colorRenderTargets[0], binding.depthRenderTarget, binding.colorLoadActions[0], binding.colorStoreActions[0], binding.depthLoadAction, binding.depthStoreAction, binding.flags);
			}
			else
			{
				this.SetRenderTargetMulti_Internal(binding.colorRenderTargets, binding.depthRenderTarget, binding.colorLoadActions, binding.colorStoreActions, binding.depthLoadAction, binding.depthStoreAction, binding.flags);
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0003A297 File Offset: 0x00038497
		private void SetRenderTargetSingle_Internal(RenderTargetIdentifier rt, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
		{
			this.SetRenderTargetSingle_Internal_Injected(ref rt, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction);
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x0003A2A7 File Offset: 0x000384A7
		private void SetRenderTargetColorDepth_Internal(RenderTargetIdentifier color, RenderTargetIdentifier depth, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, RenderTargetFlags flags)
		{
			this.SetRenderTargetColorDepth_Internal_Injected(ref color, ref depth, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction, flags);
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x0003A2BC File Offset: 0x000384BC
		private void SetRenderTargetMulti_Internal(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, RenderTargetFlags flags)
		{
			this.SetRenderTargetMulti_Internal_Injected(colors, ref depth, colorLoadActions, colorStoreActions, depthLoadAction, depthStoreAction, flags);
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0003A2D0 File Offset: 0x000384D0
		private void SetRenderTargetColorDepthSubtarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			this.SetRenderTargetColorDepthSubtarget_Injected(ref color, ref depth, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction, mipLevel, cubemapFace, depthSlice);
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x0003A2F4 File Offset: 0x000384F4
		private void SetRenderTargetMultiSubtarget(RenderTargetIdentifier[] colors, RenderTargetIdentifier depth, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, int mipLevel, CubemapFace cubemapFace, int depthSlice)
		{
			this.SetRenderTargetMultiSubtarget_Injected(colors, ref depth, colorLoadActions, colorStoreActions, depthLoadAction, depthStoreAction, mipLevel, cubemapFace, depthSlice);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0003A318 File Offset: 0x00038518
		[NativeMethod("ProcessVTFeedback")]
		private void Internal_ProcessVTFeedback(RenderTargetIdentifier rt, IntPtr resolver, int slice, int x, int width, int y, int height, int mip)
		{
			this.Internal_ProcessVTFeedback_Injected(ref rt, resolver, slice, x, width, y, height, mip);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0003A33C File Offset: 0x0003853C
		[SecuritySafeCritical]
		public void SetBufferData(ComputeBuffer buffer, Array data)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to RenderingCommandBuffer.SetBufferData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			this.InternalSetComputeBufferData(buffer, data, 0, 0, data.Length, UnsafeUtility.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x0003A3A4 File Offset: 0x000385A4
		[SecuritySafeCritical]
		public void SetBufferData<T>(ComputeBuffer buffer, List<T> data) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to RenderingCommandBuffer.SetBufferData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			this.InternalSetComputeBufferData(buffer, NoAllocHelpers.ExtractArrayFromList(data), 0, 0, NoAllocHelpers.SafeLength<T>(data), Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x0003A416 File Offset: 0x00038616
		[SecuritySafeCritical]
		public void SetBufferData<T>(ComputeBuffer buffer, NativeArray<T> data) where T : struct
		{
			this.InternalSetComputeBufferNativeData(buffer, (IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, 0, data.Length, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0003A440 File Offset: 0x00038640
		[SecuritySafeCritical]
		public void SetBufferData(ComputeBuffer buffer, Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to RenderingCommandBuffer.SetBufferData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			bool flag3 = managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetComputeBufferData(buffer, data, managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0003A4E8 File Offset: 0x000386E8
		[SecuritySafeCritical]
		public void SetBufferData<T>(ComputeBuffer buffer, List<T> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to RenderingCommandBuffer.SetBufferData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag3 = managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Count;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetComputeBufferData(buffer, NoAllocHelpers.ExtractArrayFromList(data), managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0003A59C File Offset: 0x0003879C
		[SecuritySafeCritical]
		public void SetBufferData<T>(ComputeBuffer buffer, NativeArray<T> data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			bool flag = nativeBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || nativeBufferStartIndex + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (nativeBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", nativeBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetComputeBufferNativeData(buffer, (IntPtr)data.GetUnsafeReadOnlyPtr<T>(), nativeBufferStartIndex, graphicsBufferStartIndex, count, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0003A610 File Offset: 0x00038810
		public void SetBufferCounterValue(ComputeBuffer buffer, uint counterValue)
		{
			this.InternalSetComputeBufferCounterValue(buffer, counterValue);
		}

		// Token: 0x0600232F RID: 9007
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::InternalSetGraphicsBufferNativeData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetComputeBufferNativeData([NotNull("ArgumentNullException")] ComputeBuffer buffer, IntPtr data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

		// Token: 0x06002330 RID: 9008
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::InternalSetGraphicsBufferData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetComputeBufferData([NotNull("ArgumentNullException")] ComputeBuffer buffer, Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

		// Token: 0x06002331 RID: 9009
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::InternalSetGraphicsBufferCounterValue", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetComputeBufferCounterValue([NotNull("ArgumentNullException")] ComputeBuffer buffer, uint counterValue);

		// Token: 0x06002332 RID: 9010 RVA: 0x0003A61C File Offset: 0x0003881C
		[SecuritySafeCritical]
		public void SetBufferData(GraphicsBuffer buffer, Array data)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to RenderingCommandBuffer.SetBufferData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			this.InternalSetGraphicsBufferData(buffer, data, 0, 0, data.Length, UnsafeUtility.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0003A684 File Offset: 0x00038884
		[SecuritySafeCritical]
		public void SetBufferData<T>(GraphicsBuffer buffer, List<T> data) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to RenderingCommandBuffer.SetBufferData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			this.InternalSetGraphicsBufferData(buffer, NoAllocHelpers.ExtractArrayFromList(data), 0, 0, NoAllocHelpers.SafeLength<T>(data), Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0003A6F6 File Offset: 0x000388F6
		[SecuritySafeCritical]
		public void SetBufferData<T>(GraphicsBuffer buffer, NativeArray<T> data) where T : struct
		{
			this.InternalSetGraphicsBufferNativeData(buffer, (IntPtr)data.GetUnsafeReadOnlyPtr<T>(), 0, 0, data.Length, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0003A720 File Offset: 0x00038920
		[SecuritySafeCritical]
		public void SetBufferData(GraphicsBuffer buffer, Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsArrayBlittable(data);
			if (flag2)
			{
				throw new ArgumentException(string.Format("Array passed to RenderingCommandBuffer.SetBufferData(array) must be blittable.\n{0}", UnsafeUtility.GetReasonForArrayNonBlittable(data)));
			}
			bool flag3 = managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetGraphicsBufferData(buffer, data, managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0003A7C8 File Offset: 0x000389C8
		[SecuritySafeCritical]
		public void SetBufferData<T>(GraphicsBuffer buffer, List<T> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			bool flag = data == null;
			if (flag)
			{
				throw new ArgumentNullException("data");
			}
			bool flag2 = !UnsafeUtility.IsGenericListBlittable<T>();
			if (flag2)
			{
				throw new ArgumentException(string.Format("List<{0}> passed to RenderingCommandBuffer.SetBufferData(List<>) must be blittable.\n{1}", typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
			}
			bool flag3 = managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Count;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetGraphicsBufferData(buffer, NoAllocHelpers.ExtractArrayFromList(data), managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(typeof(T)));
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0003A87C File Offset: 0x00038A7C
		[SecuritySafeCritical]
		public void SetBufferData<T>(GraphicsBuffer buffer, NativeArray<T> data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			bool flag = nativeBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || nativeBufferStartIndex + count > data.Length;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Bad indices/count arguments (nativeBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", nativeBufferStartIndex, graphicsBufferStartIndex, count));
			}
			this.InternalSetGraphicsBufferNativeData(buffer, (IntPtr)data.GetUnsafeReadOnlyPtr<T>(), nativeBufferStartIndex, graphicsBufferStartIndex, count, UnsafeUtility.SizeOf<T>());
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0003A8F0 File Offset: 0x00038AF0
		public void SetBufferCounterValue(GraphicsBuffer buffer, uint counterValue)
		{
			this.InternalSetGraphicsBufferCounterValue(buffer, counterValue);
		}

		// Token: 0x06002339 RID: 9017
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::InternalSetGraphicsBufferNativeData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetGraphicsBufferNativeData([NotNull("ArgumentNullException")] GraphicsBuffer buffer, IntPtr data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

		// Token: 0x0600233A RID: 9018
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::InternalSetGraphicsBufferData", HasExplicitThis = true, ThrowsException = true)]
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetGraphicsBufferData([NotNull("ArgumentNullException")] GraphicsBuffer buffer, Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

		// Token: 0x0600233B RID: 9019
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::InternalSetGraphicsBufferCounterValue", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalSetGraphicsBufferCounterValue([NotNull("ArgumentNullException")] GraphicsBuffer buffer, uint counterValue);

		// Token: 0x0600233C RID: 9020
		[FreeFunction(Name = "RenderingCommandBuffer_Bindings::CopyBuffer", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyBufferImpl([NotNull("ArgumentNullException")] GraphicsBuffer source, [NotNull("ArgumentNullException")] GraphicsBuffer dest);

		// Token: 0x0600233D RID: 9021 RVA: 0x0003A8FC File Offset: 0x00038AFC
		~CommandBuffer()
		{
			this.Dispose(false);
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0003A930 File Offset: 0x00038B30
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0003A942 File Offset: 0x00038B42
		private void Dispose(bool disposing)
		{
			this.ReleaseBuffer();
			this.m_Ptr = IntPtr.Zero;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0003A957 File Offset: 0x00038B57
		public CommandBuffer()
		{
			this.m_Ptr = CommandBuffer.InitBuffer();
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0003A96C File Offset: 0x00038B6C
		public void Release()
		{
			this.Dispose();
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x0003A978 File Offset: 0x00038B78
		public GraphicsFence CreateAsyncGraphicsFence()
		{
			return this.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x0003A994 File Offset: 0x00038B94
		public GraphicsFence CreateAsyncGraphicsFence(SynchronisationStage stage)
		{
			return this.CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x0003A9B4 File Offset: 0x00038BB4
		public GraphicsFence CreateGraphicsFence(GraphicsFenceType fenceType, SynchronisationStageFlags stage)
		{
			GraphicsFence graphicsFence = default(GraphicsFence);
			graphicsFence.m_FenceType = fenceType;
			graphicsFence.m_Ptr = this.CreateGPUFence_Internal(fenceType, stage);
			graphicsFence.InitPostAllocation();
			graphicsFence.Validate();
			return graphicsFence;
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0003A9F6 File Offset: 0x00038BF6
		public void WaitOnAsyncGraphicsFence(GraphicsFence fence)
		{
			this.WaitOnAsyncGraphicsFence(fence, SynchronisationStage.VertexProcessing);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0003AA02 File Offset: 0x00038C02
		public void WaitOnAsyncGraphicsFence(GraphicsFence fence, SynchronisationStage stage)
		{
			this.WaitOnAsyncGraphicsFence(fence, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x0003AA14 File Offset: 0x00038C14
		public void WaitOnAsyncGraphicsFence(GraphicsFence fence, SynchronisationStageFlags stage)
		{
			bool flag = fence.m_FenceType > GraphicsFenceType.AsyncQueueSynchronisation;
			if (flag)
			{
				throw new ArgumentException("Attempting to call WaitOnAsyncGPUFence on a fence that is not of GraphicsFenceType.AsyncQueueSynchronization");
			}
			fence.Validate();
			bool flag2 = fence.IsFencePending();
			if (flag2)
			{
				this.WaitOnGPUFence_Internal(fence.m_Ptr, stage);
			}
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x0003AA5B File Offset: 0x00038C5B
		public void SetComputeFloatParam(ComputeShader computeShader, string name, float val)
		{
			this.SetComputeFloatParam(computeShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0003AA6D File Offset: 0x00038C6D
		public void SetComputeIntParam(ComputeShader computeShader, string name, int val)
		{
			this.SetComputeIntParam(computeShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0003AA7F File Offset: 0x00038C7F
		public void SetComputeVectorParam(ComputeShader computeShader, string name, Vector4 val)
		{
			this.SetComputeVectorParam(computeShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0003AA91 File Offset: 0x00038C91
		public void SetComputeVectorArrayParam(ComputeShader computeShader, string name, Vector4[] values)
		{
			this.SetComputeVectorArrayParam(computeShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0003AAA3 File Offset: 0x00038CA3
		public void SetComputeMatrixParam(ComputeShader computeShader, string name, Matrix4x4 val)
		{
			this.SetComputeMatrixParam(computeShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0003AAB5 File Offset: 0x00038CB5
		public void SetComputeMatrixArrayParam(ComputeShader computeShader, string name, Matrix4x4[] values)
		{
			this.SetComputeMatrixArrayParam(computeShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0003AAC7 File Offset: 0x00038CC7
		public void SetComputeFloatParams(ComputeShader computeShader, string name, params float[] values)
		{
			this.Internal_SetComputeFloats(computeShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0003AAD9 File Offset: 0x00038CD9
		public void SetComputeFloatParams(ComputeShader computeShader, int nameID, params float[] values)
		{
			this.Internal_SetComputeFloats(computeShader, nameID, values);
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0003AAE6 File Offset: 0x00038CE6
		public void SetComputeIntParams(ComputeShader computeShader, string name, params int[] values)
		{
			this.Internal_SetComputeInts(computeShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0003AAF8 File Offset: 0x00038CF8
		public void SetComputeIntParams(ComputeShader computeShader, int nameID, params int[] values)
		{
			this.Internal_SetComputeInts(computeShader, nameID, values);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0003AB05 File Offset: 0x00038D05
		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, RenderTargetIdentifier rt)
		{
			this.Internal_SetComputeTextureParam(computeShader, kernelIndex, Shader.PropertyToID(name), ref rt, 0, RenderTextureSubElement.Default);
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0003AB1B File Offset: 0x00038D1B
		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, int nameID, RenderTargetIdentifier rt)
		{
			this.Internal_SetComputeTextureParam(computeShader, kernelIndex, nameID, ref rt, 0, RenderTextureSubElement.Default);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0003AB2C File Offset: 0x00038D2C
		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, RenderTargetIdentifier rt, int mipLevel)
		{
			this.Internal_SetComputeTextureParam(computeShader, kernelIndex, Shader.PropertyToID(name), ref rt, mipLevel, RenderTextureSubElement.Default);
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0003AB43 File Offset: 0x00038D43
		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, int nameID, RenderTargetIdentifier rt, int mipLevel)
		{
			this.Internal_SetComputeTextureParam(computeShader, kernelIndex, nameID, ref rt, mipLevel, RenderTextureSubElement.Default);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0003AB55 File Offset: 0x00038D55
		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, RenderTargetIdentifier rt, int mipLevel, RenderTextureSubElement element)
		{
			this.Internal_SetComputeTextureParam(computeShader, kernelIndex, Shader.PropertyToID(name), ref rt, mipLevel, element);
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0003AB6D File Offset: 0x00038D6D
		public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, int nameID, RenderTargetIdentifier rt, int mipLevel, RenderTextureSubElement element)
		{
			this.Internal_SetComputeTextureParam(computeShader, kernelIndex, nameID, ref rt, mipLevel, element);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0003AB80 File Offset: 0x00038D80
		public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, int nameID, ComputeBuffer buffer)
		{
			this.Internal_SetComputeBufferParam(computeShader, kernelIndex, nameID, buffer);
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0003AB8F File Offset: 0x00038D8F
		public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, string name, ComputeBuffer buffer)
		{
			this.Internal_SetComputeBufferParam(computeShader, kernelIndex, Shader.PropertyToID(name), buffer);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0003ABA3 File Offset: 0x00038DA3
		public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, int nameID, GraphicsBufferHandle bufferHandle)
		{
			this.Internal_SetComputeGraphicsBufferHandleParam(computeShader, kernelIndex, nameID, bufferHandle);
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0003ABB2 File Offset: 0x00038DB2
		public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, string name, GraphicsBufferHandle bufferHandle)
		{
			this.Internal_SetComputeGraphicsBufferHandleParam(computeShader, kernelIndex, Shader.PropertyToID(name), bufferHandle);
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0003ABC6 File Offset: 0x00038DC6
		public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, int nameID, GraphicsBuffer buffer)
		{
			this.Internal_SetComputeGraphicsBufferParam(computeShader, kernelIndex, nameID, buffer);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0003ABD5 File Offset: 0x00038DD5
		public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, string name, GraphicsBuffer buffer)
		{
			this.Internal_SetComputeGraphicsBufferParam(computeShader, kernelIndex, Shader.PropertyToID(name), buffer);
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0003ABE9 File Offset: 0x00038DE9
		public void SetComputeConstantBufferParam(ComputeShader computeShader, int nameID, ComputeBuffer buffer, int offset, int size)
		{
			this.Internal_SetComputeConstantComputeBufferParam(computeShader, nameID, buffer, offset, size);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0003ABFA File Offset: 0x00038DFA
		public void SetComputeConstantBufferParam(ComputeShader computeShader, string name, ComputeBuffer buffer, int offset, int size)
		{
			this.Internal_SetComputeConstantComputeBufferParam(computeShader, Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0003AC10 File Offset: 0x00038E10
		public void SetComputeConstantBufferParam(ComputeShader computeShader, int nameID, GraphicsBuffer buffer, int offset, int size)
		{
			this.Internal_SetComputeConstantGraphicsBufferParam(computeShader, nameID, buffer, offset, size);
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0003AC21 File Offset: 0x00038E21
		public void SetComputeConstantBufferParam(ComputeShader computeShader, string name, GraphicsBuffer buffer, int offset, int size)
		{
			this.Internal_SetComputeConstantGraphicsBufferParam(computeShader, Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0003AC37 File Offset: 0x00038E37
		public void SetComputeParamsFromMaterial(ComputeShader computeShader, int kernelIndex, Material material)
		{
			this.Internal_SetComputeParamsFromMaterial(computeShader, kernelIndex, material);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0003AC44 File Offset: 0x00038E44
		public void DispatchCompute(ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ)
		{
			this.Internal_DispatchCompute(computeShader, kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0003AC58 File Offset: 0x00038E58
		public void DispatchCompute(ComputeShader computeShader, int kernelIndex, ComputeBuffer indirectBuffer, uint argsOffset)
		{
			bool flag = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			this.Internal_DispatchComputeIndirect(computeShader, kernelIndex, indirectBuffer, argsOffset);
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0003AC98 File Offset: 0x00038E98
		public void DispatchCompute(ComputeShader computeShader, int kernelIndex, GraphicsBuffer indirectBuffer, uint argsOffset)
		{
			bool flag = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			this.Internal_DispatchComputeIndirectGraphicsBuffer(computeShader, kernelIndex, indirectBuffer, argsOffset);
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0003ACD8 File Offset: 0x00038ED8
		public void BuildRayTracingAccelerationStructure(RayTracingAccelerationStructure accelerationStructure)
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			this.Internal_BuildRayTracingAccelerationStructure(accelerationStructure, vector);
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0003AD05 File Offset: 0x00038F05
		public void BuildRayTracingAccelerationStructure(RayTracingAccelerationStructure accelerationStructure, Vector3 relativeOrigin)
		{
			this.Internal_BuildRayTracingAccelerationStructure(accelerationStructure, relativeOrigin);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0003AD11 File Offset: 0x00038F11
		public void SetRayTracingAccelerationStructure(RayTracingShader rayTracingShader, string name, RayTracingAccelerationStructure rayTracingAccelerationStructure)
		{
			this.Internal_SetRayTracingAccelerationStructure(rayTracingShader, Shader.PropertyToID(name), rayTracingAccelerationStructure);
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0003AD23 File Offset: 0x00038F23
		public void SetRayTracingAccelerationStructure(RayTracingShader rayTracingShader, int nameID, RayTracingAccelerationStructure rayTracingAccelerationStructure)
		{
			this.Internal_SetRayTracingAccelerationStructure(rayTracingShader, nameID, rayTracingAccelerationStructure);
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x0003AD30 File Offset: 0x00038F30
		public void SetRayTracingBufferParam(RayTracingShader rayTracingShader, string name, ComputeBuffer buffer)
		{
			this.Internal_SetRayTracingBufferParam(rayTracingShader, Shader.PropertyToID(name), buffer);
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0003AD42 File Offset: 0x00038F42
		public void SetRayTracingBufferParam(RayTracingShader rayTracingShader, int nameID, ComputeBuffer buffer)
		{
			this.Internal_SetRayTracingBufferParam(rayTracingShader, nameID, buffer);
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0003AD4F File Offset: 0x00038F4F
		public void SetRayTracingConstantBufferParam(RayTracingShader rayTracingShader, int nameID, ComputeBuffer buffer, int offset, int size)
		{
			this.Internal_SetRayTracingConstantComputeBufferParam(rayTracingShader, nameID, buffer, offset, size);
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0003AD60 File Offset: 0x00038F60
		public void SetRayTracingConstantBufferParam(RayTracingShader rayTracingShader, string name, ComputeBuffer buffer, int offset, int size)
		{
			this.Internal_SetRayTracingConstantComputeBufferParam(rayTracingShader, Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0003AD76 File Offset: 0x00038F76
		public void SetRayTracingConstantBufferParam(RayTracingShader rayTracingShader, int nameID, GraphicsBuffer buffer, int offset, int size)
		{
			this.Internal_SetRayTracingConstantGraphicsBufferParam(rayTracingShader, nameID, buffer, offset, size);
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0003AD87 File Offset: 0x00038F87
		public void SetRayTracingConstantBufferParam(RayTracingShader rayTracingShader, string name, GraphicsBuffer buffer, int offset, int size)
		{
			this.Internal_SetRayTracingConstantGraphicsBufferParam(rayTracingShader, Shader.PropertyToID(name), buffer, offset, size);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0003AD9D File Offset: 0x00038F9D
		public void SetRayTracingTextureParam(RayTracingShader rayTracingShader, string name, RenderTargetIdentifier rt)
		{
			this.Internal_SetRayTracingTextureParam(rayTracingShader, Shader.PropertyToID(name), ref rt);
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0003ADB0 File Offset: 0x00038FB0
		public void SetRayTracingTextureParam(RayTracingShader rayTracingShader, int nameID, RenderTargetIdentifier rt)
		{
			this.Internal_SetRayTracingTextureParam(rayTracingShader, nameID, ref rt);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0003ADBE File Offset: 0x00038FBE
		public void SetRayTracingFloatParam(RayTracingShader rayTracingShader, string name, float val)
		{
			this.Internal_SetRayTracingFloatParam(rayTracingShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0003ADD0 File Offset: 0x00038FD0
		public void SetRayTracingFloatParam(RayTracingShader rayTracingShader, int nameID, float val)
		{
			this.Internal_SetRayTracingFloatParam(rayTracingShader, nameID, val);
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0003ADDD File Offset: 0x00038FDD
		public void SetRayTracingFloatParams(RayTracingShader rayTracingShader, string name, params float[] values)
		{
			this.Internal_SetRayTracingFloats(rayTracingShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0003ADEF File Offset: 0x00038FEF
		public void SetRayTracingFloatParams(RayTracingShader rayTracingShader, int nameID, params float[] values)
		{
			this.Internal_SetRayTracingFloats(rayTracingShader, nameID, values);
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0003ADFC File Offset: 0x00038FFC
		public void SetRayTracingIntParam(RayTracingShader rayTracingShader, string name, int val)
		{
			this.Internal_SetRayTracingIntParam(rayTracingShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0003AE0E File Offset: 0x0003900E
		public void SetRayTracingIntParam(RayTracingShader rayTracingShader, int nameID, int val)
		{
			this.Internal_SetRayTracingIntParam(rayTracingShader, nameID, val);
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0003AE1B File Offset: 0x0003901B
		public void SetRayTracingIntParams(RayTracingShader rayTracingShader, string name, params int[] values)
		{
			this.Internal_SetRayTracingInts(rayTracingShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0003AE2D File Offset: 0x0003902D
		public void SetRayTracingIntParams(RayTracingShader rayTracingShader, int nameID, params int[] values)
		{
			this.Internal_SetRayTracingInts(rayTracingShader, nameID, values);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0003AE3A File Offset: 0x0003903A
		public void SetRayTracingVectorParam(RayTracingShader rayTracingShader, string name, Vector4 val)
		{
			this.Internal_SetRayTracingVectorParam(rayTracingShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0003AE4C File Offset: 0x0003904C
		public void SetRayTracingVectorParam(RayTracingShader rayTracingShader, int nameID, Vector4 val)
		{
			this.Internal_SetRayTracingVectorParam(rayTracingShader, nameID, val);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0003AE59 File Offset: 0x00039059
		public void SetRayTracingVectorArrayParam(RayTracingShader rayTracingShader, string name, params Vector4[] values)
		{
			this.Internal_SetRayTracingVectorArrayParam(rayTracingShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0003AE6B File Offset: 0x0003906B
		public void SetRayTracingVectorArrayParam(RayTracingShader rayTracingShader, int nameID, params Vector4[] values)
		{
			this.Internal_SetRayTracingVectorArrayParam(rayTracingShader, nameID, values);
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0003AE78 File Offset: 0x00039078
		public void SetRayTracingMatrixParam(RayTracingShader rayTracingShader, string name, Matrix4x4 val)
		{
			this.Internal_SetRayTracingMatrixParam(rayTracingShader, Shader.PropertyToID(name), val);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0003AE8A File Offset: 0x0003908A
		public void SetRayTracingMatrixParam(RayTracingShader rayTracingShader, int nameID, Matrix4x4 val)
		{
			this.Internal_SetRayTracingMatrixParam(rayTracingShader, nameID, val);
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0003AE97 File Offset: 0x00039097
		public void SetRayTracingMatrixArrayParam(RayTracingShader rayTracingShader, string name, params Matrix4x4[] values)
		{
			this.Internal_SetRayTracingMatrixArrayParam(rayTracingShader, Shader.PropertyToID(name), values);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x0003AEA9 File Offset: 0x000390A9
		public void SetRayTracingMatrixArrayParam(RayTracingShader rayTracingShader, int nameID, params Matrix4x4[] values)
		{
			this.Internal_SetRayTracingMatrixArrayParam(rayTracingShader, nameID, values);
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0003AEB6 File Offset: 0x000390B6
		public void DispatchRays(RayTracingShader rayTracingShader, string rayGenName, uint width, uint height, uint depth, Camera camera = null)
		{
			this.Internal_DispatchRays(rayTracingShader, rayGenName, width, height, depth, camera);
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0003AEC9 File Offset: 0x000390C9
		public void GenerateMips(RenderTargetIdentifier rt)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_GenerateMips(rt);
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x0003AEE0 File Offset: 0x000390E0
		public void GenerateMips(RenderTexture rt)
		{
			bool flag = rt == null;
			if (flag)
			{
				throw new ArgumentNullException("rt");
			}
			this.GenerateMips(new RenderTargetIdentifier(rt));
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0003AF14 File Offset: 0x00039114
		public void ResolveAntiAliasedSurface(RenderTexture rt, RenderTexture target = null)
		{
			bool flag = rt == null;
			if (flag)
			{
				throw new ArgumentNullException("rt");
			}
			this.Internal_ResolveAntiAliasedSurface(rt, target);
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x0003AF44 File Offset: 0x00039144
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			bool flag = mesh == null;
			if (flag)
			{
				throw new ArgumentNullException("mesh");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag2 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag2)
			{
				submeshIndex = Mathf.Clamp(submeshIndex, 0, mesh.subMeshCount - 1);
				Debug.LogWarning(string.Format("submeshIndex out of range. Clampped to {0}.", submeshIndex));
			}
			bool flag3 = material == null;
			if (flag3)
			{
				throw new ArgumentNullException("material");
			}
			this.Internal_DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x0003AFDB File Offset: 0x000391DB
		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass)
		{
			this.DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, null);
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x0003AFED File Offset: 0x000391ED
		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex)
		{
			this.DrawMesh(mesh, matrix, material, submeshIndex, -1);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0003AFFD File Offset: 0x000391FD
		[ExcludeFromDocs]
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material)
		{
			this.DrawMesh(mesh, matrix, material, 0);
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0003B00C File Offset: 0x0003920C
		public void DrawRenderer(Renderer renderer, Material material, [DefaultValue("0")] int submeshIndex, [DefaultValue("-1")] int shaderPass)
		{
			bool flag = renderer == null;
			if (flag)
			{
				throw new ArgumentNullException("renderer");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag2 = submeshIndex < 0;
			if (flag2)
			{
				submeshIndex = Mathf.Max(submeshIndex, 0);
				Debug.LogWarning(string.Format("submeshIndex out of range. Clampped to {0}.", submeshIndex));
			}
			bool flag3 = material == null;
			if (flag3)
			{
				throw new ArgumentNullException("material");
			}
			this.Internal_DrawRenderer(renderer, material, submeshIndex, shaderPass);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0003B084 File Offset: 0x00039284
		[ExcludeFromDocs]
		public void DrawRenderer(Renderer renderer, Material material, int submeshIndex)
		{
			this.DrawRenderer(renderer, material, submeshIndex, -1);
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0003B092 File Offset: 0x00039292
		[ExcludeFromDocs]
		public void DrawRenderer(Renderer renderer, Material material)
		{
			this.DrawRenderer(renderer, material, 0);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0003B09F File Offset: 0x0003929F
		public void DrawRendererList(RendererList rendererList)
		{
			this.Internal_DrawRendererList(rendererList);
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0003B0AC File Offset: 0x000392AC
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, [DefaultValue("1")] int instanceCount, [DefaultValue("null")] MaterialPropertyBlock properties)
		{
			bool flag = material == null;
			if (flag)
			{
				throw new ArgumentNullException("material");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x0003B0EB File Offset: 0x000392EB
		[ExcludeFromDocs]
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount)
		{
			this.DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, null);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0003B0FF File Offset: 0x000392FF
		[ExcludeFromDocs]
		public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount)
		{
			this.DrawProcedural(matrix, material, shaderPass, topology, vertexCount, 1);
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0003B114 File Offset: 0x00039314
		public void DrawProcedural(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount, MaterialPropertyBlock properties)
		{
			bool flag = indexBuffer == null;
			if (flag)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag2 = material == null;
			if (flag2)
			{
				throw new ArgumentNullException("material");
			}
			this.Internal_DrawProceduralIndexed(indexBuffer, matrix, material, shaderPass, topology, indexCount, instanceCount, properties);
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x0003B160 File Offset: 0x00039360
		public void DrawProcedural(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount)
		{
			this.DrawProcedural(indexBuffer, matrix, material, shaderPass, topology, indexCount, instanceCount, null);
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0003B181 File Offset: 0x00039381
		public void DrawProcedural(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount)
		{
			this.DrawProcedural(indexBuffer, matrix, material, shaderPass, topology, indexCount, 1);
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0003B198 File Offset: 0x00039398
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = material == null;
			if (flag2)
			{
				throw new ArgumentNullException("material");
			}
			bool flag3 = bufferWithArgs == null;
			if (flag3)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0003B202 File Offset: 0x00039402
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
		{
			this.DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, null);
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0003B216 File Offset: 0x00039416
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
		{
			this.DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, 0);
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0003B228 File Offset: 0x00039428
		public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = indexBuffer == null;
			if (flag2)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag3 = material == null;
			if (flag3)
			{
				throw new ArgumentNullException("material");
			}
			bool flag4 = bufferWithArgs == null;
			if (flag4)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			this.Internal_DrawProceduralIndexedIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0003B2A0 File Offset: 0x000394A0
		public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
		{
			this.DrawProceduralIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, null);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x0003B2C1 File Offset: 0x000394C1
		public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
		{
			this.DrawProceduralIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, 0);
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0003B2D8 File Offset: 0x000394D8
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = material == null;
			if (flag2)
			{
				throw new ArgumentNullException("material");
			}
			bool flag3 = bufferWithArgs == null;
			if (flag3)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_DrawProceduralIndirectGraphicsBuffer(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0003B342 File Offset: 0x00039542
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset)
		{
			this.DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, null);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0003B356 File Offset: 0x00039556
		public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs)
		{
			this.DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, 0);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0003B368 File Offset: 0x00039568
		public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag2 = indexBuffer == null;
			if (flag2)
			{
				throw new ArgumentNullException("indexBuffer");
			}
			bool flag3 = material == null;
			if (flag3)
			{
				throw new ArgumentNullException("material");
			}
			bool flag4 = bufferWithArgs == null;
			if (flag4)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			this.Internal_DrawProceduralIndexedIndirectGraphicsBuffer(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0003B3E0 File Offset: 0x000395E0
		public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset)
		{
			this.DrawProceduralIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, null);
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0003B401 File Offset: 0x00039601
		public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs)
		{
			this.DrawProceduralIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, 0);
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0003B418 File Offset: 0x00039618
		public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("DrawMeshInstanced is not supported.");
			}
			bool flag2 = mesh == null;
			if (flag2)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag3 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag4 = material == null;
			if (flag4)
			{
				throw new ArgumentNullException("material");
			}
			bool flag5 = matrices == null;
			if (flag5)
			{
				throw new ArgumentNullException("matrices");
			}
			bool flag6 = count < 0 || count > Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length);
			if (flag6)
			{
				throw new ArgumentOutOfRangeException("count", string.Format("Count must be in the range of 0 to {0}.", Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length)));
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag7 = count > 0;
			if (flag7)
			{
				this.Internal_DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, properties);
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x0003B514 File Offset: 0x00039714
		public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count)
		{
			this.DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, null);
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0003B528 File Offset: 0x00039728
		public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices)
		{
			this.DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, matrices.Length);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0003B540 File Offset: 0x00039740
		public void DrawMeshInstancedProcedural(Mesh mesh, int submeshIndex, Material material, int shaderPass, int count, MaterialPropertyBlock properties = null)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("DrawMeshInstancedProcedural is not supported.");
			}
			bool flag2 = mesh == null;
			if (flag2)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag3 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag4 = material == null;
			if (flag4)
			{
				throw new ArgumentNullException("material");
			}
			bool flag5 = count <= 0;
			if (flag5)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			bool flag6 = count > 0;
			if (flag6)
			{
				this.Internal_DrawMeshInstancedProcedural(mesh, submeshIndex, material, shaderPass, count, properties);
			}
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0003B5F8 File Offset: 0x000397F8
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag3 = mesh == null;
			if (flag3)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag4 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag5 = material == null;
			if (flag5)
			{
				throw new ArgumentNullException("material");
			}
			bool flag6 = bufferWithArgs == null;
			if (flag6)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			this.Internal_DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0003B6B1 File Offset: 0x000398B1
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset)
		{
			this.DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, null);
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0003B6C5 File Offset: 0x000398C5
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs)
		{
			this.DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, 0, null);
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0003B6D8 File Offset: 0x000398D8
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
		{
			bool flag = !SystemInfo.supportsInstancing;
			if (flag)
			{
				throw new InvalidOperationException("Instancing is not supported.");
			}
			bool flag2 = !SystemInfo.supportsIndirectArgumentsBuffer;
			if (flag2)
			{
				throw new InvalidOperationException("Indirect argument buffers are not supported.");
			}
			bool flag3 = mesh == null;
			if (flag3)
			{
				throw new ArgumentNullException("mesh");
			}
			bool flag4 = submeshIndex < 0 || submeshIndex >= mesh.subMeshCount;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
			}
			bool flag5 = material == null;
			if (flag5)
			{
				throw new ArgumentNullException("material");
			}
			bool flag6 = bufferWithArgs == null;
			if (flag6)
			{
				throw new ArgumentNullException("bufferWithArgs");
			}
			this.Internal_DrawMeshInstancedIndirectGraphicsBuffer(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, properties);
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0003B791 File Offset: 0x00039991
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, GraphicsBuffer bufferWithArgs, int argsOffset)
		{
			this.DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, null);
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0003B7A5 File Offset: 0x000399A5
		public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, GraphicsBuffer bufferWithArgs)
		{
			this.DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, 0, null);
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0003B7B8 File Offset: 0x000399B8
		public void DrawOcclusionMesh(RectInt normalizedCamViewport)
		{
			this.Internal_DrawOcclusionMesh(normalizedCamViewport);
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x0003B7C3 File Offset: 0x000399C3
		public void SetRandomWriteTarget(int index, RenderTargetIdentifier rt)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.SetRandomWriteTarget_Texture(index, ref rt);
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0003B7D9 File Offset: 0x000399D9
		public void SetRandomWriteTarget(int index, ComputeBuffer buffer, bool preserveCounterValue)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.SetRandomWriteTarget_Buffer(index, buffer, preserveCounterValue);
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0003B7EF File Offset: 0x000399EF
		public void SetRandomWriteTarget(int index, ComputeBuffer buffer)
		{
			this.SetRandomWriteTarget(index, buffer, false);
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0003B7FC File Offset: 0x000399FC
		public void SetRandomWriteTarget(int index, GraphicsBuffer buffer, bool preserveCounterValue)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.SetRandomWriteTarget_GraphicsBuffer(index, buffer, preserveCounterValue);
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0003B812 File Offset: 0x00039A12
		public void SetRandomWriteTarget(int index, GraphicsBuffer buffer)
		{
			this.SetRandomWriteTarget(index, buffer, false);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0003B81F File Offset: 0x00039A1F
		public void CopyCounterValue(ComputeBuffer src, ComputeBuffer dst, uint dstOffsetBytes)
		{
			this.CopyCounterValueCC(src, dst, dstOffsetBytes);
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0003B82C File Offset: 0x00039A2C
		public void CopyCounterValue(GraphicsBuffer src, ComputeBuffer dst, uint dstOffsetBytes)
		{
			this.CopyCounterValueGC(src, dst, dstOffsetBytes);
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0003B839 File Offset: 0x00039A39
		public void CopyCounterValue(ComputeBuffer src, GraphicsBuffer dst, uint dstOffsetBytes)
		{
			this.CopyCounterValueCG(src, dst, dstOffsetBytes);
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0003B846 File Offset: 0x00039A46
		public void CopyCounterValue(GraphicsBuffer src, GraphicsBuffer dst, uint dstOffsetBytes)
		{
			this.CopyCounterValueGG(src, dst, dstOffsetBytes);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0003B854 File Offset: 0x00039A54
		public void CopyTexture(RenderTargetIdentifier src, RenderTargetIdentifier dst)
		{
			this.CopyTexture_Internal(ref src, -1, -1, -1, -1, -1, -1, ref dst, -1, -1, -1, -1, 1);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0003B878 File Offset: 0x00039A78
		public void CopyTexture(RenderTargetIdentifier src, int srcElement, RenderTargetIdentifier dst, int dstElement)
		{
			this.CopyTexture_Internal(ref src, srcElement, -1, -1, -1, -1, -1, ref dst, dstElement, -1, -1, -1, 2);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x0003B8A0 File Offset: 0x00039AA0
		public void CopyTexture(RenderTargetIdentifier src, int srcElement, int srcMip, RenderTargetIdentifier dst, int dstElement, int dstMip)
		{
			this.CopyTexture_Internal(ref src, srcElement, srcMip, -1, -1, -1, -1, ref dst, dstElement, dstMip, -1, -1, 3);
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x0003B8C8 File Offset: 0x00039AC8
		public void CopyTexture(RenderTargetIdentifier src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, RenderTargetIdentifier dst, int dstElement, int dstMip, int dstX, int dstY)
		{
			this.CopyTexture_Internal(ref src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, ref dst, dstElement, dstMip, dstX, dstY, 4);
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0003B8F4 File Offset: 0x00039AF4
		public void Blit(Texture source, RenderTargetIdentifier dest)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Texture(source, ref dest, null, -1, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0003B93C File Offset: 0x00039B3C
		public void Blit(Texture source, RenderTargetIdentifier dest, Vector2 scale, Vector2 offset)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Texture(source, ref dest, null, -1, scale, offset, Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0003B968 File Offset: 0x00039B68
		public void Blit(Texture source, RenderTargetIdentifier dest, Material mat)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Texture(source, ref dest, mat, -1, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0003B9B0 File Offset: 0x00039BB0
		public void Blit(Texture source, RenderTargetIdentifier dest, Material mat, int pass)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Texture(source, ref dest, mat, pass, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0003B9F8 File Offset: 0x00039BF8
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, null, -1, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0003BA40 File Offset: 0x00039C40
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Vector2 scale, Vector2 offset)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, null, -1, scale, offset, Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0003BA70 File Offset: 0x00039C70
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, mat, -1, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0003BAB8 File Offset: 0x00039CB8
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, mat, pass, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, 0);
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0003BB04 File Offset: 0x00039D04
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, int sourceDepthSlice, int destDepthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, null, -1, new Vector2(1f, 1f), new Vector2(0f, 0f), sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x0003BB4C File Offset: 0x00039D4C
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, null, -1, scale, offset, sourceDepthSlice, destDepthSlice);
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0003BB78 File Offset: 0x00039D78
		public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass, int destDepthSlice)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Blit_Identifier(ref source, ref dest, mat, pass, new Vector2(1f, 1f), new Vector2(0f, 0f), Texture2DArray.allSlices, destDepthSlice);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x0003BBC2 File Offset: 0x00039DC2
		public void SetGlobalFloat(string name, float value)
		{
			this.SetGlobalFloat(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0003BBD3 File Offset: 0x00039DD3
		public void SetGlobalInt(string name, int value)
		{
			this.SetGlobalInt(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x0003BBE4 File Offset: 0x00039DE4
		public void SetGlobalInteger(string name, int value)
		{
			this.SetGlobalInteger(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x0003BBF5 File Offset: 0x00039DF5
		public void SetGlobalVector(string name, Vector4 value)
		{
			this.SetGlobalVector(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0003BC06 File Offset: 0x00039E06
		public void SetGlobalColor(string name, Color value)
		{
			this.SetGlobalColor(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0003BC17 File Offset: 0x00039E17
		public void SetGlobalMatrix(string name, Matrix4x4 value)
		{
			this.SetGlobalMatrix(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x0003BC28 File Offset: 0x00039E28
		public void SetGlobalFloatArray(string propertyName, List<float> values)
		{
			this.SetGlobalFloatArray(Shader.PropertyToID(propertyName), values);
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0003BC3C File Offset: 0x00039E3C
		public void SetGlobalFloatArray(int nameID, List<float> values)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			bool flag2 = values.Count == 0;
			if (flag2)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			this.SetGlobalFloatArrayListImpl(nameID, values);
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0003BC7E File Offset: 0x00039E7E
		public void SetGlobalFloatArray(string propertyName, float[] values)
		{
			this.SetGlobalFloatArray(Shader.PropertyToID(propertyName), values);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0003BC8F File Offset: 0x00039E8F
		public void SetGlobalVectorArray(string propertyName, List<Vector4> values)
		{
			this.SetGlobalVectorArray(Shader.PropertyToID(propertyName), values);
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0003BCA0 File Offset: 0x00039EA0
		public void SetGlobalVectorArray(int nameID, List<Vector4> values)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			bool flag2 = values.Count == 0;
			if (flag2)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			this.SetGlobalVectorArrayListImpl(nameID, values);
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0003BCE2 File Offset: 0x00039EE2
		public void SetGlobalVectorArray(string propertyName, Vector4[] values)
		{
			this.SetGlobalVectorArray(Shader.PropertyToID(propertyName), values);
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x0003BCF3 File Offset: 0x00039EF3
		public void SetGlobalMatrixArray(string propertyName, List<Matrix4x4> values)
		{
			this.SetGlobalMatrixArray(Shader.PropertyToID(propertyName), values);
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0003BD04 File Offset: 0x00039F04
		public void SetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
		{
			bool flag = values == null;
			if (flag)
			{
				throw new ArgumentNullException("values");
			}
			bool flag2 = values.Count == 0;
			if (flag2)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			this.SetGlobalMatrixArrayListImpl(nameID, values);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x0003BD46 File Offset: 0x00039F46
		public void SetGlobalMatrixArray(string propertyName, Matrix4x4[] values)
		{
			this.SetGlobalMatrixArray(Shader.PropertyToID(propertyName), values);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x0003BD57 File Offset: 0x00039F57
		public void SetGlobalTexture(string name, RenderTargetIdentifier value)
		{
			this.SetGlobalTexture(Shader.PropertyToID(name), value, RenderTextureSubElement.Default);
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0003BD69 File Offset: 0x00039F69
		public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
		{
			this.SetGlobalTexture_Impl(nameID, ref value, RenderTextureSubElement.Default);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0003BD77 File Offset: 0x00039F77
		public void SetGlobalTexture(string name, RenderTargetIdentifier value, RenderTextureSubElement element)
		{
			this.SetGlobalTexture(Shader.PropertyToID(name), value, element);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0003BD89 File Offset: 0x00039F89
		public void SetGlobalTexture(int nameID, RenderTargetIdentifier value, RenderTextureSubElement element)
		{
			this.SetGlobalTexture_Impl(nameID, ref value, element);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0003BD97 File Offset: 0x00039F97
		public void SetGlobalBuffer(string name, ComputeBuffer value)
		{
			this.SetGlobalBufferInternal(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0003BDA8 File Offset: 0x00039FA8
		public void SetGlobalBuffer(int nameID, ComputeBuffer value)
		{
			this.SetGlobalBufferInternal(nameID, value);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x0003BDB4 File Offset: 0x00039FB4
		public void SetGlobalBuffer(string name, GraphicsBuffer value)
		{
			this.SetGlobalGraphicsBufferInternal(Shader.PropertyToID(name), value);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0003BDC5 File Offset: 0x00039FC5
		public void SetGlobalBuffer(int nameID, GraphicsBuffer value)
		{
			this.SetGlobalGraphicsBufferInternal(nameID, value);
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0003BDD1 File Offset: 0x00039FD1
		public void SetGlobalConstantBuffer(ComputeBuffer buffer, int nameID, int offset, int size)
		{
			this.SetGlobalConstantBufferInternal(buffer, nameID, offset, size);
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x0003BDE0 File Offset: 0x00039FE0
		public void SetGlobalConstantBuffer(ComputeBuffer buffer, string name, int offset, int size)
		{
			this.SetGlobalConstantBufferInternal(buffer, Shader.PropertyToID(name), offset, size);
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x0003BDF4 File Offset: 0x00039FF4
		public void SetGlobalConstantBuffer(GraphicsBuffer buffer, int nameID, int offset, int size)
		{
			this.SetGlobalConstantGraphicsBufferInternal(buffer, nameID, offset, size);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x0003BE03 File Offset: 0x0003A003
		public void SetGlobalConstantBuffer(GraphicsBuffer buffer, string name, int offset, int size)
		{
			this.SetGlobalConstantGraphicsBufferInternal(buffer, Shader.PropertyToID(name), offset, size);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x0003BE17 File Offset: 0x0003A017
		public void SetShadowSamplingMode(RenderTargetIdentifier shadowmap, ShadowSamplingMode mode)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.SetShadowSamplingMode_Impl(ref shadowmap, mode);
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x0003BE2D File Offset: 0x0003A02D
		public void SetSinglePassStereo(SinglePassStereoMode mode)
		{
			this.Internal_SetSinglePassStereo(mode);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0003BE38 File Offset: 0x0003A038
		public void IssuePluginEvent(IntPtr callback, int eventID)
		{
			bool flag = callback == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Null callback specified.");
			}
			this.IssuePluginEventInternal(callback, eventID);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x0003BE6C File Offset: 0x0003A06C
		public void IssuePluginEventAndData(IntPtr callback, int eventID, IntPtr data)
		{
			bool flag = callback == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Null callback specified.");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.IssuePluginEventAndDataInternal(callback, eventID, data);
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0003BEA8 File Offset: 0x0003A0A8
		public void IssuePluginEventAndDataWithFlags(IntPtr callback, int eventID, CustomMarkerCallbackFlags flags, IntPtr data)
		{
			bool flag = callback == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Null callback specified.");
			}
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.IssuePluginEventAndDataInternalWithFlags(callback, eventID, flags, data);
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x0003BEE5 File Offset: 0x0003A0E5
		public void IssuePluginCustomBlit(IntPtr callback, uint command, RenderTargetIdentifier source, RenderTargetIdentifier dest, uint commandParam, uint commandFlags)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.IssuePluginCustomBlitInternal(callback, command, ref source, ref dest, commandParam, commandFlags);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x0003BF02 File Offset: 0x0003A102
		[Obsolete("Use IssuePluginCustomTextureUpdateV2 to register TextureUpdate callbacks instead. Callbacks will be passed event IDs kUnityRenderingExtEventUpdateTextureBeginV2 or kUnityRenderingExtEventUpdateTextureEndV2, and data parameter of type UnityRenderingExtTextureUpdateParamsV2.", false)]
		public void IssuePluginCustomTextureUpdate(IntPtr callback, Texture targetTexture, uint userData)
		{
			this.IssuePluginCustomTextureUpdateInternal(callback, targetTexture, userData, false);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x0003BF02 File Offset: 0x0003A102
		[Obsolete("Use IssuePluginCustomTextureUpdateV2 to register TextureUpdate callbacks instead. Callbacks will be passed event IDs kUnityRenderingExtEventUpdateTextureBeginV2 or kUnityRenderingExtEventUpdateTextureEndV2, and data parameter of type UnityRenderingExtTextureUpdateParamsV2.", false)]
		public void IssuePluginCustomTextureUpdateV1(IntPtr callback, Texture targetTexture, uint userData)
		{
			this.IssuePluginCustomTextureUpdateInternal(callback, targetTexture, userData, false);
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x0003BF10 File Offset: 0x0003A110
		public void IssuePluginCustomTextureUpdateV2(IntPtr callback, Texture targetTexture, uint userData)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.IssuePluginCustomTextureUpdateInternal(callback, targetTexture, userData, true);
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x0003BF28 File Offset: 0x0003A128
		public void ProcessVTFeedback(RenderTargetIdentifier rt, IntPtr resolver, int slice, int x, int width, int y, int height, int mip)
		{
			this.ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
			this.Internal_ProcessVTFeedback(rt, resolver, slice, x, width, y, height, mip);
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0003BF53 File Offset: 0x0003A153
		public void CopyBuffer(GraphicsBuffer source, GraphicsBuffer dest)
		{
			Graphics.ValidateCopyBuffer(source, dest);
			this.CopyBufferImpl(source, dest);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x0003BF68 File Offset: 0x0003A168
		[Obsolete("CommandBuffer.CreateGPUFence has been deprecated. Use CreateGraphicsFence instead (UnityUpgradable) -> CreateAsyncGraphicsFence(*)", false)]
		public GPUFence CreateGPUFence(SynchronisationStage stage)
		{
			return default(GPUFence);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x0003BF84 File Offset: 0x0003A184
		[Obsolete("CommandBuffer.CreateGPUFence has been deprecated. Use CreateGraphicsFence instead (UnityUpgradable) -> CreateAsyncGraphicsFence()", false)]
		public GPUFence CreateGPUFence()
		{
			return default(GPUFence);
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("CommandBuffer.WaitOnGPUFence has been deprecated. Use WaitOnGraphicsFence instead (UnityUpgradable) -> WaitOnAsyncGraphicsFence(*)", false)]
		public void WaitOnGPUFence(GPUFence fence, SynchronisationStage stage)
		{
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("CommandBuffer.WaitOnGPUFence has been deprecated. Use WaitOnGraphicsFence instead (UnityUpgradable) -> WaitOnAsyncGraphicsFence(*)", false)]
		public void WaitOnGPUFence(GPUFence fence)
		{
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0003BF9F File Offset: 0x0003A19F
		[Obsolete("CommandBuffer.SetComputeBufferData has been deprecated. Use SetBufferData instead (UnityUpgradable) -> SetBufferData(*)", false)]
		public void SetComputeBufferData(ComputeBuffer buffer, Array data)
		{
			this.SetBufferData(buffer, data);
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x0003BFAB File Offset: 0x0003A1AB
		[Obsolete("CommandBuffer.SetComputeBufferData has been deprecated. Use SetBufferData instead (UnityUpgradable) -> SetBufferData<T>(*)", false)]
		public void SetComputeBufferData<T>(ComputeBuffer buffer, List<T> data) where T : struct
		{
			this.SetBufferData<T>(buffer, data);
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x0003BFB7 File Offset: 0x0003A1B7
		[Obsolete("CommandBuffer.SetComputeBufferData has been deprecated. Use SetBufferData instead (UnityUpgradable) -> SetBufferData<T>(*)", false)]
		public void SetComputeBufferData<T>(ComputeBuffer buffer, NativeArray<T> data) where T : struct
		{
			this.SetBufferData<T>(buffer, data);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x0003BFC3 File Offset: 0x0003A1C3
		[Obsolete("CommandBuffer.SetComputeBufferData has been deprecated. Use SetBufferData instead (UnityUpgradable) -> SetBufferData(*)", false)]
		public void SetComputeBufferData(ComputeBuffer buffer, Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
		{
			this.SetBufferData(buffer, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0003BFD4 File Offset: 0x0003A1D4
		[Obsolete("CommandBuffer.SetComputeBufferData has been deprecated. Use SetBufferData instead (UnityUpgradable) -> SetBufferData<T>(*)", false)]
		public void SetComputeBufferData<T>(ComputeBuffer buffer, List<T> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			this.SetBufferData<T>(buffer, data, managedBufferStartIndex, graphicsBufferStartIndex, count);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0003BFE5 File Offset: 0x0003A1E5
		[Obsolete("CommandBuffer.SetComputeBufferData has been deprecated. Use SetBufferData instead (UnityUpgradable) -> SetBufferData<T>(*)", false)]
		public void SetComputeBufferData<T>(ComputeBuffer buffer, NativeArray<T> data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
		{
			this.SetBufferData<T>(buffer, data, nativeBufferStartIndex, graphicsBufferStartIndex, count);
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0003BFF6 File Offset: 0x0003A1F6
		[Obsolete("CommandBuffer.SetComputeBufferCounterValue has been deprecated. Use SetBufferCounterValue instead (UnityUpgradable) -> SetBufferCounterValue(*)", false)]
		public void SetComputeBufferCounterValue(ComputeBuffer buffer, uint counterValue)
		{
			this.SetBufferCounterValue(buffer, counterValue);
		}

		// Token: 0x060023F4 RID: 9204
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ConvertTexture_Internal_Injected(ref RenderTargetIdentifier src, int srcElement, ref RenderTargetIdentifier dst, int dstElement);

		// Token: 0x060023F5 RID: 9205
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetComputeVectorParam_Injected(ComputeShader computeShader, int nameID, ref Vector4 val);

		// Token: 0x060023F6 RID: 9206
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetComputeMatrixParam_Injected(ComputeShader computeShader, int nameID, ref Matrix4x4 val);

		// Token: 0x060023F7 RID: 9207
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetComputeGraphicsBufferHandleParam_Injected(ComputeShader computeShader, int kernelIndex, int nameID, ref GraphicsBufferHandle bufferHandle);

		// Token: 0x060023F8 RID: 9208
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingVectorParam_Injected(RayTracingShader rayTracingShader, int nameID, ref Vector4 val);

		// Token: 0x060023F9 RID: 9209
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetRayTracingMatrixParam_Injected(RayTracingShader rayTracingShader, int nameID, ref Matrix4x4 val);

		// Token: 0x060023FA RID: 9210
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_BuildRayTracingAccelerationStructure_Injected(RayTracingAccelerationStructure accelerationStructure, ref Vector3 relativeOrigin);

		// Token: 0x060023FB RID: 9211
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GenerateMips_Injected(ref RenderTargetIdentifier rt);

		// Token: 0x060023FC RID: 9212
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawMesh_Injected(Mesh mesh, ref Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties);

		// Token: 0x060023FD RID: 9213
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawRendererList_Injected(ref RendererList rendererList);

		// Token: 0x060023FE RID: 9214
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawProcedural_Injected(ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties);

		// Token: 0x060023FF RID: 9215
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawProceduralIndexed_Injected(GraphicsBuffer indexBuffer, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount, MaterialPropertyBlock properties);

		// Token: 0x06002400 RID: 9216
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawProceduralIndirect_Injected(ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		// Token: 0x06002401 RID: 9217
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawProceduralIndexedIndirect_Injected(GraphicsBuffer indexBuffer, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		// Token: 0x06002402 RID: 9218
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawProceduralIndirectGraphicsBuffer_Injected(ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		// Token: 0x06002403 RID: 9219
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawProceduralIndexedIndirectGraphicsBuffer_Injected(GraphicsBuffer indexBuffer, ref Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, GraphicsBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

		// Token: 0x06002404 RID: 9220
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_DrawOcclusionMesh_Injected(ref RectInt normalizedCamViewport);

		// Token: 0x06002405 RID: 9221
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetViewport_Injected(ref Rect pixelRect);

		// Token: 0x06002406 RID: 9222
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EnableScissorRect_Injected(ref Rect scissor);

		// Token: 0x06002407 RID: 9223
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Blit_Texture_Injected(Texture source, ref RenderTargetIdentifier dest, Material mat, int pass, ref Vector2 scale, ref Vector2 offset, int sourceDepthSlice, int destDepthSlice);

		// Token: 0x06002408 RID: 9224
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Blit_Identifier_Injected(ref RenderTargetIdentifier source, ref RenderTargetIdentifier dest, Material mat, int pass, ref Vector2 scale, ref Vector2 offset, int sourceDepthSlice, int destDepthSlice);

		// Token: 0x06002409 RID: 9225
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetTemporaryRTWithDescriptor_Injected(int nameID, ref RenderTextureDescriptor desc, FilterMode filter);

		// Token: 0x0600240A RID: 9226
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearRenderTarget_Injected(RTClearFlags clearFlags, ref Color backgroundColor, float depth, uint stencil);

		// Token: 0x0600240B RID: 9227
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalVector_Injected(int nameID, ref Vector4 value);

		// Token: 0x0600240C RID: 9228
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalColor_Injected(int nameID, ref Color value);

		// Token: 0x0600240D RID: 9229
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalMatrix_Injected(int nameID, ref Matrix4x4 value);

		// Token: 0x0600240E RID: 9230
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EnableGlobalKeyword_Injected(ref GlobalKeyword keyword);

		// Token: 0x0600240F RID: 9231
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EnableMaterialKeyword_Injected(Material material, ref LocalKeyword keyword);

		// Token: 0x06002410 RID: 9232
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EnableComputeKeyword_Injected(ComputeShader computeShader, ref LocalKeyword keyword);

		// Token: 0x06002411 RID: 9233
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DisableGlobalKeyword_Injected(ref GlobalKeyword keyword);

		// Token: 0x06002412 RID: 9234
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DisableMaterialKeyword_Injected(Material material, ref LocalKeyword keyword);

		// Token: 0x06002413 RID: 9235
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DisableComputeKeyword_Injected(ComputeShader computeShader, ref LocalKeyword keyword);

		// Token: 0x06002414 RID: 9236
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetGlobalKeyword_Injected(ref GlobalKeyword keyword, bool value);

		// Token: 0x06002415 RID: 9237
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMaterialKeyword_Injected(Material material, ref LocalKeyword keyword, bool value);

		// Token: 0x06002416 RID: 9238
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetComputeKeyword_Injected(ComputeShader computeShader, ref LocalKeyword keyword, bool value);

		// Token: 0x06002417 RID: 9239
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetViewMatrix_Injected(ref Matrix4x4 view);

		// Token: 0x06002418 RID: 9240
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetProjectionMatrix_Injected(ref Matrix4x4 proj);

		// Token: 0x06002419 RID: 9241
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetViewProjectionMatrices_Injected(ref Matrix4x4 view, ref Matrix4x4 proj);

		// Token: 0x0600241A RID: 9242
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void IncrementUpdateCount_Injected(ref RenderTargetIdentifier dest);

		// Token: 0x0600241B RID: 9243
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTargetSingle_Internal_Injected(ref RenderTargetIdentifier rt, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction);

		// Token: 0x0600241C RID: 9244
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTargetColorDepth_Internal_Injected(ref RenderTargetIdentifier color, ref RenderTargetIdentifier depth, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, RenderTargetFlags flags);

		// Token: 0x0600241D RID: 9245
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTargetMulti_Internal_Injected(RenderTargetIdentifier[] colors, ref RenderTargetIdentifier depth, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, RenderTargetFlags flags);

		// Token: 0x0600241E RID: 9246
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTargetColorDepthSubtarget_Injected(ref RenderTargetIdentifier color, ref RenderTargetIdentifier depth, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, int mipLevel, CubemapFace cubemapFace, int depthSlice);

		// Token: 0x0600241F RID: 9247
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTargetMultiSubtarget_Injected(RenderTargetIdentifier[] colors, ref RenderTargetIdentifier depth, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction, int mipLevel, CubemapFace cubemapFace, int depthSlice);

		// Token: 0x06002420 RID: 9248
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_ProcessVTFeedback_Injected(ref RenderTargetIdentifier rt, IntPtr resolver, int slice, int x, int width, int y, int height, int mip);

		// Token: 0x04000D21 RID: 3361
		internal IntPtr m_Ptr;
	}
}
