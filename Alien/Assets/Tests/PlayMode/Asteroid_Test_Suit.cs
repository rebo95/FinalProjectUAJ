using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Asteroid_Test_Suit
{
    private Game game;


    //Comprueba si los asteroides se mueven en la dirección y sentido correctos
    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialYPos = asteroid.transform.position.y;

        yield return new WaitForSeconds(0.1f);
        Assert.Less(asteroid.transform.position.y, initialYPos);
    }


    //Comprueba si destruir los asteroides con las balas se incrementa la puntuación
    [UnityTest]
    public IEnumerator DestroyedAsteroidRaisesScore()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        GameObject laser = game.GetShip().SpawnLaser();

        asteroid.transform.position = laser.transform.position = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(game.score, 1);
    }


    //Comprueba si al llegar al número de asteroides destruidos
    //Se incrementa la velocidad de los mimsos.
    [UnityTest]
    public IEnumerator ReachingAScoreDestroyingAsteroidsIncrementAsteroidsVel()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float iniVel = asteroid.GetComponent<Asteroid>().speed;
        Game.AsteroidDestroyed();

        for (int i = 0; i < game.GetInstance().pointsToIncreaseDifficulty - 1; i++)
        {
            Game.AsteroidDestroyed();
        }

        asteroid = game.GetSpawner().SpawnAsteroid();
        float finalVel = asteroid.GetComponent<Asteroid>().speed;
        yield return new WaitForSeconds(0.1f);

        Assert.Greater(finalVel, iniVel);

    }




    //Incializador
    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }

    //Destructor
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game.gameObject);
    }
}
