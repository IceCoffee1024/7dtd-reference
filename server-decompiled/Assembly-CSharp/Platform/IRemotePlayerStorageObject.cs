using System;

namespace Platform
{
	// Token: 0x02001BF9 RID: 7161
	public interface IRemotePlayerStorageObject
	{
		// Token: 0x0600D592 RID: 54674
		void ReadInto(PooledBinaryReader _reader);

		// Token: 0x0600D593 RID: 54675
		void WriteFrom(PooledBinaryWriter _writer);
	}
}
