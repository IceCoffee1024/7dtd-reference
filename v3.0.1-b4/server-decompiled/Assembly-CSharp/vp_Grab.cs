using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015C4 RID: 5572
[RequireComponent(typeof(Rigidbody))]
public class vp_Grab : vp_Interactable
{
	// Token: 0x0600ADD2 RID: 44498 RVA: 0x0040D118 File Offset: 0x0040B318
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (!base.GetComponent<Rigidbody>() || !base.GetComponent<Collider>())
		{
			base.enabled = false;
		}
		if (base.GetComponent<Rigidbody>() != null)
		{
			this.m_DefaultGravity = base.GetComponent<Rigidbody>().useGravity;
			this.m_DefaultDrag = this.m_Transform.GetComponent<Rigidbody>().drag;
			this.m_DefaultAngularDrag = this.m_Transform.GetComponent<Rigidbody>().angularDrag;
		}
		this.InteractType = vp_Interactable.vp_InteractType.Normal;
		this.m_InteractManager = global::UnityEngine.Object.FindAnyObjectByType<vp_FPInteractManager>();
	}

	// Token: 0x0600ADD3 RID: 44499 RVA: 0x0040D1A9 File Offset: 0x0040B3A9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		if (!this.m_IsGrabbed || this.m_Transform.parent == null)
		{
			return;
		}
		this.UpdateShake();
		this.UpdatePosition();
		this.UpdateRotation();
		this.UpdateBurden();
		this.DampenForces();
	}

	// Token: 0x0600ADD4 RID: 44500 RVA: 0x0040D1E5 File Offset: 0x0040B3E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (!this.m_IsGrabbed || this.m_Transform.parent == null)
		{
			return;
		}
		this.UpdateInput();
	}

	// Token: 0x0600ADD5 RID: 44501 RVA: 0x0040D20C File Offset: 0x0040B40C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateInput()
	{
		this.m_CurrentMouseMove.x = this.m_Player.InputRawLook.Get().x * Time.timeScale;
		this.m_CurrentMouseMove.y = this.m_Player.InputRawLook.Get().y * Time.timeScale;
		if (this.m_Player.InputGetButtonDown.Send("Attack"))
		{
			this.m_Player.Interact.TryStart(true);
			return;
		}
		if (this.m_Player.CurrentWeaponIndex.Get() != 0)
		{
			this.m_Player.SetWeapon.TryStart<int>(0);
			return;
		}
	}

	// Token: 0x0600ADD6 RID: 44502 RVA: 0x0040D2C8 File Offset: 0x0040B4C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateShake()
	{
		this.m_CurrentShake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
		this.m_Transform.localEulerAngles += this.m_CurrentShake;
	}

	// Token: 0x0600ADD7 RID: 44503 RVA: 0x0040D304 File Offset: 0x0040B504
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePosition()
	{
		this.m_CurrentSwayForce += this.m_Player.Velocity.Get() * 0.005f;
		this.m_CurrentSwayForce.y = this.m_CurrentSwayForce.y + this.m_CurrentFootstepForce;
		this.m_CurrentSwayForce += this.m_Camera.Transform.TransformDirection(new Vector3(this.m_CurrentMouseMove.x * 0.05f, (this.m_Player.Rotation.Get().x > this.m_Camera.RotationPitchLimit.y) ? (this.m_CurrentMouseMove.y * 0.015f) : (this.m_CurrentMouseMove.y * 0.05f), 0f));
		this.m_TempCarryingOffset = (this.m_Player.IsFirstPerson.Get() ? this.CarryingOffset : (this.CarryingOffset - this.m_Camera.Position3rdPersonOffset));
		this.m_Transform.position = Vector3.Lerp(this.m_Transform.position, this.m_Camera.Transform.position - this.m_CurrentSwayForce + this.m_Camera.Transform.right * this.m_TempCarryingOffset.x + this.m_Camera.Transform.up * this.m_Transform.localScale.y * (this.m_TempCarryingOffset.y + this.m_CurrentShake.y * 0.5f) + this.m_Camera.Transform.forward * this.m_TempCarryingOffset.z, (this.m_FetchProgress < 1f) ? this.m_FetchProgress : (Time.deltaTime * (this.Stiffness * 60f)));
	}

	// Token: 0x0600ADD8 RID: 44504 RVA: 0x0040D510 File Offset: 0x0040B710
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateRotation()
	{
		this.m_Camera.RotationPitchLimit = Vector2.Lerp(this.m_Camera.RotationPitchLimit, new Vector2(this.m_Camera.RotationPitchLimit.x, this.CameraPitchDownLimit), this.m_FetchProgress);
		if (this.m_DisableAngleSwayTimer.Active)
		{
			return;
		}
		this.m_CurrentSwayTorque += this.m_Player.Velocity.Get() * 0.005f;
		this.m_CurrentRotationSway = this.m_Camera.Transform.InverseTransformDirection(this.m_CurrentSwayTorque * 1.5f);
		this.m_CurrentRotationSway.y = this.m_CurrentRotationSway.z;
		this.m_CurrentRotationSway.z = this.m_CurrentRotationSway.x;
		this.m_CurrentRotationSway.x = -this.m_CurrentRotationSway.y * 0.5f;
		this.m_CurrentRotationSway.y = 0f;
		Quaternion localRotation = this.m_Transform.localRotation;
		this.m_Transform.Rotate(this.m_Camera.transform.forward, this.m_CurrentRotationSway.z * -0.5f * Time.timeScale);
		this.m_Transform.Rotate(this.m_Camera.transform.right, this.m_CurrentRotationSway.x * -0.5f * Time.timeScale);
		Quaternion localRotation2 = this.m_Transform.localRotation;
		this.m_Transform.localRotation = localRotation;
		this.m_Transform.localRotation = Quaternion.Slerp(localRotation2, Quaternion.Euler(this.m_CurrentHoldAngle + this.m_CurrentShake * 50f), Time.deltaTime * (this.Stiffness * 60f));
	}

	// Token: 0x0600ADD9 RID: 44505 RVA: 0x0040D6E4 File Offset: 0x0040B8E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBurden()
	{
		if (this.Burden <= 0f)
		{
			return;
		}
		this.m_Player.MotorThrottle.Set(this.m_Player.MotorThrottle.Get() * (1f - Mathf.Clamp01(this.Burden)));
	}

	// Token: 0x0600ADDA RID: 44506 RVA: 0x0040D73F File Offset: 0x0040B93F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void DampenForces()
	{
		this.m_CurrentSwayForce *= 0.9f;
		this.m_CurrentSwayTorque *= 0.9f;
		this.m_CurrentFootstepForce *= 0.9f;
	}

	// Token: 0x0600ADDB RID: 44507 RVA: 0x0040D780 File Offset: 0x0040B980
	public override bool TryInteract(vp_FPPlayerEventHandler player)
	{
		if (this.m_Player == null)
		{
			this.m_Player = player;
		}
		if (player == null)
		{
			return false;
		}
		if (this.m_Controller == null)
		{
			this.m_Controller = this.m_Player.GetComponent<vp_FPController>();
		}
		if (this.m_Controller == null)
		{
			return false;
		}
		if (this.m_Camera == null)
		{
			this.m_Camera = this.m_Player.GetComponentInChildren<vp_FPCamera>();
		}
		if (this.m_Camera == null)
		{
			return false;
		}
		if (this.m_WeaponHandler == null)
		{
			this.m_WeaponHandler = this.m_Player.GetComponentInChildren<vp_WeaponHandler>();
		}
		if (this.m_Audio == null)
		{
			this.m_Audio = this.m_Player.GetComponent<AudioSource>();
		}
		this.m_Player.Register(this);
		if (!this.m_IsGrabbed)
		{
			this.StartGrab();
		}
		else
		{
			this.StopGrab();
		}
		this.m_Player.Interactable.Set(this);
		if (this.GrabStateCrosshair != null)
		{
			this.m_Player.Crosshair.Set(this.GrabStateCrosshair);
		}
		else
		{
			this.m_Player.Crosshair.Set(new Texture2D(0, 0));
		}
		return true;
	}

	// Token: 0x0600ADDC RID: 44508 RVA: 0x0040D8CC File Offset: 0x0040BACC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void StartGrab()
	{
		vp_AudioUtility.PlayRandomSound(this.m_Audio, this.GrabSounds, this.SoundsPitch);
		if (!string.IsNullOrEmpty(this.OnGrabText))
		{
			this.m_Player.HUDText.Send(this.OnGrabText);
		}
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Combine(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
		this.m_LastWeaponEquipped = this.m_Player.CurrentWeaponIndex.Get();
		this.m_OriginalPitchDownLimit = this.m_Camera.RotationPitchLimit.y;
		this.m_FetchProgress = 0f;
		if (this.m_LastWeaponEquipped != 0)
		{
			this.m_Player.SetWeapon.TryStart<int>(0);
		}
		else if (!this.m_IsFetching)
		{
			base.StartCoroutine("Fetch");
		}
		if (this.m_Transform.GetComponent<Rigidbody>() != null)
		{
			this.m_Transform.GetComponent<Rigidbody>().useGravity = false;
			this.m_Transform.GetComponent<Rigidbody>().drag = this.Stiffness * 60f;
			this.m_Transform.GetComponent<Rigidbody>().angularDrag = this.Stiffness * 60f;
		}
		if (this.m_Controller.Transform.GetComponent<Collider>().enabled && this.m_Transform.GetComponent<Collider>().enabled)
		{
			Physics.IgnoreCollision(this.m_Controller.Transform.GetComponent<Collider>(), this.m_Transform.GetComponent<Collider>(), true);
		}
		this.m_Transform.parent = this.m_Camera.Transform;
		this.m_CurrentHoldAngle = this.m_Transform.localEulerAngles;
		this.m_IsGrabbed = true;
	}

	// Token: 0x0600ADDD RID: 44509 RVA: 0x0040DA80 File Offset: 0x0040BC80
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void StopGrab()
	{
		this.m_IsGrabbed = false;
		this.m_FetchProgress = 1f;
		vp_FPCamera camera = this.m_Camera;
		camera.BobStepCallback = (vp_FPCamera.BobStepDelegate)Delegate.Remove(camera.BobStepCallback, new vp_FPCamera.BobStepDelegate(this.Footstep));
		this.m_Player.SetWeapon.TryStart<int>(this.m_LastWeaponEquipped);
		if (this.m_Transform.GetComponent<Rigidbody>() != null)
		{
			this.m_Transform.GetComponent<Rigidbody>().useGravity = this.m_DefaultGravity;
			this.m_Transform.GetComponent<Rigidbody>().drag = this.m_DefaultDrag;
			this.m_Transform.GetComponent<Rigidbody>().angularDrag = this.m_DefaultAngularDrag;
		}
		if (!this.m_Player.Dead.Active && vp_Utility.IsActive(this.m_Transform.gameObject) && this.m_Controller.Transform.GetComponent<Collider>().enabled && this.m_Transform.GetComponent<Collider>().enabled)
		{
			Physics.IgnoreCollision(this.m_Controller.Transform.GetComponent<Collider>(), this.m_Transform.GetComponent<Collider>(), false);
		}
		Vector3 eulerAngles = this.m_Transform.eulerAngles;
		this.m_Transform.parent = null;
		this.m_Transform.eulerAngles = eulerAngles;
		if (this.m_Transform.GetComponent<Rigidbody>() != null)
		{
			this.m_Transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.m_Transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		if (this.m_Player.InputGetButtonDown.Send("Attack"))
		{
			vp_AudioUtility.PlayRandomSound(this.m_Audio, this.ThrowSounds, this.SoundsPitch);
			if (this.m_Transform.GetComponent<Rigidbody>() != null)
			{
				this.m_Transform.GetComponent<Rigidbody>().AddForce(this.m_Player.Velocity.Get() + this.m_Player.CameraLookDirection.Get() * this.ThrowStrength, ForceMode.Impulse);
				if (this.AllowThrowRotation)
				{
					this.m_Transform.GetComponent<Rigidbody>().AddTorque(this.m_Camera.Transform.forward * ((global::UnityEngine.Random.value > 0.5f) ? 0.5f : (-0.5f)) + this.m_Camera.Transform.right * ((global::UnityEngine.Random.value > 0.5f) ? 0.5f : (-0.5f)), ForceMode.Impulse);
				}
			}
		}
		else
		{
			vp_AudioUtility.PlayRandomSound(this.m_Audio, this.DropSounds, this.SoundsPitch);
			if (this.m_Transform.GetComponent<Rigidbody>() != null)
			{
				this.m_Transform.GetComponent<Rigidbody>().AddForce(this.m_Player.Velocity.Get() + this.m_Player.CameraLookDirection.Get(), ForceMode.Impulse);
			}
		}
		if (this.m_InteractManager == null)
		{
			this.m_InteractManager = global::UnityEngine.Object.FindAnyObjectByType<vp_FPInteractManager>();
		}
		this.m_InteractManager.CrosshairTimeoutTimer = Time.time + 0.5f;
		vp_Timer.In(0.1f, delegate
		{
			this.m_Camera.RotationPitchLimit.y = this.m_OriginalPitchDownLimit;
		}, null);
	}

	// Token: 0x0600ADDE RID: 44510 RVA: 0x0040DDC6 File Offset: 0x0040BFC6
	public override void FinishInteraction()
	{
		if (this.m_IsGrabbed)
		{
			this.StopGrab();
		}
	}

	// Token: 0x0600ADDF RID: 44511 RVA: 0x0040DDD6 File Offset: 0x0040BFD6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator Fetch()
	{
		this.m_IsFetching = true;
		this.m_CurrentSwayForce = Vector3.zero;
		this.m_CurrentSwayTorque = Vector3.zero;
		this.m_CurrentFootstepForce = 0f;
		this.m_FetchProgress = 0f;
		this.duration = Vector3.Distance(this.m_Camera.Transform.position, this.m_Transform.position) * 0.5f;
		vp_Timer.In(this.duration + 1f, delegate
		{
		}, this.m_DisableAngleSwayTimer);
		while (this.m_FetchProgress < 1f)
		{
			this.m_FetchProgress += Time.deltaTime / this.duration;
			yield return new WaitForEndOfFrame();
		}
		this.m_IsFetching = false;
		yield break;
	}

	// Token: 0x0600ADE0 RID: 44512 RVA: 0x0040DDE8 File Offset: 0x0040BFE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCollisionEnter(Collision col)
	{
		if (!this.m_IsGrabbed)
		{
			return;
		}
		if (this.m_FetchProgress < 1f)
		{
			this.m_FetchProgress *= 1.2f;
		}
		vp_Timer.In(2f, delegate
		{
		}, this.m_DisableAngleSwayTimer);
	}

	// Token: 0x0600ADE1 RID: 44513 RVA: 0x0040DE4C File Offset: 0x0040C04C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCollisionStay(Collision col)
	{
		if (!this.m_IsGrabbed || this.MaxCollisionCount == 0)
		{
			return;
		}
		if (col.collider != this.m_LastExternalCollider)
		{
			if (!col.collider.GetComponent<Rigidbody>() || col.collider.GetComponent<Rigidbody>().isKinematic)
			{
				this.m_LastExternalCollider = col.collider;
				this.m_CollisionCount = 1;
				return;
			}
		}
		else
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.m_Transform.position, this.m_Camera.Transform.forward, out raycastHit, 1f) && raycastHit.collider == this.m_LastExternalCollider && this.m_FetchProgress >= 1f)
			{
				this.m_CollisionCount = this.MaxCollisionCount;
			}
			this.m_CollisionCount++;
			if (this.m_CollisionCount > this.MaxCollisionCount && (!Physics.Raycast(col.contacts[0].point + Vector3.up * 0.1f, -Vector3.up, out raycastHit, 0.2f) || !(raycastHit.collider == this.m_LastExternalCollider)))
			{
				this.m_CollisionCount = 0;
				this.m_FetchProgress = 1f;
				this.m_LastExternalCollider = null;
				if (this.m_Player != null)
				{
					this.m_Player.Interact.TryStart(true);
				}
			}
		}
	}

	// Token: 0x0600ADE2 RID: 44514 RVA: 0x0040DFB7 File Offset: 0x0040C1B7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnCollisionExit()
	{
		this.m_CurrentHoldAngle = this.m_Transform.localEulerAngles;
	}

	// Token: 0x0600ADE3 RID: 44515 RVA: 0x0040DFCA File Offset: 0x0040C1CA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Footstep()
	{
		this.m_CurrentFootstepForce += this.FootstepForce;
	}

	// Token: 0x0600ADE4 RID: 44516 RVA: 0x0040DFDF File Offset: 0x0040C1DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_FallImpact(float impact)
	{
		this.m_CurrentSwayForce.y = this.m_CurrentSwayForce.y + impact * this.Kneeling;
	}

	// Token: 0x0600ADE5 RID: 44517 RVA: 0x0040DFF8 File Offset: 0x0040C1F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		if (this.m_IsGrabbed && !this.m_IsFetching)
		{
			base.StartCoroutine("Fetch");
		}
	}

	// Token: 0x0600ADE6 RID: 44518 RVA: 0x0040E018 File Offset: 0x0040C218
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_SetWeapon()
	{
		int num = (int)this.m_Player.SetWeapon.Argument;
		return !this.m_IsGrabbed || num == 0;
	}

	// Token: 0x0400819A RID: 33178
	public string OnGrabText = "";

	// Token: 0x0400819B RID: 33179
	public Texture GrabStateCrosshair;

	// Token: 0x0400819C RID: 33180
	public float FootstepForce = 0.015f;

	// Token: 0x0400819D RID: 33181
	public float Kneeling = 5f;

	// Token: 0x0400819E RID: 33182
	public float Stiffness = 0.5f;

	// Token: 0x0400819F RID: 33183
	public float ShakeSpeed = 0.1f;

	// Token: 0x040081A0 RID: 33184
	public Vector3 ShakeAmplitude = Vector3.one;

	// Token: 0x040081A1 RID: 33185
	public float ThrowStrength = 6f;

	// Token: 0x040081A2 RID: 33186
	public bool AllowThrowRotation = true;

	// Token: 0x040081A3 RID: 33187
	public float Burden;

	// Token: 0x040081A4 RID: 33188
	public int MaxCollisionCount = 20;

	// Token: 0x040081A5 RID: 33189
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentShake = Vector3.zero;

	// Token: 0x040081A6 RID: 33190
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentRotationSway = Vector3.zero;

	// Token: 0x040081A7 RID: 33191
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_CurrentMouseMove;

	// Token: 0x040081A8 RID: 33192
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentSwayForce;

	// Token: 0x040081A9 RID: 33193
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentSwayTorque;

	// Token: 0x040081AA RID: 33194
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CurrentFootstepForce;

	// Token: 0x040081AB RID: 33195
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CurrentHoldAngle = Vector3.one;

	// Token: 0x040081AC RID: 33196
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Collider m_LastExternalCollider;

	// Token: 0x040081AD RID: 33197
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_CollisionCount;

	// Token: 0x040081AE RID: 33198
	public Vector3 CarryingOffset = new Vector3(0f, -0.5f, 1.5f);

	// Token: 0x040081AF RID: 33199
	public float CameraPitchDownLimit;

	// Token: 0x040081B0 RID: 33200
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_TempCarryingOffset = Vector3.zero;

	// Token: 0x040081B1 RID: 33201
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsFetching;

	// Token: 0x040081B2 RID: 33202
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float duration;

	// Token: 0x040081B3 RID: 33203
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FetchProgress;

	// Token: 0x040081B4 RID: 33204
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPInteractManager m_InteractManager;

	// Token: 0x040081B5 RID: 33205
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081B6 RID: 33206
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_LastWeaponEquipped;

	// Token: 0x040081B7 RID: 33207
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsGrabbed;

	// Token: 0x040081B8 RID: 33208
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalPitchDownLimit;

	// Token: 0x040081B9 RID: 33209
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_DefaultGravity;

	// Token: 0x040081BA RID: 33210
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DefaultDrag;

	// Token: 0x040081BB RID: 33211
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_DefaultAngularDrag;

	// Token: 0x040081BC RID: 33212
	public Vector2 SoundsPitch = new Vector2(1f, 1.5f);

	// Token: 0x040081BD RID: 33213
	public List<AudioClip> GrabSounds = new List<AudioClip>();

	// Token: 0x040081BE RID: 33214
	public List<AudioClip> DropSounds = new List<AudioClip>();

	// Token: 0x040081BF RID: 33215
	public List<AudioClip> ThrowSounds = new List<AudioClip>();

	// Token: 0x040081C0 RID: 33216
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_DisableAngleSwayTimer = new vp_Timer.Handle();
}
