using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000449 RID: 1097
[Preserve]
public class EAIPathTest : EAIBase
{
	// Token: 0x06002181 RID: 8577 RVA: 0x000CAA33 File Offset: 0x000C8C33
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
		this.banditEntity = _theEntity as EntityBandit;
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x000CAA4F File Offset: 0x000C8C4F
	public void SetTargetMove(Vector3 _targetWorld, EntityAlive _targetYaw, bool _canBreakBlocks)
	{
		if (this.banditEntity != null)
		{
			this.targetWorldPosition = _targetWorld;
			this.targetYaw = _targetYaw;
			this.newTarget = true;
			this.cancel = false;
			this.canBreakBlocks = _canBreakBlocks;
		}
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x000CAA82 File Offset: 0x000C8C82
	public void CancelTargetMove()
	{
		this.targetWorldPosition = Vector3.zero;
		this.newTarget = false;
		this.cancel = true;
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x000CAA9D File Offset: 0x000C8C9D
	public override bool CanExecute()
	{
		return this.newTarget;
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x000CAAA5 File Offset: 0x000C8CA5
	public override bool Continue()
	{
		return !this.cancel;
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x000CAAB0 File Offset: 0x000C8CB0
	public override void Update()
	{
		base.Update();
		if (this.newTarget)
		{
			this.newTarget = false;
			PathInfoSingleTarget pathInfoSingleTarget = new PathInfoSingleTarget(this.theEntity, this.targetWorldPosition, this.canBreakBlocks, this.theEntity.GetMoveSpeed(), null);
			PathFinderThread.Instance.FindPath(pathInfoSingleTarget);
			if (this.banditEntity != null)
			{
				this.banditEntity.focusBody.SetFocus(FocusPriority.Highest, new AIFocusBody(this.targetYaw)
				{
					ConditionDistance = new AIFocusConditionDistance(5f, this.targetWorldPosition)
				});
			}
		}
		Vector3 vector = this.targetWorldPosition - Origin.position;
		Debug.DrawLine(vector, vector + Vector3.up * 2f, Color.blue);
		PathEntity path = this.theEntity.navigator.getPath();
		if (path != null)
		{
			for (int i = 0; i < path.getCurrentPathLength() - 1; i++)
			{
				Vector3 vector2 = path.getPathPointFromIndex(i).projectedLocation - Origin.position;
				Color color = new Color(1f, 1f, 0.1f);
				if (i < path.getCurrentPathIndex())
				{
					color = new Color(0.3f, 0.3f, 0f);
				}
				Utils.DrawLine(vector2, path.getPathPointFromIndex(i + 1).projectedLocation - Origin.position, color, color * 0.5f, 3, 0.1f);
			}
		}
	}

	// Token: 0x04001718 RID: 5912
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityBandit banditEntity;

	// Token: 0x04001719 RID: 5913
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetWorldPosition;

	// Token: 0x0400171A RID: 5914
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetYaw;

	// Token: 0x0400171B RID: 5915
	[PublicizedFrom(EAccessModifier.Private)]
	public bool newTarget;

	// Token: 0x0400171C RID: 5916
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancel;

	// Token: 0x0400171D RID: 5917
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canBreakBlocks;
}
