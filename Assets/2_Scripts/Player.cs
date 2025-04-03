using UnityEngine;

public class Player
{
    public int health = 100;
        public static int PlayerCount = 0;
    public Player()
    {
        PlayerCount++;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
    }
    public void Attack()
    {
        int damge = 10;
        Debug.Log("공격력: " + damge);
    }
    public void Defend()
    {
        int damge = 5;
        Debug.Log("방어력: " + damge);
    }
}
        