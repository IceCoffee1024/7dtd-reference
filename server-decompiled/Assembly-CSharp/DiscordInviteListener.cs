using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x02000336 RID: 822
public class DiscordInviteListener : IJoinSessionGameInviteListener
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00089032 File Offset: 0x00087232
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

	// Token: 0x060017D6 RID: 6102 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(IPlatform _owner)
	{
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00089048 File Offset: 0x00087248
	public void SetPendingInvite(string _sessionId, string _password)
	{
		this.pendingActivityInvite = new ValueTuple<string, string>?(new ValueTuple<string, string>(_sessionId, _password));
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x0008905C File Offset: 0x0008725C
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

	// Token: 0x060017D9 RID: 6105 RVA: 0x0008908A File Offset: 0x0008728A
	public IEnumerator ConnectToInvite(string _invite, string _password = null, Action<bool> _onFinished = null)
	{
		yield return InviteManager.HandleSessionIdInvite(_invite, _password, _onFinished);
		yield break;
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x000890A7 File Offset: 0x000872A7
	public string GetListenerIdentifier()
	{
		return "DCD";
	}

	// Token: 0x04000F70 RID: 3952
	[PublicizedFrom(EAccessModifier.Private)]
	public static DiscordInviteListener listenerInstance;

	// Token: 0x04000F71 RID: 3953
	[TupleElementNames(new string[] { "session", "password" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public ValueTuple<string, string>? pendingActivityInvite;
}
