using System;
using UnityEngine;

// Token: 0x020012A2 RID: 4770
public class MovementInput
{
	// Token: 0x0600974C RID: 38732 RVA: 0x003906F9 File Offset: 0x0038E8F9
	public bool IsMoving()
	{
		return Mathf.Abs(this.moveStrafe) > 0.05f || Mathf.Abs(this.moveForward) > 0.05f;
	}

	// Token: 0x0600974D RID: 38733 RVA: 0x00390724 File Offset: 0x0038E924
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

	// Token: 0x0600974E RID: 38734 RVA: 0x00390780 File Offset: 0x0038E980
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

	// Token: 0x0600974F RID: 38735 RVA: 0x0039082C File Offset: 0x0038EA2C
	public bool Equals(MovementInput _other)
	{
		return this.moveStrafe == _other.moveStrafe && this.moveForward == _other.moveForward && this.rotation.Equals(_other.rotation) && this.cameraRotation.Equals(_other.cameraRotation) && this.jump == _other.jump && this.sneak == _other.sneak && this.down == _other.down && this.downToggle == _other.downToggle && this.useItemOnBackAction == _other.useItemOnBackAction && this.running == _other.running;
	}

	// Token: 0x0400714B RID: 29003
	public float moveStrafe;

	// Token: 0x0400714C RID: 29004
	public float moveForward;

	// Token: 0x0400714D RID: 29005
	public Vector3 rotation;

	// Token: 0x0400714E RID: 29006
	public Vector3 cameraRotation;

	// Token: 0x0400714F RID: 29007
	public float cameraDistance;

	// Token: 0x04007150 RID: 29008
	public bool running;

	// Token: 0x04007151 RID: 29009
	public bool jump;

	// Token: 0x04007152 RID: 29010
	public bool sneak;

	// Token: 0x04007153 RID: 29011
	public bool useItemOnBackAction;

	// Token: 0x04007154 RID: 29012
	public bool down;

	// Token: 0x04007155 RID: 29013
	public bool downToggle;

	// Token: 0x04007156 RID: 29014
	public bool bDetachedCameraMove;

	// Token: 0x04007157 RID: 29015
	public bool bCameraChange;

	// Token: 0x04007158 RID: 29016
	public bool bCameraPositionLocked;

	// Token: 0x04007159 RID: 29017
	public bool lastInputController;
}
