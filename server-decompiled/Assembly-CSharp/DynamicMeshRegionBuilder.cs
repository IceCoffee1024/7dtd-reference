using System;
using System.Threading;

// Token: 0x0200039E RID: 926
public class DynamicMeshRegionBuilder
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x06001B7F RID: 7039 RVA: 0x000A34C7 File Offset: 0x000A16C7
	public static World world
	{
		get
		{
			return GameManager.Instance.World;
		}
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x000A34D3 File Offset: 0x000A16D3
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

	// Token: 0x06001B81 RID: 7041 RVA: 0x000A3510 File Offset: 0x000A1710
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

	// Token: 0x06001B82 RID: 7042 RVA: 0x000A3554 File Offset: 0x000A1754
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

	// Token: 0x040011BA RID: 4538
	public string Error;

	// Token: 0x040011BB RID: 4539
	public bool StopRequested;

	// Token: 0x040011BC RID: 4540
	public DynamicMeshBuilderStatus Status;

	// Token: 0x040011BD RID: 4541
	public ExportMeshResult Result = ExportMeshResult.Missing;

	// Token: 0x040011BE RID: 4542
	public DynamicMeshRegion Region;

	// Token: 0x040011BF RID: 4543
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread thread;
}
