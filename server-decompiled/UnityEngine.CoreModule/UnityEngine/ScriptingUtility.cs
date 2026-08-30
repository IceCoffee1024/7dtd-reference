using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000258 RID: 600
	internal class ScriptingUtility
	{
		// Token: 0x06001975 RID: 6517 RVA: 0x0002A7F0 File Offset: 0x000289F0
		[RequiredByNativeCode]
		private static bool IsManagedCodeWorking()
		{
			ScriptingUtility.TestClass testClass = new ScriptingUtility.TestClass
			{
				value = 42
			};
			return testClass.value == 42;
		}

		// Token: 0x02000259 RID: 601
		private struct TestClass
		{
			// Token: 0x040008E0 RID: 2272
			public int value;
		}
	}
}
