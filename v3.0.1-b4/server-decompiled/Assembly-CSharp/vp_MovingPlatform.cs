using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015AF RID: 5551
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class vp_MovingPlatform : MonoBehaviour
{
	// Token: 0x170013EE RID: 5102
	// (get) Token: 0x0600AD67 RID: 44391 RVA: 0x0040A281 File Offset: 0x00408481
	public int TargetedWaypoint
	{
		get
		{
			return this.m_TargetedWayPoint;
		}
	}

	// Token: 0x0600AD68 RID: 44392 RVA: 0x0040A28C File Offset: 0x0040848C
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

	// Token: 0x0600AD69 RID: 44393 RVA: 0x0040A478 File Offset: 0x00408678
	[PublicizedFrom(EAccessModifier.Private)]
	public void FixedUpdate()
	{
		this.UpdatePath();
		this.UpdateMovement();
		this.UpdateRotation();
		this.UpdateVelocity();
	}

	// Token: 0x0600AD6A RID: 44394 RVA: 0x0040A494 File Offset: 0x00408694
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

	// Token: 0x0600AD6B RID: 44395 RVA: 0x0040A592 File Offset: 0x00408792
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnStart()
	{
		if (this.SoundStart != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStart);
		}
	}

	// Token: 0x0600AD6C RID: 44396 RVA: 0x0040A5B3 File Offset: 0x004087B3
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnArriveAtWaypoint()
	{
		if (this.SoundWaypoint != null)
		{
			this.m_Audio.PlayOneShot(this.SoundWaypoint);
		}
	}

	// Token: 0x0600AD6D RID: 44397 RVA: 0x0040A5D4 File Offset: 0x004087D4
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

	// Token: 0x0600AD6E RID: 44398 RVA: 0x0040A610 File Offset: 0x00408810
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

	// Token: 0x0600AD6F RID: 44399 RVA: 0x0040A68C File Offset: 0x0040888C
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

	// Token: 0x0600AD70 RID: 44400 RVA: 0x0040A838 File Offset: 0x00408A38
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

	// Token: 0x0600AD71 RID: 44401 RVA: 0x0040AA6C File Offset: 0x00408C6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpdateVelocity()
	{
		this.m_MoveTime += this.MoveSpeed * 0.01f * vp_TimeUtility.AdjustedTimeScale;
		this.m_PhysicsCurrentMoveVelocity = (this.m_Transform.position - this.m_PrevPos).magnitude;
		this.m_PhysicsCurrentRotationVelocity = (this.m_Transform.eulerAngles - this.m_PrevAngle).magnitude;
		this.m_PrevPos = this.m_Transform.position;
		this.m_PrevAngle = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600AD72 RID: 44402 RVA: 0x0040AB04 File Offset: 0x00408D04
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

	// Token: 0x0600AD73 RID: 44403 RVA: 0x0040AB68 File Offset: 0x00408D68
	[PublicizedFrom(EAccessModifier.Protected)]
	public float GetDistanceLeft()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return 0f;
		}
		return Vector3.Distance(this.m_Transform.position, this.m_Waypoints[this.m_NextWaypoint].position);
	}

	// Token: 0x0600AD74 RID: 44404 RVA: 0x0040ABA4 File Offset: 0x00408DA4
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

	// Token: 0x0600AD75 RID: 44405 RVA: 0x0040AC99 File Offset: 0x00408E99
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

	// Token: 0x0600AD76 RID: 44406 RVA: 0x0040ACC0 File Offset: 0x00408EC0
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

	// Token: 0x0600AD77 RID: 44407 RVA: 0x0040ACD8 File Offset: 0x00408ED8
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

	// Token: 0x0600AD78 RID: 44408 RVA: 0x0040ACF4 File Offset: 0x00408EF4
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

	// Token: 0x0600AD79 RID: 44409 RVA: 0x0040AD68 File Offset: 0x00408F68
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

	// Token: 0x0600AD7A RID: 44410 RVA: 0x0040AE68 File Offset: 0x00409068
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

	// Token: 0x0600AD7B RID: 44411 RVA: 0x0040B04F File Offset: 0x0040924F
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

	// Token: 0x040080FB RID: 33019
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040080FC RID: 33020
	public vp_MovingPlatform.PathMoveType PathType;

	// Token: 0x040080FD RID: 33021
	public GameObject PathWaypoints;

	// Token: 0x040080FE RID: 33022
	public vp_MovingPlatform.Direction PathDirection;

	// Token: 0x040080FF RID: 33023
	public int MoveAutoStartTarget = 1000;

	// Token: 0x04008100 RID: 33024
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Transform> m_Waypoints = new List<Transform>();

	// Token: 0x04008101 RID: 33025
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_NextWaypoint;

	// Token: 0x04008102 RID: 33026
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetPosition = Vector3.zero;

	// Token: 0x04008103 RID: 33027
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentTargetAngle = Vector3.zero;

	// Token: 0x04008104 RID: 33028
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_TargetedWayPoint;

	// Token: 0x04008105 RID: 33029
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_TravelDistance;

	// Token: 0x04008106 RID: 33030
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_OriginalAngle = Vector3.zero;

	// Token: 0x04008107 RID: 33031
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CurrentWaypoint;

	// Token: 0x04008108 RID: 33032
	public float MoveSpeed = 0.1f;

	// Token: 0x04008109 RID: 33033
	public float MoveReturnDelay;

	// Token: 0x0400810A RID: 33034
	public float MoveCooldown;

	// Token: 0x0400810B RID: 33035
	public vp_MovingPlatform.MovementInterpolationMode MoveInterpolationMode;

	// Token: 0x0400810C RID: 33036
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Moving;

	// Token: 0x0400810D RID: 33037
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_NextAllowedMoveTime;

	// Token: 0x0400810E RID: 33038
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_MoveTime;

	// Token: 0x0400810F RID: 33039
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ReturnDelayTimer = new vp_Timer.Handle();

	// Token: 0x04008110 RID: 33040
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x04008111 RID: 33041
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_EaseInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04008112 RID: 33042
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04008113 RID: 33043
	public float RotationEaseAmount = 0.1f;

	// Token: 0x04008114 RID: 33044
	public Vector3 RotationSpeed = Vector3.zero;

	// Token: 0x04008115 RID: 33045
	public vp_MovingPlatform.RotateInterpolationMode RotationInterpolationMode;

	// Token: 0x04008116 RID: 33046
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_PrevAngle = Vector3.zero;

	// Token: 0x04008117 RID: 33047
	public AudioClip SoundStart;

	// Token: 0x04008118 RID: 33048
	public AudioClip SoundStop;

	// Token: 0x04008119 RID: 33049
	public AudioClip SoundMove;

	// Token: 0x0400811A RID: 33050
	public AudioClip SoundWaypoint;

	// Token: 0x0400811B RID: 33051
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400811C RID: 33052
	public bool PhysicsSnapPlayerToTopOnIntersect = true;

	// Token: 0x0400811D RID: 33053
	public float m_PhysicsPushForce = 2f;

	// Token: 0x0400811E RID: 33054
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_RigidBody;

	// Token: 0x0400811F RID: 33055
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_Collider;

	// Token: 0x04008120 RID: 33056
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_PlayerCollider;

	// Token: 0x04008121 RID: 33057
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_PlayerToPush;

	// Token: 0x04008122 RID: 33058
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentMoveVelocity;

	// Token: 0x04008123 RID: 33059
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_PhysicsCurrentRotationVelocity;

	// Token: 0x04008124 RID: 33060
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_PlayerEventHandler> m_KnownPlayers = new Dictionary<Collider, vp_PlayerEventHandler>();

	// Token: 0x020015B0 RID: 5552
	[PublicizedFrom(EAccessModifier.Protected)]
	public class WaypointComparer : IComparer
	{
		// Token: 0x0600AD7E RID: 44414 RVA: 0x0040B16C File Offset: 0x0040936C
		[PublicizedFrom(EAccessModifier.Private)]
		public int System.Collections.IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((Transform)x).name, ((Transform)y).name);
		}
	}

	// Token: 0x020015B1 RID: 5553
	public enum PathMoveType
	{
		// Token: 0x04008126 RID: 33062
		PingPong,
		// Token: 0x04008127 RID: 33063
		Loop,
		// Token: 0x04008128 RID: 33064
		Target
	}

	// Token: 0x020015B2 RID: 5554
	public enum Direction
	{
		// Token: 0x0400812A RID: 33066
		Forward,
		// Token: 0x0400812B RID: 33067
		Backwards,
		// Token: 0x0400812C RID: 33068
		Direct
	}

	// Token: 0x020015B3 RID: 5555
	public enum MovementInterpolationMode
	{
		// Token: 0x0400812E RID: 33070
		EaseInOut,
		// Token: 0x0400812F RID: 33071
		EaseIn,
		// Token: 0x04008130 RID: 33072
		EaseOut,
		// Token: 0x04008131 RID: 33073
		EaseOut2,
		// Token: 0x04008132 RID: 33074
		Slerp,
		// Token: 0x04008133 RID: 33075
		Lerp
	}

	// Token: 0x020015B4 RID: 5556
	public enum RotateInterpolationMode
	{
		// Token: 0x04008135 RID: 33077
		SyncToMovement,
		// Token: 0x04008136 RID: 33078
		EaseOut,
		// Token: 0x04008137 RID: 33079
		CustomEaseOut,
		// Token: 0x04008138 RID: 33080
		CustomRotate
	}
}
