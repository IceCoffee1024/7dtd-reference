using System;
using UnityEngine.Scripting;

// Token: 0x020007CC RID: 1996
[Preserve]
public class NetPackageEntityAddScoreServer : NetPackageEntityAddScoreClient
{
	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06003B08 RID: 15112 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B09 RID: 15113 RVA: 0x0017C8FF File Offset: 0x0017AAFF
	public new NetPackageEntityAddScoreServer Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		base.Setup(_entityId, _zombieKills, _playerKills, _otherTeamNumber, _conditions);
		return this;
	}

	// Token: 0x06003B0A RID: 15114 RVA: 0x0017C910 File Offset: 0x0017AB10
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.AddScoreServer(this.entityId, this.zombieKills, this.playerKills, this.otherTeamNumber, this.conditions);
	}
}
