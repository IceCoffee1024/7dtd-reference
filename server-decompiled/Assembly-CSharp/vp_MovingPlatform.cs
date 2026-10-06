using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015EC RID: 5612
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class vp_MovingPlatform : MonoBehaviour
{
	// Token: 0x1700146C RID: 5228
	// (get) Token: 0x0600AF17 RID: 44823 RVA: 0x00414415 File Offset: 0x00412615
	public int TargetedWaypoint
	{
		get
		{
			return this.m_TargetedWayPoint;
		}
	}

	// Token: 0x0600AF18 RID: 44824 RVA: 0x00414420 File Offset: 0x00412620
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Transform = base.transform;
		this.m_Collider = base.GetComponentInChildren<Collider>();
		this.m_RigidBody = base.GetComponent<Rigidbody>();
		this.m_RigidBody.useGravity = false;
		this.m_RigidBody.isKinematic = true;
		this.m_NextWaypoint = 0;
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Audio.loop = true;
		this.m_Audio.clip = this.SoundMove;
		if (this.PathWaypoints == null)
		{
			return;
		}
		base.gameObject.layer = 28;
		foreach (object obj in this.PathWaypoints.transform)
		{
			Transform transform = (Transform)obj;
			if (vp_Utility.IsActive(transform.gameObject))
			{
				this.m_Waypoints.Add(transform);
				transform.gameObject.layer = 28;
			}
			if (transform.GetComponent<Renderer>() != null)
			{
				transform.GetComponent<Renderer>().enabled = false;
			}
			if (transform.GetComponent<Collider>() != null)
			{
				transform.GetComponent<Collider>().enabled = false;
			}
		}
		IComparer comparer = new vp_MovingPlatform.WaypointComparer();
		this.m_Waypoints.Sort(new Comparison<Transform>(comparer.Compare));
		if (this.m_Waypoints.Count > 0)
		{
			this.m_CurrentTargetPosition = this.m_Waypoints[this.m_NextWaypoint].position;
			this.m_CurrentTargetAngle = this.m_Waypoints[this.m_NextWaypoint].eulerAngles;
			this.m_Transform.position = this.m_CurrentTargetPosition;
			this.m_Transform.eulerAngles = this.m_CurrentTargetAngle;
			if (this.MoveAutoStartTarget > this.m_Waypoints.Count - 1)
			{
				this.MoveAutoStartTarget = this.m_Waypoints.Count - 1;
			}
		}
	}

	// Token: 0x0600AF19 RID: 44825 RVA: 0x0041460C File Offset: 0x0041280C
	[PublicizedFrom(EAccessModifier.Private)]
	public void FixedUpdate()
	{
		this.UpdatePath();
		this.UpdateMovement();
		this.UpdateRotation();
		this.UpdateVelocity();
	}

	// Token: 0x0600AF1A RID: 44826 RVA: 0x00414628 File Offset: 0x00412828
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdatePath()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return;
		}
		if (this.GetDistanceLeft() < 0.01f && Time.time >= this.m_NextAllowedMoveTime)
		{
			switch (this.PathType)
			{
			case vp_MovingPlatform.PathMoveType.PingPong:
				if (this.PathDirection == vp_MovingPlatform.Direction.Backwards)
				{
					if (this.m_NextWaypoint == 0)
					{
						this.PathDirection = vp_MovingPlatform.Direction.Forward;
					}
				}
				else if (this.m_NextWaypoint == this.m_Waypoints.Count - 1)
				{
					this.PathDirection = vp_MovingPlatform.Direction.Backwards;
				}
				this.OnArriveAtWaypoint();
				this.GoToNextWaypoint();
				break;
			case vp_MovingPlatform.PathMoveType.Loop:
				this.OnArriveAtWaypoint();
				this.GoToNextWaypoint();
				return;
			case vp_MovingPlatform.PathMoveType.Target:
				if (this.m_NextWaypoint != this.m_TargetedWayPoint)
				{
					if (this.m_Moving)
					{
						if (this.m_PhysicsCurrentMoveVelocity == 0f)
						{
							this.OnStart();
						}
						else
						{
							this.OnArriveAtWaypoint();
						}
					}
					this.GoToNextWaypoint();
					return;
				}
				if (this.m_Moving)
				{
					this.OnStop();
					return;
				}
				if (this.m_NextWaypoint != 0)
				{
					this.OnArriveAtDestination();
					return;
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600AF1B RID: 44827 RVA: 0x00414726 File Offset: 0x00412926
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnStart()
	{
		if (this.SoundStart != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStart);
		}
	}

	// Token: 0x0600AF1C RID: 44828 RVA: 0x00414747 File Offset: 0x00412947
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnArriveAtWaypoint()
	{
		if (this.SoundWaypoint != null)
		{
			this.m_Audio.PlayOneShot(this.SoundWaypoint);
		}
	}

	// Token: 0x0600AF1D RID: 44829 RVA: 0x00414768 File Offset: 0x00412968
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnArriveAtDestination()
	{
		if (this.MoveReturnDelay > 0f && !this.m_ReturnDelayTimer.Active)
		{
			vp_Timer.In(this.MoveReturnDelay, delegate
			{
				this.GoTo(0);
			}, this.m_ReturnDelayTimer);
		}
	}

	// Token: 0x0600AF1E RID: 44830 RVA: 0x004147A4 File Offset: 0x004129A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnStop()
	{
		this.m_Audio.Stop();
		if (this.SoundStop != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStop);
		}
		this.m_Transform.position = this.m_CurrentTargetPosition;
		this.m_Transform.eulerAngles = this.m_CurrentTargetAngle;
		this.m_Moving = false;
		if (this.m_NextWaypoint == 0)
		{
			this.m_NextAllowedMoveTime = Time.time + this.MoveCooldown;
		}
	}

	// Token: 0x0600AF1F RID: 44831 RVA: 0x00414820 File Offset: 0x00412A20
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateMovement()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return;
		}
		switch (this.MoveInterpolationMode)
		{
		case vp_MovingPlatform.MovementInterpolationMode.EaseInOut:
			this.m_Transform.position = vp_MathUtility.NaNSafeVector3(Vector3.Lerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_EaseInOutCurve.Evaluate(this.m_MoveTime)), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.EaseIn:
			this.m_Transform.position = vp_MathUtility.NaNSafeVector3(Vector3.MoveTowards(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_MoveTime), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.EaseOut:
			this.m_Transform.position = vp_MathUtility.NaNSafeVector3(Vector3.Lerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_LinearCurve.Evaluate(this.m_MoveTime)), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.EaseOut2:
			this.m_Transform.position = vp_MathUtility.NaNSafeVector3(Vector3.Lerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.MoveSpeed * 0.25f), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.Slerp:
			this.m_Transform.position = vp_MathUtility.NaNSafeVector3(Vector3.Slerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_LinearCurve.Evaluate(this.m_MoveTime)), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.Lerp:
			this.m_Transform.position = vp_MathUtility.NaNSafeVector3(Vector3.MoveTowards(this.m_Transform.position, this.m_CurrentTargetPosition, this.MoveSpeed), default(Vector3));
			return;
		default:
			return;
		}
	}

	// Token: 0x0600AF20 RID: 44832 RVA: 0x004149CC File Offset: 0x00412BCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateRotation()
	{
		switch (this.RotationInterpolationMode)
		{
		case vp_MovingPlatform.RotateInterpolationMode.SyncToMovement:
			if (this.m_Moving)
			{
				this.m_Transform.eulerAngles = vp_MathUtility.NaNSafeVector3(new Vector3(Mathf.LerpAngle(this.m_OriginalAngle.x, this.m_CurrentTargetAngle.x, 1f - this.GetDistanceLeft() / this.m_TravelDistance), Mathf.LerpAngle(this.m_OriginalAngle.y, this.m_CurrentTargetAngle.y, 1f - this.GetDistanceLeft() / this.m_TravelDistance), Mathf.LerpAngle(this.m_OriginalAngle.z, this.m_CurrentTargetAngle.z, 1f - this.GetDistanceLeft() / this.m_TravelDistance)), default(Vector3));
				return;
			}
			break;
		case vp_MovingPlatform.RotateInterpolationMode.EaseOut:
			this.m_Transform.eulerAngles = vp_MathUtility.NaNSafeVector3(new Vector3(Mathf.LerpAngle(this.m_Transform.eulerAngles.x, this.m_CurrentTargetAngle.x, this.m_LinearCurve.Evaluate(this.m_MoveTime)), Mathf.LerpAngle(this.m_Transform.eulerAngles.y, this.m_CurrentTargetAngle.y, this.m_LinearCurve.Evaluate(this.m_MoveTime)), Mathf.LerpAngle(this.m_Transform.eulerAngles.z, this.m_CurrentTargetAngle.z, this.m_LinearCurve.Evaluate(this.m_MoveTime))), default(Vector3));
			return;
		case vp_MovingPlatform.RotateInterpolationMode.CustomEaseOut:
			this.m_Transform.eulerAngles = vp_MathUtility.NaNSafeVector3(new Vector3(Mathf.LerpAngle(this.m_Transform.eulerAngles.x, this.m_CurrentTargetAngle.x, this.RotationEaseAmount), Mathf.LerpAngle(this.m_Transform.eulerAngles.y, this.m_CurrentTargetAngle.y, this.RotationEaseAmount), Mathf.LerpAngle(this.m_Transform.eulerAngles.z, this.m_CurrentTargetAngle.z, this.RotationEaseAmount)), default(Vector3));
			return;
		case vp_MovingPlatform.RotateInterpolationMode.CustomRotate:
			this.m_Transform.Rotate(this.RotationSpeed);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600AF21 RID: 44833 RVA: 0x00414C00 File Offset: 0x00412E00
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateVelocity()
	{
		this.m_MoveTime += this.MoveSpeed * 0.01f * vp_TimeUtility.AdjustedTimeScale;
		this.m_PhysicsCurrentMoveVelocity = (this.m_Transform.position - this.m_PrevPos).magnitude;
		this.m_PhysicsCurrentRotationVelocity = (this.m_Transform.eulerAngles - this.m_PrevAngle).magnitude;
		this.m_PrevPos = this.m_Transform.position;
		this.m_PrevAngle = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600AF22 RID: 44834 RVA: 0x00414C98 File Offset: 0x00412E98
	public void GoTo(int targetWayPoint)
	{
		if (Time.time < this.m_NextAllowedMoveTime)
		{
			return;
		}
		if (this.PathType != vp_MovingPlatform.PathMoveType.Target)
		{
			return;
		}
		this.m_TargetedWayPoint = this.GetValidWaypoint(targetWayPoint);
		if (targetWayPoint > this.m_NextWaypoint)
		{
			if (this.PathDirection != vp_MovingPlatform.Direction.Direct)
			{
				this.PathDirection = vp_MovingPlatform.Direction.Forward;
			}
		}
		else if (this.PathDirection != vp_MovingPlatform.Direction.Direct)
		{
			this.PathDirection = vp_MovingPlatform.Direction.Backwards;
		}
		this.m_Moving = true;
	}

	// Token: 0x0600AF23 RID: 44835 RVA: 0x00414CFC File Offset: 0x00412EFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public float GetDistanceLeft()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return 0f;
		}
		return Vector3.Distance(this.m_Transform.position, this.m_Waypoints[this.m_NextWaypoint].position);
	}

	// Token: 0x0600AF24 RID: 44836 RVA: 0x00414D38 File Offset: 0x00412F38
	[PublicizedFrom(EAccessModifier.Protected)]
	public void GoToNextWaypoint()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return;
		}
		this.m_MoveTime = 0f;
		if (!this.m_Audio.isPlaying)
		{
			this.m_Audio.Play();
		}
		this.m_CurrentWaypoint = this.m_NextWaypoint;
		switch (this.PathDirection)
		{
		case vp_MovingPlatform.Direction.Forward:
			this.m_NextWaypoint = this.GetValidWaypoint(this.m_NextWaypoint + 1);
			break;
		case vp_MovingPlatform.Direction.Backwards:
			this.m_NextWaypoint = this.GetValidWaypoint(this.m_NextWaypoint - 1);
			break;
		case vp_MovingPlatform.Direction.Direct:
			this.m_NextWaypoint = this.m_TargetedWayPoint;
			break;
		}
		this.m_OriginalAngle = this.m_CurrentTargetAngle;
		this.m_CurrentTargetPosition = this.m_Waypoints[this.m_NextWaypoint].position;
		this.m_CurrentTargetAngle = this.m_Waypoints[this.m_NextWaypoint].eulerAngles;
		this.m_TravelDistance = this.GetDistanceLeft();
		this.m_Moving = true;
	}

	// Token: 0x0600AF25 RID: 44837 RVA: 0x00414E2D File Offset: 0x0041302D
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetValidWaypoint(int wayPoint)
	{
		if (wayPoint < 0)
		{
			return this.m_Waypoints.Count - 1;
		}
		if (wayPoint > this.m_Waypoints.Count - 1)
		{
			return 0;
		}
		return wayPoint;
	}

	// Token: 0x0600AF26 RID: 44838 RVA: 0x00414E54 File Offset: 0x00413054
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnTriggerEnter(Collider col)
	{
		if (!this.GetPlayer(col))
		{
			return;
		}
		this.TryPushPlayer();
		this.TryAutoStart();
	}

	// Token: 0x0600AF27 RID: 44839 RVA: 0x00414E6C File Offset: 0x0041306C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnTriggerStay(Collider col)
	{
		if (!this.PhysicsSnapPlayerToTopOnIntersect)
		{
			return;
		}
		if (!this.GetPlayer(col))
		{
			return;
		}
		this.TrySnapPlayerToTop();
	}

	// Token: 0x0600AF28 RID: 44840 RVA: 0x00414E88 File Offset: 0x00413088
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool GetPlayer(Collider col)
	{
		if (!this.m_KnownPlayers.ContainsKey(col))
		{
			if (col.gameObject.layer != 30)
			{
				return false;
			}
			vp_PlayerEventHandler component = col.transform.root.GetComponent<vp_PlayerEventHandler>();
			if (component == null)
			{
				return false;
			}
			this.m_KnownPlayers.Add(col, component);
		}
		if (!this.m_KnownPlayers.TryGetValue(col, out this.m_PlayerToPush))
		{
			return false;
		}
		this.m_PlayerCollider = col;
		return true;
	}

	// Token: 0x0600AF29 RID: 44841 RVA: 0x00414EFC File Offset: 0x004130FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TryPushPlayer()
	{
		if (this.m_PlayerToPush == null || this.m_PlayerToPush.Platform == null)
		{
			return;
		}
		if (this.m_PlayerToPush.Position.Get().y > this.m_Collider.bounds.max.y)
		{
			return;
		}
		if (this.m_PlayerToPush.Platform.Get() == this.m_Transform)
		{
			return;
		}
		float num = this.m_PhysicsCurrentMoveVelocity;
		if (num == 0f)
		{
			num = this.m_PhysicsCurrentRotationVelocity * 0.1f;
		}
		if (num > 0f)
		{
			this.m_PlayerToPush.ForceImpact.Send(vp_3DUtility.HorizontalVector(-(this.m_Transform.position - this.m_PlayerCollider.bounds.center).normalized * num * this.m_PhysicsPushForce));
		}
	}

	// Token: 0x0600AF2A RID: 44842 RVA: 0x00414FFC File Offset: 0x004131FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TrySnapPlayerToTop()
	{
		if (this.m_PlayerToPush == null || this.m_PlayerToPush.Platform == null)
		{
			return;
		}
		if (this.m_PlayerToPush.Position.Get().y > this.m_Collider.bounds.max.y)
		{
			return;
		}
		if (this.m_PlayerToPush.Platform.Get() == this.m_Transform)
		{
			return;
		}
		if (this.RotationSpeed.x != 0f || this.RotationSpeed.z != 0f || this.m_CurrentTargetAngle.x != 0f || this.m_CurrentTargetAngle.z != 0f)
		{
			return;
		}
		if (this.m_Collider.bounds.max.x < this.m_PlayerCollider.bounds.max.x || this.m_Collider.bounds.max.z < this.m_PlayerCollider.bounds.max.z || this.m_Collider.bounds.min.x > this.m_PlayerCollider.bounds.min.x || this.m_Collider.bounds.min.z > this.m_PlayerCollider.bounds.min.z)
		{
			return;
		}
		Vector3 vector = this.m_PlayerToPush.Position.Get();
		vector.y = this.m_Collider.bounds.max.y - 0.1f;
		this.m_PlayerToPush.Position.Set(vector);
	}

	// Token: 0x0600AF2B RID: 44843 RVA: 0x004151E3 File Offset: 0x004133E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TryAutoStart()
	{
		if (this.MoveAutoStartTarget == 0)
		{
			return;
		}
		if (this.m_PhysicsCurrentMoveVelocity > 0f || this.m_Moving)
		{
			return;
		}
		this.GoTo(this.MoveAutoStartTarget);
	}

	// Token: 0x04008257 RID: 33367
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008258 RID: 33368
	public vp_MovingPlatform.PathMoveType PathType;

	// Token: 0x04008259 RID: 33369
	public GameObject PathWaypoints;

	// Token: 0x0400825A RID: 33370
	public vp_MovingPlatform.Direction PathDirection;

	// Token: 0x0400825B RID: 33371
	public int MoveAutoStartTarget = 1000;

	// Token: 0x0400825C RID: 33372
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Waypoints = new List<Transform>();

	// Token: 0x0400825D RID: 33373
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_NextWaypoint;

	// Token: 0x0400825E RID: 33374
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetPosition = Vector3.zero;

	// Token: 0x0400825F RID: 33375
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetAngle = Vector3.zero;

	// Token: 0x04008260 RID: 33376
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_TargetedWayPoint;

	// Token: 0x04008261 RID: 33377
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TravelDistance;

	// Token: 0x04008262 RID: 33378
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalAngle = Vector3.zero;

	// Token: 0x04008263 RID: 33379
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWaypoint;

	// Token: 0x04008264 RID: 33380
	public float MoveSpeed = 0.1f;

	// Token: 0x04008265 RID: 33381
	public float MoveReturnDelay;

	// Token: 0x04008266 RID: 33382
	public float MoveCooldown;

	// Token: 0x04008267 RID: 33383
	public vp_MovingPlatform.MovementInterpolationMode MoveInterpolationMode;

	// Token: 0x04008268 RID: 33384
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Moving;

	// Token: 0x04008269 RID: 33385
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedMoveTime;

	// Token: 0x0400826A RID: 33386
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MoveTime;

	// Token: 0x0400826B RID: 33387
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ReturnDelayTimer = new vp_Timer.Handle();

	// Token: 0x0400826C RID: 33388
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x0400826D RID: 33389
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_EaseInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x0400826E RID: 33390
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400826F RID: 33391
	public float RotationEaseAmount = 0.1f;

	// Token: 0x04008270 RID: 33392
	public Vector3 RotationSpeed = Vector3.zero;

	// Token: 0x04008271 RID: 33393
	public vp_MovingPlatform.RotateInterpolationMode RotationInterpolationMode;

	// Token: 0x04008272 RID: 33394
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevAngle = Vector3.zero;

	// Token: 0x04008273 RID: 33395
	public AudioClip SoundStart;

	// Token: 0x04008274 RID: 33396
	public AudioClip SoundStop;

	// Token: 0x04008275 RID: 33397
	public AudioClip SoundMove;

	// Token: 0x04008276 RID: 33398
	public AudioClip SoundWaypoint;

	// Token: 0x04008277 RID: 33399
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008278 RID: 33400
	public bool PhysicsSnapPlayerToTopOnIntersect = true;

	// Token: 0x04008279 RID: 33401
	public float m_PhysicsPushForce = 2f;

	// Token: 0x0400827A RID: 33402
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_RigidBody;

	// Token: 0x0400827B RID: 33403
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x0400827C RID: 33404
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_PlayerCollider;

	// Token: 0x0400827D RID: 33405
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_PlayerToPush;

	// Token: 0x0400827E RID: 33406
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentMoveVelocity;

	// Token: 0x0400827F RID: 33407
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentRotationVelocity;

	// Token: 0x04008280 RID: 33408
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_PlayerEventHandler> m_KnownPlayers = new Dictionary<Collider, vp_PlayerEventHandler>();

	// Token: 0x020015ED RID: 5613
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WaypointComparer : IComparer
	{
		// Token: 0x0600AF2E RID: 44846 RVA: 0x00415300 File Offset: 0x00413500
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((Transform)x).name, ((Transform)y).name);
		}
	}

	// Token: 0x020015EE RID: 5614
	public enum PathMoveType
	{
		// Token: 0x04008282 RID: 33410
		PingPong,
		// Token: 0x04008283 RID: 33411
		Loop,
		// Token: 0x04008284 RID: 33412
		Target
	}

	// Token: 0x020015EF RID: 5615
	public enum Direction
	{
		// Token: 0x04008286 RID: 33414
		Forward,
		// Token: 0x04008287 RID: 33415
		Backwards,
		// Token: 0x04008288 RID: 33416
		Direct
	}

	// Token: 0x020015F0 RID: 5616
	public enum MovementInterpolationMode
	{
		// Token: 0x0400828A RID: 33418
		EaseInOut,
		// Token: 0x0400828B RID: 33419
		EaseIn,
		// Token: 0x0400828C RID: 33420
		EaseOut,
		// Token: 0x0400828D RID: 33421
		EaseOut2,
		// Token: 0x0400828E RID: 33422
		Slerp,
		// Token: 0x0400828F RID: 33423
		Lerp
	}

	// Token: 0x020015F1 RID: 5617
	public enum RotateInterpolationMode
	{
		// Token: 0x04008291 RID: 33425
		SyncToMovement,
		// Token: 0x04008292 RID: 33426
		EaseOut,
		// Token: 0x04008293 RID: 33427
		CustomEaseOut,
		// Token: 0x04008294 RID: 33428
		CustomRotate
	}
}
