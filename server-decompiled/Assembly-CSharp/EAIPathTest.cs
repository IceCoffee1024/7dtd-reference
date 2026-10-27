using System;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000451 RID: 1105
[Preserve]
public class EAIPathTest : EAIBase
{
	// Token: 0x06002192 RID: 8594 RVA: 0x000CB69F File Offset: 0x000C989F
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
		this.banditEntity = _theEntity as EntityBandit;
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000CB6BB File Offset: 0x000C98BB
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

	// Token: 0x06002194 RID: 8596 RVA: 0x000CB6EE File Offset: 0x000C98EE
	public void CancelTargetMove()
	{
		this.targetWorldPosition = Vector3.zero;
		this.newTarget = false;
		this.cancel = true;
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000CB709 File Offset: 0x000C9909
	public override bool CanExecute()
	{
		return this.newTarget;
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000CB711 File Offset: 0x000C9911
	public override bool Continue()
	{
		return !this.cancel;
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000CB71C File Offset: 0x000C991C
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

	// Token: 0x04001734 RID: 5940
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityBandit banditEntity;

	// Token: 0x04001735 RID: 5941
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 targetWorldPosition;

	// Token: 0x04001736 RID: 5942
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive targetYaw;

	// Token: 0x04001737 RID: 5943
	[PublicizedFrom(EAccessModifier.Private)]
	public bool newTarget;

	// Token: 0x04001738 RID: 5944
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancel;

	// Token: 0x04001739 RID: 5945
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canBreakBlocks;
}
