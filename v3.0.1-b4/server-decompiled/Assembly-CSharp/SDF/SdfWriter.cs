using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x0200166D RID: 5741
	public static class SdfWriter
	{
		// Token: 0x0600B3E3 RID: 46051 RVA: 0x004329B4 File Offset: 0x00430BB4
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
