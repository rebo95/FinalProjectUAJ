using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PowerUp_Test_Suit
{
    private Game game;

    //Comprueba si los powerUps se mueven en la dirección y sentido correctos
    [UnityTest]
    public IEnumerator PowerUpMoveDown()
    {
        GameObject powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUp Variant"));
        float initialYPos = powerUp.transform.position.y;

        yield return new WaitForSeconds(0.1f);
        Assert.Less(powerUp.transform.position.y, initialYPos);
        Object.Destroy(powerUp);
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

