using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200045E RID: 1118
[Preserve]
public class EAITerritorial : EAIBase
{
	// Token: 0x060021EC RID: 8684 RVA: 0x000C9649 File Offset: 0x000C7849
	public EAITerritorial()
	{
		this.MutexBits = 1;
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000C9340 File Offset: 0x000C7540
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000CD840 File Offset: 0x000CBA40
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

	// Token: 0x060021EF RID: 8687 RVA: 0x000CD89B File Offset: 0x000CBA9B
	public override bool Continue()
	{
		return !this.theEntity.getNavigator().noPathAndNotPlanningOne();
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000CD8B0 File Offset: 0x000CBAB0
	public override void Start()
	{
		this.theEntity.FindPath(this.movePos, this.theEntity.GetMoveSpeed(), false, this);
	}

	// Token: 0x04001773 RID: 6003
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 movePos;
}
