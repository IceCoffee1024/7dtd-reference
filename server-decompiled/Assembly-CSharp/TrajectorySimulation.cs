using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014FA RID: 5370
public class TrajectorySimulation
{
	// Token: 0x0600A85D RID: 43101 RVA: 0x003EA5D4 File Offset: 0x003E87D4
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

	// Token: 0x0600A85E RID: 43102 RVA: 0x003EA600 File Offset: 0x003E8800
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

	// Token: 0x0600A85F RID: 43103 RVA: 0x003EA6A8 File Offset: 0x003E88A8
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

	// Token: 0x0600A860 RID: 43104 RVA: 0x003EA74C File Offset: 0x003E894C
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

	// Token: 0x0600A861 RID: 43105 RVA: 0x003EAC33 File Offset: 0x003E8E33
	public static void Cleanup()
	{
		TrajectorySimulation.Trajectories.Clear();
	}

	// Token: 0x0600A862 RID: 43106 RVA: 0x003EAC3F File Offset: 0x003E8E3F
	public static void Queue(TrajectorySimulation trajectory)
	{
		if (!TrajectorySimulation.Trajectories.Contains(trajectory))
		{
			TrajectorySimulation.Trajectories.Enqueue(trajectory);
		}
		trajectory.TrajectoryState = TrajectorySimulation.State.ReadyToSimulate;
	}

	// Token: 0x0600A863 RID: 43107 RVA: 0x003EAC60 File Offset: 0x003E8E60
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

	// Token: 0x04007CC4 RID: 31940
	public TrajectorySimulation.State TrajectoryState;

	// Token: 0x04007CC5 RID: 31941
	public readonly EntityAlive Owner;

	// Token: 0x04007CC6 RID: 31942
	public readonly EntityAlive Target;

	// Token: 0x04007CC7 RID: 31943
	public readonly float MaxSimTime;

	// Token: 0x04007CC8 RID: 31944
	public readonly TrajectorySimulation.ProjectileInfo Projectile;

	// Token: 0x04007CC9 RID: 31945
	public readonly float RenderDuration;

	// Token: 0x04007CCA RID: 31946
	[PublicizedFrom(EAccessModifier.Private)]
	public Action<TrajectorySimulation> OnSimulationComplete;

	// Token: 0x04007CCB RID: 31947
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LaunchPosition;

	// Token: 0x04007CCC RID: 31948
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 TargetPosition;

	// Token: 0x04007CCD RID: 31949
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Trajectory.AimTrajectory[] SimTrajectories = new Trajectory.AimTrajectory[2];

	// Token: 0x04007CCE RID: 31950
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryIndex;

	// Token: 0x04007CCF RID: 31951
	[PublicizedFrom(EAccessModifier.Private)]
	public int SimTrajectoryCount;

	// Token: 0x04007CD0 RID: 31952
	[PublicizedFrom(EAccessModifier.Private)]
	public float SimTime;

	// Token: 0x04007CD1 RID: 31953
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimVelocity;

	// Token: 0x04007CD2 RID: 31954
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimLastPos;

	// Token: 0x04007CD3 RID: 31955
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SimCurrentPos;

	// Token: 0x04007CD4 RID: 31956
	[PublicizedFrom(EAccessModifier.Private)]
	public Plane CullPlane;

	// Token: 0x04007CD5 RID: 31957
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxOwnerDelta = 16f;

	// Token: 0x04007CD6 RID: 31958
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxTargetDelta = 16f;

	// Token: 0x04007CD7 RID: 31959
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSimInterval = 0.05f;

	// Token: 0x04007CD8 RID: 31960
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Queue<TrajectorySimulation> Trajectories = new Queue<TrajectorySimulation>();

	// Token: 0x020014FB RID: 5371
	public enum State
	{
		// Token: 0x04007CDA RID: 31962
		Init,
		// Token: 0x04007CDB RID: 31963
		Invalid,
		// Token: 0x04007CDC RID: 31964
		ReadyToSimulate,
		// Token: 0x04007CDD RID: 31965
		Simulating,
		// Token: 0x04007CDE RID: 31966
		DoneWithSolution,
		// Token: 0x04007CDF RID: 31967
		DoneWithoutSolution
	}

	// Token: 0x020014FC RID: 5372
	public struct ProjectileInfo
	{
		// Token: 0x04007CE0 RID: 31968
		public float ProjectileSpeed;

		// Token: 0x04007CE1 RID: 31969
		public float ProjectileGravity;

		// Token: 0x04007CE2 RID: 31970
		public float ProjectileRadius;

		// Token: 0x04007CE3 RID: 31971
		public float ProjectileTargetRadius;
	}
}
