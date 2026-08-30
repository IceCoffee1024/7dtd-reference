using System;
using UnityEngine;

// Token: 0x0200060C RID: 1548
public class LocalPlayerFinalCamera : MonoBehaviour
{
	// Token: 0x0600325B RID: 12891 RVA: 0x0014731F File Offset: 0x0014551F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		this.entityPlayerLocal.finalCamera.fieldOfView = this.entityPlayerLocal.playerCamera.fieldOfView;
	}

	// Token: 0x0600325C RID: 12892 RVA: 0x00147341 File Offset: 0x00145541
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		this.entityPlayerLocal.renderManager.DynamicResolutionRender();
	}

	// Token: 0x040027C3 RID: 10179
	public EntityPlayerLocal entityPlayerLocal;
}
