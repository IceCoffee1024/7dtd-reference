using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000460 RID: 1120
[Preserve]
public class EAITerritorial : EAIBase
{
	// Token: 0x060021F1 RID: 8689 RVA: 0x000C99F5 File Offset: 0x000C7BF5
	public EAITerritorial()
	{
		this.MutexBits = 1;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000C96EC File Offset: 0x000C78EC
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000CDBEC File Offset: 0x000CBDEC
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

	// Token: 0x060021F4 RID: 8692 RVA: 0x000CDC47 File Offset: 0x000CBE47
	public override bool Continue()
	{
		return !this.theEntity.getNavigator().noPathAndNotPlanningOne();
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000CDC5C File Offset: 0x000CBE5C
	public override void Start()
	{
		this.theEntity.FindPath(this.movePos, this.theEntity.GetMoveSpeed(), false, this);
	}

	// Token: 0x04001783 RID: 6019
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 movePos;
}
