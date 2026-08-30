using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200021B RID: 539
	[NativeHeader("Runtime/Export/Random/Random.bindings.h")]
	public static class Random
	{
		// Token: 0x060017C0 RID: 6080
		[NativeMethod("SetSeed")]
		[StaticAccessor("GetScriptingRand()", StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InitState(int seed);

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x000277F8 File Offset: 0x000259F8
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x0002780D File Offset: 0x00025A0D
		[StaticAccessor("GetScriptingRand()", StaticAccessorType.Dot)]
		public static Random.State state
		{
			get
			{
				Random.State state;
				Random.get_state_Injected(out state);
				return state;
			}
			set
			{
				Random.set_state_Injected(ref value);
			}
		}

		// Token: 0x060017C3 RID: 6083
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Range(float minInclusive, float maxInclusive);

		// Token: 0x060017C4 RID: 6084 RVA: 0x00027818 File Offset: 0x00025A18
		public static int Range(int minInclusive, int maxExclusive)
		{
			return Random.RandomRangeInt(minInclusive, maxExclusive);
		}

		// Token: 0x060017C5 RID: 6085
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int RandomRangeInt(int minInclusive, int maxExclusive);

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060017C6 RID: 6086
		public static extern float value
		{
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060017C7 RID: 6087 RVA: 0x00027834 File Offset: 0x00025A34
		public static Vector3 insideUnitSphere
		{
			[FreeFunction]
			get
			{
				Vector3 vector;
				Random.get_insideUnitSphere_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x060017C8 RID: 6088
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRandomUnitCircle(out Vector2 output);

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060017C9 RID: 6089 RVA: 0x0002784C File Offset: 0x00025A4C
		public static Vector2 insideUnitCircle
		{
			get
			{
				Vector2 vector;
				Random.GetRandomUnitCircle(out vector);
				return vector;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x00027868 File Offset: 0x00025A68
		public static Vector3 onUnitSphere
		{
			[FreeFunction]
			get
			{
				Vector3 vector;
				Random.get_onUnitSphere_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060017CB RID: 6091 RVA: 0x00027880 File Offset: 0x00025A80
		public static Quaternion rotation
		{
			[FreeFunction]
			get
			{
				Quaternion quaternion;
				Random.get_rotation_Injected(out quaternion);
				return quaternion;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060017CC RID: 6092 RVA: 0x00027898 File Offset: 0x00025A98
		public static Quaternion rotationUniform
		{
			[FreeFunction]
			get
			{
				Quaternion quaternion;
				Random.get_rotationUniform_Injected(out quaternion);
				return quaternion;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060017CD RID: 6093
		// (set) Token: 0x060017CE RID: 6094
		[StaticAccessor("GetScriptingRand()", StaticAccessorType.Dot)]
		[Obsolete("Deprecated. Use InitState() function or Random.state property instead.")]
		public static extern int seed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x000278B0 File Offset: 0x00025AB0
		[Obsolete("Use Random.Range instead")]
		public static float RandomRange(float min, float max)
		{
			return Random.Range(min, max);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x000278CC File Offset: 0x00025ACC
		[Obsolete("Use Random.Range instead")]
		public static int RandomRange(int min, int max)
		{
			return Random.Range(min, max);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x000278E8 File Offset: 0x00025AE8
		public static Color ColorHSV()
		{
			return Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00027928 File Offset: 0x00025B28
		public static Color ColorHSV(float hueMin, float hueMax)
		{
			return Random.ColorHSV(hueMin, hueMax, 0f, 1f, 0f, 1f, 1f, 1f);
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00027960 File Offset: 0x00025B60
		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
		{
			return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 0f, 1f, 1f, 1f);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00027990 File Offset: 0x00025B90
		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
		{
			return Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, 1f, 1f);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000279BC File Offset: 0x00025BBC
		public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
		{
			float num = Mathf.Lerp(hueMin, hueMax, Random.value);
			float num2 = Mathf.Lerp(saturationMin, saturationMax, Random.value);
			float num3 = Mathf.Lerp(valueMin, valueMax, Random.value);
			Color color = Color.HSVToRGB(num, num2, num3, true);
			color.a = Mathf.Lerp(alphaMin, alphaMax, Random.value);
			return color;
		}

		// Token: 0x060017D6 RID: 6102
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_state_Injected(out Random.State ret);

		// Token: 0x060017D7 RID: 6103
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_state_Injected(ref Random.State value);

		// Token: 0x060017D8 RID: 6104
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_insideUnitSphere_Injected(out Vector3 ret);

		// Token: 0x060017D9 RID: 6105
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_onUnitSphere_Injected(out Vector3 ret);

		// Token: 0x060017DA RID: 6106
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_rotation_Injected(out Quaternion ret);

		// Token: 0x060017DB RID: 6107
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_rotationUniform_Injected(out Quaternion ret);

		// Token: 0x0200021C RID: 540
		[Serializable]
		public struct State
		{
			// Token: 0x04000886 RID: 2182
			[SerializeField]
			private int s0;

			// Token: 0x04000887 RID: 2183
			[SerializeField]
			private int s1;

			// Token: 0x04000888 RID: 2184
			[SerializeField]
			private int s2;

			// Token: 0x04000889 RID: 2185
			[SerializeField]
			private int s3;
		}
	}
}
