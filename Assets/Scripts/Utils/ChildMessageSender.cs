using UnityEngine;

public class ChildMessageSender : MonoBehaviour
{
    public void Send(string message)
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            transform.GetChild(i).BroadcastMessage(message, SendMessageOptions.DontRequireReceiver);
        }
    }
}
