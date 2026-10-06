using System;
using System.Collections.Generic;
using GamePath;
using RaycastPathing;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class EAIDroneItemTask : EAIItemTask
{
	// Token: 0x06002131 RID: 8497 RVA: 0x000C88B7 File Offset: 0x000C6AB7
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.drone = _theEntity as EntityDrone;
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000C88CC File Offset: 0x000C6ACC
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "enterTime", ref this.attackEnterTime);
		base.GetData(data, "actionTime", ref this.actionTime);
		base.GetData(data, "exitTime", ref this.attackExitTime);
		this.attackEnterTimer = this.attackEnterTime;
		base.GetData(data, "fleeDist", ref this.fleeDist);
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000C8934 File Offset: 0x000C6B34
	public override bool CanExecute()
	{
		return this.drone && PathFinderThread.Instance != null && base.CanExecute();
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000C8954 File Offset: 0x000C6B54
	public override bool Continue()
	{
		return this.drone && base.Continue();
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000C896C File Offset: 0x000C6B6C
	public override void Update()
	{
		if (!this.drone)
		{
			return;
		}
		base.Update();
		if (this.attackEnterTimer > 0f)
		{
			this.attackEnterTimer -= 0.05f;
			if (this.attackEnterTimer <= 0f)
			{
				this.OnAttackEnter();
			}
		}
		if (this.actionTimer > 0f)
		{
			this.actionTimer -= 0.05f;
			if (this.actionTimer <= 0f)
			{
				this.OnAttackAction();
			}
		}
		if (this.attackExitTimer > 0f)
		{
			this.attackExitTimer -= 0.05f;
			if (this.attackExitTimer <= 0f)
			{
				this.OnAttackExit();
			}
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000C8A23 File Offset: 0x000C6C23
	public override void Reset()
	{
		base.Reset();
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x000C8A2B File Offset: 0x000C6C2B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAttackEnter()
	{
		this.actionTimer = this.actionTime;
		this.drone.GetInstalledWeapon(this.ItemKey).PlayBark();
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x000C8A4F File Offset: 0x000C6C4F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAttackAction()
	{
		this.attackExitTimer = this.attackExitTime;
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x000C8A60 File Offset: 0x000C6C60
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnAttackExit()
	{
		this.attackEnterTimer = this.attackEnterTime;
		this.actionTimer = 0f;
		this.attackExitTimer = 0f;
		this.drone.SetRevengeTarget(null);
		this.drone.SetAttackTarget(null, 0);
		this.weaponDischarged = false;
		this.ClearPath();
		this.drone.SetState((this.drone.OrderState == EntityDrone.Orders.Stay) ? EntityDrone.State.Sentry : EntityDrone.State.Idle, true);
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000C8AD4 File Offset: 0x000C6CD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool GetPath(Vector3 seekPos, float seekDist, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		return EntityDrone.GetPath(this.currentPath, this.drone, this.drone.position, seekPos, this.drone.SpeedFlying, this, seekDist, pointRadius, debugDraw, duration);
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000C8B15 File Offset: 0x000C6D15
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] GetGroupPositions(EntityAlive target, float dist, bool debugDraw = false, float duration = 0f)
	{
		return EntityDrone.GetGroupPositions(target, dist, debugDraw, duration);
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000C8B21 File Offset: 0x000C6D21
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool IsPositionBlocked(Vector3 start, Vector3 end, int layerMask = 1073807360, bool debugDraw = false, float duration = 0f)
	{
		return EntityDrone.IsPositionBlocked(start, end, layerMask, debugDraw, 0f);
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000C8B32 File Offset: 0x000C6D32
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ClearPath()
	{
		this.currentPath.Clear();
		this.drone.OnPathInterupted();
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000C8B4C File Offset: 0x000C6D4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FollowPlannedPath(float speed, float pointRadius = 0.1f, bool debugDraw = false, float duration = 0f)
	{
		if (this.currentPath.Count > 0)
		{
			this.currentPathTarget = this.currentPath[0];
			this.drone.RotateTo((this.currentPathTarget - this.drone.position).normalized);
			this.drone.Move(this.currentPathTarget, pointRadius);
			if (this.drone.IsInRange(this.currentPathTarget, pointRadius))
			{
				this.currentPath.RemoveAt(0);
				return;
			}
			if (debugDraw && this.currentPath.Count > 1)
			{
				RaycastPathUtils.DrawLine(this.currentPath[0], this.currentPath[1], Color.green, 1f);
			}
			if (this.pathTracker.IsStuck(this.drone.position, this.currentPath[0], 0.5f, false))
			{
				if (this.currentPath.Count > 1)
				{
					this.drone.TeleportToPosition(this.currentPath[1]);
					this.currentPath.RemoveRange(0, 2);
					return;
				}
				this.drone.TeleportToPosition(this.currentPath[0]);
				this.currentPath.RemoveAt(0);
				return;
			}
		}
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000C8C94 File Offset: 0x000C6E94
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool DoMoveIntoAtkPos(EntityAlive avaliableTarget, float seekDist, Vector3 seekForward, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		Vector3 position = this.drone.position;
		Vector3 chestPosition = avaliableTarget.getChestPosition();
		Vector3 normalized = (chestPosition - position).normalized;
		bool flag = Vector3.Dot(seekForward, normalized) >= 0.9f;
		float magnitude = (chestPosition - position).magnitude;
		Utils.DrawCircleLinesHorzontal(position - Origin.position, seekDist, Color.white, Color.red, 24, 0.05f);
		if (this.currentPath.Count == 0)
		{
			Vector3[] groupPositions = this.GetGroupPositions(avaliableTarget, 1.414f, false, 0f);
			Array.Sort<Vector3>(groupPositions, (Vector3 x, Vector3 y) => Vector3.Distance(position, x).CompareTo(Vector3.Distance(position, y)));
			Vector3 vector = groupPositions[0];
			this.GetPath(vector, seekDist, pointRadius, debugDraw, duration);
			if (this.currentPath.Count > 0)
			{
				this.currentPathDest = this.currentPath[this.currentPath.Count - 1];
			}
			else
			{
				this.currentPathDest = vector;
			}
		}
		else
		{
			if ((chestPosition - this.currentPathDest).magnitude > seekDist)
			{
				this.ClearPath();
			}
			if (this.IsPositionBlocked(position, chestPosition, 1073807360, false, 0f) || (float)this.currentPath.Count > seekDist + 1f || (this.currentPath.Count > 0 && magnitude > seekDist + 1.414f))
			{
				this.FollowPlannedPath(this.drone.SpeedFlying, pointRadius, false, 0f);
			}
			else if (magnitude >= seekDist)
			{
				this.drone.RotateTo(normalized);
				this.drone.Move(chestPosition, pointRadius);
			}
		}
		if (!this.IsPositionBlocked(position, chestPosition, 1073807360, false, 0f) && magnitude <= seekDist)
		{
			this.drone.RotateTo(normalized);
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000C8E80 File Offset: 0x000C7080
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool DoFleeFromTargetEntity(EntityAlive avaliableTarget, float seekDist, float pointRadius, bool debugDraw = false, float duration = 0f)
	{
		Vector3 position = this.drone.position;
		avaliableTarget.getChestPosition();
		if (this.currentPath.Count == 0)
		{
			Vector3[] groupPositions = this.GetGroupPositions(avaliableTarget, (this.fleeDist > 0f) ? this.fleeDist : 10f, false, 0f);
			Vector3 vector = groupPositions[this.drone.rand.RandomRange(0, groupPositions.Length)];
			this.GetPath(vector, seekDist, pointRadius, debugDraw, duration);
			if (this.currentPath.Count > 0)
			{
				this.currentPathDest = this.currentPath[this.currentPath.Count - 1];
			}
			else
			{
				this.currentPathDest = vector;
			}
		}
		else
		{
			bool flag = !EntityDrone.IsPositionBlocked(position, this.currentPathDest, 1073807360, false, 0f);
			float magnitude = (this.currentPathDest - position).magnitude;
			if (magnitude > 5f || !flag)
			{
				this.FollowPlannedPath(this.drone.SpeedFlying, pointRadius, false, 0f);
			}
			else
			{
				this.drone.RotateTo((this.currentPathDest - position).normalized);
				this.drone.Move(this.currentPathDest, pointRadius);
			}
			if (magnitude <= seekDist)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040016CD RID: 5837
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityDrone drone;

	// Token: 0x040016CE RID: 5838
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<Vector3> currentPath = new List<Vector3>();

	// Token: 0x040016CF RID: 5839
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 currentPathTarget;

	// Token: 0x040016D0 RID: 5840
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 currentPathDest;

	// Token: 0x040016D1 RID: 5841
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool weaponDischarged;

	// Token: 0x040016D2 RID: 5842
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackEnterTime;

	// Token: 0x040016D3 RID: 5843
	[PublicizedFrom(EAccessModifier.Protected)]
	public float actionTime;

	// Token: 0x040016D4 RID: 5844
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackExitTime;

	// Token: 0x040016D5 RID: 5845
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackEnterTimer;

	// Token: 0x040016D6 RID: 5846
	[PublicizedFrom(EAccessModifier.Protected)]
	public float actionTimer;

	// Token: 0x040016D7 RID: 5847
	[PublicizedFrom(EAccessModifier.Protected)]
	public float attackExitTimer;

	// Token: 0x040016D8 RID: 5848
	[PublicizedFrom(EAccessModifier.Private)]
	public float fleeDist;

	// Token: 0x040016D9 RID: 5849
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone.PathTracker pathTracker = new EntityDrone.PathTracker();
}
