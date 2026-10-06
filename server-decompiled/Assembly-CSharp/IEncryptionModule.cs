using System;
using System.IO;

// Token: 0x02000793 RID: 1939
public interface IEncryptionModule
{
	// Token: 0x060039F5 RID: 14837
	bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream);

	// Token: 0x060039F6 RID: 14838
	bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream);
}
