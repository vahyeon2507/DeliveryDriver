using UnityEngine;

public class Operator : MonoBehaviour
{
    public int mathScore = 95;
    public int englishScore = 85;
    private void Update()
    {
        ex1();
        //ex2();
    }

    private void ex1()
    {
        if (mathScore >= 60 && englishScore >= 60)
        {
            Debug.Log("일반합격");
        }
 

        else
        {
            Debug.Log("불합격");
        }





    }
}