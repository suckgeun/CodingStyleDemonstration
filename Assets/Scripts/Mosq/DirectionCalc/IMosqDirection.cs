using UnityEngine;

public interface IMosqDirection
{
    Vector3 CalcDirection(int numHits, Collider[] hits);
}
