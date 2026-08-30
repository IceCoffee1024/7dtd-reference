using System;
using Audio;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02000851 RID: 2129
[Preserve]
public class NetPackageTwitchAccess : NetPackage
{
	// Token: 0x06003E33 RID: 15923 RVA: 0x001290F5 File Offset: 0x001272F5
	public NetPackageTwitchAccess Setup()
	{
		return this;
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x00189005 File Offset: 0x00187205
	public NetPackageTwitchAccess Setup(bool _hasAccess)
	{
		this.hasAccess = _hasAccess;
		return this;
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x0018900F File Offset: 0x0018720F
	public override void read(PooledBinaryReader _br)
	{
		this.hasAccess = _br.ReadBoolean();
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x0018901D File Offset: 0x0018721D
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.hasAccess);
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x00189034 File Offset: 0x00187234
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

	// Token: 0x06003E38 RID: 15928 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x04003225 RID: 12837
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasAccess;
}
