using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015FE RID: 5630
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class vp_MovingPlatform : MonoBehaviour
{
	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x0600AF6F RID: 44911 RVA: 0x00416819 File Offset: 0x00414A19
	public int TargetedWaypoint
	{
		get
		{
			return this.m_TargetedWayPoint;
		}
	}

	// Token: 0x0600AF70 RID: 44912 RVA: 0x00416824 File Offset: 0x00414A24
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

	// Token: 0x0600AF71 RID: 44913 RVA: 0x00416A10 File Offset: 0x00414C10
	[PublicizedFrom(EAccessModifier.Private)]
	public void FixedUpdate()
	{
		this.UpdatePath();
		this.UpdateMovement();
		this.UpdateRotation();
		this.UpdateVelocity();
	}

	// Token: 0x0600AF72 RID: 44914 RVA: 0x00416A2C File Offset: 0x00414C2C
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

	// Token: 0x0600AF73 RID: 44915 RVA: 0x00416B2A File Offset: 0x00414D2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnStart()
	{
		if (this.SoundStart != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStart);
		}
	}

	// Token: 0x0600AF74 RID: 44916 RVA: 0x00416B4B File Offset: 0x00414D4B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnArriveAtWaypoint()
	{
		if (this.SoundWaypoint != null)
		{
			this.m_Audio.PlayOneShot(this.SoundWaypoint);
		}
	}

	// Token: 0x0600AF75 RID: 44917 RVA: 0x00416B6C File Offset: 0x00414D6C
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

	// Token: 0x0600AF76 RID: 44918 RVA: 0x00416BA8 File Offset: 0x00414DA8
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

	// Token: 0x0600AF77 RID: 44919 RVA: 0x00416C24 File Offset: 0x00414E24
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

	// Token: 0x0600AF78 RID: 44920 RVA: 0x00416DD0 File Offset: 0x00414FD0
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

	// Token: 0x0600AF79 RID: 44921 RVA: 0x00417004 File Offset: 0x00415204
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateVelocity()
	{
		this.m_MoveTime += this.MoveSpeed * 0.01f * vp_TimeUtility.AdjustedTimeScale;
		this.m_PhysicsCurrentMoveVelocity = (this.m_Transform.position - this.m_PrevPos).magnitude;
		this.m_PhysicsCurrentRotationVelocity = (this.m_Transform.eulerAngles - this.m_PrevAngle).magnitude;
		this.m_PrevPos = this.m_Transform.position;
		this.m_PrevAngle = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600AF7A RID: 44922 RVA: 0x0041709C File Offset: 0x0041529C
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

	// Token: 0x0600AF7B RID: 44923 RVA: 0x00417100 File Offset: 0x00415300
	[PublicizedFrom(EAccessModifier.Protected)]
	public float GetDistanceLeft()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return 0f;
		}
		return Vector3.Distance(this.m_Transform.position, this.m_Waypoints[this.m_NextWaypoint].position);
	}

	// Token: 0x0600AF7C RID: 44924 RVA: 0x0041713C File Offset: 0x0041533C
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

	// Token: 0x0600AF7D RID: 44925 RVA: 0x00417231 File Offset: 0x00415431
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

	// Token: 0x0600AF7E RID: 44926 RVA: 0x00417258 File Offset: 0x00415458
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

	// Token: 0x0600AF7F RID: 44927 RVA: 0x00417270 File Offset: 0x00415470
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

	// Token: 0x0600AF80 RID: 44928 RVA: 0x0041728C File Offset: 0x0041548C
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

	// Token: 0x0600AF81 RID: 44929 RVA: 0x00417300 File Offset: 0x00415500
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

	// Token: 0x0600AF82 RID: 44930 RVA: 0x00417400 File Offset: 0x00415600
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

	// Token: 0x0600AF83 RID: 44931 RVA: 0x004175E7 File Offset: 0x004157E7
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

	// Token: 0x04008298 RID: 33432
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008299 RID: 33433
	public vp_MovingPlatform.PathMoveType PathType;

	// Token: 0x0400829A RID: 33434
	public GameObject PathWaypoints;

	// Token: 0x0400829B RID: 33435
	public vp_MovingPlatform.Direction PathDirection;

	// Token: 0x0400829C RID: 33436
	public int MoveAutoStartTarget = 1000;

	// Token: 0x0400829D RID: 33437
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Waypoints = new List<Transform>();

	// Token: 0x0400829E RID: 33438
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_NextWaypoint;

	// Token: 0x0400829F RID: 33439
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetPosition = Vector3.zero;

	// Token: 0x040082A0 RID: 33440
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetAngle = Vector3.zero;

	// Token: 0x040082A1 RID: 33441
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_TargetedWayPoint;

	// Token: 0x040082A2 RID: 33442
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TravelDistance;

	// Token: 0x040082A3 RID: 33443
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalAngle = Vector3.zero;

	// Token: 0x040082A4 RID: 33444
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWaypoint;

	// Token: 0x040082A5 RID: 33445
	public float MoveSpeed = 0.1f;

	// Token: 0x040082A6 RID: 33446
	public float MoveReturnDelay;

	// Token: 0x040082A7 RID: 33447
	public float MoveCooldown;

	// Token: 0x040082A8 RID: 33448
	public vp_MovingPlatform.MovementInterpolationMode MoveInterpolationMode;

	// Token: 0x040082A9 RID: 33449
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Moving;

	// Token: 0x040082AA RID: 33450
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedMoveTime;

	// Token: 0x040082AB RID: 33451
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MoveTime;

	// Token: 0x040082AC RID: 33452
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ReturnDelayTimer = new vp_Timer.Handle();

	// Token: 0x040082AD RID: 33453
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x040082AE RID: 33454
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_EaseInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x040082AF RID: 33455
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040082B0 RID: 33456
	public float RotationEaseAmount = 0.1f;

	// Token: 0x040082B1 RID: 33457
	public Vector3 RotationSpeed = Vector3.zero;

	// Token: 0x040082B2 RID: 33458
	public vp_MovingPlatform.RotateInterpolationMode RotationInterpolationMode;

	// Token: 0x040082B3 RID: 33459
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevAngle = Vector3.zero;

	// Token: 0x040082B4 RID: 33460
	public AudioClip SoundStart;

	// Token: 0x040082B5 RID: 33461
	public AudioClip SoundStop;

	// Token: 0x040082B6 RID: 33462
	public AudioClip SoundMove;

	// Token: 0x040082B7 RID: 33463
	public AudioClip SoundWaypoint;

	// Token: 0x040082B8 RID: 33464
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040082B9 RID: 33465
	public bool PhysicsSnapPlayerToTopOnIntersect = true;

	// Token: 0x040082BA RID: 33466
	public float m_PhysicsPushForce = 2f;

	// Token: 0x040082BB RID: 33467
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_RigidBody;

	// Token: 0x040082BC RID: 33468
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x040082BD RID: 33469
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_PlayerCollider;

	// Token: 0x040082BE RID: 33470
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_PlayerToPush;

	// Token: 0x040082BF RID: 33471
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentMoveVelocity;

	// Token: 0x040082C0 RID: 33472
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentRotationVelocity;

	// Token: 0x040082C1 RID: 33473
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_PlayerEventHandler> m_KnownPlayers = new Dictionary<Collider, vp_PlayerEventHandler>();

	// Token: 0x020015FF RID: 5631
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WaypointComparer : IComparer
	{
		// Token: 0x0600AF86 RID: 44934 RVA: 0x00417704 File Offset: 0x00415904
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((Transform)x).name, ((Transform)y).name);
		}
	}

	// Token: 0x02001600 RID: 5632
	public enum PathMoveType
	{
		// Token: 0x040082C3 RID: 33475
		PingPong,
		// Token: 0x040082C4 RID: 33476
		Loop,
		// Token: 0x040082C5 RID: 33477
		Target
	}

	// Token: 0x02001601 RID: 5633
	public enum Direction
	{
		// Token: 0x040082C7 RID: 33479
		Forward,
		// Token: 0x040082C8 RID: 33480
		Backwards,
		// Token: 0x040082C9 RID: 33481
		Direct
	}

	// Token: 0x02001602 RID: 5634
	public enum MovementInterpolationMode
	{
		// Token: 0x040082CB RID: 33483
		EaseInOut,
		// Token: 0x040082CC RID: 33484
		EaseIn,
		// Token: 0x040082CD RID: 33485
		EaseOut,
		// Token: 0x040082CE RID: 33486
		EaseOut2,
		// Token: 0x040082CF RID: 33487
		Slerp,
		// Token: 0x040082D0 RID: 33488
		Lerp
	}

	// Token: 0x02001603 RID: 5635
	public enum RotateInterpolationMode
	{
		// Token: 0x040082D2 RID: 33490
		SyncToMovement,
		// Token: 0x040082D3 RID: 33491
		EaseOut,
		// Token: 0x040082D4 RID: 33492
		CustomEaseOut,
		// Token: 0x040082D5 RID: 33493
		CustomRotate
	}
}
