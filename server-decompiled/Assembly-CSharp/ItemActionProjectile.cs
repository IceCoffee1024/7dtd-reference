using System;
using UnityEngine.Scripting;

// Token: 0x020005BB RID: 1467
[Preserve]
public class ItemActionProjectile : ItemActionAttack
{
	// Token: 0x06002FB7 RID: 12215 RVA: 0x001324D0 File Offset: 0x001306D0
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.Explosion = new ExplosionData(this.Properties, this.item.Effects);
		this.Properties.ParseFloat("FlyTime", ref this.FlyTime);
		this.Properties.ParseFloat("LifeTime", ref this.LifeTime);
		this.Properties.ParseFloat("DeadTime", ref this.DeadTime);
		this.Properties.ParseFloat("Velocity", ref this.Velocity);
		this.Gravity = -9.81f;
		this.Properties.ParseFloat("Gravity", ref this.Gravity);
		this.Properties.ParseFloat("CollisionRadius", ref this.collisionRadius);
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
	}

	// Token: 0x040024DD RID: 9437
	public new ExplosionData Explosion;

	// Token: 0x040024DE RID: 9438
	public new float Velocity;

	// Token: 0x040024DF RID: 9439
	public new float FlyTime;

	// Token: 0x040024E0 RID: 9440
	public new float LifeTime;

	// Token: 0x040024E1 RID: 9441
	public float DeadTime;

	// Token: 0x040024E2 RID: 9442
	public float Gravity;

	// Token: 0x040024E3 RID: 9443
	public float collisionRadius;
}
