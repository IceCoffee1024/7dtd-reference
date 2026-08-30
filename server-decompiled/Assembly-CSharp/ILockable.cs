using System;
using System.Collections.Generic;

// Token: 0x0200017D RID: 381
public interface ILockable
{
	// Token: 0x06000B4E RID: 2894
	bool IsLocked();

	// Token: 0x06000B4F RID: 2895
	void SetLocked(bool _isLocked);

	// Token: 0x06000B50 RID: 2896
	PlatformUserIdentifierAbs GetOwner();

	// Token: 0x06000B51 RID: 2897
	void SetOwner(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B52 RID: 2898
	bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B53 RID: 2899
	List<PlatformUserIdentifierAbs> GetUsers();

	// Token: 0x06000B54 RID: 2900
	bool LocalPlayerIsOwner();

	// Token: 0x06000B55 RID: 2901
	bool IsOwner(PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B56 RID: 2902
	bool HasPassword();

	// Token: 0x06000B57 RID: 2903
	bool SetPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B58 RID: 2904
	bool CheckPasswordHash(string _password, PlatformUserIdentifierAbs _userIdentifier);

	// Token: 0x06000B59 RID: 2905
	string GetPasswordHash();

	// Token: 0x06000B5A RID: 2906 RVA: 0x000484EC File Offset: 0x000466EC
	string GetHashForPassword(string _password)
	{
		return _password.GetStableHashCode().ToString("X8");
	}
}
