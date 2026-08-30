using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200027A RID: 634
	[RequiredByNativeCode]
	public interface ISerializationCallbackReceiver
	{
		// Token: 0x06001A35 RID: 6709
		[RequiredByNativeCode]
		void OnBeforeSerialize();

		// Token: 0x06001A36 RID: 6710
		[RequiredByNativeCode]
		void OnAfterDeserialize();
	}
}
