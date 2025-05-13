using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] int x;
    [SerializeField] int y;
    [SerializeField] int z;
    void Update()
    {
 
        transform.Rotate(x * Time.deltaTime,y* Time.deltaTime,z* Time.deltaTime);
    }
}
