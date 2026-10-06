using System;
using System.Collections.Generic;
using System.Xml;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008EC RID: 2284
[Preserve]
public class AllyStore
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x060041AF RID: 16815 RVA: 0x0019CF10 File Offset: 0x0019B110
	// (remove) Token: 0x060041B0 RID: 16816 RVA: 0x0019CF48 File Offset: 0x0019B148
	public event AllyStore.AllyChangeEvent OnAllyChangeEvent;

	// Token: 0x060041B1 RID: 16817 RVA: 0x0019CF80 File Offset: 0x0019B180
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

	// Token: 0x060041B2 RID: 16818 RVA: 0x0019CFC8 File Offset: 0x0019B1C8
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

	// Token: 0x060041B3 RID: 16819 RVA: 0x0019D034 File Offset: 0x0019B234
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

	// Token: 0x060041B4 RID: 16820 RVA: 0x0019D068 File Offset: 0x0019B268
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

	// Token: 0x060041B5 RID: 16821 RVA: 0x0019D0DC File Offset: 0x0019B2DC
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

	// Token: 0x060041B6 RID: 16822 RVA: 0x0019D110 File Offset: 0x0019B310
	public bool IsAlly(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target)
	{
		return this.GetStatus(_source, _target) == AllyStore.AllyStatus.Allies;
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x0019D11D File Offset: 0x0019B31D
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

	// Token: 0x060041B8 RID: 16824 RVA: 0x0019D134 File Offset: 0x0019B334
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

	// Token: 0x060041B9 RID: 16825 RVA: 0x0019D1A4 File Offset: 0x0019B3A4
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

	// Token: 0x060041BA RID: 16826 RVA: 0x0019D1DC File Offset: 0x0019B3DC
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

	// Token: 0x060041BB RID: 16827 RVA: 0x0019D2BC File Offset: 0x0019B4BC
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

	// Token: 0x060041BC RID: 16828 RVA: 0x0019D2F9 File Offset: 0x0019B4F9
	public void ClearAll()
	{
		this.relationships.Clear();
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x0019D308 File Offset: 0x0019B508
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

	// Token: 0x060041BE RID: 16830 RVA: 0x0019D3BC File Offset: 0x0019B5BC
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

	// Token: 0x060041BF RID: 16831 RVA: 0x0019D498 File Offset: 0x0019B698
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

	// Token: 0x060041C0 RID: 16832 RVA: 0x0019D620 File Offset: 0x0019B820
	public void Read(PooledBinaryReader _br)
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

	// Token: 0x060041C1 RID: 16833 RVA: 0x0019D670 File Offset: 0x0019B870
	public void Write(PooledBinaryWriter _bw)
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

	// Token: 0x04003588 RID: 13704
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> relationships = new Dictionary<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>>();

	// Token: 0x020008ED RID: 2285
	public enum AllyStatus : byte
	{
		// Token: 0x0400358A RID: 13706
		NotAllied,
		// Token: 0x0400358B RID: 13707
		Allies,
		// Token: 0x0400358C RID: 13708
		OutgoingInvite,
		// Token: 0x0400358D RID: 13709
		IncomingInvite
	}

	// Token: 0x020008EE RID: 2286
	public enum AllyEvent : byte
	{
		// Token: 0x0400358F RID: 13711
		None,
		// Token: 0x04003590 RID: 13712
		OutgoingSent,
		// Token: 0x04003591 RID: 13713
		OutgoingCanceled,
		// Token: 0x04003592 RID: 13714
		IncomingAccepted,
		// Token: 0x04003593 RID: 13715
		IncomingDeclined,
		// Token: 0x04003594 RID: 13716
		AllyRemoved,
		// Token: 0x04003595 RID: 13717
		OutgoingAccepted,
		// Token: 0x04003596 RID: 13718
		OutgoingDeclined,
		// Token: 0x04003597 RID: 13719
		IncomingReceived,
		// Token: 0x04003598 RID: 13720
		IncomingCanceled,
		// Token: 0x04003599 RID: 13721
		RemovedByAlly
	}

	// Token: 0x020008EF RID: 2287
	// (Invoke) Token: 0x060041C4 RID: 16836
	public delegate void AllyChangeEvent(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyEvent _allyEventSource);
}
