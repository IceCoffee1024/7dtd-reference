using System;
using System.Collections.Generic;

// Token: 0x02000792 RID: 1938
public interface INetConnection
{
	// Token: 0x060039B5 RID: 14773
	void Disconnect(bool _kick);

	// Token: 0x060039B6 RID: 14774
	bool IsDisconnected();

	// Token: 0x060039B7 RID: 14775
	void GetPackages(List<NetPackage> _dstBuf);

	// Token: 0x060039B8 RID: 14776
	void AddToSendQueue(NetPackage _package);

	// Token: 0x060039B9 RID: 14777
	void AddToSendQueue(List<NetPackage> _packages);

	// Token: 0x060039BA RID: 14778
	void FlushSendQueue();

	// Token: 0x060039BB RID: 14779
	void AppendToReaderStream(byte[] _data, int _size);

	// Token: 0x060039BC RID: 14780
	void UpgradeToFullConnection();

	// Token: 0x060039BD RID: 14781
	NetConnectionStatistics GetStats();

	// Token: 0x060039BE RID: 14782
	void SetEncryptionModule(IEncryptionModule _module);
}
