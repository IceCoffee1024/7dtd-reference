using System;

// Token: 0x02000502 RID: 1282
public class EntityLookHelper
{
	// Token: 0x060029FE RID: 10750 RVA: 0x00106CC7 File Offset: 0x00104EC7
	public EntityLookHelper(EntityAlive _e)
	{
		this.entity = _e;
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x00106CD8 File Offset: 0x00104ED8
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

	// Token: 0x04001FC6 RID: 8134
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entity;
}
