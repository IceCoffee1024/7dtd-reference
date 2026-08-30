using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000B96 RID: 2966
public class RegionFileChunkWriter
{
	// Token: 0x060059BC RID: 22972 RVA: 0x00224B7F File Offset: 0x00222D7F
	public RegionFileChunkWriter(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
	}

	// Token: 0x060059BD RID: 22973 RVA: 0x00224BA0 File Offset: 0x00222DA0
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

	// Token: 0x04004588 RID: 17800
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x04004589 RID: 17801
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerSaveStream;

	// Token: 0x0400458A RID: 17802
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateOutputStream zipSaveStream;

	// Token: 0x0400458B RID: 17803
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] saveBuffer = new byte[4096];
}
