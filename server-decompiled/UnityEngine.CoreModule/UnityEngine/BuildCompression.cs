using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000129 RID: 297
	[UsedByNativeCode]
	[Serializable]
	public struct BuildCompression
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0000B024 File Offset: 0x00009224
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0000B03C File Offset: 0x0000923C
		public CompressionType compression
		{
			get
			{
				return this._compression;
			}
			private set
			{
				this._compression = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0000B048 File Offset: 0x00009248
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0000B060 File Offset: 0x00009260
		public CompressionLevel level
		{
			get
			{
				return this._level;
			}
			private set
			{
				this._level = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0000B06C File Offset: 0x0000926C
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0000B084 File Offset: 0x00009284
		public uint blockSize
		{
			get
			{
				return this._blockSize;
			}
			private set
			{
				this._blockSize = value;
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000B08E File Offset: 0x0000928E
		private BuildCompression(CompressionType in_compression, CompressionLevel in_level, uint in_blockSize)
		{
			this = default(BuildCompression);
			this.compression = in_compression;
			this.level = in_level;
			this.blockSize = in_blockSize;
		}

		// Token: 0x040003D8 RID: 984
		public static readonly BuildCompression Uncompressed = new BuildCompression(CompressionType.None, CompressionLevel.Maximum, 131072U);

		// Token: 0x040003D9 RID: 985
		public static readonly BuildCompression LZ4 = new BuildCompression(CompressionType.Lz4HC, CompressionLevel.Maximum, 131072U);

		// Token: 0x040003DA RID: 986
		public static readonly BuildCompression LZMA = new BuildCompression(CompressionType.Lzma, CompressionLevel.Maximum, 131072U);

		// Token: 0x040003DB RID: 987
		public static readonly BuildCompression UncompressedRuntime = BuildCompression.Uncompressed;

		// Token: 0x040003DC RID: 988
		public static readonly BuildCompression LZ4Runtime = new BuildCompression(CompressionType.Lz4, CompressionLevel.Maximum, 131072U);

		// Token: 0x040003DD RID: 989
		[NativeName("compression")]
		private CompressionType _compression;

		// Token: 0x040003DE RID: 990
		[NativeName("level")]
		private CompressionLevel _level;

		// Token: 0x040003DF RID: 991
		[NativeName("blockSize")]
		private uint _blockSize;
	}
}
