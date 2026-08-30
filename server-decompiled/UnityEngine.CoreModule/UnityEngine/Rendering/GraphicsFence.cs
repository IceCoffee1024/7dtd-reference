using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200042D RID: 1069
	[NativeHeader("Runtime/Graphics/GPUFence.h")]
	[UsedByNativeCode]
	public struct GraphicsFence
	{
		// Token: 0x060021F3 RID: 8691 RVA: 0x00038A5C File Offset: 0x00036C5C
		internal static SynchronisationStageFlags TranslateSynchronizationStageToFlags(SynchronisationStage s)
		{
			return (s == SynchronisationStage.VertexProcessing) ? SynchronisationStageFlags.VertexProcessing : SynchronisationStageFlags.PixelProcessing;
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x00038A78 File Offset: 0x00036C78
		public bool passed
		{
			get
			{
				this.Validate();
				bool flag = !SystemInfo.supportsGraphicsFence;
				if (flag)
				{
					throw new NotSupportedException("Cannot determine if this GraphicsFence has passed as this platform has not implemented GraphicsFences.");
				}
				bool flag2 = this.m_FenceType == GraphicsFenceType.AsyncQueueSynchronisation && !SystemInfo.supportsAsyncCompute;
				if (flag2)
				{
					throw new NotSupportedException("Cannot determine if this AsyncQueueSynchronisation GraphicsFence has passed as this platform does not support async compute.");
				}
				bool flag3 = !this.IsFencePending();
				return flag3 || GraphicsFence.HasFencePassed_Internal(this.m_Ptr);
			}
		}

		// Token: 0x060021F5 RID: 8693
		[FreeFunction("GPUFenceInternals::HasFencePassed_Internal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasFencePassed_Internal(IntPtr fencePtr);

		// Token: 0x060021F6 RID: 8694 RVA: 0x00038AE8 File Offset: 0x00036CE8
		internal void InitPostAllocation()
		{
			bool flag = this.m_Ptr == IntPtr.Zero;
			if (flag)
			{
				bool supportsGraphicsFence = SystemInfo.supportsGraphicsFence;
				if (supportsGraphicsFence)
				{
					throw new NullReferenceException("The internal fence ptr is null, this should not be possible for fences that have been correctly constructed using Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
				}
				this.m_Version = this.GetPlatformNotSupportedVersion();
			}
			else
			{
				this.m_Version = GraphicsFence.GetVersionNumber(this.m_Ptr);
			}
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00038B40 File Offset: 0x00036D40
		internal bool IsFencePending()
		{
			bool flag = this.m_Ptr == IntPtr.Zero;
			return !flag && this.m_Version == GraphicsFence.GetVersionNumber(this.m_Ptr);
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00038B80 File Offset: 0x00036D80
		internal void Validate()
		{
			bool flag = this.m_Version == 0 || (SystemInfo.supportsGraphicsFence && this.m_Version == this.GetPlatformNotSupportedVersion());
			if (flag)
			{
				throw new InvalidOperationException("This GraphicsFence object has not been correctly constructed see Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
			}
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00038BC0 File Offset: 0x00036DC0
		private int GetPlatformNotSupportedVersion()
		{
			return -1;
		}

		// Token: 0x060021FA RID: 8698
		[FreeFunction("GPUFenceInternals::GetVersionNumber")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetVersionNumber(IntPtr fencePtr);

		// Token: 0x04000D1D RID: 3357
		internal IntPtr m_Ptr;

		// Token: 0x04000D1E RID: 3358
		internal int m_Version;

		// Token: 0x04000D1F RID: 3359
		internal GraphicsFenceType m_FenceType;
	}
}
