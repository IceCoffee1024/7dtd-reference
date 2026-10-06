using System;
using UnityEngine.Scripting;

// Token: 0x020007D8 RID: 2008
[Preserve]
public class NetPackageEntitySetSkillLevelServer : NetPackageEntitySetSkillLevelClient
{
	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x06003B76 RID: 15222 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x0017F0EA File Offset: 0x0017D2EA
	public new NetPackageEntitySetSkillLevelServer Setup(int _entityId, string skill, int _experience)
	{
		base.Setup(_entityId, skill, _experience);
		return this;
	}

	// Token: 0x06003B78 RID: 15224 RVA: 0x0017F0F8 File Offset: 0x0017D2F8
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
