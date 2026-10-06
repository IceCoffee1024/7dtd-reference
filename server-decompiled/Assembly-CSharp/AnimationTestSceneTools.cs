using System;
using UnityEngine;

// Token: 0x02001552 RID: 5458
public class AnimationTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB00 RID: 43776 RVA: 0x003FCB22 File Offset: 0x003FAD22
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.weaponPrefabCount = this.weaponPrefabs.Length;
	}

	// Token: 0x0600AB01 RID: 43777 RVA: 0x003FCB40 File Offset: 0x003FAD40
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			if (this.isCrouching)
			{
				this.isCrouching = false;
			}
			else
			{
				this.isCrouching = true;
			}
			this.anim.SetBool("IsCrouching", this.isCrouching);
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.locomotionState++;
			if (this.locomotionState >= this.locomotionSpeeds.Length)
			{
				this.locomotionState = 0;
			}
			this.forwardGoal = this.locomotionSpeeds[this.locomotionState];
		}
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime * -1f, 0f);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.Rotate(0f, this.turnRate * Time.deltaTime, 0f);
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			this.weaponPrefabIndex++;
			this.anim.SetTrigger("ItemHasChangedTrigger");
			if (this.weaponPrefabIndex >= this.weaponPrefabCount)
			{
				this.weaponPrefabIndex = 0;
			}
			this.attachWeapon(this.weaponPrefabIndex);
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			this.weaponPrefabIndex--;
			this.anim.SetTrigger("ItemHasChangedTrigger");
			if (this.weaponPrefabIndex < 0)
			{
				this.weaponPrefabIndex = this.weaponPrefabCount;
			}
			this.attachWeapon(this.weaponPrefabIndex);
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			this.anim.SetTrigger("Reload");
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.anim.SetTrigger("WeaponFire");
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.anim.ResetTrigger("WeaponFire");
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.anim.SetTrigger("IsAiming");
		}
		if (Input.GetMouseButtonUp(1))
		{
			this.anim.ResetTrigger("IsAiming");
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			this.anim.SetTrigger("PowerAttack");
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			this.anim.SetTrigger("UseItem");
		}
		this.updateYLook();
		this.forward = Mathf.Lerp(this.forward, this.forwardGoal, 0.01f);
		this.anim.SetFloat("Forward", this.forward);
	}

	// Token: 0x0600AB02 RID: 43778 RVA: 0x003FCD98 File Offset: 0x003FAF98
	[PublicizedFrom(EAccessModifier.Private)]
	public void attachWeapon(int weaponPrefabIndex)
	{
		this.removeAllWeapons();
		if (this.weaponPrefabs[weaponPrefabIndex] != null)
		{
			this.newWeapon = global::UnityEngine.Object.Instantiate<GameObject>(this.weaponPrefabs[weaponPrefabIndex]);
			this.newWeapon.transform.parent = this.weaponJoint.transform;
			this.newWeapon.transform.localPosition = Vector3.zero;
			this.newWeapon.transform.localEulerAngles = Vector3.zero;
		}
		Debug.Log(weaponPrefabIndex);
		this.anim.SetInteger("WeaponHoldType", this.weaponHoldTypes[weaponPrefabIndex]);
	}

	// Token: 0x0600AB03 RID: 43779 RVA: 0x003FCE38 File Offset: 0x003FB038
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeAllWeapons()
	{
		this.weaponJointChildrenCount = this.weaponJoint.childCount;
		if (this.weaponJointChildrenCount > 0)
		{
			for (int i = 0; i < this.weaponJointChildrenCount; i++)
			{
				this.existingChild = this.weaponJoint.GetChild(i).gameObject;
				global::UnityEngine.Object.Destroy(this.existingChild);
			}
		}
	}

	// Token: 0x0600AB04 RID: 43780 RVA: 0x003FCE94 File Offset: 0x003FB094
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateYLook()
	{
		Vector3 mousePosition = Input.mousePosition;
		this.mousePosXRatio = mousePosition.x / (float)Screen.width;
		this.mousePosYRatio = mousePosition.y / (float)Screen.height;
		this.mousePosX = (this.mousePosXRatio - 0.5f) * 2f;
		this.mousePosY = (this.mousePosYRatio - 0.5f) * -2f;
		this.anim.SetFloat("YLook", this.mousePosY);
	}

	// Token: 0x04007EE0 RID: 32480
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007EE1 RID: 32481
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007EE2 RID: 32482
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float oneHandMeleeTargetWeight;

	// Token: 0x04007EE3 RID: 32483
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float oneHandPistolTargetWeight;

	// Token: 0x04007EE4 RID: 32484
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007EE5 RID: 32485
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float layerWeight;

	// Token: 0x04007EE6 RID: 32486
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 200f;

	// Token: 0x04007EE7 RID: 32487
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007EE8 RID: 32488
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel = 1;

	// Token: 0x04007EE9 RID: 32489
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponPrefabIndex;

	// Token: 0x04007EEA RID: 32490
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponJointChildrenCount;

	// Token: 0x04007EEB RID: 32491
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currOneHandMeleeWeight;

	// Token: 0x04007EEC RID: 32492
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currOneHandPistolWeight;

	// Token: 0x04007EED RID: 32493
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCrouching;

	// Token: 0x04007EEE RID: 32494
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponPrefabCount;

	// Token: 0x04007EEF RID: 32495
	public GameObject[] weaponPrefabs;

	// Token: 0x04007EF0 RID: 32496
	public int[] weaponHoldTypes;

	// Token: 0x04007EF1 RID: 32497
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] locomotionSpeeds = new float[] { 0f, 2.08f, 4.2f };

	// Token: 0x04007EF2 RID: 32498
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int locomotionState;

	// Token: 0x04007EF3 RID: 32499
	public Transform weaponJoint;

	// Token: 0x04007EF4 RID: 32500
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject existingChild;

	// Token: 0x04007EF5 RID: 32501
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject newWeapon;

	// Token: 0x04007EF6 RID: 32502
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosXRatio;

	// Token: 0x04007EF7 RID: 32503
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosYRatio;

	// Token: 0x04007EF8 RID: 32504
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosX;

	// Token: 0x04007EF9 RID: 32505
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosY;

	// Token: 0x04007EFA RID: 32506
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float forward;

	// Token: 0x04007EFB RID: 32507
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float forwardGoal;

	// Token: 0x04007EFC RID: 32508
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float strafe;

	// Token: 0x04007EFD RID: 32509
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float YLook;

	// Token: 0x04007EFE RID: 32510
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float horizontalMax = 4.2f;

	// Token: 0x04007EFF RID: 32511
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float verticalMax = 4.2f;
}
