using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001B40 RID: 6976
	public interface IJoinSessionGameInviteListener
	{
		// Token: 0x0600D076 RID: 53366
		void Init(IPlatform _owner);

		// Token: 0x0600D077 RID: 53367
		[return: TupleElementNames(new string[] { "invite", "password" })]
		ValueTuple<string, string> TakePendingInvite();

		// Token: 0x0600D078 RID: 53368
		IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null);

		// Token: 0x0600D079 RID: 53369
		string GetListenerIdentifier();
	}
}
