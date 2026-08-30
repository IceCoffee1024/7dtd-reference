using System;

// Token: 0x02000557 RID: 1367
public class PlayerActionData
{
	// Token: 0x0400224D RID: 8781
	public static readonly PlayerActionData.ActionTab TabMovement = new PlayerActionData.ActionTab("inpTabPlayerControl", 0);

	// Token: 0x0400224E RID: 8782
	public static readonly PlayerActionData.ActionTab TabToolbelt = new PlayerActionData.ActionTab("inpTabToolbelt", 10);

	// Token: 0x0400224F RID: 8783
	public static readonly PlayerActionData.ActionTab TabVehicle = new PlayerActionData.ActionTab("inpTabVehicle", 15);

	// Token: 0x04002250 RID: 8784
	public static readonly PlayerActionData.ActionTab TabMenus = new PlayerActionData.ActionTab("inpTabMenus", 20);

	// Token: 0x04002251 RID: 8785
	public static readonly PlayerActionData.ActionTab TabUi = new PlayerActionData.ActionTab("inpTabUi", 30);

	// Token: 0x04002252 RID: 8786
	public static readonly PlayerActionData.ActionTab TabOther = new PlayerActionData.ActionTab("inpTabOther", 40);

	// Token: 0x04002253 RID: 8787
	public static readonly PlayerActionData.ActionTab TabEdit = new PlayerActionData.ActionTab("inpTabEdit", 50);

	// Token: 0x04002254 RID: 8788
	public static readonly PlayerActionData.ActionTab TabGlobal = new PlayerActionData.ActionTab("inpTabGlobal", 60);

	// Token: 0x04002255 RID: 8789
	public static readonly PlayerActionData.ActionGroup GroupPlayerControl = new PlayerActionData.ActionGroup("inpGrpPlayerControlName", null, 0, PlayerActionData.TabMovement);

	// Token: 0x04002256 RID: 8790
	public static readonly PlayerActionData.ActionGroup GroupToolbelt = new PlayerActionData.ActionGroup("inpGrpToolbeltName", null, 10, PlayerActionData.TabToolbelt);

	// Token: 0x04002257 RID: 8791
	public static readonly PlayerActionData.ActionGroup GroupVehicle = new PlayerActionData.ActionGroup("inpGrpVehicleName", null, 15, PlayerActionData.TabVehicle);

	// Token: 0x04002258 RID: 8792
	public static readonly PlayerActionData.ActionGroup GroupMenu = new PlayerActionData.ActionGroup("inpGrpMenuName", null, 20, PlayerActionData.TabMenus);

	// Token: 0x04002259 RID: 8793
	public static readonly PlayerActionData.ActionGroup GroupDialogs = new PlayerActionData.ActionGroup("inpGrpDialogsName", null, 30, PlayerActionData.TabMenus);

	// Token: 0x0400225A RID: 8794
	public static readonly PlayerActionData.ActionGroup GroupUI = new PlayerActionData.ActionGroup("inpGrpUiName", null, 40, PlayerActionData.TabUi);

	// Token: 0x0400225B RID: 8795
	public static readonly PlayerActionData.ActionGroup GroupMp = new PlayerActionData.ActionGroup("inpGrpMpName", null, 50, PlayerActionData.TabOther);

	// Token: 0x0400225C RID: 8796
	public static readonly PlayerActionData.ActionGroup GroupAdmin = new PlayerActionData.ActionGroup("inpGrpAdminName", null, 60, PlayerActionData.TabOther);

	// Token: 0x0400225D RID: 8797
	public static readonly PlayerActionData.ActionGroup GroupGlobalFunctions = new PlayerActionData.ActionGroup("inpGrpGlobalFunctionsName", null, 80, PlayerActionData.TabGlobal);

	// Token: 0x0400225E RID: 8798
	public static readonly PlayerActionData.ActionGroup GroupDebugFunctions = new PlayerActionData.ActionGroup("inpGrpDebugFunctionsName", null, 100, PlayerActionData.TabGlobal);

	// Token: 0x0400225F RID: 8799
	public static readonly PlayerActionData.ActionGroup GroupEditCamera = new PlayerActionData.ActionGroup("inpGrpCameraName", null, 20, PlayerActionData.TabEdit);

	// Token: 0x04002260 RID: 8800
	public static readonly PlayerActionData.ActionGroup GroupEditSelection = new PlayerActionData.ActionGroup("inpGrpSelectionName", null, 40, PlayerActionData.TabEdit);

	// Token: 0x04002261 RID: 8801
	public static readonly PlayerActionData.ActionGroup GroupEditOther = new PlayerActionData.ActionGroup("inpGrpOtherName", null, 60, PlayerActionData.TabEdit);

	// Token: 0x02000558 RID: 1368
	public enum EAppliesToInputType
	{
		// Token: 0x04002263 RID: 8803
		None,
		// Token: 0x04002264 RID: 8804
		KbdMouseOnly,
		// Token: 0x04002265 RID: 8805
		ControllerOnly,
		// Token: 0x04002266 RID: 8806
		Both
	}

	// Token: 0x02000559 RID: 1369
	public class ActionSetUserData
	{
		// Token: 0x06002CA9 RID: 11433 RVA: 0x0011AD06 File Offset: 0x00118F06
		public ActionSetUserData(params PlayerActionsBase[] _bindingsConflictWithSet)
		{
			this.bindingsConflictWithSet = _bindingsConflictWithSet;
		}

		// Token: 0x04002267 RID: 8807
		public readonly PlayerActionsBase[] bindingsConflictWithSet;
	}

	// Token: 0x0200055A RID: 1370
	public class ActionTab : IComparable<PlayerActionData.ActionTab>
	{
		// Token: 0x06002CAA RID: 11434 RVA: 0x0011AD15 File Offset: 0x00118F15
		public ActionTab(string _tabNameKey, int _tabPriority)
		{
			this.tabNameKey = _tabNameKey;
			this.tabPriority = _tabPriority;
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x0011AD2C File Offset: 0x00118F2C
		public int CompareTo(PlayerActionData.ActionTab _other)
		{
			return this.tabPriority.CompareTo(_other.tabPriority);
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06002CAC RID: 11436 RVA: 0x0011AD4D File Offset: 0x00118F4D
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.tabNameKey, false, null);
			}
		}

		// Token: 0x04002268 RID: 8808
		public readonly string tabNameKey;

		// Token: 0x04002269 RID: 8809
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int tabPriority;
	}

	// Token: 0x0200055B RID: 1371
	public class ActionGroup : IComparable<PlayerActionData.ActionGroup>
	{
		// Token: 0x06002CAD RID: 11437 RVA: 0x0011AD5C File Offset: 0x00118F5C
		public ActionGroup(string _groupNameKey, string _groupDescKey, int _groupPriority, PlayerActionData.ActionTab _actionTab)
		{
			this.groupNameKey = _groupNameKey;
			this.groupDescKey = _groupDescKey ?? (this.groupNameKey.Replace("Name", "") + "Desc");
			this.groupPriority = _groupPriority;
			this.actionTab = _actionTab;
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x0011ADB0 File Offset: 0x00118FB0
		public int CompareTo(PlayerActionData.ActionGroup _other)
		{
			return this.groupPriority.CompareTo(_other.groupPriority);
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06002CAF RID: 11439 RVA: 0x0011ADD1 File Offset: 0x00118FD1
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.groupNameKey, false, null);
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x0011ADE0 File Offset: 0x00118FE0
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

		// Token: 0x0400226A RID: 8810
		public readonly string groupNameKey;

		// Token: 0x0400226B RID: 8811
		public readonly string groupDescKey;

		// Token: 0x0400226C RID: 8812
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int groupPriority;

		// Token: 0x0400226D RID: 8813
		public readonly PlayerActionData.ActionTab actionTab;
	}

	// Token: 0x0200055C RID: 1372
	public class ActionUserData
	{
		// Token: 0x06002CB1 RID: 11441 RVA: 0x0011AE0C File Offset: 0x0011900C
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

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x0011AE92 File Offset: 0x00119092
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.actionNameKey, false, null);
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x0011AEA4 File Offset: 0x001190A4
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

		// Token: 0x0400226E RID: 8814
		public readonly string actionNameKey;

		// Token: 0x0400226F RID: 8815
		public readonly string actionDescKey;

		// Token: 0x04002270 RID: 8816
		public readonly PlayerActionData.ActionGroup actionGroup;

		// Token: 0x04002271 RID: 8817
		public readonly PlayerActionData.EAppliesToInputType appliesToInputType;

		// Token: 0x04002272 RID: 8818
		public readonly bool allowRebind;

		// Token: 0x04002273 RID: 8819
		public readonly bool allowMultipleBindings;

		// Token: 0x04002274 RID: 8820
		public readonly bool doNotDisplay;

		// Token: 0x04002275 RID: 8821
		public readonly bool defaultOnStartup;
	}
}
