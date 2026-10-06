using System;

// Token: 0x0200055D RID: 1373
public class PlayerActionData
{
	// Token: 0x04002255 RID: 8789
	public static readonly PlayerActionData.ActionTab TabMovement = new PlayerActionData.ActionTab("inpTabPlayerControl", 0);

	// Token: 0x04002256 RID: 8790
	public static readonly PlayerActionData.ActionTab TabToolbelt = new PlayerActionData.ActionTab("inpTabToolbelt", 10);

	// Token: 0x04002257 RID: 8791
	public static readonly PlayerActionData.ActionTab TabVehicle = new PlayerActionData.ActionTab("inpTabVehicle", 15);

	// Token: 0x04002258 RID: 8792
	public static readonly PlayerActionData.ActionTab TabMenus = new PlayerActionData.ActionTab("inpTabMenus", 20);

	// Token: 0x04002259 RID: 8793
	public static readonly PlayerActionData.ActionTab TabUi = new PlayerActionData.ActionTab("inpTabUi", 30);

	// Token: 0x0400225A RID: 8794
	public static readonly PlayerActionData.ActionTab TabOther = new PlayerActionData.ActionTab("inpTabOther", 40);

	// Token: 0x0400225B RID: 8795
	public static readonly PlayerActionData.ActionTab TabEdit = new PlayerActionData.ActionTab("inpTabEdit", 50);

	// Token: 0x0400225C RID: 8796
	public static readonly PlayerActionData.ActionTab TabGlobal = new PlayerActionData.ActionTab("inpTabGlobal", 60);

	// Token: 0x0400225D RID: 8797
	public static readonly PlayerActionData.ActionGroup GroupPlayerControl = new PlayerActionData.ActionGroup("inpGrpPlayerControlName", null, 0, PlayerActionData.TabMovement);

	// Token: 0x0400225E RID: 8798
	public static readonly PlayerActionData.ActionGroup GroupToolbelt = new PlayerActionData.ActionGroup("inpGrpToolbeltName", null, 10, PlayerActionData.TabToolbelt);

	// Token: 0x0400225F RID: 8799
	public static readonly PlayerActionData.ActionGroup GroupVehicle = new PlayerActionData.ActionGroup("inpGrpVehicleName", null, 15, PlayerActionData.TabVehicle);

	// Token: 0x04002260 RID: 8800
	public static readonly PlayerActionData.ActionGroup GroupMenu = new PlayerActionData.ActionGroup("inpGrpMenuName", null, 20, PlayerActionData.TabMenus);

	// Token: 0x04002261 RID: 8801
	public static readonly PlayerActionData.ActionGroup GroupDialogs = new PlayerActionData.ActionGroup("inpGrpDialogsName", null, 30, PlayerActionData.TabMenus);

	// Token: 0x04002262 RID: 8802
	public static readonly PlayerActionData.ActionGroup GroupUI = new PlayerActionData.ActionGroup("inpGrpUiName", null, 40, PlayerActionData.TabUi);

	// Token: 0x04002263 RID: 8803
	public static readonly PlayerActionData.ActionGroup GroupMp = new PlayerActionData.ActionGroup("inpGrpMpName", null, 50, PlayerActionData.TabOther);

	// Token: 0x04002264 RID: 8804
	public static readonly PlayerActionData.ActionGroup GroupAdmin = new PlayerActionData.ActionGroup("inpGrpAdminName", null, 60, PlayerActionData.TabOther);

	// Token: 0x04002265 RID: 8805
	public static readonly PlayerActionData.ActionGroup GroupGlobalFunctions = new PlayerActionData.ActionGroup("inpGrpGlobalFunctionsName", null, 80, PlayerActionData.TabGlobal);

	// Token: 0x04002266 RID: 8806
	public static readonly PlayerActionData.ActionGroup GroupDebugFunctions = new PlayerActionData.ActionGroup("inpGrpDebugFunctionsName", null, 100, PlayerActionData.TabGlobal);

	// Token: 0x04002267 RID: 8807
	public static readonly PlayerActionData.ActionGroup GroupEditCamera = new PlayerActionData.ActionGroup("inpGrpCameraName", null, 20, PlayerActionData.TabEdit);

	// Token: 0x04002268 RID: 8808
	public static readonly PlayerActionData.ActionGroup GroupEditSelection = new PlayerActionData.ActionGroup("inpGrpSelectionName", null, 40, PlayerActionData.TabEdit);

	// Token: 0x04002269 RID: 8809
	public static readonly PlayerActionData.ActionGroup GroupEditOther = new PlayerActionData.ActionGroup("inpGrpOtherName", null, 60, PlayerActionData.TabEdit);

	// Token: 0x0200055E RID: 1374
	public enum EAppliesToInputType
	{
		// Token: 0x0400226B RID: 8811
		None,
		// Token: 0x0400226C RID: 8812
		KbdMouseOnly,
		// Token: 0x0400226D RID: 8813
		ControllerOnly,
		// Token: 0x0400226E RID: 8814
		Both
	}

	// Token: 0x0200055F RID: 1375
	public class ActionSetUserData
	{
		// Token: 0x06002C9F RID: 11423 RVA: 0x0011BAD6 File Offset: 0x00119CD6
		public ActionSetUserData(params PlayerActionsBase[] _bindingsConflictWithSet)
		{
			this.bindingsConflictWithSet = _bindingsConflictWithSet;
		}

		// Token: 0x0400226F RID: 8815
		public readonly PlayerActionsBase[] bindingsConflictWithSet;
	}

	// Token: 0x02000560 RID: 1376
	public class ActionTab : IComparable<PlayerActionData.ActionTab>
	{
		// Token: 0x06002CA0 RID: 11424 RVA: 0x0011BAE5 File Offset: 0x00119CE5
		public ActionTab(string _tabNameKey, int _tabPriority)
		{
			this.tabNameKey = _tabNameKey;
			this.tabPriority = _tabPriority;
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x0011BAFC File Offset: 0x00119CFC
		public int CompareTo(PlayerActionData.ActionTab _other)
		{
			return this.tabPriority.CompareTo(_other.tabPriority);
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x0011BB1D File Offset: 0x00119D1D
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.tabNameKey, false, null);
			}
		}

		// Token: 0x04002270 RID: 8816
		public readonly string tabNameKey;

		// Token: 0x04002271 RID: 8817
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int tabPriority;
	}

	// Token: 0x02000561 RID: 1377
	public class ActionGroup : IComparable<PlayerActionData.ActionGroup>
	{
		// Token: 0x06002CA3 RID: 11427 RVA: 0x0011BB2C File Offset: 0x00119D2C
		public ActionGroup(string _groupNameKey, string _groupDescKey, int _groupPriority, PlayerActionData.ActionTab _actionTab)
		{
			this.groupNameKey = _groupNameKey;
			this.groupDescKey = _groupDescKey ?? (this.groupNameKey.Replace("Name", "") + "Desc");
			this.groupPriority = _groupPriority;
			this.actionTab = _actionTab;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x0011BB80 File Offset: 0x00119D80
		public int CompareTo(PlayerActionData.ActionGroup _other)
		{
			return this.groupPriority.CompareTo(_other.groupPriority);
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x0011BBA1 File Offset: 0x00119DA1
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.groupNameKey, false, null);
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002CA6 RID: 11430 RVA: 0x0011BBB0 File Offset: 0x00119DB0
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

		// Token: 0x04002272 RID: 8818
		public readonly string groupNameKey;

		// Token: 0x04002273 RID: 8819
		public readonly string groupDescKey;

		// Token: 0x04002274 RID: 8820
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int groupPriority;

		// Token: 0x04002275 RID: 8821
		public readonly PlayerActionData.ActionTab actionTab;
	}

	// Token: 0x02000562 RID: 1378
	public class ActionUserData
	{
		// Token: 0x06002CA7 RID: 11431 RVA: 0x0011BBDC File Offset: 0x00119DDC
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
		// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x0011BC62 File Offset: 0x00119E62
		public string LocalizedName
		{
			get
			{
				return Localization.Get(this.actionNameKey, false, null);
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x0011BC74 File Offset: 0x00119E74
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

		// Token: 0x04002276 RID: 8822
		public readonly string actionNameKey;

		// Token: 0x04002277 RID: 8823
		public readonly string actionDescKey;

		// Token: 0x04002278 RID: 8824
		public readonly PlayerActionData.ActionGroup actionGroup;

		// Token: 0x04002279 RID: 8825
		public readonly PlayerActionData.EAppliesToInputType appliesToInputType;

		// Token: 0x0400227A RID: 8826
		public readonly bool allowRebind;

		// Token: 0x0400227B RID: 8827
		public readonly bool allowMultipleBindings;

		// Token: 0x0400227C RID: 8828
		public readonly bool doNotDisplay;

		// Token: 0x0400227D RID: 8829
		public readonly bool defaultOnStartup;
	}
}
