using System;
using UnityEngine.Scripting;

// Token: 0x02000786 RID: 1926
[Preserve]
public class AuthFinalizer : AuthorizerAbs
{
	// Token: 0x06003955 RID: 14677 RVA: 0x001764B6 File Offset: 0x001746B6
	public AuthFinalizer()
	{
		AuthFinalizer.Instance = this;
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06003956 RID: 14678 RVA: 0x001764C4 File Offset: 0x001746C4
	public override int Order
	{
		get
		{
			return 999;
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x06003957 RID: 14679 RVA: 0x001764CB File Offset: 0x001746CB
	public override string AuthorizerName
	{
		get
		{
			return "Finalizer";
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06003958 RID: 14680 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x001764D4 File Offset: 0x001746D4
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageAuthConfirmation>().Setup());
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x00175DA4 File Offset: 0x00173FA4
	public void ReplyReceived(ClientInfo _cInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x04002EE5 RID: 12005
	public static AuthFinalizer Instance;
}
