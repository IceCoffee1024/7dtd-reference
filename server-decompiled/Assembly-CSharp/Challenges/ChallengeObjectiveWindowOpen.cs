using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001915 RID: 6421
	[Preserve]
	public class ChallengeObjectiveWindowOpen : BaseChallengeObjective
	{
		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x0600C659 RID: 50777 RVA: 0x000ED7F0 File Offset: 0x000EB9F0
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.WindowOpen;
			}
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x0600C65A RID: 50778 RVA: 0x0048EB3B File Offset: 0x0048CD3B
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveOpenWindow_keyword", false, null), string.Format("[DECEA3]{0}[-]", Localization.Get("xui" + this.WindowName, false, null)));
			}
		}

		// Token: 0x0600C65B RID: 50779 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C65C RID: 50780 RVA: 0x0048EB6F File Offset: 0x0048CD6F
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
			QuestEventManager.Current.WindowChanged += this.Current_WindowChanged;
		}

		// Token: 0x0600C65D RID: 50781 RVA: 0x0048EB9D File Offset: 0x0048CD9D
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

		// Token: 0x0600C65E RID: 50782 RVA: 0x0048EBD6 File Offset: 0x0048CDD6
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
		}

		// Token: 0x0600C65F RID: 50783 RVA: 0x0048EBEE File Offset: 0x0048CDEE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			if (this.Owner != null)
			{
				base.Current = ((this.currentOpenWindow == this.WindowName) ? 1 : 0);
			}
		}

		// Token: 0x0600C660 RID: 50784 RVA: 0x0048EC1B File Offset: 0x0048CE1B
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveWindowOpen
			{
				WindowName = this.WindowName
			};
		}

		// Token: 0x0600C661 RID: 50785 RVA: 0x0048A383 File Offset: 0x00488583
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x0400958B RID: 38283
		public string WindowName = "";

		// Token: 0x0400958C RID: 38284
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentOpenWindow = "";

		// Token: 0x0400958D RID: 38285
		public RequirementObjectiveGroupWindowOpen Parent;
	}
}
