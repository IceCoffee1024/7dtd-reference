using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015FD RID: 5629
public class vp_Climb : vp_Interactable
{
	// Token: 0x0600AF63 RID: 44899 RVA: 0x004162DE File Offset: 0x004144DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_CanClimbAgain = Time.time;
	}

	// Token: 0x0600AF64 RID: 44900 RVA: 0x004162F4 File Offset: 0x004144F4
	public override bool TryInteract(vp_FPPlayerEventHandler player)
	{
		if (!base.enabled)
		{
			return false;
		}
		if (Time.time < this.m_CanClimbAgain)
		{
			return false;
		}
		if (this.m_IsClimbing)
		{
			this.m_Player.Climb.TryStop(true);
			return false;
		}
		if (this.m_Player == null)
		{
			this.m_Player = player;
		}
		if (this.m_Player.Interactable.Get() != null)
		{
			return false;
		}
		if (this.m_Controller == null)
		{
			this.m_Controller = this.m_Player.GetComponent<vp_FPController>();
		}
		if (this.m_Player.Velocity.Get().magnitude > this.MinVelocityToClimb)
		{
			return false;
		}
		if (this.m_Camera == null)
		{
			this.m_Camera = this.m_Player.GetComponentInChildren<vp_FPCamera>();
		}
		if (this.Sounds.AudioSource == null)
		{
			this.Sounds.AudioSource = this.m_Player.GetComponent<AudioSource>();
		}
		this.m_Player.Register(this);
		this.m_Player.Interactable.Set(this);
		return this.m_Player.Climb.TryStart(true);
	}

	// Token: 0x0600AF65 RID: 44901 RVA: 0x0041642C File Offset: 0x0041462C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Climb()
	{
		this.m_Controller.PhysicsGravityModifier = 0f;
		this.m_Camera.SetRotation(this.m_Camera.Transform.eulerAngles, false);
		this.m_Player.Jump.Stop(0f);
		this.m_Player.InputAllowGameplay.Set(false);
		this.m_Player.Stop.Send();
		this.m_LastWeaponEquipped = this.m_Player.CurrentWeaponIndex.Get();
		this.m_Player.SetWeapon.TryStart<int>(0);
		this.m_Player.Interactable.Set(null);
		this.PlaySound(this.Sounds.MountSounds);
		if (this.m_Controller.Transform.GetComponent<Collider>().enabled && this.m_Transform.GetComponent<Collider>().enabled)
		{
			Physics.IgnoreCollision(this.m_Controller.Transform.GetComponent<Collider>(), this.m_Transform.GetComponent<Collider>(), true);
		}
		base.StartCoroutine("LineUp");
	}

	// Token: 0x0600AF66 RID: 44902 RVA: 0x00416554 File Offset: 0x00414754
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void PlaySound(List<AudioClip> sounds)
	{
		if (this.Sounds.AudioSource == null)
		{
			return;
		}
		if (sounds == null || sounds.Count == 0)
		{
			return;
		}
		for (;;)
		{
			this.m_SoundToPlay = sounds[global::UnityEngine.Random.Range(0, sounds.Count)];
			if (this.m_SoundToPlay == null)
			{
				break;
			}
			if (!(this.m_SoundToPlay == this.m_LastPlayedSound) || sounds.Count <= 1)
			{
				goto IL_0063;
			}
		}
		return;
		IL_0063:
		if (sounds == this.Sounds.ClimbingSounds)
		{
			this.Sounds.AudioSource.pitch = global::UnityEngine.Random.Range(this.Sounds.ClimbingPitch.x, this.Sounds.ClimbingPitch.y) * Time.timeScale;
		}
		else
		{
			this.Sounds.AudioSource.pitch = 1f;
		}
		this.Sounds.AudioSource.PlayOneShot(this.m_SoundToPlay);
		this.m_LastPlayedSound = this.m_SoundToPlay;
	}

	// Token: 0x0600AF67 RID: 44903 RVA: 0x00416646 File Offset: 0x00414846
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator LineUp()
	{
		Vector3 startPosition = this.m_Player.Position.Get();
		Vector3 endPosition = this.GetNewPosition();
		Quaternion startingRotation = this.m_Camera.transform.rotation;
		Quaternion endRotation = Quaternion.LookRotation(-this.m_Transform.forward);
		bool flag = this.m_Controller.Transform.position.y > this.m_Transform.GetComponent<Collider>().bounds.center.y;
		if (flag)
		{
			endPosition += Vector3.down * this.m_Controller.CharacterController.height;
		}
		else
		{
			endPosition += this.m_Controller.Transform.up * (this.m_Controller.CharacterController.height / 2f);
		}
		if (flag && this.m_Transform.InverseTransformDirection(-this.m_Player.CameraLookDirection.Get()).z > 0f)
		{
			endRotation = Quaternion.Euler(new Vector3(45f, endRotation.eulerAngles.y, endRotation.eulerAngles.z));
		}
		else
		{
			endRotation = Quaternion.Euler(new Vector3(-45f, endRotation.eulerAngles.y, endRotation.eulerAngles.z));
		}
		endPosition = new Vector3(this.m_Transform.GetComponent<Collider>().bounds.center.x, endPosition.y, this.m_Transform.GetComponent<Collider>().bounds.center.z);
		endPosition += this.m_Transform.forward;
		float t = 0f;
		float duration = Vector3.Distance(this.m_Controller.Transform.position, endPosition) / ((!flag) ? (this.MountSpeed / 1.25f) : this.MountSpeed);
		while (t < 1f)
		{
			t += Time.deltaTime / duration;
			Vector3 vector = Vector3.Lerp(startPosition, endPosition, t);
			this.m_Player.Position.Set(vector);
			Quaternion quaternion = Quaternion.Slerp(startingRotation, endRotation, t);
			this.m_Player.Rotation.Set(new Vector2(this.MountAutoRotatePitch ? quaternion.eulerAngles.x : this.m_Player.Rotation.Get().x, quaternion.eulerAngles.y));
			yield return new WaitForEndOfFrame();
		}
		this.m_CachedDirection = this.m_Camera.Transform.forward;
		this.m_CachedRotation = this.m_Player.Rotation.Get();
		this.m_IsClimbing = true;
		yield break;
	}

	// Token: 0x0600AF68 RID: 44904 RVA: 0x00416658 File Offset: 0x00414858
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Climb()
	{
		this.m_Player.Interactable.Set(null);
		this.m_Player.InputAllowGameplay.Set(true);
		this.m_Player.SetWeapon.TryStart<int>(this.m_LastWeaponEquipped);
		this.m_Player.Unregister(this);
		this.m_CanClimbAgain = Time.time + this.ClimbAgainTimeout;
		if (this.m_Controller.Transform.GetComponent<Collider>().enabled && this.m_Transform.GetComponent<Collider>().enabled)
		{
			Physics.IgnoreCollision(this.m_Controller.Transform.GetComponent<Collider>(), this.m_Transform.GetComponent<Collider>(), false);
		}
		this.PlaySound(this.Sounds.DismountSounds);
		Vector3 vector = this.m_Controller.Transform.forward * this.DismountForce;
		if (this.m_Transform.GetComponent<Collider>().bounds.center.y < this.m_Player.Position.Get().y)
		{
			vector *= 2f;
			vector.y = this.DismountForce * 0.5f;
		}
		else
		{
			vector = -vector * 0.5f;
		}
		this.m_Player.Stop.Send();
		this.m_Controller.AddForce(vector);
		this.m_IsClimbing = false;
		this.m_Player.SetState("Default", true, true, false);
		base.StartCoroutine("RestorePitch");
	}

	// Token: 0x0600AF69 RID: 44905 RVA: 0x004167EF File Offset: 0x004149EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator RestorePitch()
	{
		float t = 0f;
		while (t < 1f && this.m_Player.InputRawLook.Get().y == 0f)
		{
			t += Time.deltaTime;
			this.m_Player.Rotation.Set(Vector2.Lerp(this.m_Player.Rotation.Get(), new Vector2(0f, this.m_Player.Rotation.Get().y), t));
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x0600AF6A RID: 44906 RVA: 0x004167FE File Offset: 0x004149FE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Interact()
	{
		if (this.m_IsClimbing)
		{
			this.m_Player.Climb.TryStop(true);
		}
		return true;
	}

	// Token: 0x0600AF6B RID: 44907 RVA: 0x0041681B File Offset: 0x00414A1B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void FixedUpdate()
	{
		this.Climbing();
	}

	// Token: 0x0600AF6C RID: 44908 RVA: 0x00416823 File Offset: 0x00414A23
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.InputJump();
	}

	// Token: 0x0600AF6D RID: 44909 RVA: 0x0041682B File Offset: 0x00414A2B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		this.FinishInteraction();
	}

	// Token: 0x0600AF6E RID: 44910 RVA: 0x00416833 File Offset: 0x00414A33
	public override void FinishInteraction()
	{
		if (this.m_IsClimbing)
		{
			this.m_Player.Climb.TryStop(true);
		}
	}

	// Token: 0x0600AF6F RID: 44911 RVA: 0x00416850 File Offset: 0x00414A50
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Climbing()
	{
		if (this.m_Player == null || !this.m_IsClimbing)
		{
			return;
		}
		this.m_Controller.PhysicsGravityModifier = 0f;
		this.m_Camera.RotationYawLimit = new Vector2(this.m_CachedRotation.y - 90f, this.m_CachedRotation.y + 90f);
		this.m_Camera.RotationPitchLimit = new Vector2(90f, -90f);
		Vector3 vector = this.GetNewPosition();
		Vector3 vector2 = Vector3.zero;
		float num = this.m_Player.Rotation.Get().x / 90f;
		float num2 = this.MinimumClimbSpeed / this.ClimbSpeed;
		if (Mathf.Abs(num) < num2)
		{
			num = ((num > 0f) ? num2 : (num2 * -1f));
		}
		if (num < 0f)
		{
			vector2 = Vector3.up * -num;
		}
		else if (num > 0f)
		{
			vector2 = Vector3.down * num;
		}
		float num3 = this.ClimbSpeed;
		float num4 = (vector2 * this.m_Player.InputClimbVector.Get()).y;
		if (this.SimpleClimb)
		{
			vector2 = Vector3.up;
			num3 *= 0.75f;
			num4 = this.m_Player.InputClimbVector.Get();
		}
		if ((num4 > 0f && vector.y > vp_Climb.GetTopOfCollider(this.m_Transform) - this.m_Controller.CharacterController.height * 0.25f) || (num4 < 0f && this.m_Controller.Grounded && this.m_Controller.GroundTransform.GetInstanceID() != this.m_Transform.GetInstanceID()))
		{
			this.m_Player.Climb.TryStop(true);
			return;
		}
		if (this.m_Player.InputClimbVector.Get() == 0f)
		{
			this.m_ClimbingSoundTimer.Cancel();
		}
		if (this.m_Player.InputClimbVector.Get() != 0f && !this.m_ClimbingSoundTimer.Active && this.Sounds.ClimbingSounds.Count > 0)
		{
			float num5 = Mathf.Abs(5f / vector2.y * (Time.deltaTime * 5f) / this.Sounds.ClimbingSoundSpeed);
			vp_Timer.In(this.SimpleClimb ? (num5 * 3f) : num5, delegate
			{
				this.PlaySound(this.Sounds.ClimbingSounds);
			}, this.m_ClimbingSoundTimer);
		}
		vector += vector2 * num3 * Time.deltaTime * this.m_Player.InputClimbVector.Get();
		this.m_Player.Position.Set(Vector3.Slerp(this.m_Controller.Transform.position, vector, Time.deltaTime * num3));
	}

	// Token: 0x0600AF70 RID: 44912 RVA: 0x00416B44 File Offset: 0x00414D44
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector3 GetNewPosition()
	{
		Vector3 vector = this.m_Controller.Transform.position;
		RaycastHit raycastHit;
		Physics.Raycast(new Ray(this.m_Controller.Transform.position, this.m_CachedDirection), out raycastHit, this.DistanceToClimbable * 4f);
		if (raycastHit.collider != null && raycastHit.transform.GetInstanceID() == this.m_Transform.GetInstanceID() && (raycastHit.distance > this.DistanceToClimbable || raycastHit.distance < this.DistanceToClimbable))
		{
			vector = (vector - raycastHit.point).normalized * this.DistanceToClimbable + raycastHit.point;
		}
		return vector;
	}

	// Token: 0x0600AF71 RID: 44913 RVA: 0x00416C08 File Offset: 0x00414E08
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputJump()
	{
		if (!this.m_IsClimbing)
		{
			return;
		}
		if (this.m_Player == null)
		{
			return;
		}
		if (this.m_Player.InputGetButton.Send("Jump") || this.m_Player.InputGetButtonDown.Send("Interact"))
		{
			this.m_Player.Climb.TryStop(true);
			if (this.m_Player.InputGetButton.Send("Jump"))
			{
				this.m_Controller.AddForce(-this.m_Controller.Transform.forward * this.m_Controller.MotorJumpForce);
			}
		}
	}

	// Token: 0x0600AF72 RID: 44914 RVA: 0x00416CC4 File Offset: 0x00414EC4
	public static float GetTopOfCollider(Transform t)
	{
		return t.position.y + t.GetComponent<Collider>().bounds.size.y / 2f;
	}

	// Token: 0x040082D1 RID: 33489
	public float MinimumClimbSpeed = 3f;

	// Token: 0x040082D2 RID: 33490
	public float ClimbSpeed = 16f;

	// Token: 0x040082D3 RID: 33491
	public float MountSpeed = 5f;

	// Token: 0x040082D4 RID: 33492
	public float DistanceToClimbable = 1f;

	// Token: 0x040082D5 RID: 33493
	public float MinVelocityToClimb = 7f;

	// Token: 0x040082D6 RID: 33494
	public float ClimbAgainTimeout = 1f;

	// Token: 0x040082D7 RID: 33495
	public bool MountAutoRotatePitch;

	// Token: 0x040082D8 RID: 33496
	public bool SimpleClimb = true;

	// Token: 0x040082D9 RID: 33497
	public float DismountForce = 0.2f;

	// Token: 0x040082DA RID: 33498
	public vp_Climb.vp_ClimbingSounds Sounds;

	// Token: 0x040082DB RID: 33499
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_LastWeaponEquipped;

	// Token: 0x040082DC RID: 33500
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsClimbing;

	// Token: 0x040082DD RID: 33501
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_CanClimbAgain;

	// Token: 0x040082DE RID: 33502
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_CachedDirection = Vector3.zero;

	// Token: 0x040082DF RID: 33503
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_CachedRotation = Vector2.zero;

	// Token: 0x040082E0 RID: 33504
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ClimbingSoundTimer = new vp_Timer.Handle();

	// Token: 0x040082E1 RID: 33505
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_SoundToPlay;

	// Token: 0x040082E2 RID: 33506
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip m_LastPlayedSound;

	// Token: 0x020015FE RID: 5630
	[Serializable]
	public class vp_ClimbingSounds
	{
		// Token: 0x040082E3 RID: 33507
		public AudioSource AudioSource;

		// Token: 0x040082E4 RID: 33508
		public List<AudioClip> MountSounds = new List<AudioClip>();

		// Token: 0x040082E5 RID: 33509
		public List<AudioClip> DismountSounds = new List<AudioClip>();

		// Token: 0x040082E6 RID: 33510
		public float ClimbingSoundSpeed = 4f;

		// Token: 0x040082E7 RID: 33511
		public Vector2 ClimbingPitch = new Vector2(1f, 1.5f);

		// Token: 0x040082E8 RID: 33512
		public List<AudioClip> ClimbingSounds = new List<AudioClip>();
	}
}
