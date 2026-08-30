using System;

namespace SharpEXR
{
	// Token: 0x020016A0 RID: 5792
	public class EXRFormatException : Exception
	{
		// Token: 0x0600B4AF RID: 46255 RVA: 0x003BBEDB File Offset: 0x003BA0DB
		public EXRFormatException()
		{
		}

		// Token: 0x0600B4B0 RID: 46256 RVA: 0x003BBEE3 File Offset: 0x003BA0E3
		public EXRFormatException(string message)
			: base(message)
		{
		}

		// Token: 0x0600B4B1 RID: 46257 RVA: 0x003BBEEC File Offset: 0x003BA0EC
		public EXRFormatException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
