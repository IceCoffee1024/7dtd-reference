using System;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x020004BC RID: 1212
[Preserve]
public class EntityNPC : EntityAlive
{
	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x0600266D RID: 9837 RVA: 0x000EB306 File Offset: 0x000E9506
	public override string LocalizedEntityName
	{
		get
		{
			return Localization.Get(this.EntityName, false, null);
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x0600266E RID: 9838 RVA: 0x000EB315 File Offset: 0x000E9515
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

	// Token: 0x0600266F RID: 9839 RVA: 0x000EB33C File Offset: 0x000E953C
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		if (entityClass.Properties.Values.ContainsKey(EntityClass.PropNPCID))
		{
			this.npcID = entityClass.Properties.Values[EntityClass.PropNPCID];
		}
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000EB392 File Offset: 0x000E9592
	public override void Read(byte _version, BinaryReader _br)
	{
		base.Read(_version, _br);
		if (_version < 33)
		{
			GameUtils.ReadItemStack(_br);
		}
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x000E4994 File Offset: 0x000E2B94
	public override bool IsSavedToFile()
	{
		return (base.GetSpawnerSource() != EnumSpawnerSource.Dynamic || this.IsDead()) && base.IsSavedToFile();
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x000EB3A8 File Offset: 0x000E95A8
	public override float GetSeeDistance()
	{
		return 80f;
	}

	// Token: 0x06002673 RID: 9843 RVA: 0x000EB3B0 File Offset: 0x000E95B0
	public override void VisiblityCheck(float _distanceSqr, bool _masterIsZooming)
	{
		bool flag = _distanceSqr < (float)(_masterIsZooming ? 14400 : 8100);
		this.emodel.SetVisible(flag, false);
	}

	// Token: 0x06002674 RID: 9844 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CanBePushed()
	{
		return false;
	}

	// Token: 0x06002675 RID: 9845 RVA: 0x000EB3DE File Offset: 0x000E95DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool canDespawn()
	{
		return this.world.GetPlayers().Count == 0 && base.canDespawn();
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isRadiationSensitive()
	{
		return false;
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isDetailedHeadBodyColliders()
	{
		return true;
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x06002679 RID: 9849 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PlayVoiceSetEntry(string name, EntityPlayer player, bool ignoreTime = true, bool showReactionAnim = true)
	{
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000EB3FC File Offset: 0x000E95FC
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

	// Token: 0x04001C78 RID: 7288
	public string npcID = "";
}
