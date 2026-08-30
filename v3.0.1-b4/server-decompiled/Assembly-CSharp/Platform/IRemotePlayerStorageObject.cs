using System;
using System.IO;

namespace Platform
{
	// Token: 0x02001B6F RID: 7023
	public interface IRemotePlayerStorageObject
	{
		// Token: 0x0600D17A RID: 53626
		void ReadInto(BinaryReader _reader);

		// Token: 0x0600D17B RID: 53627
		void WriteFrom(BinaryWriter _writer);
	}
}
