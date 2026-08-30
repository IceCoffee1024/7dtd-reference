using System;

namespace UnityEngine.Assertions
{
	// Token: 0x020004F6 RID: 1270
	internal class AssertionMessageUtil
	{
		// Token: 0x06002C4A RID: 11338 RVA: 0x0004A61C File Offset: 0x0004881C
		public static string GetMessage(string failureMessage)
		{
			return UnityString.Format("{0} {1}", new object[] { "Assertion failure.", failureMessage });
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x0004A64C File Offset: 0x0004884C
		public static string GetMessage(string failureMessage, string expected)
		{
			return AssertionMessageUtil.GetMessage(UnityString.Format("{0}{1}{2} {3}", new object[]
			{
				failureMessage,
				Environment.NewLine,
				"Expected:",
				expected
			}));
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x0004A68C File Offset: 0x0004888C
		public static string GetEqualityMessage(object actual, object expected, bool expectEqual)
		{
			return AssertionMessageUtil.GetMessage(UnityString.Format("Values are {0}equal.", new object[] { expectEqual ? "not " : "" }), UnityString.Format("{0} {2} {1}", new object[]
			{
				actual,
				expected,
				expectEqual ? "==" : "!="
			}));
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x0004A6F0 File Offset: 0x000488F0
		public static string NullFailureMessage(object value, bool expectNull)
		{
			return AssertionMessageUtil.GetMessage(UnityString.Format("Value was {0}Null", new object[] { expectNull ? "not " : "" }), UnityString.Format("Value was {0}Null", new object[] { expectNull ? "" : "not " }));
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x0004A74C File Offset: 0x0004894C
		public static string BooleanFailureMessage(bool expected)
		{
			return AssertionMessageUtil.GetMessage("Value was " + (!expected).ToString(), expected.ToString());
		}

		// Token: 0x0400112F RID: 4399
		private const string k_Expected = "Expected:";

		// Token: 0x04001130 RID: 4400
		private const string k_AssertionFailed = "Assertion failure.";
	}
}
