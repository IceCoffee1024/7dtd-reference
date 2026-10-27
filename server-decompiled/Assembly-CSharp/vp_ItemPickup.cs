using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015F8 RID: 5624
[RequireComponent(typeof(SphereCollider))]
[Serializable]
public class vp_ItemPickup : MonoBehaviour
{
	// Token: 0x17001473 RID: 5235
	// (get) Token: 0x0600AF52 RID: 44882 RVA: 0x00415C9F File Offset: 0x00413E9F
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

	// Token: 0x17001474 RID: 5236
	// (get) Token: 0x0600AF53 RID: 44883 RVA: 0x00415CCB File Offset: 0x00413ECB
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

	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x0600AF54 RID: 44884 RVA: 0x00415CF2 File Offset: 0x00413EF2
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

	// Token: 0x0600AF55 RID: 44885 RVA: 0x00415D30 File Offset: 0x00413F30
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

	// Token: 0x0600AF56 RID: 44886 RVA: 0x00415DD4 File Offset: 0x00413FD4
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

	// Token: 0x0600AF57 RID: 44887 RVA: 0x00415E4C File Offset: 0x0041404C
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

	// Token: 0x0600AF58 RID: 44888 RVA: 0x00415EC0 File Offset: 0x004140C0
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

	// Token: 0x0600AF59 RID: 44889 RVA: 0x00415EF0 File Offset: 0x004140F0
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

	// Token: 0x0600AF5A RID: 44890 RVA: 0x00416141 File Offset: 0x00414341
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit()
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AF5B RID: 44891 RVA: 0x0041614C File Offset: 0x0041434C
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

	// Token: 0x0600AF5C RID: 44892 RVA: 0x004011DF File Offset: 0x003FF3DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600AF5D RID: 44893 RVA: 0x004162FC File Offset: 0x004144FC
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

	// Token: 0x04008277 RID: 33399
	public int ID;

	// Token: 0x04008278 RID: 33400
	public int Amount;

	// Token: 0x04008279 RID: 33401
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_ItemType;

	// Token: 0x0400827A RID: 33402
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemType m_ItemTypeObject;

	// Token: 0x0400827B RID: 33403
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400827C RID: 33404
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.ItemSection m_Item;

	// Token: 0x0400827D RID: 33405
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.RecipientTagsSection m_Recipient;

	// Token: 0x0400827E RID: 33406
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.SoundSection m_Sound;

	// Token: 0x0400827F RID: 33407
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.MessageSection m_Messages;

	// Token: 0x04008280 RID: 33408
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x04008281 RID: 33409
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PickedUpAmount;

	// Token: 0x04008282 RID: 33410
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008283 RID: 33411
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008284 RID: 33412
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string MissingItemTypeError = "Warning: {0} has no ItemType object!";

	// Token: 0x04008285 RID: 33413
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x04008286 RID: 33414
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Inventory> m_ColliderInventories = new Dictionary<Collider, vp_Inventory>();

	// Token: 0x020015F9 RID: 5625
	[Serializable]
	public class ItemSection
	{
		// Token: 0x04008287 RID: 33415
		public vp_ItemType Type;
	}

	// Token: 0x020015FA RID: 5626
	[Serializable]
	public class RecipientTagsSection
	{
		// Token: 0x04008288 RID: 33416
		public List<string> Tags = new List<string>();
	}

	// Token: 0x020015FB RID: 5627
	[Serializable]
	public class SoundSection
	{
		// Token: 0x04008289 RID: 33417
		public AudioClip PickupSound;

		// Token: 0x0400828A RID: 33418
		public bool PickupSoundSlomo = true;

		// Token: 0x0400828B RID: 33419
		public AudioClip PickupFailSound;

		// Token: 0x0400828C RID: 33420
		public bool FailSoundSlomo = true;
	}

	// Token: 0x020015FC RID: 5628
	[Serializable]
	public class MessageSection
	{
		// Token: 0x0400828D RID: 33421
		public string SuccessSingle = "Picked up {2}.";

		// Token: 0x0400828E RID: 33422
		public string SuccessMultiple = "Picked up {4} {1}s.";

		// Token: 0x0400828F RID: 33423
		public string FailSingle = "Can't pick up {2} right now.";

		// Token: 0x04008290 RID: 33424
		public string FailMultiple = "Can't pick up {4} {1}s right now.";
	}
}
