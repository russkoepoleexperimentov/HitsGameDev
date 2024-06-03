using Interaction;
using UnityEngine;

namespace General
 {
    public class Session
    {
        public ObjectGrabber Grabber { get; private set; }

        public Session() { 
        }

        public void SetActor(GameObject actor)
        {
            Grabber = actor.GetComponent<ObjectGrabber>();
        }
    }
}
