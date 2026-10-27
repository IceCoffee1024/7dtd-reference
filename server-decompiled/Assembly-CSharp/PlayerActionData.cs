using System;

// Token: 0x02000561 RID: 1377
public class PlayerActionData
{
	// Token: 0x0400226C RID: 8812
	public static readonly PlayerActionData.ActionTab TabMovement = new PlayerActionData.ActionTab("inpTabPlayerControl", 0);

	// Token: 0x0400226D RID: 8813
	public static readonly PlayerActionData.ActionTab TabToolbelt = new PlayerActionData.ActionTab("inpTabToolbelt", 10);

	// Token: 0x0400226E RID: 8814
	public static readonly PlayerActionData.ActionTab TabVehicle = new PlayerActionData.ActionTab("inpTabVehicle", 15);

	// Token: 0x0400226F RID: 8815
	public static readonly PlayerActionData.ActionTab TabMenus = new PlayerActionData.ActionTab("inpTabMenus", 20);

	// Token: 0x04002270 RID: 8816
	public static readonly PlayerActionData.ActionTab TabUi = new PlayerActionData.ActionTab("inpTabUi", 30);

	// Token: 0x04002271 RID: 8817
	public static readonly PlayerActionData.ActionTab TabOther = new PlayerActionData.ActionTab("inpTabOther", 40);

	// Token: 0x04002272 RID: 8818
	public static readonly PlayerActionData.ActionTab TabEdit = new PlayerActionData.ActionTab("inpTabEdit", 50);

	// Token: 0x04002273 RID: 8819
	public static readonly PlayerActionData.ActionTab TabGlobal = new PlayerActionData.ActionTab("inpTabGlobal", 60);

	// Token: 0x04002274 RID: 8820
	public static readonly PlayerActionData.ActionGroup GroupPlayerControl = new PlayerActionData.ActionGroup("inpGrpPlayerControlName", null, 0, PlayerActionData.TabMovement);

	// Token: 0x04002275 RID: 8821
	public static readonly PlayerActionData.ActionGroup GroupToolbelt = new PlayerActionData.ActionGroup("inpGrpToolbeltName", null, 10, PlayerActionData.TabToolbelt);

	// Token: 0x04002276 RID: 8822
	public static readonly PlayerActionData.ActionGroup GroupVehicle = new PlayerActionData.ActionGroup("inpGrpVehicleName", null, 15, PlayerActionData.TabVehicle);

	// Token: 0x04002277 RID: 8823
	public static readonly PlayerActionData.ActionGroup GroupMenu = new PlayerActionData.ActionGroup("inpGrpMenuName", null, 20, PlayerActionData.TabMenus);

	// Token: 0x04002278 RID: 8824
	public static readonly PlayerActionData.ActionGroup GroupDialogs = new PlayerActionData.ActionGroup("inpGrpDialogsName", null, 30, PlayerActionData.TabMenus);

	// Token: 0x04002279 RID: 8825
	public static readonly PlayerActionData.ActionGroup GroupUI = new PlayerActionData.ActionGroup("inpGrpUiName", null, 40, PlayerActionData.TabUi);

	// Token: 0x0400227A RID: 8826
	public static readonly PlayerActionData.ActionGroup GroupMp = new PlayerActionData.ActionGroup("inpGrpMpName", null, 50, PlayerActionData.TabOther);

	// Token: 0x0400227B RID: 8827
	public static readonly PlayerActionData.ActionGroup GroupAdmin = new PlayerActionData.ActionGroup("inpGrpAdminName", null, 60, PlayerActionData.TabOther);

	// Token: 0x0400227C RID: 8828
	public static readonly PlayerActionData.ActionGroup GroupGlobalFunctions = new PlayerActionData.ActionGroup("inpGrpGlobalFunctionsName", null, 80, PlayerActionData.TabGlobal);

	// Token: 0x0400227D RID: 8829
	public static readonly PlayerActionData.ActionGroup GroupDebugFunctions = new PlayerActionData.ActionGroup("inpGrpDebugFunctionsName", null, 100, PlayerActionData.TabGlobal);

	// Token: 0x0400227E RID: 8830
	public static readonly PlayerActionData.ActionGroup GroupEditCamera = new PlayerActionData.ActionGroup("inpGrpCameraName", null, 20, PlayerActionData.TabEdit);

	// Token: 0x0400227F RID: 8831
	public static readonly PlayerActionData.ActionGroup GroupEditSelection = new PlayerActionData.ActionGroup("inpGrpSelectionName", null, 40, PlayerActionData.TabEdit);

	// Token: 0x04002280 RID: 8832
	public static readonly PlayerActionData.ActionGroup GroupEditOther = new PlayerActionData.ActionGroup("inpGrpOtherName", null, 60, PlayerActionData.TabEdit);

	// Token: 0x02000562 RID: 1378
	public enum EAppliesToInputType
	{
		// Token: 0x04002282 RID: 8834
		None,
		// Token: 0x04002283 RID: 8835
		KbdMouseOnly,
		// Token: 0x04002284 RID: 8836
		ControllerOnly,
		// Token: 0x04002285 RID: 8837
		Both
	}

	// Token: 0x02000563 RID: 1379
	public class ActionSetUserData
	{
		// Token: 0x06002CA7 RID: 11431 RVA: 0x0011C062 File Offset: 0x0011A262
		public ActionSetUserData(params PlayerActionsBase[] _bindingsConflictWithSet)
		{
			this.bindingsConflictWithSet = _bindingsConflictWithSet;
		}

		// Token: 0x04002286 RID: 8838
		public readonly PlayerActionsBase[] bindingsConflictWithSet;
	}

	// Token: 0x02000564 RID: 1380
	public class ActionTab : IComparable<PlayerActionData.ActionTab>
	{
		// Token: 0x06002CA8 RID: 11432 RVA: 0x0011C071 File Offset: 0x0011A271
		public ActionTab(string _tabNameKey, int _tabPriority)
		{
			this.tabNameKey = _tabNameKey;
			this.tabPriority = _tabPriority;
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x0011C088 File Offset: 0x0011A288
		public int CompareTo(PlayerActionData.ActionTab _other)
		{
			return this.tabPriority.CompareTo(_other.tabPriority);
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x0011C0A9 File Offset: 0x0011A2A9
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.tabNameKey, false, null);
			}
		}

		// Token: 0x04002287 RID: 8839
		public readonly string tabNameKey;

		// Token: 0x04002288 RID: 8840
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int tabPriority;
	}

	// Token: 0x02000565 RID: 1381
	public class ActionGroup : IComparable<PlayerActionData.ActionGroup>
	{
		// Token: 0x06002CAB RID: 11435 RVA: 0x0011C0B8 File Offset: 0x0011A2B8
		public ActionGroup(string _groupNameKey, string _groupDescKey, int _groupPriority, PlayerActionData.ActionTab _actionTab)
		{
			this.groupNameKey = _groupNameKey;
			this.groupDescKey = _groupDescKey ?? (this.groupNameKey.Replace("Name", "") + "Desc");
			this.groupPriority = _groupPriority;
			this.actionTab = _actionTab;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x0011C10C File Offset: 0x0011A30C
		public int CompareTo(PlayerActionData.ActionGroup _other)
		{
			return this.groupPriority.CompareTo(_other.groupPriority);
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06002CAD RID: 11437 RVA: 0x0011C12D File Offset: 0x0011A32D
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.groupNameKey, false, null);
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002CAE RID: 11438 RVA: 0x0011C13C File Offset: 0x0011A33C
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

		// Token: 0x04002289 RID: 8841
		public readonly string groupNameKey;

		// Token: 0x0400228A RID: 8842
		public readonly string groupDescKey;

		// Token: 0x0400228B RID: 8843
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int groupPriority;

		// Token: 0x0400228C RID: 8844
		public readonly PlayerActionData.ActionTab actionTab;
	}

	// Token: 0x02000566 RID: 1382
	public class ActionUserData
	{
		// Token: 0x06002CAF RID: 11439 RVA: 0x0011C168 File Offset: 0x0011A368
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

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x0011C1EE File Offset: 0x0011A3EE
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.actionNameKey, false, null);
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x0011C200 File Offset: 0x0011A400
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

		// Token: 0x0400228D RID: 8845
		public readonly string actionNameKey;

		// Token: 0x0400228E RID: 8846
		public readonly string actionDescKey;

		// Token: 0x0400228F RID: 8847
		public readonly PlayerActionData.ActionGroup actionGroup;

		// Token: 0x04002290 RID: 8848
		public readonly PlayerActionData.EAppliesToInputType appliesToInputType;

		// Token: 0x04002291 RID: 8849
		public readonly bool allowRebind;

		// Token: 0x04002292 RID: 8850
		public readonly bool allowMultipleBindings;

		// Token: 0x04002293 RID: 8851
		public readonly bool doNotDisplay;

		// Token: 0x04002294 RID: 8852
		public readonly bool defaultOnStartup;
	}
}
