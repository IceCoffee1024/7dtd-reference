using System;
using System.IO;

// Token: 0x02000791 RID: 1937
public interface IEncryptionModule
{
	// Token: 0x060039B3 RID: 14771
	bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream);

	// Token: 0x060039B4 RID: 14772
	bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream);
}
