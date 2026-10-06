using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000BA8 RID: 2984
public class RegionFileChunkWriter
{
	// Token: 0x060059D6 RID: 22998 RVA: 0x00227E0F File Offset: 0x0022600F
	public RegionFileChunkWriter(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
	}

	// Token: 0x060059D7 RID: 22999 RVA: 0x00227E30 File Offset: 0x00226030
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

	// Token: 0x040045C6 RID: 17862
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040045C7 RID: 17863
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerSaveStream;

	// Token: 0x040045C8 RID: 17864
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateOutputStream zipSaveStream;

	// Token: 0x040045C9 RID: 17865
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] saveBuffer = new byte[4096];
}
