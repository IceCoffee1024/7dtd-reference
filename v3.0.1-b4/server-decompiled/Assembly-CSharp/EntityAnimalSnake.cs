using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000481 RID: 1153
[Preserve]
public class EntityAnimalSnake : EntityEnemyAnimal
{
	// Token: 0x060023D3 RID: 9171 RVA: 0x000D907C File Offset: 0x000D727C
	public override Vector3 GetAttackTargetHitPosition()
	{
		Vector3 position = this.attackTarget.position;
		position.y += 0.5f;
		return position;
	}
}
