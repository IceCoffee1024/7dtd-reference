using System;
using System.Threading;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x020002D5 RID: 725
public class WinFormInstance : IConsoleServer
{
	// Token: 0x060014D0 RID: 5328 RVA: 0x0007E1FC File Offset: 0x0007C3FC
	public WinFormInstance()
	{
		try
		{
			this.windowThread = new Thread(new ThreadStart(this.windowThreadMain))
			{
				Name = "WinFormInstance"
			};
			this.windowThread.SetApartmentState(ApartmentState.STA);
			this.windowThread.Start();
			Thread.Sleep(250);
			Log.Out("Started Terminal Window");
		}
		catch (Exception ex)
		{
			string text = "Error in WinFormInstance.ctor: ";
			Exception ex2 = ex;
			Log.Out(text + ((ex2 != null) ? ex2.ToString() : null));
		}
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x0007E290 File Offset: 0x0007C490
	[PublicizedFrom(EAccessModifier.Private)]
	public void windowThreadMain()
	{
		this.form = new WinFormConnection(this);
		Log.Out("WinThread started");
		global::System.Windows.Forms.Application.ThreadException += this.ApplicationOnThreadException;
		global::System.Windows.Forms.Application.Run(this.form);
		Profiler.EndThreadProfiling();
		this.form = null;
		Log.Out("WinThread ended");
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x0007E2E5 File Offset: 0x0007C4E5
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplicationOnThreadException(object _sender, ThreadExceptionEventArgs _threadExceptionEventArgs)
	{
		Log.Error("TerminalWindow Exeption:");
		Log.Exception(_threadExceptionEventArgs.Exception);
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x0007E2FC File Offset: 0x0007C4FC
	public void Disconnect()
	{
		if (this.form == null)
		{
			return;
		}
		Log.Out("Closing Terminal Window");
		WinFormConnection winFormConnection = this.form;
		this.form = null;
		winFormConnection.CloseTerminal();
		this.windowThread.Join();
		Log.Out("Ended Terminal Window");
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x0007E338 File Offset: 0x0007C538
	public void SendLine(string _line)
	{
		if (_line != null && this.form != null)
		{
			this.form.SendLine(_line);
		}
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x0007E351 File Offset: 0x0007C551
	public void SendLog(string _formattedMessage, string _plainMessage, string _trace, LogType _type, DateTime _timestamp, long _uptime)
	{
		if (_formattedMessage != null && this.form != null)
		{
			this.form.SendLog(_formattedMessage, _plainMessage, _trace, _type, _timestamp, _uptime);
		}
	}

	// Token: 0x04000DCE RID: 3534
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Thread windowThread;

	// Token: 0x04000DCF RID: 3535
	[PublicizedFrom(EAccessModifier.Private)]
	public bool stopThread;

	// Token: 0x04000DD0 RID: 3536
	[PublicizedFrom(EAccessModifier.Private)]
	public WinFormConnection form;
}
