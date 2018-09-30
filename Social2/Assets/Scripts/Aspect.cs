using UnityEngine;
public class Aspect : MonoBehaviour
{
    public enum AspectTypes
    {
        PLAYER,
        ENEMY,
        POTION, 
        SWORD,
        MIRROR,
        REWARD
    }
    public AspectTypes aspectType;
}