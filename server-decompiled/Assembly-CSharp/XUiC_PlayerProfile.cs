using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E42 RID: 3650
[Preserve]
public class XUiC_PlayerProfile : XUiC_PlayGameDialogBase
{
	// Token: 0x06006F8E RID: 28558 RVA: 0x002B53F1 File Offset: 0x002B35F1
	public override void Init()
	{
		base.Init();
		XUiC_PlayerProfile.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006F8F RID: 28559 RVA: 0x002B5409 File Offset: 0x002B3609
	[XuiBindEvent("OnPress", "btnProfileEdit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnProfileEdit_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openCustomCharacterWindow();
	}

	// Token: 0x06006F90 RID: 28560 RVA: 0x002B5414 File Offset: 0x002B3614
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

	// Token: 0x06006F91 RID: 28561 RVA: 0x002B548E File Offset: 0x002B368E
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

	// Token: 0x06006F92 RID: 28562 RVA: 0x002B54C2 File Offset: 0x002B36C2
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

	// Token: 0x06006F93 RID: 28563 RVA: 0x002B54DE File Offset: 0x002B36DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void openCustomCharacterWindow()
	{
		this.previewWindow.ViewComponent.IsVisible = false;
		XUiC_CustomCharacterWindowGroup.Open(this.xui, delegate
		{
			this.previewWindow.ViewComponent.IsVisible = true;
		});
	}

	// Token: 0x06006F94 RID: 28564 RVA: 0x002B5508 File Offset: 0x002B3708
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

	// Token: 0x06006F95 RID: 28565 RVA: 0x002B557C File Offset: 0x002B377C
	public override void OnOpen()
	{
		base.OnOpen();
		this.selectFirstProfile();
	}

	// Token: 0x06006F96 RID: 28566 RVA: 0x002B558A File Offset: 0x002B378A
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

	// Token: 0x06006F97 RID: 28567 RVA: 0x002B55AD File Offset: 0x002B37AD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06006F98 RID: 28568 RVA: 0x002B55C0 File Offset: 0x002B37C0
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

	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06006F99 RID: 28569 RVA: 0x00020119 File Offset: 0x0001E319
	[XuiXmlBinding("can_create_userprofile")]
	public bool MaxUserProfilesReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06006F9A RID: 28570 RVA: 0x002B5638 File Offset: 0x002B3838
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

	// Token: 0x06006F9B RID: 28571 RVA: 0x002B56AD File Offset: 0x002B38AD
	public static void Open(XUi _xuiInstance, Action _onCloseAction = null)
	{
		_xuiInstance.GetChildByType<XUiC_PlayerProfile>().onCloseAction = _onCloseAction;
		_xuiInstance.playerUI.windowManager.Open(XUiC_PlayerProfile.ID, true);
	}

	// Token: 0x040054BF RID: 21695
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxUserProfiles = -1;

	// Token: 0x040054C0 RID: 21696
	public static string ID = "";

	// Token: 0x040054C1 RID: 21697
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ProfilesList profiles;

	// Token: 0x040054C2 RID: 21698
	[XuiBindComponent("btnProfileCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileCreate;

	// Token: 0x040054C3 RID: 21699
	[XuiBindComponent("btnProfileDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileDelete;

	// Token: 0x040054C4 RID: 21700
	[XuiBindComponent("btnProfileEdit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileEdit;

	// Token: 0x040054C5 RID: 21701
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x040054C6 RID: 21702
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseAction;
}
