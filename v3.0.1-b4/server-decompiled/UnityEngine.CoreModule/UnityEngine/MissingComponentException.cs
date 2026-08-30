using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	// Token: 0x0200025D RID: 605
	[Serializable]
	public class MissingComponentException : SystemException
	{
		// Token: 0x06001982 RID: 6530 RVA: 0x0002AD53 File Offset: 0x00028F53
		public MissingComponentException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0002AD6E File Offset: 0x00028F6E
		public MissingComponentException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0002AD85 File Offset: 0x00028F85
		public MissingComponentException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0002AD9D File Offset: 0x00028F9D
		protected MissingComponentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040008E4 RID: 2276
		private const int Result = -2147467261;

		// Token: 0x040008E5 RID: 2277
		private string unityStackTrace;
	}
}
