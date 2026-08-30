using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000226 RID: 550
	[RequiredByNativeCode]
	internal class AsyncInstantiateOperationHelper
	{
		// Token: 0x0600182B RID: 6187 RVA: 0x00028042 File Offset: 0x00026242
		[RequiredByNativeCode]
		public static void SetAsyncInstantiateOperationResult(AsyncInstantiateOperation op, Object[] result)
		{
			op.m_Result = result;
		}
	}
}
