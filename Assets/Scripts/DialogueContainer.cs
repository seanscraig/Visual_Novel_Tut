using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteChange
{
    public Actor actor;
    public int expression;
    public int onScreenImageID;
}

[Serializable]
public class BackgroundChange
{
    public Sprite sprite;
    public int onScreenImageID;
}

[Serializable]
public class DialogueLine
{
    public string line;
    public Actor actor;
    public List<SpriteChange> spriteChanges;
    public List<BackgroundChange> backgroundChanges;
}

[CreateAssetMenu]
public class DialogueContainer : ScriptableObject
{
    public List<DialogueLine> lines;
}
