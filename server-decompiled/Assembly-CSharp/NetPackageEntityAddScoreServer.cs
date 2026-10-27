using System;
using UnityEngine.Scripting;

// Token: 0x020007E4 RID: 2020
[Preserve]
public class NetPackageEntityAddScoreServer : NetPackageEntityAddScoreClient
{
	// Token: 0x17000658 RID: 1624
	// (get) Token: 0x06003BA3 RID: 15267 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0018035B File Offset: 0x0017E55B
	public new NetPackageEntityAddScoreServer Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		base.Setup(_entityId, _zombieKills, _playerKills, _otherTeamNumber, _conditions);
		return this;
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0018036C File Offset: 0x0017E56C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.AddScoreServer(this.entityId, this.zombieKills, this.playerKills, this.otherTeamNumber, this.conditions);
	}
}
