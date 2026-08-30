using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004AB RID: 1195
[Preserve]
public class EntityEnemyAnimal : EntityEnemy
{
	// Token: 0x060025A6 RID: 9638 RVA: 0x000E569C File Offset: 0x000E389C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.ModelTransform)
		{
			this.animator = this.ModelTransform.GetComponentInChildren<Animator>();
		}
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000D999F File Offset: 0x000D7B9F
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x000D99B5 File Offset: 0x000D7BB5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000E56C4 File Offset: 0x000E38C4
	public override bool CanDamageEntity(int _sourceEntityId)
	{
		Entity entity = this.world.GetEntity(_sourceEntityId);
		return !entity || entity.entityClass != this.entityClass;
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x000E56F8 File Offset: 0x000E38F8
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

	// Token: 0x04001BFA RID: 7162
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
