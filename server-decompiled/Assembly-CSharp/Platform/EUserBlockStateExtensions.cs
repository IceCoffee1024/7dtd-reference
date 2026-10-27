using System;

namespace Platform
{
	// Token: 0x02001C4F RID: 7247
	public static class EUserBlockStateExtensions
	{
		// Token: 0x0600D722 RID: 55074 RVA: 0x004D3708 File Offset: 0x004D1908
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
