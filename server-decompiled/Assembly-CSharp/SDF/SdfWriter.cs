using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x02001693 RID: 5779
	public static class SdfWriter
	{
		// Token: 0x0600B566 RID: 46438 RVA: 0x0043872C File Offset: 0x0043692C
		public static void Write(Stream fs, Dictionary<string, SdfTag> sdfTags)
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(fs);
				pooledBinaryWriter.Seek(0, SeekOrigin.Begin);
				foreach (KeyValuePair<string, SdfTag> keyValuePair in sdfTags)
				{
					SdfTag value = keyValuePair.Value;
					pooledBinaryWriter.Write((byte)value.TagType);
					pooledBinaryWriter.Write((short)value.Name.Length);
					pooledBinaryWriter.Write(value.Name);
					value.WritePayload(pooledBinaryWriter);
				}
			}
		}
	}
}
