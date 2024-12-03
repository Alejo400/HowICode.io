using System.Collections;
using UnityEngine;
using EnemyBullet;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyCircleBulletAb", menuName = "AtkCircleBullet")]
public class EnemyCircleBulletAbility : EnemyAbility<GameObject>
{
    [SerializeField] int numberOfBullets;
    [SerializeField] float TimeEachFireCircle;
    EntityContainer PoolBullet;
    [SerializeField] string IDBullet;
    void OnEnable(){
        BulletManager.OnInitializeBullets += AssignPoolBullet;
    }
    void AssignPoolBullet(Dictionary<string,EntityIDContainer> pool){
        PoolBullet = pool[IDBullet];
    }
    public override IEnumerator ExecuteAbility(EnemyData _EnemyData, GameObject _Target, 
                                                GameObject gameObject)
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeEachFireCircle);
            float angleStep = 360f / numberOfBullets; // Espacio angular entre cada bala
            float currentAngle = 0f; // Ángulo inicial

            for (int i = 0; i < numberOfBullets; i++)
            {
                // Obtener una bala del Object Pool
                GameObject bullet = ObjectPool.Instance.Requestobject(PoolBullet.EntitiesList);
                if (bullet == null) continue; // Asegurarse de que el pool devuelva un objeto

                // Configurar daño de la bala
                bullet.GetComponent<OnDamage>().Damage = _EnemyData.Damage;

                // Posicionar la bala en el centro del enemigo
                bullet.transform.position = gameObject.transform.position;

                // Calcular dirección de la bala usando el ángulo actual
                float radianAngle = currentAngle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

                // Configurar la dirección en el componente MoveForward de la bala
                bullet.GetComponent<Bullet>().SetDirection(direction);

                // Activar la bala
                bullet.SetActive(true);

                // Incrementar el ángulo para la próxima bala
                currentAngle += angleStep;
            }
        }
    }
}
