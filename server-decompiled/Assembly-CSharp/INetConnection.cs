using System;
using System.Collections.Generic;

// Token: 0x02000795 RID: 1941
public interface INetConnection
{
	// Token: 0x060039F8 RID: 14840
	void Disconnect(bool _kick);

	// Token: 0x060039F9 RID: 14841
	bool IsDisconnected();

	// Token: 0x060039FA RID: 14842
	void GetPackages(List<NetPackage> _dstBuf);

	// Token: 0x060039FB RID: 14843
	void AddToSendQueue(NetPackage _package);

	// Token: 0x060039FC RID: 14844
	void AddToSendQueue(List<NetPackage> _packages);

	// Token: 0x060039FD RID: 14845
	void FlushSendQueue();

	// Token: 0x060039FE RID: 14846
	void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x060039FF RID: 14847
	void UpgradeToFullConnection();

	// Token: 0x06003A00 RID: 14848
	NetConnectionStatistics GetStats();

	// Token: 0x06003A01 RID: 14849
	void SetEncryptionModule(IEncryptionModule _module);
}
