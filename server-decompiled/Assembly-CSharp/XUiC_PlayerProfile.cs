using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E37 RID: 3639
[Preserve]
public class XUiC_PlayerProfile : XUiC_PlayGameDialogBase
{
	// Token: 0x06006F69 RID: 28521 RVA: 0x002B3E85 File Offset: 0x002B2085
	public override void Init()
	{
		base.Init();
		XUiC_PlayerProfile.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006F6A RID: 28522 RVA: 0x002B3E9D File Offset: 0x002B209D
	[XuiBindEvent("OnPress", "btnProfileEdit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnProfileEdit_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openCustomCharacterWindow();
	}

	// Token: 0x06006F6B RID: 28523 RVA: 0x002B3EA8 File Offset: 0x002B20A8
	[XuiBindEvent("OnPress", "btnProfileDelete")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnProfileDelete_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_ProfilesList.ListEntry selectedEntry = this.profiles.SelectedEntryData;
		Action <>9__1;
		Action <>9__2;
		XUiC_MessageBoxWindowGroup.ShowCustom(this.xui, Localization.Get("xuiDeleteProfile", false, null), string.Format(Localization.Get("xuiProfilesDeleteConfirmation", false, null), Utils.EscapeBbCodes(selectedEntry.Name, false, false)), "", delegate(XUiC_MessageBoxWindowGroup _box)
		{
			XUiC_MessageBoxWindowGroup.MessageButton messageButton = _box.Buttons[0];
			string text = "btnConfirm";
			Action action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate
				{
					this.btnProfileDelete.SelectCursorElement(false, false);
					ProfileSDF.DeleteProfile(selectedEntry.Name);
					ProfileSDF.SetSelectedProfile("");
					this.profiles.RebuildList(false);
					this.selectFirstProfile();
					this.IsDirty = true;
				});
			}
			messageButton.DefaultConfirm(text, action, true, 0f, 1.5f, false);
			XUiC_MessageBoxWindowGroup.MessageButton messageButton2 = _box.Buttons[2];
			string text2 = "xuiCancel";
			Action action2;
			if ((action2 = <>9__2) == null)
			{
				action2 = (<>9__2 = delegate
				{
					this.btnProfileDelete.SelectCursorElement(false, false);
				});
			}
			messageButton2.DefaultCancel(text2, action2, true, 0f, 0f, false);
		}, false, false, -1, -1);
	}

	// Token: 0x06006F6C RID: 28524 RVA: 0x002B3F22 File Offset: 0x002B2122
	[XuiBindEvent("OnPress", "btnProfileCreate")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnProfileCreate_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_PlayerProfileCreate.Open(this.xui, delegate(string _name)
		{
			if (_name == null)
			{
				this.btnProfileCreate.SelectCursorElement(false, false);
				return;
			}
			ProfileSDF.SaveProfile(_name, "", true, "White", 1, "Blue01", "", "", "", "", "");
			ProfileSDF.SetSelectedProfile(_name);
			ProfileSDF.Save();
			this.profiles.RebuildList(false);
			this.openCustomCharacterWindow();
		});
		this.xui.playerUI.windowManager.Open("playerProfilesCreate", false);
	}

	// Token: 0x06006F6D RID: 28525 RVA: 0x002B3F56 File Offset: 0x002B2156
	[XuiBindEvent("SelectionChanged", "profiles")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Profiles_OnSelectionChanged(XUiC_List<XUiC_ProfilesList.ListEntry> _list, XUiC_ProfilesList.ListEntry _previousEntry, XUiC_ProfilesList.ListEntry _newEntry)
	{
		if (_newEntry != null)
		{
			ProfileSDF.SetSelectedProfile(_newEntry.Name);
			ProfileSDF.Save();
		}
		this.IsDirty = true;
	}

	// Token: 0x06006F6E RID: 28526 RVA: 0x002B3F72 File Offset: 0x002B2172
	[PublicizedFrom(EAccessModifier.Private)]
	public void openCustomCharacterWindow()
	{
		this.previewWindow.ViewComponent.IsVisible = false;
		XUiC_CustomCharacterWindowGroup.Open(this.xui, delegate
		{
			this.previewWindow.ViewComponent.IsVisible = true;
		});
	}

	// Token: 0x06006F6F RID: 28527 RVA: 0x002B3F9C File Offset: 0x002B219C
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectFirstProfile()
	{
		string text = ProfileSDF.CurrentProfileName();
		if (!string.IsNullOrEmpty(text))
		{
			return;
		}
		using (IEnumerator<XUiC_ProfilesList.ListEntry> enumerator = this.profiles.AllEntries().GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				text = enumerator.Current.Name;
				this.profiles.SelectByName(text);
			}
		}
		ProfileSDF.SetSelectedProfile(text);
	}

	// Token: 0x06006F70 RID: 28528 RVA: 0x002B4010 File Offset: 0x002B2210
	public override void OnOpen()
	{
		base.OnOpen();
		this.selectFirstProfile();
	}

	// Token: 0x06006F71 RID: 28529 RVA: 0x002B401E File Offset: 0x002B221E
	public override void OnClose()
	{
		base.OnClose();
		if (this.onCloseAction == null)
		{
			return;
		}
		this.onCloseAction();
		this.onCloseAction = null;
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x002B4041 File Offset: 0x002B2241
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x17000CA0 RID: 3232
	// (get) Token: 0x06006F73 RID: 28531 RVA: 0x002B4054 File Offset: 0x002B2254
	[XuiXmlBinding("user_profile_count")]
	public int UserProfileCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.profiles == null)
			{
				return 0;
			}
			int num = 0;
			using (IEnumerator<XUiC_ProfilesList.ListEntry> enumerator = this.profiles.AllEntries().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (Archetype.GetArchetype(PlayerProfile.LoadProfile(enumerator.Current.Name).ProfileArchetype).CanCustomize)
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x17000CA1 RID: 3233
	// (get) Token: 0x06006F74 RID: 28532 RVA: 0x00020119 File Offset: 0x0001E319
	[XuiXmlBinding("can_create_userprofile")]
	public bool MaxUserProfilesReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000CA2 RID: 3234
	// (get) Token: 0x06006F75 RID: 28533 RVA: 0x002B40CC File Offset: 0x002B22CC
	[XuiXmlBinding("can_modify_profile")]
	public bool CanModifyProfile
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.profiles == null)
			{
				return false;
			}
			bool flag = !string.IsNullOrEmpty(ProfileSDF.CurrentProfileName());
			bool flag2 = this.profiles.SelectedEntryData != null;
			bool flag3 = flag && flag2;
			if (flag3)
			{
				PlayerProfile playerProfile = PlayerProfile.LoadLocalProfile();
				Archetype archetype;
				if ((archetype = Archetype.GetArchetype(playerProfile.ProfileArchetype)) == null)
				{
					archetype = Archetype.GetArchetype(playerProfile.IsMale ? "BaseMale" : "BaseFemale");
				}
				Archetype archetype2 = archetype;
				if (archetype2 != null)
				{
					flag3 &= archetype2.CanCustomize;
				}
			}
			return flag3;
		}
	}

	// Token: 0x06006F76 RID: 28534 RVA: 0x002B4141 File Offset: 0x002B2341
	public static void Open(XUi _xuiInstance, Action _onCloseAction = null)
	{
		_xuiInstance.GetChildByType<XUiC_PlayerProfile>().onCloseAction = _onCloseAction;
		_xuiInstance.playerUI.windowManager.Open(XUiC_PlayerProfile.ID, true);
	}

	// Token: 0x0400549A RID: 21658
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxUserProfiles = -1;

	// Token: 0x0400549B RID: 21659
	public static string ID = "";

	// Token: 0x0400549C RID: 21660
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ProfilesList profiles;

	// Token: 0x0400549D RID: 21661
	[XuiBindComponent("btnProfileCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileCreate;

	// Token: 0x0400549E RID: 21662
	[XuiBindComponent("btnProfileDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileDelete;

	// Token: 0x0400549F RID: 21663
	[XuiBindComponent("btnProfileEdit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileEdit;

	// Token: 0x040054A0 RID: 21664
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x040054A1 RID: 21665
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseAction;
}
