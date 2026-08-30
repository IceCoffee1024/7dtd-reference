using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001D2 RID: 466
	[NativeHeader("Runtime/Graphics/Texture3D.h")]
	[ExcludeFromPreset]
	public sealed class Texture3D : Texture
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600130E RID: 4878
		public extern int depth
		{
			[NativeName("GetTextureLayerCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600130F RID: 4879
		public extern TextureFormat format
		{
			[NativeName("GetTextureFormat")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001310 RID: 4880
		public override extern bool isReadable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0001A9EF File Offset: 0x00018BEF
		[NativeName("SetPixel")]
		private void SetPixelImpl(int mip, int x, int y, int z, Color color)
		{
			this.SetPixelImpl_Injected(mip, x, y, z, ref color);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0001AA00 File Offset: 0x00018C00
		[NativeName("GetPixel")]
		private Color GetPixelImpl(int mip, int x, int y, int z)
		{
			Color color;
			this.GetPixelImpl_Injected(mip, x, y, z, out color);
			return color;
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x0001AA1C File Offset: 0x00018C1C
		[NativeName("GetPixelBilinear")]
		private Color GetPixelBilinearImpl(int mip, float u, float v, float w)
		{
			Color color;
			this.GetPixelBilinearImpl_Injected(mip, u, v, w, out color);
			return color;
		}

		// Token: 0x06001314 RID: 4884
		[FreeFunction("Texture3DScripting::Create")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_CreateImpl([Writable] Texture3D mono, int w, int h, int d, int mipCount, GraphicsFormat format, TextureColorSpace colorSpace, TextureCreationFlags flags, IntPtr nativeTex);

		// Token: 0x06001315 RID: 4885 RVA: 0x0001AA38 File Offset: 0x00018C38
		private static void Internal_Create([Writable] Texture3D mono, int w, int h, int d, int mipCount, GraphicsFormat format, TextureColorSpace colorSpace, TextureCreationFlags flags, IntPtr nativeTex)
		{
			bool flag = !Texture3D.Internal_CreateImpl(mono, w, h, d, mipCount, format, colorSpace, flags, nativeTex);
			if (flag)
			{
				throw new UnityException("Failed to create texture because of invalid parameters.");
			}
		}

		// Token: 0x06001316 RID: 4886
		[FreeFunction("Texture3DScripting::UpdateExternalTexture", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateExternalTexture(IntPtr nativeTex);

		// Token: 0x06001317 RID: 4887
		[FreeFunction(Name = "Texture3DScripting::Apply", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

		// Token: 0x06001318 RID: 4888
		[FreeFunction(Name = "Texture3DScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(int miplevel);

		// Token: 0x06001319 RID: 4889 RVA: 0x0001AA6C File Offset: 0x00018C6C
		public Color[] GetPixels()
		{
			return this.GetPixels(0);
		}

		// Token: 0x0600131A RID: 4890
		[FreeFunction(Name = "Texture3DScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32(int miplevel);

		// Token: 0x0600131B RID: 4891 RVA: 0x0001AA88 File Offset: 0x00018C88
		public Color32[] GetPixels32()
		{
			return this.GetPixels32(0);
		}

		// Token: 0x0600131C RID: 4892
		[FreeFunction(Name = "Texture3DScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels([Unmarshalled] Color[] colors, int miplevel);

		// Token: 0x0600131D RID: 4893 RVA: 0x0001AAA1 File Offset: 0x00018CA1
		public void SetPixels(Color[] colors)
		{
			this.SetPixels(colors, 0);
		}

		// Token: 0x0600131E RID: 4894
		[FreeFunction(Name = "Texture3DScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels32([Unmarshalled] Color32[] colors, int miplevel);

		// Token: 0x0600131F RID: 4895 RVA: 0x0001AAAD File Offset: 0x00018CAD
		public void SetPixels32(Color32[] colors)
		{
			this.SetPixels32(colors, 0);
		}

		// Token: 0x06001320 RID: 4896
		[FreeFunction(Name = "Texture3DScripting::SetPixelDataArray", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool SetPixelDataImplArray(Array data, int mipLevel, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

		// Token: 0x06001321 RID: 4897
		[FreeFunction(Name = "Texture3DScripting::SetPixelData", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool SetPixelDataImpl(IntPtr data, int mipLevel, int elementSize, int dataArraySize, int sourceDataStartIndex = 0);

		// Token: 0x06001322 RID: 4898
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr GetImageDataPointer();

		// Token: 0x06001323 RID: 4899 RVA: 0x0001AAB9 File Offset: 0x00018CB9
		[ExcludeFromDocs]
		public Texture3D(int width, int height, int depth, DefaultFormat format, TextureCreationFlags flags)
			: this(width, height, depth, SystemInfo.GetGraphicsFormat(format), flags)
		{
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0001AACF File Offset: 0x00018CCF
		[ExcludeFromDocs]
		public Texture3D(int width, int height, int depth, DefaultFormat format, TextureCreationFlags flags, int mipCount)
			: this(width, height, depth, SystemInfo.GetGraphicsFormat(format), flags, mipCount)
		{
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0001AAE7 File Offset: 0x00018CE7
		[RequiredByNativeCode]
		[ExcludeFromDocs]
		public Texture3D(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags)
			: this(width, height, depth, format, flags, Texture.GenerateAllMips)
		{
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0001AB00 File Offset: 0x00018D00
		[ExcludeFromDocs]
		public Texture3D(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags, [DefaultValue("Texture.GenerateAllMips")] int mipCount)
		{
			bool flag = !base.ValidateFormat(format, FormatUsage.Sample);
			if (!flag)
			{
				Texture3D.ValidateIsNotCrunched(flags);
				Texture3D.Internal_Create(this, width, height, depth, mipCount, format, base.GetTextureColorSpace(format), flags, IntPtr.Zero);
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0001AB4C File Offset: 0x00018D4C
		[ExcludeFromDocs]
		public Texture3D(int width, int height, int depth, TextureFormat textureFormat, int mipCount)
			: this(width, height, depth, textureFormat, mipCount, IntPtr.Zero)
		{
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0001AB62 File Offset: 0x00018D62
		public Texture3D(int width, int height, int depth, TextureFormat textureFormat, int mipCount, [DefaultValue("IntPtr.Zero")] IntPtr nativeTex)
			: this(width, height, depth, textureFormat, mipCount, nativeTex, false)
		{
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0001AB78 File Offset: 0x00018D78
		public Texture3D(int width, int height, int depth, TextureFormat textureFormat, int mipCount, [DefaultValue("IntPtr.Zero")] IntPtr nativeTex, [DefaultValue("false")] bool createUninitialized)
		{
			bool flag = !base.ValidateFormat(textureFormat);
			if (!flag)
			{
				GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, false);
				TextureCreationFlags textureCreationFlags = ((mipCount != 1) ? TextureCreationFlags.MipChain : TextureCreationFlags.None);
				bool flag2 = GraphicsFormatUtility.IsCrunchFormat(textureFormat);
				if (flag2)
				{
					textureCreationFlags |= TextureCreationFlags.Crunch;
				}
				if (createUninitialized)
				{
					textureCreationFlags |= TextureCreationFlags.DontInitializePixels | TextureCreationFlags.DontUploadUponCreate;
				}
				Texture3D.ValidateIsNotCrunched(textureCreationFlags);
				Texture3D.Internal_Create(this, width, height, depth, mipCount, graphicsFormat, base.GetTextureColorSpace(true), textureCreationFlags, nativeTex);
			}
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0001ABEF File Offset: 0x00018DEF
		[ExcludeFromDocs]
		public Texture3D(int width, int height, int depth, TextureFormat textureFormat, bool mipChain)
			: this(width, height, depth, textureFormat, mipChain ? Texture.GenerateAllMips : 1)
		{
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0001AC0A File Offset: 0x00018E0A
		public Texture3D(int width, int height, int depth, TextureFormat textureFormat, bool mipChain, [DefaultValue("false")] bool createUninitialized)
			: this(width, height, depth, textureFormat, mipChain ? Texture.GenerateAllMips : 1, IntPtr.Zero, createUninitialized)
		{
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0001AC2C File Offset: 0x00018E2C
		public Texture3D(int width, int height, int depth, TextureFormat textureFormat, bool mipChain, [DefaultValue("IntPtr.Zero")] IntPtr nativeTex)
			: this(width, height, depth, textureFormat, mipChain ? Texture.GenerateAllMips : 1, nativeTex)
		{
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0001AC4C File Offset: 0x00018E4C
		public static Texture3D CreateExternalTexture(int width, int height, int depth, TextureFormat format, bool mipChain, IntPtr nativeTex)
		{
			bool flag = nativeTex == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("nativeTex may not be zero");
			}
			return new Texture3D(width, height, depth, format, mipChain ? (-1) : 1, nativeTex, false);
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0001AC90 File Offset: 0x00018E90
		public void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			this.ApplyImpl(updateMipmaps, makeNoLongerReadable);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0001ACBC File Offset: 0x00018EBC
		[ExcludeFromDocs]
		public void Apply(bool updateMipmaps)
		{
			this.Apply(updateMipmaps, false);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0001ACC8 File Offset: 0x00018EC8
		[ExcludeFromDocs]
		public void Apply()
		{
			this.Apply(true, false);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0001ACD4 File Offset: 0x00018ED4
		[ExcludeFromDocs]
		public void SetPixel(int x, int y, int z, Color color)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			this.SetPixelImpl(0, x, y, z, color);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0001AD04 File Offset: 0x00018F04
		public void SetPixel(int x, int y, int z, Color color, [DefaultValue("0")] int mipLevel)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			this.SetPixelImpl(mipLevel, x, y, z, color);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0001AD38 File Offset: 0x00018F38
		[ExcludeFromDocs]
		public Color GetPixel(int x, int y, int z)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelImpl(0, x, y, z);
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0001AD6C File Offset: 0x00018F6C
		public Color GetPixel(int x, int y, int z, [DefaultValue("0")] int mipLevel)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelImpl(mipLevel, x, y, z);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0001ADA0 File Offset: 0x00018FA0
		[ExcludeFromDocs]
		public Color GetPixelBilinear(float u, float v, float w)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelBilinearImpl(0, u, v, w);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x0001ADD4 File Offset: 0x00018FD4
		public Color GetPixelBilinear(float u, float v, float w, [DefaultValue("0")] int mipLevel)
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelBilinearImpl(mipLevel, u, v, w);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0001AE08 File Offset: 0x00019008
		public void SetPixelData<T>(T[] data, int mipLevel, [DefaultValue("0")] int sourceDataStartIndex = 0)
		{
			bool flag = sourceDataStartIndex < 0;
			if (flag)
			{
				throw new UnityException("SetPixelData: sourceDataStartIndex cannot be less than 0.");
			}
			bool flag2 = !this.isReadable;
			if (flag2)
			{
				throw base.CreateNonReadableException(this);
			}
			bool flag3 = data == null || data.Length == 0;
			if (flag3)
			{
				throw new UnityException("No texture data provided to SetPixelData.");
			}
			this.SetPixelDataImplArray(data, mipLevel, Marshal.SizeOf<T>(data[0]), data.Length, sourceDataStartIndex);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0001AE74 File Offset: 0x00019074
		public void SetPixelData<T>(NativeArray<T> data, int mipLevel, [DefaultValue("0")] int sourceDataStartIndex = 0) where T : struct
		{
			bool flag = sourceDataStartIndex < 0;
			if (flag)
			{
				throw new UnityException("SetPixelData: sourceDataStartIndex cannot be less than 0.");
			}
			bool flag2 = !this.isReadable;
			if (flag2)
			{
				throw base.CreateNonReadableException(this);
			}
			bool flag3 = !data.IsCreated || data.Length == 0;
			if (flag3)
			{
				throw new UnityException("No texture data provided to SetPixelData.");
			}
			this.SetPixelDataImpl((IntPtr)data.GetUnsafeReadOnlyPtr<T>(), mipLevel, UnsafeUtility.SizeOf<T>(), data.Length, sourceDataStartIndex);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0001AEF0 File Offset: 0x000190F0
		public unsafe NativeArray<T> GetPixelData<T>(int mipLevel) where T : struct
		{
			bool flag = !this.isReadable;
			if (flag)
			{
				throw base.CreateNonReadableException(this);
			}
			bool flag2 = mipLevel < 0 || mipLevel >= base.mipmapCount;
			if (flag2)
			{
				throw new ArgumentException("The passed in miplevel " + mipLevel.ToString() + " is invalid. The valid range is 0 through  " + (base.mipmapCount - 1).ToString());
			}
			bool flag3 = this.GetImageDataPointer().ToInt64() == 0L;
			if (flag3)
			{
				throw new UnityException("Texture '" + base.name + "' has no data.");
			}
			ulong pixelDataOffset = base.GetPixelDataOffset(mipLevel, 0);
			ulong pixelDataSize = base.GetPixelDataSize(mipLevel, 0);
			int num = UnsafeUtility.SizeOf<T>();
			ulong num2 = pixelDataSize / (ulong)((long)num);
			bool flag4 = num2 > 2147483647UL;
			if (flag4)
			{
				throw base.CreateNativeArrayLengthOverflowException();
			}
			IntPtr intPtr = new IntPtr((long)this.GetImageDataPointer() + (long)pixelDataOffset);
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)intPtr, (int)num2, Allocator.None);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0001AFF0 File Offset: 0x000191F0
		private static void ValidateIsNotCrunched(TextureCreationFlags flags)
		{
			bool flag = (flags &= TextureCreationFlags.Crunch) > TextureCreationFlags.None;
			if (flag)
			{
				throw new ArgumentException("Crunched Texture3D is not supported.");
			}
		}

		// Token: 0x0600133B RID: 4923
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPixelImpl_Injected(int mip, int x, int y, int z, ref Color color);

		// Token: 0x0600133C RID: 4924
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPixelImpl_Injected(int mip, int x, int y, int z, out Color ret);

		// Token: 0x0600133D RID: 4925
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPixelBilinearImpl_Injected(int mip, float u, float v, float w, out Color ret);
	}
}
