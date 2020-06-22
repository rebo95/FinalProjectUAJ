using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests
{
    class GameplayScenesProvider : IEnumerable<string>
    {
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled || scene.path == null || !Path.GetFileName(scene.path).StartsWith("Game"))
                    continue;

                yield return scene.path;
            }
        }

        public IEnumerator GetEnumerator() => ((IEnumerable<string>)this).GetEnumerator();
    }

    [TestFixture]
    [TestFixtureSource(typeof(GameplayScenesProvider))]
    public class SceneValidationTests
    {
        private readonly string _scenePath;
        private Scene _scene;

        public SceneValidationTests(string scenePath)
        {
            _scenePath = scenePath;
        }

        [OneTimeSetUp]
        public void LoadScene()
        {
            _scene = SceneManager.GetSceneAt(0);
            if (SceneManager.sceneCount > 1 || _scene.path != _scenePath)
                _scene = EditorSceneManager.OpenScene(_scenePath, OpenSceneMode.Single);
        }

        [Test]
        public void IsPlayerReady()
        {
            var player = Object.FindObjectOfType<Ship>();
            if (player == null)
                Assert.Ignore("No player in this scene");

            Assert.That(!player.isDead, "Player should be alive!");
            Assert.That(player.canShoot, "Player should be able to shoot!");
        }

        [Test]
        public void HasSpawner()
        {
            Assert.That(Object.FindObjectOfType<Spawner>());
        }
    }
}
