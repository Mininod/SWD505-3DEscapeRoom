using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectType
{
    //For the system
    None,

    //Collectable items
    Potato,
    Battery,
    SuperChargedPotato,
    CircuitAdapter,
    PotatoCircuit,
    GunPowder,
    ConductivePaste,
    LockExplosive,
    Shoe,
    TitaniumSpray,
    DoorStop,
    DoorHandle,
    Valve,
    Spaghetti,
    PickLock,
    DuctTape,
    MemoryStick,

    //Interactables/Tooltip items
    DoorA,      //to be openeded by the handle
    DoorHandleAttachSpot,   //for door A
    DoorB,      //to be opened by keypad
    DoorC,      //towards the escape pod, to be opened by the second keypad
    SleepingChamber,
    Crate,      //the one to be exploded open
    ControlPanel,
    Lever,      //controls/updates the control terminal
    PipeWithoutHandle,  //slot for handle
    PipeHandle,         //handles on pipes
    KeyPad,     //??? add keypad text ???
    FuseBoxA,    //where you place the sc potato
    FuseBoxWithPotato,  //Where you collect the sc potato from
    FuseBoxB,   //Where you place the potato circuit thing
    EscapePodButton,
    LockExplosivePlantSpot,      //to denote where to place the explosive (not actually collectable)#
    DoorStopSpot,       //place to put the door stop

    //Journals
    Journal1,
    Journal2,
    Journal3,
    Journal4,
    Journal5,
    Journal6,
    Journal7,
    Journal8,
    Journal9,
    Journal10
    
}



