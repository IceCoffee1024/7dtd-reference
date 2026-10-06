using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02000339 RID: 825
public class DiscordInviteListener : IJoinSessionGameInviteListener
{
	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x060017D2 RID: 6098 RVA: 0x000892E6 File Offset: 0x000874E6
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

	// Token: 0x060017D3 RID: 6099 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(IPlatform _owner)
	{
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x000892FC File Offset: 0x000874FC
	public void SetPendingInvite(string _sessionId, string _password)
	{
		this.pendingActivityInvite = new ValueTuple<string, string>?(new ValueTuple<string, string>(_sessionId, _password));
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x00089310 File Offset: 0x00087510
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

	// Token: 0x060017D6 RID: 6102 RVA: 0x0008933E File Offset: 0x0008753E
	public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
	{
		yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
		yield break;
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x0008935B File Offset: 0x0008755B
	public string GetListenerIdentifier()
	{
		return "DCD";
	}

	// Token: 0x04000F69 RID: 3945
	[PublicizedFrom(EAccessModifier.Private)]
	public static DiscordInviteListener listenerInstance;

	// Token: 0x04000F6A RID: 3946
	[TupleElementNames(new string[] { "session", "password" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<string, string>? pendingActivityInvite;
}
