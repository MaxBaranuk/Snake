using UnityEngine;

public static class AssetProvider
{
   private static Chain _chainPrefab;
   private static GameObject _wallPrefab;
   private static SceneObject _fruitPrefab;
   private static Snake _snakePrefab;
   private static GameObject _cameraPrefab;

   public static void Init()
   {
      _chainPrefab = Resources.Load<Chain>("Chain");
      _wallPrefab = Resources.Load<GameObject>("Wall");
      _fruitPrefab = Resources.Load<SceneObject>("Fruit");
      _snakePrefab = Resources.Load<Snake>("Head");
      _cameraPrefab = Resources.Load<GameObject>("CameraHolder");
   }

   public static Chain GetChain()
   {
      return Object.Instantiate(_chainPrefab);
   }
   
   public static Snake GetHead()
   {
      return Object.Instantiate(_snakePrefab);
   }
   
   public static GameObject GetWall()
   {
      return Object.Instantiate(_wallPrefab);
   }
   
   public static GameObject GetCamera()
   {
      return Object.Instantiate(_cameraPrefab);
   }
   
   public static SceneObject GetRandomFruit()
   {
      var fruit = Object.Instantiate(_fruitPrefab);
      var typeIndex = Random.Range(0, 6);
      var renderer = fruit.GetComponent<MeshRenderer>();
      
      switch (typeIndex)
      {
         case 0:
            fruit.Action = SceneObject.ActionType.Decrease;   
            renderer.material.color = Color.magenta;
            break;
         case 1:
            fruit.Action = SceneObject.ActionType.Increase;  
            renderer.material.color = Color.yellow;
            break;
         case 2:
            fruit.Action = SceneObject.ActionType.AddPoints; 
            renderer.material.color = Color.green;
            break;
         case 3:
            fruit.Action = SceneObject.ActionType.Fast;    
            renderer.material.color = Color.red;
            break;
         case 4:
            fruit.Action = SceneObject.ActionType.Slow;   
            renderer.material.color = Color.blue;
            break;
         case 5:
            fruit.Action = SceneObject.ActionType.Invert; 
            renderer.material.color = Color.gray;
            break;
      }
      return fruit;
   }
}
