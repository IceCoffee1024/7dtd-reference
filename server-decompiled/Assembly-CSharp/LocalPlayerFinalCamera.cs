using System;
using UnityEngine;

// Token: 0x02000617 RID: 1559
public class LocalPlayerFinalCamera : MonoBehaviour
{
	// Token: 0x060032F1 RID: 13041 RVA: 0x00149DE7 File Offset: 0x00147FE7
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		this.entityPlayerLocal.finalCamera.fieldOfView = this.entityPlayerLocal.playerCamera.fieldOfView;
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x00149E09 File Offset: 0x00148009
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		this.entityPlayerLocal.renderManager.DynamicResolutionRender();
	}

	// Token: 0x0400282F RID: 10287
	public EntityPlayerLocal entityPlayerLocal;
}
