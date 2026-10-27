using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200150C RID: 5388
public class TrajectorySimulation
{
	// Token: 0x0600A8B5 RID: 43189 RVA: 0x003EC9D8 File Offset: 0x003EABD8
	public bool TryGetSolution(out Trajectory.AimTrajectory trajectory)
	{
		if (this.TrajectoryState == TrajectorySimulation.State.DoneWithSolution)
		{
			trajectory = this.SimTrajectories[this.SimTrajectoryIndex];
			return true;
		}
		trajectory = default(Trajectory.AimTrajectory);
		return false;
	}

	// Token: 0x0600A8B6 RID: 43190 RVA: 0x003ECA04 File Offset: 0x003EAC04
	public TrajectorySimulation(EntityAlive owner, EntityAlive target, float maxSimTime, TrajectorySimulation.ProjectileInfo projectile, float renderDuration, Action<TrajectorySimulation> onSimulationComplete)
	{
		this.TrajectoryState = TrajectorySimulation.State.ReadyToSimulate;
		this.Owner = owner;
		this.Target = target;
		this.MaxSimTime = maxSimTime;
		this.Projectile = projectile;
		this.RenderDuration = renderDuration;
		this.LaunchPosition = this.Owner.position;
		this.TargetPosition = this.Target.position;
		this.OnSimulationComplete = onSimulationComplete;
		this.CullPlane = new Plane(Vector3.up, this.TargetPosition - Vector3.up * this.Projectile.ProjectileTargetRadius);
	}

	// Token: 0x0600A8B7 RID: 43191 RVA: 0x003ECAAC File Offset: 0x003EACAC
	public bool IsValid()
	{
		if (this.TrajectoryState == TrajectorySimulation.State.Invalid)
		{
			return false;
		}
		if (!(this.Owner != null) || !(this.Target != null))
		{
			return false;
		}
		Vector3 position = this.Owner.transform.position;
		Vector3 position2 = this.Target.transform.position;
		float sqrMagnitude = (position - this.LaunchPosition).sqrMagnitude;
		float sqrMagnitude2 = (position2 - this.TargetPosition).sqrMagnitude;
		if (sqrMagnitude > 16f)
		{
			this.TrajectoryState = TrajectorySimulation.State.Invalid;
			return false;
		}
		if (sqrMagnitude2 > 16f)
		{
			this.TrajectoryState = TrajectorySimulation.State.Invalid;
			return false;
		}
		return true;
	}

	// Token: 0x0600A8B8 RID: 43192 RVA: 0x003ECB50 File Offset: 0x003EAD50
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickSimulation(int maxIterations = 20)
	{
		if (this.Owner == null || this.Target == null)
		{
			this.TrajectoryState = TrajectorySimulation.State.Invalid;
			this.OnSimulationComplete(this);
			return;
		}
		if (this.TrajectoryState == TrajectorySimulation.State.ReadyToSimulate)
		{
			this.LaunchPosition = this.Owner.getHeadPosition();
			this.TargetPosition = this.Target.position;
			this.SimTrajectoryIndex = 0;
			this.SimTrajectoryCount = Trajectory.Calculate(this.LaunchPosition, this.TargetPosition, this.Projectile.ProjectileSpeed, this.Projectile.ProjectileGravity, this.SimTrajectories);
			if (this.SimTrajectoryCount == 0)
			{
				this.TrajectoryState = TrajectorySimulation.State.DoneWithoutSolution;
				this.OnSimulationComplete(this);
				return;
			}
			this.SimTime = 0f;
			this.SimVelocity = this.SimTrajectories[this.SimTrajectoryIndex].LaunchVelocity;
			this.SimCurrentPos = (this.SimLastPos = this.Owner.getHeadPosition());
			this.TrajectoryState = TrajectorySimulation.State.Simulating;
		}
		if (this.SimTrajectoryIndex >= this.SimTrajectoryCount)
		{
			this.TrajectoryState = TrajectorySimulation.State.DoneWithoutSolution;
			this.OnSimulationComplete(this);
			return;
		}
		if (this.TrajectoryState != TrajectorySimulation.State.Simulating)
		{
			return;
		}
		World world = GameManager.Instance.World;
		while (this.SimTime < this.MaxSimTime)
		{
			if (maxIterations <= 0)
			{
				return;
			}
			if (this.CullPlane.GetDistanceToPoint(this.SimCurrentPos) < 0f)
			{
				break;
			}
			if (this.SimVelocity.magnitude < 1f)
			{
				if (this.RenderDuration > 0f)
				{
					Utils.DrawBounds(new Bounds(this.SimCurrentPos, Vector3.one * this.Projectile.ProjectileRadius), Color.magenta, this.RenderDuration);
					break;
				}
				break;
			}
			else
			{
				Vector3 vector = this.SimVelocity * 0.05f;
				this.SimCurrentPos += vector;
				this.SimVelocity.y = this.SimVelocity.y + this.Projectile.ProjectileGravity * 0.05f;
				if (this.RenderDuration > 0f)
				{
					Debug.DrawLine(this.SimLastPos, this.SimCurrentPos, Color.green, this.RenderDuration);
				}
				float num;
				Vector3 vector2 = vector.NormalizeReturnMagnitude(out num);
				Ray ray = new Ray(this.SimCurrentPos, vector2);
				if (Voxel.Raycast(world, ray, num, 1082195968, 8191, this.Projectile.ProjectileRadius))
				{
					if (Voxel.phyxRaycastHit.distance == 0f)
					{
						if (this.RenderDuration > 0f)
						{
							Utils.DrawBounds(new Bounds(Voxel.phyxRaycastHit.point, Vector3.one * this.Projectile.ProjectileRadius), Color.red, this.RenderDuration);
							break;
						}
						break;
					}
					else
					{
						this.SimCurrentPos += ray.GetPoint(Voxel.phyxRaycastHit.distance);
						this.SimVelocity = Vector3.Reflect(this.SimVelocity, Voxel.phyxRaycastHit.normal);
						this.SimVelocity *= 0.25f;
						float num2 = num - Voxel.phyxRaycastHit.distance;
						if (num2 > 0f)
						{
							this.SimCurrentPos = Voxel.phyxRaycastHit.point + Vector3.Reflect(vector2, Voxel.phyxRaycastHit.normal) * num2;
						}
						if (this.RenderDuration > 0f)
						{
							Debug.DrawRay(Voxel.phyxRaycastHit.point, Voxel.phyxRaycastHit.normal * 0.2f, Color.yellow, this.RenderDuration);
						}
						if (Vector3.Dot(Voxel.phyxRaycastHit.normal, Vector3.up) > 0.707f)
						{
							break;
						}
					}
				}
				this.SimLastPos = this.SimCurrentPos;
				this.SimTime += 0.05f;
				maxIterations--;
			}
		}
		if (MathUtils.DistanceToSegment(this.SimCurrentPos, this.TargetPosition, this.Target.getChestPosition()) < this.Projectile.ProjectileTargetRadius)
		{
			this.TrajectoryState = TrajectorySimulation.State.DoneWithSolution;
			if (this.RenderDuration > 0f)
			{
				Utils.DrawBounds(new Bounds(this.SimCurrentPos, new Vector3(1f, 0f, 1f) * this.Projectile.ProjectileRadius), Color.blue, this.RenderDuration);
			}
			this.OnSimulationComplete(this);
			return;
		}
		this.SimTrajectoryIndex++;
		if (this.SimTrajectoryIndex < this.SimTrajectoryCount)
		{
			this.SimTime = 0f;
			this.SimVelocity = this.SimTrajectories[this.SimTrajectoryIndex].LaunchVelocity;
			this.SimCurrentPos = (this.SimLastPos = this.Owner.getHeadPosition());
			this.TrajectoryState = TrajectorySimulation.State.Simulating;
			return;
		}
		this.TrajectoryState = TrajectorySimulation.State.DoneWithoutSolution;
		this.OnSimulationComplete(this);
	}

	// Token: 0x0600A8B9 RID: 43193 RVA: 0x003ED037 File Offset: 0x003EB237
	public static void Cleanup()
	{
		TrajectorySimulation.Trajectories.Clear();
	}

	// Token: 0x0600A8BA RID: 43194 RVA: 0x003ED043 File Offset: 0x003EB243
	public static void Queue(TrajectorySimulation trajectory)
	{
		if (!TrajectorySimulation.Trajectories.Contains(trajectory))
		{
			TrajectorySimulation.Trajectories.Enqueue(trajectory);
		}
		trajectory.TrajectoryState = TrajectorySimulation.State.ReadyToSimulate;
	}

	// Token: 0x0600A8BB RID: 43195 RVA: 0x003ED064 File Offset: 0x003EB264
	public static void UpdateSimulationQueue()
	{
		TrajectorySimulation trajectorySimulation;
		if (TrajectorySimulation.Trajectories.TryPeek(out trajectorySimulation))
		{
			trajectorySimulation.TickSimulation(20);
			if (trajectorySimulation.TrajectoryState != TrajectorySimulation.State.Simulating)
			{
				TrajectorySimulation.Trajectories.Dequeue();
			}
		}
	}

	// Token: 0x04007D05 RID: 32005
	public TrajectorySimulation.State TrajectoryState;

	// Token: 0x04007D06 RID: 32006
	public readonly EntityAlive Owner;

	// Token: 0x04007D07 RID: 32007
	public readonly EntityAlive Target;

	// Token: 0x04007D08 RID: 32008
	public readonly float MaxSimTime;

	// Token: 0x04007D09 RID: 32009
	public readonly TrajectorySimulation.ProjectileInfo Projectile;

	// Token: 0x04007D0A RID: 32010
	public readonly float RenderDuration;

	// Token: 0x04007D0B RID: 32011
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<TrajectorySimulation> OnSimulationComplete;

	// Token: 0x04007D0C RID: 32012
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LaunchPosition;

	// Token: 0x04007D0D RID: 32013
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 TargetPosition;

	// Token: 0x04007D0E RID: 32014
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Trajectory.AimTrajectory[] SimTrajectories = new Trajectory.AimTrajectory[2];

	// Token: 0x04007D0F RID: 32015
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryIndex;

	// Token: 0x04007D10 RID: 32016
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryCount;

	// Token: 0x04007D11 RID: 32017
	[PublicizedFrom(EAccessModifier.Private)]
	public float SimTime;

	// Token: 0x04007D12 RID: 32018
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimVelocity;

	// Token: 0x04007D13 RID: 32019
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimLastPos;

	// Token: 0x04007D14 RID: 32020
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimCurrentPos;

	// Token: 0x04007D15 RID: 32021
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane CullPlane;

	// Token: 0x04007D16 RID: 32022
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxOwnerDelta = 16f;

	// Token: 0x04007D17 RID: 32023
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxTargetDelta = 16f;

	// Token: 0x04007D18 RID: 32024
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSimInterval = 0.05f;

	// Token: 0x04007D19 RID: 32025
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Queue<TrajectorySimulation> Trajectories = new Queue<TrajectorySimulation>();

	// Token: 0x0200150D RID: 5389
	public enum State
	{
		// Token: 0x04007D1B RID: 32027
		Init,
		// Token: 0x04007D1C RID: 32028
		Invalid,
		// Token: 0x04007D1D RID: 32029
		ReadyToSimulate,
		// Token: 0x04007D1E RID: 32030
		Simulating,
		// Token: 0x04007D1F RID: 32031
		DoneWithSolution,
		// Token: 0x04007D20 RID: 32032
		DoneWithoutSolution
	}

	// Token: 0x0200150E RID: 5390
	public struct ProjectileInfo
	{
		// Token: 0x04007D21 RID: 32033
		public float ProjectileSpeed;

		// Token: 0x04007D22 RID: 32034
		public float ProjectileGravity;

		// Token: 0x04007D23 RID: 32035
		public float ProjectileRadius;

		// Token: 0x04007D24 RID: 32036
		public float ProjectileTargetRadius;
	}
}
