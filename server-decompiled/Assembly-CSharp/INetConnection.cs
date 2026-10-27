using System;
using System.Collections.Generic;

// Token: 0x0200079D RID: 1949
public interface INetConnection
{
	// Token: 0x06003A0F RID: 14863
	void Disconnect(bool _kick);

	// Token: 0x06003A10 RID: 14864
	bool IsDisconnected();

	// Token: 0x06003A11 RID: 14865
	void GetPackages(List<NetPackage> _dstBuf);

	// Token: 0x06003A12 RID: 14866
	void AddToSendQueue(NetPackage _package);

	// Token: 0x06003A13 RID: 14867
	void AddToSendQueue(List<NetPackage> _packages);

	// Token: 0x06003A14 RID: 14868
	void FlushSendQueue();

	// Token: 0x06003A15 RID: 14869
	void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x06003A16 RID: 14870
	void UpgradeToFullConnection();

	// Token: 0x06003A17 RID: 14871
	NetConnectionStatistics GetStats();

	// Token: 0x06003A18 RID: 14872
	void SetEncryptionModule(IEncryptionModule _module);
}
