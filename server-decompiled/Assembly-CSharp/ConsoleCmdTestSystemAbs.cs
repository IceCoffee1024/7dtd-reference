using System;

// Token: 0x020002B6 RID: 694
public abstract class ConsoleCmdTestSystemAbs : ConsoleCmdAbstract
{
	// Token: 0x060013E5 RID: 5093 RVA: 0x00079B24 File Offset: 0x00077D24
	public static void AssertAreApproximatelyEqual(long expected, long actual, long tolerance, string message)
	{
		Math.Abs(expected - actual);
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x00079B30 File Offset: 0x00077D30
	public static void AssertArraysAreEqual<T>(T[] expected, T[] actual, string message)
	{
		for (int i = 0; i < expected.Length; i++)
		{
		}
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x00079B4C File Offset: 0x00077D4C
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

	// Token: 0x060013E8 RID: 5096 RVA: 0x00079B8C File Offset: 0x00077D8C
	public static bool ExecutesWithoutExceptions(params Action[] testCases)
	{
		bool flag = true;
		foreach (Action action in testCases)
		{
			flag &= ConsoleCmdTestSystemAbs.ExecutesWithoutException(action);
		}
		return flag;
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x00079BBC File Offset: 0x00077DBC
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

	// Token: 0x060013EA RID: 5098 RVA: 0x00060633 File Offset: 0x0005E833
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleCmdTestSystemAbs()
	{
	}

	// Token: 0x04000D93 RID: 3475
	public const string SuccessString = "<color=green>Success</color>";

	// Token: 0x04000D94 RID: 3476
	public const string FailureString = "<color=red>Failure</color>";
}
