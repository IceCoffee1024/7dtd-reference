using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B67 RID: 7015
	public interface IJoinSessionGameInviteListener
	{
		// Token: 0x0600D201 RID: 53761
		void Init(IPlatform _owner);

		// Token: 0x0600D202 RID: 53762
		[return: TupleElementNames(new string[] { "invite", "password" })]
		ValueTuple<string, string> TakePendingInvite();

		// Token: 0x0600D203 RID: 53763
		IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null);

		// Token: 0x0600D204 RID: 53764
		string GetListenerIdentifier();
	}
}
