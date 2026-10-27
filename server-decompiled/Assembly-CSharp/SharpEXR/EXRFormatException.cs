using System;

namespace SharpEXR
{
	// Token: 0x02001711 RID: 5905
	public class EXRFormatException : Exception
	{
		// Token: 0x0600B85D RID: 47197 RVA: 0x003C81BF File Offset: 0x003C63BF
		public EXRFormatException()
		{
		}

		// Token: 0x0600B85E RID: 47198 RVA: 0x003C81C7 File Offset: 0x003C63C7
		public EXRFormatException(string message)
			: base(message)
		{
		}

		// Token: 0x0600B85F RID: 47199 RVA: 0x003C81D0 File Offset: 0x003C63D0
		public EXRFormatException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
