using System;
using UnityEngine;

// Token: 0x02001564 RID: 5476
public class AnimationTestSceneTools : MonoBehaviour
{
	// Token: 0x0600AB58 RID: 43864 RVA: 0x003FEF26 File Offset: 0x003FD126
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.weaponPrefabCount = this.weaponPrefabs.Length;
	}

	// Token: 0x0600AB59 RID: 43865 RVA: 0x003FEF44 File Offset: 0x003FD144
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

	// Token: 0x0600AB5A RID: 43866 RVA: 0x003FF19C File Offset: 0x003FD39C
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

	// Token: 0x0600AB5B RID: 43867 RVA: 0x003FF23C File Offset: 0x003FD43C
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

	// Token: 0x0600AB5C RID: 43868 RVA: 0x003FF298 File Offset: 0x003FD498
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

	// Token: 0x04007F21 RID: 32545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Animator anim;

	// Token: 0x04007F22 RID: 32546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int layerIndex;

	// Token: 0x04007F23 RID: 32547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float oneHandMeleeTargetWeight;

	// Token: 0x04007F24 RID: 32548
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float oneHandPistolTargetWeight;

	// Token: 0x04007F25 RID: 32549
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maxLayers;

	// Token: 0x04007F26 RID: 32550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float layerWeight;

	// Token: 0x04007F27 RID: 32551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float turnRate = 200f;

	// Token: 0x04007F28 RID: 32552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int totalModels;

	// Token: 0x04007F29 RID: 32553
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentModel = 1;

	// Token: 0x04007F2A RID: 32554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponPrefabIndex;

	// Token: 0x04007F2B RID: 32555
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponJointChildrenCount;

	// Token: 0x04007F2C RID: 32556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currOneHandMeleeWeight;

	// Token: 0x04007F2D RID: 32557
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currOneHandPistolWeight;

	// Token: 0x04007F2E RID: 32558
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isCrouching;

	// Token: 0x04007F2F RID: 32559
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int weaponPrefabCount;

	// Token: 0x04007F30 RID: 32560
	public GameObject[] weaponPrefabs;

	// Token: 0x04007F31 RID: 32561
	public int[] weaponHoldTypes;

	// Token: 0x04007F32 RID: 32562
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] locomotionSpeeds = new float[] { 0f, 2.08f, 4.2f };

	// Token: 0x04007F33 RID: 32563
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int locomotionState;

	// Token: 0x04007F34 RID: 32564
	public Transform weaponJoint;

	// Token: 0x04007F35 RID: 32565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject existingChild;

	// Token: 0x04007F36 RID: 32566
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GameObject newWeapon;

	// Token: 0x04007F37 RID: 32567
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosXRatio;

	// Token: 0x04007F38 RID: 32568
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosYRatio;

	// Token: 0x04007F39 RID: 32569
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosX;

	// Token: 0x04007F3A RID: 32570
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mousePosY;

	// Token: 0x04007F3B RID: 32571
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float forward;

	// Token: 0x04007F3C RID: 32572
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float forwardGoal;

	// Token: 0x04007F3D RID: 32573
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float strafe;

	// Token: 0x04007F3E RID: 32574
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float YLook;

	// Token: 0x04007F3F RID: 32575
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float horizontalMax = 4.2f;

	// Token: 0x04007F40 RID: 32576
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float verticalMax = 4.2f;
}
