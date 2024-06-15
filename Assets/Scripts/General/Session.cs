using Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            actor.GetComponent<PlayerHealth>().Die += () => {
                if(!actor.GetComponent<ActorCheckPoints>().Load())
                    ScreenFade.Singleton.InOut(() =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    });
            };
        }
    }
}
