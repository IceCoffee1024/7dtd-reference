using System;
using UnityEngine.Scripting;

// Token: 0x0200079A RID: 1946
[Preserve]
public class AuthFinalizer : AuthorizerAbs
{
	// Token: 0x06003A06 RID: 14854 RVA: 0x00179E86 File Offset: 0x00178086
	public AuthFinalizer()
	{
		AuthFinalizer.Instance = this;
	}

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x06003A07 RID: 14855 RVA: 0x00179E94 File Offset: 0x00178094
	public override int Order
	{
		get
		{
			return 999;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06003A08 RID: 14856 RVA: 0x00179E9B File Offset: 0x0017809B
	public override string AuthorizerName
	{
		get
		{
			return "Finalizer";
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06003A09 RID: 14857 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003A0A RID: 14858 RVA: 0x00179EA4 File Offset: 0x001780A4
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageAuthConfirmation>().Setup());
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x00179774 File Offset: 0x00177974
	public void ReplyReceived(ClientInfo _cInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x04002F74 RID: 12148
	public static AuthFinalizer Instance;
}
