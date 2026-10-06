using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004B1 RID: 1201
[Preserve]
public class EntityEnemyAnimal : EntityEnemy
{
	// Token: 0x060025A5 RID: 9637 RVA: 0x000E5994 File Offset: 0x000E3B94
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.ModelTransform)
		{
			this.animator = this.ModelTransform.GetComponentInChildren<Animator>();
		}
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000D9ABB File Offset: 0x000D7CBB
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000D9AD1 File Offset: 0x000D7CD1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000E59BC File Offset: 0x000E3BBC
	public override bool CanDamageEntity(int _sourceEntityId)
	{
		Entity entity = this.world.GetEntity(_sourceEntityId);
		return !entity || entity.entityClass != this.entityClass;
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000E59F0 File Offset: 0x000E3BF0
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

	// Token: 0x04001BFB RID: 7163
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
