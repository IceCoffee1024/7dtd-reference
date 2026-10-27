using System;
using System.IO;
using Noemax.GZip;

// Token: 0x02000BB3 RID: 2995
public class RegionFileChunkWriter
{
	// Token: 0x060059FB RID: 23035 RVA: 0x0022936B File Offset: 0x0022756B
	public RegionFileChunkWriter(RegionFileAccessAbstract regionFileAccess)
	{
		this.regionFileAccess = regionFileAccess;
	}

	// Token: 0x060059FC RID: 23036 RVA: 0x0022938C File Offset: 0x0022758C
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

	// Token: 0x040045EB RID: 17899
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileAccessAbstract regionFileAccess;

	// Token: 0x040045EC RID: 17900
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream innerSaveStream;

	// Token: 0x040045ED RID: 17901
	[PublicizedFrom(EAccessModifier.Private)]
	public DeflateOutputStream zipSaveStream;

	// Token: 0x040045EE RID: 17902
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] saveBuffer = new byte[4096];
}
