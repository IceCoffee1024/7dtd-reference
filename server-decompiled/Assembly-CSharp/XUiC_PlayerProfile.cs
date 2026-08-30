using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000E13 RID: 3603
[Preserve]
public class XUiC_PlayerProfile : XUiC_PlayGameDialogBase
{
	// Token: 0x06006ED1 RID: 28369 RVA: 0x002AD5C1 File Offset: 0x002AB7C1
	public override void Init()
	{
		base.Init();
		XUiC_PlayerProfile.ID = base.WindowGroup.Id;
	}

	// Token: 0x06006ED2 RID: 28370 RVA: 0x002AD5D9 File Offset: 0x002AB7D9
	[XuiBindEvent("OnPress", "btnProfileEdit")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnProfileEdit_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.openCustomCharacterWindow();
	}

	// Token: 0x06006ED3 RID: 28371 RVA: 0x002AD5E4 File Offset: 0x002AB7E4
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

	// Token: 0x06006ED4 RID: 28372 RVA: 0x002AD65E File Offset: 0x002AB85E
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

	// Token: 0x06006ED5 RID: 28373 RVA: 0x002AD692 File Offset: 0x002AB892
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

	// Token: 0x06006ED6 RID: 28374 RVA: 0x002AD6AE File Offset: 0x002AB8AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void openCustomCharacterWindow()
	{
		this.previewWindow.ViewComponent.IsVisible = false;
		XUiC_CustomCharacterWindowGroup.Open(this.xui, delegate
		{
			this.previewWindow.ViewComponent.IsVisible = true;
		});
	}

	// Token: 0x06006ED7 RID: 28375 RVA: 0x002AD6D8 File Offset: 0x002AB8D8
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

	// Token: 0x06006ED8 RID: 28376 RVA: 0x002AD74C File Offset: 0x002AB94C
	public override void OnOpen()
	{
		base.OnOpen();
		this.selectFirstProfile();
	}

	// Token: 0x06006ED9 RID: 28377 RVA: 0x002AD75A File Offset: 0x002AB95A
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

	// Token: 0x06006EDA RID: 28378 RVA: 0x002AD77D File Offset: 0x002AB97D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x06006EDB RID: 28379 RVA: 0x002AD790 File Offset: 0x002AB990
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

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x06006EDC RID: 28380 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[XuiXmlBinding("can_create_userprofile")]
	public bool MaxUserProfilesReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06006EDD RID: 28381 RVA: 0x002AD808 File Offset: 0x002ABA08
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

	// Token: 0x06006EDE RID: 28382 RVA: 0x002AD87D File Offset: 0x002ABA7D
	public static void Open(XUi _xuiInstance, Action _onCloseAction = null)
	{
		_xuiInstance.GetChildByType<XUiC_PlayerProfile>().onCloseAction = _onCloseAction;
		_xuiInstance.playerUI.windowManager.Open(XUiC_PlayerProfile.ID, true);
	}

	// Token: 0x040053BF RID: 21439
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxUserProfiles = -1;

	// Token: 0x040053C0 RID: 21440
	public static string ID = "";

	// Token: 0x040053C1 RID: 21441
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ProfilesList profiles;

	// Token: 0x040053C2 RID: 21442
	[XuiBindComponent("btnProfileCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileCreate;

	// Token: 0x040053C3 RID: 21443
	[XuiBindComponent("btnProfileDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileDelete;

	// Token: 0x040053C4 RID: 21444
	[XuiBindComponent("btnProfileEdit", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnProfileEdit;

	// Token: 0x040053C5 RID: 21445
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SDCSPreviewWindow previewWindow;

	// Token: 0x040053C6 RID: 21446
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseAction;
}
