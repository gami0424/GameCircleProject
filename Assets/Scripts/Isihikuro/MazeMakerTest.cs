using Unity.AI.Navigation;
using UnityEngine;


public class MazeMakerTest : MonoBehaviour
{
    public Transform mazeMain;
    public GameObject[] road_X, road_T, road_L, road_I;
    int[,] stageMap =
    {
        {2,0},{1,3},{0,0},{2,1},
        {1,2},{0,0},{0,0},{1,0},
        {1,2},{0,0},{0,0},{1,0},
        {2,3},{0,0},{1,1},{2,2},
    };
    const float disStage = 26f;
    const float disRoad = 13f;
    Vector3 offset = new Vector3(disRoad, 0, disRoad);

    void Awake()
    {
        MazeMake();
    }

    public void MazeMake()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int ii = 0; ii < 4; ii++)
            {
                GameObject stageOri = null;
                int num = (3 - i) * 4 + ii;
                int rand;
                switch (stageMap[num, 0])
                {
                    case 0:
                        rand = Random.Range(0, road_X.Length);
                        stageOri = road_X[rand];
                        break;
                    case 1:
                        rand = Random.Range(0, road_T.Length);
                        stageOri = road_T[rand];
                        break;
                    case 2:
                        rand = Random.Range(0, road_L.Length);
                        stageOri = road_L[rand];
                        break;
                }
                GameObject stage = Instantiate(stageOri);
                stage.transform.SetParent(mazeMain);
                stage.transform.localPosition = new Vector3((ii - 2) * disStage, 0, (i - 2) * disStage) + offset;
                stage.transform.localScale = Vector3.one * 2;

                stage.transform.localEulerAngles = new Vector3(0, 90 * stageMap[num, 1], 0);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int ii = 0; ii < 3; ii++)
            {
                GameObject stage = Instantiate(road_I[0]);
                stage.transform.SetParent(mazeMain);
                stage.transform.localPosition = new Vector3((i - 2) * disStage, 0, (ii - 2) * disStage + offset.z) + offset;
                stage.transform.localScale = Vector3.one;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int ii = 0; ii < 4; ii++)
            {
                GameObject stage = Instantiate(road_I[0]);
                stage.transform.SetParent(mazeMain);
                stage.transform.localPosition = new Vector3((i - 2) * disStage + offset.x, 0, (ii - 2) * disStage) + offset;
                stage.transform.localScale = Vector3.one;

                stage.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
        }

        mazeMain.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
