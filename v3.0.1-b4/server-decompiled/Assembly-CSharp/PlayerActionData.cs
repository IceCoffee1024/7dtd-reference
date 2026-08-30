using System;

// Token: 0x02000552 RID: 1362
public class PlayerActionData
{
	// Token: 0x0400221B RID: 8731
	public static readonly PlayerActionData.ActionTab TabMovement = new PlayerActionData.ActionTab("inpTabPlayerControl", 0);

	// Token: 0x0400221C RID: 8732
	public static readonly PlayerActionData.ActionTab TabToolbelt = new PlayerActionData.ActionTab("inpTabToolbelt", 10);

	// Token: 0x0400221D RID: 8733
	public static readonly PlayerActionData.ActionTab TabVehicle = new PlayerActionData.ActionTab("inpTabVehicle", 15);

	// Token: 0x0400221E RID: 8734
	public static readonly PlayerActionData.ActionTab TabMenus = new PlayerActionData.ActionTab("inpTabMenus", 20);

	// Token: 0x0400221F RID: 8735
	public static readonly PlayerActionData.ActionTab TabUi = new PlayerActionData.ActionTab("inpTabUi", 30);

	// Token: 0x04002220 RID: 8736
	public static readonly PlayerActionData.ActionTab TabOther = new PlayerActionData.ActionTab("inpTabOther", 40);

	// Token: 0x04002221 RID: 8737
	public static readonly PlayerActionData.ActionTab TabEdit = new PlayerActionData.ActionTab("inpTabEdit", 50);

	// Token: 0x04002222 RID: 8738
	public static readonly PlayerActionData.ActionTab TabGlobal = new PlayerActionData.ActionTab("inpTabGlobal", 60);

	// Token: 0x04002223 RID: 8739
	public static readonly PlayerActionData.ActionGroup GroupPlayerControl = new PlayerActionData.ActionGroup("inpGrpPlayerControlName", null, 0, PlayerActionData.TabMovement);

	// Token: 0x04002224 RID: 8740
	public static readonly PlayerActionData.ActionGroup GroupToolbelt = new PlayerActionData.ActionGroup("inpGrpToolbeltName", null, 10, PlayerActionData.TabToolbelt);

	// Token: 0x04002225 RID: 8741
	public static readonly PlayerActionData.ActionGroup GroupVehicle = new PlayerActionData.ActionGroup("inpGrpVehicleName", null, 15, PlayerActionData.TabVehicle);

	// Token: 0x04002226 RID: 8742
	public static readonly PlayerActionData.ActionGroup GroupMenu = new PlayerActionData.ActionGroup("inpGrpMenuName", null, 20, PlayerActionData.TabMenus);

	// Token: 0x04002227 RID: 8743
	public static readonly PlayerActionData.ActionGroup GroupDialogs = new PlayerActionData.ActionGroup("inpGrpDialogsName", null, 30, PlayerActionData.TabMenus);

	// Token: 0x04002228 RID: 8744
	public static readonly PlayerActionData.ActionGroup GroupUI = new PlayerActionData.ActionGroup("inpGrpUiName", null, 40, PlayerActionData.TabUi);

	// Token: 0x04002229 RID: 8745
	public static readonly PlayerActionData.ActionGroup GroupMp = new PlayerActionData.ActionGroup("inpGrpMpName", null, 50, PlayerActionData.TabOther);

	// Token: 0x0400222A RID: 8746
	public static readonly PlayerActionData.ActionGroup GroupAdmin = new PlayerActionData.ActionGroup("inpGrpAdminName", null, 60, PlayerActionData.TabOther);

	// Token: 0x0400222B RID: 8747
	public static readonly PlayerActionData.ActionGroup GroupGlobalFunctions = new PlayerActionData.ActionGroup("inpGrpGlobalFunctionsName", null, 80, PlayerActionData.TabGlobal);

	// Token: 0x0400222C RID: 8748
	public static readonly PlayerActionData.ActionGroup GroupDebugFunctions = new PlayerActionData.ActionGroup("inpGrpDebugFunctionsName", null, 100, PlayerActionData.TabGlobal);

	// Token: 0x0400222D RID: 8749
	public static readonly PlayerActionData.ActionGroup GroupEditCamera = new PlayerActionData.ActionGroup("inpGrpCameraName", null, 20, PlayerActionData.TabEdit);

	// Token: 0x0400222E RID: 8750
	public static readonly PlayerActionData.ActionGroup GroupEditSelection = new PlayerActionData.ActionGroup("inpGrpSelectionName", null, 40, PlayerActionData.TabEdit);

	// Token: 0x0400222F RID: 8751
	public static readonly PlayerActionData.ActionGroup GroupEditOther = new PlayerActionData.ActionGroup("inpGrpOtherName", null, 60, PlayerActionData.TabEdit);

	// Token: 0x02000553 RID: 1363
	public enum EAppliesToInputType
	{
		// Token: 0x04002231 RID: 8753
		None,
		// Token: 0x04002232 RID: 8754
		KbdMouseOnly,
		// Token: 0x04002233 RID: 8755
		ControllerOnly,
		// Token: 0x04002234 RID: 8756
		Both
	}

	// Token: 0x02000554 RID: 1364
	public class ActionSetUserData
	{
		// Token: 0x06002C7F RID: 11391 RVA: 0x00119C92 File Offset: 0x00117E92
		public ActionSetUserData(params PlayerActionsBase[] _bindingsConflictWithSet)
		{
			this.bindingsConflictWithSet = _bindingsConflictWithSet;
		}

		// Token: 0x04002235 RID: 8757
		public readonly PlayerActionsBase[] bindingsConflictWithSet;
	}

	// Token: 0x02000555 RID: 1365
	public class ActionTab : IComparable<PlayerActionData.ActionTab>
	{
		// Token: 0x06002C80 RID: 11392 RVA: 0x00119CA1 File Offset: 0x00117EA1
		public ActionTab(string _tabNameKey, int _tabPriority)
		{
			this.tabNameKey = _tabNameKey;
			this.tabPriority = _tabPriority;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x00119CB8 File Offset: 0x00117EB8
		public int CompareTo(PlayerActionData.ActionTab _other)
		{
			return this.tabPriority.CompareTo(_other.tabPriority);
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06002C82 RID: 11394 RVA: 0x00119CD9 File Offset: 0x00117ED9
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.tabNameKey, false, null);
			}
		}

		// Token: 0x04002236 RID: 8758
		public readonly string tabNameKey;

		// Token: 0x04002237 RID: 8759
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int tabPriority;
	}

	// Token: 0x02000556 RID: 1366
	public class ActionGroup : IComparable<PlayerActionData.ActionGroup>
	{
		// Token: 0x06002C83 RID: 11395 RVA: 0x00119CE8 File Offset: 0x00117EE8
		public ActionGroup(string _groupNameKey, string _groupDescKey, int _groupPriority, PlayerActionData.ActionTab _actionTab)
		{
			this.groupNameKey = _groupNameKey;
			this.groupDescKey = _groupDescKey ?? (this.groupNameKey.Replace("Name", "") + "Desc");
			this.groupPriority = _groupPriority;
			this.actionTab = _actionTab;
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x00119D3C File Offset: 0x00117F3C
		public int CompareTo(PlayerActionData.ActionGroup _other)
		{
			return this.groupPriority.CompareTo(_other.groupPriority);
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x00119D5D File Offset: 0x00117F5D
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.groupNameKey, false, null);
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x00119D6C File Offset: 0x00117F6C
		public string LocalizedDescription
		{
			get
			{
				string text = Localization.Get(this.groupDescKey, false, null);
				if (!(text != this.groupDescKey))
				{
					return null;
				}
				return text;
			}
		}

		// Token: 0x04002238 RID: 8760
		public readonly string groupNameKey;

		// Token: 0x04002239 RID: 8761
		public readonly string groupDescKey;

		// Token: 0x0400223A RID: 8762
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int groupPriority;

		// Token: 0x0400223B RID: 8763
		public readonly PlayerActionData.ActionTab actionTab;
	}

	// Token: 0x02000557 RID: 1367
	public class ActionUserData
	{
		// Token: 0x06002C87 RID: 11399 RVA: 0x00119D98 File Offset: 0x00117F98
		public ActionUserData(string _actionNameKey, string _actionDescKey, PlayerActionData.ActionGroup _actionGroup, PlayerActionData.EAppliesToInputType _appliesToInputType = PlayerActionData.EAppliesToInputType.Both, bool _allowRebind = true, bool _allowMultipleRebindings = false, bool _doNotDisplay = false, bool _defaultOnStartup = true)
		{
			this.actionNameKey = _actionNameKey;
			this.actionDescKey = _actionDescKey ?? (this.actionNameKey.Replace("Name", "") + "Desc");
			this.actionGroup = _actionGroup;
			this.appliesToInputType = _appliesToInputType;
			this.allowRebind = _allowRebind;
			this.allowMultipleBindings = _allowMultipleRebindings;
			this.doNotDisplay = _doNotDisplay;
			this.defaultOnStartup = _defaultOnStartup;
			if (this.actionGroup == null)
			{
				throw new ArgumentNullException("_actionGroup");
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x00119E1E File Offset: 0x0011801E
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.actionNameKey, false, null);
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x00119E30 File Offset: 0x00118030
		public string LocalizedDescription
		{
			get
			{
				string text = Localization.Get(this.actionDescKey, false, null);
				if (!(text != this.actionDescKey))
				{
					return null;
				}
				return text;
			}
		}

		// Token: 0x0400223C RID: 8764
		public readonly string actionNameKey;

		// Token: 0x0400223D RID: 8765
		public readonly string actionDescKey;

		// Token: 0x0400223E RID: 8766
		public readonly PlayerActionData.ActionGroup actionGroup;

		// Token: 0x0400223F RID: 8767
		public readonly PlayerActionData.EAppliesToInputType appliesToInputType;

		// Token: 0x04002240 RID: 8768
		public readonly bool allowRebind;

		// Token: 0x04002241 RID: 8769
		public readonly bool allowMultipleBindings;

		// Token: 0x04002242 RID: 8770
		public readonly bool doNotDisplay;

		// Token: 0x04002243 RID: 8771
		public readonly bool defaultOnStartup;
	}
}
