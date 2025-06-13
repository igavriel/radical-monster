using UnityEngine;

public static class Util
{
    public static readonly string MAIN_SCENE_NAME = "1-main";
    public static readonly string SLEEP_SCENE_NAME = "3-2-sleep";
    public static readonly string WAKE_SCENE_NAME = "3-3-wake";

    static string[] sleepMessages =
    {
        "הגיע הזמן לישון...",
        "נא לא לגעת בכלום!",
        ". . . z Z z",
        "חלומות פז!",
        "אל תשכחו לכבות את האור!",
    };

    static string[] wakeMessages =
    {
        "אהה, התעוררתי!",
        "אני מרגיש רענן!",
        "איזה שינה טובה!",
        "היי, מה קורה?",
        "הגיע הזמן לקום!",
    };

    public static string GetRandomSleepMessage() =>
        sleepMessages[Random.Range(0, sleepMessages.Length)];

    public static string GetRandomWakeMessage() =>
        wakeMessages[Random.Range(0, wakeMessages.Length)];

    public static void AssertObject(Object obj, string message)
    {
        if (obj == null)
        {
            Debug.LogError(message);
#if UNITY_EDITOR
            throw new UnityException(message);
#endif
        }
    }

    public static string GetFormattedTime(float seconds, bool reverse = false)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        int minutes = Mathf.FloorToInt((seconds % 3600) / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        string formattedTime = $"{hours:D2}:{minutes:D2}:{secs:D2}";
        if (reverse)
        {
            formattedTime = reverseString(formattedTime);
        }
        return formattedTime;
    }

    public static string reverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
}
