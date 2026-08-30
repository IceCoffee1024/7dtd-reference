using System;
using System.Text;

// Token: 0x0200110D RID: 4365
public class XUiM_InGameService : XUiModel
{
	// Token: 0x060089CE RID: 35278 RVA: 0x0034859C File Offset: 0x0034679C
	public static string GetServiceStats(XUi _xui, InGameService _service)
	{
		if (_service == null)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (_service.ServiceType == InGameService.InGameServiceTypes.VendingRent)
		{
			TraderInfo traderInfo = (_xui.Trader.Trader as TileEntityVendingMachine).TraderData.TraderInfo;
			stringBuilder.Append(XUiM_InGameService.StringFormatHandler(Localization.Get("xuiCost", false, null), traderInfo.RentCost));
			stringBuilder.Append(XUiM_InGameService.StringFormatHandler(Localization.Get("xuiGameTime", false, null), traderInfo.RentTimeInDays, Localization.Get("xuiGameDays", false, null)));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060089CF RID: 35279 RVA: 0x00126DC2 File Offset: 0x00124FC2
	[PublicizedFrom(EAccessModifier.Private)]
	public static string StringFormatHandler(string _title, object _value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", _title, _value);
	}

	// Token: 0x060089D0 RID: 35280 RVA: 0x00348633 File Offset: 0x00346833
	[PublicizedFrom(EAccessModifier.Private)]
	public static string StringFormatHandler(string _title, object _value, string _units)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1} {2}[-]\n", _title, _value, _units);
	}
}
