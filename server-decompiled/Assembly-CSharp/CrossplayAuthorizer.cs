using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000797 RID: 1943
[Preserve]
public class CrossplayAuthorizer : AuthorizerAbs
{
	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x060039ED RID: 14829 RVA: 0x00179BE4 File Offset: 0x00177DE4
	public override int Order
	{
		get
		{
			return 550;
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x060039EE RID: 14830 RVA: 0x00179BEB File Offset: 0x00177DEB
	public override string AuthorizerName
	{
		get
		{
			return "Crossplay";
		}
	}

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x060039EF RID: 14831 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x00179BF4 File Offset: 0x00177DF4
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
