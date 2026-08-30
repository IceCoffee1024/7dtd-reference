using System;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018EF RID: 6383
	[Preserve]
	public class ChallengeObjectiveWindowOpen : BaseChallengeObjective
	{
		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x0600C4D2 RID: 50386 RVA: 0x000ECB9C File Offset: 0x000EAD9C
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.WindowOpen;
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x0600C4D3 RID: 50387 RVA: 0x004887EF File Offset: 0x004869EF
		public override string DescriptionText
		{
			get
			{
				return string.Format(Localization.Get("ObjectiveOpenWindow_keyword", false, null), string.Format("[DECEA3]{0}[-]", Localization.Get("xui" + this.WindowName, false, null)));
			}
		}

		// Token: 0x0600C4D4 RID: 50388 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C4D5 RID: 50389 RVA: 0x00488823 File Offset: 0x00486A23
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
			QuestEventManager.Current.WindowChanged += this.Current_WindowChanged;
		}

		// Token: 0x0600C4D6 RID: 50390 RVA: 0x00488851 File Offset: 0x00486A51
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

		// Token: 0x0600C4D7 RID: 50391 RVA: 0x0048888A File Offset: 0x00486A8A
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.WindowChanged -= this.Current_WindowChanged;
		}

		// Token: 0x0600C4D8 RID: 50392 RVA: 0x004888A2 File Offset: 0x00486AA2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleUpdatingCurrent()
		{
			base.HandleUpdatingCurrent();
			if (this.Owner != null)
			{
				base.Current = ((this.currentOpenWindow == this.WindowName) ? 1 : 0);
			}
		}

		// Token: 0x0600C4D9 RID: 50393 RVA: 0x004888CF File Offset: 0x00486ACF
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveWindowOpen
			{
				WindowName = this.WindowName
			};
		}

		// Token: 0x0600C4DA RID: 50394 RVA: 0x0048403F File Offset: 0x0048223F
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true);
			}
		}

		// Token: 0x04009462 RID: 37986
		public string WindowName = "";

		// Token: 0x04009463 RID: 37987
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentOpenWindow = "";

		// Token: 0x04009464 RID: 37988
		public RequirementObjectiveGroupWindowOpen Parent;
	}
}
