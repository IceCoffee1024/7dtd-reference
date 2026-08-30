using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000458 RID: 1112
[Preserve]
public class EAITerritorial : EAIBase
{
	// Token: 0x060021E0 RID: 8672 RVA: 0x000C8D89 File Offset: 0x000C6F89
	public EAITerritorial()
	{
		this.MutexBits = 1;
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000C8A80 File Offset: 0x000C6C80
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000CCFF0 File Offset: 0x000CB1F0
	public override bool CanExecute()
	{
		if (this.theEntity.isWithinHomeDistanceCurrentPosition())
		{
			return false;
		}
		ChunkCoordinates homePosition = this.theEntity.getHomePosition();
		Vector3 vector = RandomPositionGenerator.CalcTowards(this.theEntity, 5, 15, 7, homePosition.position.ToVector3());
		if (vector.Equals(Vector3.zero))
		{
			return false;
		}
		this.movePos = vector;
		return true;
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000CD04B File Offset: 0x000CB24B
	public override bool Continue()
	{
		return !this.theEntity.getNavigator().noPathAndNotPlanningOne();
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000CD060 File Offset: 0x000CB260
	public override void Start()
	{
		this.theEntity.FindPath(this.movePos, this.theEntity.GetMoveSpeed(), false, this);
	}

	// Token: 0x04001766 RID: 5990
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 movePos;
}
