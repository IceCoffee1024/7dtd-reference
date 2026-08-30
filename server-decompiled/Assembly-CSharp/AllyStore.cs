using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008DB RID: 2267
[Preserve]
public class AllyStore
{
	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060041B4 RID: 16820 RVA: 0x0019AD24 File Offset: 0x00198F24
	// (remove) Token: 0x060041B5 RID: 16821 RVA: 0x0019AD5C File Offset: 0x00198F5C
	public event AllyStore.AllyChangeEvent OnAllyChangeEvent;

	// Token: 0x060041B6 RID: 16822 RVA: 0x0019AD94 File Offset: 0x00198F94
	public void AllyUpdateRequest(PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		PlatformUserIdentifierAbs internalLocalUserIdentifier = PlatformManager.InternalLocalUserIdentifier;
		if (internalLocalUserIdentifier == null || _target == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageAllyRequest>().Setup(internalLocalUserIdentifier, _target, _addAlly), false);
			return;
		}
		this.ProcessAllyRequest(internalLocalUserIdentifier, _target, _addAlly);
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x0019ADDC File Offset: 0x00198FDC
	public void ProcessAllyRequest(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (_source == null || _target == null)
		{
			return;
		}
		AllyStore.AllyStatus allyStatus;
		AllyStore.AllyEvent allyEvent;
		AllyStore.AllyEvent allyEvent2;
		AllyStore.ComputeTransition(this.GetStatus(_source, _target), _addAlly, out allyStatus, out allyEvent, out allyEvent2);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageAllyResponse>().Setup(_source, _target, allyStatus, allyEvent, allyEvent2), false, -1, -1, -1, null, 192, false);
		this.AllyUpdateResponse(_source, _target, allyStatus, allyEvent, allyEvent2);
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x0019AE48 File Offset: 0x00199048
	public void ApplyTransition(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, bool _addAlly)
	{
		if (_source == null || _target == null)
		{
			return;
		}
		AllyStore.AllyStatus allyStatus;
		AllyStore.AllyEvent allyEvent;
		AllyStore.AllyEvent allyEvent2;
		AllyStore.ComputeTransition(this.GetStatus(_source, _target), _addAlly, out allyStatus, out allyEvent, out allyEvent2);
		this.SetStatus(_source, _target, allyStatus);
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x0019AE7C File Offset: 0x0019907C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ComputeTransition(AllyStore.AllyStatus _oldStatus, bool addAlly, out AllyStore.AllyStatus _newStatus, out AllyStore.AllyEvent _eventSource, out AllyStore.AllyEvent _eventTarget)
	{
		_newStatus = _oldStatus;
		_eventSource = AllyStore.AllyEvent.None;
		_eventTarget = AllyStore.AllyEvent.None;
		switch (_oldStatus)
		{
		case AllyStore.AllyStatus.NotAllied:
			if (addAlly)
			{
				_newStatus = AllyStore.AllyStatus.OutgoingInvite;
				_eventSource = AllyStore.AllyEvent.OutgoingSent;
				_eventTarget = AllyStore.AllyEvent.IncomingReceived;
				return;
			}
			break;
		case AllyStore.AllyStatus.Allies:
			if (!addAlly)
			{
				_newStatus = AllyStore.AllyStatus.NotAllied;
				_eventSource = AllyStore.AllyEvent.AllyRemoved;
				_eventTarget = AllyStore.AllyEvent.RemovedByAlly;
			}
			break;
		case AllyStore.AllyStatus.OutgoingInvite:
			if (!addAlly)
			{
				_newStatus = AllyStore.AllyStatus.NotAllied;
				_eventSource = AllyStore.AllyEvent.OutgoingCanceled;
				_eventTarget = AllyStore.AllyEvent.IncomingCanceled;
				return;
			}
			break;
		case AllyStore.AllyStatus.IncomingInvite:
			if (addAlly)
			{
				_newStatus = AllyStore.AllyStatus.Allies;
				_eventSource = AllyStore.AllyEvent.IncomingAccepted;
				_eventTarget = AllyStore.AllyEvent.OutgoingAccepted;
			}
			if (!addAlly)
			{
				_newStatus = AllyStore.AllyStatus.NotAllied;
				_eventSource = AllyStore.AllyEvent.IncomingDeclined;
				_eventTarget = AllyStore.AllyEvent.OutgoingDeclined;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x0019AEF0 File Offset: 0x001990F0
	public void AllyUpdateResponse(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyStatus _newStatus, AllyStore.AllyEvent _allyEventSource, AllyStore.AllyEvent _allyEventTarget)
	{
		this.SetStatus(_source, _target, _newStatus);
		AllyStore.AllyChangeEvent onAllyChangeEvent = this.OnAllyChangeEvent;
		if (onAllyChangeEvent != null)
		{
			onAllyChangeEvent(_source, _target, _allyEventSource);
		}
		AllyStore.AllyChangeEvent onAllyChangeEvent2 = this.OnAllyChangeEvent;
		if (onAllyChangeEvent2 == null)
		{
			return;
		}
		onAllyChangeEvent2(_target, _source, _allyEventTarget);
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x0019AF24 File Offset: 0x00199124
	public bool IsAlly(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target)
	{
		return this.GetStatus(_source, _target) == AllyStore.AllyStatus.Allies;
	}

	// Token: 0x060041BC RID: 16828 RVA: 0x0019AF31 File Offset: 0x00199131
	public IEnumerable<PlatformUserIdentifierAbs> EnumerateAllies(PlatformUserIdentifierAbs _id)
	{
		Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus> dictionary;
		if (_id == null || !this.relationships.TryGetValue(_id, out dictionary))
		{
			yield break;
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, AllyStore.AllyStatus> keyValuePair in dictionary)
		{
			if (keyValuePair.Value == AllyStore.AllyStatus.Allies)
			{
				yield return keyValuePair.Key;
			}
		}
		Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>.Enumerator enumerator = default(Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x0019AF48 File Offset: 0x00199148
	public bool HasAllies(PlatformUserIdentifierAbs _id)
	{
		Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus> dictionary;
		if (_id == null || !this.relationships.TryGetValue(_id, out dictionary))
		{
			return false;
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, AllyStore.AllyStatus> keyValuePair in dictionary)
		{
			if (keyValuePair.Value == AllyStore.AllyStatus.Allies)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060041BE RID: 16830 RVA: 0x0019AFB8 File Offset: 0x001991B8
	public AllyStore.AllyStatus GetStatus(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target)
	{
		if (_source == null || _target == null)
		{
			return AllyStore.AllyStatus.NotAllied;
		}
		Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus> dictionary;
		if (!this.relationships.TryGetValue(_source, out dictionary))
		{
			return AllyStore.AllyStatus.NotAllied;
		}
		AllyStore.AllyStatus allyStatus;
		if (!dictionary.TryGetValue(_target, out allyStatus))
		{
			return AllyStore.AllyStatus.NotAllied;
		}
		return allyStatus;
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x0019AFF0 File Offset: 0x001991F0
	public void SetStatus(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyStatus _status)
	{
		if (_source == null || _target == null)
		{
			return;
		}
		if (_status == AllyStore.AllyStatus.NotAllied)
		{
			this.ClearStatus(_source, _target);
			return;
		}
		if (!this.relationships.ContainsKey(_source))
		{
			this.relationships.Add(_source, new Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>());
		}
		if (!this.relationships.ContainsKey(_target))
		{
			this.relationships.Add(_target, new Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>());
		}
		if (_status == AllyStore.AllyStatus.Allies)
		{
			this.relationships[_source][_target] = AllyStore.AllyStatus.Allies;
			this.relationships[_target][_source] = AllyStore.AllyStatus.Allies;
			return;
		}
		if (_status == AllyStore.AllyStatus.OutgoingInvite)
		{
			this.relationships[_source][_target] = AllyStore.AllyStatus.OutgoingInvite;
			this.relationships[_target][_source] = AllyStore.AllyStatus.IncomingInvite;
			return;
		}
		if (_status == AllyStore.AllyStatus.IncomingInvite)
		{
			this.relationships[_source][_target] = AllyStore.AllyStatus.IncomingInvite;
			this.relationships[_target][_source] = AllyStore.AllyStatus.OutgoingInvite;
		}
	}

	// Token: 0x060041C0 RID: 16832 RVA: 0x0019B0D0 File Offset: 0x001992D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearStatus(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target)
	{
		Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus> dictionary;
		if (this.relationships.TryGetValue(_source, out dictionary))
		{
			dictionary.Remove(_target);
		}
		Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus> dictionary2;
		if (this.relationships.TryGetValue(_target, out dictionary2))
		{
			dictionary2.Remove(_source);
		}
	}

	// Token: 0x060041C1 RID: 16833 RVA: 0x0019B10D File Offset: 0x0019930D
	public void ClearAll()
	{
		this.relationships.Clear();
	}

	// Token: 0x060041C2 RID: 16834 RVA: 0x0019B11C File Offset: 0x0019931C
	public void CopyFrom(AllyStore _other)
	{
		this.relationships.Clear();
		if (_other == null)
		{
			return;
		}
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> keyValuePair in _other.relationships)
		{
			foreach (KeyValuePair<PlatformUserIdentifierAbs, AllyStore.AllyStatus> keyValuePair2 in keyValuePair.Value)
			{
				this.SetStatus(keyValuePair.Key, keyValuePair2.Key, keyValuePair2.Value);
			}
		}
	}

	// Token: 0x060041C3 RID: 16835 RVA: 0x0019B1D0 File Offset: 0x001993D0
	public void ReadXml(XmlElement _alliesElement, int _readVersion)
	{
		if (_readVersion == 0)
		{
			return;
		}
		foreach (object obj in _alliesElement.ChildNodes)
		{
			XmlElement xmlElement = ((XmlNode)obj) as XmlElement;
			if (xmlElement != null && xmlElement.Name == "ally")
			{
				PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromXml(xmlElement, true, "a");
				PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromXml(xmlElement, true, "b");
				if (platformUserIdentifierAbs != null && platformUserIdentifierAbs2 != null)
				{
					if (xmlElement.GetAttribute("status") == "allies")
					{
						this.SetStatus(platformUserIdentifierAbs, platformUserIdentifierAbs2, AllyStore.AllyStatus.Allies);
					}
					else if (xmlElement.GetAttribute("status") == "pending")
					{
						this.SetStatus(platformUserIdentifierAbs, platformUserIdentifierAbs2, AllyStore.AllyStatus.OutgoingInvite);
					}
				}
			}
		}
	}

	// Token: 0x060041C4 RID: 16836 RVA: 0x0019B2AC File Offset: 0x001994AC
	public void WriteXml(XmlElement _root)
	{
		bool flag = false;
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> keyValuePair in this.relationships)
		{
			if (keyValuePair.Value.Count > 0)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		XmlElement xmlElement = _root.AddXmlElement("allies");
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> keyValuePair2 in this.relationships)
		{
			foreach (KeyValuePair<PlatformUserIdentifierAbs, AllyStore.AllyStatus> keyValuePair3 in keyValuePair2.Value)
			{
				PlatformUserIdentifierAbs key = keyValuePair2.Key;
				PlatformUserIdentifierAbs key2 = keyValuePair3.Key;
				AllyStore.AllyStatus value = keyValuePair3.Value;
				if (value != AllyStore.AllyStatus.NotAllied && value != AllyStore.AllyStatus.IncomingInvite && (value != AllyStore.AllyStatus.Allies || string.CompareOrdinal(key.CombinedString, key2.CombinedString) <= 0))
				{
					XmlElement xmlElement2 = xmlElement.AddXmlElement("ally");
					key.ToXml(xmlElement2, "a");
					key2.ToXml(xmlElement2, "b");
					if (value == AllyStore.AllyStatus.Allies)
					{
						xmlElement2.SetAttribute("status", "allies");
					}
					else
					{
						xmlElement2.SetAttribute("status", "pending");
					}
				}
			}
		}
	}

	// Token: 0x060041C5 RID: 16837 RVA: 0x0019B434 File Offset: 0x00199634
	public void Read(BinaryReader _br)
	{
		this.relationships.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromStream(_br, false, false);
			AllyStore.AllyStatus allyStatus = (AllyStore.AllyStatus)_br.ReadByte();
			this.SetStatus(platformUserIdentifierAbs, platformUserIdentifierAbs2, allyStatus);
		}
	}

	// Token: 0x060041C6 RID: 16838 RVA: 0x0019B484 File Offset: 0x00199684
	public void Write(BinaryWriter _bw)
	{
		int num = 0;
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> keyValuePair in this.relationships)
		{
			foreach (KeyValuePair<PlatformUserIdentifierAbs, AllyStore.AllyStatus> keyValuePair2 in keyValuePair.Value)
			{
				num++;
			}
		}
		num /= 2;
		_bw.Write(num);
		foreach (KeyValuePair<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> keyValuePair3 in this.relationships)
		{
			foreach (KeyValuePair<PlatformUserIdentifierAbs, AllyStore.AllyStatus> keyValuePair4 in keyValuePair3.Value)
			{
				if (string.CompareOrdinal(keyValuePair3.Key.CombinedString, keyValuePair4.Key.CombinedString) <= 0)
				{
					keyValuePair3.Key.ToStream(_bw, false);
					keyValuePair4.Key.ToStream(_bw, false);
					_bw.Write((byte)keyValuePair4.Value);
				}
			}
		}
	}

	// Token: 0x04003513 RID: 13587
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> relationships = new Dictionary<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>>();

	// Token: 0x020008DC RID: 2268
	public enum AllyStatus : byte
	{
		// Token: 0x04003515 RID: 13589
		NotAllied,
		// Token: 0x04003516 RID: 13590
		Allies,
		// Token: 0x04003517 RID: 13591
		OutgoingInvite,
		// Token: 0x04003518 RID: 13592
		IncomingInvite
	}

	// Token: 0x020008DD RID: 2269
	public enum AllyEvent : byte
	{
		// Token: 0x0400351A RID: 13594
		None,
		// Token: 0x0400351B RID: 13595
		OutgoingSent,
		// Token: 0x0400351C RID: 13596
		OutgoingCanceled,
		// Token: 0x0400351D RID: 13597
		IncomingAccepted,
		// Token: 0x0400351E RID: 13598
		IncomingDeclined,
		// Token: 0x0400351F RID: 13599
		AllyRemoved,
		// Token: 0x04003520 RID: 13600
		OutgoingAccepted,
		// Token: 0x04003521 RID: 13601
		OutgoingDeclined,
		// Token: 0x04003522 RID: 13602
		IncomingReceived,
		// Token: 0x04003523 RID: 13603
		IncomingCanceled,
		// Token: 0x04003524 RID: 13604
		RemovedByAlly
	}

	// Token: 0x020008DE RID: 2270
	// (Invoke) Token: 0x060041C9 RID: 16841
	public delegate void AllyChangeEvent(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyEvent _allyEventSource);
}
