using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "avatarSource")]
public class AvatarSource : ScriptableObject
{
    static AvatarSource i;
    public static AvatarSource instance
    {
        get
        {
            if (!i)
                i = Resources.Load<AvatarSource>("Data/Avatar/avatarSource");
            return i;
        }
    }

    //public List<Sprite> avatars;
    public List<DataAvatar> dataAvatars;
}

[Serializable]
public class DataAvatar
{
    public TypeAvatar typeAvatar;
    public Sprite sprite;
    public BuyingType buyingType;
}