using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C81 RID: 3201
public interface IPowered
{
	// Token: 0x06006155 RID: 24917
	Vector3i GetParent();

	// Token: 0x06006156 RID: 24918
	PowerItem GetPowerItem();

	// Token: 0x06006157 RID: 24919
	void DrawWires();

	// Token: 0x06006158 RID: 24920
	void RemoveWires();

	// Token: 0x06006159 RID: 24921
	void MarkWireDirty();

	// Token: 0x0600615A RID: 24922
	void MarkChanged();

	// Token: 0x0600615B RID: 24923
	void AddWireData(Vector3i child);

	// Token: 0x0600615C RID: 24924
	Vector3 GetWireOffset();

	// Token: 0x0600615D RID: 24925
	int GetRequiredPower();

	// Token: 0x0600615E RID: 24926
	bool CanHaveParent(IPowered newParent);

	// Token: 0x0600615F RID: 24927
	void SetParentWithWireTool(IPowered parent, int entityID);

	// Token: 0x06006160 RID: 24928
	void RemoveParentWithWiringTool(int wiringEntityID);

	// Token: 0x06006161 RID: 24929
	void SetWireData(List<Vector3i> wireChildren);

	// Token: 0x06006162 RID: 24930
	void SendWireData();

	// Token: 0x06006163 RID: 24931
	void CreateWireDataFromPowerItem();
}
