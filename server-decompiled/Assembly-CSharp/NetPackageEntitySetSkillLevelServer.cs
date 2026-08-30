using System;
using UnityEngine.Scripting;

// Token: 0x020007CA RID: 1994
[Preserve]
public class NetPackageEntitySetSkillLevelServer : NetPackageEntitySetSkillLevelClient
{
	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06003AFD RID: 15101 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x0017C792 File Offset: 0x0017A992
	public new NetPackageEntitySetSkillLevelServer Setup(int _entityId, string skill, int _experience)
	{
		base.Setup(_entityId, skill, _experience);
		return this;
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x0017C7A0 File Offset: 0x0017A9A0
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
