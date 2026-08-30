using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E28 RID: 3624
[Preserve]
public class XUiC_PlayerProfile : XUiC_PlayGameDialogBase
{
	// Token: 0x06006F71 RID: 28529 RVA: 0x002B0AD5 File Offset: 0x002AECD5
	public override void Init()
	{
		base.Init();
		XUiC_PlayerProfile.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x002B0AED File Offset: 0x002AECED
	[XuiBindEvent("OnPress", "btnProfileEdit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnProfileEdit_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openCustomCharacterWindow();
	}

	// Token: 0x06006F73 RID: 28531 RVA: 0x002B0AF8 File Offset: 0x002AECF8
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

	// Token: 0x06006F74 RID: 28532 RVA: 0x002B0B72 File Offset: 0x002AED72
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

	// Token: 0x06006F75 RID: 28533 RVA: 0x002B0BA6 File Offset: 0x002AEDA6
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

	// Token: 0x06006F76 RID: 28534 RVA: 0x002B0BC2 File Offset: 0x002AEDC2
	[PublicizedFrom(EAccessModifier.Private)]
	public void openCustomCharacterWindow()
	{
		this.previewWindow.ViewComponent.IsVisible = false;
		XUiC_CustomCharacterWindowGroup.Open(this.xui, delegate
		{
			this.previewWindow.ViewComponent.IsVisible = true;
		});
	}

	// Token: 0x06006F77 RID: 28535 RVA: 0x002B0BEC File Offset: 0x002AEDEC
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

	// Token: 0x06006F78 RID: 28536 RVA: 0x002B0C60 File Offset: 0x002AEE60
	public override void OnOpen()
	{
		base.OnOpen();
		this.selectFirstProfile();
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x002B0C6E File Offset: 0x002AEE6E
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

	// Token: 0x06006F7A RID: 28538 RVA: 0x002B0C91 File Offset: 0x002AEE91
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x06006F7B RID: 28539 RVA: 0x002B0CA4 File Offset: 0x002AEEA4
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

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x06006F7C RID: 28540 RVA: 0x00020021 File Offset: 0x0001E221
	[XuiXmlBinding("can_create_userprofile")]
	public bool MaxUserProfilesReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x06006F7D RID: 28541 RVA: 0x002B0D1C File Offset: 0x002AEF1C
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

	// Token: 0x06006F7E RID: 28542 RVA: 0x002B0D91 File Offset: 0x002AEF91
	public static void Open(XUi _xuiInstance, Action _onCloseAction = null)
	{
		_xuiInstance.GetChildByType<XUiC_PlayerProfile>().onCloseAction = _onCloseAction;
		_xuiInstance.playerUI.windowManager.Open(XUiC_PlayerProfile.ID, true);
	}

	// Token: 0x04005463 RID: 21603
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxUserProfiles = -1;

	// Token: 0x04005464 RID: 21604
	public static string ID = "";

	// Token: 0x04005465 RID: 21605
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ProfilesList profiles;

	// Token: 0x04005466 RID: 21606
	[XuiBindComponent("btnProfileCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileCreate;

	// Token: 0x04005467 RID: 21607
	[XuiBindComponent("btnProfileDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileDelete;

	// Token: 0x04005468 RID: 21608
	[XuiBindComponent("btnProfileEdit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileEdit;

	// Token: 0x04005469 RID: 21609
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x0400546A RID: 21610
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseAction;
}
