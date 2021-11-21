using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class alteranTrailScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombModule bombModule;

    public GameObject[] pathObjects;
    private int speedTime;
    private int battery;
    private int increasePower;
    private Vector3[] pathPos;
    private Vector3 currentPos;

    public Material[] water;
    public Material[] Background;
    public Light Sun;
    public GameObject Sick;
    public GameObject Sandstorms;
    public GameObject Gunshoot;
    public ParticleSystem Meteors;
    public ParticleSystem BatteryBlow;
    public ParticleSystem FoodLocker;
    public GameObject Jeep;
    public GameObject Input;
    public GameObject FoodCache;
    public GameObject Terrain;
    public Renderer WaterRender;
    public Renderer BackRender;
    public TextMesh Health;
    public TextMesh Life;
    public TextMesh Food;
    public TextMesh Distance;
    public TextMesh Days;
    public TextMesh Equation;
    public TextMesh Screen;

    private string answerKey = "";
    private string answer;


    private int retries = 0;
    private int events = 0;
    private int inputZero = 0;
    private int inputOne = 1;
    private int inputTwo = 2;
    private int inputThree = 3;
    private int inputFour = 4;
    private int inputFive = 5;
    private int inputSix = 6;
    private int inputSeven = 7;
    private int inputEight = 8;
    private int inputNine = 9;

    public KMSelectable RetryButton;
    public KMSelectable RestButton;
    public KMSelectable HealButton;
    public KMSelectable SlowButton;
    public KMSelectable NormalButton;
    public KMSelectable FastButton;
    public KMSelectable Input0;
    public KMSelectable Input1;
    public KMSelectable Input2;
    public KMSelectable Input3;
    public KMSelectable Input4;
    public KMSelectable Input5;
    public KMSelectable Input6;
    public KMSelectable Input7;
    public KMSelectable Input8;
    public KMSelectable Input9;
    public KMSelectable Submit;
    public KMSelectable Clear;

    private bool retry;
    private bool fast;
    private bool normal;
    private bool slow;
    private bool rest;
    private bool heal;
    private bool showHeal;
    private bool showDay = false;
    private bool showPath;
    private bool showEquation;
    private bool showInput;
    private bool showTerrain;

    private bool sunny = false;
    private bool cloud = false;
    private bool sickly = false;
    private bool showCache = false;
    private bool showSandstorm = false;
    private bool showMeteors = false;
    private bool showGun = false;

    private int health;
    private int power;
    private int time;
    private int food;
    private int distance;

    private static int moduleIdCounter = 1;
    private int moduleId;

    // Use this for initialization
    void Start()
    {
        cloud = false;
        sunny = false;
        sickly = false;
        showSandstorm = false;
        showMeteors = false;
        StartCoroutine(Water());
        if (retries == 0)
        {
            moduleId = moduleIdCounter++;
            Sun.range *= transform.lossyScale.x;
        }
        health = 100;
        power = 100;
        food = 100;
        distance = 0;
        showPath = false;
        retry = false;
        fast = false;
        normal = false;
        slow = false;
        showTerrain = true;
        heal = false;
        showHeal = true;
        showEquation = false;
        showInput = false;
        showCache = false;
        showSandstorm = false;
        showMeteors = false;
        showGun = false;
        rest = false;
        currentPos = pathObjects[20].transform.position;
        if (retries == 0)
        {
            RetryButton.OnInteract += delegate { Retry(); return false; };
            RestButton.OnInteract += delegate { Rest(); return false; };
            HealButton.OnInteract += delegate { Heal(); return false; };
            FastButton.OnInteract += delegate { Fast(); return false; };
            NormalButton.OnInteract += delegate { Normal(); return false; };
            SlowButton.OnInteract += delegate { Slow(); return false; };
            Submit.OnInteract += delegate { Submits(); return false; };
            Clear.OnInteract += delegate { Clears(); return false; };
            Input0.OnInteract += delegate { I0(); return false; };
            Input1.OnInteract += delegate { I1(); return false; };
            Input2.OnInteract += delegate { I2(); return false; };
            Input3.OnInteract += delegate { I3(); return false; };
            Input4.OnInteract += delegate { I4(); return false; };
            Input5.OnInteract += delegate { I5(); return false; };
            Input6.OnInteract += delegate { I6(); return false; };
            Input7.OnInteract += delegate { I7(); return false; };
            Input8.OnInteract += delegate { I8(); return false; };
            Input9.OnInteract += delegate { I9(); return false; };
        }
        StartCoroutine(Day());
    }

    void MoveTheJeep()
    {
        showTerrain = true;
        showEquation = false;
        showInput = false;
        if (showHeal == true)
        {
            normal = false;
            fast = false;
            slow = false;
            heal = false;
            rest = false;
            StartCoroutine(TimingFriending());
        }
        if (showHeal == false)
        {
            normal = false;
            fast = false;
            slow = false;
            rest = false;
            StartCoroutine(TimingFriending());
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (health <= 0)
        {
            Debug.LogFormat("[The Alteran Trail #{0}] You took too much damage and died", moduleId);
            time = 0;
            retry = true;
            showPath = false;
            fast = false;
            normal = false;
            slow = false;
            showTerrain = false;
            heal = false;
            showDay = false;
            showHeal = false;
            showEquation = false;
            showInput = false;
            showCache = false;
            showSandstorm = false;
            showMeteors = false;
            showGun = false;
            rest = false;
        }

        if (cloud == true && Sun.intensity > 0.1f)
        {
            Sun.intensity = Sun.intensity - 0.1f;
        }
        else if (sunny == true && Sun.intensity < 1f)
        {
            Sun.intensity = Sun.intensity + 0.1f;
        }
        else if (cloud == false && sunny == false)
        {
            Sun.intensity = 0.5f;
        }
        if (showInput == true)
        {
            Health.text = "";
            Life.text = "";
            Food.text = "";
            Distance.text = "";

        }
        else
        {
            Health.text = "hp: " + health;
            Life.text = "power: " + power + " mah";
            Food.text = "food: " + food + " kg";
            Distance.text = "distance: " + distance + " km";

        }
        if (showTerrain == true)
        {
            Terrain.gameObject.SetActive(true);
        }
        else
        {
            Terrain.gameObject.SetActive(false);
        }
        if (showMeteors == true)
        {
            Meteors.gameObject.SetActive(true);
        }
        else
        {
            Meteors.gameObject.SetActive(false);
        }
        if (showCache == true)
        {
            FoodCache.gameObject.SetActive(true);
        }
        else
        {
            FoodCache.gameObject.SetActive(false);
        }
        if (showInput == true)
        {
            Input.gameObject.SetActive(true);
        }
        else
        {
            Input.gameObject.SetActive(false);
        }
        if (showSandstorm == true)
        {
            Sandstorms.gameObject.SetActive(true);
        }
        else
        {
            Sandstorms.gameObject.SetActive(false);
        }
        if (showGun == true)
        {
            Gunshoot.gameObject.SetActive(true);
        }
        else
        {
            Gunshoot.gameObject.SetActive(false);
        }
        if (sickly == true)
        {
            Sick.gameObject.SetActive(true);
        }
        else
        {
            Sick.gameObject.SetActive(false);
        }
        if (showEquation == true)
        {
            Equation.gameObject.SetActive(true);
        }
        else
        {
            Equation.gameObject.SetActive(false);
        }
        if (showDay == true)
        {
            Days.gameObject.SetActive(true);
        }
        else
        {
            Days.gameObject.SetActive(false);
        }
        if (normal == true)
        {
            NormalButton.gameObject.SetActive(true);
        }
        else
        {
            NormalButton.gameObject.SetActive(false);
        }

        if (slow == true)
        {
            SlowButton.gameObject.SetActive(true);
        }
        else
        {
            SlowButton.gameObject.SetActive(false);
        }

        if (retry == true)
        {
            RetryButton.gameObject.SetActive(true);
        }
        else
        {
            RetryButton.gameObject.SetActive(false);
        }

        if (fast == true)
        {
            FastButton.gameObject.SetActive(true);
        }
        else
        {
            FastButton.gameObject.SetActive(false);
        }

        if (heal == true)
        {
            HealButton.gameObject.SetActive(true);
        }
        else
        {
            HealButton.gameObject.SetActive(false);
        }
        if (health >= 99)
        {
            HealButton.gameObject.SetActive(false);
        }

        if (rest == true)
        {
            RestButton.gameObject.SetActive(true);
        }
        else
        {
            RestButton.gameObject.SetActive(false);
        }

        if (increasePower > 0 && power > 0 && power < 100)
        {
            power++;
            increasePower--;
        }
        if (increasePower > 0 && power == 100)
        {
            increasePower--;
        }

        if (speedTime > 0)
        {
            distance++;
            speedTime--;
        }
       
        if (power > 0 && battery > 0)
        {
            power--;
            battery--;
        }

        if (power == 0 && battery > 0)
        {
            battery--;
        }

        if (power > 99)
        {
            rest = false;
            power = 100;
        }

        if (power <= 0)
        {
            slow = false;
            normal = false;
            fast = false;
            power = 0;
        }

        if (power < 35)
        {
            fast = false;
        }

        if (power < 15)
        {
            normal = false;
        }

        if (power < 10)
        {
            slow = false;
        }

        for (int i = 0; i < 21; i++)
        {
            if (showPath == true)
            {
                pathPos[i] = pathObjects[i].gameObject.transform.position;
                pathObjects[i].gameObject.SetActive(true);
            }
            else
            {
                pathObjects[i].gameObject.SetActive(false);
            }
        }

        if (distance == 0)
        {
            currentPos = pathObjects[20].transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[20].transform.rotation;
        }

        if (distance == 50)
        {
            currentPos = pathObjects[0].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[0].transform.rotation;
        }

        if (distance == 100)
        {
            currentPos = pathObjects[1].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[1].transform.rotation;
        }

        if (distance == 150)
        {
            currentPos = pathObjects[2].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[2].transform.rotation;
        }

        if (distance == 200)
        {
            currentPos = pathObjects[3].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[3].transform.rotation;
        }

        if (distance == 250)
        {
            currentPos = pathObjects[4].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[4].transform.rotation;
        }

        if (distance == 300)
        {
            currentPos = pathObjects[5].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[5].transform.rotation;
        }

        if (distance == 350)
        {
            currentPos = pathObjects[6].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[6].transform.rotation;
        }

        if (distance == 400)
        {
            currentPos = pathObjects[7].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[7].transform.rotation;
        }

        if (distance == 450)
        {
            currentPos = pathObjects[8].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[8].transform.rotation;
        }

        if (distance == 500)
        {
            currentPos = pathObjects[9].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[9].transform.rotation;
        }

        if (distance == 550)
        {
            currentPos = pathObjects[10].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[10].transform.rotation;
        }

        if (distance == 600)
        {
            currentPos = pathObjects[11].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[11].transform.rotation;
        }

        if (distance == 650)
        {
            currentPos = pathObjects[12].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[12].transform.rotation;
        }

        if (distance == 700)
        {
            currentPos = pathObjects[13].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[13].transform.rotation;
        }

        if (distance == 750)
        {
            currentPos = pathObjects[14].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[14].transform.rotation;
        }

        if (distance == 800)
        {
            currentPos = pathObjects[15].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[15].transform.rotation;
        }

        if (distance == 850)
        {
            currentPos = pathObjects[16].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[16].transform.rotation;
        }

        if (distance == 900)
        {
            currentPos = pathObjects[17].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[17].transform.rotation;
        }

        if (distance == 950)
        {
            currentPos = pathObjects[18].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[18].transform.rotation;
        }

        if (distance == 1000)
        {
            currentPos = pathObjects[19].gameObject.transform.position;
            Jeep.transform.position = currentPos;
            Jeep.transform.rotation = pathObjects[19].transform.rotation;
        }
    }

    void Events()
    {
        cloud = false;
        sunny = false;
        sickly = false;
        showSandstorm = false;
        showMeteors = false;
        events = Rnd.Range(0, 12);
        switch (events)
        {
            case 0:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Cloudy", moduleId);
                Cloudy();
                break;
            case 1:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Sunny", moduleId);
                Ssun();
                break;
            case 2:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Alteran Flu", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -10 health", moduleId);
                Ssick();
                break;
            case 3:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Battery Explosion", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -25 mAH", moduleId);
                BatteryBoom();
                break;
            case 4:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Food Cache", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] +20 kg", moduleId);
                FoodStatch();
                break;
            case 5:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Food Locker Breach", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -25 kg", moduleId);
                FoodLockerBreach();
                break;
            case 6:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Sandstorm", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -15 health", moduleId);
                Sandstorm();
                break;
            case 7:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Meteor Shower", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -10 health overtime", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -10 mAH overtime", moduleId);
                StartCoroutine(Meteor());
                break;
            case 8:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Alteran Wolf", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -15 health", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] +10 kg", moduleId);
                StartCoroutine(Wolf());
                break;
            case 9:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Acromantula", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -15 health overtime", moduleId);
                StartCoroutine(Acromantula());
                break;
            case 10:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Bandits", moduleId);
                Debug.LogFormat("[The Alteran Trail #{0}] -15 health", moduleId);
                StartCoroutine(Bandits());
                break;
            case 11:
                Debug.LogFormat("[The Alteran Trail #{0}] Today's Event: Normal", moduleId);
                break;
        }
    }

    void BatteryBoom()
    {
        audio.PlaySoundAtTransform("FullPop", transform);
        battery = 25;
        BatteryBlow.Play();
    }
    void FoodLockerBreach()
    {
        if (food >= 25)
        {
            for (int i = 0; i < 25; i++)
            {
                food--;
                FoodLocker.Play();
            }
        }
        else if (food == 0)
        {

        }
        else
        {
            while (food > 0)
            {
                food--;
                FoodLocker.Play();
            }
        }
    }

    void Ssun()
    {
        sunny = true;
    }

    void Ssick()
    {
        sickly = true;
        for (int i = 0; i < 10; i++)
        {
            health--;
        }
    }

    void FoodStatch()
    {
        StartCoroutine(FoodCaches());
    }

    void Cloudy()
    {
        cloud = true;
    }

    void Sandstorm()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.2f);
        showSandstorm = true;
        for (int i = 0; i < 15; i++)
        {
            health--;
        }
    }

    void WolfAction()
    {
        for (int i = 0; i < 15; i++)
        {
            health--;
        }
        for (int i = 0; i < 10; i++)
        {
            food++;
        }
    }

    void Retry()
    {
        audio.PlaySoundAtTransform("PressButton", RetryButton.transform);
        RetryButton.AddInteractionPunch();
        StopAllCoroutines();
        time = 0;
        retries++;
        Start();
    }

    void Rest()
    {
        Debug.LogFormat("[The Alteran Trail #{0}] You pressed the rest button, opening the battery charging station", moduleId);
        audio.PlaySoundAtTransform("PressButton", RestButton.transform);
        RestButton.AddInteractionPunch();
        BackRender.material = Background[1];
        normal = false;
        fast = false;
        slow = false;
        heal = false;
        showTerrain = false;
        rest = false;
        GenerateQuestion();
    }

    void GenerateQuestion()
    {
        int generateion = Rnd.Range(1, 12);
        double x = 0;
        double b = 0;
        double c = 0;
        double d = 0;
        string equation;
        Equation.fontSize = 50;
        int difficulty = Rnd.Range(1, 10);
        switch (generateion)
        {
            case 0:
                x = Rnd.Range(1, 5) * difficulty + 1;
                b = Rnd.Range(1, 5) * difficulty + 1;
                c = Rnd.Range(1, 5) * difficulty + 1;
                d = (b * x) + c;
                answer = x.ToString();
                equation = b + "x + " + c + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 1:
                x = Rnd.Range(1, 5) * difficulty + 1;
                b = Rnd.Range(1, 5) * difficulty + 1;
                c = Rnd.Range(1, 5) * difficulty + 1;
                d = (b * x) - c;
                answer = x.ToString();
                equation = b + "x - " + c + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 2:
                difficulty = Rnd.Range(1, 6);
                b = Rnd.Range(1, 5) * difficulty + 1;
                c = Rnd.Range(1, 5) * difficulty + 1;
                d = b * c * difficulty;
                x = (d-c) * b;
                answer = x.ToString();
                equation = "x/" + b + " + " + c + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 3:
                difficulty = Rnd.Range(1, 6);
                b = Rnd.Range(1, 5) * difficulty + 1;
                c = Rnd.Range(1, 5) * difficulty + 1;
                d = b * Rnd.Range(1, 5) * difficulty + 1;
                x = (d + c) * b;
                answer = x.ToString();
                equation = "x/" + b + " - " + c + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 4:
                x = Rnd.Range(1, 5) * difficulty + 1;
                c = 0;
                b = Rnd.Range(1, 5) * difficulty + 1;
                d = x + b;
                answer = x.ToString();
                equation = "x" + " + " + b + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 5:
                x = Rnd.Range(1, 5) * difficulty + 1;
                c = 0;
                b = Rnd.Range(1, 5) * difficulty + 1;
                d = x - b;
                answer = x.ToString();
                equation = "x" + " - " + b + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 6:
                x = Rnd.Range(1, 5) * difficulty + 1;
                c = 0;
                b = Rnd.Range(1, 5) * difficulty + 1;
                d = x * b;
                answer = x.ToString();
                equation = b + "x" + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 7:
                c = 0;
                b = Rnd.Range(1, 5) * difficulty + 1;
                d = Rnd.Range(1, 5) * difficulty + 1;
                x = b * d;
                answer = x.ToString();
                equation = "x/" + b + " = " + d;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 8:
                x = Rnd.Range(2, 10);
                c = 0;
                b = Rnd.Range(2, 10);
                if(b == 1)
                {
                    b += 1;
                }
                if(x == 0 && b == 0)
                {
                    x = Rnd.Range(2, 10);
                    c = 0;
                    b = Rnd.Range(2, 10);
                }
                d = Math.Pow(x, b);
                answer = x.ToString();
                equation = "x to the power of " + b + " = " + d;
                Equation.fontSize = 30;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 9:
                x = Rnd.Range(2, 10);
                c = 0;
                b = Rnd.Range(2, 10);
                if (x == 0 && b == 0)
                {
                    x = Rnd.Range(2, 10);
                    c = 0;
                    b = Rnd.Range(2, 10);
                }
                d = Math.Pow(b, x);
                answer = x.ToString();
                equation = b + " to the power of x" + " = " + d;
                Equation.fontSize = 30;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 10:
                x = Rnd.Range(2, 10);
                c = 0;
                b = Rnd.Range(2, 10);
                if (x == 0 && b == 0)
                {
                    x = Rnd.Range(2, 10);
                    c = 0;
                    b = Rnd.Range(2, 10);
                }
                d = Math.Pow(b, x);
                answer = x.ToString();
                equation = "the xth root of " + d + " = " + b;
                Equation.fontSize = 30;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
            case 11:
                x = Rnd.Range(2, 10);
                c = 0;
                b = Rnd.Range(2, 10);
                if (x == 1 && b == 1)
                {
                    x = Rnd.Range(2, 10);
                    c = 0;
                    b = Rnd.Range(2, 10);
                }
                d = Math.Pow(b, x);
                answer = x.ToString();
                equation = "log " + d + " base " + b + " = " + "x";
                Equation.fontSize = 30;
                showEquation = true;
                showInput = true;
                Equation.text = equation;
                break;
        }

        Debug.LogFormat("[The Alteran Trail #{0}] Generated Equation: {1}", moduleId, Equation.text);
        Debug.LogFormat("[The Alteran Trail #{0}] Answer: {1}", moduleId, answer);
    }
    void Heal()
    {
        Debug.LogFormat("[The Alteran Trail #{0}] You pressed the heal button, health is now 100", moduleId);
        audio.PlaySoundAtTransform("PressButton", HealButton.transform);
        showHeal = false;
        heal = false;
        health = 100;
        HealButton.AddInteractionPunch();
        MoveTheJeep();
    }
    void Fast()
    {
        Debug.LogFormat("[The Alteran Trail #{0}] You pressed the fast button, traveled a distance of 100km", moduleId);
        audio.PlaySoundAtTransform("PressButton", FastButton.transform);
        speedTime = 100;
        battery = 35;
        FastButton.AddInteractionPunch();
        MoveTheJeep();
    }
    void Normal()
    {
        Debug.LogFormat("[The Alteran Trail #{0}] You pressed the normal button, traveled a distance of 50km", moduleId);
        audio.PlaySoundAtTransform("PressButton", NormalButton.transform);
        speedTime = 50;
        battery = 15;
        NormalButton.AddInteractionPunch();
        MoveTheJeep();
    }
    void Slow()
    {
        Debug.LogFormat("[The Alteran Trail #{0}] You pressed the slow button, traveled a distance of 25km", moduleId);
        audio.PlaySoundAtTransform("PressButton", SlowButton.transform);
        speedTime = 25;
        battery = 10;
        SlowButton.AddInteractionPunch();
        MoveTheJeep();
    }

    void I0()
    {
        audio.PlaySoundAtTransform("Tap", Input0.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputZero.ToString()).ToString();
            Screen.text = answerKey;
        }
            
    }

    void I1()
    {
        audio.PlaySoundAtTransform("Tap", Input1.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputOne.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I2()
    {
        audio.PlaySoundAtTransform("Tap", Input2.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputTwo.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I3()
    {
        audio.PlaySoundAtTransform("Tap", Input3.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputThree.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I4()
    {
        audio.PlaySoundAtTransform("Tap", Input4.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputFour.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I5()
    {
        audio.PlaySoundAtTransform("Tap", Input5.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputFive.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I6()
    {
        audio.PlaySoundAtTransform("Tap", Input6.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputSix.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I7()
    {
        audio.PlaySoundAtTransform("Tap", Input7.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputSeven.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I8()
    {
        audio.PlaySoundAtTransform("Tap", Input8.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputEight.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I9()
    {
        audio.PlaySoundAtTransform("Tap", Input9.transform);
        if (answerKey.Length <= 5)
        {
            answerKey = (answerKey + inputNine.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void Clears()
    {
        audio.PlaySoundAtTransform("Tap", Clear.transform);
        Clear.AddInteractionPunch();
        Screen.text = "";
        answerKey = "";
    }

    void Submits()
    {
        audio.PlaySoundAtTransform("Tap", Submit.transform);
        Submit.AddInteractionPunch();
        BackRender.material = Background[0];
        if (Screen.text == answer)
        {
            Debug.LogFormat("[The Alteran Trail #{0}] Submitted '{1}' to the battery charging station, which is correct", moduleId, Screen.text);
            answerKey = "";
            answer = "";
            Screen.text = "";
            if (cloud == true)
            {
                Debug.LogFormat("[The Alteran Trail #{0}] +15 mAH", moduleId);
                if (power <= 0)
                {
                    power = 1;
                    increasePower = 14;
                }
                else
                {
                    increasePower = 15;
                }
            }
            else if (sunny == true)
            {
                Debug.LogFormat("[The Alteran Trail #{0}] +40 mAH", moduleId);
                if (power <= 0)
                {
                    power = 1;
                    increasePower = 39;
                }
                else
                {
                    increasePower = 40;
                }
            }
            else
            {
                Debug.LogFormat("[The Alteran Trail #{0}] +30 mAH", moduleId);
                if (power <= 0)
                {
                    power = 1;
                    increasePower = 29;
                }
                else
                {
                    increasePower = 30;
                }
            }
            MoveTheJeep();
        }
        else
        {
            Debug.LogFormat("[The Alteran Trail #{0}] Submitted '{1}' to the battery charging station, which is incorrect. Strike!", moduleId, Screen.text);
            answerKey = "";
            answer = "";
            Screen.text = "";
            showTerrain = true;
            showInput = false;
            showEquation = false;
            bombModule.HandleStrike();
            MoveTheJeep();
        }
    }

    IEnumerator Meteor()
    {
        showMeteors = true;
        Meteors.Play();
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < 5; i++)
        {
            health = health - 2;
            battery = battery + 2;
            audio.PlaySoundAtTransform("old_explode", transform);
            GetComponent<KMSelectable>().AddInteractionPunch(2f);
            yield return new WaitForSeconds(1.5f);
        }
        showMeteors = false;
    }

    IEnumerator Wolf()
    {
        BackRender.material = Background[2];
        showTerrain = false;
        yield return new WaitForSeconds(1.5f);
        WolfAction();
        BackRender.material = Background[0];
        showTerrain = true;
    }

    IEnumerator Acromantula()
    {
        BackRender.material = Background[3];
        showTerrain = false;
        yield return new WaitForSeconds(1.5f);
        BackRender.material = Background[0];
        showTerrain = true;
        for (int i = 0; i < 15; i++)
        {
            health = health - 1;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator Bandits()
    {
        for (int i = 0; i < 15; i++)
        {
            audio.PlaySoundAtTransform("SmallPop", transform);
            showGun = true;
            health = health - 1;
            yield return new WaitForSeconds(0.05f);
            showGun = false;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator Water()
    {
        WaterRender.material = water[0];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[1];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[2];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[3];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[4];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[5];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[6];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[7];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[8];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[9];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[10];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[11];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[12];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[13];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[14];
        yield return new WaitForSeconds(0.1f);
        WaterRender.material = water[15];
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Water());

    }

    IEnumerator TimingFriending()
    {
        yield return new WaitForSeconds(1.125f);
        int tempHealth = health;
        int tempFood = food;
        for (int i = 0; i < 5; i++)
        {
            if (health <= 90 && food > 0)
            {
                health++;
                food = food - 2;
            }
            if (food < 0)
            {
                food = 0;
            }
            yield return new WaitForSeconds(0.05f);
        }
        if (food != tempFood && health != tempHealth)
        {
            Debug.LogFormat("[The Alteran Trail #{0}] -{1} kg (Regen)", moduleId, tempFood - food);
            Debug.LogFormat("[The Alteran Trail #{0}] +{1} health (Regen)", moduleId, health - tempHealth);
        }
        yield return new WaitForSeconds(0.625f);
        Events();
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Day());
    }

    IEnumerator FoodCaches()
    {
        if (distance <= 900)
        {
            showCache = true;
            for (int i = 0; i < 20; i++)
            {
                food = food + 1;
                yield return new WaitForSeconds(0.05f);
            }
            showCache = false;
        }
    }
    IEnumerator Day()
    {
        time = time + 1;
        Days.text = time.ToString();
        Debug.LogFormat("[The Alteran Trail #{0}] It is Day {1} of your mission", moduleId, time);
        showDay = true;
        yield return new WaitForSeconds(2.0f);
        showDay = false;
        if (showHeal == true)
        {
            normal = true;
            fast = true;
            slow = true;
            heal = true;
            rest = true;
            showTerrain = true;
            showEquation = false;
            showInput = false;
        }
        if (showHeal == false)
        {
            normal = true;
            fast = true;
            slow = true;
            rest = true;
            showTerrain = true;
            showEquation = false;
            showInput = false;
        }
        if (distance >= 1000)
        {
            Debug.LogFormat("[The Alteran Trail #{0}] Successfully traveled at least 1000km, delivery complete!", moduleId);
            bombModule.HandlePass();
            Sun.intensity = 0;
            normal = false;
            fast = false;
            slow = false;
            rest = false;
            heal = false;
            cloud = true;
            showCache = false;
            showGun = false;
            sickly = false;
            showMeteors = false;
            showSandstorm = false;
        }
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} fast/normal/slow/rest/heal/retry [Presses the specified button] | !{0} submit <ans> [Submits the specified answer 'ans' for the battery charging station]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*fast\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (showInput || !fast)
            {
                yield return "sendtochaterror That button cannot be pressed right now!";
                yield break;
            }
            FastButton.OnInteract();
        }
        if (Regex.IsMatch(command, @"^\s*normal\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (showInput || !normal)
            {
                yield return "sendtochaterror That button cannot be pressed right now!";
                yield break;
            }
            NormalButton.OnInteract();
        }
        if (Regex.IsMatch(command, @"^\s*slow\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (showInput || !slow)
            {
                yield return "sendtochaterror That button cannot be pressed right now!";
                yield break;
            }
            SlowButton.OnInteract();
        }
        if (Regex.IsMatch(command, @"^\s*rest\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (showInput || !rest)
            {
                yield return "sendtochaterror That button cannot be pressed right now!";
                yield break;
            }
            RestButton.OnInteract();
        }
        if (Regex.IsMatch(command, @"^\s*heal\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (showInput || !HealButton.gameObject.activeSelf)
            {
                yield return "sendtochaterror That button cannot be pressed right now!";
                yield break;
            }
            HealButton.OnInteract();
        }
        if (Regex.IsMatch(command, @"^\s*retry\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (showInput || !retry)
            {
                yield return "sendtochaterror That button cannot be pressed right now!";
                yield break;
            }
            RetryButton.OnInteract();
        }
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length > 2)
            {
                yield return "sendtochaterror Too many parameters!";
                yield break;
            }
            else if (parameters.Length == 2)
            {
                int temp = -1;
                if (!int.TryParse(parameters[1], out temp))
                {
                    yield return "sendtochaterror!f The specified answer '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                if (temp < 0 || temp > 999999)
                {
                    yield return "sendtochaterror The specified answer '" + parameters[1] + "' is invalid!";
                    yield break;
                }
                if (!showInput)
                {
                    yield return "sendtochaterror An answer cannot be submitted to the battery charging station right now!";
                    yield break;
                }
                KMSelectable[] keypad = { Input0, Input1, Input2, Input3, Input4, Input5, Input6, Input7, Input8, Input9 };
                for (int j = 0; j < answer.Length; j++)
                {
                    keypad[int.Parse(answer[j].ToString())].OnInteract();
                    yield return new WaitForSeconds(.1f);
                }
                Submit.OnInteract();
            }
            else if (parameters.Length == 1)
            {
                yield return "sendtochaterror Please specify an answer to submit!";
                yield break;
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        KMSelectable[] keypad = { Input0, Input1, Input2, Input3, Input4, Input5, Input6, Input7, Input8, Input9 };
        if (showInput)
        {
            bool clrPress = false;
            if (Screen.text.Length > answer.Length)
            {
                Clear.OnInteract();
                yield return new WaitForSeconds(.1f);
                clrPress = true;
            }
            else
            {
                for (int i = 0; i < Screen.text.Length; i++)
                {
                    if (i == answer.Length)
                        break;
                    if (Screen.text[i] != answer[i])
                    {
                        Clear.OnInteract();
                        yield return new WaitForSeconds(.1f);
                        clrPress = true;
                        break;
                    }
                }
            }
            int start = 0;
            if (!clrPress)
                start = Screen.text.Length;
            for (int j = start; j < answer.Length; j++)
            {
                keypad[int.Parse(answer[j].ToString())].OnInteract();
                yield return new WaitForSeconds(.1f);
            }
            Submit.OnInteract();
        }
        while (distance < 1000)
        {
            while (!fast && !rest) { if (distance >= 1000) { break; } if (retry) { RetryButton.OnInteract(); } yield return true; }
            if (distance < 1000)
            {
                if (health <= 15 && HealButton.gameObject.activeSelf)
                    HealButton.OnInteract();
                else if (!fast)
                {
                    RestButton.OnInteract();
                    yield return new WaitForSeconds(.1f);
                    for (int j = 0; j < answer.Length; j++)
                    {
                        keypad[int.Parse(answer[j].ToString())].OnInteract();
                        yield return new WaitForSeconds(.1f);
                    }
                    Submit.OnInteract();
                }
                else
                    FastButton.OnInteract();
            }
        }
        while (showDay) yield return true;
    }
}