using System;

// Token: 0x020002AD RID: 685
public abstract class ConsoleCmdTestSystemAbs : ConsoleCmdAbstract
{
	// Token: 0x060013D2 RID: 5074 RVA: 0x00079044 File Offset: 0x00077244
	public static void AssertAreApproximatelyEqual(long expected, long actual, long tolerance, string message)
	{
		Math.Abs(expected - actual);
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00079050 File Offset: 0x00077250
	public static void AssertArraysAreEqual<T>(T[] expected, T[] actual, string message)
	{
		for (int i = 0; i < expected.Length; i++)
		{
		}
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x0007906C File Offset: 0x0007726C
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

	// Token: 0x060013D5 RID: 5077 RVA: 0x000790AC File Offset: 0x000772AC
	public static bool ExecutesWithoutExceptions(params Action[] testCases)
	{
		bool flag = true;
		foreach (Action action in testCases)
		{
			flag &= ConsoleCmdTestSystemAbs.ExecutesWithoutException(action);
		}
		return flag;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x000790DC File Offset: 0x000772DC
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

	// Token: 0x060013D7 RID: 5079 RVA: 0x0005FE37 File Offset: 0x0005E037
	[PublicizedFrom(EAccessModifier.Protected)]
	public ConsoleCmdTestSystemAbs()
	{
	}

	// Token: 0x04000D88 RID: 3464
	public const string SuccessString = "<color=green>Success</color>";

	// Token: 0x04000D89 RID: 3465
	public const string FailureString = "<color=red>Failure</color>";
}
