using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
[PublicizedFrom(EAccessModifier.Internal)]
public class AnimatorStateTPVHolsterUnholster : StateMachineBehaviour
{
	// Token: 0x0600033D RID: 829 RVA: 0x00018546 File Offset: 0x00016746
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.GetComponentInParent<EntityAlive>() != null;
	}
}
