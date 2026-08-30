using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000227 RID: 551
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Export/Scripting/AsyncOperation.bindings.h")]
	[NativeHeader("Runtime/Misc/AsyncOperation.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class AsyncOperation : YieldInstruction
	{
		// Token: 0x0600182D RID: 6189
		[StaticAccessor("AsyncOperationBindings", StaticAccessorType.DoubleColon)]
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalDestroy(IntPtr ptr);

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600182E RID: 6190
		public extern bool isDone
		{
			[NativeMethod("IsDone")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600182F RID: 6191
		public extern float progress
		{
			[NativeMethod("GetProgress")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001830 RID: 6192
		// (set) Token: 0x06001831 RID: 6193
		public extern int priority
		{
			[NativeMethod("GetPriority")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetPriority")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001832 RID: 6194
		// (set) Token: 0x06001833 RID: 6195
		public extern bool allowSceneActivation
		{
			[NativeMethod("GetAllowSceneActivation")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetAllowSceneActivation")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0002804C File Offset: 0x0002624C
		~AsyncOperation()
		{
			AsyncOperation.InternalDestroy(this.m_Ptr);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x00028084 File Offset: 0x00026284
		[RequiredByNativeCode]
		internal void InvokeCompletionEvent()
		{
			bool flag = this.m_completeCallback != null;
			if (flag)
			{
				this.m_completeCallback(this);
				this.m_completeCallback = null;
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001836 RID: 6198 RVA: 0x000280B8 File Offset: 0x000262B8
		// (remove) Token: 0x06001837 RID: 6199 RVA: 0x000280F5 File Offset: 0x000262F5
		public event Action<AsyncOperation> completed
		{
			add
			{
				bool isDone = this.isDone;
				if (isDone)
				{
					value(this);
				}
				else
				{
					this.m_completeCallback = (Action<AsyncOperation>)Delegate.Combine(this.m_completeCallback, value);
				}
			}
			remove
			{
				this.m_completeCallback = (Action<AsyncOperation>)Delegate.Remove(this.m_completeCallback, value);
			}
		}

		// Token: 0x04000892 RID: 2194
		internal IntPtr m_Ptr;

		// Token: 0x04000893 RID: 2195
		private Action<AsyncOperation> m_completeCallback;
	}
}
