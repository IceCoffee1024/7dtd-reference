using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class AvatarRootMotion : MonoBehaviour
{
	// Token: 0x06000525 RID: 1317 RVA: 0x0002340E File Offset: 0x0002160E
	public void Init(AvatarController _mainController, Animator _root)
	{
		this.mainController = _mainController;
		this.root = _root;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0002341E File Offset: 0x0002161E
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnAnimatorMove()
	{
		if (this.mainController != null && this.root != null)
		{
			this.mainController.NotifyAnimatorMove(this.root);
		}
	}

	// Token: 0x04000583 RID: 1411
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AvatarController mainController;

	// Token: 0x04000584 RID: 1412
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator root;
}
