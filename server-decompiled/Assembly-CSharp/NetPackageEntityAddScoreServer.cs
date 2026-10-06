using System;
using UnityEngine.Scripting;

// Token: 0x020007DA RID: 2010
[Preserve]
public class NetPackageEntityAddScoreServer : NetPackageEntityAddScoreClient
{
	// Token: 0x17000652 RID: 1618
	// (get) Token: 0x06003B80 RID: 15232 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x0017F257 File Offset: 0x0017D457
	public new NetPackageEntityAddScoreServer Setup(int _entityId, int _zombieKills, int _playerKills, int _otherTeamNumber, int _conditions)
	{
		base.Setup(_entityId, _zombieKills, _playerKills, _otherTeamNumber, _conditions);
		return this;
	}

	// Token: 0x06003B82 RID: 15234 RVA: 0x0017F268 File Offset: 0x0017D468
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.gameManager.AddScoreServer(this.entityId, this.zombieKills, this.playerKills, this.otherTeamNumber, this.conditions);
	}
}
