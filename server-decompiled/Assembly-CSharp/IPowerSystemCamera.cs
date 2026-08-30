using System;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public interface IPowerSystemCamera
{
	// Token: 0x06001DB1 RID: 7601
	void SetPitch(float pitch);

	// Token: 0x06001DB2 RID: 7602
	void SetYaw(float yaw);

	// Token: 0x06001DB3 RID: 7603
	float GetPitch();

	// Token: 0x06001DB4 RID: 7604
	float GetYaw();

	// Token: 0x06001DB5 RID: 7605
	Transform GetCameraTransform();

	// Token: 0x06001DB6 RID: 7606
	void SetUserAccessing(int userAccessingId);

	// Token: 0x06001DB7 RID: 7607
	bool HasCone();

	// Token: 0x06001DB8 RID: 7608
	void SetConeColor(Color _color);

	// Token: 0x06001DB9 RID: 7609
	Color GetOriginalConeColor();

	// Token: 0x06001DBA RID: 7610
	void SetConeActive(bool _active);

	// Token: 0x06001DBB RID: 7611
	bool GetConeActive();

	// Token: 0x06001DBC RID: 7612
	bool HasLaser();

	// Token: 0x06001DBD RID: 7613
	void SetLaserColor(Color _color);

	// Token: 0x06001DBE RID: 7614
	Color GetOriginalLaserColor();

	// Token: 0x06001DBF RID: 7615
	void SetLaserActive(bool _active);

	// Token: 0x06001DC0 RID: 7616
	bool GetLaserActive();
}
