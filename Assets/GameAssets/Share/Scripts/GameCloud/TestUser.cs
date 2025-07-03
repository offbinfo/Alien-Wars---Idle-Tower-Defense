using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TestUser : ILocalUser
{
    public static TestUser instance = new TestUser ();

    public IUserProfile [] friends => null;

    public bool authenticated => true;

    public bool underage => false;

    public string userName => SystemInfo.deviceName;

    public string id => SystemInfo.deviceUniqueIdentifier;

    public bool isFriend => false;

    public UserState state => UserState.Online;

    public Texture2D image => null;

    public void Authenticate (Action<bool> callback)
    {

    }

    public void Authenticate (Action<bool, string> callback)
    {

    }

    public void LoadFriends (Action<bool> callback)
    {

    }
}
