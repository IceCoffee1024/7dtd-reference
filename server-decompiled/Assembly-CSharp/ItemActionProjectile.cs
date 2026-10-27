using System;
using UnityEngine.Scripting;

// Token: 0x020005C8 RID: 1480
[Preserve]
public class ItemActionProjectile : ItemActionAttack
{
	// Token: 0x06003036 RID: 12342 RVA: 0x00134BD0 File Offset: 0x00132DD0
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

	// Token: 0x06003037 RID: 12343 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
	}

	// Token: 0x04002525 RID: 9509
	public new ExplosionData Explosion;

	// Token: 0x04002526 RID: 9510
	public new float Velocity;

	// Token: 0x04002527 RID: 9511
	public new float FlyTime;

	// Token: 0x04002528 RID: 9512
	public new float LifeTime;

	// Token: 0x04002529 RID: 9513
	public float DeadTime;

	// Token: 0x0400252A RID: 9514
	public float Gravity;

	// Token: 0x0400252B RID: 9515
	public float collisionRadius;
}
