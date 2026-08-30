using System;
using System.Collections.Generic;

// Token: 0x02000788 RID: 1928
public interface INetConnection
{
	// Token: 0x0600395D RID: 14685
	void Disconnect(bool _kick);

	// Token: 0x0600395E RID: 14686
	bool IsDisconnected();

	// Token: 0x0600395F RID: 14687
	void GetPackages(List<NetPackage> _dstBuf);

	// Token: 0x06003960 RID: 14688
	void AddToSendQueue(NetPackage _package);

	// Token: 0x06003961 RID: 14689
	void AddToSendQueue(List<NetPackage> _packages);

	// Token: 0x06003962 RID: 14690
	void FlushSendQueue();

	// Token: 0x06003963 RID: 14691
	void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x06003964 RID: 14692
	void UpgradeToFullConnection();

	// Token: 0x06003965 RID: 14693
	NetConnectionStatistics GetStats();

	// Token: 0x06003966 RID: 14694
	void SetEncryptionModule(IEncryptionModule _module);
}
