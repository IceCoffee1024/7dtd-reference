using System;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class FirstPersonAnimator : BodyAnimator
{
	// Token: 0x06000590 RID: 1424 RVA: 0x0002783C File Offset: 0x00025A3C
	public FirstPersonAnimator(EntityAlive _entity, AvatarCharacterController.AnimationStates _animStates, Transform _bodyTransform, BodyAnimator.EnumState _defaultState)
	{
		BodyAnimator.BodyParts bodyParts = new BodyAnimator.BodyParts(_bodyTransform, _bodyTransform.FindInChilds((_entity.emodel is EModelSDCS) ? "RightWeapon" : "Gunjoint", false));
		base.initBodyAnimator(_entity, bodyParts, _defaultState);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00027880 File Offset: 0x00025A80
	public override void SetDrunk(float _numBeers)
	{
		Animator animator = base.Animator;
		if (animator)
		{
			animator.SetFloat("drunk", _numBeers);
		}
	}
}
