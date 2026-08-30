using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020000E7 RID: 231
	[RequiredByNativeCode]
	[NativeHeader("Runtime/Math/AnimationCurve.bindings.h")]
	[StructLayout(LayoutKind.Sequential)]
	public class AnimationCurve : IEquatable<AnimationCurve>
	{
		// Token: 0x06000419 RID: 1049
		[FreeFunction("AnimationCurveBindings::Internal_Destroy", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		// Token: 0x0600041A RID: 1050
		[FreeFunction("AnimationCurveBindings::Internal_Create", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create(Keyframe[] keys);

		// Token: 0x0600041B RID: 1051
		[FreeFunction("AnimationCurveBindings::Internal_Equals", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_Equals(IntPtr other);

		// Token: 0x0600041C RID: 1052
		[FreeFunction("AnimationCurveBindings::Internal_CopyFrom", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_CopyFrom(IntPtr other);

		// Token: 0x0600041D RID: 1053 RVA: 0x00006F88 File Offset: 0x00005188
		~AnimationCurve()
		{
			AnimationCurve.Internal_Destroy(this.m_Ptr);
		}

		// Token: 0x0600041E RID: 1054
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float Evaluate(float time);

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00006FC0 File Offset: 0x000051C0
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00006FD8 File Offset: 0x000051D8
		public Keyframe[] keys
		{
			get
			{
				return this.GetKeys();
			}
			set
			{
				this.SetKeys(value);
			}
		}

		// Token: 0x06000421 RID: 1057
		[FreeFunction("AnimationCurveBindings::AddKeySmoothTangents", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int AddKey(float time, float value);

		// Token: 0x06000422 RID: 1058 RVA: 0x00006FE4 File Offset: 0x000051E4
		public int AddKey(Keyframe key)
		{
			return this.AddKey_Internal(key);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00006FFD File Offset: 0x000051FD
		[NativeMethod("AddKey", IsThreadSafe = true)]
		private int AddKey_Internal(Keyframe key)
		{
			return this.AddKey_Internal_Injected(ref key);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00007007 File Offset: 0x00005207
		[NativeThrows]
		[FreeFunction("AnimationCurveBindings::MoveKey", HasExplicitThis = true, IsThreadSafe = true)]
		public int MoveKey(int index, Keyframe key)
		{
			return this.MoveKey_Injected(index, ref key);
		}

		// Token: 0x06000425 RID: 1061
		[FreeFunction("AnimationCurveBindings::ClearKeys", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearKeys();

		// Token: 0x06000426 RID: 1062
		[NativeThrows]
		[FreeFunction("AnimationCurveBindings::RemoveKey", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveKey(int index);

		// Token: 0x170000B3 RID: 179
		public Keyframe this[int index]
		{
			get
			{
				return this.GetKey(index);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000428 RID: 1064
		public extern int length
		{
			[NativeMethod("GetKeyCount", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000429 RID: 1065
		[FreeFunction("AnimationCurveBindings::SetKeys", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetKeys(Keyframe[] keys);

		// Token: 0x0600042A RID: 1066 RVA: 0x00007030 File Offset: 0x00005230
		[NativeThrows]
		[FreeFunction("AnimationCurveBindings::GetKey", HasExplicitThis = true, IsThreadSafe = true)]
		private Keyframe GetKey(int index)
		{
			Keyframe keyframe;
			this.GetKey_Injected(index, out keyframe);
			return keyframe;
		}

		// Token: 0x0600042B RID: 1067
		[FreeFunction("AnimationCurveBindings::GetKeys", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Keyframe[] GetKeys();

		// Token: 0x0600042C RID: 1068
		[FreeFunction("AnimationCurveBindings::GetHashCode", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern int GetHashCode();

		// Token: 0x0600042D RID: 1069
		[NativeThrows]
		[FreeFunction("AnimationCurveBindings::SmoothTangents", HasExplicitThis = true, IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SmoothTangents(int index, float weight);

		// Token: 0x0600042E RID: 1070 RVA: 0x00007048 File Offset: 0x00005248
		public static AnimationCurve Constant(float timeStart, float timeEnd, float value)
		{
			return AnimationCurve.Linear(timeStart, value, timeEnd, value);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00007064 File Offset: 0x00005264
		public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			bool flag = timeStart == timeEnd;
			AnimationCurve animationCurve;
			if (flag)
			{
				Keyframe keyframe = new Keyframe(timeStart, valueStart);
				animationCurve = new AnimationCurve(new Keyframe[] { keyframe });
			}
			else
			{
				float num = (valueEnd - valueStart) / (timeEnd - timeStart);
				Keyframe[] array = new Keyframe[]
				{
					new Keyframe(timeStart, valueStart, 0f, num),
					new Keyframe(timeEnd, valueEnd, num, 0f)
				};
				animationCurve = new AnimationCurve(array);
			}
			return animationCurve;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000070E0 File Offset: 0x000052E0
		public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			bool flag = timeStart == timeEnd;
			AnimationCurve animationCurve;
			if (flag)
			{
				Keyframe keyframe = new Keyframe(timeStart, valueStart);
				animationCurve = new AnimationCurve(new Keyframe[] { keyframe });
			}
			else
			{
				Keyframe[] array = new Keyframe[]
				{
					new Keyframe(timeStart, valueStart, 0f, 0f),
					new Keyframe(timeEnd, valueEnd, 0f, 0f)
				};
				animationCurve = new AnimationCurve(array);
			}
			return animationCurve;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000431 RID: 1073
		// (set) Token: 0x06000432 RID: 1074
		public extern WrapMode preWrapMode
		{
			[NativeMethod("GetPreInfinity", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetPreInfinity", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000433 RID: 1075
		// (set) Token: 0x06000434 RID: 1076
		public extern WrapMode postWrapMode
		{
			[NativeMethod("GetPostInfinity", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetPostInfinity", IsThreadSafe = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00007157 File Offset: 0x00005357
		public AnimationCurve(params Keyframe[] keys)
		{
			this.m_Ptr = AnimationCurve.Internal_Create(keys);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000716D File Offset: 0x0000536D
		[RequiredByNativeCode]
		public AnimationCurve()
		{
			this.m_Ptr = AnimationCurve.Internal_Create(null);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00007184 File Offset: 0x00005384
		public override bool Equals(object o)
		{
			bool flag = o == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this == o;
				flag2 = flag3 || (o.GetType() == base.GetType() && this.Equals((AnimationCurve)o));
			}
			return flag2;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000071D4 File Offset: 0x000053D4
		public bool Equals(AnimationCurve other)
		{
			bool flag = other == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this == other;
				flag2 = flag3 || this.m_Ptr.Equals(other.m_Ptr) || this.Internal_Equals(other.m_Ptr);
			}
			return flag2;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00007227 File Offset: 0x00005427
		public void CopyFrom(AnimationCurve other)
		{
			this.Internal_CopyFrom(other.m_Ptr);
		}

		// Token: 0x0600043A RID: 1082
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int AddKey_Internal_Injected(ref Keyframe key);

		// Token: 0x0600043B RID: 1083
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int MoveKey_Injected(int index, ref Keyframe key);

		// Token: 0x0600043C RID: 1084
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetKey_Injected(int index, out Keyframe ret);

		// Token: 0x0400028D RID: 653
		internal IntPtr m_Ptr;
	}
}
