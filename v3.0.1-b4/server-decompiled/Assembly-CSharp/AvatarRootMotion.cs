using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class AvatarRootMotion : MonoBehaviour
{
	// Token: 0x0600051E RID: 1310 RVA: 0x00023252 File Offset: 0x00021452
	public void Init(AvatarController _mainController, Animator _root)
	{
		this.mainController = _mainController;
		this.root = _root;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00023262 File Offset: 0x00021462
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnAnimatorMove()
	{
		if (this.mainController != null && this.root != null)
		{
			this.mainController.NotifyAnimatorMove(this.root);
		}
	}

	// Token: 0x04000580 RID: 1408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AvatarController mainController;

	// Token: 0x04000581 RID: 1409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator root;
}
