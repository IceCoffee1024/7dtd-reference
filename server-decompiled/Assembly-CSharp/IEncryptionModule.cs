using System;
using System.IO;

// Token: 0x0200079B RID: 1947
public interface IEncryptionModule
{
	// Token: 0x06003A0C RID: 14860
	bool EncryptStream(ClientInfo _cInfo, MemoryStream _stream);

	// Token: 0x06003A0D RID: 14861
	bool DecryptStream(ClientInfo _cInfo, MemoryStream _stream);
}
