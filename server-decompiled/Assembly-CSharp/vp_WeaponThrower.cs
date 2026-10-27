using System;
using UnityEngine;

// Token: 0x02001649 RID: 5705
public class vp_WeaponThrower : MonoBehaviour
{
	// Token: 0x17001521 RID: 5409
	// (get) Token: 0x0600B2C1 RID: 45761 RVA: 0x0042B9D4 File Offset: 0x00429BD4
	public Transform Transform
	{
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x17001522 RID: 5410
	// (get) Token: 0x0600B2C2 RID: 45762 RVA: 0x0042B9F6 File Offset: 0x00429BF6
	public Transform Root
	{
		get
		{
			if (this.m_Root == null)
			{
				this.m_Root = this.Transform.root;
			}
			return this.m_Root;
		}
	}

	// Token: 0x17001523 RID: 5411
	// (get) Token: 0x0600B2C3 RID: 45763 RVA: 0x0042BA1D File Offset: 0x00429C1D
	public vp_Weapon Weapon
	{
		get
		{
			if (this.m_Weapon == null)
			{
				this.m_Weapon = (vp_Weapon)this.Transform.GetComponent(typeof(vp_Weapon));
			}
			return this.m_Weapon;
		}
	}

	// Token: 0x17001524 RID: 5412
	// (get) Token: 0x0600B2C4 RID: 45764 RVA: 0x0042BA53 File Offset: 0x00429C53
	public vp_WeaponShooter Shooter
	{
		get
		{
			if (this.m_Shooter == null)
			{
				this.m_Shooter = (vp_WeaponShooter)this.Transform.GetComponent(typeof(vp_WeaponShooter));
			}
			return this.m_Shooter;
		}
	}

	// Token: 0x17001525 RID: 5413
	// (get) Token: 0x0600B2C5 RID: 45765 RVA: 0x0042BA8C File Offset: 0x00429C8C
	public vp_UnitBankType UnitBankType
	{
		get
		{
			if (this.ItemIdentifier == null)
			{
				return null;
			}
			vp_ItemType itemType = this.m_ItemIdentifier.GetItemType();
			if (itemType == null)
			{
				return null;
			}
			vp_UnitBankType vp_UnitBankType = itemType as vp_UnitBankType;
			if (vp_UnitBankType == null)
			{
				return null;
			}
			return vp_UnitBankType;
		}
	}

	// Token: 0x17001526 RID: 5414
	// (get) Token: 0x0600B2C6 RID: 45766 RVA: 0x0042BAD4 File Offset: 0x00429CD4
	public vp_UnitBankInstance UnitBank
	{
		get
		{
			if (this.m_UnitBank == null && this.UnitBankType != null && this.Inventory != null)
			{
				foreach (vp_UnitBankInstance vp_UnitBankInstance in this.Inventory.UnitBankInstances)
				{
					if (vp_UnitBankInstance.UnitType == this.UnitBankType.Unit)
					{
						this.m_UnitBank = vp_UnitBankInstance;
					}
				}
			}
			return this.m_UnitBank;
		}
	}

	// Token: 0x17001527 RID: 5415
	// (get) Token: 0x0600B2C7 RID: 45767 RVA: 0x0042BB70 File Offset: 0x00429D70
	public vp_ItemIdentifier ItemIdentifier
	{
		get
		{
			if (this.m_ItemIdentifier == null)
			{
				this.m_ItemIdentifier = (vp_ItemIdentifier)this.Transform.GetComponent(typeof(vp_ItemIdentifier));
			}
			return this.m_ItemIdentifier;
		}
	}

	// Token: 0x17001528 RID: 5416
	// (get) Token: 0x0600B2C8 RID: 45768 RVA: 0x0042BBA6 File Offset: 0x00429DA6
	public vp_PlayerEventHandler Player
	{
		get
		{
			if (this.m_Player == null)
			{
				this.m_Player = (vp_PlayerEventHandler)this.Root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
			}
			return this.m_Player;
		}
	}

	// Token: 0x17001529 RID: 5417
	// (get) Token: 0x0600B2C9 RID: 45769 RVA: 0x0042BBDC File Offset: 0x00429DDC
	public vp_PlayerInventory Inventory
	{
		get
		{
			if (this.m_Inventory == null)
			{
				this.m_Inventory = (vp_PlayerInventory)this.Root.GetComponentInChildren(typeof(vp_PlayerInventory));
			}
			return this.m_Inventory;
		}
	}

	// Token: 0x0600B2CA RID: 45770 RVA: 0x0042BC14 File Offset: 0x00429E14
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player == null)
		{
			return;
		}
		this.Player.Register(this);
		this.TryStoreAttackMinDuration();
		this.Inventory.SetItemCap(this.ItemIdentifier.Type, 1, true);
		this.Inventory.CapsEnabled = true;
	}

	// Token: 0x0600B2CB RID: 45771 RVA: 0x0042BC66 File Offset: 0x00429E66
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.TryRestoreAttackMinDuration();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B2CC RID: 45772 RVA: 0x0042BC88 File Offset: 0x00429E88
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.TryStoreAttackMinDuration();
		if (this.Weapon == null)
		{
			Debug.LogError("Throwing weapon setup error (" + ((this != null) ? this.ToString() : null) + ") requires a vp_Weapon or vp_FPWeapon component.");
			return;
		}
		if (this.UnitBankType == null)
		{
			Debug.LogError("Throwing weapon setup error (" + ((this != null) ? this.ToString() : null) + ") requires a vp_ItemIdentifier component with a valid UnitBank.");
			return;
		}
		if (this.Weapon.AnimationType != 3)
		{
			string[] array = new string[5];
			array[0] = "Throwing weapon setup error (";
			array[1] = ((this != null) ? this.ToString() : null);
			array[2] = ") Please set 'Animation -> Type' of '";
			int num = 3;
			vp_Weapon weapon = this.Weapon;
			array[num] = ((weapon != null) ? weapon.ToString() : null);
			array[4] = "' item type to 'Thrown'.";
			Debug.LogError(string.Concat(array));
		}
		if (this.UnitBankType.Capacity != 1)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Throwing weapon setup error (",
				(this != null) ? this.ToString() : null,
				") Please set 'Capacity' for the '",
				this.UnitBankType.name,
				"' item type to '1'."
			}));
		}
	}

	// Token: 0x0600B2CD RID: 45773 RVA: 0x0042BDAC File Offset: 0x00429FAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void TryStoreAttackMinDuration()
	{
		if (this.Player.Attack == null)
		{
			return;
		}
		if (this.m_OriginalAttackMinDuration == 0f)
		{
			return;
		}
		this.m_OriginalAttackMinDuration = this.Player.Attack.MinDuration;
		this.Player.Attack.MinDuration = this.AttackMinDuration;
	}

	// Token: 0x0600B2CE RID: 45774 RVA: 0x0042BE01 File Offset: 0x0042A001
	[PublicizedFrom(EAccessModifier.Private)]
	public void TryRestoreAttackMinDuration()
	{
		if (this.Player.Attack == null)
		{
			return;
		}
		if (this.m_OriginalAttackMinDuration != 0f)
		{
			return;
		}
		this.Player.Attack.MinDuration = this.m_OriginalAttackMinDuration;
	}

	// Token: 0x1700152A RID: 5418
	// (get) Token: 0x0600B2CF RID: 45775 RVA: 0x0042BE35 File Offset: 0x0042A035
	public bool HaveAmmoForCurrentWeapon
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Player.CurrentWeaponAmmoCount.Get() > 0 || this.Player.CurrentWeaponClipCount.Get() > 0;
		}
	}

	// Token: 0x0600B2D0 RID: 45776 RVA: 0x0042BE69 File Offset: 0x0042A069
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryReload()
	{
		return this.UnitBank != null && this.Inventory.TryReload(this.UnitBank);
	}

	// Token: 0x0600B2D1 RID: 45777 RVA: 0x0042BE88 File Offset: 0x0042A088
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Attack()
	{
		if (!this.Player.IsFirstPerson.Get())
		{
			vp_Timer.In(this.Shooter.ProjectileSpawnDelay, delegate
			{
				this.Weapon.Weapon3rdPersonModel.GetComponent<Renderer>().enabled = false;
			}, null);
			vp_Timer.In(this.Shooter.ProjectileSpawnDelay + 1f, delegate
			{
				if (this.HaveAmmoForCurrentWeapon)
				{
					this.Weapon.Weapon3rdPersonModel.GetComponent<Renderer>().enabled = true;
				}
			}, null);
		}
		if (this.Player.CurrentWeaponAmmoCount.Get() < 1)
		{
			this.TryReload();
		}
		vp_Timer.In(this.Shooter.ProjectileSpawnDelay + 0.5f, delegate
		{
			this.Player.Attack.Stop(0f);
		}, null);
	}

	// Token: 0x0600B2D2 RID: 45778 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return false;
	}

	// Token: 0x0600B2D3 RID: 45779 RVA: 0x0042BF2E File Offset: 0x0042A12E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		this.TryReload();
	}

	// Token: 0x0600B2D4 RID: 45780 RVA: 0x0042BF37 File Offset: 0x0042A137
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_UnitBank = null;
	}

	// Token: 0x04008638 RID: 34360
	public float AttackMinDuration = 1f;

	// Token: 0x04008639 RID: 34361
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalAttackMinDuration;

	// Token: 0x0400863A RID: 34362
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400863B RID: 34363
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x0400863C RID: 34364
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x0400863D RID: 34365
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponShooter m_Shooter;

	// Token: 0x0400863E RID: 34366
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_UnitBankType m_UnitBankType;

	// Token: 0x0400863F RID: 34367
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_UnitBankInstance m_UnitBank;

	// Token: 0x04008640 RID: 34368
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_ItemIdentifier;

	// Token: 0x04008641 RID: 34369
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008642 RID: 34370
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;
}
