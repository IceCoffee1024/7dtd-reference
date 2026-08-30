using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x020001D5 RID: 469
	[NativeHeader("Runtime/Graphics/SparseTexture.h")]
	public sealed class SparseTexture : Texture
	{
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001383 RID: 4995
		public extern int tileWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001384 RID: 4996
		public extern int tileHeight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001385 RID: 4997
		public extern bool isCreated
		{
			[NativeName("IsInitialized")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001386 RID: 4998
		[FreeFunction(Name = "SparseTextureScripting::Create", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] SparseTexture mono, int width, int height, GraphicsFormat format, TextureColorSpace colorSpace, int mipCount);

		// Token: 0x06001387 RID: 4999
		[FreeFunction(Name = "SparseTextureScripting::UpdateTile", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateTile(int tileX, int tileY, int miplevel, [Unmarshalled] Color32[] data);

		// Token: 0x06001388 RID: 5000
		[FreeFunction(Name = "SparseTextureScripting::UpdateTileRaw", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateTileRaw(int tileX, int tileY, int miplevel, [Unmarshalled] byte[] data);

		// Token: 0x06001389 RID: 5001 RVA: 0x0001BA4B File Offset: 0x00019C4B
		public void UnloadTile(int tileX, int tileY, int miplevel)
		{
			this.UpdateTileRaw(tileX, tileY, miplevel, null);
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0001BA5C File Offset: 0x00019C5C
		internal bool ValidateFormat(TextureFormat format, int width, int height)
		{
			bool flag = base.ValidateFormat(format);
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = TextureFormat.PVRTC_RGB2 <= format && format <= TextureFormat.PVRTC_RGBA4;
				bool flag4 = flag3 && (width != height || !Mathf.IsPowerOfTwo(width));
				if (flag4)
				{
					throw new UnityException(string.Format("'{0}' demands texture to be square and have power-of-two dimensions", format.ToString()));
				}
			}
			return flag;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0001BAC8 File Offset: 0x00019CC8
		internal bool ValidateFormat(GraphicsFormat format, int width, int height)
		{
			bool flag = base.ValidateFormat(format, FormatUsage.Sparse);
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = GraphicsFormatUtility.IsPVRTCFormat(format);
				bool flag4 = flag3 && (width != height || !Mathf.IsPowerOfTwo(width));
				if (flag4)
				{
					throw new UnityException(string.Format("'{0}' demands texture to be square and have power-of-two dimensions", format.ToString()));
				}
			}
			return flag;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0001BB2C File Offset: 0x00019D2C
		internal bool ValidateSize(int width, int height, GraphicsFormat format)
		{
			bool flag = (ulong)GraphicsFormatUtility.GetBlockSize(format) * (ulong)((long)width / (long)((ulong)GraphicsFormatUtility.GetBlockWidth(format))) * (ulong)((long)height / (long)((ulong)GraphicsFormatUtility.GetBlockHeight(format))) < 65536UL;
			bool flag2;
			if (flag)
			{
				Debug.LogError("SparseTexture creation failed. The minimum size in bytes of a SparseTexture is 64KB.", this);
				flag2 = false;
			}
			else
			{
				flag2 = true;
			}
			return flag2;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0001BB7C File Offset: 0x00019D7C
		private static void ValidateIsNotCrunched(TextureFormat textureFormat)
		{
			bool flag = GraphicsFormatUtility.IsCrunchFormat(textureFormat);
			if (flag)
			{
				throw new ArgumentException("Crunched SparseTexture is not supported.");
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0001BB9F File Offset: 0x00019D9F
		[ExcludeFromDocs]
		public SparseTexture(int width, int height, DefaultFormat format, int mipCount)
			: this(width, height, SystemInfo.GetGraphicsFormat(format), mipCount)
		{
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0001BBB4 File Offset: 0x00019DB4
		[ExcludeFromDocs]
		public SparseTexture(int width, int height, GraphicsFormat format, int mipCount)
		{
			bool flag = !this.ValidateFormat(format, width, height);
			if (!flag)
			{
				bool flag2 = !this.ValidateSize(width, height, format);
				if (!flag2)
				{
					SparseTexture.Internal_Create(this, width, height, format, base.GetTextureColorSpace(format), mipCount);
				}
			}
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0001BC00 File Offset: 0x00019E00
		[ExcludeFromDocs]
		public SparseTexture(int width, int height, TextureFormat textureFormat, int mipCount)
			: this(width, height, textureFormat, mipCount, false)
		{
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0001BC10 File Offset: 0x00019E10
		public SparseTexture(int width, int height, TextureFormat textureFormat, int mipCount, [DefaultValue("false")] bool linear)
		{
			bool flag = !this.ValidateFormat(textureFormat, width, height);
			if (!flag)
			{
				SparseTexture.ValidateIsNotCrunched(textureFormat);
				GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
				bool flag2 = !SystemInfo.IsFormatSupported(graphicsFormat, FormatUsage.Sparse);
				if (flag2)
				{
					Debug.LogError(string.Format("Creation of a SparseTexture with '{0}' is not supported on this platform.", textureFormat));
				}
				else
				{
					bool flag3 = !this.ValidateSize(width, height, graphicsFormat);
					if (!flag3)
					{
						SparseTexture.Internal_Create(this, width, height, graphicsFormat, base.GetTextureColorSpace(linear), mipCount);
					}
				}
			}
		}
	}
}
