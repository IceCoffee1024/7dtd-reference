using System;
using System.Text;

// Token: 0x0200113E RID: 4414
public class XUiM_InGameService : XUiModel
{
	// Token: 0x06008B2C RID: 35628 RVA: 0x0035124C File Offset: 0x0034F44C
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

	// Token: 0x06008B2D RID: 35629 RVA: 0x001289D2 File Offset: 0x00126BD2
	[PublicizedFrom(EAccessModifier.Private)]
	public static string StringFormatHandler(string _title, object _value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", _title, _value);
	}

	// Token: 0x06008B2E RID: 35630 RVA: 0x003512E3 File Offset: 0x0034F4E3
	[PublicizedFrom(EAccessModifier.Private)]
	public static string StringFormatHandler(string _title, object _value, string _units)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1} {2}[-]\n", _title, _value, _units);
	}
}
