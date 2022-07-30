using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Tells Random to use the Unity Engine random number generator.
using Random = UnityEngine.Random;

//Map生成のscriptを記述する
public class BoardManager : MonoBehaviour
{
    //Map上にランダム生成するアイテム最小値、最大値を決めるclass
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //Mapの縦横
    public int columns = 8;
    public int rows = 8;

    //生成するアイテムの個数
    public Count wallCount = new Count(3, 9);
    public Count foodCount = new Count(1, 5);

    //MAPの材料
    public GameObject exit;
    public GameObject floor;
    public GameObject Wall;
    public GameObject OuterWall;
    public GameObject enemy;
    public GameObject food;

    //変換用
    private Transform boardHolder;

    //6×6のマスでobjectがない場所を管理する
    private List<Vector3> gridPositons = new List<Vector3>();

    /*
    void Start()
    {
        //Mapの生成
        BoardSetup();

        //gridPositionsのクリアと再取得
        InitialiseList();

        //ランダムに壁を生成
        LayoutObjectAtRandom(Wall, wallCount.minimum, wallCount.maximum);

        //食べ物生成
        LayoutObjectAtRandom(food, foodCount.minimum, foodCount.maximum);

        //右上に出口の設置
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0), Quaternion.identity);
    }
    */

    //フィールド生成
    void BoardSetup()
    {
        //Boardをインスタンス化してboardHolderに設定
        boardHolder = new GameObject("Board").transform;

        //rowsとcolumnsは8
        //Playerの位置が0,0なので、-1から始まる
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                //床を設置してインスタンス化の準備
                GameObject toInsutantiate = floor;

                //8×8マス外は外壁を設置してインスタンス化の準備
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInsutantiate = OuterWall;
                }

                //toInsutantiateに設定されたもの(床or壁)をインスタンス化
                GameObject instance =
                    Instantiate(toInsutantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;

                //インスタンス化された床or外壁の親要素をboardHolderに設定
                instance.transform.SetParent(boardHolder);
            }
        }
    }
    //gridPositionsをクリアする
    void InitialiseList()
    {
        //リストをクリアする
        gridPositons.Clear();

        //6×6のますをリストに取得する
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                //gridPositionsにx,yの値をいれる
                gridPositons.Add(new Vector3(x, y, 0));
            }
        }

    }

    //gridPositionsからランダムな位置を取得する
    Vector3 RandomPosition()
    {
        //randomIndexを宣言して、gridPositionsの数から数値をランダムで入れる
        int randomIndex = Random.Range(0, gridPositons.Count);

        //randomPositionを宣言して、gridPositionsのrandomIndexに設定する
        Vector3 randomPosition = gridPositons[randomIndex];

        //使用したgridPositionsの要素を削除(削除することでこのマスが使われないようにする)
        gridPositons.RemoveAt(randomIndex);

        return randomPosition;
    }

    //Mapにランダムで引数のものを配置する(敵、壁、アイテム)
    void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        //生成するアイテムの個数を最小最大値からランダムに決め、objectCountに設定する
        int objectCount = Random.Range(minimum, maximum);

        //設置するオブジェクトの数分ループで回す
        for (int i = 0; i < objectCount; i++)
        {
            //現在オブジェクトが置かれていない、ランダムな位置を取得
            Vector3 randomPosition = RandomPosition();

            //生成
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(Wall, wallCount.minimum, wallCount.maximum);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(food, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(enemy, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}