using System;
using Audio;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000860 RID: 2144
[Preserve]
public class NetPackageTwitchAccess : NetPackage
{
	// Token: 0x06003E3A RID: 15930 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackageTwitchAccess Setup()
	{
		return this;
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x0018AF71 File Offset: 0x00189171
	public NetPackageTwitchAccess Setup(bool _hasAccess)
	{
		this.hasAccess = _hasAccess;
		return this;
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x0018AF7B File Offset: 0x0018917B
	public override void read(PooledBinaryReader _br)
	{
		this.hasAccess = _br.ReadBoolean();
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x0018AF89 File Offset: 0x00189189
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.hasAccess);
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x0018AFA0 File Offset: 0x001891A0
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

	// Token: 0x04003291 RID: 12945
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAccess;
}
