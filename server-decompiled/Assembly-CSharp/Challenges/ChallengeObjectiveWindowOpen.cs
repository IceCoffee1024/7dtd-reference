using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001941 RID: 6465
	[Preserve]
	public class ChallengeObjectiveWindowOpen : BaseChallengeObjective
	{
		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x0600C7B1 RID: 51121 RVA: 0x000ED904 File Offset: 0x000EBB04
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.WindowOpen;
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x0600C7B2 RID: 51122 RVA: 0x00493027 File Offset: 0x00491227
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveOpenWindow_keyword", false, null), string.Format("[DECEA3]{0}[-]", Localization.Get("xui" + this.WindowName, false, null)));
			}
		}

		// Token: 0x0600C7B3 RID: 51123 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C7B4 RID: 51124 RVA: 0x0049305B File Offset: 0x0049125B
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
			QuestEventManager.Current.WindowChanged += this.Current_WindowChanged;
		}

		// Token: 0x0600C7B5 RID: 51125 RVA: 0x00493089 File Offset: 0x00491289
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_WindowChanged(string _windowName)
		{
			if (_windowName == "windowpaging")
			{
				return;
			}
			if (this.CheckBaseRequirements())
			{
				return;
			}
			this.currentOpenWindow = _windowName;
			this.HandleUpdatingCurrent();
			this.CheckObjectiveComplete(true);
			this.Parent.CheckPrerequisites();
		}

		// Token: 0x0600C7B6 RID: 51126 RVA: 0x004930C2 File Offset: 0x004912C2
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
		}

		// Token: 0x0600C7B7 RID: 51127 RVA: 0x004930DA File Offset: 0x004912DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			if (this.Owner != null)
			{
				base.Current = ((this.currentOpenWindow == this.WindowName) ? 1 : 0);
			}
		}

		// Token: 0x0600C7B8 RID: 51128 RVA: 0x00493107 File Offset: 0x00491307
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveWindowOpen
			{
				WindowName = this.WindowName
			};
		}

		// Token: 0x0600C7B9 RID: 51129 RVA: 0x0048E873 File Offset: 0x0048CA73
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x04009651 RID: 38481
		public string WindowName = "";

		// Token: 0x04009652 RID: 38482
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentOpenWindow = "";

		// Token: 0x04009653 RID: 38483
		public RequirementObjectiveGroupWindowOpen Parent;
	}
}
