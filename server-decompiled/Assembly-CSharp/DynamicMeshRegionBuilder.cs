using System;
using System.Threading;

// Token: 0x02000395 RID: 917
public class DynamicMeshRegionBuilder
{
	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06001B65 RID: 7013 RVA: 0x000A2937 File Offset: 0x000A0B37
	public static World world
	{
		get
		{
			return GameManager.Instance.World;
		}
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x000A2943 File Offset: 0x000A0B43
	public bool AddNewItem(DynamicMeshRegion region)
	{
		if (this.Status != DynamicMeshBuilderStatus.Ready)
		{
			Log.Warning("Builder thread tried to start when not ready. Current Status: " + this.Status.ToString());
			return false;
		}
		this.Region = region;
		this.Status = DynamicMeshBuilderStatus.StartingExport;
		return true;
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000A2980 File Offset: 0x000A0B80
	public void RequestStop(bool forceStop = false)
	{
		this.StopRequested = true;
		if (forceStop)
		{
			try
			{
				Thread thread = this.thread;
				if (thread != null)
				{
					thread.Abort();
				}
			}
			catch (Exception)
			{
			}
			this.Status = DynamicMeshBuilderStatus.Stopped;
		}
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x000A29C4 File Offset: 0x000A0BC4
	public void StartThread()
	{
		this.thread = new Thread(delegate
		{
			try
			{
				while (!this.StopRequested)
				{
					if (GameManager.Instance == null)
					{
						return;
					}
					if (GameManager.Instance.World == null)
					{
						return;
					}
					if (this.Status == DynamicMeshBuilderStatus.Ready || this.Status == DynamicMeshBuilderStatus.Complete)
					{
						Thread.Sleep(100);
					}
					else
					{
						if (this.Status != DynamicMeshBuilderStatus.StartingExport)
						{
							Log.Error("Builder thread and wrong state: " + this.Status.ToString());
							this.Status = DynamicMeshBuilderStatus.Error;
							return;
						}
						throw new NotImplementedException("No build method");
					}
				}
			}
			catch (Exception ex)
			{
				this.Error = "Builder error: " + ex.Message;
				Log.Error(this.Error);
			}
			this.Status = DynamicMeshBuilderStatus.Stopped;
		});
		this.thread.Start();
	}

	// Token: 0x0400119C RID: 4508
	public string Error;

	// Token: 0x0400119D RID: 4509
	public bool StopRequested;

	// Token: 0x0400119E RID: 4510
	public DynamicMeshBuilderStatus Status;

	// Token: 0x0400119F RID: 4511
	public ExportMeshResult Result = ExportMeshResult.Missing;

	// Token: 0x040011A0 RID: 4512
	public DynamicMeshRegion Region;

	// Token: 0x040011A1 RID: 4513
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread thread;
}
