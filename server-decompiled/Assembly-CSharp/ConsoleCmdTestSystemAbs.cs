using System;

// Token: 0x020002B1 RID: 689
public abstract class ConsoleCmdTestSystemAbs : ConsoleCmdAbstract
{
	// Token: 0x060013E4 RID: 5092 RVA: 0x000796A0 File Offset: 0x000778A0
	public static void AssertAreApproximatelyEqual(long expected, long actual, long tolerance, string message)
	{
		Math.Abs(expected - actual);
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x000796AC File Offset: 0x000778AC
	public static void AssertArraysAreEqual<T>(T[] expected, T[] actual, string message)
	{
		for (int i = 0; i < expected.Length; i++)
		{
		}
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x000796C8 File Offset: 0x000778C8
	public static bool AssertException(Action action)
	{
		bool flag;
		try
		{
			action();
			flag = false;
		}
		catch (Exception ex)
		{
			Log.Out(string.Format("Expected exception:\n{0}", ex));
			flag = true;
		}
		return flag;
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x00079708 File Offset: 0x00077908
	public static bool ExecutesWithoutExceptions(params Action[] testCases)
	{
		bool flag = true;
		foreach (Action action in testCases)
		{
			flag &= ConsoleCmdTestSystemAbs.ExecutesWithoutException(action);
		}
		return flag;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x00079738 File Offset: 0x00077938
	public static bool ExecutesWithoutException(Action testCase)
	{
		bool flag = true;
		try
		{
			Log.Out(testCase.Method.Name ?? "");
			testCase();
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
			flag = false;
		}
		finally
		{
			Log.Out(testCase.Method.Name + " result: " + (flag ? "<color=green>Success</color>" : "<color=red>Failure</color>") + ".");
		}
		return flag;
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x0006037F File Offset: 0x0005E57F
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleCmdTestSystemAbs()
	{
	}

	// Token: 0x04000D99 RID: 3481
	public const string SuccessString = "<color=green>Success</color>";

	// Token: 0x04000D9A RID: 3482
	public const string FailureString = "<color=red>Failure</color>";
}
