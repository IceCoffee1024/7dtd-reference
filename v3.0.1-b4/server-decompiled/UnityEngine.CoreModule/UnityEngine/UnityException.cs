using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200025C RID: 604
	[RequiredByNativeCode]
	[Serializable]
	public class UnityException : SystemException
	{
		// Token: 0x0600197E RID: 6526 RVA: 0x0002AD53 File Offset: 0x00028F53
		public UnityException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0002AD6E File Offset: 0x00028F6E
		public UnityException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0002AD85 File Offset: 0x00028F85
		public UnityException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0002AD9D File Offset: 0x00028F9D
		protected UnityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040008E2 RID: 2274
		private const int Result = -2147467261;

		// Token: 0x040008E3 RID: 2275
		private string unityStackTrace;
	}
}
