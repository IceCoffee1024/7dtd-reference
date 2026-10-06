using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x020016BF RID: 5823
	public static class SdfWriter
	{
		// Token: 0x0600B6BD RID: 46781 RVA: 0x0043CC4C File Offset: 0x0043AE4C
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
