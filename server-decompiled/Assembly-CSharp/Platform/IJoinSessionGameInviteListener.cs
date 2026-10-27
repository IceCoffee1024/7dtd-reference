using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Platform
{
	// Token: 0x02001BCA RID: 7114
	public interface IJoinSessionGameInviteListener
	{
		// Token: 0x0600D48D RID: 54413
		void Init(IPlatform _owner);

		// Token: 0x0600D48E RID: 54414
		[return: TupleElementNames(new string[] { "invite", "password" })]
		ValueTuple<string, string> TakePendingInvite();

		// Token: 0x0600D48F RID: 54415
		IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null);

		// Token: 0x0600D490 RID: 54416
		string GetListenerIdentifier();
	}
}
