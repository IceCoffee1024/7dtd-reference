using System;
using UnityEngine.Scripting;

// Token: 0x020007D7 RID: 2007
[Preserve]
public class NetPackageEntityAddScoreServer : NetPackageEntityAddScoreClient
{
	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06003B67 RID: 15207 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0017ECD7 File Offset: 0x0017CED7
	public new NetPackageEntityAddScoreServer Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		base.Setup(_entityId, _zombieKills, _playerKills, _otherTeamNumber, _conditions);
		return this;
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x0017ECE8 File Offset: 0x0017CEE8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.AddScoreServer(this.entityId, this.zombieKills, this.playerKills, this.otherTeamNumber, this.conditions);
	}
}
