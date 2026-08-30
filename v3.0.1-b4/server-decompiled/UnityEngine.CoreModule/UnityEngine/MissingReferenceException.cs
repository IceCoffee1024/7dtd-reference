using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	// Token: 0x0200025F RID: 607
	[Serializable]
	public class MissingReferenceException : SystemException
	{
		// Token: 0x0600198A RID: 6538 RVA: 0x0002AD53 File Offset: 0x00028F53
		public MissingReferenceException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0002AD6E File Offset: 0x00028F6E
		public MissingReferenceException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0002AD85 File Offset: 0x00028F85
		public MissingReferenceException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0002AD9D File Offset: 0x00028F9D
		protected MissingReferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040008E8 RID: 2280
		private const int Result = -2147467261;

		// Token: 0x040008E9 RID: 2281
		private string unityStackTrace;
	}
}
