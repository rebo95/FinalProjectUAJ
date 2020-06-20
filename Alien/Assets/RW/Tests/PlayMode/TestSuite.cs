using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{
    private Game game;

    [SetUp]
    public void Setup()
    {
        GameObject gameGameObject =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
        game = gameGameObject.GetComponent<Game>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(game.gameObject);
    }


    [UnityTest]
    public IEnumerator AsteroidsMoveDown()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float initialYPos = asteroid.transform.position.y;
        
        yield return new WaitForSeconds(0.1f);
        Assert.Less(asteroid.transform.position.y, initialYPos);
    }


    [UnityTest]
    public IEnumerator CollisionsWithAsteroidReducesLifes()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = game.GetShip().transform.position;
        
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(game.MaxLifes - 1, game.currentLifes);
    }

    [UnityTest]
    public IEnumerator DestroyedAsteroidRaisesScore()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        GameObject laser = game.GetShip().SpawnLaser();

        asteroid.transform.position = laser.transform.position = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(game.score, 1);
    }

    [UnityTest]
    public IEnumerator DestroyingAsteroidsIncrementAsteroidsVel()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        float iniVel = asteroid.GetComponent<Asteroid>().speed;
        GameObject laser = game.GetShip().SpawnLaser();
        asteroid.transform.position = laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);


        for(int i = 0; i < game.GetInstance().pointsToIncreaseDifficulty - 1; i++)
        {
            asteroid = game.GetSpawner().SpawnAsteroid();
            laser = game.GetShip().SpawnLaser();
            asteroid.transform.position = laser.transform.position = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
        }

        asteroid = game.GetSpawner().SpawnAsteroid();
        float finalVel = asteroid.GetComponent<Asteroid>().speed;
        yield return new WaitForSeconds(0.1f);


        Assert.Greater(finalVel, iniVel);

    }
}

