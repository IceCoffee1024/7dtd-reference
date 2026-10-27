using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001972 RID: 6514
	[Preserve]
	public class ChallengeObjectiveWindowOpen : BaseChallengeObjective
	{
		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x0600C8C5 RID: 51397 RVA: 0x000EDD14 File Offset: 0x000EBF14
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.WindowOpen;
			}
		}

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x0600C8C6 RID: 51398 RVA: 0x00496A1B File Offset: 0x00494C1B
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveOpenWindow_keyword", false, null), string.Format("[DECEA3]{0}[-]", Localization.Get("xui" + this.WindowName, false, null)));
			}
		}

		// Token: 0x0600C8C7 RID: 51399 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C8C8 RID: 51400 RVA: 0x00496A4F File Offset: 0x00494C4F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
			QuestEventManager.Current.WindowChanged += this.Current_WindowChanged;
		}

		// Token: 0x0600C8C9 RID: 51401 RVA: 0x00496A7D File Offset: 0x00494C7D
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

		// Token: 0x0600C8CA RID: 51402 RVA: 0x00496AB6 File Offset: 0x00494CB6
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
		}

		// Token: 0x0600C8CB RID: 51403 RVA: 0x00496ACE File Offset: 0x00494CCE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			if (this.Owner != null)
			{
				base.Current = ((this.currentOpenWindow == this.WindowName) ? 1 : 0);
			}
		}

		// Token: 0x0600C8CC RID: 51404 RVA: 0x00496AFB File Offset: 0x00494CFB
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveWindowOpen
			{
				WindowName = this.WindowName
			};
		}

		// Token: 0x0600C8CD RID: 51405 RVA: 0x00492267 File Offset: 0x00490467
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x0400973A RID: 38714
		public string WindowName = "";

		// Token: 0x0400973B RID: 38715
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentOpenWindow = "";

		// Token: 0x0400973C RID: 38716
		public RequirementObjectiveGroupWindowOpen Parent;
	}
}
