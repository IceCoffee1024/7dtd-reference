using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200177D RID: 6013
	[BurstCompile(CompileSynchronously = true)]
	public class RawStamp
	{
		// Token: 0x0600BAA4 RID: 47780 RVA: 0x0044E0A3 File Offset: 0x0044C2A3
		[BurstCompile(CompileSynchronously = true)]
		public static void SmoothAlpha(ref RawStamp.Data d, int _boxSize)
		{
			RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.Invoke(ref d, _boxSize);
		}

		// Token: 0x0600BAA5 RID: 47781 RVA: 0x0044E0AC File Offset: 0x0044C2AC
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

		// Token: 0x0600BAA6 RID: 47782 RVA: 0x0044E18A File Offset: 0x0044C38A
		public void Clear()
		{
			this.data.heightPixels.Dispose();
			this.data.alphaPixels.Dispose();
			this.data.waterPixels.Dispose();
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x0600BAA7 RID: 47783 RVA: 0x0044E1BC File Offset: 0x0044C3BC
		public int width
		{
			get
			{
				return this.data.width;
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x0600BAA8 RID: 47784 RVA: 0x0044E1C9 File Offset: 0x0044C3C9
		public int height
		{
			get
			{
				return this.data.height;
			}
		}

		// Token: 0x0600BAAA RID: 47786 RVA: 0x0044E1D8 File Offset: 0x0044C3D8
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

		// Token: 0x04008B5B RID: 35675
		public string name;

		// Token: 0x04008B5C RID: 35676
		public RawStamp.Data data;

		// Token: 0x0200177E RID: 6014
		public struct Data
		{
			// Token: 0x04008B5D RID: 35677
			public float heightConst;

			// Token: 0x04008B5E RID: 35678
			public NativeArray<float> heightPixels;

			// Token: 0x04008B5F RID: 35679
			public float alphaConst;

			// Token: 0x04008B60 RID: 35680
			public NativeArray<float> alphaPixels;

			// Token: 0x04008B61 RID: 35681
			public NativeArray<float> waterPixels;

			// Token: 0x04008B62 RID: 35682
			public int width;

			// Token: 0x04008B63 RID: 35683
			public int height;
		}

		// Token: 0x0200177F RID: 6015
		// (Invoke) Token: 0x0600BAAC RID: 47788
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void SmoothAlpha_0000BA57$PostfixBurstDelegate(ref RawStamp.Data d, int _boxSize);

		// Token: 0x02001780 RID: 6016
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class SmoothAlpha_0000BA57$BurstDirectCall
		{
			// Token: 0x0600BAAF RID: 47791 RVA: 0x0044E2BA File Offset: 0x0044C4BA
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.Pointer == 0)
				{
					RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.DeferredCompilation, methodof(RawStamp.SmoothAlpha$BurstManaged(ref RawStamp.Data, int)).MethodHandle, typeof(RawStamp.SmoothAlpha_0000BA57$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BAB0 RID: 47792 RVA: 0x0044E2E8 File Offset: 0x0044C4E8
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BAB1 RID: 47793 RVA: 0x0044E300 File Offset: 0x0044C500
			public static void Constructor()
			{
				RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RawStamp.SmoothAlpha(ref RawStamp.Data, int)).MethodHandle);
			}

			// Token: 0x0600BAB2 RID: 47794 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BAB3 RID: 47795 RVA: 0x0044E311 File Offset: 0x0044C511
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static SmoothAlpha_0000BA57$BurstDirectCall()
			{
				RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BAB4 RID: 47796 RVA: 0x0044E318 File Offset: 0x0044C518
			public static void Invoke(ref RawStamp.Data d, int _boxSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.RawStamp/Data&,System.Int32), ref d, _boxSize, functionPointer);
						return;
					}
				}
				RawStamp.SmoothAlpha$BurstManaged(ref d, _boxSize);
			}

			// Token: 0x04008B64 RID: 35684
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B65 RID: 35685
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
