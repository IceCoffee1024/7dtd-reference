using System;
using UnityEngine;

// Token: 0x0200061E RID: 1566
public class LocalPlayerFinalCamera : MonoBehaviour
{
	// Token: 0x060032FF RID: 13055 RVA: 0x0014A9DB File Offset: 0x00148BDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		this.entityPlayerLocal.finalCamera.fieldOfView = this.entityPlayerLocal.playerCamera.fieldOfView;
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x0014A9FD File Offset: 0x00148BFD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		this.entityPlayerLocal.renderManager.DynamicResolutionRender();
	}

	// Token: 0x0400284A RID: 10314
	public EntityPlayerLocal entityPlayerLocal;
}
