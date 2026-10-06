using System;

namespace Platform
{
	// Token: 0x02001BC8 RID: 7112
	public interface IRemotePlayerStorageObject
	{
		// Token: 0x0600D47E RID: 54398
		void ReadInto(PooledBinaryReader _reader);

		// Token: 0x0600D47F RID: 54399
		void WriteFrom(PooledBinaryWriter _writer);
	}
}
