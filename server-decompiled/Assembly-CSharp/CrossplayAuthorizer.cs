using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200078F RID: 1935
[Preserve]
public class CrossplayAuthorizer : AuthorizerAbs
{
	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060039D6 RID: 14806 RVA: 0x00178C4C File Offset: 0x00176E4C
	public override int Order
	{
		get
		{
			return 550;
		}
	}

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x060039D7 RID: 14807 RVA: 0x00178C53 File Offset: 0x00176E53
	public override string AuthorizerName
	{
		get
		{
			return "Crossplay";
		}
	}

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x060039D8 RID: 14808 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x00178C5C File Offset: 0x00176E5C
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		bool @bool = GamePrefs.GetBool(EnumGamePrefs.ServerAllowCrossplay);
		IUserClient user = PlatformManager.MultiPlatform.User;
		bool flag = user == null || user.Permissions.HasCrossplay();
		if (@bool && flag)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		if (_clientInfo.device.ToPlayGroup() != DeviceFlag.StandaloneWindows.ToPlayGroup())
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.CrossplayDisabled, 0, default(DateTime), "")));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
