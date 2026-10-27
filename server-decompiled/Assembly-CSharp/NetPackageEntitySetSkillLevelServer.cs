using System;
using UnityEngine.Scripting;

// Token: 0x020007E2 RID: 2018
[Preserve]
public class NetPackageEntitySetSkillLevelServer : NetPackageEntitySetSkillLevelClient
{
	// Token: 0x17000656 RID: 1622
	// (get) Token: 0x06003B99 RID: 15257 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B9A RID: 15258 RVA: 0x001801EE File Offset: 0x0017E3EE
	public new NetPackageEntitySetSkillLevelServer Setup(int _entityId, string skill, int _experience)
	{
		base.Setup(_entityId, skill, _experience);
		return this;
	}

	// Token: 0x06003B9B RID: 15259 RVA: 0x001801FC File Offset: 0x0017E3FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityPlayer entityPlayer = (EntityPlayer)_world.GetEntity(this.entityId);
		if (entityPlayer == null)
		{
			return;
		}
		if (entityPlayer.isEntityRemote)
		{
			entityPlayer.Progression.GetProgressionValue(this.skill).Level = this.level;
		}
	}
}
