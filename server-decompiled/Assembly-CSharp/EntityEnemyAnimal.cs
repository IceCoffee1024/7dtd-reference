using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004B3 RID: 1203
[Preserve]
public class EntityEnemyAnimal : EntityEnemy
{
	// Token: 0x060025AC RID: 9644 RVA: 0x000E5DA4 File Offset: 0x000E3FA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		if (this.ModelTransform)
		{
			this.animator = this.ModelTransform.GetComponentInChildren<Animator>();
		}
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x000D9E3F File Offset: 0x000D803F
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x000D9E55 File Offset: 0x000D8055
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x000E5DCC File Offset: 0x000E3FCC
	public override bool CanDamageEntity(int _sourceEntityId)
	{
		Entity entity = this.world.GetEntity(_sourceEntityId);
		return !entity || entity.entityClass != this.entityClass;
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x000E5E00 File Offset: 0x000E4000
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

	// Token: 0x04001C0B RID: 7179
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator animator;
}
