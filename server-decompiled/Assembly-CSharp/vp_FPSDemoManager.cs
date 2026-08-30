using System;
using UnityEngine;

// Token: 0x020015A0 RID: 5536
public class vp_FPSDemoManager : vp_DemoManager
{
	// Token: 0x17001406 RID: 5126
	// (get) Token: 0x0600AD3E RID: 44350 RVA: 0x00409830 File Offset: 0x00407A30
	public vp_Shooter CurrentShooter
	{
		get
		{
			if (this.m_CurrentShooter == null || (this.m_CurrentShooter != null && (!this.m_CurrentShooter.enabled || !vp_Utility.IsActive(this.m_CurrentShooter.gameObject))))
			{
				this.m_CurrentShooter = this.Player.GetComponentInChildren<vp_Shooter>();
			}
			return this.m_CurrentShooter;
		}
	}

	// Token: 0x17001407 RID: 5127
	// (get) Token: 0x0600AD3F RID: 44351 RVA: 0x00409890 File Offset: 0x00407A90
	// (set) Token: 0x0600AD40 RID: 44352 RVA: 0x004098CC File Offset: 0x00407ACC
	public bool DrawCrosshair
	{
		get
		{
			vp_SimpleCrosshair vp_SimpleCrosshair = (vp_SimpleCrosshair)this.Player.GetComponent(typeof(vp_SimpleCrosshair));
			return !(vp_SimpleCrosshair == null) && vp_SimpleCrosshair.enabled;
		}
		set
		{
			vp_SimpleCrosshair vp_SimpleCrosshair = (vp_SimpleCrosshair)this.Player.GetComponent(typeof(vp_SimpleCrosshair));
			if (vp_SimpleCrosshair != null)
			{
				vp_SimpleCrosshair.enabled = value;
			}
		}
	}

	// Token: 0x0600AD41 RID: 44353 RVA: 0x00409904 File Offset: 0x00407B04
	public vp_FPSDemoManager(GameObject player)
	{
		this.Player = player;
		this.Controller = this.Player.GetComponent<vp_FPController>();
		this.Camera = this.Player.GetComponentInChildren<vp_FPCamera>();
		this.WeaponHandler = this.Player.GetComponentInChildren<vp_WeaponHandler>();
		this.PlayerEventHandler = (vp_FPPlayerEventHandler)this.Player.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		this.Input = this.Player.GetComponent<vp_FPInput>();
		this.Earthquake = global::UnityEngine.Object.FindAnyObjectByType<vp_FPEarthquake>();
		if (Screen.width < 1024)
		{
			this.EditorPreviewSectionExpanded = false;
		}
	}

	// Token: 0x0600AD42 RID: 44354 RVA: 0x004099CC File Offset: 0x00407BCC
	public void Teleport(Vector3 pos, Vector2 startAngle)
	{
		this.Controller.SetPosition(pos);
		this.Camera.SetRotation(startAngle);
	}

	// Token: 0x0600AD43 RID: 44355 RVA: 0x004099E8 File Offset: 0x00407BE8
	public void SmoothLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = Vector3.SmoothDamp(this.m_CurrentLookPoint, lookPoint, ref this.m_LookVelocity, this.LookDamping);
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600AD44 RID: 44356 RVA: 0x00409A64 File Offset: 0x00407C64
	public void SnapLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = lookPoint;
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600AD45 RID: 44357 RVA: 0x00409AC8 File Offset: 0x00407CC8
	public void FreezePlayer(Vector3 pos, Vector2 startAngle, bool freezeCamera)
	{
		this.m_UnFreezePosition = this.Controller.transform.position;
		this.Teleport(pos, startAngle);
		this.Controller.SetState("Freeze", true, false, false);
		this.Controller.Stop();
		if (freezeCamera)
		{
			this.Camera.SetState("Freeze", true, false, false);
			this.Input.SetState("Freeze", true, false, false);
		}
	}

	// Token: 0x0600AD46 RID: 44358 RVA: 0x00409B3A File Offset: 0x00407D3A
	public void FreezePlayer(Vector3 pos, Vector2 startAngle)
	{
		this.FreezePlayer(pos, startAngle, false);
	}

	// Token: 0x0600AD47 RID: 44359 RVA: 0x00409B48 File Offset: 0x00407D48
	public void UnFreezePlayer()
	{
		this.Controller.transform.position = this.m_UnFreezePosition;
		this.m_UnFreezePosition = Vector3.zero;
		this.Controller.SetState("Freeze", false, false, false);
		this.Camera.SetState("Freeze", false, false, false);
		this.Input.SetState("Freeze", false, false, false);
		this.Input.Refresh();
	}

	// Token: 0x0600AD48 RID: 44360 RVA: 0x00409BBC File Offset: 0x00407DBC
	public void LockControls()
	{
		this.Input.AllowGameplayInput = false;
		this.Input.MouseLookSensitivity = Vector2.zero;
		if (this.WeaponHandler.CurrentWeapon != null)
		{
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).RotationLookSway = Vector2.zero;
		}
	}

	// Token: 0x0600AD49 RID: 44361 RVA: 0x00409C18 File Offset: 0x00407E18
	public void SetWeaponPreset(TextAsset weaponPreset, TextAsset shooterPreset = null, bool smoothFade = true)
	{
		if (this.WeaponHandler.CurrentWeapon == null)
		{
			return;
		}
		this.WeaponHandler.CurrentWeapon.Load(weaponPreset);
		if (!smoothFade)
		{
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).SnapSprings();
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).SnapPivot();
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).SnapZoom();
		}
		this.WeaponHandler.CurrentWeapon.Refresh();
		if (shooterPreset != null && this.CurrentShooter != null)
		{
			this.CurrentShooter.Load(shooterPreset);
		}
		this.CurrentShooter.Refresh();
	}

	// Token: 0x0600AD4A RID: 44362 RVA: 0x00409CCC File Offset: 0x00407ECC
	public void RefreshDefaultState()
	{
		if (this.Controller != null)
		{
			this.Controller.RefreshDefaultState();
		}
		if (this.Camera != null)
		{
			this.Camera.RefreshDefaultState();
			if (this.WeaponHandler.CurrentWeapon != null)
			{
				this.WeaponHandler.CurrentWeapon.RefreshDefaultState();
			}
			if (this.CurrentShooter != null)
			{
				this.CurrentShooter.RefreshDefaultState();
			}
		}
		if (this.Input != null)
		{
			this.Input.RefreshDefaultState();
		}
	}

	// Token: 0x0600AD4B RID: 44363 RVA: 0x00409D60 File Offset: 0x00407F60
	public void ResetState()
	{
		if (this.Controller != null)
		{
			this.Controller.ResetState();
		}
		if (this.Camera != null)
		{
			this.Camera.ResetState();
			if (this.WeaponHandler.CurrentWeapon != null)
			{
				this.WeaponHandler.CurrentWeapon.ResetState();
			}
			if (this.CurrentShooter != null)
			{
				this.CurrentShooter.ResetState();
			}
		}
		if (this.Input != null)
		{
			this.Input.ResetState();
		}
	}

	// Token: 0x0600AD4C RID: 44364 RVA: 0x00409DF4 File Offset: 0x00407FF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Reset()
	{
		base.Reset();
		this.PlayerEventHandler.RefreshActivityStates();
		this.WeaponHandler.SetWeapon(0);
		this.PlayerEventHandler.CameraEarthQuake.Stop(0f);
		this.Camera.BobStepCallback = null;
		this.Camera.SnapSprings();
		if (this.WeaponHandler.CurrentWeapon != null)
		{
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).SetPivotVisible(false);
			this.WeaponHandler.CurrentWeapon.SnapSprings();
			vp_Layer.Set(this.WeaponHandler.CurrentWeapon.gameObject, 10, true);
		}
		if (Screen.width < 1024)
		{
			this.EditorPreviewSectionExpanded = false;
		}
		else
		{
			this.EditorPreviewSectionExpanded = true;
		}
		if (this.m_UnFreezePosition != Vector3.zero)
		{
			this.UnFreezePlayer();
		}
	}

	// Token: 0x0600AD4D RID: 44365 RVA: 0x00409ECF File Offset: 0x004080CF
	public void ForceCameraShake(float speed, Vector3 amplitude)
	{
		this.Camera.ShakeSpeed = speed;
		this.Camera.ShakeAmplitude = amplitude;
	}

	// Token: 0x0600AD4E RID: 44366 RVA: 0x00409EE9 File Offset: 0x004080E9
	public void ForceCameraShake()
	{
		this.ForceCameraShake(0.0727273f, new Vector3(-10f, 10f, 0f));
	}

	// Token: 0x040080CB RID: 32971
	public GameObject Player;

	// Token: 0x040080CC RID: 32972
	public vp_FPController Controller;

	// Token: 0x040080CD RID: 32973
	public vp_FPCamera Camera;

	// Token: 0x040080CE RID: 32974
	public vp_WeaponHandler WeaponHandler;

	// Token: 0x040080CF RID: 32975
	public vp_FPInput Input;

	// Token: 0x040080D0 RID: 32976
	public vp_FPEarthquake Earthquake;

	// Token: 0x040080D1 RID: 32977
	public vp_FPPlayerEventHandler PlayerEventHandler;

	// Token: 0x040080D2 RID: 32978
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_UnFreezePosition = Vector3.zero;

	// Token: 0x040080D3 RID: 32979
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_CurrentLookPoint = Vector3.zero;

	// Token: 0x040080D4 RID: 32980
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_LookVelocity = Vector3.zero;

	// Token: 0x040080D5 RID: 32981
	public float LookDamping = 0.3f;

	// Token: 0x040080D6 RID: 32982
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Shooter m_CurrentShooter;
}
