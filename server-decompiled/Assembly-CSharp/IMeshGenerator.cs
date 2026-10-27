using System;

// Token: 0x0200064B RID: 1611
public interface IMeshGenerator
{
	// Token: 0x0600344A RID: 13386
	bool IsLayerEmpty(int _layerIdx);

	// Token: 0x0600344B RID: 13387
	bool IsLayerEmpty(int _startLayerIdx, int _endLayerIdx);
}
