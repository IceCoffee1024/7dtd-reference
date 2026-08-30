using System;
using System.Collections.Generic;
using System.IO;

namespace SDF
{
	// Token: 0x0200166C RID: 5740
	public static class SdfReader
	{
		// Token: 0x0600B3E2 RID: 46050 RVA: 0x00432844 File Offset: 0x00430A44
		public static Dictionary<string, SdfTag> Read(Stream fs)
		{
			Dictionary<string, SdfTag> dictionary = new Dictionary<string, SdfTag>();
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(fs);
				try
				{
					while (pooledBinaryReader.BaseStream.Position < pooledBinaryReader.BaseStream.Length)
					{
						SdfTagType sdfTagType = (SdfTagType)pooledBinaryReader.ReadByte();
						pooledBinaryReader.ReadInt16();
						string text = pooledBinaryReader.ReadString();
						if (sdfTagType == SdfTagType.Float)
						{
							SdfFloat sdfFloat = new SdfFloat(text, pooledBinaryReader.ReadSingle());
							dictionary.Add(text, sdfFloat);
						}
						else if (sdfTagType == SdfTagType.Int)
						{
							SdfInt sdfInt = new SdfInt(text, pooledBinaryReader.ReadInt32());
							dictionary.Add(text, sdfInt);
						}
						else if (sdfTagType == SdfTagType.String)
						{
							pooledBinaryReader.ReadInt16();
							SdfString sdfString = new SdfString(text, Utils.FromBase64(pooledBinaryReader.ReadString()));
							dictionary.Add(text, sdfString);
						}
						else if (sdfTagType == SdfTagType.Binary)
						{
							pooledBinaryReader.ReadInt16();
							SdfString sdfString2 = new SdfString(text, pooledBinaryReader.ReadString());
							dictionary.Add(text, sdfString2);
						}
						else if (sdfTagType == SdfTagType.Bool)
						{
							SdfBool sdfBool = new SdfBool(text, pooledBinaryReader.ReadBoolean());
							dictionary.Add(text, sdfBool);
						}
						else if (sdfTagType == SdfTagType.ByteArray)
						{
							short num = pooledBinaryReader.ReadInt16();
							SdfByteArray sdfByteArray = new SdfByteArray(text, pooledBinaryReader.ReadBytes((int)num));
							dictionary.Add(text, sdfByteArray);
						}
					}
				}
				catch
				{
				}
			}
			return dictionary;
		}
	}
}
