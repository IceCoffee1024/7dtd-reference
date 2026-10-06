using System;
using UnityEngine;

// Token: 0x020015C2 RID: 5570
public class vp_FPSDemoManager : vp_DemoManager
{
	// Token: 0x1700144B RID: 5195
	// (get) Token: 0x0600ADF1 RID: 44529 RVA: 0x0040E684 File Offset: 0x0040C884
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

	// Token: 0x1700144C RID: 5196
	// (get) Token: 0x0600ADF2 RID: 44530 RVA: 0x0040E6E4 File Offset: 0x0040C8E4
	// (set) Token: 0x0600ADF3 RID: 44531 RVA: 0x0040E720 File Offset: 0x0040C920
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

	// Token: 0x0600ADF4 RID: 44532 RVA: 0x0040E758 File Offset: 0x0040C958
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

	// Token: 0x0600ADF5 RID: 44533 RVA: 0x0040E820 File Offset: 0x0040CA20
	public void Teleport(Vector3 pos, Vector2 startAngle)
	{
		this.Controller.SetPosition(pos);
		this.Camera.SetRotation(startAngle);
	}

	// Token: 0x0600ADF6 RID: 44534 RVA: 0x0040E83C File Offset: 0x0040CA3C
	public void SmoothLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = Vector3.SmoothDamp(this.m_CurrentLookPoint, lookPoint, ref this.m_LookVelocity, this.LookDamping);
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600ADF7 RID: 44535 RVA: 0x0040E8B8 File Offset: 0x0040CAB8
	public void SnapLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = lookPoint;
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600ADF8 RID: 44536 RVA: 0x0040E91C File Offset: 0x0040CB1C
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

	// Token: 0x0600ADF9 RID: 44537 RVA: 0x0040E98E File Offset: 0x0040CB8E
	public void FreezePlayer(Vector3 pos, Vector2 startAngle)
	{
		this.FreezePlayer(pos, startAngle, false);
	}

	// Token: 0x0600ADFA RID: 44538 RVA: 0x0040E99C File Offset: 0x0040CB9C
	public void UnFreezePlayer()
	{
		this.Controller.transform.position = this.m_UnFreezePosition;
		this.m_UnFreezePosition = Vector3.zero;
		this.Controller.SetState("Freeze", false, false, false);
		this.Camera.SetState("Freeze", false, false, false);
		this.Input.SetState("Freeze", false, false, false);
		this.Input.Refresh();
	}

	// Token: 0x0600ADFB RID: 44539 RVA: 0x0040EA10 File Offset: 0x0040CC10
	public void LockControls()
	{
		this.Input.AllowGameplayInput = false;
		this.Input.MouseLookSensitivity = Vector2.zero;
		if (this.WeaponHandler.CurrentWeapon != null)
		{
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).RotationLookSway = Vector2.zero;
		}
	}

	// Token: 0x0600ADFC RID: 44540 RVA: 0x0040EA6C File Offset: 0x0040CC6C
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

	// Token: 0x0600ADFD RID: 44541 RVA: 0x0040EB20 File Offset: 0x0040CD20
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

	// Token: 0x0600ADFE RID: 44542 RVA: 0x0040EBB4 File Offset: 0x0040CDB4
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

	// Token: 0x0600ADFF RID: 44543 RVA: 0x0040EC48 File Offset: 0x0040CE48
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

	// Token: 0x0600AE00 RID: 44544 RVA: 0x0040ED23 File Offset: 0x0040CF23
	public void ForceCameraShake(float speed, Vector3 amplitude)
	{
		this.Camera.ShakeSpeed = speed;
		this.Camera.ShakeAmplitude = amplitude;
	}

	// Token: 0x0600AE01 RID: 44545 RVA: 0x0040ED3D File Offset: 0x0040CF3D
	public void ForceCameraShake()
	{
		this.ForceCameraShake(0.0727273f, new Vector3(-10f, 10f, 0f));
	}

	// Token: 0x0400814E RID: 33102
	public GameObject Player;

	// Token: 0x0400814F RID: 33103
	public vp_FPController Controller;

	// Token: 0x04008150 RID: 33104
	public vp_FPCamera Camera;

	// Token: 0x04008151 RID: 33105
	public vp_WeaponHandler WeaponHandler;

	// Token: 0x04008152 RID: 33106
	public vp_FPInput Input;

	// Token: 0x04008153 RID: 33107
	public vp_FPEarthquake Earthquake;

	// Token: 0x04008154 RID: 33108
	public vp_FPPlayerEventHandler PlayerEventHandler;

	// Token: 0x04008155 RID: 33109
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_UnFreezePosition = Vector3.zero;

	// Token: 0x04008156 RID: 33110
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_CurrentLookPoint = Vector3.zero;

	// Token: 0x04008157 RID: 33111
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_LookVelocity = Vector3.zero;

	// Token: 0x04008158 RID: 33112
	public float LookDamping = 0.3f;

	// Token: 0x04008159 RID: 33113
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Shooter m_CurrentShooter;
}
