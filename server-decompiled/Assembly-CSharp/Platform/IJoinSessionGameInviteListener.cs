using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B99 RID: 7065
	public interface IJoinSessionGameInviteListener
	{
		// Token: 0x0600D379 RID: 54137
		void Init(IPlatform _owner);

		// Token: 0x0600D37A RID: 54138
		[return: TupleElementNames(new string[] { "invite", "password" })]
		ValueTuple<string, string> TakePendingInvite();

		// Token: 0x0600D37B RID: 54139
		IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null);

		// Token: 0x0600D37C RID: 54140
		string GetListenerIdentifier();
	}
}
