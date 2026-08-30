using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F24 RID: 3876
[Preserve]
public class XUiC_DialogResponseList : XUiController
{
	// Token: 0x17000E37 RID: 3639
	// (get) Token: 0x060079A6 RID: 31142 RVA: 0x002DD80E File Offset: 0x002DBA0E
	// (set) Token: 0x060079A7 RID: 31143 RVA: 0x002DD816 File Offset: 0x002DBA16
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

	// Token: 0x060079A8 RID: 31144 RVA: 0x002DD82C File Offset: 0x002DBA2C
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

	// Token: 0x060079A9 RID: 31145 RVA: 0x002DD8BD File Offset: 0x002DBABD
	public override void OnOpen()
	{
		base.OnOpen();
		this.lblResponderName.Text = Localization.Get(this.xui.Dialog.Respondent.EntityName, false, null);
	}

	// Token: 0x060079AA RID: 31146 RVA: 0x002DD8EC File Offset: 0x002DBAEC
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

	// Token: 0x060079AB RID: 31147 RVA: 0x002DDA68 File Offset: 0x002DBC68
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

	// Token: 0x060079AC RID: 31148 RVA: 0x002DDABF File Offset: 0x002DBCBF
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x040059F3 RID: 23027
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog conversation;

	// Token: 0x040059F4 RID: 23028
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x040059F5 RID: 23029
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_DialogResponseEntry> entryList = new List<XUiC_DialogResponseEntry>();

	// Token: 0x040059F6 RID: 23030
	[PublicizedFrom(EAccessModifier.Private)]
	public int length;

	// Token: 0x040059F7 RID: 23031
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> uniqueResponseIDs = new List<string>();

	// Token: 0x040059F8 RID: 23032
	[PublicizedFrom(EAccessModifier.Private)]
	public Dialog currentDialog;

	// Token: 0x040059F9 RID: 23033
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblResponderName;
}
