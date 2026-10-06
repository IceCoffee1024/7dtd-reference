using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001616 RID: 5654
public class vp_FPInput : vp_Component
{
	// Token: 0x170014B1 RID: 5297
	// (get) Token: 0x0600B092 RID: 45202 RVA: 0x0041E3E0 File Offset: 0x0041C5E0
	public Vector2 MousePos
	{
		get
		{
			return this.m_MousePos;
		}
	}

	// Token: 0x170014B2 RID: 5298
	// (get) Token: 0x0600B093 RID: 45203 RVA: 0x0041E3E8 File Offset: 0x0041C5E8
	// (set) Token: 0x0600B094 RID: 45204 RVA: 0x0041E3F0 File Offset: 0x0041C5F0
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

	// Token: 0x170014B3 RID: 5299
	// (get) Token: 0x0600B095 RID: 45205 RVA: 0x0041E3F9 File Offset: 0x0041C5F9
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

	// Token: 0x0600B096 RID: 45206 RVA: 0x0041E425 File Offset: 0x0041C625
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Register(this);
		}
	}

	// Token: 0x0600B097 RID: 45207 RVA: 0x0041E441 File Offset: 0x0041C641
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.FPPlayer != null)
		{
			this.FPPlayer.Unregister(this);
		}
	}

	// Token: 0x0600B098 RID: 45208 RVA: 0x0041E460 File Offset: 0x0041C660
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

	// Token: 0x0600B099 RID: 45209 RVA: 0x0041E4D0 File Offset: 0x0041C6D0
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

	// Token: 0x0600B09A RID: 45210 RVA: 0x0041E503 File Offset: 0x0041C703
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputMove()
	{
		this.FPPlayer.InputMoveVector.Set(new Vector2(vp_Input.GetAxisRaw("Horizontal"), vp_Input.GetAxisRaw("Vertical")));
	}

	// Token: 0x0600B09B RID: 45211 RVA: 0x0041E533 File Offset: 0x0041C733
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

	// Token: 0x0600B09C RID: 45212 RVA: 0x0041E566 File Offset: 0x0041C766
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

	// Token: 0x0600B09D RID: 45213 RVA: 0x0041E59C File Offset: 0x0041C79C
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

	// Token: 0x0600B09E RID: 45214 RVA: 0x0041E5D0 File Offset: 0x0041C7D0
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

	// Token: 0x0600B09F RID: 45215 RVA: 0x0041E630 File Offset: 0x0041C830
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

	// Token: 0x0600B0A0 RID: 45216 RVA: 0x0041E66B File Offset: 0x0041C86B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InputReload()
	{
		if (vp_Input.GetButtonDown("Reload"))
		{
			this.FPPlayer.Reload.TryStart(true);
		}
	}

	// Token: 0x0600B0A1 RID: 45217 RVA: 0x0041E68C File Offset: 0x0041C88C
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

	// Token: 0x0600B0A2 RID: 45218 RVA: 0x0041E80B File Offset: 0x0041CA0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdatePause()
	{
		if (vp_Input.GetButtonDown("Pause"))
		{
			this.FPPlayer.Pause.Set(!this.FPPlayer.Pause.Get());
		}
	}

	// Token: 0x0600B0A3 RID: 45219 RVA: 0x0041E848 File Offset: 0x0041CA48
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

	// Token: 0x0600B0A4 RID: 45220 RVA: 0x0041E924 File Offset: 0x0041CB24
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

	// Token: 0x0600B0A5 RID: 45221 RVA: 0x0041EB44 File Offset: 0x0041CD44
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

	// Token: 0x170014B4 RID: 5300
	// (get) Token: 0x0600B0A6 RID: 45222 RVA: 0x0041EB96 File Offset: 0x0041CD96
	// (set) Token: 0x0600B0A7 RID: 45223 RVA: 0x0041EB9E File Offset: 0x0041CD9E
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

	// Token: 0x170014B5 RID: 5301
	// (get) Token: 0x0600B0A8 RID: 45224 RVA: 0x0041EBBD File Offset: 0x0041CDBD
	public virtual float OnValue_InputClimbVector
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return vp_Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x170014B6 RID: 5302
	// (get) Token: 0x0600B0A9 RID: 45225 RVA: 0x0041E3E8 File Offset: 0x0041C5E8
	// (set) Token: 0x0600B0AA RID: 45226 RVA: 0x0041E3F0 File Offset: 0x0041C5F0
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

	// Token: 0x170014B7 RID: 5303
	// (get) Token: 0x0600B0AB RID: 45227 RVA: 0x0041EBC9 File Offset: 0x0041CDC9
	// (set) Token: 0x0600B0AC RID: 45228 RVA: 0x0041EBD0 File Offset: 0x0041CDD0
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

	// Token: 0x0600B0AD RID: 45229 RVA: 0x0041EBE2 File Offset: 0x0041CDE2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButton(string button)
	{
		return vp_Input.GetButton(button);
	}

	// Token: 0x0600B0AE RID: 45230 RVA: 0x0041EBEA File Offset: 0x0041CDEA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonUp(string button)
	{
		return vp_Input.GetButtonUp(button);
	}

	// Token: 0x0600B0AF RID: 45231 RVA: 0x0041EBF2 File Offset: 0x0041CDF2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool OnMessage_InputGetButtonDown(string button)
	{
		return vp_Input.GetButtonDown(button);
	}

	// Token: 0x170014B8 RID: 5304
	// (get) Token: 0x0600B0B0 RID: 45232 RVA: 0x0041EBFA File Offset: 0x0041CDFA
	public virtual Vector2 OnValue_InputSmoothLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLook();
		}
	}

	// Token: 0x170014B9 RID: 5305
	// (get) Token: 0x0600B0B1 RID: 45233 RVA: 0x0041EC02 File Offset: 0x0041CE02
	public virtual Vector2 OnValue_InputRawLook
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.GetMouseLookRaw();
		}
	}

	// Token: 0x040083F4 RID: 33780
	public Vector2 MouseLookSensitivity = new Vector2(5f, 5f);

	// Token: 0x040083F5 RID: 33781
	public int MouseLookSmoothSteps = 10;

	// Token: 0x040083F6 RID: 33782
	public float MouseLookSmoothWeight = 0.5f;

	// Token: 0x040083F7 RID: 33783
	public bool MouseLookAcceleration;

	// Token: 0x040083F8 RID: 33784
	public float MouseLookAccelerationThreshold = 0.4f;

	// Token: 0x040083F9 RID: 33785
	public bool MouseLookInvert;

	// Token: 0x040083FA RID: 33786
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookSmoothMove = Vector2.zero;

	// Token: 0x040083FB RID: 33787
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MouseLookRawMove = Vector2.zero;

	// Token: 0x040083FC RID: 33788
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector2> m_MouseLookSmoothBuffer = new List<Vector2>();

	// Token: 0x040083FD RID: 33789
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_LastMouseLookFrame = -1;

	// Token: 0x040083FE RID: 33790
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_CurrentMouseLook = Vector2.zero;

	// Token: 0x040083FF RID: 33791
	public Rect[] MouseCursorZones;

	// Token: 0x04008400 RID: 33792
	public bool MouseCursorForced;

	// Token: 0x04008401 RID: 33793
	public bool MouseCursorBlocksMouseLook = true;

	// Token: 0x04008402 RID: 33794
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MousePos = Vector2.zero;

	// Token: 0x04008403 RID: 33795
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 m_MoveVector = Vector2.zero;

	// Token: 0x04008404 RID: 33796
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AllowGameplayInput = true;

	// Token: 0x04008405 RID: 33797
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_FPPlayer;
}
