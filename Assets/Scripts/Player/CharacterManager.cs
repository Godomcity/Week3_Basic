using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // 중앙관리 스크립트 싱글톤을 통해 하나만 유지하고 Player의 정보를 받아올 수 있게 도와준다.

    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get 
        { 
            if(instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return instance;
        }
    }

    public Player player;
    public Player Player
    {
        get { return player; }
        set {  player = value; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance == this)
            {
                Destroy(gameObject);
            }
        }
    }
}