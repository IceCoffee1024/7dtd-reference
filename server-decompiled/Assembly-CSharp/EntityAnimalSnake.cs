using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000489 RID: 1161
[Preserve]
public class EntityAnimalSnake : EntityEnemyAnimal
{
	// Token: 0x060023ED RID: 9197 RVA: 0x000D9FF4 File Offset: 0x000D81F4
	public override Vector3 GetAttackTargetHitPosition()
	{
		Vector3 position = this.attackTarget.position;
		position.y += 0.5f;
		return position;
	}
}
