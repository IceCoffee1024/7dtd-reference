using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	// Token: 0x0200025E RID: 606
	[Serializable]
	public class UnassignedReferenceException : SystemException
	{
		// Token: 0x06001986 RID: 6534 RVA: 0x0002AD53 File Offset: 0x00028F53
		public UnassignedReferenceException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0002AD6E File Offset: 0x00028F6E
		public UnassignedReferenceException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0002AD85 File Offset: 0x00028F85
		public UnassignedReferenceException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0002AD9D File Offset: 0x00028F9D
		protected UnassignedReferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040008E6 RID: 2278
		private const int Result = -2147467261;

		// Token: 0x040008E7 RID: 2279
		private string unityStackTrace;
	}
}
