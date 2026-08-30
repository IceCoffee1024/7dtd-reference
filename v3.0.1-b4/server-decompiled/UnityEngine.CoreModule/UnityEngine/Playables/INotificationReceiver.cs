using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000495 RID: 1173
	[RequiredByNativeCode]
	public interface INotificationReceiver
	{
		// Token: 0x06002861 RID: 10337
		[RequiredByNativeCode]
		void OnNotify(Playable origin, INotification notification, object context);
	}
}
