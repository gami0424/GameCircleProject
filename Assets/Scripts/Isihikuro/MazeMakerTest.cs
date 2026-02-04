using UnityEngine;


public class MazeMakerTest : MonoBehaviour
{
    public Transform mazeMain;
    public GameObject[] road_X, road_T, road_L, road_I;

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
                GameObject stage = Instantiate(road_X[0]);
                stage.transform.SetParent(mazeMain);
                stage.transform.localPosition = new Vector3((i - 2) * disStage / 2, 0, (ii - 2) * disStage / 2) + offset;
                stage.transform.localScale = Vector3.one;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int ii = 0; ii < 3; ii++)
            {
                GameObject stage = Instantiate(road_I[0]);
                stage.transform.SetParent(mazeMain);
                stage.transform.localPosition = new Vector3((i - 2) * disStage / 2, 0, (ii - 2) * disStage / 2 + offset.z / 2) + offset;
                stage.transform.localScale = Vector3.one;
            }
        }
    }
}
