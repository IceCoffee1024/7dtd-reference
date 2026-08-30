using System;
using System.IO;

namespace Platform
{
	// Token: 0x02001B96 RID: 7062
	public interface IRemotePlayerStorageObject
	{
		// Token: 0x0600D306 RID: 54022
		void ReadInto(BinaryReader _reader);

		// Token: 0x0600D307 RID: 54023
		void WriteFrom(BinaryWriter _writer);
	}
}
