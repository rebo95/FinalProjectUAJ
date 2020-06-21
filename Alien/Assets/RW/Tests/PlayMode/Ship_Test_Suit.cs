using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Ship_Test_Suit
{
    private Game game;

    [UnityTest]
    public IEnumerator CollisionsWithAsteroidReducesLifes()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = game.GetShip().transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(game.MaxLifes - 1, game.currentLifes);
    }



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
}

