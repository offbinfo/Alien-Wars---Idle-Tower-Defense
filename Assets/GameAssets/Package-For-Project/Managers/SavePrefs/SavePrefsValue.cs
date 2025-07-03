public class SavePrefsValue
{
    string key;
    object valueDefault;
    EventID eventID;

    public SavePrefsValue (string key, object valueDefault, EventID eventID)
    {
        this.key = key;
        this.valueDefault = valueDefault;
        this.eventID = eventID;
    }

    public int IntValue
    {
        get => SavePrefs.GetInt (key, (int)valueDefault);
        set
        {
            SavePrefs.SetInt (key, value);
            EventDispatcher.PostEvent (eventID, value);
        }
    }

    public float FloatValue
    {
        get => SavePrefs.GetFloat (key, (float)valueDefault);
        set
        {
            SavePrefs.SetFloat (key, value);
            EventDispatcher.PostEvent (eventID, value);
        }
    }

    public string StringValue
    {
        get => SavePrefs.GetString (key, (string)valueDefault);
        set
        {
            SavePrefs.SetString (key, value);
            EventDispatcher.PostEvent (eventID, value);
        }
    }
}
