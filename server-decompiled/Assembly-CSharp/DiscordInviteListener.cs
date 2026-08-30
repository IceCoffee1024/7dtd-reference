using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02000332 RID: 818
public class DiscordInviteListener : IJoinSessionGameInviteListener
{
	// Token: 0x170002DD RID: 733
	// (get) Token: 0x060017C0 RID: 6080 RVA: 0x00088992 File Offset: 0x00086B92
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

	// Token: 0x060017C1 RID: 6081 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(IPlatform _owner)
	{
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000889A8 File Offset: 0x00086BA8
	public void SetPendingInvite(string _sessionId, string _password)
	{
		this.pendingActivityInvite = new ValueTuple<string, string>?(new ValueTuple<string, string>(_sessionId, _password));
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x000889BC File Offset: 0x00086BBC
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

	// Token: 0x060017C4 RID: 6084 RVA: 0x000889EA File Offset: 0x00086BEA
	public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
	{
		yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
		yield break;
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x00088A07 File Offset: 0x00086C07
	public string GetListenerIdentifier()
	{
		return "DCD";
	}

	// Token: 0x04000F5F RID: 3935
	[PublicizedFrom(EAccessModifier.Private)]
	public static DiscordInviteListener listenerInstance;

	// Token: 0x04000F60 RID: 3936
	[TupleElementNames(new string[] { "session", "password" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<string, string>? pendingActivityInvite;
}
