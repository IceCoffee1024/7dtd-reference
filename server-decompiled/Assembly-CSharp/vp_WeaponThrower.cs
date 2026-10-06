using System;
using UnityEngine;

// Token: 0x02001637 RID: 5687
public class vp_WeaponThrower : MonoBehaviour
{
	// Token: 0x17001515 RID: 5397
	// (get) Token: 0x0600B269 RID: 45673 RVA: 0x004295D0 File Offset: 0x004277D0
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

	// Token: 0x17001516 RID: 5398
	// (get) Token: 0x0600B26A RID: 45674 RVA: 0x004295F2 File Offset: 0x004277F2
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

	// Token: 0x17001517 RID: 5399
	// (get) Token: 0x0600B26B RID: 45675 RVA: 0x00429619 File Offset: 0x00427819
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

	// Token: 0x17001518 RID: 5400
	// (get) Token: 0x0600B26C RID: 45676 RVA: 0x0042964F File Offset: 0x0042784F
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

	// Token: 0x17001519 RID: 5401
	// (get) Token: 0x0600B26D RID: 45677 RVA: 0x00429688 File Offset: 0x00427888
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

	// Token: 0x1700151A RID: 5402
	// (get) Token: 0x0600B26E RID: 45678 RVA: 0x004296D0 File Offset: 0x004278D0
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

	// Token: 0x1700151B RID: 5403
	// (get) Token: 0x0600B26F RID: 45679 RVA: 0x0042976C File Offset: 0x0042796C
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

	// Token: 0x1700151C RID: 5404
	// (get) Token: 0x0600B270 RID: 45680 RVA: 0x004297A2 File Offset: 0x004279A2
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

	// Token: 0x1700151D RID: 5405
	// (get) Token: 0x0600B271 RID: 45681 RVA: 0x004297D8 File Offset: 0x004279D8
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

	// Token: 0x0600B272 RID: 45682 RVA: 0x00429810 File Offset: 0x00427A10
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

	// Token: 0x0600B273 RID: 45683 RVA: 0x00429862 File Offset: 0x00427A62
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		this.TryRestoreAttackMinDuration();
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B274 RID: 45684 RVA: 0x00429884 File Offset: 0x00427A84
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

	// Token: 0x0600B275 RID: 45685 RVA: 0x004299A8 File Offset: 0x00427BA8
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

	// Token: 0x0600B276 RID: 45686 RVA: 0x004299FD File Offset: 0x00427BFD
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

	// Token: 0x1700151E RID: 5406
	// (get) Token: 0x0600B277 RID: 45687 RVA: 0x00429A31 File Offset: 0x00427C31
	public bool HaveAmmoForCurrentWeapon
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.Player.CurrentWeaponAmmoCount.Get() > 0 || this.Player.CurrentWeaponClipCount.Get() > 0;
		}
	}

	// Token: 0x0600B278 RID: 45688 RVA: 0x00429A65 File Offset: 0x00427C65
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryReload()
	{
		return this.UnitBank != null && this.Inventory.TryReload(this.UnitBank);
	}

	// Token: 0x0600B279 RID: 45689 RVA: 0x00429A84 File Offset: 0x00427C84
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

	// Token: 0x0600B27A RID: 45690 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Reload()
	{
		return false;
	}

	// Token: 0x0600B27B RID: 45691 RVA: 0x00429B2A File Offset: 0x00427D2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Attack()
	{
		this.TryReload();
	}

	// Token: 0x0600B27C RID: 45692 RVA: 0x00429B33 File Offset: 0x00427D33
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_SetWeapon()
	{
		this.m_UnitBank = null;
	}

	// Token: 0x040085F7 RID: 34295
	public float AttackMinDuration = 1f;

	// Token: 0x040085F8 RID: 34296
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_OriginalAttackMinDuration;

	// Token: 0x040085F9 RID: 34297
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040085FA RID: 34298
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x040085FB RID: 34299
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Weapon m_Weapon;

	// Token: 0x040085FC RID: 34300
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponShooter m_Shooter;

	// Token: 0x040085FD RID: 34301
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_UnitBankType m_UnitBankType;

	// Token: 0x040085FE RID: 34302
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_UnitBankInstance m_UnitBank;

	// Token: 0x040085FF RID: 34303
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_ItemIdentifier m_ItemIdentifier;

	// Token: 0x04008600 RID: 34304
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008601 RID: 34305
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;
}
