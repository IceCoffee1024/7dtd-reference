using System;
using Audio;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200086A RID: 2154
[Preserve]
public class NetPackageTwitchAccess : NetPackage
{
	// Token: 0x06003E5D RID: 15965 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackageTwitchAccess Setup()
	{
		return this;
	}

	// Token: 0x06003E5E RID: 15966 RVA: 0x0018C019 File Offset: 0x0018A219
	public NetPackageTwitchAccess Setup(bool _hasAccess)
	{
		this.hasAccess = _hasAccess;
		return this;
	}

	// Token: 0x06003E5F RID: 15967 RVA: 0x0018C023 File Offset: 0x0018A223
	public override void read(PooledBinaryReader _br)
	{
		this.hasAccess = _br.ReadBoolean();
	}

	// Token: 0x06003E60 RID: 15968 RVA: 0x0018C031 File Offset: 0x0018A231
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.hasAccess);
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x0018C048 File Offset: 0x0018A248
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

	// Token: 0x040032B1 RID: 12977
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAccess;
}
