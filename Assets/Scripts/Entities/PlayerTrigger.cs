using UnityEngine;

public class PlayerTrigger : BaseTrigger
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CallOnEnter();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CallOnExit();
        }
    }
}
