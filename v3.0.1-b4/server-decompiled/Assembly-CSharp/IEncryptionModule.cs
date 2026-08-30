using System;
using System.IO;

// Token: 0x02000787 RID: 1927
public interface IEncryptionModule
{
	// Token: 0x0600395B RID: 14683
	bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream);

	// Token: 0x0600395C RID: 14684
	bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream);
}
