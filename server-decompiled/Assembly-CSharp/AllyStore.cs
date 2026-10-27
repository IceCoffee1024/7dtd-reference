using System;
using System.Collections.Generic;
using System.Xml;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020008F6 RID: 2294
[Preserve]
public class AllyStore
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x060041D2 RID: 16850 RVA: 0x0019DFB8 File Offset: 0x0019C1B8
	// (remove) Token: 0x060041D3 RID: 16851 RVA: 0x0019DFF0 File Offset: 0x0019C1F0
	public event AllyStore.AllyChangeEvent OnAllyChangeEvent;

	// Token: 0x060041D4 RID: 16852 RVA: 0x0019E028 File Offset: 0x0019C228
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

	// Token: 0x060041D5 RID: 16853 RVA: 0x0019E070 File Offset: 0x0019C270
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

	// Token: 0x060041D6 RID: 16854 RVA: 0x0019E0DC File Offset: 0x0019C2DC
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

	// Token: 0x060041D7 RID: 16855 RVA: 0x0019E110 File Offset: 0x0019C310
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

	// Token: 0x060041D8 RID: 16856 RVA: 0x0019E184 File Offset: 0x0019C384
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

	// Token: 0x060041D9 RID: 16857 RVA: 0x0019E1B8 File Offset: 0x0019C3B8
	public bool IsAlly(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target)
	{
		return this.GetStatus(_source, _target) == AllyStore.AllyStatus.Allies;
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x0019E1C5 File Offset: 0x0019C3C5
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

	// Token: 0x060041DB RID: 16859 RVA: 0x0019E1DC File Offset: 0x0019C3DC
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

	// Token: 0x060041DC RID: 16860 RVA: 0x0019E24C File Offset: 0x0019C44C
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

	// Token: 0x060041DD RID: 16861 RVA: 0x0019E284 File Offset: 0x0019C484
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

	// Token: 0x060041DE RID: 16862 RVA: 0x0019E364 File Offset: 0x0019C564
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

	// Token: 0x060041DF RID: 16863 RVA: 0x0019E3A1 File Offset: 0x0019C5A1
	public void ClearAll()
	{
		this.relationships.Clear();
	}

	// Token: 0x060041E0 RID: 16864 RVA: 0x0019E3B0 File Offset: 0x0019C5B0
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

	// Token: 0x060041E1 RID: 16865 RVA: 0x0019E464 File Offset: 0x0019C664
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

	// Token: 0x060041E2 RID: 16866 RVA: 0x0019E540 File Offset: 0x0019C740
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

	// Token: 0x060041E3 RID: 16867 RVA: 0x0019E6C8 File Offset: 0x0019C8C8
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

	// Token: 0x060041E4 RID: 16868 RVA: 0x0019E718 File Offset: 0x0019C918
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

	// Token: 0x040035A8 RID: 13736
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>> relationships = new Dictionary<PlatformUserIdentifierAbs, Dictionary<PlatformUserIdentifierAbs, AllyStore.AllyStatus>>();

	// Token: 0x020008F7 RID: 2295
	public enum AllyStatus : byte
	{
		// Token: 0x040035AA RID: 13738
		NotAllied,
		// Token: 0x040035AB RID: 13739
		Allies,
		// Token: 0x040035AC RID: 13740
		OutgoingInvite,
		// Token: 0x040035AD RID: 13741
		IncomingInvite
	}

	// Token: 0x020008F8 RID: 2296
	public enum AllyEvent : byte
	{
		// Token: 0x040035AF RID: 13743
		None,
		// Token: 0x040035B0 RID: 13744
		OutgoingSent,
		// Token: 0x040035B1 RID: 13745
		OutgoingCanceled,
		// Token: 0x040035B2 RID: 13746
		IncomingAccepted,
		// Token: 0x040035B3 RID: 13747
		IncomingDeclined,
		// Token: 0x040035B4 RID: 13748
		AllyRemoved,
		// Token: 0x040035B5 RID: 13749
		OutgoingAccepted,
		// Token: 0x040035B6 RID: 13750
		OutgoingDeclined,
		// Token: 0x040035B7 RID: 13751
		IncomingReceived,
		// Token: 0x040035B8 RID: 13752
		IncomingCanceled,
		// Token: 0x040035B9 RID: 13753
		RemovedByAlly
	}

	// Token: 0x020008F9 RID: 2297
	// (Invoke) Token: 0x060041E7 RID: 16871
	public delegate void AllyChangeEvent(PlatformUserIdentifierAbs _source, PlatformUserIdentifierAbs _target, AllyStore.AllyEvent _allyEventSource);
}
