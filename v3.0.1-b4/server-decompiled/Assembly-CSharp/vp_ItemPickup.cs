using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A9 RID: 5545
[RequireComponent(typeof(SphereCollider))]
[Serializable]
public class vp_ItemPickup : MonoBehaviour
{
	// Token: 0x170013E9 RID: 5097
	// (get) Token: 0x0600AD4A RID: 44362 RVA: 0x00409707 File Offset: 0x00407907
	public Type ItemType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_ItemType == null)
			{
				this.m_ItemType = this.m_Item.Type.GetType();
			}
			return this.m_ItemType;
		}
	}

	// Token: 0x170013EA RID: 5098
	// (get) Token: 0x0600AD4B RID: 44363 RVA: 0x00409733 File Offset: 0x00407933
	public vp_ItemType ItemTypeObject
	{
		get
		{
			if (this.m_ItemTypeObject == null)
			{
				this.m_ItemTypeObject = this.m_Item.Type;
			}
			return this.m_ItemTypeObject;
		}
	}

	// Token: 0x170013EB RID: 5099
	// (get) Token: 0x0600AD4C RID: 44364 RVA: 0x0040975A File Offset: 0x0040795A
	public AudioSource Audio
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Audio == null)
			{
				if (base.GetComponent<AudioSource>() == null)
				{
					base.gameObject.AddComponent<AudioSource>();
				}
				this.m_Audio = base.GetComponent<AudioSource>();
			}
			return this.m_Audio;
		}
	}

	// Token: 0x0600AD4D RID: 44365 RVA: 0x00409798 File Offset: 0x00407998
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (this.ItemType == typeof(vp_UnitType))
		{
			this.Amount = Mathf.Max(1, this.Amount);
		}
		base.GetComponent<Collider>().isTrigger = true;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Transform = base.transform;
		if (this.m_Sound.PickupSound != null || this.m_Sound.PickupFailSound != null)
		{
			this.Audio.clip = this.m_Sound.PickupSound;
			this.Audio.playOnAwake = false;
		}
	}

	// Token: 0x0600AD4E RID: 44366 RVA: 0x0040983C File Offset: 0x00407A3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_Depleted && !this.Audio.isPlaying)
		{
			base.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
		}
		if (!this.m_Depleted && this.m_Rigidbody != null && this.m_Rigidbody.IsSleeping() && !this.m_Rigidbody.isKinematic)
		{
			vp_Timer.In(0.5f, delegate
			{
				this.m_Rigidbody.isKinematic = true;
				foreach (Collider collider in base.GetComponents<Collider>())
				{
					if (!collider.isTrigger)
					{
						collider.enabled = false;
					}
				}
			}, null);
		}
	}

	// Token: 0x0600AD4F RID: 44367 RVA: 0x004098B4 File Offset: 0x00407AB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Rigidbody != null)
		{
			this.m_Rigidbody.isKinematic = false;
			foreach (Collider collider in base.GetComponents<Collider>())
			{
				if (!collider.isTrigger)
				{
					collider.enabled = true;
				}
			}
		}
		base.GetComponent<Renderer>().enabled = true;
		this.m_Depleted = false;
		this.m_AlreadyFailed = false;
		vp_GlobalEvent<vp_ItemPickup>.Send("NetworkRespawnPickup", this);
	}

	// Token: 0x0600AD50 RID: 44368 RVA: 0x00409928 File Offset: 0x00407B28
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerEnter(Collider col)
	{
		if (this.ItemType == null)
		{
			return;
		}
		if (!vp_Gameplay.isMaster)
		{
			return;
		}
		if (!base.GetComponent<Collider>().enabled)
		{
			return;
		}
		this.TryGiveTo(col);
	}

	// Token: 0x0600AD51 RID: 44369 RVA: 0x00409958 File Offset: 0x00407B58
	public void TryGiveTo(Collider col)
	{
		if (this.m_Depleted)
		{
			return;
		}
		vp_Inventory vp_Inventory;
		if (!vp_ItemPickup.m_ColliderInventories.TryGetValue(col, out vp_Inventory))
		{
			vp_Inventory = vp_TargetEventReturn<vp_Inventory>.SendUpwards(col, "GetInventory", vp_TargetEventOptions.DontRequireReceiver);
			vp_ItemPickup.m_ColliderInventories.Add(col, vp_Inventory);
		}
		if (vp_Inventory == null)
		{
			return;
		}
		if (this.m_Recipient.Tags.Count > 0 && !this.m_Recipient.Tags.Contains(col.gameObject.tag))
		{
			return;
		}
		bool flag = false;
		int num = vp_TargetEventReturn<vp_ItemType, int>.SendUpwards(col, "GetItemCount", this.m_Item.Type, vp_TargetEventOptions.DontRequireReceiver);
		if (this.ItemType == typeof(vp_ItemType))
		{
			flag = vp_TargetEventReturn<vp_ItemType, int, bool>.SendUpwards(col, "TryGiveItem", this.m_Item.Type, this.ID, vp_TargetEventOptions.DontRequireReceiver);
		}
		else if (this.ItemType == typeof(vp_UnitBankType))
		{
			flag = vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.SendUpwards(col, "TryGiveUnitBank", this.m_Item.Type as vp_UnitBankType, this.Amount, this.ID, vp_TargetEventOptions.DontRequireReceiver);
		}
		else if (this.ItemType == typeof(vp_UnitType))
		{
			flag = vp_TargetEventReturn<vp_UnitType, int, bool>.SendUpwards(col, "TryGiveUnits", this.m_Item.Type as vp_UnitType, this.Amount, vp_TargetEventOptions.DontRequireReceiver);
		}
		else if (this.ItemType.BaseType == typeof(vp_ItemType))
		{
			flag = vp_TargetEventReturn<vp_ItemType, int, bool>.SendUpwards(col, "TryGiveItem", this.m_Item.Type, this.ID, vp_TargetEventOptions.DontRequireReceiver);
		}
		else if (this.ItemType.BaseType == typeof(vp_UnitBankType))
		{
			flag = vp_TargetEventReturn<vp_UnitBankType, int, int, bool>.SendUpwards(col, "TryGiveUnitBank", this.m_Item.Type as vp_UnitBankType, this.Amount, this.ID, vp_TargetEventOptions.DontRequireReceiver);
		}
		else if (this.ItemType.BaseType == typeof(vp_UnitType))
		{
			flag = vp_TargetEventReturn<vp_UnitType, int, bool>.SendUpwards(col, "TryGiveUnits", this.m_Item.Type as vp_UnitType, this.Amount, vp_TargetEventOptions.DontRequireReceiver);
		}
		if (flag)
		{
			this.m_PickedUpAmount = vp_TargetEventReturn<vp_ItemType, int>.SendUpwards(col, "GetItemCount", this.m_Item.Type, vp_TargetEventOptions.DontRequireReceiver) - num;
			this.OnSuccess(col.transform);
			return;
		}
		this.OnFail(col.transform);
	}

	// Token: 0x0600AD52 RID: 44370 RVA: 0x00409BA9 File Offset: 0x00407DA9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit()
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AD53 RID: 44371 RVA: 0x00409BB4 File Offset: 0x00407DB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnSuccess(Transform recipient)
	{
		this.m_Depleted = true;
		if (this.m_Sound.PickupSound != null)
		{
			this.Audio.pitch = (this.m_Sound.PickupSoundSlomo ? Time.timeScale : 1f);
			this.Audio.Play();
		}
		base.GetComponent<Renderer>().enabled = false;
		string text;
		if (this.m_PickedUpAmount < 2 || this.ItemType == typeof(vp_UnitBankType) || this.ItemType.BaseType == typeof(vp_UnitBankType))
		{
			text = string.Format(this.m_Messages.SuccessSingle, new object[]
			{
				this.m_Item.Type.IndefiniteArticle,
				this.m_Item.Type.DisplayName,
				this.m_Item.Type.DisplayNameFull,
				this.m_Item.Type.Description,
				this.m_PickedUpAmount.ToString()
			});
		}
		else
		{
			text = string.Format(this.m_Messages.SuccessMultiple, new object[]
			{
				this.m_Item.Type.IndefiniteArticle,
				this.m_Item.Type.DisplayName,
				this.m_Item.Type.DisplayNameFull,
				this.m_Item.Type.Description,
				this.m_PickedUpAmount.ToString()
			});
		}
		vp_GlobalEvent<Transform, string>.Send("HUDText", recipient, text);
		if (vp_Gameplay.isMultiplayer && vp_Gameplay.isMaster)
		{
			vp_GlobalEvent<vp_ItemPickup, Transform>.Send("NetworkGivePickup", this, recipient);
		}
	}

	// Token: 0x0600AD54 RID: 44372 RVA: 0x003F4C47 File Offset: 0x003F2E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600AD55 RID: 44373 RVA: 0x00409D64 File Offset: 0x00407F64
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnFail(Transform recipient)
	{
		if (!this.m_AlreadyFailed && this.m_Sound.PickupFailSound != null)
		{
			this.Audio.pitch = (this.m_Sound.FailSoundSlomo ? Time.timeScale : 1f);
			this.Audio.PlayOneShot(this.m_Sound.PickupFailSound);
		}
		this.m_AlreadyFailed = true;
		string text;
		if (this.m_PickedUpAmount < 2 || this.ItemType == typeof(vp_UnitBankType) || this.ItemType.BaseType == typeof(vp_UnitBankType))
		{
			text = string.Format(this.m_Messages.FailSingle, new object[]
			{
				this.m_Item.Type.IndefiniteArticle,
				this.m_Item.Type.DisplayName,
				this.m_Item.Type.DisplayNameFull,
				this.m_Item.Type.Description,
				this.Amount.ToString()
			});
		}
		else
		{
			text = string.Format(this.m_Messages.FailMultiple, new object[]
			{
				this.m_Item.Type.IndefiniteArticle,
				this.m_Item.Type.DisplayName,
				this.m_Item.Type.DisplayNameFull,
				this.m_Item.Type.Description,
				this.Amount.ToString()
			});
		}
		vp_GlobalEvent<Transform, string>.Send("HUDText", recipient, text);
	}

	// Token: 0x040080DA RID: 32986
	public int ID;

	// Token: 0x040080DB RID: 32987
	public int Amount;

	// Token: 0x040080DC RID: 32988
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_ItemType;

	// Token: 0x040080DD RID: 32989
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemType m_ItemTypeObject;

	// Token: 0x040080DE RID: 32990
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040080DF RID: 32991
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.ItemSection m_Item;

	// Token: 0x040080E0 RID: 32992
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.RecipientTagsSection m_Recipient;

	// Token: 0x040080E1 RID: 32993
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.SoundSection m_Sound;

	// Token: 0x040080E2 RID: 32994
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.MessageSection m_Messages;

	// Token: 0x040080E3 RID: 32995
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x040080E4 RID: 32996
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PickedUpAmount;

	// Token: 0x040080E5 RID: 32997
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040080E6 RID: 32998
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040080E7 RID: 32999
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string MissingItemTypeError = "Warning: {0} has no ItemType object!";

	// Token: 0x040080E8 RID: 33000
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x040080E9 RID: 33001
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Inventory> m_ColliderInventories = new Dictionary<Collider, vp_Inventory>();

	// Token: 0x020015AA RID: 5546
	[Serializable]
	public class ItemSection
	{
		// Token: 0x040080EA RID: 33002
		public vp_ItemType Type;
	}

	// Token: 0x020015AB RID: 5547
	[Serializable]
	public class RecipientTagsSection
	{
		// Token: 0x040080EB RID: 33003
		public List<string> Tags = new List<string>();
	}

	// Token: 0x020015AC RID: 5548
	[Serializable]
	public class SoundSection
	{
		// Token: 0x040080EC RID: 33004
		public AudioClip PickupSound;

		// Token: 0x040080ED RID: 33005
		public bool PickupSoundSlomo = true;

		// Token: 0x040080EE RID: 33006
		public AudioClip PickupFailSound;

		// Token: 0x040080EF RID: 33007
		public bool FailSoundSlomo = true;
	}

	// Token: 0x020015AD RID: 5549
	[Serializable]
	public class MessageSection
	{
		// Token: 0x040080F0 RID: 33008
		public string SuccessSingle = "Picked up {2}.";

		// Token: 0x040080F1 RID: 33009
		public string SuccessMultiple = "Picked up {4} {1}s.";

		// Token: 0x040080F2 RID: 33010
		public string FailSingle = "Can't pick up {2} right now.";

		// Token: 0x040080F3 RID: 33011
		public string FailMultiple = "Can't pick up {4} {1}s right now.";
	}
}
