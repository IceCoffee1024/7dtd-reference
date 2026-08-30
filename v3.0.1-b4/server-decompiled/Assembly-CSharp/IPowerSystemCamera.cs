using System;
using UnityEngine;

// Token: 0x020003D2 RID: 978
public interface IPowerSystemCamera
{
	// Token: 0x06001D94 RID: 7572
	void SetPitch(float pitch);

	// Token: 0x06001D95 RID: 7573
	void SetYaw(float yaw);

	// Token: 0x06001D96 RID: 7574
	float GetPitch();

	// Token: 0x06001D97 RID: 7575
	float GetYaw();

	// Token: 0x06001D98 RID: 7576
	Transform GetCameraTransform();

	// Token: 0x06001D99 RID: 7577
	void SetUserAccessing(int userAccessingId);

	// Token: 0x06001D9A RID: 7578
	bool HasCone();

	// Token: 0x06001D9B RID: 7579
	void SetConeColor(Color _color);

	// Token: 0x06001D9C RID: 7580
	Color GetOriginalConeColor();

	// Token: 0x06001D9D RID: 7581
	void SetConeActive(bool _active);

	// Token: 0x06001D9E RID: 7582
	bool GetConeActive();

	// Token: 0x06001D9F RID: 7583
	bool HasLaser();

	// Token: 0x06001DA0 RID: 7584
	void SetLaserColor(Color _color);

	// Token: 0x06001DA1 RID: 7585
	Color GetOriginalLaserColor();

	// Token: 0x06001DA2 RID: 7586
	void SetLaserActive(bool _active);

	// Token: 0x06001DA3 RID: 7587
	bool GetLaserActive();
}
