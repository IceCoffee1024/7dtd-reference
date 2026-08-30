using System;

// Token: 0x02000062 RID: 98
public static class Submission
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060001E1 RID: 481 RVA: 0x00010E10 File Offset: 0x0000F010
	public static bool Enabled
	{
		get
		{
			if (!Submission.isSubmissionChecked)
			{
				string[] commandLineArgs = GameStartupHelper.GetCommandLineArgs();
				for (int i = 0; i < commandLineArgs.Length; i++)
				{
					if (commandLineArgs[i].EqualsCaseInsensitive(Constants.cArgSubmissionBuild))
					{
						Log.Out("Submission Enabled by argument");
						Submission.isSubmission = true;
					}
				}
				Submission.isSubmissionChecked = true;
			}
			return Submission.isSubmission;
		}
	}

	// Token: 0x040002A6 RID: 678
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isSubmissionChecked;

	// Token: 0x040002A7 RID: 679
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isSubmission;
}
