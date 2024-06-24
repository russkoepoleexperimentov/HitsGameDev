using Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
 {
    public class Session
    {
        public ObjectGrabber Grabber { get; private set; }
        public ActorCheckPoints Checkpoints { get; private set; }

        public Session() { 
        }

        public void SetActor(GameObject actor)
        {
            Grabber = actor.GetComponent<ObjectGrabber>();
            Checkpoints = actor.GetComponent<ActorCheckPoints>();

            actor.GetComponent<PlayerHealth>().Die += () => {
                if (!Checkpoints.Load(false))
                {
                    /*ScreenFade.Singleton.InOut(() =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    });*/

                    var operation = new LoadingSceneOperation(SceneManager.GetActiveScene().name);
                    LoadingScreen.Instance.AddToQueue(operation);
                }
            };
        }
    }
}
