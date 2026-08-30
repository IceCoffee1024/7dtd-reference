using System;
using UnityEngine;

// Token: 0x02001278 RID: 4728
public class MovementInput
{
	// Token: 0x06009667 RID: 38503 RVA: 0x00389AA9 File Offset: 0x00387CA9
	public bool IsMoving()
	{
		return Mathf.Abs(this.moveStrafe) > 0.05f || Mathf.Abs(this.moveForward) > 0.05f;
	}

	// Token: 0x06009668 RID: 38504 RVA: 0x00389AD4 File Offset: 0x00387CD4
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

	// Token: 0x06009669 RID: 38505 RVA: 0x00389B30 File Offset: 0x00387D30
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

	// Token: 0x0600966A RID: 38506 RVA: 0x00389BDC File Offset: 0x00387DDC
	public bool Equals(MovementInput _other)
	{
		return this.moveStrafe == _other.moveStrafe && this.moveForward == _other.moveForward && this.rotation.Equals(_other.rotation) && this.cameraRotation.Equals(_other.cameraRotation) && this.jump == _other.jump && this.sneak == _other.sneak && this.down == _other.down && this.downToggle == _other.downToggle && this.useItemOnBackAction == _other.useItemOnBackAction && this.running == _other.running;
	}

	// Token: 0x040070A0 RID: 28832
	public float moveStrafe;

	// Token: 0x040070A1 RID: 28833
	public float moveForward;

	// Token: 0x040070A2 RID: 28834
	public Vector3 rotation;

	// Token: 0x040070A3 RID: 28835
	public Vector3 cameraRotation;

	// Token: 0x040070A4 RID: 28836
	public float cameraDistance;

	// Token: 0x040070A5 RID: 28837
	public bool running;

	// Token: 0x040070A6 RID: 28838
	public bool jump;

	// Token: 0x040070A7 RID: 28839
	public bool sneak;

	// Token: 0x040070A8 RID: 28840
	public bool useItemOnBackAction;

	// Token: 0x040070A9 RID: 28841
	public bool down;

	// Token: 0x040070AA RID: 28842
	public bool downToggle;

	// Token: 0x040070AB RID: 28843
	public bool bDetachedCameraMove;

	// Token: 0x040070AC RID: 28844
	public bool bCameraChange;

	// Token: 0x040070AD RID: 28845
	public bool bCameraPositionLocked;

	// Token: 0x040070AE RID: 28846
	public bool lastInputController;
}
