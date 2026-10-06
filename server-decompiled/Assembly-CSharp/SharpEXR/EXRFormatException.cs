using System;

namespace SharpEXR
{
	// Token: 0x020016F2 RID: 5874
	public class EXRFormatException : Exception
	{
		// Token: 0x0600B789 RID: 46985 RVA: 0x003C5DBB File Offset: 0x003C3FBB
		public EXRFormatException()
		{
		}

		// Token: 0x0600B78A RID: 46986 RVA: 0x003C5DC3 File Offset: 0x003C3FC3
		public EXRFormatException(string message)
			: base(message)
		{
		}

		// Token: 0x0600B78B RID: 46987 RVA: 0x003C5DCC File Offset: 0x003C3FCC
		public EXRFormatException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
