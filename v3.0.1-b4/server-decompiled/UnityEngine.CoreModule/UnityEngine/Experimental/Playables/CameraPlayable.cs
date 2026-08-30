using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Playables
{
	// Token: 0x020004CD RID: 1229
	[RequiredByNativeCode]
	[StaticAccessor("CameraPlayableBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Export/Director/CameraPlayable.bindings.h")]
	[NativeHeader("Runtime/Camera//Director/CameraPlayable.h")]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	public struct CameraPlayable : IPlayable, IEquatable<CameraPlayable>
	{
		// Token: 0x06002AED RID: 10989 RVA: 0x00048B84 File Offset: 0x00046D84
		public static CameraPlayable Create(PlayableGraph graph, Camera camera)
		{
			PlayableHandle playableHandle = CameraPlayable.CreateHandle(graph, camera);
			return new CameraPlayable(playableHandle);
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x00048BA4 File Offset: 0x00046DA4
		private static PlayableHandle CreateHandle(PlayableGraph graph, Camera camera)
		{
			PlayableHandle @null = PlayableHandle.Null;
			bool flag = !CameraPlayable.InternalCreateCameraPlayable(ref graph, camera, ref @null);
			PlayableHandle playableHandle;
			if (flag)
			{
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				playableHandle = @null;
			}
			return playableHandle;
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x00048BD8 File Offset: 0x00046DD8
		internal CameraPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOfType<CameraPlayable>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an CameraPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x00048C14 File Offset: 0x00046E14
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x00048C2C File Offset: 0x00046E2C
		public static implicit operator Playable(CameraPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x00048C4C File Offset: 0x00046E4C
		public static explicit operator CameraPlayable(Playable playable)
		{
			return new CameraPlayable(playable.GetHandle());
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x00048C6C File Offset: 0x00046E6C
		public bool Equals(CameraPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x00048C90 File Offset: 0x00046E90
		public Camera GetCamera()
		{
			return CameraPlayable.GetCameraInternal(ref this.m_Handle);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x00048CAD File Offset: 0x00046EAD
		public void SetCamera(Camera value)
		{
			CameraPlayable.SetCameraInternal(ref this.m_Handle, value);
		}

		// Token: 0x06002AF6 RID: 10998
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Camera GetCameraInternal(ref PlayableHandle hdl);

		// Token: 0x06002AF7 RID: 10999
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCameraInternal(ref PlayableHandle hdl, Camera camera);

		// Token: 0x06002AF8 RID: 11000
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalCreateCameraPlayable(ref PlayableGraph graph, Camera camera, ref PlayableHandle handle);

		// Token: 0x06002AF9 RID: 11001
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ValidateType(ref PlayableHandle hdl);

		// Token: 0x04001028 RID: 4136
		private PlayableHandle m_Handle;
	}
}
