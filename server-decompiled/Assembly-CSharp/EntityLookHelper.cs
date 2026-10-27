using System;

// Token: 0x0200050F RID: 1295
public class EntityLookHelper
{
	// Token: 0x06002A24 RID: 10788 RVA: 0x00108B8F File Offset: 0x00106D8F
	public EntityLookHelper(EntityAlive _e)
	{
		this.entity = _e;
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x00108BA0 File Offset: 0x00106DA0
	public void onUpdateLook()
	{
		if (this.entity.rotation.x > 1f)
		{
			EntityAlive entityAlive = this.entity;
			entityAlive.rotation.x = entityAlive.rotation.x - 1f;
			return;
		}
		if (this.entity.rotation.x < -1f)
		{
			EntityAlive entityAlive2 = this.entity;
			entityAlive2.rotation.x = entityAlive2.rotation.x + 1f;
		}
	}

	// Token: 0x0400200D RID: 8205
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entity;
}
