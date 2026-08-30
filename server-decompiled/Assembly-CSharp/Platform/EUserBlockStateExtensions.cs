using System;

namespace Platform
{
	// Token: 0x02001BEC RID: 7148
	public static class EUserBlockStateExtensions
	{
		// Token: 0x0600D496 RID: 54422 RVA: 0x004CB0F8 File Offset: 0x004C92F8
		public static bool IsBlocked(this EUserBlockState blockState)
		{
			bool flag;
			if (blockState != EUserBlockState.NotBlocked)
			{
				if (blockState - EUserBlockState.InGame > 1)
				{
					throw new ArgumentOutOfRangeException("blockState", blockState, string.Format("{0} not implemented for {1}.{2}", "IsBlocked", "EUserBlockState", blockState));
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}
	}
}
