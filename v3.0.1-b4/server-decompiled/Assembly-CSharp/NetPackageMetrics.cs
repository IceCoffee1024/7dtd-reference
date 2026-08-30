using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000790 RID: 1936
[Preserve]
public class NetPackageMetrics
{
	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x0600398C RID: 14732 RVA: 0x001779F8 File Offset: 0x00175BF8
	public static NetPackageMetrics Instance
	{
		get
		{
			return NetPackageMetrics._instance;
		}
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x00177A00 File Offset: 0x00175C00
	public NetPackageMetrics()
	{
		NetPackageMetrics._instance = this;
		this.lastUpdateTime = Time.time - this.updateLength;
		this.receivedNetPackageCounts = new Dictionary<string, int>();
		this.receivedNetPackageSizes = new Dictionary<string, List<int>>();
		this.sentNetPackageCounts = new Dictionary<string, int>();
		this.sentNetPackageSizes = new Dictionary<string, List<int>>();
		this.packagesSent = new Dictionary<string, List<PackagesSentInfoEntry>>();
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x00177A8C File Offset: 0x00175C8C
	public void ResetStats()
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.ResetNetworkStatistics();
		this.receivedNetPackageCounts.Clear();
		this.sentNetPackageCounts.Clear();
		this.sentNetPackageSizes = new Dictionary<string, List<int>>();
		this.receivedNetPackageSizes = new Dictionary<string, List<int>>();
		this.packagesSent = new Dictionary<string, List<PackagesSentInfoEntry>>();
		NetPackageMetrics.tick = 0;
		Log.Out("Network stats reset");
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x00177AEA File Offset: 0x00175CEA
	public void SetUpdateLength(float length)
	{
		Log.Out("Setting network stat length to " + length.ToString() + " seconds");
		this.updateLength = length;
		this.lastUpdateTime = Time.time;
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x00177B19 File Offset: 0x00175D19
	public void RestartTimer()
	{
		this.lastUpdateTime = Time.time;
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x00177B26 File Offset: 0x00175D26
	public void RecordForPeriod(float length)
	{
		this.active = true;
		this.ResetStats();
		this.SetUpdateLength(length);
		this.RestartTimer();
		SingletonMonoBehaviour<ConnectionManager>.Instance.EnableNetworkStatistics();
		this.entityAliveCount = global::UnityEngine.Object.FindObjectsOfType<EntityAlive>().Length;
		this.playerCount = global::UnityEngine.Object.FindObjectsOfType<EntityPlayer>().Length;
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x00177B66 File Offset: 0x00175D66
	public void CopyToClipboard()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.lastStatsOutput;
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x00177B84 File Offset: 0x00175D84
	public void CopyToCSV(bool includeDetails = false)
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.ProduceCSV(includeDetails);
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x00177BA4 File Offset: 0x00175DA4
	public void RegisterReceivedPackage(string packageType, int length)
	{
		if (!this.active)
		{
			return;
		}
		if (this.receivedNetPackageCounts.ContainsKey(packageType))
		{
			Dictionary<string, int> dictionary = this.receivedNetPackageCounts;
			int num = dictionary[packageType];
			dictionary[packageType] = num + 1;
		}
		else
		{
			this.receivedNetPackageCounts[packageType] = 1;
			this.receivedNetPackageSizes[packageType] = new List<int>();
		}
		this.receivedNetPackageSizes[packageType].Add(length);
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x00177C14 File Offset: 0x00175E14
	public void RegisterSentPackage(string packageType, int length)
	{
		if (!this.active)
		{
			return;
		}
		if (this.sentNetPackageCounts.ContainsKey(packageType))
		{
			Dictionary<string, int> dictionary = this.sentNetPackageCounts;
			int num = dictionary[packageType];
			dictionary[packageType] = num + 1;
		}
		else
		{
			this.sentNetPackageCounts[packageType] = 1;
			this.sentNetPackageSizes[packageType] = new List<int>();
		}
		this.sentNetPackageSizes[packageType].Add(length);
	}

	// Token: 0x06003996 RID: 14742 RVA: 0x00177C84 File Offset: 0x00175E84
	public void RegisterPackagesSent(List<NetPackageInfo> packages, int count, long uncompressedSize, long compressedSize, float timeStamp, string client)
	{
		if (!this.active)
		{
			return;
		}
		PackagesSentInfoEntry packagesSentInfoEntry = new PackagesSentInfoEntry
		{
			packages = new List<NetPackageInfo>(packages),
			count = count,
			bCompressed = (compressedSize != -1L),
			uncompressedSize = uncompressedSize,
			compressedSize = ((compressedSize == -1L) ? uncompressedSize : compressedSize),
			timestamp = timeStamp,
			client = client
		};
		if (!this.packagesSent.ContainsKey(client))
		{
			this.packagesSent[client] = new List<PackagesSentInfoEntry>();
		}
		this.packagesSent[client].Add(packagesSentInfoEntry);
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x00177D20 File Offset: 0x00175F20
	public string ProduceCSV(bool includeDetails = false)
	{
		string text = "";
		int num = 0;
		int num2 = 0;
		text += "NET METRICS";
		text = text + "\nPrioritization Enabled: " + GameManager.enableNetworkdPrioritization.ToString();
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer)
		{
			text = text + "\nPlayer name: " + primaryPlayer.EntityName;
		}
		text += "\n\nSent NetPackages:";
		text += "\npackage type,count,total length, average length";
		foreach (KeyValuePair<string, int> keyValuePair in this.sentNetPackageCounts)
		{
			num2 += keyValuePair.Value;
			float num3 = 0f;
			foreach (int num4 in this.sentNetPackageSizes[keyValuePair.Key])
			{
				num3 += (float)num4;
			}
			float num5 = num3 / (float)keyValuePair.Value;
			text = string.Concat(new string[]
			{
				text,
				"\n",
				keyValuePair.Key,
				",",
				keyValuePair.Value.ToString(),
				",",
				num3.ToString(),
				",",
				num5.ToString()
			});
		}
		text += "\n\nReceived NetPackages:";
		text += "\npackage type,count,total length, average length";
		foreach (KeyValuePair<string, int> keyValuePair2 in this.receivedNetPackageCounts)
		{
			num += keyValuePair2.Value;
			float num3 = 0f;
			foreach (int num6 in this.receivedNetPackageSizes[keyValuePair2.Key])
			{
				num3 += (float)num6;
			}
			float num5 = num3 / (float)keyValuePair2.Value;
			text = string.Concat(new string[]
			{
				text,
				"\n",
				keyValuePair2.Key,
				",",
				keyValuePair2.Value.ToString(),
				",",
				num3.ToString(),
				",",
				num5.ToString()
			});
		}
		text += "\n\n\nTotals";
		text = text + "\nPackages Sent: " + num2.ToString();
		text = text + "\nPackages Received: " + num.ToString();
		text = text + "\n\nPlayers: " + this.playerCount.ToString();
		text = text + "\n\nEntityAlive Count: " + this.entityAliveCount.ToString();
		foreach (KeyValuePair<string, List<PackagesSentInfoEntry>> keyValuePair3 in this.packagesSent)
		{
			new List<PackagesSentInfoEntry>();
			text = text + "\n Packages sent for client " + keyValuePair3.Key + "\n\n";
			text += "\nmilliseconds,count,uncompressed size, compressed size";
			foreach (PackagesSentInfoEntry packagesSentInfoEntry in keyValuePair3.Value)
			{
				if (this.includeRelPosRot)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						packagesSentInfoEntry.timestamp.ToString(),
						",",
						packagesSentInfoEntry.count.ToString(),
						",",
						packagesSentInfoEntry.uncompressedSize.ToString(),
						",",
						packagesSentInfoEntry.compressedSize.ToString()
					});
				}
				else
				{
					int num7 = 0;
					int num8 = 0;
					foreach (NetPackageInfo netPackageInfo in packagesSentInfoEntry.packages)
					{
						if (!(netPackageInfo.netPackageType == "NetPackageEntityRelPosAndRot"))
						{
							num7++;
							num8 += netPackageInfo.length;
						}
					}
					text = string.Concat(new string[]
					{
						text,
						"\n",
						packagesSentInfoEntry.timestamp.ToString(),
						",",
						num7.ToString(),
						",",
						num8.ToString()
					});
				}
				if (includeDetails)
				{
					text += "\n,package type,package length";
					foreach (NetPackageInfo netPackageInfo2 in packagesSentInfoEntry.packages)
					{
						if (this.includeRelPosRot || !(netPackageInfo2.netPackageType == "NetPackageEntityRelPosAndRot"))
						{
							text = string.Concat(new string[]
							{
								text,
								"\n,",
								netPackageInfo2.netPackageType,
								",",
								netPackageInfo2.length.ToString()
							});
						}
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x00178320 File Offset: 0x00176520
	public void Update()
	{
		if (!this.active)
		{
			return;
		}
		if (Time.time - this.lastUpdateTime >= this.updateLength)
		{
			this.lastStatsOutput = SingletonMonoBehaviour<ConnectionManager>.Instance.PrintNetworkStatistics();
			int num = 0;
			int num2 = 0;
			this.lastStatsOutput += "NET METRICS";
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer)
			{
				this.lastStatsOutput = this.lastStatsOutput + "\nPlayer name: " + primaryPlayer.EntityName;
			}
			this.lastStatsOutput += "\n\nSent NetPackages:";
			foreach (KeyValuePair<string, int> keyValuePair in this.sentNetPackageCounts)
			{
				num2 += keyValuePair.Value;
				float num3 = 0f;
				foreach (int num4 in this.sentNetPackageSizes[keyValuePair.Key])
				{
					num3 += (float)num4;
				}
				float num5 = num3 / (float)keyValuePair.Value;
				this.lastStatsOutput = string.Concat(new string[]
				{
					this.lastStatsOutput,
					"\n",
					keyValuePair.Key,
					": Count:",
					keyValuePair.Value.ToString(),
					" Total Size: ",
					num3.ToString(),
					" Avg Size: ",
					num5.ToString()
				});
			}
			this.lastStatsOutput += "\n\nReceived NetPackages:";
			foreach (KeyValuePair<string, int> keyValuePair2 in this.receivedNetPackageCounts)
			{
				num += keyValuePair2.Value;
				float num3 = 0f;
				foreach (int num6 in this.receivedNetPackageSizes[keyValuePair2.Key])
				{
					num3 += (float)num6;
				}
				float num5 = num3 / (float)keyValuePair2.Value;
				this.lastStatsOutput = string.Concat(new string[]
				{
					this.lastStatsOutput,
					"\n",
					keyValuePair2.Key,
					": Count:",
					keyValuePair2.Value.ToString(),
					" Total Size: ",
					num3.ToString(),
					" Avg Size: ",
					num5.ToString()
				});
			}
			this.lastStatsOutput += "\n\n\nTotals";
			this.lastStatsOutput = this.lastStatsOutput + "\nPackages Sent: " + num2.ToString();
			this.lastStatsOutput = this.lastStatsOutput + "\nPackages Received: " + num.ToString();
			this.lastStatsOutput = this.lastStatsOutput + "\n\nPlayers: " + this.playerCount.ToString();
			this.lastStatsOutput = this.lastStatsOutput + "\n\nEntityAlive Count: " + this.entityAliveCount.ToString();
			Log.Out(this.lastStatsOutput);
			string text = this.ProduceCSV(false);
			this.lastUpdateTime = Time.time;
			this.active = false;
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				NetPackageNetMetrics netPackageNetMetrics = NetPackageNetMetrics.SetupClient(this.lastStatsOutput, text);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageNetMetrics, false);
			}
		}
	}

	// Token: 0x06003999 RID: 14745 RVA: 0x001786F0 File Offset: 0x001768F0
	public void OutputPackageSentDetails(string client, float timestamp)
	{
		List<PackagesSentInfoEntry> list;
		if (this.packagesSent.TryGetValue(client, out list))
		{
			PackagesSentInfoEntry packagesSentInfoEntry = list.Find((PackagesSentInfoEntry x) => x.timestamp == timestamp);
			if (packagesSentInfoEntry != null)
			{
				string text = string.Concat(new string[]
				{
					"Packages for ",
					client,
					" at timestamp ",
					timestamp.ToString(),
					"\n\n"
				});
				text += this.GetOutputPacakageSentDetails(packagesSentInfoEntry);
				Log.Out(text);
				TextEditor textEditor = new TextEditor();
				textEditor.text = text;
				textEditor.SelectAll();
				textEditor.Copy();
				Log.Out("Copied to clipboard");
			}
		}
	}

	// Token: 0x0600399A RID: 14746 RVA: 0x001787A0 File Offset: 0x001769A0
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetOutputPacakageSentDetails(PackagesSentInfoEntry entry)
	{
		string text = "\nPackage Type, Length";
		foreach (NetPackageInfo netPackageInfo in entry.packages)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n",
				netPackageInfo.netPackageType,
				",",
				netPackageInfo.length.ToString()
			});
		}
		return text;
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x0017882C File Offset: 0x00176A2C
	public void AppendClientCSV(string csv)
	{
		this.clientCSV = this.clientCSV + "\n\n" + csv;
	}

	// Token: 0x04002F16 RID: 12054
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageMetrics _instance;

	// Token: 0x04002F17 RID: 12055
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateLength = 5f;

	// Token: 0x04002F18 RID: 12056
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdateTime;

	// Token: 0x04002F19 RID: 12057
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastStatsOutput = "";

	// Token: 0x04002F1A RID: 12058
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, int> receivedNetPackageCounts;

	// Token: 0x04002F1B RID: 12059
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<int>> receivedNetPackageSizes;

	// Token: 0x04002F1C RID: 12060
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, int> sentNetPackageCounts;

	// Token: 0x04002F1D RID: 12061
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<int>> sentNetPackageSizes;

	// Token: 0x04002F1E RID: 12062
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerCount;

	// Token: 0x04002F1F RID: 12063
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityAliveCount;

	// Token: 0x04002F20 RID: 12064
	[PublicizedFrom(EAccessModifier.Private)]
	public bool active;

	// Token: 0x04002F21 RID: 12065
	public string clientCSV = "";

	// Token: 0x04002F22 RID: 12066
	[PublicizedFrom(EAccessModifier.Private)]
	public static int tick;

	// Token: 0x04002F23 RID: 12067
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, List<PackagesSentInfoEntry>> packagesSent;

	// Token: 0x04002F24 RID: 12068
	public bool includeRelPosRot = true;
}
