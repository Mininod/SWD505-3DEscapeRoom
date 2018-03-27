using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;


public class UnitTesting : MonoBehaviour
{
    [UnityTest]
    public IEnumerator test1()
    {
        //Arrange

        yield return null;

        //Act

        //Assert
        Assert.AreEqual(1, 1);
    }

}
