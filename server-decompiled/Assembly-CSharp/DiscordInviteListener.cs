using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x0200033B RID: 827
public class DiscordInviteListener : IJoinSessionGameInviteListener
{
	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0008951A File Offset: 0x0008771A
	public static DiscordInviteListener ListenerInstance
	{
		get
		{
			DiscordInviteListener discordInviteListener;
			if ((discordInviteListener = DiscordInviteListener.listenerInstance) == null)
			{
				discordInviteListener = (DiscordInviteListener.listenerInstance = new DiscordInviteListener());
			}
			return discordInviteListener;
		}
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(IPlatform _owner)
	{
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00089530 File Offset: 0x00087730
	public void SetPendingInvite(string _sessionId, string _password)
	{
		this.pendingActivityInvite = new ValueTuple<string, string>?(new ValueTuple<string, string>(_sessionId, _password));
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00089544 File Offset: 0x00087744
	[return: TupleElementNames(new string[] { "invite", "password" })]
	public ValueTuple<string, string> TakePendingInvite()
	{
		if (this.pendingActivityInvite == null)
		{
			return new ValueTuple<string, string>(null, null);
		}
		ValueTuple<string, string> value = this.pendingActivityInvite.Value;
		this.pendingActivityInvite = null;
		return value;
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00089572 File Offset: 0x00087772
	public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
	{
		yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
		yield break;
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x0008958F File Offset: 0x0008778F
	public string GetListenerIdentifier()
	{
		return "DCD";
	}

	// Token: 0x04000F79 RID: 3961
	[PublicizedFrom(EAccessModifier.Private)]
	public static DiscordInviteListener listenerInstance;

	// Token: 0x04000F7A RID: 3962
	[TupleElementNames(new string[] { "session", "password" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<string, string>? pendingActivityInvite;
}
