using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        // 외부에서 가져갔을 때
        get
        {
            // 만약 _instance가 비어있다면
            if (_instance == null)
            {
                // 새로운 "CharacterManager" 라는 오브젝트를 만들어서 <CharacterManager>.cs 스크립트를 컴포넌트로 붙이고
                // _Instance에 인스턴스 값 등록
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }

            return _instance;
        }
    }

    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }  // value는 외부에서 수정 되었을 때 그 값을 자동 저장
    }

    private void Awake()
    {
        if (_instance == null)
        { 
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
