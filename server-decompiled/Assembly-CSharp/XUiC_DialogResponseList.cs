using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F33 RID: 3891
[Preserve]
public class XUiC_DialogResponseList : XUiController
{
	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x060079A1 RID: 31137 RVA: 0x002E0BAA File Offset: 0x002DEDAA
	// (set) Token: 0x060079A2 RID: 31138 RVA: 0x002E0BB2 File Offset: 0x002DEDB2
	public Dialog CurrentDialog
	{
		get
		{
			return this.currentDialog;
		}
		set
		{
			this.currentDialog = value;
			base.RefreshBindings();
			this.IsDirty = true;
		}
	}

	// Token: 0x060079A3 RID: 31139 RVA: 0x002E0BC8 File Offset: 0x002DEDC8
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
		this.lblResponderName = base.GetChildById("lblName").ViewComponent as XUiV_Label;
		XUiController childById = base.GetChildById("items");
		for (int i = 0; i < childById.Children.Count; i++)
		{
			XUiC_DialogResponseEntry xuiC_DialogResponseEntry = childById.Children[i] as XUiC_DialogResponseEntry;
			if (xuiC_DialogResponseEntry != null)
			{
				this.entryList.Add(xuiC_DialogResponseEntry);
				this.length++;
			}
		}
	}

	// Token: 0x060079A4 RID: 31140 RVA: 0x002E0C59 File Offset: 0x002DEE59
	public override void OnOpen()
	{
		base.OnOpen();
		this.lblResponderName.Text = Localization.Get(this.xui.Dialog.Respondent.EntityName, false, null);
	}

	// Token: 0x060079A5 RID: 31141 RVA: 0x002E0C88 File Offset: 0x002DEE88
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			List<BaseResponseEntry> list = new List<BaseResponseEntry>();
			this.uniqueResponseIDs.Clear();
			if (this.currentDialog != null)
			{
				list = this.currentDialog.CurrentStatement.GetResponses();
			}
			int num = 0;
			for (int i = 0; i < this.entryList.Count; i++)
			{
				XUiC_DialogResponseEntry xuiC_DialogResponseEntry = this.entryList[i];
				if (xuiC_DialogResponseEntry != null)
				{
					xuiC_DialogResponseEntry.OnPress -= this.OnPressResponse;
					if (num < list.Count)
					{
						xuiC_DialogResponseEntry.ViewComponent.SoundPlayOnClick = true;
						if (list[num].UniqueID == "" || !this.uniqueResponseIDs.Contains(list[num].UniqueID))
						{
							xuiC_DialogResponseEntry.CurrentResponse = list[num].Response;
							xuiC_DialogResponseEntry.OnPress += this.OnPressResponse;
						}
						else
						{
							xuiC_DialogResponseEntry.CurrentResponse = null;
						}
						if (xuiC_DialogResponseEntry.CurrentResponse == null)
						{
							i--;
						}
						else if (list[num].UniqueID != "")
						{
							this.uniqueResponseIDs.Add(list[num].UniqueID);
						}
						num++;
					}
					else
					{
						xuiC_DialogResponseEntry.ViewComponent.SoundPlayOnClick = false;
						xuiC_DialogResponseEntry.CurrentResponse = null;
					}
				}
			}
			if (list.Count > 0)
			{
				this.entryList[0].SelectCursorElement(true, false);
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x060079A6 RID: 31142 RVA: 0x002E0E04 File Offset: 0x002DF004
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressResponse(XUiController _sender, int _mouseButton)
	{
		if (((XUiC_DialogResponseEntry)_sender).HasRequirement)
		{
			DialogResponse currentResponse = ((XUiC_DialogResponseEntry)_sender).CurrentResponse;
			this.currentDialog.SelectResponse(currentResponse, this.xui.playerUI.entityPlayer);
			((XUiC_DialogWindowGroup)this.windowGroup.Controller).RefreshDialog();
		}
	}

	// Token: 0x060079A7 RID: 31143 RVA: 0x002E0E5B File Offset: 0x002DF05B
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005A2B RID: 23083
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog conversation;

	// Token: 0x04005A2C RID: 23084
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005A2D RID: 23085
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_DialogResponseEntry> entryList = new List<XUiC_DialogResponseEntry>();

	// Token: 0x04005A2E RID: 23086
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x04005A2F RID: 23087
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> uniqueResponseIDs = new List<string>();

	// Token: 0x04005A30 RID: 23088
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;

	// Token: 0x04005A31 RID: 23089
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblResponderName;
}
