using UnityEngine;

public class House
{
    public string tv = "�Ž� Tv";
    private string diary = "��� ���̾";
    protected string secretKey = "�� ��й�ȣ";


    public string GetDiary()
    {
        Driver driver = new Driver();
   
        return diary;
    }

}
