using System;
using UnityEngine.Scripting;

// Token: 0x02000792 RID: 1938
[Preserve]
public class AuthFinalizer : AuthorizerAbs
{
	// Token: 0x060039EF RID: 14831 RVA: 0x00178EEE File Offset: 0x001770EE
	public AuthFinalizer()
	{
		AuthFinalizer.Instance = this;
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060039F0 RID: 14832 RVA: 0x00178EFC File Offset: 0x001770FC
	public override int Order
	{
		get
		{
			return 999;
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060039F1 RID: 14833 RVA: 0x00178F03 File Offset: 0x00177103
	public override string AuthorizerName
	{
		get
		{
			return "Finalizer";
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x060039F2 RID: 14834 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039F3 RID: 14835 RVA: 0x00178F0C File Offset: 0x0017710C
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageAuthConfirmation>().Setup());
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
	}

	// Token: 0x060039F4 RID: 14836 RVA: 0x001787DC File Offset: 0x001769DC
	public void ReplyReceived(ClientInfo _cInfo)
	{
		this.authResponsesHandler.AuthorizationAccepted(this, _cInfo);
	}

	// Token: 0x04002F58 RID: 12120
	public static AuthFinalizer Instance;
}
