using System;
using UnityEngine.Scripting;

// Token: 0x020005C0 RID: 1472
[Preserve]
public class ItemActionProjectile : ItemActionAttack
{
	// Token: 0x06002FE5 RID: 12261 RVA: 0x00133540 File Offset: 0x00131740
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

	// Token: 0x06002FE6 RID: 12262 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
	}

	// Token: 0x04002511 RID: 9489
	public new ExplosionData Explosion;

	// Token: 0x04002512 RID: 9490
	public new float Velocity;

	// Token: 0x04002513 RID: 9491
	public new float FlyTime;

	// Token: 0x04002514 RID: 9492
	public new float LifeTime;

	// Token: 0x04002515 RID: 9493
	public float DeadTime;

	// Token: 0x04002516 RID: 9494
	public float Gravity;

	// Token: 0x04002517 RID: 9495
	public float collisionRadius;
}
