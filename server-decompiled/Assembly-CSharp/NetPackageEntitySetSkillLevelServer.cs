using System;
using UnityEngine.Scripting;

// Token: 0x020007D5 RID: 2005
[Preserve]
public class NetPackageEntitySetSkillLevelServer : NetPackageEntitySetSkillLevelClient
{
	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06003B5C RID: 15196 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x0017EB6A File Offset: 0x0017CD6A
	public new NetPackageEntitySetSkillLevelServer Setup(int _entityId, string skill, int _experience)
	{
		base.Setup(_entityId, skill, _experience);
		return this;
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x0017EB78 File Offset: 0x0017CD78
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
