using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x020016DE RID: 5854
	public static class SdfWriter
	{
		// Token: 0x0600B791 RID: 46993 RVA: 0x0043F6F0 File Offset: 0x0043D8F0
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
