using UnityEngine;

namespace General
{
    public static class SessionProvider
    {
        public static Session Current { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        public static void OnAppLaunch() 
        {
            Current = new Session();
        }
    }
}

