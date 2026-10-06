using System;
using UnityEngine.Scripting;

// Token: 0x020005C1 RID: 1473
[Preserve]
public class ItemActionProjectile : ItemActionAttack
{
	// Token: 0x06003028 RID: 12328 RVA: 0x00133FDC File Offset: 0x001321DC
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

	// Token: 0x06003029 RID: 12329 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
	}

	// Token: 0x0400250A RID: 9482
	public new ExplosionData Explosion;

	// Token: 0x0400250B RID: 9483
	public new float Velocity;

	// Token: 0x0400250C RID: 9484
	public new float FlyTime;

	// Token: 0x0400250D RID: 9485
	public new float LifeTime;

	// Token: 0x0400250E RID: 9486
	public float DeadTime;

	// Token: 0x0400250F RID: 9487
	public float Gravity;

	// Token: 0x04002510 RID: 9488
	public float collisionRadius;
}
