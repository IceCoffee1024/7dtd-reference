using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200174C RID: 5964
	[BurstCompile(CompileSynchronously = true)]
	public class RawStamp
	{
		// Token: 0x0600B990 RID: 47504 RVA: 0x0044A6F7 File Offset: 0x004488F7
		[BurstCompile(CompileSynchronously = true)]
		public static void SmoothAlpha(ref RawStamp.Data d, int _boxSize)
		{
			RawStamp.SmoothAlpha_0000B943$BurstDirectCall.Invoke(ref d, _boxSize);
		}

		// Token: 0x0600B991 RID: 47505 RVA: 0x0044A700 File Offset: 0x00448900
		public void BoxAlpha()
		{
			for (int i = 0; i < this.height; i += 4)
			{
				for (int j = 0; j < this.width; j += 4)
				{
					int num = j + i * this.width;
					double num2 = 0.0;
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < 4; l++)
						{
							num2 += (double)this.data.alphaPixels[num + l + k * this.width];
						}
					}
					num2 /= 16.0;
					for (int m = 0; m < 4; m++)
					{
						for (int n = 0; n < 4; n++)
						{
							this.data.alphaPixels[num + n + m * this.width] = (float)num2;
						}
					}
				}
			}
		}

		// Token: 0x0600B992 RID: 47506 RVA: 0x0044A7DE File Offset: 0x004489DE
		public void Clear()
		{
			this.data.heightPixels.Dispose();
			this.data.alphaPixels.Dispose();
			this.data.waterPixels.Dispose();
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x0600B993 RID: 47507 RVA: 0x0044A810 File Offset: 0x00448A10
		public int width
		{
			get
			{
				return this.data.width;
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x0600B994 RID: 47508 RVA: 0x0044A81D File Offset: 0x00448A1D
		public int height
		{
			get
			{
				return this.data.height;
			}
		}

		// Token: 0x0600B996 RID: 47510 RVA: 0x0044A82C File Offset: 0x00448A2C
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SmoothAlpha$BurstManaged(ref RawStamp.Data d, int _boxSize)
		{
			NativeArray<float> nativeArray = new NativeArray<float>(d.alphaPixels.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < d.height; i++)
			{
				for (int j = 0; j < d.width; j++)
				{
					float num = 0f;
					int num2 = 0;
					for (int k = -1; k < _boxSize; k++)
					{
						int num3 = i + k;
						if (num3 < d.height)
						{
							for (int l = -1; l < _boxSize; l++)
							{
								int num4 = j + l;
								if (num4 < d.width)
								{
									num += d.alphaPixels[num4 + num3 * d.width];
									num2++;
								}
							}
						}
					}
					num /= (float)num2;
					nativeArray[j + i * d.width] = num;
				}
			}
			d.alphaPixels.Dispose();
			d.alphaPixels = nativeArray;
		}

		// Token: 0x04008A72 RID: 35442
		public string name;

		// Token: 0x04008A73 RID: 35443
		public RawStamp.Data data;

		// Token: 0x0200174D RID: 5965
		public struct Data
		{
			// Token: 0x04008A74 RID: 35444
			public float heightConst;

			// Token: 0x04008A75 RID: 35445
			public NativeArray<float> heightPixels;

			// Token: 0x04008A76 RID: 35446
			public float alphaConst;

			// Token: 0x04008A77 RID: 35447
			public NativeArray<float> alphaPixels;

			// Token: 0x04008A78 RID: 35448
			public NativeArray<float> waterPixels;

			// Token: 0x04008A79 RID: 35449
			public int width;

			// Token: 0x04008A7A RID: 35450
			public int height;
		}

		// Token: 0x0200174E RID: 5966
		// (Invoke) Token: 0x0600B998 RID: 47512
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void SmoothAlpha_0000B943$PostfixBurstDelegate(ref RawStamp.Data d, int _boxSize);

		// Token: 0x0200174F RID: 5967
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class SmoothAlpha_0000B943$BurstDirectCall
		{
			// Token: 0x0600B99B RID: 47515 RVA: 0x0044A90E File Offset: 0x00448B0E
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RawStamp.SmoothAlpha_0000B943$BurstDirectCall.Pointer == 0)
				{
					RawStamp.SmoothAlpha_0000B943$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RawStamp.SmoothAlpha_0000B943$BurstDirectCall.DeferredCompilation, methodof(RawStamp.SmoothAlpha$BurstManaged(ref RawStamp.Data, int)).MethodHandle, typeof(RawStamp.SmoothAlpha_0000B943$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RawStamp.SmoothAlpha_0000B943$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B99C RID: 47516 RVA: 0x0044A93C File Offset: 0x00448B3C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RawStamp.SmoothAlpha_0000B943$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B99D RID: 47517 RVA: 0x0044A954 File Offset: 0x00448B54
			public static void Constructor()
			{
				RawStamp.SmoothAlpha_0000B943$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RawStamp.SmoothAlpha(ref RawStamp.Data, int)).MethodHandle);
			}

			// Token: 0x0600B99E RID: 47518 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B99F RID: 47519 RVA: 0x0044A965 File Offset: 0x00448B65
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static SmoothAlpha_0000B943$BurstDirectCall()
			{
				RawStamp.SmoothAlpha_0000B943$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B9A0 RID: 47520 RVA: 0x0044A96C File Offset: 0x00448B6C
			public static void Invoke(ref RawStamp.Data d, int _boxSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RawStamp.SmoothAlpha_0000B943$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.RawStamp/Data&,System.Int32), ref d, _boxSize, functionPointer);
						return;
					}
				}
				RawStamp.SmoothAlpha$BurstManaged(ref d, _boxSize);
			}

			// Token: 0x04008A7B RID: 35451
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A7C RID: 35452
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
