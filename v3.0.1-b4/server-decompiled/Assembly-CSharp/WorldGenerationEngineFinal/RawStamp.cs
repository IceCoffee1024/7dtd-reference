using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016FA RID: 5882
	[BurstCompile(CompileSynchronously = true)]
	public class RawStamp
	{
		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0044045F File Offset: 0x0043E65F
		[BurstCompile(CompileSynchronously = true)]
		public static void SmoothAlpha(ref RawStamp.Data d, int _boxSize)
		{
			RawStamp.SmoothAlpha_0000B669$BurstDirectCall.Invoke(ref d, _boxSize);
		}

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x00440468 File Offset: 0x0043E668
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

		// Token: 0x0600B6B8 RID: 46776 RVA: 0x00440546 File Offset: 0x0043E746
		public void Clear()
		{
			this.data.heightPixels.Dispose();
			this.data.alphaPixels.Dispose();
			this.data.waterPixels.Dispose();
		}

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x0600B6B9 RID: 46777 RVA: 0x00440578 File Offset: 0x0043E778
		public int width
		{
			get
			{
				return this.data.width;
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x0600B6BA RID: 46778 RVA: 0x00440585 File Offset: 0x0043E785
		public int height
		{
			get
			{
				return this.data.height;
			}
		}

		// Token: 0x0600B6BC RID: 46780 RVA: 0x00440594 File Offset: 0x0043E794
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

		// Token: 0x04008891 RID: 34961
		public string name;

		// Token: 0x04008892 RID: 34962
		public RawStamp.Data data;

		// Token: 0x020016FB RID: 5883
		public struct Data
		{
			// Token: 0x04008893 RID: 34963
			public float heightConst;

			// Token: 0x04008894 RID: 34964
			public NativeArray<float> heightPixels;

			// Token: 0x04008895 RID: 34965
			public float alphaConst;

			// Token: 0x04008896 RID: 34966
			public NativeArray<float> alphaPixels;

			// Token: 0x04008897 RID: 34967
			public NativeArray<float> waterPixels;

			// Token: 0x04008898 RID: 34968
			public int width;

			// Token: 0x04008899 RID: 34969
			public int height;
		}

		// Token: 0x020016FC RID: 5884
		// (Invoke) Token: 0x0600B6BE RID: 46782
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void SmoothAlpha_0000B669$PostfixBurstDelegate(ref RawStamp.Data d, int _boxSize);

		// Token: 0x020016FD RID: 5885
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class SmoothAlpha_0000B669$BurstDirectCall
		{
			// Token: 0x0600B6C1 RID: 46785 RVA: 0x00440676 File Offset: 0x0043E876
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RawStamp.SmoothAlpha_0000B669$BurstDirectCall.Pointer == 0)
				{
					RawStamp.SmoothAlpha_0000B669$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RawStamp.SmoothAlpha_0000B669$BurstDirectCall.DeferredCompilation, methodof(RawStamp.SmoothAlpha$BurstManaged(ref RawStamp.Data, int)).MethodHandle, typeof(RawStamp.SmoothAlpha_0000B669$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RawStamp.SmoothAlpha_0000B669$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B6C2 RID: 46786 RVA: 0x004406A4 File Offset: 0x0043E8A4
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RawStamp.SmoothAlpha_0000B669$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B6C3 RID: 46787 RVA: 0x004406BC File Offset: 0x0043E8BC
			public static void Constructor()
			{
				RawStamp.SmoothAlpha_0000B669$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RawStamp.SmoothAlpha(ref RawStamp.Data, int)).MethodHandle);
			}

			// Token: 0x0600B6C4 RID: 46788 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B6C5 RID: 46789 RVA: 0x004406CD File Offset: 0x0043E8CD
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static SmoothAlpha_0000B669$BurstDirectCall()
			{
				RawStamp.SmoothAlpha_0000B669$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B6C6 RID: 46790 RVA: 0x004406D4 File Offset: 0x0043E8D4
			public static void Invoke(ref RawStamp.Data d, int _boxSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RawStamp.SmoothAlpha_0000B669$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.RawStamp/Data&,System.Int32), ref d, _boxSize, functionPointer);
						return;
					}
				}
				RawStamp.SmoothAlpha$BurstManaged(ref d, _boxSize);
			}

			// Token: 0x0400889A RID: 34970
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x0400889B RID: 34971
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
