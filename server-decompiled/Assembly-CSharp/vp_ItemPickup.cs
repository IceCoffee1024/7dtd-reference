using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015C4 RID: 5572
[RequireComponent(typeof(SphereCollider))]
[Serializable]
public class vp_ItemPickup : MonoBehaviour
{
	// Token: 0x17001422 RID: 5154
	// (get) Token: 0x0600AE47 RID: 44615 RVA: 0x0040EA47 File Offset: 0x0040CC47
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

	// Token: 0x17001423 RID: 5155
	// (get) Token: 0x0600AE48 RID: 44616 RVA: 0x0040EA73 File Offset: 0x0040CC73
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

	// Token: 0x17001424 RID: 5156
	// (get) Token: 0x0600AE49 RID: 44617 RVA: 0x0040EA9A File Offset: 0x0040CC9A
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

	// Token: 0x0600AE4A RID: 44618 RVA: 0x0040EAD8 File Offset: 0x0040CCD8
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

	// Token: 0x0600AE4B RID: 44619 RVA: 0x0040EB7C File Offset: 0x0040CD7C
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

	// Token: 0x0600AE4C RID: 44620 RVA: 0x0040EBF4 File Offset: 0x0040CDF4
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

	// Token: 0x0600AE4D RID: 44621 RVA: 0x0040EC68 File Offset: 0x0040CE68
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

	// Token: 0x0600AE4E RID: 44622 RVA: 0x0040EC98 File Offset: 0x0040CE98
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

	// Token: 0x0600AE4F RID: 44623 RVA: 0x0040EEE9 File Offset: 0x0040D0E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit()
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AE50 RID: 44624 RVA: 0x0040EEF4 File Offset: 0x0040D0F4
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

	// Token: 0x0600AE51 RID: 44625 RVA: 0x003F9F87 File Offset: 0x003F8187
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Die()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x0600AE52 RID: 44626 RVA: 0x0040F0A4 File Offset: 0x0040D2A4
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

	// Token: 0x040081B3 RID: 33203
	public int ID;

	// Token: 0x040081B4 RID: 33204
	public int Amount;

	// Token: 0x040081B5 RID: 33205
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Type m_ItemType;

	// Token: 0x040081B6 RID: 33206
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemType m_ItemTypeObject;

	// Token: 0x040081B7 RID: 33207
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081B8 RID: 33208
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.ItemSection m_Item;

	// Token: 0x040081B9 RID: 33209
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.RecipientTagsSection m_Recipient;

	// Token: 0x040081BA RID: 33210
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.SoundSection m_Sound;

	// Token: 0x040081BB RID: 33211
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_ItemPickup.MessageSection m_Messages;

	// Token: 0x040081BC RID: 33212
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x040081BD RID: 33213
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int m_PickedUpAmount;

	// Token: 0x040081BE RID: 33214
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040081BF RID: 33215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081C0 RID: 33216
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string MissingItemTypeError = "Warning: {0} has no ItemType object!";

	// Token: 0x040081C1 RID: 33217
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x040081C2 RID: 33218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<Collider, vp_Inventory> m_ColliderInventories = new Dictionary<Collider, vp_Inventory>();

	// Token: 0x020015C5 RID: 5573
	[Serializable]
	public class ItemSection
	{
		// Token: 0x040081C3 RID: 33219
		public vp_ItemType Type;
	}

	// Token: 0x020015C6 RID: 5574
	[Serializable]
	public class RecipientTagsSection
	{
		// Token: 0x040081C4 RID: 33220
		public List<string> Tags = new List<string>();
	}

	// Token: 0x020015C7 RID: 5575
	[Serializable]
	public class SoundSection
	{
		// Token: 0x040081C5 RID: 33221
		public AudioClip PickupSound;

		// Token: 0x040081C6 RID: 33222
		public bool PickupSoundSlomo = true;

		// Token: 0x040081C7 RID: 33223
		public AudioClip PickupFailSound;

		// Token: 0x040081C8 RID: 33224
		public bool FailSoundSlomo = true;
	}

	// Token: 0x020015C8 RID: 5576
	[Serializable]
	public class MessageSection
	{
		// Token: 0x040081C9 RID: 33225
		public string SuccessSingle = "Picked up {2}.";

		// Token: 0x040081CA RID: 33226
		public string SuccessMultiple = "Picked up {4} {1}s.";

		// Token: 0x040081CB RID: 33227
		public string FailSingle = "Can't pick up {2} right now.";

		// Token: 0x040081CC RID: 33228
		public string FailMultiple = "Can't pick up {4} {1}s right now.";
	}
}
