using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015F7 RID: 5623
public class vp_FPInput : vp_Component
{
	// Token: 0x1700146E RID: 5230
	// (get) Token: 0x0600B000 RID: 45056 RVA: 0x0041A704 File Offset: 0x00418904
	public Vector2 MousePos
	{
		get
		{
			return this.m_MousePos;
		}
	}

	// Token: 0x1700146F RID: 5231
	// (get) Token: 0x0600B001 RID: 45057 RVA: 0x0041A70C File Offset: 0x0041890C
	// (set) Token: 0x0600B002 RID: 45058 RVA: 0x0041A714 File Offset: 0x00418914
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

	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x0600B003 RID: 45059 RVA: 0x0041A71D File Offset: 0x0041891D
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

	// Token: 0x0600B004 RID: 45060 RVA: 0x0041A749 File Offset: 0x00418949
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600B005 RID: 45061 RVA: 0x0041A765 File Offset: 0x00418965
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B006 RID: 45062 RVA: 0x0041A784 File Offset: 0x00418984
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

	// Token: 0x0600B007 RID: 45063 RVA: 0x0041A7F4 File Offset: 0x004189F4
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

	// Token: 0x0600B008 RID: 45064 RVA: 0x0041A827 File Offset: 0x00418A27
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputMove()
	{
		this.FPPlayer.InputMoveVector.Set(new Vector2(vp_Input.GetAxisRaw("Horizontal"), vp_Input.GetAxisRaw("Vertical")));
	}

	// Token: 0x0600B009 RID: 45065 RVA: 0x0041A857 File Offset: 0x00418A57
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

	// Token: 0x0600B00A RID: 45066 RVA: 0x0041A88A File Offset: 0x00418A8A
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

	// Token: 0x0600B00B RID: 45067 RVA: 0x0041A8C0 File Offset: 0x00418AC0
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

	// Token: 0x0600B00C RID: 45068 RVA: 0x0041A8F4 File Offset: 0x00418AF4
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

	// Token: 0x0600B00D RID: 45069 RVA: 0x0041A954 File Offset: 0x00418B54
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

	// Token: 0x0600B00E RID: 45070 RVA: 0x0041A98F File Offset: 0x00418B8F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputReload()
	{
		if (vp_Input.GetButtonDown("Reload"))
		{
			this.FPPlayer.Reload.TryStart(true);
		}
	}

	// Token: 0x0600B00F RID: 45071 RVA: 0x0041A9B0 File Offset: 0x00418BB0
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

	// Token: 0x0600B010 RID: 45072 RVA: 0x0041AB2F File Offset: 0x00418D2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePause()
	{
		if (vp_Input.GetButtonDown("Pause"))
		{
			this.FPPlayer.Pause.Set(!this.FPPlayer.Pause.Get());
		}
	}

	// Token: 0x0600B011 RID: 45073 RVA: 0x0041AB6C File Offset: 0x00418D6C
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

	// Token: 0x0600B012 RID: 45074 RVA: 0x0041AC48 File Offset: 0x00418E48
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

	// Token: 0x0600B013 RID: 45075 RVA: 0x0041AE68 File Offset: 0x00419068
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

	// Token: 0x17001471 RID: 5233
	// (get) Token: 0x0600B014 RID: 45076 RVA: 0x0041AEBA File Offset: 0x004190BA
	// (set) Token: 0x0600B015 RID: 45077 RVA: 0x0041AEC2 File Offset: 0x004190C2
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

	// Token: 0x17001472 RID: 5234
	// (get) Token: 0x0600B016 RID: 45078 RVA: 0x0041AEE1 File Offset: 0x004190E1
	public virtual float OnValue_InputClimbVector
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x17001473 RID: 5235
	// (get) Token: 0x0600B017 RID: 45079 RVA: 0x0041A70C File Offset: 0x0041890C
	// (set) Token: 0x0600B018 RID: 45080 RVA: 0x0041A714 File Offset: 0x00418914
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

	// Token: 0x17001474 RID: 5236
	// (get) Token: 0x0600B019 RID: 45081 RVA: 0x0041AEED File Offset: 0x004190ED
	// (set) Token: 0x0600B01A RID: 45082 RVA: 0x0041AEF4 File Offset: 0x004190F4
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

	// Token: 0x0600B01B RID: 45083 RVA: 0x0041AF06 File Offset: 0x00419106
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButton(string button)
	{
		return vp_Input.GetButton(button);
	}

	// Token: 0x0600B01C RID: 45084 RVA: 0x0041AF0E File Offset: 0x0041910E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonUp(string button)
	{
		return vp_Input.GetButtonUp(button);
	}

	// Token: 0x0600B01D RID: 45085 RVA: 0x0041AF16 File Offset: 0x00419116
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonDown(string button)
	{
		return vp_Input.GetButtonDown(button);
	}

	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x0600B01E RID: 45086 RVA: 0x0041AF1E File Offset: 0x0041911E
	public virtual Vector2 OnValue_InputSmoothLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLook();
		}
	}

	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x0600B01F RID: 45087 RVA: 0x0041AF26 File Offset: 0x00419126
	public virtual Vector2 OnValue_InputRawLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLookRaw();
		}
	}

	// Token: 0x0400839E RID: 33694
	public Vector2 MouseLookSensitivity = new Vector2(5f, 5f);

	// Token: 0x0400839F RID: 33695
	public int MouseLookSmoothSteps = 10;

	// Token: 0x040083A0 RID: 33696
	public float MouseLookSmoothWeight = 0.5f;

	// Token: 0x040083A1 RID: 33697
	public bool MouseLookAcceleration;

	// Token: 0x040083A2 RID: 33698
	public float MouseLookAccelerationThreshold = 0.4f;

	// Token: 0x040083A3 RID: 33699
	public bool MouseLookInvert;

	// Token: 0x040083A4 RID: 33700
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookSmoothMove = Vector2.zero;

	// Token: 0x040083A5 RID: 33701
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookRawMove = Vector2.zero;

	// Token: 0x040083A6 RID: 33702
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	// Token: 0x040083A7 RID: 33703
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_LastMouseLookFrame = -1;

	// Token: 0x040083A8 RID: 33704
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_CurrentMouseLook = Vector2.zero;

	// Token: 0x040083A9 RID: 33705
	public Rect[] MouseCursorZones;

	// Token: 0x040083AA RID: 33706
	public bool MouseCursorForced;

	// Token: 0x040083AB RID: 33707
	public bool MouseCursorBlocksMouseLook = true;

	// Token: 0x040083AC RID: 33708
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MousePos = Vector2.zero;

	// Token: 0x040083AD RID: 33709
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MoveVector = Vector2.zero;

	// Token: 0x040083AE RID: 33710
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AllowGameplayInput = true;

	// Token: 0x040083AF RID: 33711
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
