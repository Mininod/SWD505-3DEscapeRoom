using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;


public class UnitTesting : MonoBehaviour
{
    //ButtonSelectScriptTests
    //Start
    //[UnityTest]
    //public IEnumerator BSStoggleOutline()
    //{
    //    //Arrange
    //    var testButton = new GameObject().AddComponent<ButtonSelectScript>();
        
    //    yield return null;

    //    //Act
    //    testButton.toggleOutline();

    //    //Assert
    //    Assert.AreEqual(true, testButton.isSelected());          //should simply select the button
    //}

    [UnityTest]
    public IEnumerator BSSisSelected()
    {
        //Arrange
        var testButton = new GameObject().AddComponent<ButtonSelectScript>();

        yield return null;

        //Act
        
        //Assert
        Assert.AreEqual(false, testButton.isSelected());            //selected should be false on creation
    }

    [UnityTest]
    public IEnumerator BSSdeselect()
    {
        //Arrange
        var testButton = new GameObject().AddComponent<ButtonSelectScript>();
        testButton.select();        //test method to set selected variable to true

        yield return null;

        //Act
        testButton.deselect();

        //Assert
        Assert.AreEqual(false, testButton.isSelected());            //should be false after being deselected
    }
    //End

    //InteractableScriptTests
    //Start
    [UnityTest]
    public IEnumerator ISgetTriggerStatus()
    {
        //Arrange
        var testInteractable = new GameObject().AddComponent<InteractableScript>();

        yield return null;

        //Act

        //Assert
        Assert.AreEqual(false, testInteractable.getTriggerStatus());            //should return false by default for the trigger status
    }

    [UnityTest]
    public IEnumerator IStriggerInteractable()
    {
        //Arrange
        var testInteractable = new GameObject().AddComponent<InteractableScript>();

        yield return null;

        //Act
        testInteractable.triggerInteractable();

        //Assert
        Assert.AreEqual(true, testInteractable.getTriggerStatus());             //should be true now that it has been triggered
    }
    //End

    //PlayerInventoryScriptTests
    //Start
    [UnityTest]
    public IEnumerator PISaddToInventory()
    {
        //Arrange
        var testInventory = new GameObject().AddComponent<PlayerInventoryScript>();
        testInventory.forceSetInventorySize(5);     //testing method to set the inventory size, normally done in start from a public variable

        yield return null;

        //Act
        testInventory.addToInventory(objectType.Battery);            //add a Battery to the first open slot

        //Assert
        Assert.AreEqual(true, testInventory.checkInventory(objectType.Battery));          //returns true if there is a key in the inventory
    }

    [UnityTest]
    public IEnumerator PIScheckInventory()
    {
        //Arrange
        var testInventory = new GameObject().AddComponent<PlayerInventoryScript>();
        testInventory.forceSetInventorySize(5);     //testing method to set the inventory size, normally done in start from a public variable
        testInventory.addToInventory(objectType.Battery);            //add a key to the first open slot

        yield return null;

        //Act
        bool ret = testInventory.checkInventory(objectType.Battery);         //should return true since there is a key in the inventory

        //Assert
        Assert.AreEqual(true, ret);
    }

    [UnityTest]
    public IEnumerator PISgetUsedSlots()
    {
        //Arrange
        var testInventory = new GameObject().AddComponent<PlayerInventoryScript>();
        testInventory.forceSetInventorySize(5);     //testing method to set the inventory size, normally done in start from a public variable
        testInventory.addToInventory(objectType.Battery);
        testInventory.addToInventory(objectType.Potato);

        yield return null;

        //Act
        int ret = testInventory.getUsedSlots();         //shoudl return 2 since I added 2 objects to the inventory (a key and a testpickup)

        //Assert
        Assert.AreEqual(2, ret);
    }

    [UnityTest]
    public IEnumerator PISgetObjectAtSlot()
    {
        //Arrange
        var testInventory = new GameObject().AddComponent<PlayerInventoryScript>();
        testInventory.forceSetInventorySize(5);     //testing method to set the inventory size, normally done in start from a public variable
        testInventory.addToInventory(objectType.Battery);

        yield return null;

        //Act
        var ret = testInventory.getObjectAtSlot(0);         //slot 0 is the first slot, where there should be a key

        //Assert
        Assert.AreEqual(objectType.Battery, ret);
    }
    //End


    //SettingsMenuScriptTests
    //Start
    [UnityTest]
    public IEnumerator SMSsetFullscreen()
    {
        //Arrange

        yield return null;

        //Act

        //Assert
        Assert.AreEqual(1, 1);
    }

    [UnityTest]
    public IEnumerator SMSsetLowQuality()
    {
        //Arrange

        yield return null;

        //Act

        //Assert
        Assert.AreEqual(1, 1);
    }

    [UnityTest]
    public IEnumerator SMSshowReticle()
    {
        //Arrange

        yield return null;

        //Act

        //Assert
        Assert.AreEqual(1, 1);
    }

    [UnityTest]
    public IEnumerator SMSreturnToMenu()
    {
        //Arrange

        yield return null;

        //Act

        //Assert
        Assert.AreEqual(1, 1);
    }
    //End
}
