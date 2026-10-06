using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000487 RID: 1159
[Preserve]
public class EntityAnimalSnake : EntityEnemyAnimal
{
	// Token: 0x060023E7 RID: 9191 RVA: 0x000D9C70 File Offset: 0x000D7E70
	public override Vector3 GetAttackTargetHitPosition()
	{
		Vector3 position = this.attackTarget.position;
		position.y += 0.5f;
		return position;
	}
}
