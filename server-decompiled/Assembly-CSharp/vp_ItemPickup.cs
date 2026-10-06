using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E6 RID: 5606
[RequireComponent(typeof(SphereCollider))]
[Serializable]
public class vp_ItemPickup : MonoBehaviour
{
	// Token: 0x17001467 RID: 5223
	// (get) Token: 0x0600AEFA RID: 44794 RVA: 0x0041389B File Offset: 0x00411A9B
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

	// Token: 0x17001468 RID: 5224
	// (get) Token: 0x0600AEFB RID: 44795 RVA: 0x004138C7 File Offset: 0x00411AC7
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

	// Token: 0x17001469 RID: 5225
	// (get) Token: 0x0600AEFC RID: 44796 RVA: 0x004138EE File Offset: 0x00411AEE
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

	// Token: 0x0600AEFD RID: 44797 RVA: 0x0041392C File Offset: 0x00411B2C
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

	// Token: 0x0600AEFE RID: 44798 RVA: 0x004139D0 File Offset: 0x00411BD0
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

	// Token: 0x0600AEFF RID: 44799 RVA: 0x00413A48 File Offset: 0x00411C48
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

	// Token: 0x0600AF00 RID: 44800 RVA: 0x00413ABC File Offset: 0x00411CBC
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

	// Token: 0x0600AF01 RID: 44801 RVA: 0x00413AEC File Offset: 0x00411CEC
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

	// Token: 0x0600AF02 RID: 44802 RVA: 0x00413D3D File Offset: 0x00411F3D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit()
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AF03 RID: 44803 RVA: 0x00413D48 File Offset: 0x00411F48
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

	// Token: 0x0600AF04 RID: 44804 RVA: 0x003FEDDB File Offset: 0x003FCFDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600AF05 RID: 44805 RVA: 0x00413EF8 File Offset: 0x004120F8
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

	// Token: 0x04008236 RID: 33334
	public int ID;

	// Token: 0x04008237 RID: 33335
	public int Amount;

	// Token: 0x04008238 RID: 33336
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_ItemType;

	// Token: 0x04008239 RID: 33337
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemType m_ItemTypeObject;

	// Token: 0x0400823A RID: 33338
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400823B RID: 33339
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.ItemSection m_Item;

	// Token: 0x0400823C RID: 33340
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.RecipientTagsSection m_Recipient;

	// Token: 0x0400823D RID: 33341
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.SoundSection m_Sound;

	// Token: 0x0400823E RID: 33342
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.MessageSection m_Messages;

	// Token: 0x0400823F RID: 33343
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x04008240 RID: 33344
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PickedUpAmount;

	// Token: 0x04008241 RID: 33345
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008242 RID: 33346
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008243 RID: 33347
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string MissingItemTypeError = "Warning: {0} has no ItemType object!";

	// Token: 0x04008244 RID: 33348
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x04008245 RID: 33349
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Inventory> m_ColliderInventories = new Dictionary<Collider, vp_Inventory>();

	// Token: 0x020015E7 RID: 5607
	[Serializable]
	public class ItemSection
	{
		// Token: 0x04008246 RID: 33350
		public vp_ItemType Type;
	}

	// Token: 0x020015E8 RID: 5608
	[Serializable]
	public class RecipientTagsSection
	{
		// Token: 0x04008247 RID: 33351
		public List<string> Tags = new List<string>();
	}

	// Token: 0x020015E9 RID: 5609
	[Serializable]
	public class SoundSection
	{
		// Token: 0x04008248 RID: 33352
		public AudioClip PickupSound;

		// Token: 0x04008249 RID: 33353
		public bool PickupSoundSlomo = true;

		// Token: 0x0400824A RID: 33354
		public AudioClip PickupFailSound;

		// Token: 0x0400824B RID: 33355
		public bool FailSoundSlomo = true;
	}

	// Token: 0x020015EA RID: 5610
	[Serializable]
	public class MessageSection
	{
		// Token: 0x0400824C RID: 33356
		public string SuccessSingle = "Picked up {2}.";

		// Token: 0x0400824D RID: 33357
		public string SuccessMultiple = "Picked up {4} {1}s.";

		// Token: 0x0400824E RID: 33358
		public string FailSingle = "Can't pick up {2} right now.";

		// Token: 0x0400824F RID: 33359
		public string FailMultiple = "Can't pick up {4} {1}s right now.";
	}
}
