using UnityEngine;

public interface ILaserHandler 
{
    public void OnLaser(Laser laser, Ray laserRay, RaycastHit laserHit);
}
