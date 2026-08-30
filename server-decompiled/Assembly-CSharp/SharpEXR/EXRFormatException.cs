using System;

namespace SharpEXR
{
	// Token: 0x020016C6 RID: 5830
	public class EXRFormatException : Exception
	{
		// Token: 0x0600B632 RID: 46642 RVA: 0x003C121F File Offset: 0x003BF41F
		public EXRFormatException()
		{
		}

		// Token: 0x0600B633 RID: 46643 RVA: 0x003C1227 File Offset: 0x003BF427
		public EXRFormatException(string message)
			: base(message)
		{
		}

		// Token: 0x0600B634 RID: 46644 RVA: 0x003C1230 File Offset: 0x003BF430
		public EXRFormatException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
