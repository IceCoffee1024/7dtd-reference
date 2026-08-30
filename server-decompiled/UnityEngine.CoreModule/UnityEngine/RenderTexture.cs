using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001D6 RID: 470
	[UsedByNativeCode]
	[NativeHeader("Runtime/Camera/Camera.h")]
	[NativeHeader("Runtime/Graphics/RenderTexture.h")]
	[NativeHeader("Runtime/Graphics/RenderBufferManager.h")]
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public class RenderTexture : Texture
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001392 RID: 5010
		// (set) Token: 0x06001393 RID: 5011
		public override extern int width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001394 RID: 5012
		// (set) Token: 0x06001395 RID: 5013
		public override extern int height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001396 RID: 5014
		// (set) Token: 0x06001397 RID: 5015
		public override extern TextureDimension dimension
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06001398 RID: 5016
		[NativeName("GetColorFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern GraphicsFormat GetColorFormat(bool suppressWarnings);

		// Token: 0x06001399 RID: 5017
		[NativeName("SetColorFormat")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColorFormat(GraphicsFormat format);

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x0001BC98 File Offset: 0x00019E98
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x0001BCB1 File Offset: 0x00019EB1
		public new GraphicsFormat graphicsFormat
		{
			get
			{
				return this.GetColorFormat(true);
			}
			set
			{
				this.SetColorFormat(value);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600139C RID: 5020
		// (set) Token: 0x0600139D RID: 5021
		[NativeProperty("MipMap")]
		public extern bool useMipMap
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x0600139E RID: 5022
		[NativeProperty("SRGBReadWrite")]
		public extern bool sRGB
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x0600139F RID: 5023
		// (set) Token: 0x060013A0 RID: 5024
		[NativeProperty("VRUsage")]
		public extern VRTextureUsage vrUsage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060013A1 RID: 5025
		// (set) Token: 0x060013A2 RID: 5026
		[NativeProperty("Memoryless")]
		public extern RenderTextureMemoryless memorylessMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x0001BCBC File Offset: 0x00019EBC
		// (set) Token: 0x060013A4 RID: 5028 RVA: 0x0001BD00 File Offset: 0x00019F00
		public RenderTextureFormat format
		{
			get
			{
				bool flag = this.graphicsFormat > GraphicsFormat.None;
				RenderTextureFormat renderTextureFormat;
				if (flag)
				{
					renderTextureFormat = GraphicsFormatUtility.GetRenderTextureFormat(this.graphicsFormat);
				}
				else
				{
					renderTextureFormat = ((this.GetDescriptor().shadowSamplingMode != ShadowSamplingMode.None) ? RenderTextureFormat.Shadowmap : RenderTextureFormat.Depth);
				}
				return renderTextureFormat;
			}
			set
			{
				this.graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(value, this.sRGB);
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060013A5 RID: 5029
		// (set) Token: 0x060013A6 RID: 5030
		public extern GraphicsFormat stencilFormat
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060013A7 RID: 5031
		// (set) Token: 0x060013A8 RID: 5032
		public extern GraphicsFormat depthStencilFormat
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060013A9 RID: 5033
		// (set) Token: 0x060013AA RID: 5034
		public extern bool autoGenerateMips
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060013AB RID: 5035
		// (set) Token: 0x060013AC RID: 5036
		public extern int volumeDepth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060013AD RID: 5037
		// (set) Token: 0x060013AE RID: 5038
		public extern int antiAliasing
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060013AF RID: 5039
		// (set) Token: 0x060013B0 RID: 5040
		public extern bool bindTextureMS
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060013B1 RID: 5041
		// (set) Token: 0x060013B2 RID: 5042
		public extern bool enableRandomWrite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060013B3 RID: 5043
		// (set) Token: 0x060013B4 RID: 5044
		public extern bool useDynamicScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060013B5 RID: 5045
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetIsPowerOfTwo();

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0001BD18 File Offset: 0x00019F18
		// (set) Token: 0x060013B7 RID: 5047 RVA: 0x00002669 File Offset: 0x00000869
		public bool isPowerOfTwo
		{
			get
			{
				return this.GetIsPowerOfTwo();
			}
			set
			{
			}
		}

		// Token: 0x060013B8 RID: 5048
		[FreeFunction("RenderTexture::GetActive")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderTexture GetActive();

		// Token: 0x060013B9 RID: 5049
		[FreeFunction("RenderTextureScripting::SetActive")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetActive(RenderTexture rt);

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x0001BD30 File Offset: 0x00019F30
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x0001BD47 File Offset: 0x00019F47
		public static RenderTexture active
		{
			get
			{
				return RenderTexture.GetActive();
			}
			set
			{
				RenderTexture.SetActive(value);
			}
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0001BD54 File Offset: 0x00019F54
		[FreeFunction(Name = "RenderTextureScripting::GetColorBuffer", HasExplicitThis = true)]
		private RenderBuffer GetColorBuffer()
		{
			RenderBuffer renderBuffer;
			this.GetColorBuffer_Injected(out renderBuffer);
			return renderBuffer;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0001BD6C File Offset: 0x00019F6C
		[FreeFunction(Name = "RenderTextureScripting::GetDepthBuffer", HasExplicitThis = true)]
		private RenderBuffer GetDepthBuffer()
		{
			RenderBuffer renderBuffer;
			this.GetDepthBuffer_Injected(out renderBuffer);
			return renderBuffer;
		}

		// Token: 0x060013BE RID: 5054
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMipMapCount(int count);

		// Token: 0x060013BF RID: 5055
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetShadowSamplingMode(ShadowSamplingMode samplingMode);

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0001BD84 File Offset: 0x00019F84
		public RenderBuffer colorBuffer
		{
			get
			{
				return this.GetColorBuffer();
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0001BD9C File Offset: 0x00019F9C
		public RenderBuffer depthBuffer
		{
			get
			{
				return this.GetDepthBuffer();
			}
		}

		// Token: 0x060013C2 RID: 5058
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeDepthBufferPtr();

		// Token: 0x060013C3 RID: 5059
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DiscardContents(bool discardColor, bool discardDepth);

		// Token: 0x060013C4 RID: 5060
		[Obsolete("This function has no effect.", false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void MarkRestoreExpected();

		// Token: 0x060013C5 RID: 5061 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		public void DiscardContents()
		{
			this.DiscardContents(true, true);
		}

		// Token: 0x060013C6 RID: 5062
		[NativeName("ResolveAntiAliasedSurface")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResolveAA();

		// Token: 0x060013C7 RID: 5063
		[NativeName("ResolveAntiAliasedSurface")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ResolveAATo(RenderTexture rt);

		// Token: 0x060013C8 RID: 5064 RVA: 0x0001BDC0 File Offset: 0x00019FC0
		public void ResolveAntiAliasedSurface()
		{
			this.ResolveAA();
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0001BDCA File Offset: 0x00019FCA
		public void ResolveAntiAliasedSurface(RenderTexture target)
		{
			this.ResolveAATo(target);
		}

		// Token: 0x060013CA RID: 5066
		[FreeFunction(Name = "RenderTextureScripting::SetGlobalShaderProperty", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalShaderProperty(string propertyName);

		// Token: 0x060013CB RID: 5067
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Create();

		// Token: 0x060013CC RID: 5068
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Release();

		// Token: 0x060013CD RID: 5069
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsCreated();

		// Token: 0x060013CE RID: 5070
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GenerateMips();

		// Token: 0x060013CF RID: 5071
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ConvertToEquirect(RenderTexture equirect, Camera.MonoOrStereoscopicEye eye = Camera.MonoOrStereoscopicEye.Mono);

		// Token: 0x060013D0 RID: 5072
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetSRGBReadWrite(bool srgb);

		// Token: 0x060013D1 RID: 5073
		[FreeFunction("RenderTextureScripting::Create")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] RenderTexture rt);

		// Token: 0x060013D2 RID: 5074
		[FreeFunction("RenderTextureSupportsStencil")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SupportsStencil(RenderTexture rt);

		// Token: 0x060013D3 RID: 5075 RVA: 0x0001BDD5 File Offset: 0x00019FD5
		[NativeName("SetRenderTextureDescFromScript")]
		private void SetRenderTextureDescriptor(RenderTextureDescriptor desc)
		{
			this.SetRenderTextureDescriptor_Injected(ref desc);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0001BDE0 File Offset: 0x00019FE0
		[NativeName("GetRenderTextureDesc")]
		private RenderTextureDescriptor GetDescriptor()
		{
			RenderTextureDescriptor renderTextureDescriptor;
			this.GetDescriptor_Injected(out renderTextureDescriptor);
			return renderTextureDescriptor;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x0001BDF6 File Offset: 0x00019FF6
		[FreeFunction("GetRenderBufferManager().GetTextures().GetTempBuffer")]
		private static RenderTexture GetTemporary_Internal(RenderTextureDescriptor desc)
		{
			return RenderTexture.GetTemporary_Internal_Injected(ref desc);
		}

		// Token: 0x060013D6 RID: 5078
		[FreeFunction("GetRenderBufferManager().GetTextures().ReleaseTempBuffer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReleaseTemporary(RenderTexture temp);

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060013D7 RID: 5079
		// (set) Token: 0x060013D8 RID: 5080
		public extern int depth
		{
			[FreeFunction("RenderTextureScripting::GetDepth", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("RenderTextureScripting::SetDepth", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0001BDFF File Offset: 0x00019FFF
		[RequiredByNativeCode]
		protected internal RenderTexture()
		{
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0001BE09 File Offset: 0x0001A009
		public RenderTexture(RenderTextureDescriptor desc)
		{
			RenderTexture.ValidateRenderTextureDesc(desc);
			RenderTexture.Internal_Create(this);
			this.SetRenderTextureDescriptor(desc);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0001BE2C File Offset: 0x0001A02C
		public RenderTexture(RenderTexture textureToCopy)
		{
			bool flag = textureToCopy == null;
			if (flag)
			{
				throw new ArgumentNullException("textureToCopy");
			}
			RenderTexture.ValidateRenderTextureDesc(textureToCopy.descriptor);
			RenderTexture.Internal_Create(this);
			this.SetRenderTextureDescriptor(textureToCopy.descriptor);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0001BE77 File Offset: 0x0001A077
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, int depth, DefaultFormat format)
			: this(width, height, RenderTexture.GetDefaultColorFormat(format), RenderTexture.GetDefaultDepthStencilFormat(format, depth), Texture.GenerateAllMips)
		{
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0001BE97 File Offset: 0x0001A097
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, int depth, GraphicsFormat format)
			: this(width, height, depth, format, Texture.GenerateAllMips)
		{
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0001BEAC File Offset: 0x0001A0AC
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, int depth, GraphicsFormat format, int mipCount)
		{
			bool flag = format != GraphicsFormat.None && !base.ValidateFormat(format, FormatUsage.Render);
			if (!flag)
			{
				RenderTexture.Internal_Create(this);
				this.depthStencilFormat = RenderTexture.GetDepthStencilFormatLegacy(depth, format);
				this.width = width;
				this.height = height;
				this.graphicsFormat = format;
				this.SetMipMapCount(mipCount);
				this.SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(format));
			}
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0001BF20 File Offset: 0x0001A120
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat, int mipCount)
		{
			bool flag = colorFormat != GraphicsFormat.None && !base.ValidateFormat(colorFormat, FormatUsage.Render);
			if (!flag)
			{
				RenderTexture.Internal_Create(this);
				this.width = width;
				this.height = height;
				this.depthStencilFormat = depthStencilFormat;
				this.graphicsFormat = colorFormat;
				this.SetMipMapCount(mipCount);
				this.SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(colorFormat));
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0001BF8A File Offset: 0x0001A18A
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat)
			: this(width, height, colorFormat, depthStencilFormat, Texture.GenerateAllMips)
		{
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0001BF9E File Offset: 0x0001A19E
		public RenderTexture(int width, int height, int depth, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite)
		{
			this.Initialize(width, height, depth, format, readWrite, Texture.GenerateAllMips);
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0001BFBB File Offset: 0x0001A1BB
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, int depth, RenderTextureFormat format)
			: this(width, height, depth, format, Texture.GenerateAllMips)
		{
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0001BFCF File Offset: 0x0001A1CF
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, int depth)
			: this(width, height, depth, RenderTextureFormat.Default)
		{
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0001BFDD File Offset: 0x0001A1DD
		[ExcludeFromDocs]
		public RenderTexture(int width, int height, int depth, RenderTextureFormat format, int mipCount)
		{
			this.Initialize(width, height, depth, format, RenderTextureReadWrite.Default, mipCount);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0001BFF8 File Offset: 0x0001A1F8
		private void Initialize(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite, int mipCount)
		{
			GraphicsFormat compatibleFormat = RenderTexture.GetCompatibleFormat(format, readWrite);
			GraphicsFormat depthStencilFormatLegacy = RenderTexture.GetDepthStencilFormatLegacy(depth, format);
			bool flag = compatibleFormat > GraphicsFormat.None;
			if (flag)
			{
				bool flag2 = !base.ValidateFormat(compatibleFormat, FormatUsage.Render);
				if (flag2)
				{
					return;
				}
			}
			RenderTexture.Internal_Create(this);
			this.width = width;
			this.height = height;
			this.depthStencilFormat = depthStencilFormatLegacy;
			this.graphicsFormat = compatibleFormat;
			this.SetMipMapCount(mipCount);
			this.SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(compatibleFormat));
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0001C074 File Offset: 0x0001A274
		internal static GraphicsFormat GetDepthStencilFormatLegacy(int depthBits, GraphicsFormat colorFormat)
		{
			return (colorFormat == GraphicsFormat.ShadowAuto) ? GraphicsFormatUtility.GetDepthStencilFormat(depthBits, 0) : GraphicsFormatUtility.GetDepthStencilFormat(depthBits);
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0001C0A0 File Offset: 0x0001A2A0
		internal static GraphicsFormat GetDepthStencilFormatLegacy(int depthBits, RenderTextureFormat format)
		{
			return RenderTexture.GetDepthStencilFormatLegacy(depthBits, format == RenderTextureFormat.Shadowmap);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0001C0BC File Offset: 0x0001A2BC
		internal static GraphicsFormat GetDepthStencilFormatLegacy(int depthBits, DefaultFormat format)
		{
			return RenderTexture.GetDepthStencilFormatLegacy(depthBits, format == DefaultFormat.Shadow);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0001C0D8 File Offset: 0x0001A2D8
		internal static GraphicsFormat GetDepthStencilFormatLegacy(int depthBits, bool requestedShadowMap)
		{
			return requestedShadowMap ? GraphicsFormatUtility.GetDepthStencilFormat(depthBits, 0) : GraphicsFormatUtility.GetDepthStencilFormat(depthBits);
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x0001C0FC File Offset: 0x0001A2FC
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x0001C114 File Offset: 0x0001A314
		public RenderTextureDescriptor descriptor
		{
			get
			{
				return this.GetDescriptor();
			}
			set
			{
				RenderTexture.ValidateRenderTextureDesc(value);
				this.SetRenderTextureDescriptor(value);
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0001C128 File Offset: 0x0001A328
		private static void ValidateRenderTextureDesc(RenderTextureDescriptor desc)
		{
			bool flag = desc.graphicsFormat == GraphicsFormat.None && desc.depthStencilFormat == GraphicsFormat.None;
			if (flag)
			{
				throw new ArgumentException("RenderTextureDesc graphicsFormat and depthStencilFormat cannot both be None.");
			}
			bool flag2 = desc.graphicsFormat != GraphicsFormat.None && !SystemInfo.IsFormatSupported(desc.graphicsFormat, FormatUsage.Render);
			if (flag2)
			{
				throw new ArgumentException("RenderTextureDesc graphicsFormat must be a supported GraphicsFormat. " + desc.graphicsFormat.ToString() + " is not supported on this platform.", "desc.graphicsFormat");
			}
			bool flag3 = desc.depthStencilFormat != GraphicsFormat.None && !GraphicsFormatUtility.IsDepthStencilFormat(desc.depthStencilFormat);
			if (flag3)
			{
				throw new ArgumentException("RenderTextureDesc depthStencilFormat must be a supported depth/stencil GraphicsFormat. " + desc.depthStencilFormat.ToString() + " is not supported on this platform.", "desc.depthStencilFormat");
			}
			bool flag4 = desc.width <= 0;
			if (flag4)
			{
				throw new ArgumentException("RenderTextureDesc width must be greater than zero.", "desc.width");
			}
			bool flag5 = desc.height <= 0;
			if (flag5)
			{
				throw new ArgumentException("RenderTextureDesc height must be greater than zero.", "desc.height");
			}
			bool flag6 = desc.volumeDepth <= 0;
			if (flag6)
			{
				throw new ArgumentException("RenderTextureDesc volumeDepth must be greater than zero.", "desc.volumeDepth");
			}
			bool flag7 = desc.msaaSamples != 1 && desc.msaaSamples != 2 && desc.msaaSamples != 4 && desc.msaaSamples != 8;
			if (flag7)
			{
				throw new ArgumentException("RenderTextureDesc msaaSamples must be 1, 2, 4, or 8.", "desc.msaaSamples");
			}
			bool flag8 = desc.dimension == TextureDimension.CubeArray && desc.volumeDepth % 6 != 0;
			if (flag8)
			{
				throw new ArgumentException("RenderTextureDesc volumeDepth must be a multiple of 6 when dimension is CubeArray", "desc.volumeDepth");
			}
			bool flag9 = desc.graphicsFormat != GraphicsFormat.ShadowAuto && desc.graphicsFormat != GraphicsFormat.DepthAuto && GraphicsFormatUtility.IsDepthStencilFormat(desc.graphicsFormat);
			if (flag9)
			{
				throw new ArgumentException("RenderTextureDesc graphicsFormat must not be a depth/stencil format. " + desc.graphicsFormat.ToString() + " is not supported.", "desc.graphicsFormat");
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0001C334 File Offset: 0x0001A534
		internal static GraphicsFormat GetDefaultColorFormat(DefaultFormat format)
		{
			GraphicsFormat graphicsFormat;
			if (format != DefaultFormat.DepthStencil)
			{
				if (format != DefaultFormat.Shadow)
				{
					graphicsFormat = SystemInfo.GetGraphicsFormat(format);
				}
				else
				{
					graphicsFormat = GraphicsFormat.ShadowAuto;
				}
			}
			else
			{
				graphicsFormat = GraphicsFormat.DepthAuto;
			}
			return graphicsFormat;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0001C36C File Offset: 0x0001A56C
		internal static GraphicsFormat GetDefaultDepthStencilFormat(DefaultFormat format, int depth)
		{
			GraphicsFormat graphicsFormat;
			if (format - DefaultFormat.DepthStencil > 1)
			{
				graphicsFormat = RenderTexture.GetDepthStencilFormatLegacy(depth, format);
			}
			else
			{
				graphicsFormat = SystemInfo.GetGraphicsFormat(format);
			}
			return graphicsFormat;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0001C39C File Offset: 0x0001A59C
		internal static GraphicsFormat GetCompatibleFormat(RenderTextureFormat renderTextureFormat, RenderTextureReadWrite readWrite)
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(renderTextureFormat, readWrite);
			GraphicsFormat compatibleFormat = SystemInfo.GetCompatibleFormat(graphicsFormat, FormatUsage.Render);
			bool flag = graphicsFormat == compatibleFormat;
			GraphicsFormat graphicsFormat2;
			if (flag)
			{
				graphicsFormat2 = graphicsFormat;
			}
			else
			{
				Debug.LogWarning(string.Format("'{0}' is not supported. RenderTexture::GetTemporary fallbacks to {1} format on this platform. Use 'SystemInfo.IsFormatSupported' C# API to check format support.", graphicsFormat.ToString(), compatibleFormat.ToString()));
				graphicsFormat2 = compatibleFormat;
			}
			return graphicsFormat2;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0001C3F8 File Offset: 0x0001A5F8
		public static RenderTexture GetTemporary(RenderTextureDescriptor desc)
		{
			RenderTexture.ValidateRenderTextureDesc(desc);
			desc.createdFromScript = true;
			return RenderTexture.GetTemporary_Internal(desc);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0001C420 File Offset: 0x0001A620
		private static RenderTexture GetTemporaryImpl(int width, int height, GraphicsFormat depthStencilFormat, GraphicsFormat colorFormat, int antiAliasing = 1, RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, bool useDynamicScale = false)
		{
			return RenderTexture.GetTemporary(new RenderTextureDescriptor(width, height, colorFormat, depthStencilFormat)
			{
				msaaSamples = antiAliasing,
				memoryless = memorylessMode,
				vrUsage = vrUsage,
				useDynamicScale = useDynamicScale
			});
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0001C46C File Offset: 0x0001A66C
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, [DefaultValue("1")] int antiAliasing, [DefaultValue("RenderTextureMemoryless.None")] RenderTextureMemoryless memorylessMode, [DefaultValue("VRTextureUsage.None")] VRTextureUsage vrUsage, [DefaultValue("false")] bool useDynamicScale)
		{
			return RenderTexture.GetTemporaryImpl(width, height, RenderTexture.GetDepthStencilFormatLegacy(depthBuffer, format), format, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0001C498 File Offset: 0x0001A698
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, int antiAliasing, RenderTextureMemoryless memorylessMode, VRTextureUsage vrUsage)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing, memorylessMode, vrUsage, false);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0001C4BC File Offset: 0x0001A6BC
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, int antiAliasing, RenderTextureMemoryless memorylessMode)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing, memorylessMode, VRTextureUsage.None);
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0001C4DC File Offset: 0x0001A6DC
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, int antiAliasing)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing, RenderTextureMemoryless.None);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0001C4FC File Offset: 0x0001A6FC
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, 1);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0001C518 File Offset: 0x0001A718
		public static RenderTexture GetTemporary(int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing, [DefaultValue("RenderTextureMemoryless.None")] RenderTextureMemoryless memorylessMode, [DefaultValue("VRTextureUsage.None")] VRTextureUsage vrUsage, [DefaultValue("false")] bool useDynamicScale)
		{
			GraphicsFormat compatibleFormat = RenderTexture.GetCompatibleFormat(format, readWrite);
			GraphicsFormat depthStencilFormatLegacy = RenderTexture.GetDepthStencilFormatLegacy(depthBuffer, format);
			return RenderTexture.GetTemporaryImpl(width, height, depthStencilFormatLegacy, compatibleFormat, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0001C54C File Offset: 0x0001A74C
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode, VRTextureUsage vrUsage)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, false);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0001C570 File Offset: 0x0001A770
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, VRTextureUsage.None);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0001C594 File Offset: 0x0001A794
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, RenderTextureMemoryless.None);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0001C5B4 File Offset: 0x0001A7B4
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, 1);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, RenderTextureReadWrite.Default);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
		{
			return RenderTexture.GetTemporary(width, height, depthBuffer, RenderTextureFormat.Default);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0001C60C File Offset: 0x0001A80C
		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height)
		{
			return RenderTexture.GetTemporary(width, height, 0);
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x0001C628 File Offset: 0x0001A828
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x0001C643 File Offset: 0x0001A843
		[Obsolete("Use RenderTexture.dimension instead.", false)]
		public bool isCubemap
		{
			get
			{
				return this.dimension == TextureDimension.Cube;
			}
			set
			{
				this.dimension = (value ? TextureDimension.Cube : TextureDimension.Tex2D);
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0001C654 File Offset: 0x0001A854
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x0001C66F File Offset: 0x0001A86F
		[Obsolete("Use RenderTexture.dimension instead.", false)]
		public bool isVolume
		{
			get
			{
				return this.dimension == TextureDimension.Tex3D;
			}
			set
			{
				this.dimension = (value ? TextureDimension.Tex3D : TextureDimension.Tex2D);
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0001C680 File Offset: 0x0001A880
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x00002669 File Offset: 0x00000869
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("RenderTexture.enabled is always now, no need to use it.", false)]
		public static bool enabled
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0001C694 File Offset: 0x0001A894
		[Obsolete("GetTexelOffset always returns zero now, no point in using it.", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Vector2 GetTexelOffset()
		{
			return Vector2.zero;
		}

		// Token: 0x06001406 RID: 5126
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetColorBuffer_Injected(out RenderBuffer ret);

		// Token: 0x06001407 RID: 5127
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetDepthBuffer_Injected(out RenderBuffer ret);

		// Token: 0x06001408 RID: 5128
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRenderTextureDescriptor_Injected(ref RenderTextureDescriptor desc);

		// Token: 0x06001409 RID: 5129
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetDescriptor_Injected(out RenderTextureDescriptor ret);

		// Token: 0x0600140A RID: 5130
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderTexture GetTemporary_Internal_Injected(ref RenderTextureDescriptor desc);
	}
}
