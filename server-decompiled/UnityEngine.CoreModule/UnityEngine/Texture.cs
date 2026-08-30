using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001CE RID: 462
	[UsedByNativeCode]
	[NativeHeader("Runtime/Streaming/TextureStreamingManager.h")]
	[NativeHeader("Runtime/Graphics/Texture.h")]
	public class Texture : Object
	{
		// Token: 0x0600121B RID: 4635 RVA: 0x000111F6 File Offset: 0x0000F3F6
		protected Texture()
		{
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600121C RID: 4636
		// (set) Token: 0x0600121D RID: 4637
		[Obsolete("masterTextureLimit has been deprecated. Use globalMipmapLimit instead (UnityUpgradable) -> globalMipmapLimit", false)]
		[NativeProperty("ActiveGlobalMipmapLimit")]
		public static extern int masterTextureLimit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600121E RID: 4638
		// (set) Token: 0x0600121F RID: 4639
		[NativeProperty("ActiveGlobalMipmapLimit")]
		[Obsolete("globalMipmapLimit is not supported. Use QualitySettings.globalTextureMipmapLimit or Mipmap Limit Groups instead.", false)]
		public static extern int globalMipmapLimit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001220 RID: 4640
		public extern int mipmapCount
		{
			[NativeName("GetMipmapCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001221 RID: 4641
		// (set) Token: 0x06001222 RID: 4642
		[NativeProperty("AnisoLimit")]
		public static extern AnisotropicFiltering anisotropicFiltering
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001223 RID: 4643
		[NativeName("SetGlobalAnisoLimits")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax);

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x00019458 File Offset: 0x00017658
		public virtual GraphicsFormat graphicsFormat
		{
			get
			{
				return GraphicsFormatUtility.GetFormat(this);
			}
		}

		// Token: 0x06001225 RID: 4645
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetDataWidth();

		// Token: 0x06001226 RID: 4646
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetDataHeight();

		// Token: 0x06001227 RID: 4647
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern TextureDimension GetDimension();

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00019470 File Offset: 0x00017670
		// (set) Token: 0x06001229 RID: 4649 RVA: 0x00019488 File Offset: 0x00017688
		public virtual int width
		{
			get
			{
				return this.GetDataWidth();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x00019490 File Offset: 0x00017690
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x00019488 File Offset: 0x00017688
		public virtual int height
		{
			get
			{
				return this.GetDataHeight();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x000194A8 File Offset: 0x000176A8
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x00019488 File Offset: 0x00017688
		public virtual TextureDimension dimension
		{
			get
			{
				return this.GetDimension();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600122E RID: 4654
		internal extern bool isNativeTexture
		{
			[NativeName("IsNativeTexture")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600122F RID: 4655
		public virtual extern bool isReadable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001230 RID: 4656
		// (set) Token: 0x06001231 RID: 4657
		public extern TextureWrapMode wrapMode
		{
			[NativeName("GetWrapModeU")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001232 RID: 4658
		// (set) Token: 0x06001233 RID: 4659
		public extern TextureWrapMode wrapModeU
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001234 RID: 4660
		// (set) Token: 0x06001235 RID: 4661
		public extern TextureWrapMode wrapModeV
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001236 RID: 4662
		// (set) Token: 0x06001237 RID: 4663
		public extern TextureWrapMode wrapModeW
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001238 RID: 4664
		// (set) Token: 0x06001239 RID: 4665
		public extern FilterMode filterMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600123A RID: 4666
		// (set) Token: 0x0600123B RID: 4667
		public extern int anisoLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600123C RID: 4668
		// (set) Token: 0x0600123D RID: 4669
		public extern float mipMapBias
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x000194C0 File Offset: 0x000176C0
		public Vector2 texelSize
		{
			[NativeName("GetTexelSize")]
			get
			{
				Vector2 vector;
				this.get_texelSize_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x0600123F RID: 4671
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeTexturePtr();

		// Token: 0x06001240 RID: 4672 RVA: 0x000194D8 File Offset: 0x000176D8
		[Obsolete("Use GetNativeTexturePtr instead.", false)]
		public int GetNativeTextureID()
		{
			return (int)this.GetNativeTexturePtr();
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001241 RID: 4673
		public extern uint updateCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001242 RID: 4674
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void IncrementUpdateCount();

		// Token: 0x06001243 RID: 4675
		[NativeMethod("GetActiveTextureColorSpace")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_GetActiveTextureColorSpace();

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x000194F8 File Offset: 0x000176F8
		internal ColorSpace activeTextureColorSpace
		{
			[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule", "Unity.UIElements" })]
			get
			{
				return (this.Internal_GetActiveTextureColorSpace() == 0) ? ColorSpace.Linear : ColorSpace.Gamma;
			}
		}

		// Token: 0x06001245 RID: 4677
		[NativeMethod("GetStoredColorSpace")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern TextureColorSpace Internal_GetStoredColorSpace();

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x00019518 File Offset: 0x00017718
		public bool isDataSRGB
		{
			get
			{
				return this.Internal_GetStoredColorSpace() == TextureColorSpace.sRGB;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001247 RID: 4679
		public static extern ulong totalTextureMemory
		{
			[FreeFunction("GetTextureStreamingManager().GetTotalTextureMemory")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001248 RID: 4680
		public static extern ulong desiredTextureMemory
		{
			[FreeFunction("GetTextureStreamingManager().GetDesiredTextureMemory")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001249 RID: 4681
		public static extern ulong targetTextureMemory
		{
			[FreeFunction("GetTextureStreamingManager().GetTargetTextureMemory")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x0600124A RID: 4682
		public static extern ulong currentTextureMemory
		{
			[FreeFunction("GetTextureStreamingManager().GetCurrentTextureMemory")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600124B RID: 4683
		public static extern ulong nonStreamingTextureMemory
		{
			[FreeFunction("GetTextureStreamingManager().GetNonStreamingTextureMemory")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600124C RID: 4684
		public static extern ulong streamingMipmapUploadCount
		{
			[FreeFunction("GetTextureStreamingManager().GetStreamingMipmapUploadCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x0600124D RID: 4685
		public static extern ulong streamingRendererCount
		{
			[FreeFunction("GetTextureStreamingManager().GetStreamingRendererCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600124E RID: 4686
		public static extern ulong streamingTextureCount
		{
			[FreeFunction("GetTextureStreamingManager().GetStreamingTextureCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600124F RID: 4687
		public static extern ulong nonStreamingTextureCount
		{
			[FreeFunction("GetTextureStreamingManager().GetNonStreamingTextureCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001250 RID: 4688
		public static extern ulong streamingTexturePendingLoadCount
		{
			[FreeFunction("GetTextureStreamingManager().GetStreamingTexturePendingLoadCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001251 RID: 4689
		public static extern ulong streamingTextureLoadingCount
		{
			[FreeFunction("GetTextureStreamingManager().GetStreamingTextureLoadingCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001252 RID: 4690
		[FreeFunction("GetTextureStreamingManager().SetStreamingTextureMaterialDebugProperties")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStreamingTextureMaterialDebugProperties();

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001253 RID: 4691
		// (set) Token: 0x06001254 RID: 4692
		public static extern bool streamingTextureForceLoadAll
		{
			[FreeFunction(Name = "GetTextureStreamingManager().GetForceLoadAll")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(Name = "GetTextureStreamingManager().SetForceLoadAll")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001255 RID: 4693
		// (set) Token: 0x06001256 RID: 4694
		public static extern bool streamingTextureDiscardUnusedMips
		{
			[FreeFunction(Name = "GetTextureStreamingManager().GetDiscardUnusedMips")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(Name = "GetTextureStreamingManager().SetDiscardUnusedMips")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001257 RID: 4695
		// (set) Token: 0x06001258 RID: 4696
		public static extern bool allowThreadedTextureCreation
		{
			[FreeFunction(Name = "Texture2DScripting::IsCreateTextureThreadedEnabled")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(Name = "Texture2DScripting::EnableCreateTextureThreaded")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001259 RID: 4697
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ulong GetPixelDataSize(int mipLevel, int element = 0);

		// Token: 0x0600125A RID: 4698
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern ulong GetPixelDataOffset(int mipLevel, int element = 0);

		// Token: 0x0600125B RID: 4699 RVA: 0x00019534 File Offset: 0x00017734
		internal TextureColorSpace GetTextureColorSpace(bool linear)
		{
			return linear ? TextureColorSpace.Linear : TextureColorSpace.sRGB;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00019550 File Offset: 0x00017750
		internal TextureColorSpace GetTextureColorSpace(GraphicsFormat format)
		{
			return this.GetTextureColorSpace(!GraphicsFormatUtility.IsSRGBFormat(format));
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00019574 File Offset: 0x00017774
		internal bool ValidateFormat(RenderTextureFormat format)
		{
			bool flag = SystemInfo.SupportsRenderTextureFormat(format);
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				Debug.LogError(string.Format("RenderTexture creation failed. '{0}' is not supported on this platform. Use 'SystemInfo.SupportsRenderTextureFormat' C# API to check format support.", format.ToString()), this);
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000195B8 File Offset: 0x000177B8
		internal bool ValidateFormat(TextureFormat format)
		{
			bool flag = SystemInfo.SupportsTextureFormat(format);
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				bool flag3 = GraphicsFormatUtility.IsCompressedFormat(format) && GraphicsFormatUtility.CanDecompressFormat(GraphicsFormatUtility.GetGraphicsFormat(format, false));
				if (flag3)
				{
					Debug.LogWarning(string.Format("'{0}' is not supported on this platform. Decompressing texture. Use 'SystemInfo.SupportsTextureFormat' C# API to check format support.", format.ToString()), this);
					flag2 = true;
				}
				else
				{
					Debug.LogError(string.Format("Texture creation failed. '{0}' is not supported on this platform. Use 'SystemInfo.SupportsTextureFormat' C# API to check format support.", format.ToString()), this);
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00019638 File Offset: 0x00017838
		internal bool ValidateFormat(GraphicsFormat format, FormatUsage usage)
		{
			bool flag = usage != FormatUsage.Render && (format == GraphicsFormat.ShadowAuto || format == GraphicsFormat.DepthAuto);
			bool flag2;
			if (flag)
			{
				Debug.LogWarning(string.Format("'{0}' is not allowed because it is an auto format and not an exact format. Use GraphicsFormatUtility.GetDepthStencilFormat to get an exact depth/stencil format.", format.ToString()), this);
				flag2 = false;
			}
			else
			{
				bool flag3 = SystemInfo.IsFormatSupported(format, usage);
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					Debug.LogError(string.Format("Texture creation failed. '{0}' is not supported for {1} usage on this platform. Use 'SystemInfo.IsFormatSupported' C# API to check format support.", format.ToString(), usage.ToString()), this);
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x000196C8 File Offset: 0x000178C8
		internal UnityException CreateNonReadableException(Texture t)
		{
			return new UnityException(string.Format("Texture '{0}' is not readable, the texture memory can not be accessed from scripts. You can make the texture readable in the Texture Import Settings.", t.name));
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000196F0 File Offset: 0x000178F0
		internal UnityException CreateNativeArrayLengthOverflowException()
		{
			return new UnityException("Failed to create NativeArray, length exceeds the allowed maximum of Int32.MaxValue. Use a larger type as template argument to reduce the array length.");
		}

		// Token: 0x06001263 RID: 4707
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_texelSize_Injected(out Vector2 ret);

		// Token: 0x04000655 RID: 1621
		public static readonly int GenerateAllMips = -1;
	}
}
