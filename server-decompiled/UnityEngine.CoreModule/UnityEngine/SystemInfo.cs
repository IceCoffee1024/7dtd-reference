using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x0200028E RID: 654
	[NativeHeader("Runtime/Misc/SystemInfo.h")]
	[NativeHeader("Runtime/Shaders/GraphicsCapsScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/Mesh/MeshScriptBindings.h")]
	[NativeHeader("Runtime/Input/GetInput.h")]
	[NativeHeader("Runtime/Graphics/GraphicsFormatUtility.bindings.h")]
	[NativeHeader("Runtime/Camera/RenderLoops/MotionVectorRenderLoop.h")]
	public sealed class SystemInfo
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0002D694 File Offset: 0x0002B894
		[NativeProperty]
		public static float batteryLevel
		{
			get
			{
				return SystemInfo.GetBatteryLevel();
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0002D6AC File Offset: 0x0002B8AC
		public static BatteryStatus batteryStatus
		{
			get
			{
				return SystemInfo.GetBatteryStatus();
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
		public static string operatingSystem
		{
			get
			{
				return SystemInfo.GetOperatingSystem();
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x0002D6DC File Offset: 0x0002B8DC
		public static OperatingSystemFamily operatingSystemFamily
		{
			get
			{
				return SystemInfo.GetOperatingSystemFamily();
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x0002D6F4 File Offset: 0x0002B8F4
		public static string processorType
		{
			get
			{
				return SystemInfo.GetProcessorType();
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x0002D70C File Offset: 0x0002B90C
		public static int processorFrequency
		{
			get
			{
				return SystemInfo.GetProcessorFrequencyMHz();
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x0002D724 File Offset: 0x0002B924
		public static int processorCount
		{
			get
			{
				return SystemInfo.GetProcessorCount();
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x0002D73C File Offset: 0x0002B93C
		public static int systemMemorySize
		{
			get
			{
				return SystemInfo.GetPhysicalMemoryMB();
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x0002D754 File Offset: 0x0002B954
		public static string deviceUniqueIdentifier
		{
			get
			{
				return SystemInfo.GetDeviceUniqueIdentifier();
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x0002D76C File Offset: 0x0002B96C
		public static string deviceName
		{
			get
			{
				return SystemInfo.GetDeviceName();
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x0002D784 File Offset: 0x0002B984
		public static string deviceModel
		{
			get
			{
				return SystemInfo.GetDeviceModel();
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x0002D79C File Offset: 0x0002B99C
		public static bool supportsAccelerometer
		{
			get
			{
				return SystemInfo.SupportsAccelerometer();
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x0002D7B4 File Offset: 0x0002B9B4
		public static bool supportsGyroscope
		{
			get
			{
				return SystemInfo.IsGyroAvailable();
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0002D7CC File Offset: 0x0002B9CC
		public static bool supportsLocationService
		{
			get
			{
				return SystemInfo.SupportsLocationService();
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0002D7E4 File Offset: 0x0002B9E4
		public static bool supportsVibration
		{
			get
			{
				return SystemInfo.SupportsVibration();
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x0002D7FC File Offset: 0x0002B9FC
		public static bool supportsAudio
		{
			get
			{
				return SystemInfo.SupportsAudio();
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0002D814 File Offset: 0x0002BA14
		public static DeviceType deviceType
		{
			get
			{
				return SystemInfo.GetDeviceType();
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x0002D82C File Offset: 0x0002BA2C
		public static int graphicsMemorySize
		{
			get
			{
				return SystemInfo.GetGraphicsMemorySize();
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x0002D844 File Offset: 0x0002BA44
		public static string graphicsDeviceName
		{
			get
			{
				return SystemInfo.GetGraphicsDeviceName();
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x0002D85C File Offset: 0x0002BA5C
		public static string graphicsDeviceVendor
		{
			get
			{
				return SystemInfo.GetGraphicsDeviceVendor();
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x0002D874 File Offset: 0x0002BA74
		public static int graphicsDeviceID
		{
			get
			{
				return SystemInfo.GetGraphicsDeviceID();
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x0002D88C File Offset: 0x0002BA8C
		public static int graphicsDeviceVendorID
		{
			get
			{
				return SystemInfo.GetGraphicsDeviceVendorID();
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0002D8A4 File Offset: 0x0002BAA4
		public static GraphicsDeviceType graphicsDeviceType
		{
			get
			{
				return SystemInfo.GetGraphicsDeviceType();
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0002D8BC File Offset: 0x0002BABC
		public static bool graphicsUVStartsAtTop
		{
			get
			{
				return SystemInfo.GetGraphicsUVStartsAtTop();
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x0002D8D4 File Offset: 0x0002BAD4
		public static string graphicsDeviceVersion
		{
			get
			{
				return SystemInfo.GetGraphicsDeviceVersion();
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x0002D8EC File Offset: 0x0002BAEC
		public static int graphicsShaderLevel
		{
			get
			{
				return SystemInfo.GetGraphicsShaderLevel();
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x0002D904 File Offset: 0x0002BB04
		public static bool graphicsMultiThreaded
		{
			get
			{
				return SystemInfo.GetGraphicsMultiThreaded();
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x0002D91C File Offset: 0x0002BB1C
		public static RenderingThreadingMode renderingThreadingMode
		{
			get
			{
				return SystemInfo.GetRenderingThreadingMode();
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0002D934 File Offset: 0x0002BB34
		public static FoveatedRenderingCaps foveatedRenderingCaps
		{
			get
			{
				return SystemInfo.GetFoveatedRenderingCaps();
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x0002D94C File Offset: 0x0002BB4C
		public static bool hasHiddenSurfaceRemovalOnGPU
		{
			get
			{
				return SystemInfo.HasHiddenSurfaceRemovalOnGPU();
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001B00 RID: 6912 RVA: 0x0002D964 File Offset: 0x0002BB64
		public static bool hasDynamicUniformArrayIndexingInFragmentShaders
		{
			get
			{
				return SystemInfo.HasDynamicUniformArrayIndexingInFragmentShaders();
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x0002D97C File Offset: 0x0002BB7C
		public static bool supportsShadows
		{
			get
			{
				return SystemInfo.SupportsShadows();
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001B02 RID: 6914 RVA: 0x0002D994 File Offset: 0x0002BB94
		public static bool supportsRawShadowDepthSampling
		{
			get
			{
				return SystemInfo.SupportsRawShadowDepthSampling();
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x0002D9AC File Offset: 0x0002BBAC
		[Obsolete("supportsRenderTextures always returns true, no need to call it")]
		public static bool supportsRenderTextures
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0002D9C0 File Offset: 0x0002BBC0
		public static bool supportsMotionVectors
		{
			get
			{
				return SystemInfo.SupportsMotionVectors();
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001B05 RID: 6917 RVA: 0x0002D9D8 File Offset: 0x0002BBD8
		[Obsolete("supportsRenderToCubemap always returns true, no need to call it")]
		public static bool supportsRenderToCubemap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0002D9EC File Offset: 0x0002BBEC
		[Obsolete("supportsImageEffects always returns true, no need to call it")]
		public static bool supportsImageEffects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x0002DA00 File Offset: 0x0002BC00
		public static bool supports3DTextures
		{
			get
			{
				return SystemInfo.Supports3DTextures();
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x0002DA18 File Offset: 0x0002BC18
		public static bool supportsCompressed3DTextures
		{
			get
			{
				return SystemInfo.SupportsCompressed3DTextures();
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x0002DA30 File Offset: 0x0002BC30
		public static bool supports2DArrayTextures
		{
			get
			{
				return SystemInfo.Supports2DArrayTextures();
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x0002DA48 File Offset: 0x0002BC48
		public static bool supports3DRenderTextures
		{
			get
			{
				return SystemInfo.Supports3DRenderTextures();
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x0002DA60 File Offset: 0x0002BC60
		public static bool supportsCubemapArrayTextures
		{
			get
			{
				return SystemInfo.SupportsCubemapArrayTextures();
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x0002DA78 File Offset: 0x0002BC78
		public static bool supportsAnisotropicFilter
		{
			get
			{
				return SystemInfo.SupportsAnisotropicFilter();
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0002DA90 File Offset: 0x0002BC90
		public static CopyTextureSupport copyTextureSupport
		{
			get
			{
				return SystemInfo.GetCopyTextureSupport();
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x0002DAA8 File Offset: 0x0002BCA8
		public static bool supportsComputeShaders
		{
			get
			{
				return SystemInfo.SupportsComputeShaders();
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x0002DAC0 File Offset: 0x0002BCC0
		public static bool supportsGeometryShaders
		{
			get
			{
				return SystemInfo.SupportsGeometryShaders();
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001B10 RID: 6928 RVA: 0x0002DAD8 File Offset: 0x0002BCD8
		public static bool supportsTessellationShaders
		{
			get
			{
				return SystemInfo.SupportsTessellationShaders();
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x0002DAF0 File Offset: 0x0002BCF0
		public static bool supportsRenderTargetArrayIndexFromVertexShader
		{
			get
			{
				return SystemInfo.SupportsRenderTargetArrayIndexFromVertexShader();
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x0002DB08 File Offset: 0x0002BD08
		public static bool supportsInstancing
		{
			get
			{
				return SystemInfo.SupportsInstancing();
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0002DB20 File Offset: 0x0002BD20
		public static bool supportsHardwareQuadTopology
		{
			get
			{
				return SystemInfo.SupportsHardwareQuadTopology();
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x0002DB38 File Offset: 0x0002BD38
		public static bool supports32bitsIndexBuffer
		{
			get
			{
				return SystemInfo.Supports32bitsIndexBuffer();
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0002DB50 File Offset: 0x0002BD50
		public static bool supportsSparseTextures
		{
			get
			{
				return SystemInfo.SupportsSparseTextures();
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0002DB68 File Offset: 0x0002BD68
		public static int supportedRenderTargetCount
		{
			get
			{
				return SystemInfo.SupportedRenderTargetCount();
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0002DB80 File Offset: 0x0002BD80
		public static bool supportsSeparatedRenderTargetsBlend
		{
			get
			{
				return SystemInfo.SupportsSeparatedRenderTargetsBlend();
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x0002DB98 File Offset: 0x0002BD98
		public static int supportedRandomWriteTargetCount
		{
			get
			{
				return SystemInfo.SupportedRandomWriteTargetCount();
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0002DBB0 File Offset: 0x0002BDB0
		public static int supportsMultisampledTextures
		{
			get
			{
				return SystemInfo.SupportsMultisampledTextures();
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0002DBC8 File Offset: 0x0002BDC8
		public static bool supportsMultisampled2DArrayTextures
		{
			get
			{
				return SystemInfo.SupportsMultisampled2DArrayTextures();
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x0002DBE0 File Offset: 0x0002BDE0
		public static bool supportsMultisampleAutoResolve
		{
			get
			{
				return SystemInfo.SupportsMultisampleAutoResolve();
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x0002DBF8 File Offset: 0x0002BDF8
		public static int supportsTextureWrapMirrorOnce
		{
			get
			{
				return SystemInfo.SupportsTextureWrapMirrorOnce();
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x0002DC10 File Offset: 0x0002BE10
		public static bool usesReversedZBuffer
		{
			get
			{
				return SystemInfo.UsesReversedZBuffer();
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x0002DC28 File Offset: 0x0002BE28
		[Obsolete("supportsStencil always returns true, no need to call it")]
		public static int supportsStencil
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0002DC3C File Offset: 0x0002BE3C
		private static bool IsValidEnumValue(Enum value)
		{
			bool flag = !Enum.IsDefined(value.GetType(), value);
			return !flag;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0002DC68 File Offset: 0x0002BE68
		public static bool SupportsRenderTextureFormat(RenderTextureFormat format)
		{
			bool flag = !SystemInfo.IsValidEnumValue(format);
			if (flag)
			{
				throw new ArgumentException("Failed SupportsRenderTextureFormat; format is not a valid RenderTextureFormat");
			}
			return SystemInfo.HasRenderTextureNative(format);
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0002DCA0 File Offset: 0x0002BEA0
		public static bool SupportsBlendingOnRenderTextureFormat(RenderTextureFormat format)
		{
			bool flag = !SystemInfo.IsValidEnumValue(format);
			if (flag)
			{
				throw new ArgumentException("Failed SupportsBlendingOnRenderTextureFormat; format is not a valid RenderTextureFormat");
			}
			return SystemInfo.SupportsBlendingOnRenderTextureFormatNative(format);
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0002DCD8 File Offset: 0x0002BED8
		public static bool SupportsRandomWriteOnRenderTextureFormat(RenderTextureFormat format)
		{
			bool flag = !SystemInfo.IsValidEnumValue(format);
			if (flag)
			{
				throw new ArgumentException("Failed SupportsRandomWriteOnRenderTextureFormat; format is not a valid RenderTextureFormat");
			}
			return SystemInfo.SupportsRandomWriteOnRenderTextureFormatNative(format);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0002DD10 File Offset: 0x0002BF10
		public static bool SupportsTextureFormat(TextureFormat format)
		{
			bool flag = !SystemInfo.IsValidEnumValue(format);
			if (flag)
			{
				throw new ArgumentException("Failed SupportsTextureFormat; format is not a valid TextureFormat");
			}
			return SystemInfo.SupportsTextureFormatNative(format);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0002DD48 File Offset: 0x0002BF48
		public static bool SupportsVertexAttributeFormat(VertexAttributeFormat format, int dimension)
		{
			bool flag = !SystemInfo.IsValidEnumValue(format);
			if (flag)
			{
				throw new ArgumentException("Failed SupportsVertexAttributeFormat; format is not a valid VertexAttributeFormat");
			}
			bool flag2 = dimension < 1 || dimension > 4;
			if (flag2)
			{
				throw new ArgumentException("Failed SupportsVertexAttributeFormat; dimension must be in 1..4 range");
			}
			return SystemInfo.SupportsVertexAttributeFormatNative(format, dimension);
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x0002DD98 File Offset: 0x0002BF98
		public static NPOTSupport npotSupport
		{
			get
			{
				return SystemInfo.GetNPOTSupport();
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x0002DDB0 File Offset: 0x0002BFB0
		public static int maxTextureSize
		{
			get
			{
				return SystemInfo.GetMaxTextureSize();
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0002DDC8 File Offset: 0x0002BFC8
		public static int maxTexture3DSize
		{
			get
			{
				return SystemInfo.GetMaxTexture3DSize();
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x0002DDE0 File Offset: 0x0002BFE0
		public static int maxTextureArraySlices
		{
			get
			{
				return SystemInfo.GetMaxTextureArraySlices();
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0002DDF8 File Offset: 0x0002BFF8
		public static int maxCubemapSize
		{
			get
			{
				return SystemInfo.GetMaxCubemapSize();
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x0002DE10 File Offset: 0x0002C010
		public static int maxAnisotropyLevel
		{
			get
			{
				return SystemInfo.GetMaxAnisotropyLevel();
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0002DE28 File Offset: 0x0002C028
		internal static int maxRenderTextureSize
		{
			get
			{
				return SystemInfo.GetMaxRenderTextureSize();
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x0002DE40 File Offset: 0x0002C040
		public static int maxComputeBufferInputsVertex
		{
			get
			{
				return SystemInfo.MaxComputeBufferInputsVertex();
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0002DE58 File Offset: 0x0002C058
		public static int maxComputeBufferInputsFragment
		{
			get
			{
				return SystemInfo.MaxComputeBufferInputsFragment();
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x0002DE70 File Offset: 0x0002C070
		public static int maxComputeBufferInputsGeometry
		{
			get
			{
				return SystemInfo.MaxComputeBufferInputsGeometry();
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001B2F RID: 6959 RVA: 0x0002DE88 File Offset: 0x0002C088
		public static int maxComputeBufferInputsDomain
		{
			get
			{
				return SystemInfo.MaxComputeBufferInputsDomain();
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001B30 RID: 6960 RVA: 0x0002DEA0 File Offset: 0x0002C0A0
		public static int maxComputeBufferInputsHull
		{
			get
			{
				return SystemInfo.MaxComputeBufferInputsHull();
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001B31 RID: 6961 RVA: 0x0002DEB8 File Offset: 0x0002C0B8
		public static int maxComputeBufferInputsCompute
		{
			get
			{
				return SystemInfo.MaxComputeBufferInputsCompute();
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x0002DED0 File Offset: 0x0002C0D0
		public static int maxComputeWorkGroupSize
		{
			get
			{
				return SystemInfo.GetMaxComputeWorkGroupSize();
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x0002DEE8 File Offset: 0x0002C0E8
		public static int maxComputeWorkGroupSizeX
		{
			get
			{
				return SystemInfo.GetMaxComputeWorkGroupSizeX();
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x0002DF00 File Offset: 0x0002C100
		public static int maxComputeWorkGroupSizeY
		{
			get
			{
				return SystemInfo.GetMaxComputeWorkGroupSizeY();
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x0002DF18 File Offset: 0x0002C118
		public static int maxComputeWorkGroupSizeZ
		{
			get
			{
				return SystemInfo.GetMaxComputeWorkGroupSizeZ();
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001B36 RID: 6966 RVA: 0x0002DF30 File Offset: 0x0002C130
		public static int computeSubGroupSize
		{
			get
			{
				return SystemInfo.GetComputeSubGroupSize();
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x0002DF48 File Offset: 0x0002C148
		public static bool supportsAsyncCompute
		{
			get
			{
				return SystemInfo.SupportsAsyncCompute();
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x0002DF60 File Offset: 0x0002C160
		public static bool supportsGpuRecorder
		{
			get
			{
				return SystemInfo.SupportsGpuRecorder();
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x0002DF78 File Offset: 0x0002C178
		public static bool supportsGraphicsFence
		{
			get
			{
				return SystemInfo.SupportsGPUFence();
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x0002DF90 File Offset: 0x0002C190
		public static bool supportsAsyncGPUReadback
		{
			get
			{
				return SystemInfo.SupportsAsyncGPUReadback();
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x0002DFA8 File Offset: 0x0002C1A8
		public static bool supportsRayTracing
		{
			get
			{
				return SystemInfo.SupportsRayTracing();
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001B3C RID: 6972 RVA: 0x0002DFC0 File Offset: 0x0002C1C0
		public static bool supportsSetConstantBuffer
		{
			get
			{
				return SystemInfo.SupportsSetConstantBuffer();
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x0002DFD8 File Offset: 0x0002C1D8
		public static int constantBufferOffsetAlignment
		{
			get
			{
				return SystemInfo.MinConstantBufferOffsetAlignment();
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x0002DFF0 File Offset: 0x0002C1F0
		public static int maxConstantBufferSize
		{
			get
			{
				return SystemInfo.MaxConstantBufferSize();
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001B3F RID: 6975 RVA: 0x0002E008 File Offset: 0x0002C208
		public static long maxGraphicsBufferSize
		{
			get
			{
				return SystemInfo.MaxGraphicsBufferSize();
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x0002E020 File Offset: 0x0002C220
		[Obsolete("Use SystemInfo.constantBufferOffsetAlignment instead.")]
		public static bool minConstantBufferOffsetAlignment
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001B41 RID: 6977 RVA: 0x0002E034 File Offset: 0x0002C234
		public static bool hasMipMaxLevel
		{
			get
			{
				return SystemInfo.HasMipMaxLevel();
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x0002E04C File Offset: 0x0002C24C
		public static bool supportsMipStreaming
		{
			get
			{
				return SystemInfo.SupportsMipStreaming();
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x0002E064 File Offset: 0x0002C264
		[Obsolete("graphicsPixelFillrate is no longer supported in Unity 5.0+.")]
		public static int graphicsPixelFillrate
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x0002E078 File Offset: 0x0002C278
		public static bool usesLoadStoreActions
		{
			get
			{
				return SystemInfo.UsesLoadStoreActions();
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001B45 RID: 6981 RVA: 0x0002E090 File Offset: 0x0002C290
		public static HDRDisplaySupportFlags hdrDisplaySupportFlags
		{
			get
			{
				return SystemInfo.GetHDRDisplaySupportFlags();
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x0002E0A8 File Offset: 0x0002C2A8
		public static bool supportsConservativeRaster
		{
			get
			{
				return SystemInfo.SupportsConservativeRaster();
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x0002E0C0 File Offset: 0x0002C2C0
		public static bool supportsMultiview
		{
			get
			{
				return SystemInfo.SupportsMultiview();
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x0002E0D8 File Offset: 0x0002C2D8
		public static bool supportsStoreAndResolveAction
		{
			get
			{
				return SystemInfo.SupportsStoreAndResolveAction();
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001B49 RID: 6985 RVA: 0x0002E0F0 File Offset: 0x0002C2F0
		public static bool supportsMultisampleResolveDepth
		{
			get
			{
				return SystemInfo.SupportsMultisampleResolveDepth();
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001B4A RID: 6986 RVA: 0x0002E108 File Offset: 0x0002C308
		public static bool supportsMultisampleResolveStencil
		{
			get
			{
				return SystemInfo.SupportsMultisampleResolveStencil();
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001B4B RID: 6987 RVA: 0x0002E120 File Offset: 0x0002C320
		public static bool supportsIndirectArgumentsBuffer
		{
			get
			{
				return SystemInfo.SupportsIndirectArgumentsBuffer();
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001B4C RID: 6988 RVA: 0x0002E138 File Offset: 0x0002C338
		[Obsolete("Vertex program support is required in Unity 5.0+")]
		public static bool supportsVertexPrograms
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001B4D RID: 6989
		[FreeFunction("systeminfo::GetBatteryLevel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetBatteryLevel();

		// Token: 0x06001B4E RID: 6990
		[FreeFunction("systeminfo::GetBatteryStatus")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern BatteryStatus GetBatteryStatus();

		// Token: 0x06001B4F RID: 6991
		[FreeFunction("systeminfo::GetOperatingSystem")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetOperatingSystem();

		// Token: 0x06001B50 RID: 6992
		[FreeFunction("systeminfo::GetOperatingSystemFamily")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern OperatingSystemFamily GetOperatingSystemFamily();

		// Token: 0x06001B51 RID: 6993
		[FreeFunction("systeminfo::GetProcessorType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetProcessorType();

		// Token: 0x06001B52 RID: 6994
		[FreeFunction("systeminfo::GetProcessorFrequencyMHz")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetProcessorFrequencyMHz();

		// Token: 0x06001B53 RID: 6995
		[FreeFunction("systeminfo::GetProcessorCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetProcessorCount();

		// Token: 0x06001B54 RID: 6996
		[FreeFunction("systeminfo::GetPhysicalMemoryMB")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPhysicalMemoryMB();

		// Token: 0x06001B55 RID: 6997
		[FreeFunction("systeminfo::GetDeviceUniqueIdentifier")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetDeviceUniqueIdentifier();

		// Token: 0x06001B56 RID: 6998
		[FreeFunction("systeminfo::GetDeviceName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetDeviceName();

		// Token: 0x06001B57 RID: 6999
		[FreeFunction("systeminfo::GetDeviceModel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetDeviceModel();

		// Token: 0x06001B58 RID: 7000
		[FreeFunction("systeminfo::SupportsAccelerometer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsAccelerometer();

		// Token: 0x06001B59 RID: 7001
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsGyroAvailable();

		// Token: 0x06001B5A RID: 7002
		[FreeFunction("systeminfo::SupportsLocationService")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsLocationService();

		// Token: 0x06001B5B RID: 7003
		[FreeFunction("systeminfo::SupportsVibration")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsVibration();

		// Token: 0x06001B5C RID: 7004
		[FreeFunction("systeminfo::SupportsAudio")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsAudio();

		// Token: 0x06001B5D RID: 7005
		[FreeFunction("systeminfo::GetDeviceType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern DeviceType GetDeviceType();

		// Token: 0x06001B5E RID: 7006
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsMemorySize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGraphicsMemorySize();

		// Token: 0x06001B5F RID: 7007
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsDeviceName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetGraphicsDeviceName();

		// Token: 0x06001B60 RID: 7008
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsDeviceVendor")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetGraphicsDeviceVendor();

		// Token: 0x06001B61 RID: 7009
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsDeviceID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGraphicsDeviceID();

		// Token: 0x06001B62 RID: 7010
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsDeviceVendorID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGraphicsDeviceVendorID();

		// Token: 0x06001B63 RID: 7011
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsDeviceType")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GraphicsDeviceType GetGraphicsDeviceType();

		// Token: 0x06001B64 RID: 7012
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsUVStartsAtTop")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetGraphicsUVStartsAtTop();

		// Token: 0x06001B65 RID: 7013
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsDeviceVersion")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetGraphicsDeviceVersion();

		// Token: 0x06001B66 RID: 7014
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsShaderLevel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGraphicsShaderLevel();

		// Token: 0x06001B67 RID: 7015
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsMultiThreaded")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetGraphicsMultiThreaded();

		// Token: 0x06001B68 RID: 7016
		[FreeFunction("ScriptingGraphicsCaps::GetRenderingThreadingMode")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderingThreadingMode GetRenderingThreadingMode();

		// Token: 0x06001B69 RID: 7017
		[FreeFunction("ScriptingGraphicsCaps::GetFoveatedRenderingCaps")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FoveatedRenderingCaps GetFoveatedRenderingCaps();

		// Token: 0x06001B6A RID: 7018
		[FreeFunction("ScriptingGraphicsCaps::HasHiddenSurfaceRemovalOnGPU")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasHiddenSurfaceRemovalOnGPU();

		// Token: 0x06001B6B RID: 7019
		[FreeFunction("ScriptingGraphicsCaps::HasDynamicUniformArrayIndexingInFragmentShaders")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasDynamicUniformArrayIndexingInFragmentShaders();

		// Token: 0x06001B6C RID: 7020
		[FreeFunction("ScriptingGraphicsCaps::SupportsShadows")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsShadows();

		// Token: 0x06001B6D RID: 7021
		[FreeFunction("ScriptingGraphicsCaps::SupportsRawShadowDepthSampling")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsRawShadowDepthSampling();

		// Token: 0x06001B6E RID: 7022
		[FreeFunction("SupportsMotionVectors")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMotionVectors();

		// Token: 0x06001B6F RID: 7023
		[FreeFunction("ScriptingGraphicsCaps::Supports3DTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Supports3DTextures();

		// Token: 0x06001B70 RID: 7024
		[FreeFunction("ScriptingGraphicsCaps::SupportsCompressed3DTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsCompressed3DTextures();

		// Token: 0x06001B71 RID: 7025
		[FreeFunction("ScriptingGraphicsCaps::Supports2DArrayTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Supports2DArrayTextures();

		// Token: 0x06001B72 RID: 7026
		[FreeFunction("ScriptingGraphicsCaps::Supports3DRenderTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Supports3DRenderTextures();

		// Token: 0x06001B73 RID: 7027
		[FreeFunction("ScriptingGraphicsCaps::SupportsCubemapArrayTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsCubemapArrayTextures();

		// Token: 0x06001B74 RID: 7028
		[FreeFunction("ScriptingGraphicsCaps::SupportsAnisotropicFilter")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsAnisotropicFilter();

		// Token: 0x06001B75 RID: 7029
		[FreeFunction("ScriptingGraphicsCaps::GetCopyTextureSupport")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CopyTextureSupport GetCopyTextureSupport();

		// Token: 0x06001B76 RID: 7030
		[FreeFunction("ScriptingGraphicsCaps::SupportsComputeShaders")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsComputeShaders();

		// Token: 0x06001B77 RID: 7031
		[FreeFunction("ScriptingGraphicsCaps::SupportsGeometryShaders")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsGeometryShaders();

		// Token: 0x06001B78 RID: 7032
		[FreeFunction("ScriptingGraphicsCaps::SupportsTessellationShaders")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsTessellationShaders();

		// Token: 0x06001B79 RID: 7033
		[FreeFunction("ScriptingGraphicsCaps::SupportsRenderTargetArrayIndexFromVertexShader")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsRenderTargetArrayIndexFromVertexShader();

		// Token: 0x06001B7A RID: 7034
		[FreeFunction("ScriptingGraphicsCaps::SupportsInstancing")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsInstancing();

		// Token: 0x06001B7B RID: 7035
		[FreeFunction("ScriptingGraphicsCaps::SupportsHardwareQuadTopology")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsHardwareQuadTopology();

		// Token: 0x06001B7C RID: 7036
		[FreeFunction("ScriptingGraphicsCaps::Supports32bitsIndexBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Supports32bitsIndexBuffer();

		// Token: 0x06001B7D RID: 7037
		[FreeFunction("ScriptingGraphicsCaps::SupportsSparseTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsSparseTextures();

		// Token: 0x06001B7E RID: 7038
		[FreeFunction("ScriptingGraphicsCaps::SupportedRenderTargetCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SupportedRenderTargetCount();

		// Token: 0x06001B7F RID: 7039
		[FreeFunction("ScriptingGraphicsCaps::SupportsSeparatedRenderTargetsBlend")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsSeparatedRenderTargetsBlend();

		// Token: 0x06001B80 RID: 7040
		[FreeFunction("ScriptingGraphicsCaps::SupportedRandomWriteTargetCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SupportedRandomWriteTargetCount();

		// Token: 0x06001B81 RID: 7041
		[FreeFunction("ScriptingGraphicsCaps::MaxComputeBufferInputsVertex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxComputeBufferInputsVertex();

		// Token: 0x06001B82 RID: 7042
		[FreeFunction("ScriptingGraphicsCaps::MaxComputeBufferInputsFragment")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxComputeBufferInputsFragment();

		// Token: 0x06001B83 RID: 7043
		[FreeFunction("ScriptingGraphicsCaps::MaxComputeBufferInputsGeometry")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxComputeBufferInputsGeometry();

		// Token: 0x06001B84 RID: 7044
		[FreeFunction("ScriptingGraphicsCaps::MaxComputeBufferInputsDomain")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxComputeBufferInputsDomain();

		// Token: 0x06001B85 RID: 7045
		[FreeFunction("ScriptingGraphicsCaps::MaxComputeBufferInputsHull")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxComputeBufferInputsHull();

		// Token: 0x06001B86 RID: 7046
		[FreeFunction("ScriptingGraphicsCaps::MaxComputeBufferInputsCompute")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxComputeBufferInputsCompute();

		// Token: 0x06001B87 RID: 7047
		[FreeFunction("ScriptingGraphicsCaps::SupportsMultisampledTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SupportsMultisampledTextures();

		// Token: 0x06001B88 RID: 7048
		[FreeFunction("ScriptingGraphicsCaps::SupportsMultisampled2DArrayTextures")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMultisampled2DArrayTextures();

		// Token: 0x06001B89 RID: 7049
		[FreeFunction("ScriptingGraphicsCaps::SupportsMultisampleAutoResolve")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMultisampleAutoResolve();

		// Token: 0x06001B8A RID: 7050
		[FreeFunction("ScriptingGraphicsCaps::SupportsTextureWrapMirrorOnce")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SupportsTextureWrapMirrorOnce();

		// Token: 0x06001B8B RID: 7051
		[FreeFunction("ScriptingGraphicsCaps::UsesReversedZBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UsesReversedZBuffer();

		// Token: 0x06001B8C RID: 7052
		[FreeFunction("ScriptingGraphicsCaps::HasRenderTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasRenderTextureNative(RenderTextureFormat format);

		// Token: 0x06001B8D RID: 7053
		[FreeFunction("ScriptingGraphicsCaps::SupportsBlendingOnRenderTextureFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsBlendingOnRenderTextureFormatNative(RenderTextureFormat format);

		// Token: 0x06001B8E RID: 7054
		[FreeFunction("ScriptingGraphicsCaps::SupportsRandomWriteOnRenderTextureFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsRandomWriteOnRenderTextureFormatNative(RenderTextureFormat format);

		// Token: 0x06001B8F RID: 7055
		[FreeFunction("ScriptingGraphicsCaps::SupportsTextureFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsTextureFormatNative(TextureFormat format);

		// Token: 0x06001B90 RID: 7056
		[FreeFunction("ScriptingGraphicsCaps::SupportsVertexAttributeFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsVertexAttributeFormatNative(VertexAttributeFormat format, int dimension);

		// Token: 0x06001B91 RID: 7057
		[FreeFunction("ScriptingGraphicsCaps::GetNPOTSupport")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern NPOTSupport GetNPOTSupport();

		// Token: 0x06001B92 RID: 7058
		[FreeFunction("ScriptingGraphicsCaps::GetMaxTextureSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxTextureSize();

		// Token: 0x06001B93 RID: 7059
		[FreeFunction("ScriptingGraphicsCaps::GetMaxTexture3DSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxTexture3DSize();

		// Token: 0x06001B94 RID: 7060
		[FreeFunction("ScriptingGraphicsCaps::GetMaxTextureArraySlices")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxTextureArraySlices();

		// Token: 0x06001B95 RID: 7061
		[FreeFunction("ScriptingGraphicsCaps::GetMaxCubemapSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxCubemapSize();

		// Token: 0x06001B96 RID: 7062
		[FreeFunction("ScriptingGraphicsCaps::GetMaxAnisotropyLevel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxAnisotropyLevel();

		// Token: 0x06001B97 RID: 7063
		[FreeFunction("ScriptingGraphicsCaps::GetMaxRenderTextureSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxRenderTextureSize();

		// Token: 0x06001B98 RID: 7064
		[FreeFunction("ScriptingGraphicsCaps::GetMaxComputeWorkGroupSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxComputeWorkGroupSize();

		// Token: 0x06001B99 RID: 7065
		[FreeFunction("ScriptingGraphicsCaps::GetMaxComputeWorkGroupSizeX")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxComputeWorkGroupSizeX();

		// Token: 0x06001B9A RID: 7066
		[FreeFunction("ScriptingGraphicsCaps::GetMaxComputeWorkGroupSizeY")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxComputeWorkGroupSizeY();

		// Token: 0x06001B9B RID: 7067
		[FreeFunction("ScriptingGraphicsCaps::GetMaxComputeWorkGroupSizeZ")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxComputeWorkGroupSizeZ();

		// Token: 0x06001B9C RID: 7068
		[FreeFunction("ScriptingGraphicsCaps::GetComputeSubGroupSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetComputeSubGroupSize();

		// Token: 0x06001B9D RID: 7069
		[FreeFunction("ScriptingGraphicsCaps::SupportsAsyncCompute")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsAsyncCompute();

		// Token: 0x06001B9E RID: 7070
		[FreeFunction("ScriptingGraphicsCaps::SupportsGpuRecorder")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsGpuRecorder();

		// Token: 0x06001B9F RID: 7071
		[FreeFunction("ScriptingGraphicsCaps::SupportsGPUFence")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsGPUFence();

		// Token: 0x06001BA0 RID: 7072
		[FreeFunction("ScriptingGraphicsCaps::SupportsAsyncGPUReadback")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsAsyncGPUReadback();

		// Token: 0x06001BA1 RID: 7073
		[FreeFunction("ScriptingGraphicsCaps::SupportsRayTracing")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsRayTracing();

		// Token: 0x06001BA2 RID: 7074
		[FreeFunction("ScriptingGraphicsCaps::SupportsSetConstantBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsSetConstantBuffer();

		// Token: 0x06001BA3 RID: 7075
		[FreeFunction("ScriptingGraphicsCaps::MinConstantBufferOffsetAlignment")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MinConstantBufferOffsetAlignment();

		// Token: 0x06001BA4 RID: 7076
		[FreeFunction("ScriptingGraphicsCaps::MaxConstantBufferSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int MaxConstantBufferSize();

		// Token: 0x06001BA5 RID: 7077
		[FreeFunction("ScriptingGraphicsCaps::MaxGraphicsBufferSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long MaxGraphicsBufferSize();

		// Token: 0x06001BA6 RID: 7078
		[FreeFunction("ScriptingGraphicsCaps::HasMipMaxLevel")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasMipMaxLevel();

		// Token: 0x06001BA7 RID: 7079
		[FreeFunction("ScriptingGraphicsCaps::SupportsMipStreaming")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMipStreaming();

		// Token: 0x06001BA8 RID: 7080
		[FreeFunction("ScriptingGraphicsCaps::IsFormatSupported")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsFormatSupported(GraphicsFormat format, FormatUsage usage);

		// Token: 0x06001BA9 RID: 7081
		[FreeFunction("ScriptingGraphicsCaps::GetCompatibleFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GraphicsFormat GetCompatibleFormat(GraphicsFormat format, FormatUsage usage);

		// Token: 0x06001BAA RID: 7082
		[FreeFunction("ScriptingGraphicsCaps::GetGraphicsFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GraphicsFormat GetGraphicsFormat(DefaultFormat format);

		// Token: 0x06001BAB RID: 7083 RVA: 0x0002E14B File Offset: 0x0002C34B
		[FreeFunction("ScriptingGraphicsCaps::GetRenderTextureSupportedMSAASampleCount")]
		public static int GetRenderTextureSupportedMSAASampleCount(RenderTextureDescriptor desc)
		{
			return SystemInfo.GetRenderTextureSupportedMSAASampleCount_Injected(ref desc);
		}

		// Token: 0x06001BAC RID: 7084
		[FreeFunction("ScriptingGraphicsCaps::UsesLoadStoreActions")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool UsesLoadStoreActions();

		// Token: 0x06001BAD RID: 7085
		[FreeFunction("ScriptingGraphicsCaps::GetHDRDisplaySupportFlags")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern HDRDisplaySupportFlags GetHDRDisplaySupportFlags();

		// Token: 0x06001BAE RID: 7086
		[FreeFunction("ScriptingGraphicsCaps::SupportsConservativeRaster")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsConservativeRaster();

		// Token: 0x06001BAF RID: 7087
		[FreeFunction("ScriptingGraphicsCaps::SupportsMultiview")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMultiview();

		// Token: 0x06001BB0 RID: 7088
		[FreeFunction("ScriptingGraphicsCaps::SupportsStoreAndResolveAction")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsStoreAndResolveAction();

		// Token: 0x06001BB1 RID: 7089
		[FreeFunction("ScriptingGraphicsCaps::SupportsMultisampleResolveDepth")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMultisampleResolveDepth();

		// Token: 0x06001BB2 RID: 7090
		[FreeFunction("ScriptingGraphicsCaps::SupportsMultisampleResolveStencil")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsMultisampleResolveStencil();

		// Token: 0x06001BB3 RID: 7091
		[FreeFunction("ScriptingGraphicsCaps::SupportsIndirectArgumentsBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SupportsIndirectArgumentsBuffer();

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x0002E154 File Offset: 0x0002C354
		[Obsolete("SystemInfo.supportsGPUFence has been deprecated, use SystemInfo.supportsGraphicsFence instead (UnityUpgradable) ->  supportsGraphicsFence", true)]
		public static bool supportsGPUFence
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001BB6 RID: 7094
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetRenderTextureSupportedMSAASampleCount_Injected(ref RenderTextureDescriptor desc);

		// Token: 0x04000953 RID: 2387
		public const string unsupportedIdentifier = "n/a";
	}
}
