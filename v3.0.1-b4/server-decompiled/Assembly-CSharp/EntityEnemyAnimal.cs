using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004A7 RID: 1191
[Preserve]
public class EntityEnemyAnimal : EntityEnemy
{
	// Token: 0x06002584 RID: 9604 RVA: 0x000E4A0C File Offset: 0x000E2C0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.ModelTransform)
		{
			this.animator = this.ModelTransform.GetComponentInChildren<Animator>();
		}
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000D8E0F File Offset: 0x000D700F
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x06002586 RID: 9606 RVA: 0x000D8E25 File Offset: 0x000D7025
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x06002587 RID: 9607 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x000E4A34 File Offset: 0x000E2C34
	public override bool CanDamageEntity(int _sourceEntityId)
	{
		Entity entity = this.world.GetEntity(_sourceEntityId);
		return !entity || entity.entityClass != this.entityClass;
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x000E4A68 File Offset: 0x000E2C68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTasks()
	{
		if (this.Electrocuted)
		{
			base.SetMoveForward(0f);
			if (this.animator)
			{
				this.animator.enabled = false;
			}
			return;
		}
		if (this.animator)
		{
			this.animator.enabled = true;
		}
		base.updateTasks();
	}

	// Token: 0x04001BD8 RID: 7128
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
