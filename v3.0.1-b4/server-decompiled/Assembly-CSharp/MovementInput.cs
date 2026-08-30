using System;
using UnityEngine;

// Token: 0x0200125E RID: 4702
public class MovementInput
{
	// Token: 0x0600956C RID: 38252 RVA: 0x0038494D File Offset: 0x00382B4D
	public bool IsMoving()
	{
		return Mathf.Abs(this.moveStrafe) > 0.05f || Mathf.Abs(this.moveForward) > 0.05f;
	}

	// Token: 0x0600956D RID: 38253 RVA: 0x00384978 File Offset: 0x00382B78
	public void Clear()
	{
		this.moveStrafe = 0f;
		this.moveForward = 0f;
		this.rotation = Vector3.zero;
		this.cameraRotation = Vector3.zero;
		this.jump = false;
		this.sneak = false;
		this.useItemOnBackAction = false;
		this.down = false;
		this.downToggle = false;
	}

	// Token: 0x0600956E RID: 38254 RVA: 0x003849D4 File Offset: 0x00382BD4
	public void Copy(MovementInput _other)
	{
		_other.moveStrafe = this.moveStrafe;
		_other.moveForward = this.moveForward;
		_other.rotation = this.rotation;
		_other.cameraRotation = this.cameraRotation;
		_other.cameraDistance = this.cameraDistance;
		_other.running = this.running;
		_other.jump = this.jump;
		_other.sneak = this.sneak;
		_other.useItemOnBackAction = this.useItemOnBackAction;
		_other.down = this.down;
		_other.downToggle = this.downToggle;
		_other.bDetachedCameraMove = this.bDetachedCameraMove;
		_other.bCameraPositionLocked = this.bCameraPositionLocked;
	}

	// Token: 0x0600956F RID: 38255 RVA: 0x00384A80 File Offset: 0x00382C80
	public bool Equals(MovementInput _other)
	{
		return this.moveStrafe == _other.moveStrafe && this.moveForward == _other.moveForward && this.rotation.Equals(_other.rotation) && this.cameraRotation.Equals(_other.cameraRotation) && this.jump == _other.jump && this.sneak == _other.sneak && this.down == _other.down && this.downToggle == _other.downToggle && this.useItemOnBackAction == _other.useItemOnBackAction && this.running == _other.running;
	}

	// Token: 0x04006FCB RID: 28619
	public float moveStrafe;

	// Token: 0x04006FCC RID: 28620
	public float moveForward;

	// Token: 0x04006FCD RID: 28621
	public Vector3 rotation;

	// Token: 0x04006FCE RID: 28622
	public Vector3 cameraRotation;

	// Token: 0x04006FCF RID: 28623
	public float cameraDistance;

	// Token: 0x04006FD0 RID: 28624
	public bool running;

	// Token: 0x04006FD1 RID: 28625
	public bool jump;

	// Token: 0x04006FD2 RID: 28626
	public bool sneak;

	// Token: 0x04006FD3 RID: 28627
	public bool useItemOnBackAction;

	// Token: 0x04006FD4 RID: 28628
	public bool down;

	// Token: 0x04006FD5 RID: 28629
	public bool downToggle;

	// Token: 0x04006FD6 RID: 28630
	public bool bDetachedCameraMove;

	// Token: 0x04006FD7 RID: 28631
	public bool bCameraChange;

	// Token: 0x04006FD8 RID: 28632
	public bool bCameraPositionLocked;

	// Token: 0x04006FD9 RID: 28633
	public bool lastInputController;
}
