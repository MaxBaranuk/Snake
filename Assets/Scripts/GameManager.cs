using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text _info;
    [SerializeField] private GameObject _losePanel;
    private int _points;
    private Snake _snake;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        AssetProvider.Init();
        InitEnvironment();
        StartGame();
    }

    public void StartGame()
    {
        _losePanel.SetActive(false);
        _snake = AssetProvider.GetHead();
        _snake.CameraHolder = AssetProvider.GetCamera().transform;
        StartCoroutine(SpawnSceneObjects());
    }

    public void FinishGame()
    {
        StopCoroutine(SpawnSceneObjects());
        _losePanel.SetActive(true);
        Destroy(_snake.gameObject);
    }

    public void AddPoints(int value)
    {
        _points += value;
        _info.text = $"Score: {_points}";
    }

    private void InitEnvironment()
    {
        var env = new GameObject("Environment");
        for (var i = -10; i <= 10; i++)
        {
            var wall = AssetProvider.GetWall();
            wall.transform.position = new Vector3(-10, 0, i);
            wall.transform.parent = env.transform;
            var wall2 = AssetProvider.GetWall();
            wall2.transform.position = new Vector3(10, 0, i);
            wall2.transform.parent = env.transform;
            var wall3 = AssetProvider.GetWall();
            wall3.transform.position = new Vector3(i, 0, -10);
            wall3.transform.parent = env.transform;
            var wall4 = AssetProvider.GetWall();
            wall4.transform.position = new Vector3(-i, 0, 10);
            wall4.transform.parent = env.transform;
        }
    }

    IEnumerator SpawnSceneObjects()
    {
        yield return new WaitForSeconds(5);
        var fruit = AssetProvider.GetRandomFruit();
        fruit.transform.position = new Vector3(Random.Range(-9, 9), 0, Random.Range(-9, 9));
    }
}
