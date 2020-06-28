using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SceneLogic_Test_Suite
{
    private Game game;

    //Comprueba que el juego se reinicia correctamente cuando sale la pantalla de Game Over
    [UnityTest]
    public IEnumerator NewGameRestartsGame()
    {
        game.isGameOver = true;
        game.NewGame();

        Assert.False(game.isGameOver);
        yield return null;
    }

    //Comprueba que Game Over se lanza correctamente cuando los asteroides colisionan con la nave
    [UnityTest]
    public IEnumerator GameOverOccursOnAsteroidCollision()
    {

        for (int i = 0; i < game.GetShip().MaxLifes; i++)
        {
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            asteroid.transform.position = game.GetShip().transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        Assert.True(game.isGameOver);
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
