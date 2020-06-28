using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Ship_Test_Suit
{
    private Game game;

    //Comprueba si al destruir los asteroides se reduce el número de vidas
    [UnityTest]
    public IEnumerator CollisionsWithAsteroidReducesLifes()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = game.GetShip().transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(game.MaxLifes - 1, game.currentLifes);
    }

    //Comprueba si la nave se mueve correctamente hacia la derecha
    [UnityTest]
    public IEnumerator ShipsMoveRight()
    {
        float initialPositionX = game.GetShip().transform.position.x;

        for (int i = 0; i < 100; i++)
        {
            game.GetShip().MoveRight();
        }

        Assert.Greater(game.GetShip().transform.position.x, initialPositionX);

        yield return null;
    }

    //Comprueba si la nave se mueve correctamente a la izda
    [UnityTest]
    public IEnumerator ShipMovesLeft()
    {
        float initialPositionX = game.GetShip().transform.position.x;

        for (int i = 0; i < 100; i++)
        {
            game.GetShip().MoveLeft();
        }

        Assert.Less(game.GetShip().transform.position.x, initialPositionX);

        yield return null;
    }

    //Comprueba que los láser avanzan correctamente hacia arriba
    [UnityTest]
    public IEnumerator LaserMovesUp()
    {
        GameObject laser = game.GetShip().SpawnLaser();
        float initialPosY = laser.transform.position.y;

        yield return new WaitForSeconds(0.1f);

        Assert.Greater(laser.transform.position.y, initialPosY);
    }

    //Comprueba que los lasers destruyen los asteroides al colisionar con ellos
    [UnityTest]
    public IEnumerator LaserDestroysAsteroid()
    {
        GameObject asteroid = game.GetSpawner().SpawnAsteroid();
        asteroid.transform.position = Vector3.zero;
        GameObject laser = game.GetShip().SpawnLaser();
        laser.transform.position = Vector3.zero;
        yield return new WaitForSeconds(0.1f);
        UnityEngine.Assertions.Assert.IsNull(asteroid);
    }



    //Inicializador
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

