using System;
using UnityEngine;

// Token: 0x0200157F RID: 5503
public class vp_FPSDemo1 : MonoBehaviour
{
	// Token: 0x0600AC03 RID: 44035 RVA: 0x00400870 File Offset: 0x003FEA70
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.m_Demo = new vp_FPSDemoManager(this.Player);
		this.m_Demo.PlayerEventHandler.Register(this);
		this.m_Demo.CurrentFullScreenFadeTime = Time.time;
		this.m_Demo.DrawCrosshair = false;
		this.m_Demo.Input.MouseCursorZones = new Rect[3];
		this.m_Demo.Input.MouseCursorZones[0] = new Rect((float)Screen.width * 0.5f - 370f, 40f, 80f, 80f);
		this.m_Demo.Input.MouseCursorZones[1] = new Rect((float)Screen.width * 0.5f + 290f, 40f, 80f, 80f);
		this.m_Demo.Input.MouseCursorZones[2] = new Rect(0f, 0f, 150f, (float)Screen.height);
		vp_Utility.LockCursor = false;
		this.m_Demo.Camera.RenderingFieldOfView = 20f;
		this.m_Demo.Camera.SnapZoom();
		this.m_Demo.Camera.PositionOffset = new Vector3(0f, 1.75f, 0.1f);
		this.m_AudioSource = this.m_Demo.Camera.gameObject.AddComponent<AudioSource>();
		this.m_Demo.PlayerEventHandler.SetWeapon.Disallow(10000000f);
		this.m_Demo.PlayerEventHandler.SetPrevWeapon.Try = () => false;
		this.m_Demo.PlayerEventHandler.SetNextWeapon.Try = () => false;
	}

	// Token: 0x0600AC04 RID: 44036 RVA: 0x00400A65 File Offset: 0x003FEC65
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.m_Demo.PlayerEventHandler != null)
		{
			this.m_Demo.PlayerEventHandler.Unregister(this);
		}
	}

	// Token: 0x0600AC05 RID: 44037 RVA: 0x00400A8C File Offset: 0x003FEC8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.m_Demo.Update();
		if (this.m_Demo.CurrentScreen == 1 && this.m_Demo.WeaponHandler.CurrentWeapon != null)
		{
			this.m_Demo.WeaponHandler.SetWeapon(0);
		}
		if (this.m_Demo.CurrentScreen == 2)
		{
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				this.m_Demo.ButtonSelection = 0;
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.m_Demo.ButtonSelection = 1;
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				this.m_Demo.ButtonSelection = 2;
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				this.m_Demo.ButtonSelection = 3;
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				this.m_Demo.ButtonSelection = 4;
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				this.m_Demo.ButtonSelection = 5;
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				this.m_Demo.ButtonSelection = 6;
			}
			if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				this.m_Demo.ButtonSelection = 7;
			}
			if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				this.m_Demo.ButtonSelection = 8;
			}
			if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				this.m_Demo.ButtonSelection = 9;
			}
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.m_Demo.ButtonSelection = 10;
			}
			if (Input.GetKeyDown(KeyCode.Q))
			{
				this.m_Demo.ButtonSelection--;
				if (this.m_Demo.ButtonSelection < 1)
				{
					this.m_Demo.ButtonSelection = 10;
				}
			}
			if (Input.GetKeyDown(KeyCode.E))
			{
				this.m_Demo.ButtonSelection++;
				if (this.m_Demo.ButtonSelection > 10)
				{
					this.m_Demo.ButtonSelection = 1;
				}
			}
		}
		this.m_Demo.Input.MouseCursorBlocksMouseLook = false;
		if (this.m_Demo.CurrentScreen != 3 && this.m_ChrashingAirplaneRestoreTimer.Active)
		{
			this.m_ChrashingAirplaneRestoreTimer.Cancel();
		}
	}

	// Token: 0x0600AC06 RID: 44038 RVA: 0x00400C7C File Offset: 0x003FEE7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoIntro()
	{
		this.m_Demo.DrawBoxes("part ii: under the hood", "Ultimate FPS features a NEXT-GEN first person camera system with ultra smooth PROCEDURAL ANIMATION of player movements. Camera and weapons are manipulated using over 100 parameters, allowing for a vast range of super-lifelike behaviors.", null, this.m_ImageRightArrow, null, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.DrawCrosshair = false;
			this.m_Demo.FirstFrame = false;
			this.m_Demo.Camera.RenderingFieldOfView = 20f;
			this.m_Demo.Camera.SnapZoom();
			this.m_Demo.WeaponHandler.SetWeapon(0);
			this.m_Demo.FreezePlayer(this.m_OverviewPos, this.m_OverviewAngle, true);
			this.m_Demo.LastInputTime -= 20f;
			this.m_Demo.RefreshDefaultState();
			this.m_Demo.Input.MouseCursorForced = true;
		}
		this.m_Demo.Input.MouseCursorForced = true;
		this.m_Demo.ForceCameraShake();
	}

	// Token: 0x0600AC07 RID: 44039 RVA: 0x00400D70 File Offset: 0x003FEF70
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetWeapon(int i, string state = null, bool drawCrosshair = true, bool wieldMotion = true)
	{
		this.m_Demo.DrawCrosshair = drawCrosshair;
		if (this.m_Demo.WeaponHandler.CurrentWeaponIndex != i)
		{
			if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
			{
				if (this.m_ExamplesCurrentSel == 0)
				{
					((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapToExit();
				}
				else if (wieldMotion)
				{
					this.m_Demo.WeaponHandler.CurrentWeapon.Wield(false);
				}
			}
			vp_Timer.In(wieldMotion ? 0.2f : 0f, delegate
			{
				this.m_Demo.WeaponHandler.SetWeapon(i);
				if (this.m_Demo.WeaponHandler.CurrentWeapon != null && wieldMotion)
				{
					this.m_Demo.WeaponHandler.CurrentWeapon.Wield(true);
				}
				if (state != null)
				{
					this.m_Demo.PlayerEventHandler.ResetActivityStates();
					this.m_Demo.PlayerEventHandler.SetState(state, true, true, false);
				}
			}, this.m_WeaponSwitchTimer);
			return;
		}
		if (state != null)
		{
			this.m_Demo.PlayerEventHandler.SetState(state, true, true, false);
		}
	}

	// Token: 0x0600AC08 RID: 44040 RVA: 0x00400E70 File Offset: 0x003FF070
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoExamples()
	{
		this.m_Demo.DrawBoxes("examples", "Try MOVING, JUMPING and STRAFING with the demo presets on the left.\nNote that NO ANIMATIONS are used in this demo. Instead, the camera and weapons are manipulated using realtime SPRING PHYSICS, SINUS BOB and NOISE SHAKING.\nCombining this with traditional animations (e.g. reload) can be very powerful!", this.m_ImageLeftArrow, this.m_ImageRightArrow, null, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_AudioSource.Stop();
			this.m_Demo.DrawCrosshair = true;
			this.m_Demo.Teleport(this.m_StartPos, this.m_StartAngle);
			this.m_Demo.FirstFrame = false;
			this.m_UnFreezePosition = this.m_Demo.Controller.transform.position;
			this.m_Demo.ButtonSelection = 0;
			this.m_Demo.WeaponHandler.SetWeapon(3);
			this.m_Demo.PlayerEventHandler.SetState("Freeze", false, true, false);
			this.m_Demo.PlayerEventHandler.SetState("SystemOFF", true, true, false);
			if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
			{
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapZoom();
			}
			this.m_Demo.Camera.SnapZoom();
			this.m_Demo.Camera.SnapSprings();
			this.m_Demo.Input.MouseCursorForced = true;
		}
		if (this.m_Demo.ButtonSelection != this.m_ExamplesCurrentSel)
		{
			vp_Utility.LockCursor = true;
			this.m_Demo.ResetState();
			this.m_Demo.PlayerEventHandler.Attack.Stop(0.5f);
			this.m_Demo.Camera.BobStepCallback = null;
			this.m_Demo.Camera.SnapSprings();
			if (this.m_ExamplesCurrentSel == 9 && this.m_Demo.WeaponHandler.CurrentWeapon != null)
			{
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapZoom();
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapSprings();
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapPivot();
			}
			switch (this.m_Demo.ButtonSelection)
			{
			case 0:
				this.m_Demo.PlayerEventHandler.Attack.Stop(10000000f);
				this.m_Demo.DrawCrosshair = true;
				this.m_Demo.Controller.Stop();
				if (this.m_Demo.WeaponHandler.CurrentWeaponIndex == 5)
				{
					this.m_Demo.WeaponHandler.SetWeapon(1);
					this.m_Demo.PlayerEventHandler.SetState("SystemOFF", true, true, false);
				}
				else
				{
					this.m_Demo.Camera.SnapZoom();
					this.m_Demo.PlayerEventHandler.SetState("SystemOFF", true, true, false);
					if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
					{
						this.m_Demo.WeaponHandler.CurrentWeapon.SnapSprings();
						((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapZoom();
					}
				}
				break;
			case 1:
				this.SetWeapon(3, "MafiaBoss", true, true);
				break;
			case 2:
				this.SetWeapon(1, "ModernShooter", true, true);
				break;
			case 3:
				this.SetWeapon(4, "Barbarian", true, true);
				break;
			case 4:
				this.SetWeapon(2, "SniperBreath", true, true);
				this.m_Demo.Controller.Stop();
				this.m_Demo.Teleport(this.m_SniperPos, this.m_SniperAngle);
				break;
			case 5:
				this.SetWeapon(0, "Astronaut", false, true);
				this.m_Demo.Controller.Stop();
				this.m_Demo.Teleport(this.m_AstronautPos, this.m_AstronautAngle);
				break;
			case 6:
				this.SetWeapon(5, "MechOrDino", true, false);
				this.m_UnFreezePosition = this.m_DrunkPos;
				this.m_Demo.Controller.Stop();
				this.m_Demo.Teleport(this.m_MechPos, this.m_MechAngle);
				this.m_Demo.Camera.BobStepCallback = delegate
				{
					this.m_Demo.Camera.AddForce2(new Vector3(0f, -1f, 0f));
					if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
					{
						((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).AddForce(new Vector3(0f, 0f, 0f), new Vector3(-0.3f, 0f, 0f));
					}
					this.m_AudioSource.pitch = Time.timeScale;
					this.m_AudioSource.PlayOneShot(this.m_StompSound);
				};
				break;
			case 7:
				this.SetWeapon(3, "TankTurret", true, false);
				this.m_Demo.FreezePlayer(this.m_OverviewPos, this.m_OverviewAngle);
				this.m_Demo.Controller.Stop();
				break;
			case 8:
				this.m_Demo.Controller.Stop();
				this.SetWeapon(0, "DrunkPerson", false, true);
				this.m_Demo.Controller.Stop();
				this.m_Demo.Teleport(this.m_DrunkPos, this.m_DrunkAngle);
				this.m_Demo.Camera.StopSprings();
				this.m_Demo.Camera.Refresh();
				break;
			case 9:
				this.SetWeapon(1, "OldSchool", true, true);
				this.m_Demo.Controller.Stop();
				this.m_Demo.Teleport(this.m_OldSchoolPos, this.m_OldSchoolAngle);
				this.m_Demo.Camera.SnapSprings();
				this.m_Demo.Camera.SnapZoom();
				vp_Timer.In(0.3f, delegate
				{
					if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
					{
						vp_Shooter componentInChildren = this.m_Demo.WeaponHandler.CurrentWeapon.GetComponentInChildren<vp_Shooter>();
						componentInChildren.MuzzleFlashPosition = new Vector3(0.0025736f, -0.0813138f, 1.662671f);
						componentInChildren.Refresh();
					}
				}, null);
				break;
			case 10:
				this.SetWeapon(2, "CrazyCowboy", true, true);
				this.m_Demo.Teleport(this.m_StartPos, this.m_StartAngle);
				this.m_Demo.Controller.Stop();
				break;
			}
			this.m_ExamplesCurrentSel = this.m_Demo.ButtonSelection;
		}
		if (this.m_Demo.ShowGUI)
		{
			this.m_ExamplesCurrentSel = this.m_Demo.ButtonSelection;
			string[] array = new string[]
			{
				"System OFF", "Mafia Boss", "Modern Shooter", "Barbarian", "Sniper Breath", "Astronaut", "Mech... or Dino?", "Tank Turret", "Drunk Person", "Old School",
				"Crazy Cowboy"
			};
			this.m_Demo.ButtonSelection = this.m_Demo.ToggleColumn(140, 150, this.m_Demo.ButtonSelection, array, false, true, this.m_ImageRightPointer, this.m_ImageLeftPointer);
		}
		if (this.m_Demo.ShowGUI && vp_Utility.LockCursor)
		{
			GUI.color = new Color(1f, 1f, 1f, this.m_Demo.ClosingDown ? this.m_Demo.GlobalAlpha : 1f);
			GUI.Label(new Rect((float)(Screen.width / 2 - 200), 140f, 400f, 20f), "(Press ENTER to reenable menu)", this.m_Demo.CenterStyle);
			GUI.color = new Color(1f, 1f, 1f, 1f * this.m_Demo.GlobalAlpha);
		}
	}

	// Token: 0x0600AC09 RID: 44041 RVA: 0x00401590 File Offset: 0x003FF790
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoForces()
	{
		this.m_Demo.DrawBoxes("external forces", "The camera and weapon are mounted on 8 positional and angular SPRINGS.\nEXTERNAL FORCES can be applied to these in various ways, creating unique movement patterns every time. This is useful for shockwaves, explosion knockback and earthquakes.", this.m_ImageLeftArrow, this.m_ImageRightArrow, null, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.DrawCrosshair = false;
			this.m_Demo.ResetState();
			this.m_Demo.Camera.Load(this.StompingCamera);
			this.m_Demo.Input.Load(this.StompingInput);
			this.m_Demo.WeaponHandler.SetWeapon(1);
			this.m_Demo.Controller.Load(this.SmackController);
			this.m_Demo.Camera.SnapZoom();
			this.m_Demo.FirstFrame = false;
			this.m_Demo.Teleport(this.m_ForcesPos, this.m_ForcesAngle);
			this.m_Demo.ButtonColumnArrowY = -100f;
			this.m_Demo.Input.MouseCursorForced = true;
		}
		if (this.m_Demo.ShowGUI)
		{
			this.m_Demo.ButtonSelection = -1;
			string[] array = new string[] { "Earthquake", "Boss Stomp", "Incoming Artillery", "Crashing Airplane" };
			this.m_Demo.ButtonSelection = this.m_Demo.ButtonColumn(150, this.m_Demo.ButtonSelection, array, this.m_ImageRightPointer);
			if (this.m_Demo.ButtonSelection != -1)
			{
				switch (this.m_Demo.ButtonSelection)
				{
				case 0:
					this.m_Demo.Camera.Load(this.StompingCamera);
					this.m_Demo.Input.Load(this.StompingInput);
					this.m_Demo.Controller.Load(this.SmackController);
					this.m_Demo.PlayerEventHandler.CameraEarthQuake.TryStart<Vector3>(new Vector3(0.2f, 0.2f, 10f));
					this.m_Demo.ButtonColumnArrowFadeoutTime = Time.time + 9f;
					this.m_AudioSource.Stop();
					this.m_AudioSource.pitch = Time.timeScale;
					this.m_AudioSource.PlayOneShot(this.m_EarthquakeSound);
					break;
				case 1:
					this.m_Demo.PlayerEventHandler.CameraEarthQuake.Stop(0f);
					this.m_Demo.Camera.Load(this.ArtilleryCamera);
					this.m_Demo.Input.Load(this.ArtilleryInput);
					this.m_Demo.Controller.Load(this.SmackController);
					this.m_Demo.PlayerEventHandler.CameraGroundStomp.Send(1f);
					this.m_Demo.ButtonColumnArrowFadeoutTime = Time.time;
					this.m_AudioSource.Stop();
					this.m_AudioSource.pitch = Time.timeScale;
					this.m_AudioSource.PlayOneShot(this.m_StompSound);
					break;
				case 2:
				{
					this.m_Demo.PlayerEventHandler.CameraEarthQuake.Stop(0f);
					this.m_Demo.Camera.Load(this.ArtilleryCamera);
					this.m_Demo.Input.Load(this.ArtilleryInput);
					this.m_Demo.Controller.Load(this.ArtilleryController);
					this.m_Demo.PlayerEventHandler.CameraBombShake.Send(1f);
					this.m_Demo.Controller.AddForce(global::UnityEngine.Random.Range(-1.5f, 1.5f), 0.5f, global::UnityEngine.Random.Range(-1.5f, -0.5f));
					this.m_Demo.ButtonColumnArrowFadeoutTime = Time.time + 1f;
					this.m_AudioSource.Stop();
					this.m_AudioSource.pitch = Time.timeScale;
					this.m_AudioSource.PlayOneShot(this.m_ExplosionSound);
					Vector3 vector = this.m_Demo.Controller.transform.TransformPoint(Vector3.forward * (float)global::UnityEngine.Random.Range(1, 2));
					vector.y = this.m_Demo.Controller.transform.position.y + 1f;
					global::UnityEngine.Object.Instantiate<GameObject>(this.m_ArtilleryFX, vector, Quaternion.identity);
					break;
				}
				case 3:
					this.m_Demo.Camera.Load(this.StompingCamera);
					this.m_Demo.Input.Load(this.StompingInput);
					this.m_Demo.Controller.Load(this.SmackController);
					this.m_Demo.PlayerEventHandler.CameraEarthQuake.TryStart<Vector3>(new Vector3(0.25f, 0.2f, 10f));
					this.m_Demo.ButtonColumnArrowFadeoutTime = Time.time + 9f;
					this.m_AudioSource.Stop();
					this.m_AudioSource.pitch = Time.timeScale;
					this.m_AudioSource.PlayOneShot(this.m_EarthquakeSound);
					this.m_Demo.Camera.RenderingFieldOfView = 80f;
					this.m_Demo.Camera.RotationEarthQuakeFactor = 6.5f;
					this.m_Demo.Camera.Zoom();
					vp_Timer.In(9f, delegate
					{
						this.m_Demo.Camera.RenderingFieldOfView = 60f;
						this.m_Demo.Camera.RotationEarthQuakeFactor = 0f;
						this.m_Demo.Camera.Zoom();
					}, this.m_ChrashingAirplaneRestoreTimer);
					break;
				}
				this.m_Demo.LastInputTime = Time.time;
			}
			this.m_Demo.DrawEditorPreview(this.m_ImageWeaponPosition, this.m_ImageEditorPreview, this.m_ImageEditorScreenshot);
		}
	}

	// Token: 0x0600AC0A RID: 44042 RVA: 0x00401B3C File Offset: 0x003FFD3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoMouseInput()
	{
		this.m_Demo.DrawBoxes("mouse input", "Any good FPS should offer configurable MOUSE SMOOTHING and ACCELERATION.\n• Smoothing interpolates mouse input over several frames to reduce jittering.\n • Acceleration + low mouse sensitivity allows high precision without loss of turn speed.\n• Click the below buttons to compare some example setups.", this.m_ImageLeftArrow, this.m_ImageRightArrow, null, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.ResetState();
			this.m_AudioSource.Stop();
			this.m_Demo.DrawCrosshair = true;
			this.m_Demo.FreezePlayer(this.m_MouseLookPos, this.m_MouseLookAngle);
			this.m_Demo.FirstFrame = false;
			this.m_Demo.WeaponHandler.SetWeapon(0);
			this.m_Demo.Input.MouseCursorForced = true;
			this.m_Demo.Camera.Load(this.MouseRawUnityCamera);
			this.m_Demo.Input.Load(this.MouseRawUnityInput);
		}
		if (this.m_Demo.ShowGUI)
		{
			int buttonSelection = this.m_Demo.ButtonSelection;
			bool flag = this.m_Demo.ButtonSelection != 2;
			string[] array = new string[] { "Raw Mouse Input", "Mouse Smoothing", "Low Sens. + Acceleration" };
			this.m_Demo.ButtonSelection = this.m_Demo.ToggleColumn(200, 150, this.m_Demo.ButtonSelection, array, true, flag, this.m_ImageRightPointer, this.m_ImageLeftPointer);
			if (this.m_Demo.ButtonSelection != buttonSelection)
			{
				switch (this.m_Demo.ButtonSelection)
				{
				case 0:
					this.m_Demo.PlayerEventHandler.ResetActivityStates();
					this.m_Demo.Camera.Load(this.MouseRawUnityCamera);
					this.m_Demo.Input.Load(this.MouseRawUnityInput);
					break;
				case 1:
					this.m_Demo.PlayerEventHandler.ResetActivityStates();
					this.m_Demo.Camera.Load(this.MouseSmoothingCamera);
					this.m_Demo.Input.Load(this.MouseSmoothingInput);
					break;
				case 2:
					this.m_Demo.PlayerEventHandler.ResetActivityStates();
					this.m_Demo.Camera.Load(this.MouseLowSensCamera);
					this.m_Demo.Input.Load(this.MouseLowSensInput);
					break;
				}
				this.m_Demo.LastInputTime = Time.time;
			}
			flag = true;
			if (this.m_Demo.ButtonSelection != 2)
			{
				GUI.enabled = false;
				flag = false;
			}
			this.m_Demo.Input.MouseLookAcceleration = this.m_Demo.ButtonToggle(new Rect((float)(Screen.width / 2 + 110), 215f, 90f, 40f), "Acceleration", this.m_Demo.Input.MouseLookAcceleration, flag, this.m_ImageUpPointer);
			GUI.color = new Color(1f, 1f, 1f, 1f * this.m_Demo.GlobalAlpha);
			GUI.enabled = true;
			this.m_Demo.DrawEditorPreview(this.m_ImageCameraMouse, this.m_ImageEditorPreview, this.m_ImageEditorScreenshot);
		}
	}

	// Token: 0x0600AC0B RID: 44043 RVA: 0x00401E58 File Offset: 0x00400058
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoWeaponPerspective()
	{
		this.m_Demo.DrawBoxes("weapon perspective", "Proper WEAPON PERSPECTIVE is crucial to the final impression of your game!\nThe weapon has its own separate Field of View for full perspective control,\nalong with dynamic position and rotation offset.", this.m_ImageLeftArrow, this.m_ImageRightArrow, null, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.ResetState();
			this.m_Demo.Camera.Load(this.PerspOldCamera);
			this.m_Demo.Input.Load(this.PerspOldInput);
			this.m_Demo.Camera.SnapZoom();
			this.m_Demo.FirstFrame = false;
			this.m_Demo.FreezePlayer(this.m_OverviewPos, this.m_PerspectiveAngle, true);
			this.m_Demo.Input.MouseCursorForced = true;
			this.m_Demo.WeaponHandler.SetWeapon(3);
			this.m_Demo.SetWeaponPreset(this.PerspOldWeapon, null, true);
			if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
			{
				this.m_Demo.WeaponHandler.CurrentWeapon.SetState("WeaponPersp", true, false, false);
			}
			this.m_Demo.WeaponHandler.SetWeaponLayer(10);
			if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
			{
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapZoom();
				this.m_Demo.WeaponHandler.CurrentWeapon.SnapSprings();
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SnapPivot();
			}
		}
		if (this.m_Demo.ShowGUI)
		{
			int buttonSelection = this.m_Demo.ButtonSelection;
			string[] array = new string[] { "Old School", "1999 Internet Café", "Modern Shooter" };
			this.m_Demo.ButtonSelection = this.m_Demo.ToggleColumn(200, 150, this.m_Demo.ButtonSelection, array, true, true, this.m_ImageRightPointer, this.m_ImageLeftPointer);
			if (this.m_Demo.ButtonSelection != buttonSelection)
			{
				switch (this.m_Demo.ButtonSelection)
				{
				case 0:
					this.m_Demo.SetWeaponPreset(this.PerspOldWeapon, null, true);
					break;
				case 1:
					this.m_Demo.SetWeaponPreset(this.Persp1999Weapon, null, true);
					break;
				case 2:
					this.m_Demo.SetWeaponPreset(this.PerspModernWeapon, null, true);
					break;
				}
				this.m_Demo.LastInputTime = Time.time;
			}
			this.m_Demo.DrawEditorPreview(this.m_ImageWeaponPerspective, this.m_ImageEditorPreview, this.m_ImageEditorScreenshot);
		}
	}

	// Token: 0x0600AC0C RID: 44044 RVA: 0x004020F0 File Offset: 0x004002F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoWeaponLayer()
	{
		this.m_Demo.DrawBoxes("weapon camera", "\nThe weapon can be rendered by a SEPARATE CAMERA so that it never sticks through walls or other geometry. Try toggling the weapon camera ON and OFF below.", this.m_ImageLeftArrow, this.m_ImageRightArrow, null, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.ResetState();
			this.m_Demo.DrawCrosshair = true;
			this.m_Demo.Camera.Load(this.WallFacingCamera);
			this.m_Demo.Input.Load(this.WallFacingInput);
			this.m_Demo.WeaponHandler.SetWeapon(3);
			this.m_Demo.SetWeaponPreset(this.WallFacingWeapon, null, true);
			this.m_Demo.Camera.SnapZoom();
			this.m_WeaponLayerToggle = false;
			this.m_Demo.FirstFrame = false;
			this.m_Demo.FreezePlayer(this.m_WeaponLayerPos, this.m_WeaponLayerAngle);
			int num = (this.m_WeaponLayerToggle ? 10 : 0);
			this.m_Demo.WeaponHandler.SetWeaponLayer(num);
			this.m_Demo.Input.MouseCursorForced = true;
		}
		if (this.m_Demo.ShowGUI)
		{
			bool weaponLayerToggle = this.m_WeaponLayerToggle;
			this.m_WeaponLayerToggle = this.m_Demo.ButtonToggle(new Rect((float)(Screen.width / 2 - 45), 180f, 100f, 40f), "Weapon Camera", this.m_WeaponLayerToggle, true, this.m_ImageUpPointer);
			if (weaponLayerToggle != this.m_WeaponLayerToggle)
			{
				this.m_Demo.FreezePlayer(this.m_WeaponLayerPos, this.m_WeaponLayerAngle);
				int num2 = (this.m_WeaponLayerToggle ? 10 : 0);
				this.m_Demo.WeaponHandler.SetWeaponLayer(num2);
				this.m_Demo.LastInputTime = Time.time;
			}
		}
	}

	// Token: 0x0600AC0D RID: 44045 RVA: 0x004022AC File Offset: 0x004004AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DemoPivot()
	{
		this.m_Demo.DrawBoxes("weapon pivot", "The PIVOT POINT of the weapon model greatly affects movement pattern.\nManipulating it at runtime can be quite useful, and easy with Ultimate FPS!\nClick the examples below and move the camera around.", this.m_ImageLeftArrow, this.m_ImageRightArrow, delegate
		{
			this.m_Demo.LoadLevel(2);
		}, null, true);
		if (this.m_Demo.FirstFrame)
		{
			this.m_Demo.ResetState();
			this.m_Demo.DrawCrosshair = false;
			this.m_Demo.Camera.Load(this.DefaultCamera);
			this.m_Demo.Input.Load(this.DefaultInput);
			this.m_Demo.Controller.Load(this.ImmobileController);
			this.m_Demo.FirstFrame = false;
			this.m_Demo.FreezePlayer(this.m_OverviewPos, this.m_OverviewAngle);
			this.m_Demo.WeaponHandler.SetWeapon(1);
			this.m_Demo.SetWeaponPreset(this.DefaultWeapon, null, true);
			this.m_Demo.SetWeaponPreset(this.PivotMuzzleWeapon, null, true);
			if (this.m_Demo.WeaponHandler.CurrentWeapon != null)
			{
				((vp_FPWeapon)this.m_Demo.WeaponHandler.CurrentWeapon).SetPivotVisible(true);
			}
			this.m_Demo.Input.MouseCursorForced = true;
			this.m_Demo.WeaponHandler.SetWeaponLayer(10);
		}
		if (this.m_Demo.ShowGUI)
		{
			int buttonSelection = this.m_Demo.ButtonSelection;
			string[] array = new string[] { "Muzzle", "Grip", "Chest", "Elbow (Uzi Style)" };
			this.m_Demo.ButtonSelection = this.m_Demo.ToggleColumn(200, 150, this.m_Demo.ButtonSelection, array, true, true, this.m_ImageRightPointer, this.m_ImageLeftPointer);
			if (this.m_Demo.ButtonSelection != buttonSelection)
			{
				switch (this.m_Demo.ButtonSelection)
				{
				case 0:
					this.m_Demo.SetWeaponPreset(this.PivotMuzzleWeapon, null, true);
					break;
				case 1:
					this.m_Demo.SetWeaponPreset(this.PivotWristWeapon, null, true);
					break;
				case 2:
					this.m_Demo.SetWeaponPreset(this.PivotChestWeapon, null, true);
					break;
				case 3:
					this.m_Demo.SetWeaponPreset(this.PivotElbowWeapon, null, true);
					break;
				}
				this.m_Demo.LastInputTime = Time.time;
			}
			this.m_Demo.DrawEditorPreview(this.m_ImageWeaponPivot, this.m_ImageEditorPreview, this.m_ImageEditorScreenshot);
		}
	}

	// Token: 0x0600AC0E RID: 44046 RVA: 0x00402538 File Offset: 0x00400738
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGUI()
	{
		this.m_Demo.OnGUI();
		switch (this.m_Demo.CurrentScreen)
		{
		case 1:
			this.DemoIntro();
			return;
		case 2:
			this.DemoExamples();
			return;
		case 3:
			this.DemoForces();
			return;
		case 4:
			this.DemoMouseInput();
			return;
		case 5:
			this.DemoWeaponPerspective();
			return;
		case 6:
			this.DemoWeaponLayer();
			return;
		case 7:
			this.DemoPivot();
			return;
		default:
			return;
		}
	}

	// Token: 0x04007F1A RID: 32538
	public GameObject Player;

	// Token: 0x04007F1B RID: 32539
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPSDemoManager m_Demo;

	// Token: 0x04007F1C RID: 32540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_ExamplesCurrentSel;

	// Token: 0x04007F1D RID: 32541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle m_ChrashingAirplaneRestoreTimer = new vp_Timer.Handle();

	// Token: 0x04007F1E RID: 32542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle m_WeaponSwitchTimer = new vp_Timer.Handle();

	// Token: 0x04007F1F RID: 32543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WeaponLayerToggle;

	// Token: 0x04007F20 RID: 32544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_MouseLookPos = new Vector3(-8.093015f, 20.08f, 3.416737f);

	// Token: 0x04007F21 RID: 32545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_OverviewPos = new Vector3(1.246535f, 32.08f, 21.43753f);

	// Token: 0x04007F22 RID: 32546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_StartPos = new Vector3(-18.14881f, 20.08f, -24.16859f);

	// Token: 0x04007F23 RID: 32547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_WeaponLayerPos = new Vector3(-19.43989f, 16.08f, 2.10474f);

	// Token: 0x04007F24 RID: 32548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_ForcesPos = new Vector3(-8.093015f, 20.08f, 3.416737f);

	// Token: 0x04007F25 RID: 32549
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_MechPos = new Vector3(0.02941191f, 1.08f, -93.50691f);

	// Token: 0x04007F26 RID: 32550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_DrunkPos = new Vector3(18.48685f, 21.08f, 24.05441f);

	// Token: 0x04007F27 RID: 32551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_SniperPos = new Vector3(0.8841875f, 33.08f, 21.3446f);

	// Token: 0x04007F28 RID: 32552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_OldSchoolPos = new Vector3(25.88745f, 0.08f, 23.08822f);

	// Token: 0x04007F29 RID: 32553
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_AstronautPos = new Vector3(20f, 20f, 16f);

	// Token: 0x04007F2A RID: 32554
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_UnFreezePosition = Vector3.zero;

	// Token: 0x04007F2B RID: 32555
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_MouseLookAngle = new Vector2(0f, 33.10683f);

	// Token: 0x04007F2C RID: 32556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_OverviewAngle = new Vector2(28.89369f, 224f);

	// Token: 0x04007F2D RID: 32557
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_PerspectiveAngle = new Vector2(27f, 223f);

	// Token: 0x04007F2E RID: 32558
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_StartAngle = new Vector2(0f, 0f);

	// Token: 0x04007F2F RID: 32559
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_WeaponLayerAngle = new Vector2(0f, -90f);

	// Token: 0x04007F30 RID: 32560
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_ForcesAngle = new Vector2(0f, 0f);

	// Token: 0x04007F31 RID: 32561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_MechAngle = new Vector3(0f, 180f);

	// Token: 0x04007F32 RID: 32562
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_DrunkAngle = new Vector3(0f, -90f);

	// Token: 0x04007F33 RID: 32563
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_SniperAngle = new Vector2(20f, 180f);

	// Token: 0x04007F34 RID: 32564
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_OldSchoolAngle = new Vector2(0f, 180f);

	// Token: 0x04007F35 RID: 32565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_AstronautAngle = new Vector2(0f, 269.5f);

	// Token: 0x04007F36 RID: 32566
	public Texture m_ImageEditorPreview;

	// Token: 0x04007F37 RID: 32567
	public Texture m_ImageEditorPreviewShow;

	// Token: 0x04007F38 RID: 32568
	public Texture m_ImageCameraMouse;

	// Token: 0x04007F39 RID: 32569
	public Texture m_ImageWeaponPosition;

	// Token: 0x04007F3A RID: 32570
	public Texture m_ImageWeaponPerspective;

	// Token: 0x04007F3B RID: 32571
	public Texture m_ImageWeaponPivot;

	// Token: 0x04007F3C RID: 32572
	public Texture m_ImageEditorScreenshot;

	// Token: 0x04007F3D RID: 32573
	public Texture m_ImageLeftArrow;

	// Token: 0x04007F3E RID: 32574
	public Texture m_ImageRightArrow;

	// Token: 0x04007F3F RID: 32575
	public Texture m_ImageCheckmark;

	// Token: 0x04007F40 RID: 32576
	public Texture m_ImageLeftPointer;

	// Token: 0x04007F41 RID: 32577
	public Texture m_ImageRightPointer;

	// Token: 0x04007F42 RID: 32578
	public Texture m_ImageUpPointer;

	// Token: 0x04007F43 RID: 32579
	public Texture m_ImageCrosshair;

	// Token: 0x04007F44 RID: 32580
	public Texture m_ImageFullScreen;

	// Token: 0x04007F45 RID: 32581
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_AudioSource;

	// Token: 0x04007F46 RID: 32582
	public AudioClip m_StompSound;

	// Token: 0x04007F47 RID: 32583
	public AudioClip m_EarthquakeSound;

	// Token: 0x04007F48 RID: 32584
	public AudioClip m_ExplosionSound;

	// Token: 0x04007F49 RID: 32585
	public GameObject m_ArtilleryFX;

	// Token: 0x04007F4A RID: 32586
	public TextAsset ArtilleryCamera;

	// Token: 0x04007F4B RID: 32587
	public TextAsset ArtilleryController;

	// Token: 0x04007F4C RID: 32588
	public TextAsset ArtilleryInput;

	// Token: 0x04007F4D RID: 32589
	public TextAsset AstronautCamera;

	// Token: 0x04007F4E RID: 32590
	public TextAsset AstronautController;

	// Token: 0x04007F4F RID: 32591
	public TextAsset AstronautInput;

	// Token: 0x04007F50 RID: 32592
	public TextAsset CowboyCamera;

	// Token: 0x04007F51 RID: 32593
	public TextAsset CowboyController;

	// Token: 0x04007F52 RID: 32594
	public TextAsset CowboyWeapon;

	// Token: 0x04007F53 RID: 32595
	public TextAsset CowboyShooter;

	// Token: 0x04007F54 RID: 32596
	public TextAsset CowboyInput;

	// Token: 0x04007F55 RID: 32597
	public TextAsset CrouchController;

	// Token: 0x04007F56 RID: 32598
	public TextAsset CrouchInput;

	// Token: 0x04007F57 RID: 32599
	public TextAsset DefaultCamera;

	// Token: 0x04007F58 RID: 32600
	public TextAsset DefaultWeapon;

	// Token: 0x04007F59 RID: 32601
	public TextAsset DefaultInput;

	// Token: 0x04007F5A RID: 32602
	public TextAsset DrunkCamera;

	// Token: 0x04007F5B RID: 32603
	public TextAsset DrunkController;

	// Token: 0x04007F5C RID: 32604
	public TextAsset DrunkInput;

	// Token: 0x04007F5D RID: 32605
	public TextAsset ImmobileCamera;

	// Token: 0x04007F5E RID: 32606
	public TextAsset ImmobileController;

	// Token: 0x04007F5F RID: 32607
	public TextAsset ImmobileInput;

	// Token: 0x04007F60 RID: 32608
	public TextAsset MaceCamera;

	// Token: 0x04007F61 RID: 32609
	public TextAsset MaceWeapon;

	// Token: 0x04007F62 RID: 32610
	public TextAsset MaceInput;

	// Token: 0x04007F63 RID: 32611
	public TextAsset MafiaCamera;

	// Token: 0x04007F64 RID: 32612
	public TextAsset MafiaWeapon;

	// Token: 0x04007F65 RID: 32613
	public TextAsset MafiaShooter;

	// Token: 0x04007F66 RID: 32614
	public TextAsset MafiaInput;

	// Token: 0x04007F67 RID: 32615
	public TextAsset MechCamera;

	// Token: 0x04007F68 RID: 32616
	public TextAsset MechController;

	// Token: 0x04007F69 RID: 32617
	public TextAsset MechWeapon;

	// Token: 0x04007F6A RID: 32618
	public TextAsset MechShooter;

	// Token: 0x04007F6B RID: 32619
	public TextAsset MechInput;

	// Token: 0x04007F6C RID: 32620
	public TextAsset ModernCamera;

	// Token: 0x04007F6D RID: 32621
	public TextAsset ModernController;

	// Token: 0x04007F6E RID: 32622
	public TextAsset ModernWeapon;

	// Token: 0x04007F6F RID: 32623
	public TextAsset ModernShooter;

	// Token: 0x04007F70 RID: 32624
	public TextAsset ModernInput;

	// Token: 0x04007F71 RID: 32625
	public TextAsset MouseLowSensCamera;

	// Token: 0x04007F72 RID: 32626
	public TextAsset MouseLowSensInput;

	// Token: 0x04007F73 RID: 32627
	public TextAsset MouseRawUnityCamera;

	// Token: 0x04007F74 RID: 32628
	public TextAsset MouseRawUnityInput;

	// Token: 0x04007F75 RID: 32629
	public TextAsset MouseSmoothingCamera;

	// Token: 0x04007F76 RID: 32630
	public TextAsset MouseSmoothingInput;

	// Token: 0x04007F77 RID: 32631
	public TextAsset OldSchoolCamera;

	// Token: 0x04007F78 RID: 32632
	public TextAsset OldSchoolController;

	// Token: 0x04007F79 RID: 32633
	public TextAsset OldSchoolWeapon;

	// Token: 0x04007F7A RID: 32634
	public TextAsset OldSchoolShooter;

	// Token: 0x04007F7B RID: 32635
	public TextAsset OldSchoolInput;

	// Token: 0x04007F7C RID: 32636
	public TextAsset Persp1999Camera;

	// Token: 0x04007F7D RID: 32637
	public TextAsset Persp1999Weapon;

	// Token: 0x04007F7E RID: 32638
	public TextAsset Persp1999Input;

	// Token: 0x04007F7F RID: 32639
	public TextAsset PerspModernCamera;

	// Token: 0x04007F80 RID: 32640
	public TextAsset PerspModernWeapon;

	// Token: 0x04007F81 RID: 32641
	public TextAsset PerspModernInput;

	// Token: 0x04007F82 RID: 32642
	public TextAsset PerspOldCamera;

	// Token: 0x04007F83 RID: 32643
	public TextAsset PerspOldWeapon;

	// Token: 0x04007F84 RID: 32644
	public TextAsset PerspOldInput;

	// Token: 0x04007F85 RID: 32645
	public TextAsset PivotChestWeapon;

	// Token: 0x04007F86 RID: 32646
	public TextAsset PivotElbowWeapon;

	// Token: 0x04007F87 RID: 32647
	public TextAsset PivotMuzzleWeapon;

	// Token: 0x04007F88 RID: 32648
	public TextAsset PivotWristWeapon;

	// Token: 0x04007F89 RID: 32649
	public TextAsset SmackController;

	// Token: 0x04007F8A RID: 32650
	public TextAsset SniperCamera;

	// Token: 0x04007F8B RID: 32651
	public TextAsset SniperWeapon;

	// Token: 0x04007F8C RID: 32652
	public TextAsset SniperShooter;

	// Token: 0x04007F8D RID: 32653
	public TextAsset SniperInput;

	// Token: 0x04007F8E RID: 32654
	public TextAsset StompingCamera;

	// Token: 0x04007F8F RID: 32655
	public TextAsset StompingInput;

	// Token: 0x04007F90 RID: 32656
	public TextAsset SystemOFFCamera;

	// Token: 0x04007F91 RID: 32657
	public TextAsset SystemOFFController;

	// Token: 0x04007F92 RID: 32658
	public TextAsset SystemOFFShooter;

	// Token: 0x04007F93 RID: 32659
	public TextAsset SystemOFFWeapon;

	// Token: 0x04007F94 RID: 32660
	public TextAsset SystemOFFWeaponGlideIn;

	// Token: 0x04007F95 RID: 32661
	public TextAsset SystemOFFInput;

	// Token: 0x04007F96 RID: 32662
	public TextAsset TurretCamera;

	// Token: 0x04007F97 RID: 32663
	public TextAsset TurretWeapon;

	// Token: 0x04007F98 RID: 32664
	public TextAsset TurretShooter;

	// Token: 0x04007F99 RID: 32665
	public TextAsset TurretInput;

	// Token: 0x04007F9A RID: 32666
	public TextAsset WallFacingCamera;

	// Token: 0x04007F9B RID: 32667
	public TextAsset WallFacingWeapon;

	// Token: 0x04007F9C RID: 32668
	public TextAsset WallFacingInput;
}
