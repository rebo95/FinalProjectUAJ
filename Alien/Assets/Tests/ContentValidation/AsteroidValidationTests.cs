using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    class AsteroidsPrefabsProvider : IEnumerable<GameObject>
    {
        public IEnumerator<GameObject> GetEnumerator()
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/RW/Resources/Prefabs" }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject root = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);
                if (root.GetComponent<Asteroid>()) yield return root;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<GameObject>)this).GetEnumerator();
    }

    [TestFixture]
    [TestFixtureSource(typeof(AsteroidsPrefabsProvider))]
    public class AsteroidValidationTests
    {
        private readonly GameObject _prefab;
        private readonly Asteroid _asteroidBehaviour;

        public AsteroidValidationTests(GameObject prefab)
        {
            _prefab = prefab;
            _asteroidBehaviour = prefab.GetComponent<Asteroid>();
        }

        [Test]
        public void IsOnAsteroidsLayer()
        {
            Assert.That(LayerMask.LayerToName(_prefab.layer), Is.EqualTo("Asteroids"));
        }

        [Test]
        public void HasPositiveVelocity()
        {
            Assert.That(_asteroidBehaviour.speed, Is.GreaterThanOrEqualTo(3));
        }
    }
}
