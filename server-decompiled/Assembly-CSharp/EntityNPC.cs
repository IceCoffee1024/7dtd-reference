using System;
using UnityEngine.Scripting;

// Token: 0x020004C8 RID: 1224
[Preserve]
public class EntityNPC : EntityAlive
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x0600268C RID: 9868 RVA: 0x000EC462 File Offset: 0x000EA662
	public override string LocalizedEntityName
	{
		get
		{
			return Localization.Get(this.EntityName, false, null);
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x0600268D RID: 9869 RVA: 0x000EC471 File Offset: 0x000EA671
	public NPCInfo NPCInfo
	{
		get
		{
			if (this.npcID != "")
			{
				return NPCInfo.npcInfoList[this.npcID];
			}
			return null;
		}
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x000EC498 File Offset: 0x000EA698
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		if (entityClass.Properties.Values.ContainsKey(EntityClass.PropNPCID))
		{
			this.npcID = entityClass.Properties.Values[EntityClass.PropNPCID];
		}
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x000EC4EE File Offset: 0x000EA6EE
	public override void Read(byte _version, PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_version, _br, _eStreamMode);
		if (_version < 33)
		{
			GameUtils.ReadItemStack(_br);
		}
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000E5D2C File Offset: 0x000E3F2C
	public override bool IsSavedToFile()
	{
		return (base.GetSpawnerSource() != EnumSpawnerSource.Dynamic || this.IsDead()) && base.IsSavedToFile();
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000EC505 File Offset: 0x000EA705
	public override float GetSeeDistance()
	{
		return 80f;
	}

	// Token: 0x06002692 RID: 9874 RVA: 0x000EC50C File Offset: 0x000EA70C
	public override void VisiblityCheck(float _distanceSqr, bool _masterIsZooming)
	{
		bool flag = _distanceSqr < (float)(_masterIsZooming ? 14400 : 8100);
		this.emodel.SetVisible(flag, false);
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanBePushed()
	{
		return false;
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000EC53A File Offset: 0x000EA73A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return this.world.GetPlayers().Count == 0 && base.canDespawn();
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PlayVoiceSetEntry(string name, EntityPlayer player, bool ignoreTime = true, bool showReactionAnim = true)
	{
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x000EC558 File Offset: 0x000EA758
	public override string GetActivationText()
	{
		GameManager instance = GameManager.Instance;
		EntityPlayerLocal entityPlayerLocal;
		if (instance == null)
		{
			entityPlayerLocal = null;
		}
		else
		{
			World world = instance.World;
			entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		}
		EntityPlayerLocal entityPlayerLocal2 = entityPlayerLocal;
		if (entityPlayerLocal2 == null)
		{
			return string.Empty;
		}
		string text = entityPlayerLocal2.playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + entityPlayerLocal2.playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		return string.Format(Localization.Get("npcTooltipTalk", false, null), text, this.LocalizedEntityName);
	}

	// Token: 0x04001CA7 RID: 7335
	public string npcID = "";
}
