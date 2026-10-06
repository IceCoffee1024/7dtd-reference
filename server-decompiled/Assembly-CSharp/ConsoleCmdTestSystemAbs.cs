using System;

// Token: 0x020002B4 RID: 692
public abstract class ConsoleCmdTestSystemAbs : ConsoleCmdAbstract
{
	// Token: 0x060013E1 RID: 5089 RVA: 0x00079954 File Offset: 0x00077B54
	public static void AssertAreApproximatelyEqual(long expected, long actual, long tolerance, string message)
	{
		Math.Abs(expected - actual);
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x00079960 File Offset: 0x00077B60
	public static void AssertArraysAreEqual<T>(T[] expected, T[] actual, string message)
	{
		for (int i = 0; i < expected.Length; i++)
		{
		}
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x0007997C File Offset: 0x00077B7C
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

	// Token: 0x060013E4 RID: 5092 RVA: 0x000799BC File Offset: 0x00077BBC
	public static bool ExecutesWithoutExceptions(params Action[] testCases)
	{
		bool flag = true;
		foreach (Action action in testCases)
		{
			flag &= ConsoleCmdTestSystemAbs.ExecutesWithoutException(action);
		}
		return flag;
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x000799EC File Offset: 0x00077BEC
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

	// Token: 0x060013E6 RID: 5094 RVA: 0x0006036F File Offset: 0x0005E56F
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleCmdTestSystemAbs()
	{
	}

	// Token: 0x04000D92 RID: 3474
	public const string SuccessString = "<color=green>Success</color>";

	// Token: 0x04000D93 RID: 3475
	public const string FailureString = "<color=red>Failure</color>";
}
