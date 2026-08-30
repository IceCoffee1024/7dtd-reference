using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E1 RID: 993
	[NativeHeader("Runtime/Graphics/AsyncGPUReadbackManaged.h")]
	[NativeHeader("Runtime/Graphics/Texture.h")]
	[NativeHeader("Runtime/Shaders/ComputeShader.h")]
	[UsedByNativeCode]
	public struct AsyncGPUReadbackRequest
	{
		// Token: 0x06002152 RID: 8530 RVA: 0x0003765F File Offset: 0x0003585F
		public void Update()
		{
			AsyncGPUReadbackRequest.Update_Injected(ref this);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00037667 File Offset: 0x00035867
		public void WaitForCompletion()
		{
			AsyncGPUReadbackRequest.WaitForCompletion_Injected(ref this);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00037670 File Offset: 0x00035870
		public unsafe NativeArray<T> GetData<T>(int layer = 0) where T : struct
		{
			bool flag = !this.done || this.hasError;
			if (flag)
			{
				throw new InvalidOperationException("Cannot access the data as it is not available");
			}
			bool flag2 = layer < 0 || layer >= this.layerCount;
			if (flag2)
			{
				throw new ArgumentException(string.Format("Layer index is out of range {0} / {1}", layer, this.layerCount));
			}
			int num = UnsafeUtility.SizeOf<T>();
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)this.GetDataRaw(layer), this.layerDataSize / num, Allocator.None);
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002155 RID: 8533 RVA: 0x00037700 File Offset: 0x00035900
		public bool done
		{
			get
			{
				return this.IsDone();
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x00037718 File Offset: 0x00035918
		public bool hasError
		{
			get
			{
				return this.HasError();
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002157 RID: 8535 RVA: 0x00037730 File Offset: 0x00035930
		public int layerCount
		{
			get
			{
				return this.GetLayerCount();
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x00037748 File Offset: 0x00035948
		public int layerDataSize
		{
			get
			{
				return this.GetLayerDataSize();
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002159 RID: 8537 RVA: 0x00037760 File Offset: 0x00035960
		public int width
		{
			get
			{
				return this.GetWidth();
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600215A RID: 8538 RVA: 0x00037778 File Offset: 0x00035978
		public int height
		{
			get
			{
				return this.GetHeight();
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x00037790 File Offset: 0x00035990
		public int depth
		{
			get
			{
				return this.GetDepth();
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x000377A8 File Offset: 0x000359A8
		// (set) Token: 0x0600215D RID: 8541 RVA: 0x000377C0 File Offset: 0x000359C0
		public bool forcePlayerLoopUpdate
		{
			get
			{
				return this.GetForcePlayerLoopUpdate();
			}
			set
			{
				this.SetForcePlayerLoopUpdate(value);
			}
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000377CB File Offset: 0x000359CB
		private bool IsDone()
		{
			return AsyncGPUReadbackRequest.IsDone_Injected(ref this);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000377D3 File Offset: 0x000359D3
		private bool HasError()
		{
			return AsyncGPUReadbackRequest.HasError_Injected(ref this);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000377DB File Offset: 0x000359DB
		private int GetLayerCount()
		{
			return AsyncGPUReadbackRequest.GetLayerCount_Injected(ref this);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000377E3 File Offset: 0x000359E3
		private int GetLayerDataSize()
		{
			return AsyncGPUReadbackRequest.GetLayerDataSize_Injected(ref this);
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000377EB File Offset: 0x000359EB
		private int GetWidth()
		{
			return AsyncGPUReadbackRequest.GetWidth_Injected(ref this);
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000377F3 File Offset: 0x000359F3
		private int GetHeight()
		{
			return AsyncGPUReadbackRequest.GetHeight_Injected(ref this);
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000377FB File Offset: 0x000359FB
		private int GetDepth()
		{
			return AsyncGPUReadbackRequest.GetDepth_Injected(ref this);
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00037803 File Offset: 0x00035A03
		private bool GetForcePlayerLoopUpdate()
		{
			return AsyncGPUReadbackRequest.GetForcePlayerLoopUpdate_Injected(ref this);
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0003780B File Offset: 0x00035A0B
		private void SetForcePlayerLoopUpdate(bool b)
		{
			AsyncGPUReadbackRequest.SetForcePlayerLoopUpdate_Injected(ref this, b);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00037814 File Offset: 0x00035A14
		internal void SetScriptingCallback(Action<AsyncGPUReadbackRequest> callback)
		{
			AsyncGPUReadbackRequest.SetScriptingCallback_Injected(ref this, callback);
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0003781D File Offset: 0x00035A1D
		private IntPtr GetDataRaw(int layer)
		{
			return AsyncGPUReadbackRequest.GetDataRaw_Injected(ref this, layer);
		}

		// Token: 0x06002169 RID: 8553
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Update_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x0600216A RID: 8554
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WaitForCompletion_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x0600216B RID: 8555
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDone_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x0600216C RID: 8556
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasError_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x0600216D RID: 8557
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLayerCount_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x0600216E RID: 8558
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLayerDataSize_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x0600216F RID: 8559
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetWidth_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x06002170 RID: 8560
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetHeight_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x06002171 RID: 8561
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetDepth_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x06002172 RID: 8562
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetForcePlayerLoopUpdate_Injected(ref AsyncGPUReadbackRequest _unity_self);

		// Token: 0x06002173 RID: 8563
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetForcePlayerLoopUpdate_Injected(ref AsyncGPUReadbackRequest _unity_self, bool b);

		// Token: 0x06002174 RID: 8564
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetScriptingCallback_Injected(ref AsyncGPUReadbackRequest _unity_self, Action<AsyncGPUReadbackRequest> callback);

		// Token: 0x06002175 RID: 8565
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetDataRaw_Injected(ref AsyncGPUReadbackRequest _unity_self, int layer);

		// Token: 0x04000B13 RID: 2835
		internal IntPtr m_Ptr;

		// Token: 0x04000B14 RID: 2836
		internal int m_Version;
	}
}
