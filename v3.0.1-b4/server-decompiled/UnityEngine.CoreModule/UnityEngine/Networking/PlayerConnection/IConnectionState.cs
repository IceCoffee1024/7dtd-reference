using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Networking.PlayerConnection
{
	// Token: 0x020003CD RID: 973
	[MovedFrom("UnityEngine.Experimental.Networking.PlayerConnection")]
	public interface IConnectionState : IDisposable
	{
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002114 RID: 8468
		ConnectionTarget connectedToTarget { get; }

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002115 RID: 8469
		string connectionName { get; }
	}
}
