using System;
using UnityEngine;

// Token: 0x02001530 RID: 5424
public class AnimationTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AA4D RID: 43597 RVA: 0x003F7CCE File Offset: 0x003F5ECE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.weaponPrefabCount = this.weaponPrefabs.Length;
	}

	// Token: 0x0600AA4E RID: 43598 RVA: 0x003F7CEC File Offset: 0x003F5EEC
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

	// Token: 0x0600AA4F RID: 43599 RVA: 0x003F7F44 File Offset: 0x003F6144
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

	// Token: 0x0600AA50 RID: 43600 RVA: 0x003F7FE4 File Offset: 0x003F61E4
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

	// Token: 0x0600AA51 RID: 43601 RVA: 0x003F8040 File Offset: 0x003F6240
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

	// Token: 0x04007E5D RID: 32349
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007E5E RID: 32350
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007E5F RID: 32351
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float oneHandMeleeTargetWeight;

	// Token: 0x04007E60 RID: 32352
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float oneHandPistolTargetWeight;

	// Token: 0x04007E61 RID: 32353
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007E62 RID: 32354
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float layerWeight;

	// Token: 0x04007E63 RID: 32355
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 200f;

	// Token: 0x04007E64 RID: 32356
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007E65 RID: 32357
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel = 1;

	// Token: 0x04007E66 RID: 32358
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponPrefabIndex;

	// Token: 0x04007E67 RID: 32359
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponJointChildrenCount;

	// Token: 0x04007E68 RID: 32360
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currOneHandMeleeWeight;

	// Token: 0x04007E69 RID: 32361
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currOneHandPistolWeight;

	// Token: 0x04007E6A RID: 32362
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCrouching;

	// Token: 0x04007E6B RID: 32363
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponPrefabCount;

	// Token: 0x04007E6C RID: 32364
	public GameObject[] weaponPrefabs;

	// Token: 0x04007E6D RID: 32365
	public int[] weaponHoldTypes;

	// Token: 0x04007E6E RID: 32366
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] locomotionSpeeds = new float[] { 0f, 2.08f, 4.2f };

	// Token: 0x04007E6F RID: 32367
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int locomotionState;

	// Token: 0x04007E70 RID: 32368
	public Transform weaponJoint;

	// Token: 0x04007E71 RID: 32369
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject existingChild;

	// Token: 0x04007E72 RID: 32370
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject newWeapon;

	// Token: 0x04007E73 RID: 32371
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosXRatio;

	// Token: 0x04007E74 RID: 32372
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosYRatio;

	// Token: 0x04007E75 RID: 32373
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosX;

	// Token: 0x04007E76 RID: 32374
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosY;

	// Token: 0x04007E77 RID: 32375
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float forward;

	// Token: 0x04007E78 RID: 32376
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float forwardGoal;

	// Token: 0x04007E79 RID: 32377
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float strafe;

	// Token: 0x04007E7A RID: 32378
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float YLook;

	// Token: 0x04007E7B RID: 32379
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float horizontalMax = 4.2f;

	// Token: 0x04007E7C RID: 32380
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float verticalMax = 4.2f;
}
