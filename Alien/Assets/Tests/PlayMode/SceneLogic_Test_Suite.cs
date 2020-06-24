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
