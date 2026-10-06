using System;

// Token: 0x02000644 RID: 1604
public interface IMeshGenerator
{
	// Token: 0x0600343C RID: 13372
	bool IsLayerEmpty(int _layerIdx);

	// Token: 0x0600343D RID: 13373
	bool IsLayerEmpty(int _startLayerIdx, int _endLayerIdx);
}
