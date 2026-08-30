using System;

namespace UnityEngine.Assertions
{
	// Token: 0x020004F5 RID: 1269
	public class AssertionException : Exception
	{
		// Token: 0x06002C48 RID: 11336 RVA: 0x0004A5CB File Offset: 0x000487CB
		public AssertionException(string message, string userMessage)
			: base(message)
		{
			this.m_UserMessage = userMessage;
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x0004A5E0 File Offset: 0x000487E0
		public override string Message
		{
			get
			{
				string text = base.Message;
				bool flag = this.m_UserMessage != null;
				if (flag)
				{
					text = this.m_UserMessage + "\n" + text;
				}
				return text;
			}
		}

		// Token: 0x0400112E RID: 4398
		private string m_UserMessage;
	}
}
