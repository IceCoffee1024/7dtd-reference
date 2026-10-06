using System;
using UnityEngine;

// Token: 0x02001290 RID: 4752
public class MovementInput
{
	// Token: 0x060096F4 RID: 38644 RVA: 0x0038E2F1 File Offset: 0x0038C4F1
	public bool IsMoving()
	{
		return Mathf.Abs(this.moveStrafe) > 0.05f || Mathf.Abs(this.moveForward) > 0.05f;
	}

	// Token: 0x060096F5 RID: 38645 RVA: 0x0038E31C File Offset: 0x0038C51C
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

	// Token: 0x060096F6 RID: 38646 RVA: 0x0038E378 File Offset: 0x0038C578
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

	// Token: 0x060096F7 RID: 38647 RVA: 0x0038E424 File Offset: 0x0038C624
	public bool Equals(MovementInput _other)
	{
		return this.moveStrafe == _other.moveStrafe && this.moveForward == _other.moveForward && this.rotation.Equals(_other.rotation) && this.cameraRotation.Equals(_other.cameraRotation) && this.jump == _other.jump && this.sneak == _other.sneak && this.down == _other.down && this.downToggle == _other.downToggle && this.useItemOnBackAction == _other.useItemOnBackAction && this.running == _other.running;
	}

	// Token: 0x0400710A RID: 28938
	public float moveStrafe;

	// Token: 0x0400710B RID: 28939
	public float moveForward;

	// Token: 0x0400710C RID: 28940
	public Vector3 rotation;

	// Token: 0x0400710D RID: 28941
	public Vector3 cameraRotation;

	// Token: 0x0400710E RID: 28942
	public float cameraDistance;

	// Token: 0x0400710F RID: 28943
	public bool running;

	// Token: 0x04007110 RID: 28944
	public bool jump;

	// Token: 0x04007111 RID: 28945
	public bool sneak;

	// Token: 0x04007112 RID: 28946
	public bool useItemOnBackAction;

	// Token: 0x04007113 RID: 28947
	public bool down;

	// Token: 0x04007114 RID: 28948
	public bool downToggle;

	// Token: 0x04007115 RID: 28949
	public bool bDetachedCameraMove;

	// Token: 0x04007116 RID: 28950
	public bool bCameraChange;

	// Token: 0x04007117 RID: 28951
	public bool bCameraPositionLocked;

	// Token: 0x04007118 RID: 28952
	public bool lastInputController;
}
