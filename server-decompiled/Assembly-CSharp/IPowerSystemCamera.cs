using System;
using UnityEngine;

// Token: 0x020003DB RID: 987
public interface IPowerSystemCamera
{
	// Token: 0x06001DAD RID: 7597
	void SetPitch(float pitch);

	// Token: 0x06001DAE RID: 7598
	void SetYaw(float yaw);

	// Token: 0x06001DAF RID: 7599
	float GetPitch();

	// Token: 0x06001DB0 RID: 7600
	float GetYaw();

	// Token: 0x06001DB1 RID: 7601
	Transform GetCameraTransform();

	// Token: 0x06001DB2 RID: 7602
	void SetUserAccessing(int userAccessingId);

	// Token: 0x06001DB3 RID: 7603
	bool HasCone();

	// Token: 0x06001DB4 RID: 7604
	void SetConeColor(Color _color);

	// Token: 0x06001DB5 RID: 7605
	Color GetOriginalConeColor();

	// Token: 0x06001DB6 RID: 7606
	void SetConeActive(bool _active);

	// Token: 0x06001DB7 RID: 7607
	bool GetConeActive();

	// Token: 0x06001DB8 RID: 7608
	bool HasLaser();

	// Token: 0x06001DB9 RID: 7609
	void SetLaserColor(Color _color);

	// Token: 0x06001DBA RID: 7610
	Color GetOriginalLaserColor();

	// Token: 0x06001DBB RID: 7611
	void SetLaserActive(bool _active);

	// Token: 0x06001DBC RID: 7612
	bool GetLaserActive();
}
