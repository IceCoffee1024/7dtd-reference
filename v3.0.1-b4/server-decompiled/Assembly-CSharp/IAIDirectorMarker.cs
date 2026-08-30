using System;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public interface IAIDirectorMarker
{
	// Token: 0x060020A8 RID: 8360
	void Reference();

	// Token: 0x060020A9 RID: 8361
	bool Release();

	// Token: 0x060020AA RID: 8362
	void Tick(double dt);

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x060020AB RID: 8363
	EntityPlayer Player { get; }

	// Token: 0x060020AC RID: 8364
	double IntensityForPosition(Vector3 position);

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x060020AD RID: 8365
	Vector3 Position { get; }

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x060020AE RID: 8366
	Vector3 TargetPosition { get; }

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x060020AF RID: 8367
	bool Valid { get; }

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x060020B0 RID: 8368
	float MaxRadius { get; }

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x060020B1 RID: 8369
	float Radius { get; }

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060020B2 RID: 8370
	float TimeToLive { get; }

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x060020B3 RID: 8371
	float ValidTime { get; }

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060020B4 RID: 8372
	float Speed { get; }

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x060020B5 RID: 8373
	int Priority { get; }

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060020B6 RID: 8374
	bool InterruptsNonPlayerAttack { get; }

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060020B7 RID: 8375
	bool IsDistraction { get; }
}
