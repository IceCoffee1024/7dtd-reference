using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000B84 RID: 2948
public class RegionFileChunkWriter
{
	// Token: 0x06005934 RID: 22836 RVA: 0x00221E63 File Offset: 0x00220063
	public RegionFileChunkWriter(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x00221E84 File Offset: 0x00220084
	public void WriteStreamCompressed(string dir, int chunkX, int chunkZ, string ext, MemoryStream memoryStream)
	{
		Stream outputStream = this.regionFileAccess.GetOutputStream(dir, chunkX, chunkZ, ext);
		long num = StreamUtils.ReadInt64(memoryStream);
		StreamUtils.Write(outputStream, num);
		if (this.zipSaveStream == null || this.innerSaveStream != outputStream)
		{
			if (this.zipSaveStream != null)
			{
				Log.Warning("RFM.Save: Creating new DeflateStream, underlying Stream changed");
			}
			this.zipSaveStream = new DeflateOutputStream(outputStream, 3, true);
			this.innerSaveStream = outputStream;
		}
		StreamUtils.StreamCopy(memoryStream, this.zipSaveStream, this.saveBuffer, true);
		this.zipSaveStream.Restart();
		outputStream.Close();
	}

	// Token: 0x040044ED RID: 17645
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040044EE RID: 17646
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerSaveStream;

	// Token: 0x040044EF RID: 17647
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateOutputStream zipSaveStream;

	// Token: 0x040044F0 RID: 17648
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] saveBuffer = new byte[4096];
}
