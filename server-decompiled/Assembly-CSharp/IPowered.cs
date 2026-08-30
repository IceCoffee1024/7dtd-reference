using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C72 RID: 3186
public interface IPowered
{
	// Token: 0x06006159 RID: 24921
	Vector3i GetParent();

	// Token: 0x0600615A RID: 24922
	PowerItem GetPowerItem();

	// Token: 0x0600615B RID: 24923
	void DrawWires();

	// Token: 0x0600615C RID: 24924
	void RemoveWires();

	// Token: 0x0600615D RID: 24925
	void MarkWireDirty();

	// Token: 0x0600615E RID: 24926
	void MarkChanged();

	// Token: 0x0600615F RID: 24927
	void AddWireData(Vector3i child);

	// Token: 0x06006160 RID: 24928
	Vector3 GetWireOffset();

	// Token: 0x06006161 RID: 24929
	int GetRequiredPower();

	// Token: 0x06006162 RID: 24930
	bool CanHaveParent(IPowered newParent);

	// Token: 0x06006163 RID: 24931
	void SetParentWithWireTool(IPowered parent, int entityID);

	// Token: 0x06006164 RID: 24932
	void RemoveParentWithWiringTool(int wiringEntityID);

	// Token: 0x06006165 RID: 24933
	void SetWireData(List<Vector3i> wireChildren);

	// Token: 0x06006166 RID: 24934
	void SendWireData();

	// Token: 0x06006167 RID: 24935
	void CreateWireDataFromPowerItem();
}
