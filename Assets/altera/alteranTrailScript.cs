using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class alteranTrailScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombModule bombModule;

    private float[] path;
    public GameObject[] pathObjects;
    private float movementSpeed;
    private int speedTime;
    private int currentPath;
    private int sunRay;
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
    private bool spill = false;
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
    private bool isActive;

    // Use this for initialization
    void Start()
    {
        cloud = false;
        sunny = false;
        sickly = false;
        showSandstorm = false;
        showMeteors = false;
        StartCoroutine(Water());
        moduleId = moduleIdCounter++;
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

    void Activate()
    {
        isActive = true;
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
            showDay = false;
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
        }
        
        cloud = false;
        sunny = false;
        sickly = false;
        showSandstorm = false;
        showMeteors = false;
        events = Rnd.Range(0, 12);
        switch (events)
        {
            case 0:
                Cloudy();
                break;
            case 1:
                Ssun();
                break;
            case 2:
                Ssick();
                break;
            case 3:
                BatteryBoom();
                break;
            case 4:
                FoodStatch();
                break;
            case 5:
                FoodLockerBreach();
                break;
            case 6:
                Sandstorm();
                break;
            case 7:
               StartCoroutine(Meteor());
                break;
            case 8:
                StartCoroutine(Wolf());
                break;
            case 9:
                StartCoroutine(Acromantula());
                break;
            case 10:
                StartCoroutine(Bandits());
                break;
            case 11:
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
        audio.PlaySoundAtTransform("PressButton", transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        time = 0;
        retries++;
        Start();
    }

    void Rest()
    {
        audio.PlaySoundAtTransform("PressButton", transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
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
        Equation.fontSize = 65;
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

        
    }
    void Heal()
    {
        audio.PlaySoundAtTransform("PressButton", transform);
        showHeal = false;
        heal = false;
        health = 100;
        GetComponent<KMSelectable>().AddInteractionPunch();
        MoveTheJeep();
    }
    void Fast()
    {
        audio.PlaySoundAtTransform("PressButton", transform);
        speedTime = 100;
        battery = 35;
        GetComponent<KMSelectable>().AddInteractionPunch();
        MoveTheJeep();
    }
    void Normal()
    {
        audio.PlaySoundAtTransform("PressButton", transform);
        speedTime = 50;
        battery = 15;
        GetComponent<KMSelectable>().AddInteractionPunch();
        MoveTheJeep();
    }
    void Slow()
    {
        audio.PlaySoundAtTransform("PressButton", transform);
        speedTime = 25;
        battery = 10;
        GetComponent<KMSelectable>().AddInteractionPunch();
        MoveTheJeep();
    }

    void I0()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputZero.ToString()).ToString();
            Screen.text = answerKey;
        }
            
    }

    void I1()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputOne.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I2()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputTwo.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I3()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputThree.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I4()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputFour.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I5()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputFive.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I6()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputSix.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I7()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputSeven.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I8()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputEight.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void I9()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        if (answerKey.Length <= 3)
        {
            answerKey = (answerKey + inputNine.ToString()).ToString();
            Screen.text = answerKey;
        }
    }

    void Submits()
    {
        audio.PlaySoundAtTransform("Tap", transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        BackRender.material = Background[0];
        if (Screen.text == answer)
        {
            answerKey = "";
            answer = "";
            Screen.text = "";
            if (cloud == true)
            {
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
            answerKey = "";
            answer = "";
            Screen.text = "";
            showHeal = true;
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

        yield return new WaitForSeconds(2.0f);
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
            bombModule.HandlePass();
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
}