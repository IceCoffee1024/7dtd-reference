using System;
using UnityEngine;

// Token: 0x020015D4 RID: 5588
public class vp_FPSDemoManager : vp_DemoManager
{
	// Token: 0x17001457 RID: 5207
	// (get) Token: 0x0600AE49 RID: 44617 RVA: 0x00410A88 File Offset: 0x0040EC88
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

	// Token: 0x17001458 RID: 5208
	// (get) Token: 0x0600AE4A RID: 44618 RVA: 0x00410AE8 File Offset: 0x0040ECE8
	// (set) Token: 0x0600AE4B RID: 44619 RVA: 0x00410B24 File Offset: 0x0040ED24
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

	// Token: 0x0600AE4C RID: 44620 RVA: 0x00410B5C File Offset: 0x0040ED5C
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

	// Token: 0x0600AE4D RID: 44621 RVA: 0x00410C24 File Offset: 0x0040EE24
	public void Teleport(Vector3 pos, Vector2 startAngle)
	{
		this.Controller.SetPosition(pos);
		this.Camera.SetRotation(startAngle);
	}

	// Token: 0x0600AE4E RID: 44622 RVA: 0x00410C40 File Offset: 0x0040EE40
	public void SmoothLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = Vector3.SmoothDamp(this.m_CurrentLookPoint, lookPoint, ref this.m_LookVelocity, this.LookDamping);
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600AE4F RID: 44623 RVA: 0x00410CBC File Offset: 0x0040EEBC
	public void SnapLookAt(Vector3 lookPoint)
	{
		this.m_CurrentLookPoint = lookPoint;
		this.Camera.transform.LookAt(this.m_CurrentLookPoint);
		this.Camera.Angle = new Vector2(this.Camera.transform.eulerAngles.x, this.Camera.transform.eulerAngles.y);
	}

	// Token: 0x0600AE50 RID: 44624 RVA: 0x00410D20 File Offset: 0x0040EF20
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

	// Token: 0x0600AE51 RID: 44625 RVA: 0x00410D92 File Offset: 0x0040EF92
	public void FreezePlayer(Vector3 pos, Vector2 startAngle)
	{
		this.FreezePlayer(pos, startAngle, false);
	}

	// Token: 0x0600AE52 RID: 44626 RVA: 0x00410DA0 File Offset: 0x0040EFA0
	public void UnFreezePlayer()
	{
		this.Controller.transform.position = this.m_UnFreezePosition;
		this.m_UnFreezePosition = Vector3.zero;
		this.Controller.SetState("Freeze", false, false, false);
		this.Camera.SetState("Freeze", false, false, false);
		this.Input.SetState("Freeze", false, false, false);
		this.Input.Refresh();
	}

	// Token: 0x0600AE53 RID: 44627 RVA: 0x00410E14 File Offset: 0x0040F014
	public void LockControls()
	{
		this.Input.AllowGameplayInput = false;
		this.Input.MouseLookSensitivity = Vector2.zero;
		if (this.WeaponHandler.CurrentWeapon != null)
		{
			((vp_FPWeapon)this.WeaponHandler.CurrentWeapon).RotationLookSway = Vector2.zero;
		}
	}

	// Token: 0x0600AE54 RID: 44628 RVA: 0x00410E70 File Offset: 0x0040F070
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

	// Token: 0x0600AE55 RID: 44629 RVA: 0x00410F24 File Offset: 0x0040F124
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

	// Token: 0x0600AE56 RID: 44630 RVA: 0x00410FB8 File Offset: 0x0040F1B8
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

	// Token: 0x0600AE57 RID: 44631 RVA: 0x0041104C File Offset: 0x0040F24C
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

	// Token: 0x0600AE58 RID: 44632 RVA: 0x00411127 File Offset: 0x0040F327
	public void ForceCameraShake(float speed, Vector3 amplitude)
	{
		this.Camera.ShakeSpeed = speed;
		this.Camera.ShakeAmplitude = amplitude;
	}

	// Token: 0x0600AE59 RID: 44633 RVA: 0x00411141 File Offset: 0x0040F341
	public void ForceCameraShake()
	{
		this.ForceCameraShake(0.0727273f, new Vector3(-10f, 10f, 0f));
	}

	// Token: 0x0400818F RID: 33167
	public GameObject Player;

	// Token: 0x04008190 RID: 33168
	public vp_FPController Controller;

	// Token: 0x04008191 RID: 33169
	public vp_FPCamera Camera;

	// Token: 0x04008192 RID: 33170
	public vp_WeaponHandler WeaponHandler;

	// Token: 0x04008193 RID: 33171
	public vp_FPInput Input;

	// Token: 0x04008194 RID: 33172
	public vp_FPEarthquake Earthquake;

	// Token: 0x04008195 RID: 33173
	public vp_FPPlayerEventHandler PlayerEventHandler;

	// Token: 0x04008196 RID: 33174
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_UnFreezePosition = Vector3.zero;

	// Token: 0x04008197 RID: 33175
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_CurrentLookPoint = Vector3.zero;

	// Token: 0x04008198 RID: 33176
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 m_LookVelocity = Vector3.zero;

	// Token: 0x04008199 RID: 33177
	public float LookDamping = 0.3f;

	// Token: 0x0400819A RID: 33178
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Shooter m_CurrentShooter;
}
