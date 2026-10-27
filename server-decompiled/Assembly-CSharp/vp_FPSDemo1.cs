using System;
using UnityEngine;

// Token: 0x020015CE RID: 5582
public class vp_FPSDemo1 : MonoBehaviour
{
	// Token: 0x0600AE0B RID: 44555 RVA: 0x0040CE08 File Offset: 0x0040B008
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

	// Token: 0x0600AE0C RID: 44556 RVA: 0x0040CFFD File Offset: 0x0040B1FD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		if (this.m_Demo.PlayerEventHandler != null)
		{
			this.m_Demo.PlayerEventHandler.Unregister(this);
		}
	}

	// Token: 0x0600AE0D RID: 44557 RVA: 0x0040D024 File Offset: 0x0040B224
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

	// Token: 0x0600AE0E RID: 44558 RVA: 0x0040D214 File Offset: 0x0040B414
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

	// Token: 0x0600AE0F RID: 44559 RVA: 0x0040D308 File Offset: 0x0040B508
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

	// Token: 0x0600AE10 RID: 44560 RVA: 0x0040D408 File Offset: 0x0040B608
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

	// Token: 0x0600AE11 RID: 44561 RVA: 0x0040DB28 File Offset: 0x0040BD28
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

	// Token: 0x0600AE12 RID: 44562 RVA: 0x0040E0D4 File Offset: 0x0040C2D4
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

	// Token: 0x0600AE13 RID: 44563 RVA: 0x0040E3F0 File Offset: 0x0040C5F0
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

	// Token: 0x0600AE14 RID: 44564 RVA: 0x0040E688 File Offset: 0x0040C888
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

	// Token: 0x0600AE15 RID: 44565 RVA: 0x0040E844 File Offset: 0x0040CA44
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

	// Token: 0x0600AE16 RID: 44566 RVA: 0x0040EAD0 File Offset: 0x0040CCD0
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

	// Token: 0x040080B7 RID: 32951
	public GameObject Player;

	// Token: 0x040080B8 RID: 32952
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPSDemoManager m_Demo;

	// Token: 0x040080B9 RID: 32953
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int m_ExamplesCurrentSel;

	// Token: 0x040080BA RID: 32954
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle m_ChrashingAirplaneRestoreTimer = new vp_Timer.Handle();

	// Token: 0x040080BB RID: 32955
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle m_WeaponSwitchTimer = new vp_Timer.Handle();

	// Token: 0x040080BC RID: 32956
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WeaponLayerToggle;

	// Token: 0x040080BD RID: 32957
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_MouseLookPos = new Vector3(-8.093015f, 20.08f, 3.416737f);

	// Token: 0x040080BE RID: 32958
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_OverviewPos = new Vector3(1.246535f, 32.08f, 21.43753f);

	// Token: 0x040080BF RID: 32959
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_StartPos = new Vector3(-18.14881f, 20.08f, -24.16859f);

	// Token: 0x040080C0 RID: 32960
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_WeaponLayerPos = new Vector3(-19.43989f, 16.08f, 2.10474f);

	// Token: 0x040080C1 RID: 32961
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_ForcesPos = new Vector3(-8.093015f, 20.08f, 3.416737f);

	// Token: 0x040080C2 RID: 32962
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_MechPos = new Vector3(0.02941191f, 1.08f, -93.50691f);

	// Token: 0x040080C3 RID: 32963
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_DrunkPos = new Vector3(18.48685f, 21.08f, 24.05441f);

	// Token: 0x040080C4 RID: 32964
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_SniperPos = new Vector3(0.8841875f, 33.08f, 21.3446f);

	// Token: 0x040080C5 RID: 32965
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_OldSchoolPos = new Vector3(25.88745f, 0.08f, 23.08822f);

	// Token: 0x040080C6 RID: 32966
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 m_AstronautPos = new Vector3(20f, 20f, 16f);

	// Token: 0x040080C7 RID: 32967
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_UnFreezePosition = Vector3.zero;

	// Token: 0x040080C8 RID: 32968
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_MouseLookAngle = new Vector2(0f, 33.10683f);

	// Token: 0x040080C9 RID: 32969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_OverviewAngle = new Vector2(28.89369f, 224f);

	// Token: 0x040080CA RID: 32970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_PerspectiveAngle = new Vector2(27f, 223f);

	// Token: 0x040080CB RID: 32971
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_StartAngle = new Vector2(0f, 0f);

	// Token: 0x040080CC RID: 32972
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_WeaponLayerAngle = new Vector2(0f, -90f);

	// Token: 0x040080CD RID: 32973
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_ForcesAngle = new Vector2(0f, 0f);

	// Token: 0x040080CE RID: 32974
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_MechAngle = new Vector3(0f, 180f);

	// Token: 0x040080CF RID: 32975
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_DrunkAngle = new Vector3(0f, -90f);

	// Token: 0x040080D0 RID: 32976
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_SniperAngle = new Vector2(20f, 180f);

	// Token: 0x040080D1 RID: 32977
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_OldSchoolAngle = new Vector2(0f, 180f);

	// Token: 0x040080D2 RID: 32978
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 m_AstronautAngle = new Vector2(0f, 269.5f);

	// Token: 0x040080D3 RID: 32979
	public Texture m_ImageEditorPreview;

	// Token: 0x040080D4 RID: 32980
	public Texture m_ImageEditorPreviewShow;

	// Token: 0x040080D5 RID: 32981
	public Texture m_ImageCameraMouse;

	// Token: 0x040080D6 RID: 32982
	public Texture m_ImageWeaponPosition;

	// Token: 0x040080D7 RID: 32983
	public Texture m_ImageWeaponPerspective;

	// Token: 0x040080D8 RID: 32984
	public Texture m_ImageWeaponPivot;

	// Token: 0x040080D9 RID: 32985
	public Texture m_ImageEditorScreenshot;

	// Token: 0x040080DA RID: 32986
	public Texture m_ImageLeftArrow;

	// Token: 0x040080DB RID: 32987
	public Texture m_ImageRightArrow;

	// Token: 0x040080DC RID: 32988
	public Texture m_ImageCheckmark;

	// Token: 0x040080DD RID: 32989
	public Texture m_ImageLeftPointer;

	// Token: 0x040080DE RID: 32990
	public Texture m_ImageRightPointer;

	// Token: 0x040080DF RID: 32991
	public Texture m_ImageUpPointer;

	// Token: 0x040080E0 RID: 32992
	public Texture m_ImageCrosshair;

	// Token: 0x040080E1 RID: 32993
	public Texture m_ImageFullScreen;

	// Token: 0x040080E2 RID: 32994
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_AudioSource;

	// Token: 0x040080E3 RID: 32995
	public AudioClip m_StompSound;

	// Token: 0x040080E4 RID: 32996
	public AudioClip m_EarthquakeSound;

	// Token: 0x040080E5 RID: 32997
	public AudioClip m_ExplosionSound;

	// Token: 0x040080E6 RID: 32998
	public GameObject m_ArtilleryFX;

	// Token: 0x040080E7 RID: 32999
	public TextAsset ArtilleryCamera;

	// Token: 0x040080E8 RID: 33000
	public TextAsset ArtilleryController;

	// Token: 0x040080E9 RID: 33001
	public TextAsset ArtilleryInput;

	// Token: 0x040080EA RID: 33002
	public TextAsset AstronautCamera;

	// Token: 0x040080EB RID: 33003
	public TextAsset AstronautController;

	// Token: 0x040080EC RID: 33004
	public TextAsset AstronautInput;

	// Token: 0x040080ED RID: 33005
	public TextAsset CowboyCamera;

	// Token: 0x040080EE RID: 33006
	public TextAsset CowboyController;

	// Token: 0x040080EF RID: 33007
	public TextAsset CowboyWeapon;

	// Token: 0x040080F0 RID: 33008
	public TextAsset CowboyShooter;

	// Token: 0x040080F1 RID: 33009
	public TextAsset CowboyInput;

	// Token: 0x040080F2 RID: 33010
	public TextAsset CrouchController;

	// Token: 0x040080F3 RID: 33011
	public TextAsset CrouchInput;

	// Token: 0x040080F4 RID: 33012
	public TextAsset DefaultCamera;

	// Token: 0x040080F5 RID: 33013
	public TextAsset DefaultWeapon;

	// Token: 0x040080F6 RID: 33014
	public TextAsset DefaultInput;

	// Token: 0x040080F7 RID: 33015
	public TextAsset DrunkCamera;

	// Token: 0x040080F8 RID: 33016
	public TextAsset DrunkController;

	// Token: 0x040080F9 RID: 33017
	public TextAsset DrunkInput;

	// Token: 0x040080FA RID: 33018
	public TextAsset ImmobileCamera;

	// Token: 0x040080FB RID: 33019
	public TextAsset ImmobileController;

	// Token: 0x040080FC RID: 33020
	public TextAsset ImmobileInput;

	// Token: 0x040080FD RID: 33021
	public TextAsset MaceCamera;

	// Token: 0x040080FE RID: 33022
	public TextAsset MaceWeapon;

	// Token: 0x040080FF RID: 33023
	public TextAsset MaceInput;

	// Token: 0x04008100 RID: 33024
	public TextAsset MafiaCamera;

	// Token: 0x04008101 RID: 33025
	public TextAsset MafiaWeapon;

	// Token: 0x04008102 RID: 33026
	public TextAsset MafiaShooter;

	// Token: 0x04008103 RID: 33027
	public TextAsset MafiaInput;

	// Token: 0x04008104 RID: 33028
	public TextAsset MechCamera;

	// Token: 0x04008105 RID: 33029
	public TextAsset MechController;

	// Token: 0x04008106 RID: 33030
	public TextAsset MechWeapon;

	// Token: 0x04008107 RID: 33031
	public TextAsset MechShooter;

	// Token: 0x04008108 RID: 33032
	public TextAsset MechInput;

	// Token: 0x04008109 RID: 33033
	public TextAsset ModernCamera;

	// Token: 0x0400810A RID: 33034
	public TextAsset ModernController;

	// Token: 0x0400810B RID: 33035
	public TextAsset ModernWeapon;

	// Token: 0x0400810C RID: 33036
	public TextAsset ModernShooter;

	// Token: 0x0400810D RID: 33037
	public TextAsset ModernInput;

	// Token: 0x0400810E RID: 33038
	public TextAsset MouseLowSensCamera;

	// Token: 0x0400810F RID: 33039
	public TextAsset MouseLowSensInput;

	// Token: 0x04008110 RID: 33040
	public TextAsset MouseRawUnityCamera;

	// Token: 0x04008111 RID: 33041
	public TextAsset MouseRawUnityInput;

	// Token: 0x04008112 RID: 33042
	public TextAsset MouseSmoothingCamera;

	// Token: 0x04008113 RID: 33043
	public TextAsset MouseSmoothingInput;

	// Token: 0x04008114 RID: 33044
	public TextAsset OldSchoolCamera;

	// Token: 0x04008115 RID: 33045
	public TextAsset OldSchoolController;

	// Token: 0x04008116 RID: 33046
	public TextAsset OldSchoolWeapon;

	// Token: 0x04008117 RID: 33047
	public TextAsset OldSchoolShooter;

	// Token: 0x04008118 RID: 33048
	public TextAsset OldSchoolInput;

	// Token: 0x04008119 RID: 33049
	public TextAsset Persp1999Camera;

	// Token: 0x0400811A RID: 33050
	public TextAsset Persp1999Weapon;

	// Token: 0x0400811B RID: 33051
	public TextAsset Persp1999Input;

	// Token: 0x0400811C RID: 33052
	public TextAsset PerspModernCamera;

	// Token: 0x0400811D RID: 33053
	public TextAsset PerspModernWeapon;

	// Token: 0x0400811E RID: 33054
	public TextAsset PerspModernInput;

	// Token: 0x0400811F RID: 33055
	public TextAsset PerspOldCamera;

	// Token: 0x04008120 RID: 33056
	public TextAsset PerspOldWeapon;

	// Token: 0x04008121 RID: 33057
	public TextAsset PerspOldInput;

	// Token: 0x04008122 RID: 33058
	public TextAsset PivotChestWeapon;

	// Token: 0x04008123 RID: 33059
	public TextAsset PivotElbowWeapon;

	// Token: 0x04008124 RID: 33060
	public TextAsset PivotMuzzleWeapon;

	// Token: 0x04008125 RID: 33061
	public TextAsset PivotWristWeapon;

	// Token: 0x04008126 RID: 33062
	public TextAsset SmackController;

	// Token: 0x04008127 RID: 33063
	public TextAsset SniperCamera;

	// Token: 0x04008128 RID: 33064
	public TextAsset SniperWeapon;

	// Token: 0x04008129 RID: 33065
	public TextAsset SniperShooter;

	// Token: 0x0400812A RID: 33066
	public TextAsset SniperInput;

	// Token: 0x0400812B RID: 33067
	public TextAsset StompingCamera;

	// Token: 0x0400812C RID: 33068
	public TextAsset StompingInput;

	// Token: 0x0400812D RID: 33069
	public TextAsset SystemOFFCamera;

	// Token: 0x0400812E RID: 33070
	public TextAsset SystemOFFController;

	// Token: 0x0400812F RID: 33071
	public TextAsset SystemOFFShooter;

	// Token: 0x04008130 RID: 33072
	public TextAsset SystemOFFWeapon;

	// Token: 0x04008131 RID: 33073
	public TextAsset SystemOFFWeaponGlideIn;

	// Token: 0x04008132 RID: 33074
	public TextAsset SystemOFFInput;

	// Token: 0x04008133 RID: 33075
	public TextAsset TurretCamera;

	// Token: 0x04008134 RID: 33076
	public TextAsset TurretWeapon;

	// Token: 0x04008135 RID: 33077
	public TextAsset TurretShooter;

	// Token: 0x04008136 RID: 33078
	public TextAsset TurretInput;

	// Token: 0x04008137 RID: 33079
	public TextAsset WallFacingCamera;

	// Token: 0x04008138 RID: 33080
	public TextAsset WallFacingWeapon;

	// Token: 0x04008139 RID: 33081
	public TextAsset WallFacingInput;
}
