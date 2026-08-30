using System;
using Audio;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200085D RID: 2141
[Preserve]
public class NetPackageTwitchAccess : NetPackage
{
	// Token: 0x06003E98 RID: 16024 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackageTwitchAccess Setup()
	{
		return this;
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0018B0D5 File Offset: 0x001892D5
	public NetPackageTwitchAccess Setup(bool _hasAccess)
	{
		this.hasAccess = _hasAccess;
		return this;
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0018B0DF File Offset: 0x001892DF
	public override void read(PooledBinaryReader _br)
	{
		this.hasAccess = _br.ReadBoolean();
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0018B0ED File Offset: 0x001892ED
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.hasAccess);
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0018B104 File Offset: 0x00189304
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			AdminTools adminTools = GameManager.Instance.adminTools;
			bool flag = ((adminTools != null) ? adminTools.Users.GetUserPermissionLevel(base.Sender) : 1000) <= GamePrefs.GetInt(EnumGamePrefs.TwitchServerPermission);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTwitchAccess>().Setup(flag), false, base.Sender.entityId, -1, -1, null, 192, false);
			return;
		}
		if (this.hasAccess)
		{
			GameEventManager.Current.HandleGameEventAccessApproved();
			return;
		}
		Manager.PlayInsidePlayerHead("Misc/password_fail", -1, 0f, false, false);
		TwitchManager.Current.DeniedPermission();
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x040032A6 RID: 12966
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAccess;
}
