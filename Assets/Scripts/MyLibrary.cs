using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ludiq;
using Bolt;

public class MyLibrary : MonoBehaviour
{
    // accesseur pour get /set une valeur de variable

    // et bim un gros stack d'Attributes :D
    [Range(0.0f, 10000.0f)]
    [Header("Power settings")]
    [Tooltip("My current power in dbz units ^^")]
    [SerializeField]
    private float myCurrentPower = 9000.0f;
    public float MyCurrentPower { get => myCurrentPower; set => myCurrentPower = value; }

    [Header("Prefabs")]
    public GameObject MyProjectilePrefab;

    GraphReference MyGraphReference;

    private void Awake()
    {
        // calculer la reference 1 seule fois au start
        MyGraphReference = GraphReference.New(GetComponent<FlowMachine>(), true);
    }

    //void Start()
    //{

    //}

    public void MyPlayerSayHello()
    {
        Debug.Log("Hello depuis C#");
    }

    public void MyPlayerSay(string pStringValue)
    {
        Debug.Log(pStringValue);
    }

    public float CalcAverage(List<float> pNotes)
    {
        float lSomme = 0.0f;
        foreach (float lNote in pNotes)
        {
            lSomme += lNote;
        }
        return lSomme / pNotes.Count;
    }

    // documentation
    // https://docs.unity3d.com/2019.3/Documentation/Manual/bolt-variables-api.html
    public void OpenFire()
    {
        // on va chercher la valeur booleene de playerCanDash dans le graph de la flow machine
        bool lCanDash = (bool)Variables.Graph(MyGraphReference).Get("playerCanDash");
        // on va chercher la valeur booleene de playerCanFire dans le graph dans le graph de la flow machine (scene variables)
        bool lPlayerCanFire = (bool)Variables.Scene(gameObject).Get("playerCanFire");

        // si le player peut dash (juste pour l'exemple d'aller chercher une variable dans le Graph)
        if (lCanDash)
        {
            // si le player peut tirer
            if (lPlayerCanFire)
            {
                // on va chercher l'image du projectile dans le HUD pour la cacher au moment du tir
                Image lHudProjectileImage = GameObject.Find("HudProjectiles").GetComponent<Image>();
                // le nullcheck
                if (lHudProjectileImage)
                {
                    lHudProjectileImage.enabled = false;
                }

                // node instantiate
                GameObject currentProjectile = Instantiate(MyProjectilePrefab, transform.position, Quaternion.identity);
                Variables.Object(currentProjectile).Set("Direction", gameObject.transform.localScale.x);

                // on repasse player can fire a false pour ne pas pouvoir tirer de nouveau (systeme du "couteau")
                Variables.Scene(gameObject).Set("playerCanFire", false);
            }
        }
    }
}