using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001628 RID: 5672
public class vp_FPInput : vp_Component
{
	// Token: 0x170014BD RID: 5309
	// (get) Token: 0x0600B0EA RID: 45290 RVA: 0x004207E4 File Offset: 0x0041E9E4
	public Vector2 MousePos
	{
		get
		{
			return this.m_MousePos;
		}
	}

	// Token: 0x170014BE RID: 5310
	// (get) Token: 0x0600B0EB RID: 45291 RVA: 0x004207EC File Offset: 0x0041E9EC
	// (set) Token: 0x0600B0EC RID: 45292 RVA: 0x004207F4 File Offset: 0x0041E9F4
	public bool AllowGameplayInput
	{
		get
		{
			return this.m_AllowGameplayInput;
		}
		set
		{
			this.m_AllowGameplayInput = value;
		}
	}

	// Token: 0x170014BF RID: 5311
	// (get) Token: 0x0600B0ED RID: 45293 RVA: 0x004207FD File Offset: 0x0041E9FD
	public vp_FPPlayerEventHandler FPPlayer
	{
		get
		{
			if (this.m_FPPlayer == null)
			{
				this.m_FPPlayer = base.transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();
			}
			return this.m_FPPlayer;
		}
	}

	// Token: 0x0600B0EE RID: 45294 RVA: 0x00420829 File Offset: 0x0041EA29
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600B0EF RID: 45295 RVA: 0x00420845 File Offset: 0x0041EA45
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B0F0 RID: 45296 RVA: 0x00420864 File Offset: 0x0041EA64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		this.UpdateCursorLock();
		this.UpdatePause();
		if (this.FPPlayer.Pause.Get())
		{
			return;
		}
		if (!this.m_AllowGameplayInput)
		{
			return;
		}
		this.InputInteract();
		this.InputMove();
		this.InputRun();
		this.InputJump();
		this.InputCrouch();
		this.InputAttack();
		this.InputReload();
		this.InputSetWeapon();
		this.InputCamera();
	}

	// Token: 0x0600B0F1 RID: 45297 RVA: 0x004208D4 File Offset: 0x0041EAD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputInteract()
	{
		if (vp_Input.GetButtonDown("Interact"))
		{
			this.FPPlayer.Interact.TryStart(true);
			return;
		}
		this.FPPlayer.Interact.TryStop(true);
	}

	// Token: 0x0600B0F2 RID: 45298 RVA: 0x00420907 File Offset: 0x0041EB07
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputMove()
	{
		this.FPPlayer.InputMoveVector.Set(new Vector2(vp_Input.GetAxisRaw("Horizontal"), vp_Input.GetAxisRaw("Vertical")));
	}

	// Token: 0x0600B0F3 RID: 45299 RVA: 0x00420937 File Offset: 0x0041EB37
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputRun()
	{
		if (vp_Input.GetButton("Run"))
		{
			this.FPPlayer.Run.TryStart(true);
			return;
		}
		this.FPPlayer.Run.TryStop(true);
	}

	// Token: 0x0600B0F4 RID: 45300 RVA: 0x0042096A File Offset: 0x0041EB6A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputJump()
	{
		if (vp_Input.GetButton("Jump"))
		{
			this.FPPlayer.Jump.TryStart(true);
			return;
		}
		this.FPPlayer.Jump.Stop(0f);
	}

	// Token: 0x0600B0F5 RID: 45301 RVA: 0x004209A0 File Offset: 0x0041EBA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputCrouch()
	{
		if (vp_Input.GetButton("Crouch"))
		{
			this.FPPlayer.Crouch.TryStart(true);
			return;
		}
		this.FPPlayer.Crouch.TryStop(true);
	}

	// Token: 0x0600B0F6 RID: 45302 RVA: 0x004209D4 File Offset: 0x0041EBD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputCamera()
	{
		if (vp_Input.GetButton("Zoom"))
		{
			this.FPPlayer.Zoom.TryStart(true);
		}
		else
		{
			this.FPPlayer.Zoom.TryStop(true);
		}
		if (vp_Input.GetButtonDown("Toggle3rdPerson"))
		{
			this.FPPlayer.CameraToggle3rdPerson.Send();
		}
	}

	// Token: 0x0600B0F7 RID: 45303 RVA: 0x00420A34 File Offset: 0x0041EC34
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputAttack()
	{
		if (!vp_Utility.LockCursor)
		{
			return;
		}
		if (vp_Input.GetButton("Attack"))
		{
			this.FPPlayer.Attack.TryStart(true);
			return;
		}
		this.FPPlayer.Attack.TryStop(true);
	}

	// Token: 0x0600B0F8 RID: 45304 RVA: 0x00420A6F File Offset: 0x0041EC6F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputReload()
	{
		if (vp_Input.GetButtonDown("Reload"))
		{
			this.FPPlayer.Reload.TryStart(true);
		}
	}

	// Token: 0x0600B0F9 RID: 45305 RVA: 0x00420A90 File Offset: 0x0041EC90
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputSetWeapon()
	{
		if (vp_Input.GetButtonDown("SetPrevWeapon"))
		{
			this.FPPlayer.SetPrevWeapon.Try();
		}
		if (vp_Input.GetButtonDown("SetNextWeapon"))
		{
			this.FPPlayer.SetNextWeapon.Try();
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(5);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(6);
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(7);
		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(8);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(9);
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(10);
		}
		if (vp_Input.GetButtonDown("ClearWeapon"))
		{
			this.FPPlayer.SetWeapon.TryStart<int>(0);
		}
	}

	// Token: 0x0600B0FA RID: 45306 RVA: 0x00420C0F File Offset: 0x0041EE0F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePause()
	{
		if (vp_Input.GetButtonDown("Pause"))
		{
			this.FPPlayer.Pause.Set(!this.FPPlayer.Pause.Get());
		}
	}

	// Token: 0x0600B0FB RID: 45307 RVA: 0x00420C4C File Offset: 0x0041EE4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCursorLock()
	{
		this.m_MousePos.x = Input.mousePosition.x;
		this.m_MousePos.y = (float)Screen.height - Input.mousePosition.y;
		if (this.MouseCursorForced)
		{
			vp_Utility.LockCursor = false;
			return;
		}
		if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			if (this.MouseCursorZones.Length != 0)
			{
				foreach (Rect rect in this.MouseCursorZones)
				{
					if (rect.Contains(this.m_MousePos))
					{
						vp_Utility.LockCursor = false;
						goto IL_009B;
					}
				}
			}
			vp_Utility.LockCursor = true;
		}
		IL_009B:
		if (vp_Input.GetButtonUp("Accept1") || vp_Input.GetButtonUp("Accept2") || vp_Input.GetButtonUp("Menu"))
		{
			vp_Utility.LockCursor = !vp_Utility.LockCursor;
		}
	}

	// Token: 0x0600B0FC RID: 45308 RVA: 0x00420D28 File Offset: 0x0041EF28
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector2 GetMouseLook()
	{
		if (this.MouseCursorBlocksMouseLook && !vp_Utility.LockCursor)
		{
			return Vector2.zero;
		}
		if (this.m_LastMouseLookFrame == Time.frameCount)
		{
			return this.m_CurrentMouseLook;
		}
		this.m_LastMouseLookFrame = Time.frameCount;
		this.m_MouseLookSmoothMove.x = vp_Input.GetAxisRaw("Mouse X") * Time.timeScale;
		this.m_MouseLookSmoothMove.y = vp_Input.GetAxisRaw("Mouse Y") * Time.timeScale;
		this.MouseLookSmoothSteps = Mathf.Clamp(this.MouseLookSmoothSteps, 1, 20);
		this.MouseLookSmoothWeight = Mathf.Clamp01(this.MouseLookSmoothWeight);
		while (this.m_MouseLookSmoothBuffer.Count > this.MouseLookSmoothSteps)
		{
			this.m_MouseLookSmoothBuffer.RemoveAt(0);
		}
		this.m_MouseLookSmoothBuffer.Add(this.m_MouseLookSmoothMove);
		float num = 1f;
		Vector2 vector = Vector2.zero;
		float num2 = 0f;
		for (int i = this.m_MouseLookSmoothBuffer.Count - 1; i > 0; i--)
		{
			vector += this.m_MouseLookSmoothBuffer[i] * num;
			num2 += 1f * num;
			num *= this.MouseLookSmoothWeight / base.Delta;
		}
		num2 = Mathf.Max(1f, num2);
		this.m_CurrentMouseLook = vp_MathUtility.NaNSafeVector2(vector / num2, default(Vector2));
		float num3 = 0f;
		float num4 = Mathf.Abs(this.m_CurrentMouseLook.x);
		float num5 = Mathf.Abs(this.m_CurrentMouseLook.y);
		if (this.MouseLookAcceleration)
		{
			num3 = Mathf.Sqrt(num4 * num4 + num5 * num5) / base.Delta;
			num3 = ((num3 <= this.MouseLookAccelerationThreshold) ? 0f : num3);
		}
		this.m_CurrentMouseLook.x = this.m_CurrentMouseLook.x * (this.MouseLookSensitivity.x + num3);
		this.m_CurrentMouseLook.y = this.m_CurrentMouseLook.y * (this.MouseLookSensitivity.y + num3);
		this.m_CurrentMouseLook.y = (this.MouseLookInvert ? this.m_CurrentMouseLook.y : (-this.m_CurrentMouseLook.y));
		return this.m_CurrentMouseLook;
	}

	// Token: 0x0600B0FD RID: 45309 RVA: 0x00420F48 File Offset: 0x0041F148
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual Vector2 GetMouseLookRaw()
	{
		if (this.MouseCursorBlocksMouseLook && !vp_Utility.LockCursor)
		{
			return Vector2.zero;
		}
		this.m_MouseLookRawMove.x = vp_Input.GetAxisRaw("Mouse X");
		this.m_MouseLookRawMove.y = vp_Input.GetAxisRaw("Mouse Y");
		return this.m_MouseLookRawMove;
	}

	// Token: 0x170014C0 RID: 5312
	// (get) Token: 0x0600B0FE RID: 45310 RVA: 0x00420F9A File Offset: 0x0041F19A
	// (set) Token: 0x0600B0FF RID: 45311 RVA: 0x00420FA2 File Offset: 0x0041F1A2
	public virtual Vector2 OnValue_InputMoveVector
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_MoveVector;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_MoveVector = ((value != Vector2.zero) ? value.normalized : value);
		}
	}

	// Token: 0x170014C1 RID: 5313
	// (get) Token: 0x0600B100 RID: 45312 RVA: 0x00420FC1 File Offset: 0x0041F1C1
	public virtual float OnValue_InputClimbVector
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x170014C2 RID: 5314
	// (get) Token: 0x0600B101 RID: 45313 RVA: 0x004207EC File Offset: 0x0041E9EC
	// (set) Token: 0x0600B102 RID: 45314 RVA: 0x004207F4 File Offset: 0x0041E9F4
	public virtual bool OnValue_InputAllowGameplay
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_AllowGameplayInput;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_AllowGameplayInput = value;
		}
	}

	// Token: 0x170014C3 RID: 5315
	// (get) Token: 0x0600B103 RID: 45315 RVA: 0x00420FCD File Offset: 0x0041F1CD
	// (set) Token: 0x0600B104 RID: 45316 RVA: 0x00420FD4 File Offset: 0x0041F1D4
	public virtual bool OnValue_Pause
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_TimeUtility.Paused;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			vp_TimeUtility.Paused = !vp_Gameplay.isMultiplayer && value;
		}
	}

	// Token: 0x0600B105 RID: 45317 RVA: 0x00420FE6 File Offset: 0x0041F1E6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButton(string button)
	{
		return vp_Input.GetButton(button);
	}

	// Token: 0x0600B106 RID: 45318 RVA: 0x00420FEE File Offset: 0x0041F1EE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonUp(string button)
	{
		return vp_Input.GetButtonUp(button);
	}

	// Token: 0x0600B107 RID: 45319 RVA: 0x00420FF6 File Offset: 0x0041F1F6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonDown(string button)
	{
		return vp_Input.GetButtonDown(button);
	}

	// Token: 0x170014C4 RID: 5316
	// (get) Token: 0x0600B108 RID: 45320 RVA: 0x00420FFE File Offset: 0x0041F1FE
	public virtual Vector2 OnValue_InputSmoothLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLook();
		}
	}

	// Token: 0x170014C5 RID: 5317
	// (get) Token: 0x0600B109 RID: 45321 RVA: 0x00421006 File Offset: 0x0041F206
	public virtual Vector2 OnValue_InputRawLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLookRaw();
		}
	}

	// Token: 0x04008435 RID: 33845
	public Vector2 MouseLookSensitivity = new Vector2(5f, 5f);

	// Token: 0x04008436 RID: 33846
	public int MouseLookSmoothSteps = 10;

	// Token: 0x04008437 RID: 33847
	public float MouseLookSmoothWeight = 0.5f;

	// Token: 0x04008438 RID: 33848
	public bool MouseLookAcceleration;

	// Token: 0x04008439 RID: 33849
	public float MouseLookAccelerationThreshold = 0.4f;

	// Token: 0x0400843A RID: 33850
	public bool MouseLookInvert;

	// Token: 0x0400843B RID: 33851
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookSmoothMove = Vector2.zero;

	// Token: 0x0400843C RID: 33852
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookRawMove = Vector2.zero;

	// Token: 0x0400843D RID: 33853
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	// Token: 0x0400843E RID: 33854
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_LastMouseLookFrame = -1;

	// Token: 0x0400843F RID: 33855
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_CurrentMouseLook = Vector2.zero;

	// Token: 0x04008440 RID: 33856
	public Rect[] MouseCursorZones;

	// Token: 0x04008441 RID: 33857
	public bool MouseCursorForced;

	// Token: 0x04008442 RID: 33858
	public bool MouseCursorBlocksMouseLook = true;

	// Token: 0x04008443 RID: 33859
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MousePos = Vector2.zero;

	// Token: 0x04008444 RID: 33860
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MoveVector = Vector2.zero;

	// Token: 0x04008445 RID: 33861
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AllowGameplayInput = true;

	// Token: 0x04008446 RID: 33862
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
