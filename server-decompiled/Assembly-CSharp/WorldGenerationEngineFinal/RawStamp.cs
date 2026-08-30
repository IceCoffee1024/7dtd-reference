using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001720 RID: 5920
	[BurstCompile(CompileSynchronously = true)]
	public class RawStamp
	{
		// Token: 0x0600B839 RID: 47161 RVA: 0x004461D7 File Offset: 0x004443D7
		[BurstCompile(CompileSynchronously = true)]
		public static void SmoothAlpha(ref RawStamp.Data d, int _boxSize)
		{
			RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.Invoke(ref d, _boxSize);
		}

		// Token: 0x0600B83A RID: 47162 RVA: 0x004461E0 File Offset: 0x004443E0
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

		// Token: 0x0600B83B RID: 47163 RVA: 0x004462BE File Offset: 0x004444BE
		public void Clear()
		{
			this.data.heightPixels.Dispose();
			this.data.alphaPixels.Dispose();
			this.data.waterPixels.Dispose();
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600B83C RID: 47164 RVA: 0x004462F0 File Offset: 0x004444F0
		public int width
		{
			get
			{
				return this.data.width;
			}
		}

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600B83D RID: 47165 RVA: 0x004462FD File Offset: 0x004444FD
		public int height
		{
			get
			{
				return this.data.height;
			}
		}

		// Token: 0x0600B83F RID: 47167 RVA: 0x0044630C File Offset: 0x0044450C
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

		// Token: 0x040089AD RID: 35245
		public string name;

		// Token: 0x040089AE RID: 35246
		public RawStamp.Data data;

		// Token: 0x02001721 RID: 5921
		public struct Data
		{
			// Token: 0x040089AF RID: 35247
			public float heightConst;

			// Token: 0x040089B0 RID: 35248
			public NativeArray<float> heightPixels;

			// Token: 0x040089B1 RID: 35249
			public float alphaConst;

			// Token: 0x040089B2 RID: 35250
			public NativeArray<float> alphaPixels;

			// Token: 0x040089B3 RID: 35251
			public NativeArray<float> waterPixels;

			// Token: 0x040089B4 RID: 35252
			public int width;

			// Token: 0x040089B5 RID: 35253
			public int height;
		}

		// Token: 0x02001722 RID: 5922
		// (Invoke) Token: 0x0600B841 RID: 47169
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void SmoothAlpha_0000B7EC$PostfixBurstDelegate(ref RawStamp.Data d, int _boxSize);

		// Token: 0x02001723 RID: 5923
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class SmoothAlpha_0000B7EC$BurstDirectCall
		{
			// Token: 0x0600B844 RID: 47172 RVA: 0x004463EE File Offset: 0x004445EE
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.Pointer == 0)
				{
					RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.DeferredCompilation, methodof(RawStamp.SmoothAlpha$BurstManaged(ref RawStamp.Data, int)).MethodHandle, typeof(RawStamp.SmoothAlpha_0000B7EC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B845 RID: 47173 RVA: 0x0044641C File Offset: 0x0044461C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B846 RID: 47174 RVA: 0x00446434 File Offset: 0x00444634
			public static void Constructor()
			{
				RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RawStamp.SmoothAlpha(ref RawStamp.Data, int)).MethodHandle);
			}

			// Token: 0x0600B847 RID: 47175 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B848 RID: 47176 RVA: 0x00446445 File Offset: 0x00444645
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static SmoothAlpha_0000B7EC$BurstDirectCall()
			{
				RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B849 RID: 47177 RVA: 0x0044644C File Offset: 0x0044464C
			public static void Invoke(ref RawStamp.Data d, int _boxSize)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(WorldGenerationEngineFinal.RawStamp/Data&,System.Int32), ref d, _boxSize, functionPointer);
						return;
					}
				}
				RawStamp.SmoothAlpha$BurstManaged(ref d, _boxSize);
			}

			// Token: 0x040089B6 RID: 35254
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x040089B7 RID: 35255
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
