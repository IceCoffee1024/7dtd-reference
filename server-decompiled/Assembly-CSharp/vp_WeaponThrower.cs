using System;
using UnityEngine;

// Token: 0x02001618 RID: 5656
public class vp_WeaponThrower : MonoBehaviour
{
	// Token: 0x170014D2 RID: 5330
	// (get) Token: 0x0600B1D7 RID: 45527 RVA: 0x004258F4 File Offset: 0x00423AF4
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

	// Token: 0x170014D3 RID: 5331
	// (get) Token: 0x0600B1D8 RID: 45528 RVA: 0x00425916 File Offset: 0x00423B16
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

	// Token: 0x170014D4 RID: 5332
	// (get) Token: 0x0600B1D9 RID: 45529 RVA: 0x0042593D File Offset: 0x00423B3D
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

	// Token: 0x170014D5 RID: 5333
	// (get) Token: 0x0600B1DA RID: 45530 RVA: 0x00425973 File Offset: 0x00423B73
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

	// Token: 0x170014D6 RID: 5334
	// (get) Token: 0x0600B1DB RID: 45531 RVA: 0x004259AC File Offset: 0x00423BAC
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

	// Token: 0x170014D7 RID: 5335
	// (get) Token: 0x0600B1DC RID: 45532 RVA: 0x004259F4 File Offset: 0x00423BF4
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

	// Token: 0x170014D8 RID: 5336
	// (get) Token: 0x0600B1DD RID: 45533 RVA: 0x00425A90 File Offset: 0x00423C90
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

	// Token: 0x170014D9 RID: 5337
	// (get) Token: 0x0600B1DE RID: 45534 RVA: 0x00425AC6 File Offset: 0x00423CC6
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

	// Token: 0x170014DA RID: 5338
	// (get) Token: 0x0600B1DF RID: 45535 RVA: 0x00425AFC File Offset: 0x00423CFC
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

	// Token: 0x0600B1E0 RID: 45536 RVA: 0x00425B34 File Offset: 0x00423D34
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

	// Token: 0x0600B1E1 RID: 45537 RVA: 0x00425B86 File Offset: 0x00423D86
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.TryRestoreAttackMinDuration();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1E2 RID: 45538 RVA: 0x00425BA8 File Offset: 0x00423DA8
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

	// Token: 0x0600B1E3 RID: 45539 RVA: 0x00425CCC File Offset: 0x00423ECC
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

	// Token: 0x0600B1E4 RID: 45540 RVA: 0x00425D21 File Offset: 0x00423F21
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

	// Token: 0x170014DB RID: 5339
	// (get) Token: 0x0600B1E5 RID: 45541 RVA: 0x00425D55 File Offset: 0x00423F55
	public bool HaveAmmoForCurrentWeapon
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Player.CurrentWeaponAmmoCount.Get() > 0 || this.Player.CurrentWeaponClipCount.Get() > 0;
		}
	}

	// Token: 0x0600B1E6 RID: 45542 RVA: 0x00425D89 File Offset: 0x00423F89
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryReload()
	{
		return this.UnitBank != null && this.Inventory.TryReload(this.UnitBank);
	}

	// Token: 0x0600B1E7 RID: 45543 RVA: 0x00425DA8 File Offset: 0x00423FA8
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

	// Token: 0x0600B1E8 RID: 45544 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return false;
	}

	// Token: 0x0600B1E9 RID: 45545 RVA: 0x00425E4E File Offset: 0x0042404E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		this.TryReload();
	}

	// Token: 0x0600B1EA RID: 45546 RVA: 0x00425E57 File Offset: 0x00424057
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_UnitBank = null;
	}

	// Token: 0x040085A1 RID: 34209
	public float AttackMinDuration = 1f;

	// Token: 0x040085A2 RID: 34210
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalAttackMinDuration;

	// Token: 0x040085A3 RID: 34211
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040085A4 RID: 34212
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x040085A5 RID: 34213
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x040085A6 RID: 34214
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponShooter m_Shooter;

	// Token: 0x040085A7 RID: 34215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_UnitBankType m_UnitBankType;

	// Token: 0x040085A8 RID: 34216
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_UnitBankInstance m_UnitBank;

	// Token: 0x040085A9 RID: 34217
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_ItemIdentifier;

	// Token: 0x040085AA RID: 34218
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040085AB RID: 34219
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;
}
