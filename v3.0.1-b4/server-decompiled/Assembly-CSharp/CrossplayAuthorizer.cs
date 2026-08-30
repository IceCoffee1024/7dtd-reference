using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000783 RID: 1923
[Preserve]
public class CrossplayAuthorizer : AuthorizerAbs
{
	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x0600393C RID: 14652 RVA: 0x00176214 File Offset: 0x00174414
	public override int Order
	{
		get
		{
			return 550;
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x0600393D RID: 14653 RVA: 0x0017621B File Offset: 0x0017441B
	public override string AuthorizerName
	{
		get
		{
			return "Crossplay";
		}
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x0600393E RID: 14654 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x00176224 File Offset: 0x00174424
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
