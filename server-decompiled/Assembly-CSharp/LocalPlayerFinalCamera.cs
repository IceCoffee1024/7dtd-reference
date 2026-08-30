using System;
using UnityEngine;

// Token: 0x02000615 RID: 1557
public class LocalPlayerFinalCamera : MonoBehaviour
{
	// Token: 0x060032AE RID: 12974 RVA: 0x00149443 File Offset: 0x00147643
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreCull()
	{
		this.entityPlayerLocal.finalCamera.fieldOfView = this.entityPlayerLocal.playerCamera.fieldOfView;
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x00149465 File Offset: 0x00147665
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender()
	{
		this.entityPlayerLocal.renderManager.DynamicResolutionRender();
	}

	// Token: 0x04002836 RID: 10294
	public EntityPlayerLocal entityPlayerLocal;
}
