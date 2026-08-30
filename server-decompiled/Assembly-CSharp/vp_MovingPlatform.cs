using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015CA RID: 5578
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class vp_MovingPlatform : MonoBehaviour
{
	// Token: 0x17001427 RID: 5159
	// (get) Token: 0x0600AE64 RID: 44644 RVA: 0x0040F5C1 File Offset: 0x0040D7C1
	public int TargetedWaypoint
	{
		get
		{
			return this.m_TargetedWayPoint;
		}
	}

	// Token: 0x0600AE65 RID: 44645 RVA: 0x0040F5CC File Offset: 0x0040D7CC
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

	// Token: 0x0600AE66 RID: 44646 RVA: 0x0040F7B8 File Offset: 0x0040D9B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void FixedUpdate()
	{
		this.UpdatePath();
		this.UpdateMovement();
		this.UpdateRotation();
		this.UpdateVelocity();
	}

	// Token: 0x0600AE67 RID: 44647 RVA: 0x0040F7D4 File Offset: 0x0040D9D4
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

	// Token: 0x0600AE68 RID: 44648 RVA: 0x0040F8D2 File Offset: 0x0040DAD2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnStart()
	{
		if (this.SoundStart != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStart);
		}
	}

	// Token: 0x0600AE69 RID: 44649 RVA: 0x0040F8F3 File Offset: 0x0040DAF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnArriveAtWaypoint()
	{
		if (this.SoundWaypoint != null)
		{
			this.m_Audio.PlayOneShot(this.SoundWaypoint);
		}
	}

	// Token: 0x0600AE6A RID: 44650 RVA: 0x0040F914 File Offset: 0x0040DB14
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

	// Token: 0x0600AE6B RID: 44651 RVA: 0x0040F950 File Offset: 0x0040DB50
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

	// Token: 0x0600AE6C RID: 44652 RVA: 0x0040F9CC File Offset: 0x0040DBCC
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

	// Token: 0x0600AE6D RID: 44653 RVA: 0x0040FB78 File Offset: 0x0040DD78
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

	// Token: 0x0600AE6E RID: 44654 RVA: 0x0040FDAC File Offset: 0x0040DFAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateVelocity()
	{
		this.m_MoveTime += this.MoveSpeed * 0.01f * vp_TimeUtility.AdjustedTimeScale;
		this.m_PhysicsCurrentMoveVelocity = (this.m_Transform.position - this.m_PrevPos).magnitude;
		this.m_PhysicsCurrentRotationVelocity = (this.m_Transform.eulerAngles - this.m_PrevAngle).magnitude;
		this.m_PrevPos = this.m_Transform.position;
		this.m_PrevAngle = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600AE6F RID: 44655 RVA: 0x0040FE44 File Offset: 0x0040E044
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

	// Token: 0x0600AE70 RID: 44656 RVA: 0x0040FEA8 File Offset: 0x0040E0A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public float GetDistanceLeft()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return 0f;
		}
		return Vector3.Distance(this.m_Transform.position, this.m_Waypoints[this.m_NextWaypoint].position);
	}

	// Token: 0x0600AE71 RID: 44657 RVA: 0x0040FEE4 File Offset: 0x0040E0E4
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

	// Token: 0x0600AE72 RID: 44658 RVA: 0x0040FFD9 File Offset: 0x0040E1D9
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

	// Token: 0x0600AE73 RID: 44659 RVA: 0x00410000 File Offset: 0x0040E200
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

	// Token: 0x0600AE74 RID: 44660 RVA: 0x00410018 File Offset: 0x0040E218
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

	// Token: 0x0600AE75 RID: 44661 RVA: 0x00410034 File Offset: 0x0040E234
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

	// Token: 0x0600AE76 RID: 44662 RVA: 0x004100A8 File Offset: 0x0040E2A8
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

	// Token: 0x0600AE77 RID: 44663 RVA: 0x004101A8 File Offset: 0x0040E3A8
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

	// Token: 0x0600AE78 RID: 44664 RVA: 0x0041038F File Offset: 0x0040E58F
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

	// Token: 0x040081D4 RID: 33236
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081D5 RID: 33237
	public vp_MovingPlatform.PathMoveType PathType;

	// Token: 0x040081D6 RID: 33238
	public GameObject PathWaypoints;

	// Token: 0x040081D7 RID: 33239
	public vp_MovingPlatform.Direction PathDirection;

	// Token: 0x040081D8 RID: 33240
	public int MoveAutoStartTarget = 1000;

	// Token: 0x040081D9 RID: 33241
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Waypoints = new List<Transform>();

	// Token: 0x040081DA RID: 33242
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_NextWaypoint;

	// Token: 0x040081DB RID: 33243
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetPosition = Vector3.zero;

	// Token: 0x040081DC RID: 33244
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetAngle = Vector3.zero;

	// Token: 0x040081DD RID: 33245
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_TargetedWayPoint;

	// Token: 0x040081DE RID: 33246
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TravelDistance;

	// Token: 0x040081DF RID: 33247
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalAngle = Vector3.zero;

	// Token: 0x040081E0 RID: 33248
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWaypoint;

	// Token: 0x040081E1 RID: 33249
	public float MoveSpeed = 0.1f;

	// Token: 0x040081E2 RID: 33250
	public float MoveReturnDelay;

	// Token: 0x040081E3 RID: 33251
	public float MoveCooldown;

	// Token: 0x040081E4 RID: 33252
	public vp_MovingPlatform.MovementInterpolationMode MoveInterpolationMode;

	// Token: 0x040081E5 RID: 33253
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Moving;

	// Token: 0x040081E6 RID: 33254
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedMoveTime;

	// Token: 0x040081E7 RID: 33255
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MoveTime;

	// Token: 0x040081E8 RID: 33256
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ReturnDelayTimer = new vp_Timer.Handle();

	// Token: 0x040081E9 RID: 33257
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x040081EA RID: 33258
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_EaseInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x040081EB RID: 33259
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040081EC RID: 33260
	public float RotationEaseAmount = 0.1f;

	// Token: 0x040081ED RID: 33261
	public Vector3 RotationSpeed = Vector3.zero;

	// Token: 0x040081EE RID: 33262
	public vp_MovingPlatform.RotateInterpolationMode RotationInterpolationMode;

	// Token: 0x040081EF RID: 33263
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevAngle = Vector3.zero;

	// Token: 0x040081F0 RID: 33264
	public AudioClip SoundStart;

	// Token: 0x040081F1 RID: 33265
	public AudioClip SoundStop;

	// Token: 0x040081F2 RID: 33266
	public AudioClip SoundMove;

	// Token: 0x040081F3 RID: 33267
	public AudioClip SoundWaypoint;

	// Token: 0x040081F4 RID: 33268
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081F5 RID: 33269
	public bool PhysicsSnapPlayerToTopOnIntersect = true;

	// Token: 0x040081F6 RID: 33270
	public float m_PhysicsPushForce = 2f;

	// Token: 0x040081F7 RID: 33271
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_RigidBody;

	// Token: 0x040081F8 RID: 33272
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x040081F9 RID: 33273
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_PlayerCollider;

	// Token: 0x040081FA RID: 33274
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_PlayerToPush;

	// Token: 0x040081FB RID: 33275
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentMoveVelocity;

	// Token: 0x040081FC RID: 33276
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentRotationVelocity;

	// Token: 0x040081FD RID: 33277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_PlayerEventHandler> m_KnownPlayers = new Dictionary<Collider, vp_PlayerEventHandler>();

	// Token: 0x020015CB RID: 5579
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WaypointComparer : IComparer
	{
		// Token: 0x0600AE7B RID: 44667 RVA: 0x004104AC File Offset: 0x0040E6AC
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((Transform)x).name, ((Transform)y).name);
		}
	}

	// Token: 0x020015CC RID: 5580
	public enum PathMoveType
	{
		// Token: 0x040081FF RID: 33279
		PingPong,
		// Token: 0x04008200 RID: 33280
		Loop,
		// Token: 0x04008201 RID: 33281
		Target
	}

	// Token: 0x020015CD RID: 5581
	public enum Direction
	{
		// Token: 0x04008203 RID: 33283
		Forward,
		// Token: 0x04008204 RID: 33284
		Backwards,
		// Token: 0x04008205 RID: 33285
		Direct
	}

	// Token: 0x020015CE RID: 5582
	public enum MovementInterpolationMode
	{
		// Token: 0x04008207 RID: 33287
		EaseInOut,
		// Token: 0x04008208 RID: 33288
		EaseIn,
		// Token: 0x04008209 RID: 33289
		EaseOut,
		// Token: 0x0400820A RID: 33290
		EaseOut2,
		// Token: 0x0400820B RID: 33291
		Slerp,
		// Token: 0x0400820C RID: 33292
		Lerp
	}

	// Token: 0x020015CF RID: 5583
	public enum RotateInterpolationMode
	{
		// Token: 0x0400820E RID: 33294
		SyncToMovement,
		// Token: 0x0400820F RID: 33295
		EaseOut,
		// Token: 0x04008210 RID: 33296
		CustomEaseOut,
		// Token: 0x04008211 RID: 33297
		CustomRotate
	}
}
