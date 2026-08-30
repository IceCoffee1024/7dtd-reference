using System;
using UnityEngine;

// Token: 0x02001585 RID: 5509
public class vp_FPSDemoManager : vp_DemoManager
{
	// Token: 0x170013CD RID: 5069
	// (get) Token: 0x0600AC41 RID: 44097 RVA: 0x004044F0 File Offset: 0x004026F0
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

	// Token: 0x170013CE RID: 5070
	// (get) Token: 0x0600AC42 RID: 44098 RVA: 0x00404550 File Offset: 0x00402750
	// (set) Token: 0x0600AC43 RID: 44099 RVA: 0x0040458C File Offset: 0x0040278C
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

	// Token: 0x0600AC44 RID: 44100 RVA: 0x004045C4 File Offset: 0x004027C4
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

	// Token: 0x0600AC45 RID: 44101 RVA: 0x0040468C File Offset: 0x0040288C
	public void Teleport(Vector3 pos, Vector2 startAngle)
	{
		this.Controller.SetPosition(pos);
		this.Camera.SetRotation(startAngle);
	}

	// Token: 0x0600AC46 RID: 44102 RVA: 0x004046A8 File Offset: 0x004028A8
	public void SmoothLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = Vector3.SmoothDamp(this.m_CurrentLookPoint, lookPoint, ref this.m_LookVelocity, this.LookDamping);
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600AC47 RID: 44103 RVA: 0x00404724 File Offset: 0x00402924
	public void SnapLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = lookPoint;
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600AC48 RID: 44104 RVA: 0x00404788 File Offset: 0x00402988
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

	// Token: 0x0600AC49 RID: 44105 RVA: 0x004047FA File Offset: 0x004029FA
	public void FreezePlayer(Vector3 pos, Vector2 startAngle)
	{
		this.FreezePlayer(pos, startAngle, false);
	}

	// Token: 0x0600AC4A RID: 44106 RVA: 0x00404808 File Offset: 0x00402A08
	public void UnFreezePlayer()
	{
		this.Controller.transform.position = this.m_UnFreezePosition;
		this.m_UnFreezePosition = Vector3.zero;
		this.Controller.SetState("Freeze", false, false, false);
		this.Camera.SetState("Freeze", false, false, false);
		this.Input.SetState("Freeze", false, false, false);
		this.Input.Refresh();
	}

	// Token: 0x0600AC4B RID: 44107 RVA: 0x0040487C File Offset: 0x00402A7C
	public void LockControls()
	{
		this.Input.AllowGameplayInput = false;
		this.Input.MouseLookSensitivity = Vector2.zero;
		if (this.WeaponHandler.CurrentWeapon != null)
		{
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).RotationLookSway = Vector2.zero;
		}
	}

	// Token: 0x0600AC4C RID: 44108 RVA: 0x004048D8 File Offset: 0x00402AD8
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

	// Token: 0x0600AC4D RID: 44109 RVA: 0x0040498C File Offset: 0x00402B8C
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

	// Token: 0x0600AC4E RID: 44110 RVA: 0x00404A20 File Offset: 0x00402C20
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

	// Token: 0x0600AC4F RID: 44111 RVA: 0x00404AB4 File Offset: 0x00402CB4
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

	// Token: 0x0600AC50 RID: 44112 RVA: 0x00404B8F File Offset: 0x00402D8F
	public void ForceCameraShake(float speed, Vector3 amplitude)
	{
		this.Camera.ShakeSpeed = speed;
		this.Camera.ShakeAmplitude = amplitude;
	}

	// Token: 0x0600AC51 RID: 44113 RVA: 0x00404BA9 File Offset: 0x00402DA9
	public void ForceCameraShake()
	{
		this.ForceCameraShake(0.0727273f, new Vector3(-10f, 10f, 0f));
	}

	// Token: 0x04007FF2 RID: 32754
	public GameObject Player;

	// Token: 0x04007FF3 RID: 32755
	public vp_FPController Controller;

	// Token: 0x04007FF4 RID: 32756
	public vp_FPCamera Camera;

	// Token: 0x04007FF5 RID: 32757
	public vp_WeaponHandler WeaponHandler;

	// Token: 0x04007FF6 RID: 32758
	public vp_FPInput Input;

	// Token: 0x04007FF7 RID: 32759
	public vp_FPEarthquake Earthquake;

	// Token: 0x04007FF8 RID: 32760
	public vp_FPPlayerEventHandler PlayerEventHandler;

	// Token: 0x04007FF9 RID: 32761
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_UnFreezePosition = Vector3.zero;

	// Token: 0x04007FFA RID: 32762
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_CurrentLookPoint = Vector3.zero;

	// Token: 0x04007FFB RID: 32763
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_LookVelocity = Vector3.zero;

	// Token: 0x04007FFC RID: 32764
	public float LookDamping = 0.3f;

	// Token: 0x04007FFD RID: 32765
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Shooter m_CurrentShooter;
}
