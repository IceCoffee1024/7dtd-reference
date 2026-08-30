using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015DC RID: 5596
public class vp_FPInput : vp_Component
{
	// Token: 0x17001435 RID: 5173
	// (get) Token: 0x0600AF03 RID: 44803 RVA: 0x004153C4 File Offset: 0x004135C4
	public Vector2 MousePos
	{
		get
		{
			return this.m_MousePos;
		}
	}

	// Token: 0x17001436 RID: 5174
	// (get) Token: 0x0600AF04 RID: 44804 RVA: 0x004153CC File Offset: 0x004135CC
	// (set) Token: 0x0600AF05 RID: 44805 RVA: 0x004153D4 File Offset: 0x004135D4
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

	// Token: 0x17001437 RID: 5175
	// (get) Token: 0x0600AF06 RID: 44806 RVA: 0x004153DD File Offset: 0x004135DD
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

	// Token: 0x0600AF07 RID: 44807 RVA: 0x00415409 File Offset: 0x00413609
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600AF08 RID: 44808 RVA: 0x00415425 File Offset: 0x00413625
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600AF09 RID: 44809 RVA: 0x00415444 File Offset: 0x00413644
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

	// Token: 0x0600AF0A RID: 44810 RVA: 0x004154B4 File Offset: 0x004136B4
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

	// Token: 0x0600AF0B RID: 44811 RVA: 0x004154E7 File Offset: 0x004136E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputMove()
	{
		this.FPPlayer.InputMoveVector.Set(new Vector2(vp_Input.GetAxisRaw("Horizontal"), vp_Input.GetAxisRaw("Vertical")));
	}

	// Token: 0x0600AF0C RID: 44812 RVA: 0x00415517 File Offset: 0x00413717
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

	// Token: 0x0600AF0D RID: 44813 RVA: 0x0041554A File Offset: 0x0041374A
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

	// Token: 0x0600AF0E RID: 44814 RVA: 0x00415580 File Offset: 0x00413780
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

	// Token: 0x0600AF0F RID: 44815 RVA: 0x004155B4 File Offset: 0x004137B4
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

	// Token: 0x0600AF10 RID: 44816 RVA: 0x00415614 File Offset: 0x00413814
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

	// Token: 0x0600AF11 RID: 44817 RVA: 0x0041564F File Offset: 0x0041384F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputReload()
	{
		if (vp_Input.GetButtonDown("Reload"))
		{
			this.FPPlayer.Reload.TryStart(true);
		}
	}

	// Token: 0x0600AF12 RID: 44818 RVA: 0x00415670 File Offset: 0x00413870
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

	// Token: 0x0600AF13 RID: 44819 RVA: 0x004157EF File Offset: 0x004139EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePause()
	{
		if (vp_Input.GetButtonDown("Pause"))
		{
			this.FPPlayer.Pause.Set(!this.FPPlayer.Pause.Get());
		}
	}

	// Token: 0x0600AF14 RID: 44820 RVA: 0x0041582C File Offset: 0x00413A2C
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

	// Token: 0x0600AF15 RID: 44821 RVA: 0x00415908 File Offset: 0x00413B08
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

	// Token: 0x0600AF16 RID: 44822 RVA: 0x00415B28 File Offset: 0x00413D28
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

	// Token: 0x17001438 RID: 5176
	// (get) Token: 0x0600AF17 RID: 44823 RVA: 0x00415B7A File Offset: 0x00413D7A
	// (set) Token: 0x0600AF18 RID: 44824 RVA: 0x00415B82 File Offset: 0x00413D82
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

	// Token: 0x17001439 RID: 5177
	// (get) Token: 0x0600AF19 RID: 44825 RVA: 0x00415BA1 File Offset: 0x00413DA1
	public virtual float OnValue_InputClimbVector
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x1700143A RID: 5178
	// (get) Token: 0x0600AF1A RID: 44826 RVA: 0x004153CC File Offset: 0x004135CC
	// (set) Token: 0x0600AF1B RID: 44827 RVA: 0x004153D4 File Offset: 0x004135D4
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

	// Token: 0x1700143B RID: 5179
	// (get) Token: 0x0600AF1C RID: 44828 RVA: 0x00415BAD File Offset: 0x00413DAD
	// (set) Token: 0x0600AF1D RID: 44829 RVA: 0x00415BB4 File Offset: 0x00413DB4
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

	// Token: 0x0600AF1E RID: 44830 RVA: 0x00415BC6 File Offset: 0x00413DC6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButton(string button)
	{
		return vp_Input.GetButton(button);
	}

	// Token: 0x0600AF1F RID: 44831 RVA: 0x00415BCE File Offset: 0x00413DCE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonUp(string button)
	{
		return vp_Input.GetButtonUp(button);
	}

	// Token: 0x0600AF20 RID: 44832 RVA: 0x00415BD6 File Offset: 0x00413DD6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonDown(string button)
	{
		return vp_Input.GetButtonDown(button);
	}

	// Token: 0x1700143C RID: 5180
	// (get) Token: 0x0600AF21 RID: 44833 RVA: 0x00415BDE File Offset: 0x00413DDE
	public virtual Vector2 OnValue_InputSmoothLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLook();
		}
	}

	// Token: 0x1700143D RID: 5181
	// (get) Token: 0x0600AF22 RID: 44834 RVA: 0x00415BE6 File Offset: 0x00413DE6
	public virtual Vector2 OnValue_InputRawLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLookRaw();
		}
	}

	// Token: 0x040082C5 RID: 33477
	public Vector2 MouseLookSensitivity = new Vector2(5f, 5f);

	// Token: 0x040082C6 RID: 33478
	public int MouseLookSmoothSteps = 10;

	// Token: 0x040082C7 RID: 33479
	public float MouseLookSmoothWeight = 0.5f;

	// Token: 0x040082C8 RID: 33480
	public bool MouseLookAcceleration;

	// Token: 0x040082C9 RID: 33481
	public float MouseLookAccelerationThreshold = 0.4f;

	// Token: 0x040082CA RID: 33482
	public bool MouseLookInvert;

	// Token: 0x040082CB RID: 33483
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookSmoothMove = Vector2.zero;

	// Token: 0x040082CC RID: 33484
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookRawMove = Vector2.zero;

	// Token: 0x040082CD RID: 33485
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	// Token: 0x040082CE RID: 33486
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_LastMouseLookFrame = -1;

	// Token: 0x040082CF RID: 33487
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_CurrentMouseLook = Vector2.zero;

	// Token: 0x040082D0 RID: 33488
	public Rect[] MouseCursorZones;

	// Token: 0x040082D1 RID: 33489
	public bool MouseCursorForced;

	// Token: 0x040082D2 RID: 33490
	public bool MouseCursorBlocksMouseLook = true;

	// Token: 0x040082D3 RID: 33491
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MousePos = Vector2.zero;

	// Token: 0x040082D4 RID: 33492
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MoveVector = Vector2.zero;

	// Token: 0x040082D5 RID: 33493
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AllowGameplayInput = true;

	// Token: 0x040082D6 RID: 33494
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
