using System;

// Token: 0x0200050D RID: 1293
public class EntityLookHelper
{
	// Token: 0x06002A1D RID: 10781 RVA: 0x0010863B File Offset: 0x0010683B
	public EntityLookHelper(EntityAlive _e)
	{
		this.entity = _e;
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x0010864C File Offset: 0x0010684C
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

	// Token: 0x04001FFD RID: 8189
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entity;
}
